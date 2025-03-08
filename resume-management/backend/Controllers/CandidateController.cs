using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;

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
    }
}