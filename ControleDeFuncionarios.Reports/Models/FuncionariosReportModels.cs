using ControleDeFuncionarios.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeFuncionarios.Reports.Models
{
    /// <summary>
    /// Modelo de classes para o preenchimento do relatotório de Funcionários
    /// </summary>
    public class FuncionariosReportModels
    {
        public DateTime? DataHora { get; set; }
        public Usuario? Usuario { get; set; }
        public List<Funcionario>? Funcionarios { get; set; }
    }
}
