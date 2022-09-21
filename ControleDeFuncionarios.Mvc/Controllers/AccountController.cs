using Bogus;
using ControleDefuncionarios.Messages.Services;
using ControleDeFuncionarios.Data.Entities;
using ControleDeFuncionarios.Data.Repositories;
using ControleDeFuncionarios.Mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ControleDeFuncionarios.Mvc.Controllers
{
    public class AccountController : Controller
    {
        //ROTA: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost] //Receber o SUBMIT do formulário
        public IActionResult Login(AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //consultar o usuário no banco de dados
                    //através do email e da senha
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, model.Senha);
                    //verificar se o usuário foi encontrado
                    if (usuario != null)
                    {
                        //auteticando o usuário no AspNet MVC
                        SignIn(usuario);

                        //redirecionar para a página de consulta de contatos
                        return RedirectToAction("Consulta", "Funcionario");
                        //Contatos/Consulta
                    }
                    else
                    {
                        TempData["Mensagem"] = "Acesso negado.";
                    }
                }
                catch (Exception e)
                {
                    TempData["Mensagem"]
                   = $"Falha ao autenticar: {e.Message}.";
                }
            }
            return View();
        }
        //ROTA: /Account/Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost] //Receber o SUBMIT do formulário
        public IActionResult Register(AccountRegisterModel model)
        {
            //verificar se os dados da model são válidos, ou seja,
            //todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();
                    if (usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["Mensagem"] = "O email informado já foi cadastrado, tente outro.";
                    }
                    var usuario = new Usuario
                    {
                        IdUsuario = Guid.NewGuid(),
                        Nome = model.Nome,
                        Email = model.Email,
                        Senha = model.Senha,
                        DataCadastro = DateTime.Now
                    };

                    usuarioRepository.Create(usuario);
                    TempData["Mensagem"] = $"Parabéns {usuario.Nome}, sua conta foi cadastrada com sucesso.";
                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao cadastrar usuário: {e.Message}";
                }
            }
            return View();
        }

        //ROTA: /Account/Password
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost] //Receber o SUBMIT do formulário
        public IActionResult Password(AccountPasswordModel model)
        {
            //verificar se os dados da model são válidos, ou seja,
            //todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //procurar o usuário no banco de dados através do email
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmail(model.Email);
                    //verificar se o usuário foi encontrado
                    if (usuario != null)
                    {
                        //gerando uma nova senha para o usuário
                        var faker = new Faker();
                        var novaSenha = $"@{faker.Internet.Password(8)}";
                        //enviando o email de recuperação
                        //de senha para o usuário
                        EnviarEmailDeRecuperacaoDeSenha(usuario, novaSenha);
                        //atualizando a senha do usuário no banco de dados
                        usuarioRepository.Update
                        (usuario.IdUsuario, novaSenha);
                        TempData["Mensagem"] = "Recuperação de senha realizada com sucesso, por favor verifique seu email.";
                     ModelState.Clear();
                    }
                    else
                    {
                        TempData["Mensagem"] = $"Usuário não encontrado, verifique o email informado.";
                    }
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao recuperar senha do usuário: { e.Message}";
                }
            }
            return View();
        }


        //método para gravar o cookie de autenticação do usuário
        private void SignIn(Usuario usuario)
        {
            //criando um objeto da classe de modelo de dados
            var model = new AuthenticationModel
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataHoraAcesso = DateTime.Now
            };
            //serializando os dados da model para o formato json
            var json = JsonConvert.SerializeObject(model);
            //criando o conteudo que será gravado no cookie de autenticação
            //consiste na identificação do usuário
            var identity = new ClaimsIdentity(new[]
            {
             new Claim(ClaimTypes.Name, json)
             }, CookieAuthenticationDefaults.AuthenticationScheme);
            //gravar o cookie de autenticação do AspNet
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync
           (CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        //ROTA: /Account/Logout
        public IActionResult Logout()
        {
            SignOut(); //apagar a credencial do usuário
                       //redirecionar de volta para a página de login
            return RedirectToAction("Login", "Account");
        }

        //método para montar e enviar um email para o usuário
        private void EnviarEmailDeRecuperacaoDeSenha(Usuario usuario,
        string novaSenha)
        {
            var mailTo = usuario.Email;
            var subject = $"Recuperação de senha de acesso - Controle de Funcionários";
            var body = $@"
                           <div>
                           <p>Olá {usuario.Nome},</p>
                           <p>Utilize a senha <strong>{novaSenha}</strong>
                              para acessar sua conta.</p>
                           <p>Depois de acessar sua conta você poderá alterar
                              esta senha para outra de sua preferência.</p>
                           <p><br/>Att,<br/>Equipe Controle de Funcionários</p>
                          <p><small>Esta é uma mensagem automática, por favor
                                    não responda.</small></p>
                           </div>
                           ";
            var emailService = new EmailService();
            emailService.Send(mailTo, subject, body);
        }


    }
}
