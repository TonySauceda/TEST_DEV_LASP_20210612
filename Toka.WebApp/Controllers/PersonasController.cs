using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Toka.Core.Models;
using Toka.WebApp.Utils;

namespace Toka.WebApp.Controllers
{
    public class PersonasController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(PersonasFisicas modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            modelo.UsuarioAgrega = 0;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync($"{Constantes.TokaWebApi.PersonasFisicas}", new JsonContent(modelo)))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.Created)
                    {
                        var error = JsonConvert.DeserializeObject<AgregarPersonaFisicaResult>(responseText);
                        ViewBag.Mensaje = error.MensajeError;
                        return View(modelo);
                    }

                    var persona = JsonConvert.DeserializeObject<PersonasFisicas>(responseText);

                    return RedirectToAction("Detalle", new { Id = persona.IdPersonaFisica });
                }
            }
        }

        public async Task<IActionResult> Detalle(int id)
        {
            PersonasFisicas persona = null;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Constantes.TokaWebApi.PersonasFisicas}/{id}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        persona = JsonConvert.DeserializeObject<PersonasFisicas>(responseText);
                }
            }

            return View(persona);
        }

        public async Task<IActionResult> Editar(int id)
        {
            PersonasFisicas persona = null;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Constantes.TokaWebApi.PersonasFisicas}/{id}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        persona = JsonConvert.DeserializeObject<PersonasFisicas>(responseText);
                }
            }

            return View(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PersonasFisicas modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PutAsync($"{Constantes.TokaWebApi.PersonasFisicas}/{modelo.IdPersonaFisica}", new JsonContent(modelo)))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        var error = JsonConvert.DeserializeObject<ActualizarPersonaFisicaResult>(responseText);
                        ViewBag.Mensaje = error.MensajeError;
                        return View(modelo);
                    }
                }
            }

            TempData["Mensaje"] = "Se editó el registro con éxito";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            PersonasFisicas persona = null;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Constantes.TokaWebApi.PersonasFisicas}/{id}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        persona = JsonConvert.DeserializeObject<PersonasFisicas>(responseText);
                }
            }

            return View(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(PersonasFisicas modelo)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{Constantes.TokaWebApi.PersonasFisicas}/{modelo.IdPersonaFisica}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        var error = JsonConvert.DeserializeObject<EliminarPersonaFisicaResult>(responseText);
                        ViewBag.Mensaje = error.MensajeError;
                        return View(modelo);
                    }
                }
            }

            TempData["Mensaje"] = "Se eliminó el registro con éxito";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPersonas()
        {
            List<PersonasFisicas> personasList = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Constantes.TokaWebApi.PersonasFisicas}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        personasList = JsonConvert.DeserializeObject<List<PersonasFisicas>>(responseText);
                }
            }
            return PartialView(Constantes.VistaParcial.TablaPersonas, personasList);
        }
    }
}
