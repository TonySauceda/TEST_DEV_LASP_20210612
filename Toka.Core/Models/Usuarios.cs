using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Toka.Core.Models
{
    [Table("Tb_Usuarios")]
    public class Usuarios
    {
        [key]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El usuario es requerido")]
        [StringLength(50)]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(250)]
        public string Contraseña { get; set; }
        public bool Activo { get; set; }
    }
}