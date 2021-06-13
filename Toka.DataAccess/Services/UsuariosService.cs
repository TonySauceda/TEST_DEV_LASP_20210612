using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;

namespace Toka.DataAccess.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly TokaContext _tokaContext;

        public UsuariosService(TokaContext tokaContext)
        {
            _tokaContext = tokaContext;
        }
        public async Task<bool> ActualizarUsuarioAsync(Usuarios usuario)
        {
            _tokaContext.Usuarios.Update(usuario);
            var actualizado = await _tokaContext.SaveChangesAsync();
            return actualizado > 0;
        }

        public async Task<bool> EliminarUsuarioAsync(int idUsuario)
        {
            var usuario = await ObtenerUsuarioPorIdAsync(idUsuario);

            if (usuario == null)
                return false;

            usuario.Activo = false;
            _tokaContext.Usuarios.Update(usuario);
            var eliminado = await _tokaContext.SaveChangesAsync();
            return eliminado > 0;
        }

        public async Task<List<Usuarios>> ObtenerListaUsuariosAsync()
        {
            return await _tokaContext.Usuarios.Where(x => x.Activo).ToListAsync();
        }

        public async Task<Usuarios> ObtenerUsuarioPorIdAsync(int idUsuairo)
        {
            return await _tokaContext.Usuarios.Where(x => x.Activo && x.IdUsuario == idUsuairo).SingleOrDefaultAsync();
        }

        public async Task<Usuarios> ObtenerUsuarioPorUsuarioAsync(string usuairo)
        {
            return await _tokaContext.Usuarios.Where(x => x.Activo && x.Usuario == usuairo).SingleOrDefaultAsync();
        }

        public async Task<bool> RegistrarUsuairoAsync(Usuarios usuario)
        {
            usuario.IdUsuario = 0;
            usuario.Activo = true;
            _tokaContext.Usuarios.Add(usuario);
            var registrado = await _tokaContext.SaveChangesAsync();

            return registrado > 0;
        }
    }
}
