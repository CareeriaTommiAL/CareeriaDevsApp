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
    public class OpiskelijasController : Controller
    {
        private Stud1Entities db = new Stud1Entities();

        // GET: Opiskelijas
        public ActionResult Index()
        {
            var opiskelija = db.Opiskelija.Include(o => o.Postitoimipaikka);
            return View(opiskelija.ToList());
        }

        // GET: Opiskelijas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opiskelija opiskelija = db.Opiskelija.Find(id);
            if (opiskelija == null)
            {
                return HttpNotFound();
            }
            return View(opiskelija);
        }

        // GET: Opiskelijas/Create
        public ActionResult Create()
        {
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero");
            return View();
        }

        // POST: Opiskelijas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "opiskelija_Id,etunimi,sukunimi,postitoimipaikka_Id")] Opiskelija opiskelija)
        {
            if (ModelState.IsValid)
            {
                db.Opiskelija.Add(opiskelija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero", opiskelija.postitoimipaikka_Id);
            return View(opiskelija);
        }

        // GET: Opiskelijas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opiskelija opiskelija = db.Opiskelija.Find(id);
            if (opiskelija == null)
            {
                return HttpNotFound();
            }
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero", opiskelija.postitoimipaikka_Id);
            return View(opiskelija);
        }

        // POST: Opiskelijas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "opiskelija_Id,etunimi,sukunimi,postitoimipaikka_Id")] Opiskelija opiskelija)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opiskelija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero", opiskelija.postitoimipaikka_Id);
            return View(opiskelija);
        }

        // GET: Opiskelijas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opiskelija opiskelija = db.Opiskelija.Find(id);
            if (opiskelija == null)
            {
                return HttpNotFound();
            }
            return View(opiskelija);
        }

        // POST: Opiskelijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opiskelija opiskelija = db.Opiskelija.Find(id);
            db.Opiskelija.Remove(opiskelija);
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
