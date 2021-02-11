using System.Security.Cryptography;
using AutoMapper;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.Version1.DTO;

namespace SmartSchool.WebAPI.Helpers
{
    public class SmartSchoolProfile  : Profile
    {
        public SmartSchoolProfile()
        {
            CreateMap<Aluno, AlunoDTO>()
                .ForMember(
                    dest => dest.Nome,
                    opt => opt.MapFrom(src => $"{src.Nome} {src.Sobrenome}")
                )
                .ForMember(
                    dest => dest.Idade,
                    opt => opt.MapFrom(src => src.DataNasc.PegarIdadeAtual())
                )
            ;
            CreateMap<AlunoDTO, Aluno>(); 
            CreateMap<Aluno,AlunoRegistrarDTO>().ReverseMap();
            CreateMap<Aluno,AlunoPachDTO>().ReverseMap();  


            CreateMap<Professor, ProfessorDTO>()
                .ForMember(
                    desp => desp.Nome,
                    opt => opt.MapFrom(src => $"{src.Nome} {src.SobreNome}")
                );
            CreateMap<ProfessorDTO,Professor>();
            CreateMap<Professor,ProfessorRegistrarDTO>().ReverseMap();   

            CreateMap<DisciplinaDTO,Disciplina>().ReverseMap();
            CreateMap<CursoDTO,Curso>().ReverseMap(); 
        }
    }
}