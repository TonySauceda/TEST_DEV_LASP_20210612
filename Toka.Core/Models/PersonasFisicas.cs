using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.ValidationAttributes;

namespace Toka.Core.Models
{
    [Table("Tb_PersonasFisicas")]
    public class PersonasFisicas
    {
        [Key]
        [Display(Name = "Id")]
        public int IdPersonaFisica { get; set; }
        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }
        [Display(Name = "Fecha de actualización")]
        public DateTime? FechaActualizacion { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede ser mayor a 50 caracteres")]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "El apellido paterno no puede ser mayor a 50 caracteres")]
        [Display(Name = "Apellido paterno")]
        public string ApellidoPaterno { get; set; }
        [StringLength(50, ErrorMessage = "El apellido materno no puede ser mayor a 50 caracteres")]
        [Display(Name = "Apellido materno")]
        public string ApellidoMaterno { get; set; }
        [Required(ErrorMessage = "El RFC es requerido")]
        [ValdarRFC]
        public string RFC { get; set; }
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime FechaNacimiento { get; set; }
        public int? UsuarioAgrega { get; set; }
        public bool Activo { get; set; }

        public bool ValidarRFC()
        {
            return (RFC ?? "").Length == 13;
        }
    }
}
