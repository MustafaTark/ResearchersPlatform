using Microsoft.AspNetCore.Http;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IFilesRepository
    {
        Task<IEnumerable<VideoDto>> GetAllVideosToSection(Guid sectionId);
        Task<FileStream> GetVideoToSection(int videoId);
        public void UploadVideoToSection(Guid sectionId, IFormFile video, string title);
    }
}
