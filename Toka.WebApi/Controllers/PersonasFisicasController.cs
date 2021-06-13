using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toka.Core.Models;
using Toka.DataAccess;
using Toka.DataAccess.Services;

namespace Toka.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonasFisicasController : ControllerBase
    {
        private readonly IPersonaFisicaService _personaFisicaService;

        public PersonasFisicasController(IPersonaFisicaService personaFisicaService)
        {
            _personaFisicaService = personaFisicaService;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonasFisicas>> Get()
        {
            return await _personaFisicaService.ObtenerListaPersonasAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var personaFisica = await _personaFisicaService.ObtenerPersonaPorIdAsync(id);

            if (personaFisica == null)
                return NotFound();

            return Ok(personaFisica);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PersonasFisicas model)
        {
            var resultado = await _personaFisicaService.AgregarPersonaFisicaAsync(model);

            if (resultado.EsError)
                return BadRequest(resultado);

            var personaUri = $"{Request.Scheme}://{Request.Host.Value}{Request.Path}/{resultado.Error}";
            var persona = await _personaFisicaService.ObtenerPersonaPorIdAsync(resultado.Error);
            return Created(personaUri, persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PersonasFisicas model)
        {
            if (model.IdPersonaFisica == 0)
                model.IdPersonaFisica = id;

            if (model.IdPersonaFisica != id)
                return BadRequest(new ActualizarPersonaFisicaResult { Error = -1, MensajeError = "Error al procesar la información" });


            var resultado = await _personaFisicaService.ActualizarPersonaFisicaAsync(model);

            if (resultado.EsError)
                return BadRequest(resultado);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var persona = await _personaFisicaService.ObtenerPersonaPorIdAsync(id);

            if (persona == null)
                return NotFound();


            var resultado = await _personaFisicaService.EliminarPersonaFisicaAsync(id);

            if (resultado.EsError)
                return BadRequest(resultado);

            return NoContent();
        }
    }
}
