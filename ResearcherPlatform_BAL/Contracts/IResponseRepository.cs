using Azure;
using ResearchersPlatform_BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IResponseRepository
    {
        Task<IEnumerable<ResponseDto>> GetAllResponses(bool trackChanges);
        Task<ResponseDto?> GetResponseById(bool trackChanges , Guid responseId);
        Task<ResearchersPlatform_DAL.Models.Response?> GetSingleResponseById(Guid responseId);
        Task<IEnumerable<ResponseDto>> GetAllStudentResponses(bool trackChanges , string studentId);
        void CreateResponse(ResearchersPlatform_DAL.Models.Response response);
        void DeleteResponse(ResearchersPlatform_DAL.Models.Response response);


    }
}
