using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Toka.WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Ingrese el usuario")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Ingrese la usuario")]
        public string Contraseña { get; set; }
    }
}