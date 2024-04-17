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
        void UploadFileToIdea(Guid ideaId, Guid researcherId, IFormFile file, string title, bool isSubmitedFile);
        Task<FileStream> GetFileToIdea(int fileId);
        Task<IEnumerable<FileDto>> GetFilesToIdea(Guid ideaId);
        Task<IEnumerable<FileDto>> GetFilesToTask(Guid taskId);
        void UploadFileToTask(Guid taskId, Guid researcherId, IFormFile file, string title);
        Task<FileStream> GetFileToTask(int fileId);
        void UploadImageToUser(string userId, IFormFile img);
        Task<FileStream> GetImageToUser(string userId);
        Task DeleteVideo(int videoId);
        Task UpdateVideo(int videoId, IFormFile newVideoFile, string newTitle);
        Task<string> GetVideoToBuffer(int id);

    }
}
