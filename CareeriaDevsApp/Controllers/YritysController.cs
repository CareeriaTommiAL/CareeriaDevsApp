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
    public class YritysController : Controller
    {
        private Stud1Entities db = new Stud1Entities();

        // GET: Yritys
        public ActionResult Index()
        {
            var yritys = db.Yritys.Include(y => y.Postitoimipaikka);
            return View(yritys.ToList());
        }

        // GET: Yritys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yritys yritys = db.Yritys.Find(id);
            if (yritys == null)
            {
                return HttpNotFound();
            }
            return View(yritys);
        }

        // GET: Yritys/Create
        public ActionResult Create()
        {
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero");
            return View();
        }

        // POST: Yritys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "yritys_Id,yrityksenNimi,Y_tunnus,lahiosoite,postitoimipaikka_Id")] Yritys yritys)
        {
            if (ModelState.IsValid)
            {
                db.Yritys.Add(yritys);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero", yritys.postitoimipaikka_Id);
            return View(yritys);
        }

        // GET: Yritys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yritys yritys = db.Yritys.Find(id);
            if (yritys == null)
            {
                return HttpNotFound();
            }
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero", yritys.postitoimipaikka_Id);
            return View(yritys);
        }

        // POST: Yritys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "yritys_Id,yrityksenNimi,Y_tunnus,lahiosoite,postitoimipaikka_Id")] Yritys yritys)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yritys).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero", yritys.postitoimipaikka_Id);
            return View(yritys);
        }

        // GET: Yritys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yritys yritys = db.Yritys.Find(id);
            if (yritys == null)
            {
                return HttpNotFound();
            }
            return View(yritys);
        }

        // POST: Yritys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yritys yritys = db.Yritys.Find(id);
            db.Yritys.Remove(yritys);
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
