using System;

namespace SmartSchool.WebAPI.Version1.DTO
{
    /// <summary>
    /// Esté a classe para registro de alunoDTO
    /// </summary>
    public class AlunoRegistrarDTO
    {
        /// <summary>
        /// Id do aluno.
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// Número de matricula do aluno.
        /// </summary>
        /// <value></value>
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNasc { get; set; }
        public DateTime DataIni { get; set; } = DateTime.Now;
        public DateTime? DataFim { get; set; } = null;
        public bool Ativo { get; set; } = true;
    }
}