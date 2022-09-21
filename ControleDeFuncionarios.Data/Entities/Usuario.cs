using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeFuncionarios.Data.Entities
{   
    /// <summary>
    /// Classe de entidade
    /// </summary>
    public class Usuario
    {
        //Propriedades
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public List<Funcionario> Funcionarios { get; set; }
    }
}
