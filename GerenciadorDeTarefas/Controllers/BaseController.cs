using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    //[ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUsuarioRepository _usuarioRepository;
        public BaseController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        protected Usuario ReadToken()
        {
            var idUsuario = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(u => u.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(idUsuario))
            {
                var usuario = _usuarioRepository.GetById(int.Parse(idUsuario));
                return usuario;
            }

            throw new UnauthorizedAccessException("Token não informado ou inválido");
        }

    }

}
