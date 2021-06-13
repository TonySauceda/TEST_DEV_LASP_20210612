using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;

namespace Toka.DataAccess.Services
{
    public class PersonaFisicaService : IPersonaFisicaService
    {
        private readonly TokaContext _tokaContext;

        public PersonaFisicaService(TokaContext tokaContext)
        {
            _tokaContext = tokaContext;
        }

        public async Task<AgregarPersonaFisicaResult> AgregarPersonaFisicaAsync(PersonasFisicas personaFisica)
        {
            var parms = ObtenerParametros(personaFisica);

            string query = "EXEC dbo.sp_AgregarPersonaFisica @Nombre,@ApellidoPaterno,@ApellidoMaterno,@RFC,@FechaNacimiento,@UsuarioAgrega";
            var resultado = await _tokaContext.AgregarPersonaFisicaResult.FromSqlRaw(query, parms).ToListAsync();
            return resultado.First();
        }

        public async Task<ActualizarPersonaFisicaResult> ActualizarPersonaFisicaAsync(PersonasFisicas personaFisica)
        {
            var parms = ObtenerParametros(personaFisica, true);
            string query = "EXEC dbo.sp_ActualizarPersonaFisica @IdPersonaFisica,@Nombre,@ApellidoPaterno,@ApellidoMaterno,@RFC,@FechaNacimiento,@UsuarioAgrega";
            var resultado = await _tokaContext.ActualizarPersonaFisicaResult.FromSqlRaw(query, parms).ToListAsync();
            return resultado.First();
        }

        public async Task<EliminarPersonaFisicaResult> EliminarPersonaFisicaAsync(int idPersonaFisica)
        {
            string query = "EXEC dbo.sp_EliminarPersonaFisica @IdPersonaFisica";
            var parms = new SqlParameter[] { new SqlParameter { ParameterName = "@IdPersonaFisica", Value = idPersonaFisica } };

            var resultado = await _tokaContext.EliminarPersonaFisicaResult.FromSqlRaw(query, parms).ToListAsync();
            return resultado.First();
        }

        public async Task<List<PersonasFisicas>> ObtenerListaPersonasAsync()
        {
            return await _tokaContext.PersonasFisicas.Where(x => x.Activo).ToListAsync();
        }

        public async Task<PersonasFisicas> ObtenerPersonaPorIdAsync(int idPersonaFisica)
        {
            return await _tokaContext.PersonasFisicas.Where(x => x.Activo && x.IdPersonaFisica == idPersonaFisica).SingleOrDefaultAsync();
        }
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
