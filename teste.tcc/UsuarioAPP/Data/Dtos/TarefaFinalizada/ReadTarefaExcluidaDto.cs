using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Data.Dtos.Tarefa
{
    public class ReadTarefaExcluidaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string DataCriacao { get; set; }
        public string HoraAlerta { get; set; }
        public string DataAlerta { get; set; }
        public string DataFinalizacao { get; set; }
        public string DataExclusao { get; set; }

        public bool Finalizada { get; set; }
        public int ResponsavelId { get; set; }
        public int IdosoId { get; set; }
    }
}
