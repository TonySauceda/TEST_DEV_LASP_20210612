using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Toka.WebApp.Configuration;
using Toka.WebApp.Models;
using Toka.WebApp.Utils;

namespace Toka.WebApp.Controllers
{
    public class ReporteController : Controller
    {
        public static string Token = "";
        private readonly CandidatoApiSettings _candidatoApiSettings;

        public ReporteController(CandidatoApiSettings candidatoApiSettings)
        {
            _candidatoApiSettings = candidatoApiSettings;
        }

        public async Task<IActionResult> Index()
        {
            Token = await ObtenerToken();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte([FromQuery] int? pagina)
        {
            CustomersResponse customerResponse = new();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

                using (var response = await httpClient.GetAsync($"{Constantes.CandidatoApi.Customers}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        customerResponse = JsonConvert.DeserializeObject<CustomersResponse>(responseText);
                        if (pagina.HasValue && pagina.Value > 0)
                            Paginacion(customerResponse, pagina.Value);
                    }
                }
            }
            return PartialView(Constantes.VistaParcial.TablaReporte, customerResponse);
        }

        [NonAction]
        public async Task<string> ObtenerToken()
        {
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Constantes.CandidatoApi.Authenticate));
                requestMessage.Content = new JsonContent(_candidatoApiSettings);

                var response = await httpClient.SendAsync(requestMessage);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    var tokenRequest = JsonConvert.DeserializeObject<TokenRequest>(responseText);

                    return tokenRequest.Token;
                }
            }

            return "";
        }

        [NonAction]
        public void Paginacion(CustomersResponse customersResponse, int pagina)
        {
            customersResponse.Paginacion = true;
            customersResponse.TotalRegistros = customersResponse.Data.Count;
            customersResponse.RegistrosPorPagina = Constantes.Paginacion.NumeroRegistros;
            customersResponse.PaginaActual = pagina;
            customersResponse.Paginar();
        }
    }
}
