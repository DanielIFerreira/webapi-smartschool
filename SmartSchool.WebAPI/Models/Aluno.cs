using System;
using System.Collections.Generic;

namespace SmartSchool.WebAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Aluno
    {
        /// <summary>
        /// 
        /// </summary>
        public Aluno() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="matricula"></param>
        /// <param name="nome"></param>
        /// <param name="sobrenome"></param>
        /// <param name="telefone"></param>
        /// <param name="dataNasc"></param>
        public Aluno(int id,
                    int matricula, 
                    string nome, 
                    string sobrenome, 
                    string telefone, 
                    DateTime dataNasc)
        {
            this.Id = id;
            this.Matricula = matricula;
            this.Nome = nome;
            this.Sobrenome = sobrenome;
            this.Telefone = telefone;
            this.DataNasc = dataNasc;

        }
        public int Id { get; set; }
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNasc { get; set; }
        public DateTime DataIni { get; set; } = DateTime.Now;
        public DateTime? DataFim { get; set; } = null;
        public bool Ativo { get; set; } = true;
        public IEnumerable<AlunoDisciplina> AlunoDisciplinas { get; set; }

    }
}