using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.RepoExtentions
{
   
    public static class IdeasRepositoryExtention
    {
        public static IQueryable<IdeaDto> Search(
            this IQueryable<IdeaDto> ideas, string searchTerm, int topic, int specality,int month )
        {
            var result = ideas;
            if (topic is not 0)
            {

                result = result.Where(e => e.TopicId!.Equals(topic));
            }
            if (specality is not 0)
            {

                result = result.Where(e => e.SpecalityId!.Equals(specality));
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerCaseTerm = searchTerm.Trim().ToLower();
                result = result.Where(e => e.Name!.Contains(lowerCaseTerm));
            }
            if(month is not 0)
            {
                result = result.Where(e => e.Deadline.Month < month);
            }
            return result;
        }
    }
}
