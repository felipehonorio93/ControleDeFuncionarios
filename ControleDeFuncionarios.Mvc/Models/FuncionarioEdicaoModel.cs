using System.ComponentModel.DataAnnotations;

namespace ControleDeFuncionarios.Mvc.Models
{
    /// <summary>
    /// Classe para edição de Funcionário
    /// </summary>
    public class FuncionarioEdicaoModel
    {
        public Guid IdFuncionario { get; set; }//Campo oculto

        [MinLength(7, ErrorMessage = "Por favor informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} de caracteres.")]
        [Required(ErrorMessage = "Por favor informe o nome do Funcionário.")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Por favor informe um Endereço de Email válido.")]
        [Required(ErrorMessage = "Por favor informe o Email do Funcionário")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor, informeo telefone do Funcionário.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de nascimento do Funcionário.")]
        public string DataNascimento { get; set; }


        //[CpfValidation(ErrorMessage ="Informe um Cpf válido.")]
        [Required(ErrorMessage = "Informe o cpf do Funcionário")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de admissao do Funcionário.")]
        public string DataAdmissao { get; set; }

        [Required(ErrorMessage = "Informe o Cargo do Funcionário")]
        public string Cargo { get; set; }
              

        [Required(ErrorMessage = "Informe o Salário do Funcionário")]
        public decimal Salario { get; set; }

      // public string Foto { get; set; }
    }
}
