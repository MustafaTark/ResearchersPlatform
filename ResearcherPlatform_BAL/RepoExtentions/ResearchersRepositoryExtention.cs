using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.RepoExtentions
{
    public static class ResearchersRepositoryExtention
    {
        public static IQueryable<Researcher> Search(
            this IQueryable<Researcher> researchers, string searchTerm, Level? level, int specality)
        {
            var result = researchers;
            if (!string.IsNullOrEmpty(level.ToString()))
            {

                result = result.Where(e => e.Level==level);
            }
            if (specality is not 0)
            {

                result = result.Where(e => e.SpecalityObject!.Id!.Equals(specality));
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerCaseTerm = searchTerm.Trim().ToLower();
                result = result.Where(e => e.StudentObj!.Firstname!.Contains(lowerCaseTerm)
                || e.StudentObj!.Lastname!.Contains(lowerCaseTerm));
            }
            return result;
        }
    }
}
