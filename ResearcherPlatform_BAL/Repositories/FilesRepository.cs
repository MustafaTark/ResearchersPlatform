using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly AppDbContext _context;
        private readonly IFilesManager _filesManager;
        private readonly IMapper _mapper;


        public FilesRepository(AppDbContext context, IFilesManager filesManager, IMapper mapper)
        {
            _context = context;
            _filesManager = filesManager;
            _mapper = mapper;
        }
        public async Task<IEnumerable<VideoDto>> GetAllVideosToSection(Guid sectionId)
          => await _context.Set<Video>().Where(v=>v.SectionId == sectionId)
            .ProjectTo<VideoDto>(_mapper.ConfigurationProvider)
            .ToListAsync();   

        public async Task<FileStream> GetVideoToSection(int videoId)
        {
           var url=await _context.Videos.Where(v=>v.Id== videoId)
                .Select(v=>v.VideoUrl)
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
    }
}
