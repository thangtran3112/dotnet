using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private ApplicationDbContext _context { get; }

        public IMapper _mapper { get; }
        public CandidateController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // CRUD

        // Using FromForm to handle file uploads for resume pdf
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDto candidateCreateDto, IFormFile pdfFile)
        {
            // First => Save pdf to Server
            var fiveMb = 5 * 1024 * 1024;
            var pdfMimeType = "application/pdf";

            if (pdfFile.Length > fiveMb)
            {
                return BadRequest("File should not be larger than 5MB");
            }

            if (pdfFile.ContentType != pdfMimeType)
            {
                return BadRequest("File should be a PDF");
            }

            var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
            // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes", resumeUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "documents", "pdfs", resumeUrl);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            // Then => save url into our entity
            var newCandidate = _mapper.Map<Candidate>(candidateCreateDto);
            newCandidate.ResumeUrl = resumeUrl;
            await _context.Candidates.AddAsync(newCandidate);
            await _context.SaveChangesAsync();

            return Ok("Candidate Saved Successfully");
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<CandidateGetDto>>> GetAllCandidates()
        {
            // Include(c => c.Job) will tell EF Core to eagerly load the Job entity related to each Candidate
            var candidates = await _context.Candidates.Include(c => c.Job).ToListAsync();
            var convertedCandidates = _mapper.Map<IEnumerable<CandidateGetDto>>(candidates);
            return Ok(convertedCandidates);
        }

        // Read (Get) candidate by id
        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<CandidateGetDto>> GetCandidateById(long id)
        {
            var candidate = await _context.Candidates.Include(c => c.Job).FirstOrDefaultAsync(c => c.ID == id);

            if (candidate == null)
            {
                return NotFound($"Candidate with ID {id} not found");
            }

            var convertedCandidate = _mapper.Map<CandidateGetDto>(candidate);
            return Ok(convertedCandidate);
        }

        // Read (Download pdf resume file) given the resume url (This is not an async method)
        // We should use async method when storing the file in external storage like AWS S3
        [HttpGet]
        [Route("download/{url}")]
        public IActionResult DownloadResumeByUrl(string url)
        {
            // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes", url);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "documents", "pdfs", url);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Resume file not found for: {url}");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", url);
        }

        // Read (Download pdf resume file) given the candidate id
        [HttpGet]
        [Route("downloadById/{id}")]
        public async Task<IActionResult> DownloadResumeByCandidateId(long id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound($"Candidate with ID {id} not found");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "documents", "pdfs", candidate.ResumeUrl);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Resume file not found for candidate ID: {id}");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", candidate.ResumeUrl);
        }
    }
}