using GerenciadorDeTarefas.Dtos;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository;
using GerenciadorDeTarefas.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
      
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;          
        }

        [HttpPost]
        public IActionResult SalvarUsuario([FromBody]Usuario usuario)
        {
            try
            {
                var erros = new List<string>();
                if(string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome) || usuario.Nome.Length < 3)
                {
                    erros.Add("Nome inválido");
                }

                var obrigatorios = new List<string>() { "@", "!", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrWhiteSpace(usuario.Senha) || usuario.Senha.Length < 4 && obrigatorios.Any(e => usuario.Senha.Contains(e)))
                {
                    erros.Add("Senha inválida");
                }

                Regex regex = new Regex(@"^([\w\.\-\+\d]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if(string.IsNullOrEmpty(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Email) || regex.Match(usuario.Email).Success)
                {
                    erros.Add("Email inválido");
                }

                if (_usuarioRepository.ExisteUsuarioPorEmail(usuario.Email))
                {
                    erros.Add("Já existe uma conta com o email informado");
                }

                if(erros.Count > 0)
                {
                    return BadRequest(new ErroRespostaDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erros = erros
                    });
                }

                usuario.Email = usuario.Email.ToLower();
                usuario.Senha = MDUtils.GerarHashMD5(usuario.Senha);
                _usuarioRepository.Salvar(usuario);
                return Ok(new { msg= "Usuario criado com sucesso" });
            }
            catch(Exception e)
            {
                _logger.LogError("Ocorreu erro ao obter usuario", e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErroRespostaDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = "Ocorreu erro ao salvar usuario, tente novamente"
                });
            }
        }
    }
}
