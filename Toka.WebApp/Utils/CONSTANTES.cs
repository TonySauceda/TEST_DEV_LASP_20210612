using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toka.WebApp.Utils
{
    public class Constantes
    {
        public class TokaWebApi
        {
            public const string Base = "https://localhost:44397/api";
            public static readonly string PersonasFisicas = $"{Base}/PersonasFisicas";
            public static readonly string Registrar = $"{Base}/Usuario/Registrar";
            public static readonly string Login = $"{Base}/Usuario/Login";
        }
        public class CandidatoApi
        {
            public const string Base = "https://api.toka.com.mx/candidato/api";
            public static readonly string Authenticate = $"{Base}/login/authenticate";
            public static readonly string Customers = $"{Base}/customers";
        }

        public class Paginacion
        {
            public const int NumeroRegistros = 20;
        }

        public class VistaParcial
        {
            public const string TablaPersonas = "~/Views/Shared/_TablaPersonasPartial.cshtml";
            public const string TablaReporte = "~/Views/Shared/_TablaReportePartial.cshtml";
            public const string LoginStatus = "~/Views/Shared/_LoginStatusPartial.cshtml";
        }

        public class Identity
        {
            public const string TokaAuth = "TokaAuth";
        }
    }
}
