using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Data.Dtos.Tarefa
{
    public class CreateTarefaDto
    {
        [Required]
        public string Descricao { get; set; }

        public string DataInicio { get; set; }

        public string DataFinal { get; set; }

    }
}
