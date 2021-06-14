using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Toka.Core.Models.Responses;
using Toka.WebApp.Models;
using Toka.WebApp.Utils;

namespace Toka.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(LoginViewModel model)
        {
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync($"{Constantes.TokaWebApi.Login}", new JsonContent(model)))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        var error = JsonConvert.DeserializeObject<ErrorResponse>(responseText);
                        ViewBag.Mensaje = error.Mensaje;
                        return View(model);
                    }

                    var respuesta = JsonConvert.DeserializeObject<LoginSuccessResponse>(responseText);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,respuesta.Usuario),
                        new Claim("Id",respuesta.IdUsuario.ToString()),
                        new Claim("Token",respuesta.Token)
                    };

                    var identity = new ClaimsIdentity(claims, Constantes.Identity.TokaAuth);
                    ClaimsPrincipal claimsPrincipal = new(identity);

                    await HttpContext.SignInAsync(Constantes.Identity.TokaAuth, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Constantes.Identity.TokaAuth);

            return RedirectToAction("Index", "Home");
        }
    }
}
