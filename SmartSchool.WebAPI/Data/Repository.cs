using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;   
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0 );
        }

        public Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }
            
            query = query.AsNoTracking().OrderBy(c => c.Id);
            //Para fazer paginação buscando pelo nome ou sobrenome.
            if(!string.IsNullOrEmpty(pageParams.Nome))
                query = query.Where(aluno => aluno.Nome.ToUpper()
                                                        .Contains(pageParams.Nome.ToUpper()) ||
                                              aluno.Sobrenome.ToUpper()
                                                        .Contains(pageParams.Nome.ToUpper())          
                                                        );
            if(pageParams.Matricula > 0)
                query = query.Where(aluno => aluno.Matricula == pageParams.Matricula);
           
            if(pageParams.Ativo != null)
                query = query.Where(aluno => aluno.Ativo == (pageParams.Ativo != 0));
            // return query.ToListAsync();
            return  PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }


        public Aluno[] GetAllAlunos(bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }
            
            query = query.AsNoTracking().OrderBy(c => c.Id);
            return query.ToArray();
        }
    
        //Forma Assincrona
        
        public async Task<Aluno[]> GetAllAlunosByDisciplinaIdAsync(int disciplinaId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(a => a.AlunoDisciplinas)
                             .ThenInclude(ad => ad.Disciplina)
                             .ThenInclude(d => d.Professor);
            }

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(aluno => aluno.AlunoDisciplinas.Any(ad => ad.DisciplinaId == disciplinaId));

            return await query.ToArrayAsync();
        }
        

        public Aluno GetAllAlunosById(int id,bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }
            
            query = query.AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Where(al => al.Id == id);
            return query.FirstOrDefault();
        }

        public Aluno GetAllAlunosByName(string name, bool includeProfessor = false)
        {
           IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }
            
            query = query.AsNoTracking()
                    .OrderBy(c => c.Nome)
                    .Where(al => al.Nome == name);
            return query.FirstOrDefault();
        }

        public Professor[] GetAllProfessores(bool includeALuno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeALuno)
            {
                query = query.Include(a => a.Disciplinas)
                    .ThenInclude(d => d.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Aluno);
            }
            
            query = query.AsNoTracking().OrderBy(c => c.Id);
            return query.ToArray();
        }

        public Task<PageList<Professor>> GetAllProfessoresAsync(PageParams pageParams,bool includeALuno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeALuno)
            {
                query = query.Include(a => a.Disciplinas)
                    .ThenInclude(d => d.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Aluno);
            }
            
            query = query.AsNoTracking().OrderBy(c => c.Id);
            //return query.ToArray();
            return PageList<Professor>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize); 
        }


        public Professor[] GetAllProfessoresByDisciplinas(int disciplinaId,bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAluno)
            {
                query = query.Include(p => p.Disciplinas)
                    .ThenInclude(d => d.AlunosDisciplinas)
                    .ThenInclude(pd => pd.Aluno);
            }
            query = query.AsNoTracking()
                    .OrderBy(aluno => aluno.Id)
                    .Where(aluno => aluno.Disciplinas
                    .Any(d => d.AlunosDisciplinas
                    .Any(ad => ad.DisciplinaId == disciplinaId)
                    ));

            return query.ToArray();
           
        }

        public Professor GetAlProfessoresById(int id,bool includeAluno = true)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAluno)
            {
                query = query.Include(a => a.Disciplinas)
                    .ThenInclude(d => d.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Aluno);
            }
            
            query = query.AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Where(professor => professor.Id == id);

            return query.FirstOrDefault();
        }

        public Professor[] GetProfessorByAlunoId(int alunoId,bool includeAlunos = true)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAlunos)
            {
                query = query.Include(a => a.Disciplinas)
                    .ThenInclude(d => d.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Aluno);
            }
            
            query = query.AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Where(aluno => aluno.Disciplinas.Any(
                            d => d.AlunosDisciplinas.Any(ad => ad.AlunoId == alunoId)
                    ) );

            return query.ToArray();
        }

        // public Professor GetAllProfessorByName(string name, bool includeProfessor = false)
        // {
        //     IQueryable<Professor> query = _context.Professores;

        //     if(includeProfessor)
        //     {
        //         query = query.Include(a => a.Disciplinas)
        //             .ThenInclude(d => d.AlunosDisciplinas)
        //             .ThenInclude(ad => ad.Aluno);
        //     }
            
        //     query = query.AsNoTracking()
        //             .OrderBy(c => c.Nome)
        //             .Where(professor => professor.Nome == name);

        //     return query.FirstOrDefault();
        // }

       
    }
}