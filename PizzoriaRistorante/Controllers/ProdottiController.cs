﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
    }
}
