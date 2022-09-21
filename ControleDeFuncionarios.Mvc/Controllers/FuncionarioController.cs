using ControleDeFuncionarios.Data.Entities;
using ControleDeFuncionarios.Data.Repositories;
using ControleDeFuncionarios.Mvc.Models;
using ControleDeFuncionarios.Reports.Interfaces;
using ControleDeFuncionarios.Reports.Models;
using ControleDeFuncionarios.Reports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace ControleDeFuncionarios.Mvc.Controllers
{
    [Authorize]
    public class FuncionarioController : Controller
    {
        //ROTA: /Funcionario/Cadastro
        public IActionResult Cadastro()
        {
            return View();
        }

        
        [HttpPost]//SUBMIT POST do formulário
        public IActionResult Cadastro(CadastroFuncionarioModel model, [FromServices] IWebHostEnvironment environment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //capturar os dados do Funcionário para
                    //gravar no banco de dados
                    var funcionario = new Funcionario()
                    {
                        IdFuncionario = Guid.NewGuid(),
                        Nome = model.Nome,
                        Email = model.Email,
                        Cpf = model.Cpf,
                        Salario = model.Salario,
                        Cargo = model.Cargo,
                        Telefone = model.Telefone,
                        DataAdmissao = Convert.ToDateTime(model.DataAdmissao),
                        DataNascimento = Convert.ToDateTime(model.DataNascimento),
                        Foto = "/img/usuarios/avatar.png",
                        IdUsuario = GetUsuarioAutenticado().IdUsuario
                    };

                    //Verificar se foi enviada uma foto de Funcionário
                    UploadFoto(funcionario, environment);

                    //gravando no banco de dados
                    var funcionarioRepository = new FuncionarioRepository();
                    funcionarioRepository.Create(funcionario);

                    TempData["MensagemSucesso"] = $"Funcionario {funcionario.Nome},Cadstrado com sucesso";
                    ModelState.Clear();//Limpar os campos

                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao cadastrar funcionário: {e.Message}";

                }
            }
            return View();
        }

        //ROTA: /Funcionario/Consulta
        public IActionResult Consulta()
        {
            //Criando uma lista da Classe ConsultaFuncionarioModel
            var lista = new List<ConsultaFuncionarioModel>();

            try
            {
                var funcionarioRepository = new FuncionarioRepository();
                foreach (var item in funcionarioRepository.GetByUsuario
                    (GetUsuarioAutenticado().IdUsuario))
                {

                    var model = new ConsultaFuncionarioModel
                    {
                        IdFuncionario = item.IdFuncionario,
                        Cargo = item.Cargo,
                        Nome = item.Nome,
                        Telefone = item.Telefone,
                        DataAdmissao = item.DataAdmissao.ToString("dd/MM/yyyy"),
                        Foto= item.Foto 
                    };

                    lista.Add(model);
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Falha ao consultar Funcionários: {e.Message}";

            }

            return View(lista);
        }

        //ROTA: /Funcionário/Exclusao/id
        public IActionResult Exclusao(Guid id, [FromServices] IWebHostEnvironment environment)
        {
            try
            {
                //Acessando o Repositorio
                var funcionarioRepository = new FuncionarioRepository();
                var funcionario = funcionarioRepository.GetById(id, GetUsuarioAutenticado().IdUsuario);

                //Excluindo a foto do Funcionário
                if (!funcionario.Foto.Equals("img/usuarios/avatar.png"))
                System.IO.File.Delete(environment.WebRootPath + funcionario.Foto);

                //Excluindo o Funcionario
                funcionarioRepository.Delete(funcionario);
                TempData["MensagemSucesso"] = $"Funcionário {funcionario.Nome}, Excluido com sucesso";

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Falha ao excluir o Funcionário : {e.Message}";

            }

            return RedirectToAction("Consulta");
        }

        //ROTA: /Funcionario/Edição
        public IActionResult Edicao(Guid id)
        {

            var model = new FuncionarioEdicaoModel();
            try
            {
                //Buscando o Funcionario no Banco de Dados
                var FuncionarioRepository = new FuncionarioRepository();
                var Funcionario = FuncionarioRepository.GetById(id, GetUsuarioAutenticado().IdUsuario);

                //Preencher a model com os dados do funcionário

                model.IdFuncionario = Funcionario.IdFuncionario;
                model.Nome = Funcionario.Nome;
                model.Cpf = Funcionario.Cpf;
                model.Telefone = Funcionario.Telefone;
                model.Salario = Funcionario.Salario;
                model.Cargo = Funcionario.Cargo;
                model.Email = Funcionario.Email;
                model.DataNascimento = Funcionario.DataNascimento.ToString("dd/MM/yyyy");
                model.DataAdmissao = Funcionario.DataAdmissao.ToString("dd/MM/yyyy");
            }
            
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Falha ao editar o Funcionário: {e.Message}";
            }
            //Enviando a model para a página
            return View(model);
        }


        [HttpPost] //SUBMIT POST do formulário
        public IActionResult Edicao(FuncionarioEdicaoModel model, [FromServices] IWebHostEnvironment environment)
        {
            //Verificar se Todos os Campos passaram pela validação
            if (ModelState.IsValid)
            {
                try
                {
                    var funcionarioRepository = new FuncionarioRepository();

                    var funcionario = funcionarioRepository.GetById(model.IdFuncionario,GetUsuarioAutenticado().IdUsuario);
                                        
                    funcionario.Nome = model.Nome;
                    funcionario.Telefone = model.Telefone;
                    funcionario.Cpf = model.Cpf;
                    funcionario.Cargo = model.Cargo;
                    funcionario.DataAdmissao = Convert.ToDateTime(model.DataAdmissao);
                    funcionario.DataNascimento = Convert.ToDateTime(model.DataNascimento);
                    funcionario.Email = model.Email;
                    funcionario.Salario = model.Salario;

                    //Verificar se foi enviada uma foto de Funcionário
                    UploadFoto(funcionario, environment);
                    
                    //Atualizando no Banco de Dados
                   funcionarioRepository.Update(funcionario);

                    TempData["MensagemSucesso"] = $"Funcionário {funcionario.Nome} atualizado com sucesso.";
                    return RedirectToAction("Consulta"); //redirecionamento
                }
                catch (Exception e)
                {

                    TempData["MensagemErro"] = $"Falha ao editar o Funcionário: {e.Message}";
                }
            }
            return View(model);
        }
        
        //ROTA: /Funcionario/Relatorio
        public IActionResult Relatorio()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Relatorio( FuncionarioRelatorioModel model)
        {
            if (ModelState.IsValid)
            {
                var usuarioRepository= new UsuarioRepository();
                var funcionarioRepository= new FuncionarioRepository();

                try
                {
                    //criar o modelo de dados para geração do relatório
                    var FuncionarioReportModel = new FuncionariosReportModels()
                    {
                        DataHora = DateTime.Now,
                        Usuario = usuarioRepository.GetByEmail(GetUsuarioAutenticado().Email),
                        Funcionarios = funcionarioRepository.GetByUsuario(GetUsuarioAutenticado().IdUsuario)
                        
                    };

                    //polimorfismo
                    IFuncionariosReport funcionarioReport = null;
                    var nomeArquivo = string.Empty;
                    var tipoArquivo = string.Empty;

                    switch (model.Formato)
                    {
                        case "excel":
                            funcionarioReport = new FuncionariosReportExcel();
                            nomeArquivo = $"Funcionario_{Guid.NewGuid()}.xlsx";
                            tipoArquivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            
                            break;

                        case "pdf":
                            funcionarioReport = new FuncionariosReportPdf();
                            nomeArquivo = $"Funcionario_{Guid.NewGuid()}.pdf";
                            tipoArquivo = "application/pdf";
                            break;
                                
                    }
                    return File(funcionarioReport.Create(FuncionarioReportModel), tipoArquivo, nomeArquivo );
                }

                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao gerar relatório: { e.Message}";                    
                }

            }
            return View();
        }

        private void UploadFoto(Funcionario funcionario, IWebHostEnvironment environment)
        {
            //verificar se foi enviado uma foto do contato (upload)
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var extensao = Path.GetExtension(file.FileName);
                if (extensao.Equals(".jpg") || extensao.Equals(".jpeg") || extensao.Equals(".png"))
                {
                    //caminho da foto no banco de dados
                    funcionario.Foto = $"/img/funcionarios/{funcionario.IdFuncionario}.png";
                    //salvar a foto dentro da pasta /wwwroot
                    using (var stream = new FileStream(environment.WebRootPath+funcionario.Foto, FileMode.Create))
                    {
                        file.CopyTo(stream); //upload do arquivo!
                    }
                }
            }
        }

        //método no controlador que retorne os dados do usuário autenticado
        //no AspNet MVC (Cookie de autenticação)
        private AuthenticationModel GetUsuarioAutenticado()
        {
            //ler os dados do usuário autenticado que estão gravados
            //no arquivo de cookie do AspNet (em formato JSON)
            var json = User.Identity.Name;
            //deserializar e retornar estes dados na forma de objeto
            return JsonConvert.DeserializeObject<AuthenticationModel>(json);
        }


    }
}
