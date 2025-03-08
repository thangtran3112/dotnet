

using AutoMapper;
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
        }
    }

}