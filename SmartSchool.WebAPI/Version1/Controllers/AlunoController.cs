using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Version1.DTO;
using SmartSchool.WebAPI.Models;
using AutoMapper;
using System.Threading.Tasks;
using SmartSchool.WebAPI.Helpers;


namespace SmartSchool.WebAPI.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            var alunos = await _repo.GetAllAlunosAsync(pageParams, true);
            var alunoResult = _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
            Response.AddPagination(alunos.CurrentPage, alunos.TotalPage, alunos.PageSize, alunos.TotalCount);
            //Utilizando AutoMapper
            return Ok(alunoResult);
        }

        [HttpGet("getRegister")]
        public IActionResult GetRegister()
        {
            return Ok(new AlunoRegistrarDTO());
        }


        /// <summary>
        /// Método responsável por retonar apenas um único AlunoDTO.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ByDisciplina/{id}")]
        public async Task<IActionResult> GetByDisciplinaId(int id)
        {
            var result = await _repo.GetAllAlunosByDisciplinaIdAsync(id, false);
            return Ok(result);
        }
        /// <summary>
        /// Método responsálvel por retornar um único aluno pelo id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repo.GetAllAlunosById(id, true);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado!");
            }

            var alunoDTO = _mapper.Map<AlunoRegistrarDTO>(aluno);
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
        public IActionResult Post(AlunoRegistrarDTO model)
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
        /// Método responsável por atualizar um aluno.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult Putch(int id, AlunoPachDTO model)
        {
            var aluno = _repo.GetAllAlunosById(id);
            if (aluno == null) return BadRequest($"Aluno não foi encontrado!");

            _mapper.Map(model, aluno);
            _repo.Update(aluno);

            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoPachDTO>(aluno));
            }
            return BadRequest("Não foi possivel atualizar");
        }

        /// <summary>
        /// Método responsável por atualizar um aluno.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trocaEstado"></param>
        /// <returns></returns>
        [HttpPatch("{id}/trocarEstado")]
        public IActionResult trocarEstado(int id, TrocaEstado trocaEstado)
        {
            var aluno = _repo.GetAllAlunosById(id);
            if (aluno == null) return BadRequest($"Aluno não foi encontrado!");

            aluno.Ativo = trocaEstado.Estado;
           
            _repo.Update(aluno);

            if (_repo.SaveChanges())
            {
                var msn = aluno.Ativo ? "ativado" : "desativado";
               return Ok(new { message = $"Aluno {msn} com sucesso!"});
            }
            return BadRequest("Não foi possivel atualizar");
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