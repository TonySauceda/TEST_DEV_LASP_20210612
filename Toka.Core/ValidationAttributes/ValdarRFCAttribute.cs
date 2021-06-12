using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;

namespace Toka.Core.ValidationAttributes
{
    public class ValdarRFCAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var personaFisica = validationContext.ObjectInstance as PersonasFisicas;
            if (personaFisica != null)
            {
                return personaFisica.ValidarRFC() ? ValidationResult.Success : new ValidationResult("El RFC no es válido.");
            }

            return ValidationResult.Success;
        }
    }
}
