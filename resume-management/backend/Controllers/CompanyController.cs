using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private ApplicationDbContext _context { get; }

        public IMapper _mapper { get; }
        public CompanyController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // CRUD

        // Create
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDTO companyDto)
        {
            var newCompany = _mapper.Map<Company>(companyDto);
            await _context.Companies.AddAsync(newCompany);
            await _context.SaveChangesAsync();
            return Ok("Company created successfully");
        }

        // Read
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<CompanyGetDto>>> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            var convertedCompanies = _mapper.Map<IEnumerable<CompanyGetDto>>(companies);
            return Ok(convertedCompanies);
        }

        // Read (Get Company by ID)
        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<CompanyGetDto>> GetCompanyById(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound("Company not found");
            }
            var convertedCompany = _mapper.Map<CompanyGetDto>(company);
            return Ok(convertedCompany);
        }
    }
}