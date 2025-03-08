using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private ApplicationDbContext _context { get; }

        public IMapper _mapper { get; }
        public JobController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateJob([FromBody] JobCreateDto jobDto)
        {
            var newJob = _mapper.Map<Job>(jobDto);
            await _context.Jobs.AddAsync(newJob);
            await _context.SaveChangesAsync();
            return Ok("Job created successfully");
        }
    }
}