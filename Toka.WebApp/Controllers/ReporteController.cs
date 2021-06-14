using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Toka.WebApp.Configuration;
using Toka.WebApp.Models;
using Toka.WebApp.Utils;

namespace Toka.WebApp.Controllers
{
    [Authorize]
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
            CustomersResponse customerResponse = await ObtenerDataReporte();
            if (pagina.HasValue && pagina.Value > 0)
                Paginacion(customerResponse, pagina.Value);

            return PartialView(Constantes.VistaParcial.TablaReporte, customerResponse);
        }

        public async Task<IActionResult> DescargarReporte()
        {
            using (var workbook = new XLWorkbook())
            {
                var datos = await ObtenerDataReporte();
                var worksheet = workbook.Worksheets.Add("Reporte");
                var filaActual = 1;
                worksheet.Cell(filaActual, 1).Value = "Id Cliente";
                worksheet.Cell(filaActual, 2).Value = "Fecha de registro";
                worksheet.Cell(filaActual, 3).Value = "Razón social";
                worksheet.Cell(filaActual, 4).Value = "RFC";
                worksheet.Cell(filaActual, 5).Value = "Sucursal";
                worksheet.Cell(filaActual, 6).Value = "Id Empleado";
                worksheet.Cell(filaActual, 7).Value = "Nombre";
                worksheet.Cell(filaActual, 8).Value = "Apellido paterno";
                worksheet.Cell(filaActual, 9).Value = "Apellido materno";
                worksheet.Cell(filaActual, 10).Value = "Id Viaje";

                foreach (var data in datos.Data)
                {
                    filaActual++;
                    worksheet.Cell(filaActual, 1).Value = data.IdCliente;
                    worksheet.Cell(filaActual, 2).Value = data.FechaRegistroEmpresa;
                    worksheet.Cell(filaActual, 3).Value = data.RazonSocial;
                    worksheet.Cell(filaActual, 4).Value = data.RFC;
                    worksheet.Cell(filaActual, 5).Value = data.Sucursal;
                    worksheet.Cell(filaActual, 6).Value = data.IdEmpleado;
                    worksheet.Cell(filaActual, 7).Value = data.Nombre;
                    worksheet.Cell(filaActual, 8).Value = data.Paterno;
                    worksheet.Cell(filaActual, 9).Value = data.Materno;
                    worksheet.Cell(filaActual, 10).Value = data.IdViaje;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Reporte_{DateTime.Now:yyyyMMddhhmmss}.xlsx");
                }
            }
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
        public async Task<CustomersResponse> ObtenerDataReporte()
        {
            if (string.IsNullOrEmpty(Token))
                Token = await ObtenerToken();

            CustomersResponse customerResponse = new();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

                using (var response = await httpClient.GetAsync($"{Constantes.CandidatoApi.Customers}"))
                {
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        customerResponse = JsonConvert.DeserializeObject<CustomersResponse>(responseText);
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Token = "";
                        customerResponse = await ObtenerDataReporte();
                    }
                }
            }

            return customerResponse;
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
