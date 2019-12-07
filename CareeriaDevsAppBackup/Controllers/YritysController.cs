using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CareeriaDevsApp;
using CareeriaDevsApp.Filters;

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
        [PreventFromUrl]
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
        [PreventFromUrl]
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
            var poistettavaYritys = from l in db.Login where l.yritys_Id == id select l; //Hakee login taulusta poistettavan yrityksen loginin
            var yrityksenPuhelin = from p in db.PuhelinNumero where p.yritys_Id == id select p;
            var yrityksenSahkoposti = from s in db.Sahkoposti where s.yritys_Id == id select s;
            var yrityksenViestit = from m in db.Viesti where m.yritys_Id == id select m;

            foreach (var l in poistettavaYritys)
            {
                db.Login.Remove(l);  //Poistaa yrityksen rivin login talulusta yritys_id -perusteella
            }

            foreach (var p in yrityksenPuhelin)
            {
                db.PuhelinNumero.Remove(p);  
            }
            foreach (var s in yrityksenSahkoposti)
            {
                db.Sahkoposti.Remove(s);
            }
            foreach (var m in yrityksenViestit)
            {
                db.Viesti.Remove(m);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
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
