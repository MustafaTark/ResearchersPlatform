using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class AdminRepository : GenericRepository<User> , IAdminRepository
    {
        private readonly IMapper _mapper;
        public AdminRepository(AppDbContext context , IMapper mapper):base(context)
        {
            _mapper = mapper;
        }

        public int IdeasCount(bool trackChanges) => _context.Ideas.Count();
        

        public int ResearchersCount(bool trackChanges) => _context.Researchers.Count();

        public void AddTopic(Topic topic)
        {
            _context.Topics.Add(topic);
        }
        public void AddSkill(Skill skill)
        {
            _context.Skills.Add(skill);
        }
        public void UpdateSkill(Skill skill) => _context.Skills.Update(skill);

        public async Task<Skill> GetSkillById(int skillId)
        {
            var skill  = await _context.Skills.FirstOrDefaultAsync(i => i.Id == skillId);
            return skill!;
        }
    }
}
