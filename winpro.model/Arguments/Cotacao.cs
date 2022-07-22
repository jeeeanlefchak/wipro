using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winpro.model.Arguments
{
    public class Cotacao
    {
        public double Valor { get; set; }
        public string Codigo { get; set; }
        public DateTime? Date { get; set; }

        public string IdMoeda { get; set; }
    }
}
