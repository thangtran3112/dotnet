

using AutoMapper;
using backend.Core.Dtos.Candidate;
using backend.Core.Dtos.Company;
using backend.Core.Dtos.Job;
using backend.Core.Entities;

namespace backend.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<CompanyCreateDTO, Company>();
            CreateMap<Company, CompanyGetDto>();

            CreateMap<JobCreateDto, Job>();
            CreateMap<Job, JobGetDto>()
                .ForMember(jobGetDto => jobGetDto.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<CandidateCreateDto, Candidate>();
            CreateMap<Candidate, CandidateGetDto>()
                .ForMember(candidateGetDto => candidateGetDto.JobTitle, opt => opt.MapFrom(src => src.Job.Title));
        }
    }

}