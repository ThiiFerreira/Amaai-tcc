﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Data.Dtos.Tarefa
{
    public class CreateTarefaDto
    {
        [Required]
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string HoraAlerta { get; set; }
        public string DataAlerta { get; set; }

    }
}
