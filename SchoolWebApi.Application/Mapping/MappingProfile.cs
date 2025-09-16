using SchoolWebApi.Domain.Entities;
using SchoolWebApi.Application.Dtos;
using AutoMapper;
using SchoolWebApi.Application.Dtos.StudentDTOs;
using SchoolWebApi.Application.Dtos.TeacherDTOs;

namespace SchoolWebApi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<City, CityDto>();
        CreateMap<Department, DepartmentDto>();
        CreateMap<Subject, SubjectDto>();

        // Student <-> StudentDto mapping (ikki tomonlama)
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderId == 1 ? "Erkak" : "Ayol"))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        // Create va Update uchun alohida DTO mappinglar
        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore());

        CreateMap<StudentUpdateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore());

        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderId == 1 ? "Erkak" : "Ayol"))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.TeacherSubjects.Select(ts => ts.Subject.Name).ToList()));
        
        CreateMap<TeacherCreateDto, Teacher>()
            .ForMember(dest => dest.TeacherSubjects, opt => opt.Ignore());
    }
}