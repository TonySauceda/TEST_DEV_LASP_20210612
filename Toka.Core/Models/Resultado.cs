using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toka.Core.Models
{
    public class Resultado
    {
        public int Error { get; set; }
        public string MensajeError { get; set; }

        public bool EsError
        {
            get
            {
                return Error < 0;
            }
        }
    }
}
