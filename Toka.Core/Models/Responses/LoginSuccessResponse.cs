using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toka.Core.Models.Responses
{
    public class LoginSuccessResponse
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Token { get; set; }
    }
}
