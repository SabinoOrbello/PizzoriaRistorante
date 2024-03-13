using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzoriaRistorante.Models
{
    public class OrdiniViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        // Questa lista conterrà gli ID dei prodotti che l'utente vuole ordinare
        public List<int> ProductIds { get; set; }
    }
}