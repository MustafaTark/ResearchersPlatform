using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.Repositories;
using ResearchersPlatform_DAL.Models;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private IFilesRepository _filesRepository;
        public ResearchersController(IRepositoryManager repository , IMapper mapper , IFilesRepository files)
        {
            _repository= repository;
            _mapper= mapper;
            _filesRepository= files;
        }
        [HttpDelete("{researcherId}")]
        public async Task<IActionResult> DeleteResearcher(Guid researcherId)
        {
            if (researcherId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Researcher ID is null or empty! ");

            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if (researcher == null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            _repository.Researcher.DeleteResearcher(researcher);
            await _repository.SaveChangesAsync();
            return NoContent();

        }
        [HttpPut("Speciality/{researcherId}/{specialityId}")]
        public async Task<IActionResult> AssignSpeciality(Guid researcherId,int specialityId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId,trackChanges:true);
            if(researcher == null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var speciality = await _repository.Speciality.GetSpecialityById(specialityId, trackChanges: true);
            if (speciality == null)
            {
                return NotFound($"Speciality with ID {specialityId} doesn't exist in the database");
            }
            _repository.Researcher.AddSpeciality(researcherId, specialityId);
            await _repository.SaveChangesAsync();
            var response = new
            {   
                status = "Success",
                message = $"Speciality {speciality.Name} has been assigned to Researcher {researcherId}"
            };
            return Ok(response);
        }
        [HttpDelete("Speciality/{researcherId}")]
        public async Task<IActionResult> DeleteSpeciality(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            _repository.Researcher.DeleteSpeciality(researcherId);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Papers/{researcherId}")]
        public async Task<IActionResult> UploadPaper([FromBody] List<PaperForCreationDto> paperDto
            ,Guid researcherId)
        {
            if (paperDto is null)
            {
                return BadRequest("PaperForCreationDto object sent from client is null!");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var papers = _mapper.Map<List<Paper>>(paperDto);
            _repository.Researcher.CreatePapersToResearcher(researcherId, papers);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("Papers/{paperId}")]
        public async Task<IActionResult> UpdatePaper([FromBody] PaperDto paperDto, Guid paperId)
        {
            if(paperDto is null)
            {
                return BadRequest("PaperDto object sent from client is null");
            }
            var paper = await _repository.Paper.GetPaperById(paperId, trackChanges: true);
            if(paper is null)
            {
                return NotFound($"Paper With ID {paperId} doesn't exist in the database");
            }
            _mapper.Map(paperDto,paper);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Papers/{paperId}")]
        public async Task<IActionResult> DeletePaper(Guid paperId)
        {
            var paper = await _repository.Paper.GetPaperByIdForDeletion(paperId, trackChanges: false);
            if (paper is null)
            {
                return NotFound($"Paper With ID {paperId} doesn't exist in the database");
            }
            _repository.Paper.DeletePaper(paper);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("{researcherId}")]
        public async Task<IActionResult> GetSingleResearcher(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetSingleResearcherByIdAsync(researcherId,trackChanges: false);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            return Ok(researcher);
        }
    }
}
