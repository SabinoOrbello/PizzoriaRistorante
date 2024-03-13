using PizzoriaRistorante.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PizzoriaRistorante.Controllers
{
    public class AccountController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,Email")] Utenti model)
        {
            if (ModelState.IsValid)
            {
                // Verifica se l'utente esiste già
                var existingUser = db.Utenti.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username already in use");
                    return View(model);
                }

                // Crea un nuovo utente
                db.Utenti.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index", "Prodotti");
            }

            //  significa che qualcosa non ha funzionato, quindi visualizza nuovamente il modulo
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Utenti model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Utenti.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // utente autenticato
                    Session["UserId"] = user.UserId; // Salva l'UserId nella sessione

                    return RedirectToAction("Create", "Ordini");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("UserId"); // Rimuove l'UserId dalla sessione
            return RedirectToAction("Login", "Account");
        }





    }


}