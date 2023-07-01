using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ResearchersPlatform_BAL.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly AppDbContext _context;
        private readonly IFilesManager _filesManager;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public FilesRepository(AppDbContext context, IFilesManager filesManager, IMapper mapper, IMemoryCache memoryCache)
        {
            _context = context;
            _filesManager = filesManager;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<VideoDto>> GetAllVideosToSection(Guid sectionId)
        => await _context.Set<Video>()
                         .Where(v => v.SectionId == sectionId)
                         .ProjectTo<VideoDto>(_mapper.ConfigurationProvider)
                         .ToListAsync();
        public async Task<FileStream> GetVideoToSection(int videoId)
        {
             var url = await _context.Videos.Where(v => v.Id == videoId)
                                            .Select(v => v.VideoUrl)
                                            .SingleOrDefaultAsync();
              return _filesManager.GetFile(url!);
               
        }

        public void UploadVideoToSection(Guid sectionId,IFormFile video,string title)
        {
            
               var url= _filesManager.UploadFiles(video);
                var videoEntity = new Video
                {
                    Title = title,
                    SectionId = sectionId,
                    VideoUrl = url,
                };
                _context.Videos.Add(videoEntity);
            
        }
        public void UploadFileToIdea(Guid ideaId,Guid researcherId,IFormFile file,string title,bool isSubmitedFile)
        {
            
               var url= _filesManager.UploadFiles(file);
            var fileEntity = new IdeaFile
            {
                Title = title,
                Path = url,
                IdeaId = ideaId,
                SenderId = researcherId,
                IsSubmitedFile = isSubmitedFile
            };
                _context.IdeaFiles.Add(fileEntity);
            
        }
        public void UploadFileToTask(Guid taskId,Guid researcherId,IFormFile file,string title)
        {
            
               var url= _filesManager.UploadFiles(file);
            var fileEntity = new TaskFile
            {
                Title = title,
                Path = url,
                TaskId = taskId,
                SenderId = researcherId
          };
                _context.TaskFiles.Add(fileEntity);
            
        }
        public async Task<IEnumerable<FileDto>> GetFilesToIdea(Guid ideaId)
        {
            var files = await _context.IdeaFiles.Where(i => i.IdeaId == ideaId)
                .ProjectTo<FileDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return files;
        }
        public async Task<FileStream> GetFileToIdea(int fileId)
        {
            var file = await _context.IdeaFiles.Where(i => i.ID == fileId).FirstOrDefaultAsync();
            return _filesManager.GetFile(file!.Path);
        }
        public async Task<IEnumerable<FileDto>> GetFilesToTask(Guid taskId)
        {
            var files = await _context.TaskFiles.Where(i => i.TaskId == taskId)
                .ProjectTo<FileDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return files;
        }
        public async Task<FileStream> GetFileToTask(int fileId)
        {
            var file = await _context.TaskFiles.Where(i => i.ID == fileId).FirstOrDefaultAsync();
            return _filesManager.GetFile(file!.Path);
        }
        public void UploadImageToUser(string userId,IFormFile img)
        {
            var url = _filesManager.UploadFiles(img);
            var user = _context.Users.FirstOrDefault(u=>u.Id==userId);
            user!.ImageUrl = url;
        }
        public async Task<FileStream> GetImageToUser(string userId)
        {
            var img = await _context.Users.AsNoTracking().Where(u=>u.Id.Equals(userId)).Select(u=>u.ImageUrl).FirstOrDefaultAsync();
            return _filesManager.GetFile(img!);
        }
        //public async Task<FileStream> GetVideoToSection(int videoId)
        //{
        //    var url = await _context.Videos.Where(v => v.Id == videoId)
        //                                   .Select(v => v.VideoUrl)
        //                                   .SingleOrDefaultAsync();
        //    return _filesManager.GetFile(url!);

        //}
    }
}
