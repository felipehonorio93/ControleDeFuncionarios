using System.ComponentModel.DataAnnotations;

namespace ControleDeFuncionarios.Mvc.Models
{
    public class FuncionarioRelatorioModel
    {

        [Required(ErrorMessage = "Por favor, escolha o formato do relatório.")]
        public string Formato { get; set; }

    }
}
