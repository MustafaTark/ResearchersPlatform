using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.RepoExtentions
{
    public static class StudentRepositoryExtention
    {
        public static IQueryable<Student> Search(
            this IQueryable<Student> students, string searchTerm)
        {
            var result = students;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerCaseTerm = searchTerm.Trim().ToLower();
                result = result.Where(e => e.Firstname!.Contains(lowerCaseTerm)
                || e.Lastname!.Contains(lowerCaseTerm)||e.Email!.Contains(searchTerm));
            }
            return result;
        }
    }
}
