using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Data.Dtos.UsuarioAssistido
{
    public class CreateUsuarioAssistido
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        public string DataNascimento { get; set; }

        public string Telefone { get; set; }

        [Required]
        public string Endereco { get; set; }
        
        [Required]
        public int ResponsavelId { get; set; }
    }
}
