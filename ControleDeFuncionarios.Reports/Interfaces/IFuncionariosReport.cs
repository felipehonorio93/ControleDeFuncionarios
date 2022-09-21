using ControleDeFuncionarios.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeFuncionarios.Reports.Interfaces
{
    public interface IFuncionariosReport
    {
        /// <summary>
        /// Método para implementar a geração dos relatórios
        /// </summary>
        byte[] Create(FuncionariosReportModels model);
    }
}
