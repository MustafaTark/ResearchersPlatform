using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface ISpecialityRepository
    {
        void CreateSpeciality(Specality specality);
        Task<IEnumerable<SpecialityDto?>> GetAllSpecialites(bool trackChanges);
        Task<SpecialityDto?> GetSpecialityById(int specialityId , bool trackChanges);

    }
}
