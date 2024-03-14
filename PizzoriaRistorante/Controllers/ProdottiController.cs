using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PizzoriaRistorante.Models;

namespace PizzoriaRistorante.Controllers
{
    [Authorize]
    public class ProdottiController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Prodotti
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Prodotti.ToList());
        }

        // GET: Prodotti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // GET: Prodotti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prodotti/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Name,Image,Price,DeliveyTime,Ingredients")] Prodotti prodotti, HttpPostedFileBase Image)
        {
            if (Image != null && Image.ContentLength > 0)
            {
                string nomeFile = Path.GetFileName(Image.FileName);
                string pathToSave = Path.Combine(Server.MapPath("~/Content/Images/"), nomeFile);
                Image.SaveAs(pathToSave);
                prodotti.Image = "~/Content/Images/" + nomeFile;

            }
            if (ModelState.IsValid)
            {
                db.Prodotti.Add(prodotti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prodotti);
        }

        // GET: Prodotti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // POST: Prodotti/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ProductId,Name,Image,Price,DeliveyTime,Ingredients")] Prodotti prodotti, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    string nomeFile = Path.GetFileName(Image.FileName);
                    string pathToSave = Path.Combine(Server.MapPath("~/Content/Images/"), nomeFile);
                    Image.SaveAs(pathToSave);
                    prodotti.Image = "~/Content/Images/" + nomeFile;
                }

                db.Entry(prodotti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prodotti);
        }

        // GET: Prodotti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // POST: Prodotti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prodotti prodotti = db.Prodotti.Find(id);
            db.Prodotti.Remove(prodotti);
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

        // GET: Prodotti/Acquista/5
        public ActionResult Acquista(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }

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

            // Puoi ora utilizzare l'ID dell'utente corrente come desideri

            return View(prodotto);
        }


        [HttpPost]

        public ActionResult Acquista(int id)
        {
            // Trova il prodotto con l'ID specificato
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }
            string currentUsername = User.Identity.Name;

            // Trova l'utente con questo Username
            Utenti currentUser = db.Utenti.FirstOrDefault(u => u.Username == currentUsername);
            if (currentUser == null)
            {
                // Gestisci l'errore qui
            }

            // Ottieni l'ID dell'utente corrente
            int currentUserId = currentUser.UserId;
            // Ottieni l'ID dell'utente corrente


            // Crea un nuovo ordine per l'utente corrente
            Ordini ordine = new Ordini
            {
                UserId = currentUserId,
                OrderDate = DateTime.Now,
                Status = "In elaborazione",
                // Imposta gli altri campi dell'ordine come desideri
            };

            // Crea un nuovo DettaglioOrdini per il prodotto
            DettaglioOrdini dettaglioOrdini = new DettaglioOrdini
            {
                ProductId = prodotto.ProductId,
                Quantity = 1, // Imposta la quantità come desideri
            };

            // Aggiungi il DettaglioOrdini all'ordine
            ordine.DettaglioOrdini.Add(dettaglioOrdini);

            // Salva l'ordine nel database
            db.Ordini.Add(ordine);
            db.SaveChanges();

            // Reindirizza l'utente a una pagina di conferma
            return RedirectToAction("Conferma", new { id = ordine.OrderId });
        }

        // GET: Prodotti/AggiungiAlCarrello/5
        public ActionResult AggiungiAlCarrello(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }
            return View(prodotto);
        }


        [HttpPost]
        public ActionResult AggiungiAlCarrello(int id)
        {
            // Trova il prodotto con l'ID specificato
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }

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
            Ordini ordine = db.Ordini.FirstOrDefault(o => o.UserId == currentUserId && o.Status == "Non finalizzato");

            // Se non esiste un ordine non finalizzato, creane uno
            if (ordine == null)
            {
                ordine = new Ordini
                {
                    UserId = currentUserId,
                    OrderDate = DateTime.Now,
                    Status = "Non finalizzato",
                    // Imposta gli altri campi dell'ordine come desideri
                };
                db.Ordini.Add(ordine);
            }

            // Crea un nuovo DettaglioOrdini per il prodotto
            DettaglioOrdini dettaglioOrdini = new DettaglioOrdini
            {
                ProductId = prodotto.ProductId,
                Quantity = 1, // Imposta la quantità come desideri
            };

            // Aggiungi il DettaglioOrdini all'ordine
            ordine.DettaglioOrdini.Add(dettaglioOrdini);

            // Salva le modifiche nel database
            db.SaveChanges();

            // Reindirizza l'utente al carrello
            return RedirectToAction("Carrello", "Ordini");
        }


        // GET: Ordini/FinalizzaOrdine
        [HttpGet]
        public ActionResult FinalizzaOrdine(int? id)
        {
            // Ottieni l'ID dell'utente corrente
            string currentUserIdString = User.Identity.GetUserId();
            int currentUserId;
            if (!int.TryParse(currentUserIdString, out currentUserId))
            {
                // Gestisci l'errore di conversione qui
            }

            // Trova l'ordine non finalizzato per l'utente corrente
            Ordini ordine = db.Ordini.FirstOrDefault(o => o.UserId == currentUserId && o.Status == "Non finalizzato");
            if (ordine == null)
            {
                return HttpNotFound();
            }

            // Passa l'ordine alla vista per essere visualizzato
            return View(ordine);
        }




        // GET: Ordini/FinalizzaOrdine

        [HttpPost]
        public ActionResult FinalizzaOrdine(int id)
        {
            // Ottieni l'ID dell'utente corrente
            string currentUserIdString = User.Identity.GetUserId();
            int currentUserId;
            if (!int.TryParse(currentUserIdString, out currentUserId))
            {
                // Gestisci l'errore di conversione qui
            }

            // Trova l'ordine non finalizzato per l'utente corrente
            Ordini ordine = db.Ordini.FirstOrDefault(o => o.UserId == currentUserId && o.Status == "Non finalizzato");
            if (ordine == null)
            {
                return HttpNotFound();
            }

            // Finalizza l'ordine
            ordine.Status = "Finalizzato";

            // Salva le modifiche nel database
            db.SaveChanges();

            // Reindirizza l'utente a una pagina di conferma
            return RedirectToAction("Conferma", "Ordini", new { id = ordine.OrderId });
        }




    }
}
