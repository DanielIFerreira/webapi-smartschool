using System;

namespace SmartSchool.WebAPI.Version1.DTO
{
    /// <summary>
    /// Esté a classe para registro de professorDTO.
    /// </summary>
    public class ProfessorRegistrarDTO
    {
        /// <summary>
        /// Id do professor.
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// Número de registro.
        /// </summary>
        /// <value></value>
        public int Registro { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataIni { get; set; } = DateTime.Now;
        public DateTime? DataFim { get; set; } = null;
        public bool Ativo { get; set; } = true;
    }
}