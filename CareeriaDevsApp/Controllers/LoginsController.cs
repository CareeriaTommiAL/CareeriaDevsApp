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
    public class LoginsController : Controller
    {
        private Stud1Entities db = new Stud1Entities();

        // GET: Logins
        public ActionResult Index()
        {
            var login = db.Login.Include(l => l.Opiskelija).Include(l => l.Yritys).Include(l => l.PaaKayttaja);
            return View(login.ToList());
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi");
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi");
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "login_Id,kayttajaNimi,salasana,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Login.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", login.opiskelija_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", login.yritys_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", login.paaKayttaja_Id);
            return View(login);
        }

        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", login.opiskelija_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", login.yritys_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", login.paaKayttaja_Id);
            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "login_Id,kayttajaNimi,salasana,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", login.opiskelija_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", login.yritys_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", login.paaKayttaja_Id);
            return View(login);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Login login = db.Login.Find(id);
            db.Login.Remove(login);
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
