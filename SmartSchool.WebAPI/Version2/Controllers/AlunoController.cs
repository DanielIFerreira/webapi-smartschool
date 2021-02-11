using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;
using AutoMapper;
using SmartSchool.WebAPI.Version1.DTO;

namespace SmartSchool.WebAPI.Version2.Controllers
{
    
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    
    public class AlunoController : ControllerBase
    {
        //Injeção da Interface
        private readonly IRepository _repo;

        //Injeção de Auto Mapper
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public AlunoController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        /// <summary>
        /// Método responsalvel por retornar todos os alunos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var alunos = _repo.GetAllAlunos(true);

            //Utilizando AutoMapper
            return Ok(_mapper.Map<IEnumerable<AlunoDTO>>(alunos));
        }
        /// <summary>
        /// Método responsálvel por retornar um único aluno pelo id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var aluno = _repo.GetAllAlunosById(id, false);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado!");
            }

            var alunoDTO = _mapper.Map<AlunoDTO>(aluno);
            return Ok(alunoDTO);
        }

        
        // [HttpGet("{name}")]
        // public IActionResult GetName(string name)
        // {
        //     var aluno = _repo.GetAllAlunosByName(name, true);
        //     if (aluno == null)
        //     {
        //         return BadRequest("Aluno não encontrado!");
        //     }
        //     var alunoDTO = _mapper.Map<AlunoDTO>(aluno);
        //     return Ok(alunoDTO);
        // }

        /// <summary>
        /// Método responsálvel por criar um aluno.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] AlunoRegistrarDTO model)
        {
            var aluno= _mapper.Map<Aluno>(model);
            _repo.Add(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDTO>(aluno));
            }
            return BadRequest("Não foi possivel cadastrar!");
        }

        /// <summary>
        /// Método responsável por atualizar um aluno.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDTO model)
        {
            var aluno = _repo.GetAllAlunosById(id);
            if (aluno == null) return BadRequest($"Aluno não foi encontrado!");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDTO>(aluno));
            }
            return BadRequest("Não foi possivel cadastrar!");
        }

        

        /// <summary>
        /// Método responsável por deletar um aluno.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAllAlunosById(id);
            if (aluno == null) return BadRequest($"Aluno com id {id} não foi encontrado!");
            if (aluno.Id < 0) return BadRequest("Id invalido!");

            _repo.Remove(aluno);

            if (_repo.SaveChanges())
            {
                return Ok($"Aluno: {aluno} foi deletado com sucesso!");
            }
            return BadRequest("Não foi possivel deletar!");

        }

    }
}