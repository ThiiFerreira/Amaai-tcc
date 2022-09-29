﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Models
{
    public class Tarefa
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public string DataCriacao { get; set; }
        public string HoraAlerta { get; set; }

        public string DataAlerta { get; set; }

        public string DataFinalizacao { get; set; }

        public bool Finalizada { get; set; }

        [Required]
        public int ResponsavelId { get; set; }

        [Required]
        public int IdosoId { get; set; }
    }
}
