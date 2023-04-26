using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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
        public void CreateInvitation(Guid ideaId, Guid creatorId, Invitation invitation)
        {
            var idea = _context.Ideas.FirstOrDefault(i => i.Id == ideaId && i.CreatorId == creatorId);
            _context.Set<Invitation>().Add(invitation);
        }
        //public async void CreateInvitation(Guid ideaId, Guid creatorId, Invitation invitation)
        //{
        //    var idea = _context.Ideas.FirstOrDefault(i => i.Id == ideaId && i.CreatorId == creatorId);
        //    if (idea is not null)
        //    {
        //        var researchers = await _context.Researchers.ToListAsync();
        //        _context.Set<Invitation>().Add(invitation);

        //        foreach (var researcher in researchers)
        //        {
        //            _context.Set<Invitation>().FirstOrDefault(i => i.Researchers == researcher).Add(invitation);
        //            _context.Set<Invitation>().Where(i => i.Researchers
        //            .FirstOrDefault(r => r.Id == researcher.Id)!.Id == researcher.Id)
        //                .Add(invitation);

        //        }
        //    }
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
            => await FindByCondition(i => i.Researchers.FirstOrDefault(r => r.Id== researcherId)!.Id==researcherId , trackChanges)
            .ProjectTo<InvitationDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
}
