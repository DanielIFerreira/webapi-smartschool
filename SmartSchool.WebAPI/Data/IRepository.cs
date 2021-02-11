using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Remove<T>(T entity) where T: class;
        bool SaveChanges();

        Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false);
        Aluno[] GetAllAlunos(bool includeProfessor = false);
        Task<Aluno[]> GetAllAlunosByDisciplinaIdAsync(int disciplinaId, bool includeProfessor = false);
        
        Aluno GetAllAlunosById(int id,bool includeProfessor = false);
        //Aluno GetAllAlunosByName(string name,bool includeProfessor = false);

        Professor[] GetAllProfessores(bool includeALuno = false);
        Task<PageList<Professor>> GetAllProfessoresAsync(PageParams pageParams,bool includeALuno = false);
        Professor[] GetAllProfessoresByDisciplinas(int disciplinaId,bool includeAluno = false);
        Professor GetAlProfessoresById(int id,bool includeAluno = false);
        Professor[] GetProfessorByAlunoId(int alunoId,bool includeAlunos = false);
        //Professor GetAllProfessorByName(string name,bool includeProfessor = false);
        


    }  
    
}
