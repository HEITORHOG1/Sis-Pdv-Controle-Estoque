﻿using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("Colaborador")]
    public class Colaborador : EntityBase
    {

        public Colaborador()
        {
            Usuario = new Usuario();
            Departamento = new Departamento();
        }
        public Colaborador(Guid id, string nomeColaborador,
            Guid? DepartamentoId, string cpfColaborador,
            string cargoColaborador, string telefoneColaborador,
            string emailPessoalColaborador, string emailCorporativo, Usuario usuario)
        {
            this.nomeColaborador = nomeColaborador;
            this.DepartamentoId = DepartamentoId;
            this.cpfColaborador = cpfColaborador;
            this.cargoColaborador = cargoColaborador;
            this.telefoneColaborador = telefoneColaborador;
            this.emailPessoalColaborador = emailPessoalColaborador;
            this.emailCorporativo = emailCorporativo;
            Usuario = usuario;
            Id = id;
        }

        public string nomeColaborador { get; set; }
        public virtual Guid? DepartamentoId { get; set; }
        public string cpfColaborador { get; set; }
        public string cargoColaborador { get; set; }
        public string telefoneColaborador { get; set; }
        public string emailPessoalColaborador { get; set; }
        public string emailCorporativo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Departamento Departamento { get; set; }
        public void AlterarColaborador(Guid id, string nomeColaborador, Guid Departamento, string cpfColaborador, string cargoColaborador,
            string telefoneColaborador, string emailPessoalColaborador, string emailCorporativo, Usuario Usuario)
        {
            this.nomeColaborador = nomeColaborador;
            this.DepartamentoId = Departamento;
            this.cpfColaborador = cpfColaborador;
            this.cargoColaborador = cargoColaborador;
            this.telefoneColaborador = telefoneColaborador;
            this.emailPessoalColaborador = emailPessoalColaborador;
            this.emailCorporativo = emailCorporativo;
            this.Usuario = Usuario;
            Id = id;
        }


    }
}
