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

namespace Toka.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasFisicasController : ControllerBase
    {
        private readonly TokaContext _context;

        public PersonasFisicasController(TokaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<PersonasFisicas> Get()
        {
            return _context.PersonasFisicas.Where(x => x.Activo).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var personaFisica = _context.PersonasFisicas.Where(x => x.Activo && x.IdPersonaFisica == id).FirstOrDefault();

            if (personaFisica == null)
                return NotFound();

            return Ok(personaFisica);
        }

        [HttpPost]
        public IActionResult Post(PersonasFisicas model)
        {
            var parms = ObtenerParametros(model);

            string query = "EXEC dbo.sp_AgregarPersonaFisica @Nombre,@ApellidoPaterno,@ApellidoMaterno,@RFC,@FechaNacimiento,@UsuarioAgrega";
            var resultado = _context.Resultado.FromSqlRaw(query, parms).ToList().First();

            if (resultado.EsError)
                return BadRequest(resultado);

            var personaUri = $"{Request.Scheme}://{Request.Host.Value}{Request.Path}/{resultado.Error}";
            var persona = _context.PersonasFisicas.Find(resultado.Error);
            return Created(personaUri, persona);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, PersonasFisicas model)
        {
            if (model.IdPersonaFisica == 0)
                model.IdPersonaFisica = id;

            if (model.IdPersonaFisica != id)
                return BadRequest(new Resultado() { Error = -1, MensajeError = "Error al procesar la información" });

            var parms = ObtenerParametros(model, true);
            string query = "EXEC dbo.sp_ActualizarPersonaFisica @IdPersonaFisica,@Nombre,@ApellidoPaterno,@ApellidoMaterno,@RFC,@FechaNacimiento,@UsuarioAgrega";
            var resultado = _context.Resultado.FromSqlRaw(query, parms).ToList().First();

            if (resultado.EsError)
                return BadRequest(resultado);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var persona = _context.PersonasFisicas.Find(id);

            if (persona == null)
                return NotFound();

            string query = "EXEC dbo.sp_EliminarPersonaFisica @IdPersonaFisica";
            var parms = new SqlParameter[] { new SqlParameter { ParameterName = "@IdPersonaFisica", Value = id } };
            var resultado = _context.Resultado.FromSqlRaw(query, parms).ToList().First();

            if (resultado.EsError)
                return BadRequest(resultado);

            return NoContent();
        }

        [NonAction]
        private SqlParameter[] ObtenerParametros(PersonasFisicas model, bool modificar = false)
        {
            List<SqlParameter> resultado = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Nombre", Value = model.Nombre },
                    new SqlParameter { ParameterName = "@ApellidoPaterno", Value = model.ApellidoPaterno },
                    new SqlParameter { ParameterName = "@ApellidoMaterno", Value = model.ApellidoMaterno },
                    new SqlParameter { ParameterName = "@RFC", Value = model.RFC },
                    new SqlParameter { ParameterName = "@FechaNacimiento", Value = model.FechaNacimiento },
                    new SqlParameter { ParameterName = "@UsuarioAgrega", Value = model.UsuarioAgrega }
                };

            if (modificar)
                resultado.Add(new SqlParameter { ParameterName = "@IdPersonaFisica", Value = model.IdPersonaFisica });

            return resultado.ToArray();
        }
    }
}
