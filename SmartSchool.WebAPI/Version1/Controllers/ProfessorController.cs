using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.Version1.DTO;

namespace SmartSchool.WebAPI.Controllers
{
   
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        
        public ProfessorController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        
        /// <summary>
        /// Método responsável por retornar todos os professores.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var Professor = _repo.GetAllProfessores(true);
            return Ok(_mapper.Map<IEnumerable<ProfessorDTO>>(Professor));
        }

         [HttpGet("getRegister")]
        public IActionResult GetRegister()
        {
            return Ok(new ProfessorRegistrarDTO());
        }

        /// <summary>
        /// Método responsável por retornar um professor pelo id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var Professor = _repo.GetAlProfessoresById(id, true);
            if (Professor == null) return BadRequest("O Professor não foi encontrado");

            var professorDto = _mapper.Map<ProfessorDTO>(Professor);

            return Ok(Professor);
        }

         [HttpGet("ByAluno{alunoId}")]
        public IActionResult GetByAlunoId(int alunoId)
        {
            
            var professores= _repo.GetProfessorByAlunoId(alunoId, true);
            if (professores == null) return BadRequest("Professores não encontrado!");

            return Ok(_mapper.Map<IEnumerable<ProfessorDTO>>(professores));
        }

       
        // [HttpGet("{name}")]
        // public IActionResult GetName(string name)
        // {
        //     var professor = _repo.GetAllProfessorByName(name, true);
        //     if (professor == null) return BadRequest("Professor não encontrado!");

        //      var professorDTO = _mapper.Map<ProfessorDTO>(professor);
        //     return Ok(professorDTO);
        // }

        /// <summary>
        /// Método responsável por criar um professor.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(ProfessorRegistrarDTO model)
        {
            var professor= _mapper.Map<Professor>(model);
            _repo.Add(professor);
            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDTO>(professor));
            }
            return BadRequest("Não foi possivel cadastrar!");

        }

        /// <summary>
        /// Método responsável por atuálizar um professor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegistrarDTO model)
        {
            
            var professor = _repo.GetAlProfessoresById(id);
            if (id < 0) return BadRequest("Id não pode ser menor que 0!");
            if (professor == null) BadRequest("As informações não pode ser vazia!");
            
            _mapper.Map(model, professor);
            _repo.Update(professor);

            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDTO>(professor));
            }
            return BadRequest("Nao foi possivel atualizar!");

        }

        /// <summary>
        /// Método responsável por atuálizar um professor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProfessorRegistrarDTO model)
        {
            var professor = _repo.GetAlProfessoresById(id, false);
            if (id < 0) return BadRequest("Id não pode ser menor que 0!");
            if (professor == null) BadRequest("As informações não pode ser vazia!");

             _mapper.Map(model, professor);
            _repo.Update(professor);

            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDTO>(professor));
            }
            return BadRequest("Professor não atualizado!");

        }


        /// <summary>
        /// Método responsável por deletar um professor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var professores = _repo.GetAlProfessoresById(id);
            if (id < 0) return BadRequest("Id não pode ser menor que 0!");


            _repo.Remove(professores);

            if (_repo.SaveChanges())
            {
                return Ok("Deletado com sucesso!");
            }
            return BadRequest("Não foi possivel deletar!");

        }
    }
}