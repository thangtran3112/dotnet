using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;

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

    }
}