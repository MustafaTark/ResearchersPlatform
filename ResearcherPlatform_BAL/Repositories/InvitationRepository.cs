using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class InvitationRepository : GenericRepository<Invitation> , IInvitationRepository
    {
        private readonly IMapper _mapper;
        public InvitationRepository(AppDbContext context , IMapper mapper):base(context)
        {
            _mapper= mapper;
        }
        //public void CreateInvitationForIdea(Guid ideaId)
        //{
        //    Invitation invitation = new Invitation { IdeaId = ideaId};
        //    Create(invitation);

        //}
        public async Task<InvitationDto?> GetInvitationById(Guid invitationid, bool trackChanges)
            => await FindByCondition(i => i.Id == invitationid, trackChanges)
            .ProjectTo<InvitationDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<InvitationDto?> GetInvitationByIdeaId(Guid ideaId, bool trackChanges)
            => await FindByCondition(i => i.IdeaId == ideaId, trackChanges)
            .ProjectTo<InvitationDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<InvitationDto?>> GetAllInvitationsForResearcher(Guid researcherId, bool trackChanges)
            => await FindByCondition(i => i.ResearcherId== researcherId , trackChanges)
            .ProjectTo<InvitationDto>(_mapper.ConfigurationProvider).ToListAsync();


        public void CreateInvitations(Guid ideaId, List<Guid> researcherIds)
        {
            foreach (var id in researcherIds)
            {
                var researcherInvitations = _context!.Invitations.Any(i => i.IdeaId == ideaId && i.ResearcherId == id);
                if (researcherInvitations)
                    continue;
                var invitation = new Invitation { IdeaId = ideaId, ResearcherId = id };
                _context!.Invitations.Add(invitation!);
            }
        }

        public async Task SendInvitation(Guid invitationId , Guid ideaId, List<Guid> researcherIds)
        {
            var invitation = await FindByCondition(i => i.Id == invitationId && i.IdeaId == ideaId, trackChanges: false).FirstOrDefaultAsync();
            foreach (var id in researcherIds)
            {
                var researcher = await _context.Researchers.Where(r => r.Id == id).FirstOrDefaultAsync();
                researcher!.Invitations.Add(invitation!);
            }
        }
        public async Task<bool> ValidationInvitation(Guid invitationId, Guid researcherId)
        {
            var researcher = await _context.Researchers.FirstOrDefaultAsync(r => r.Id == researcherId
            && r.Invitations.FirstOrDefault(i => i.Id == invitationId)!.Id==invitationId);
            if (researcher is null)
                return false;
            return true;
        }
        public async Task AcceptInvitation(Guid invitationId, Guid researcherId)
        {
            var researcher = await _context.Researchers.Include(r => r.Invitations).FirstOrDefaultAsync(r => r.Id == researcherId);
            var invitation = researcher!.Invitations.FirstOrDefault(i => i.Id == invitationId);
            invitation!.IsAccepted = true;
            var idea = await _context.Ideas.FirstOrDefaultAsync(i => i.Id == invitation.IdeaId);
            _context.Set<Idea>().FirstOrDefault(i => i.Id == idea!.Id)!.Participants.Add(researcher!);
            idea!.ParticipantsNumber++;
            DeleteInvitation(invitationId, researcherId);
        }
        public async Task<bool> ValidationAcception(Guid invitationId, Guid researcherId)
        {
            var invitationn = await FindByCondition(i => i.Id == invitationId
                && i.ResearcherId == researcherId
                && i.IsAccepted == false, trackChanges: false).FirstOrDefaultAsync();
            if (invitationn is null)
                return false;
            return true;
        }
        public void DeleteInvitation(Guid invitationId, Guid researcherId)
        {
            var invitation =  FindByCondition(i => i.Id == invitationId && i.ResearcherId == researcherId ,trackChanges:true).FirstOrDefault();
            _context.Invitations.Remove(invitation!);
        }

    }
}
