using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PizzoriaRistorante.Models;

namespace PizzoriaRistorante.Controllers
{
    [Authorize]
    public class OrdiniController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordini
        public ActionResult Index()
        {

            return View(db.Ordini.ToList());
        }

        // GET: Ordini/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // GET: Ordini/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Utenti, "UserId", "Username");
            return View();
        }

        // POST: Ordini/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,UserId,OrderDate,ShippingAddress,Notes,Status")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Ordini.Add(ordini);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Utenti, "UserId", "Username", ordini.UserId);
            return View(ordini);
        }

        // GET: Ordini/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Utenti, "UserId", "Username", ordini.UserId);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,UserId,OrderDate,ShippingAddress,Notes,Status")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordini).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Utenti, "UserId", "Username", ordini.UserId);
            return View(ordini);
        }

        // GET: Ordini/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // POST: Ordini/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordini ordini = db.Ordini.Find(id);

            // Elimina o aggiorna gli oggetti DettaglioOrdini che fanno riferimento all'oggetto Ordini
            var dettaglioOrdini = db.DettaglioOrdini.Where(d => d.OrderId == id);
            foreach (var dettaglio in dettaglioOrdini)
            {
                db.DettaglioOrdini.Remove(dettaglio);
            }

            db.Ordini.Remove(ordini);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Carrello()
        {
            // Ottieni l'Username dell'utente corrente
            string currentUsername = User.Identity.Name;

            // Trova l'utente con questo Username
            Utenti currentUser = db.Utenti.FirstOrDefault(u => u.Username == currentUsername);
            if (currentUser == null)
            {
                // Gestisci l'errore qui
            }

            // Ottieni l'ID dell'utente corrente
            int currentUserId = currentUser.UserId;

            // Trova l'ordine non finalizzato per l'utente corrente
            Ordini ordine = db.Ordini.Include(o => o.DettaglioOrdini.Select(d => d.Prodotti))
                                     .FirstOrDefault(o => o.UserId == currentUserId && o.Status == "Non finalizzato");

            if (ordine == null)
            {
                // Se non esiste un ordine non finalizzato, restituisci una vista vuota
                return View(new List<DettaglioOrdini>());
            }

            // Restituisci la vista con i DettaglioOrdini dell'ordine
            return View(ordine.DettaglioOrdini.ToList());
        }


        [HttpGet]

        public ActionResult Conferma(int id)
        {
            // Trova l'ordine con l'ID specificato
            Ordini ordine = db.Ordini.Include(o => o.DettaglioOrdini.Select(d => d.Prodotti))
                                     .FirstOrDefault(o => o.OrderId == id);

            if (ordine == null)
            {
                return HttpNotFound();
            }

            // Restituisci la vista con l'ordine
            return View(ordine);
        }


    }
}
