using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzoriaRistorante.Models
{
    public class OrdiniDettaglioViewModel
    {
        public PizzoriaRistorante.Models.Ordini Ordini { get; set; }
        public PizzoriaRistorante.Models.DettaglioOrdini DettaglioOrdini { get; set; }
    }
}