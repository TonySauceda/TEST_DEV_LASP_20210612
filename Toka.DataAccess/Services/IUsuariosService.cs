using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;

namespace Toka.DataAccess.Services
{
    public interface IUsuariosService
    {
        Task<List<Usuarios>> ObtenerListaUsuariosAsync();
        Task<Usuarios> ObtenerUsuarioPorIdAsync(int idUsuairo);
        Task<Usuarios> ObtenerUsuarioPorUsuarioAsync(string usuairo);
        Task<bool> RegistrarUsuairoAsync(Usuarios usuario);
        Task<bool> ActualizarUsuarioAsync(Usuarios usuario);
        Task<bool> EliminarUsuarioAsync(int idUsuario);
    }
}
