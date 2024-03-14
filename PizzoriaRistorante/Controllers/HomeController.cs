using PizzoriaRistorante.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzoriaRistorante.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult BackOffice()
        {
            ViewBag.Title = "Back Office";

            return View();
        }

        public ActionResult TotaleOrdiniEvasi()
        {
            // Ottieni tutti gli ordini con lo stato "Finalizzato"
            var ordiniFinalizzati = db.Ordini.Where(o => o.Status == "evaso");

            // Calcola il totale degli ordini finalizzati
            int totale = ordiniFinalizzati.Count();

            // Restituisci il totale
            return View(totale); // Assicurati di avere una vista che può visualizzare un singolo numero intero
        }
    }
}
