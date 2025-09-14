using SchoolWebApi.Domain.Entities;
using SchoolWebApi.Application.Dtos;
using AutoMapper;

namespace SchoolWebApi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<City, CityDto>();
        CreateMap<Department, DepartmentDto>();
        CreateMap<Subject, SubjectDto>();


        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderId == 1 ? "Erkak" : "Ayol "))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));


        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderId == 1 ? "Erkak" : "Ayol "))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.TeacherSubjects.Select(ts => ts.Subject.Name).ToList()));
    }
}