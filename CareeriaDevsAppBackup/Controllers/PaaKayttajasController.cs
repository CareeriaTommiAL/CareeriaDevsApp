using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareeriaDevsApp;

namespace CareeriaDevsApp.Controllers
{
    public class PaaKayttajasController : Controller
    {
        private Stud1Entities db = new Stud1Entities();

        // GET: PaaKayttajas
        public ActionResult Index()
        {
            return View(db.PaaKayttaja.ToList());
        }

        // GET: PaaKayttajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaaKayttaja paaKayttaja = db.PaaKayttaja.Find(id);
            if (paaKayttaja == null)
            {
                return HttpNotFound();
            }
            return View(paaKayttaja);
        }

        // GET: PaaKayttajas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaaKayttajas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "paaKayttaja_Id,nimi")] PaaKayttaja paaKayttaja)
        {
            if (ModelState.IsValid)
            {
                db.PaaKayttaja.Add(paaKayttaja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paaKayttaja);
        }

        // GET: PaaKayttajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaaKayttaja paaKayttaja = db.PaaKayttaja.Find(id);
            if (paaKayttaja == null)
            {
                return HttpNotFound();
            }
            return View(paaKayttaja);
        }

        // POST: PaaKayttajas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "paaKayttaja_Id,nimi")] PaaKayttaja paaKayttaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paaKayttaja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paaKayttaja);
        }

        // GET: PaaKayttajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaaKayttaja paaKayttaja = db.PaaKayttaja.Find(id);
            if (paaKayttaja == null)
            {
                return HttpNotFound();
            }
            return View(paaKayttaja);
        }

        // POST: PaaKayttajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaaKayttaja paaKayttaja = db.PaaKayttaja.Find(id);
            db.PaaKayttaja.Remove(paaKayttaja);
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
