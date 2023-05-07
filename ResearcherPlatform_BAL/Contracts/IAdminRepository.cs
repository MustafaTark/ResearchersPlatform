using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IAdminRepository
    {
        int IdeasCount(bool trackChanges);
        int ResearchersCount(bool trackChanges);
        void AddSkill(Skill skill);
        void AddTopic(Topic topic);

    }
}
