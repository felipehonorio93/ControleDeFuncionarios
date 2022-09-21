namespace ControleDeFuncionarios.Mvc.Models
{
    public class ConsultaFuncionarioModel
    {
        public Guid IdFuncionario { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
        public string Cargo { get; set; }
        public string Telefone { get; set; }
        public string  DataAdmissao { get; set; }
    }
}
