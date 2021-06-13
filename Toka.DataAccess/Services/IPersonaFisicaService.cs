using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;

namespace Toka.DataAccess.Services
{
    public interface IPersonaFisicaService
    {
        Task<List<PersonasFisicas>> ObtenerListaPersonasAsync();
        Task<PersonasFisicas> ObtenerPersonaPorIdAsync(int idPersonaFisica);
        Task<AgregarPersonaFisicaResult> AgregarPersonaFisicaAsync(PersonasFisicas personaFisica);
        Task<ActualizarPersonaFisicaResult> ActualizarPersonaFisicaAsync(PersonasFisicas personaFisica);
        Task<EliminarPersonaFisicaResult> EliminarPersonaFisicaAsync(int idPersonaFisica);
    }
}
