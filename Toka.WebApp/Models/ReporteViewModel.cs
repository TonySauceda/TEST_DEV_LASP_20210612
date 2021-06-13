using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Toka.WebApp.Models
{
    public class ReporteViewModel
    {
        public int IdCliente { get; set; }
        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistroEmpresa { get; set; }
        [Display(Name = "Razón social")]
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public string Sucursal { get; set; }
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Apellido paterno")]
        public string Paterno { get; set; }
        [Display(Name = "Apellido materno")]
        public string Materno { get; set; }
        public int IdViaje { get; set; }
    }
}
