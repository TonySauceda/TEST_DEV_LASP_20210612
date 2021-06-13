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
            //public const string TokaCandidatoApi = "https://api.toka.com.mx/candidato/api";
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
        }
    }
}
