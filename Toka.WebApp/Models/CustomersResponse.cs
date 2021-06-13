using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toka.WebApp.Models
{
    public class CustomersResponse
    {
        public CustomersResponse()
        {
            Paginacion = false;
            Data = new List<ReporteViewModel>();
        }
        public List<ReporteViewModel> Data { get; set; }

        public bool Paginacion { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas
        {
            get
            {
                if (RegistrosPorPagina > 0)
                    return (int)Math.Ceiling((decimal)TotalRegistros / (decimal)RegistrosPorPagina);
                return 0;
            }
        }

        public void Paginar()
        {
            if (Paginacion && Data.Count > 0)
            {
                Data = Data.Skip((PaginaActual - 1) * RegistrosPorPagina).Take(RegistrosPorPagina).ToList();
            }
        }
    }
}
