using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;
using Toka.Core.Models.Responses;
using Toka.DataAccess.Services;
using Toka.WebApi.Configuration;

namespace Toka.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        private readonly JwtSettings _jwtSettings;

        public UsuarioController(IUsuariosService usuariosService, JwtSettings jwtSettings)
        {
            _usuariosService = usuariosService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(Usuarios model)
        {
            var existeUsuario = await _usuariosService.ObtenerUsuarioPorUsuarioAsync(model.Usuario);

            if (existeUsuario != null)
                return BadRequest(new { Mensaje = "El nombre de usuario ya existe." });

            model.Contraseña = BCrypt.Net.BCrypt.HashPassword(model.Contraseña);

            var resultado = await _usuariosService.RegistrarUsuairoAsync(model);

            if (!resultado)
                return BadRequest();

            var usuario = await _usuariosService.ObtenerUsuarioPorUsuarioAsync(model.Usuario);
            var usuarioUri = $"{Request.Scheme}://{Request.Host.Value}{Request.Path}/{usuario.IdUsuario}";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Id", usuario.IdUsuario.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Created(usuarioUri, new { usuario.IdUsuario, usuario.Usuario, token = tokenHandler.WriteToken(token) });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Usuarios model)
        {
            var usuario = await _usuariosService.ObtenerUsuarioPorUsuarioAsync(model.Usuario);

            if (usuario == null)
                return BadRequest(new ErrorResponse { Mensaje = "No existe el usuario" });

            var contraseñaValida = BCrypt.Net.BCrypt.Verify(model.Contraseña, usuario.Contraseña);

            if (!contraseñaValida)
                return BadRequest(new ErrorResponse { Mensaje = "Contraseña inválida" });


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Id", usuario.IdUsuario.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var respuesta = new LoginSuccessResponse
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Token = tokenHandler.WriteToken(token)
            };

            return Ok(respuesta);
        }
    }
}
