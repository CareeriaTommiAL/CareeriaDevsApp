using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareeriaDevsApp;
using CareeriaDevsApp.Models;

namespace CareeriaDevsApp.Controllers
{
    public class OmaSisaltosController : Controller
    {
        private Stud1Entities db = new Stud1Entities();

        //index joka pitää näkyä vain pääkäyttäjälle
        //*******************************************************
        public ActionResult Index()
        {
            var opislogid = Session["student_id"];
            if (opislogid != null)
            {
                return RedirectToAction("OpisSisalto", "OmaSisaltos");
            }
            var yrityslogid = Session["corporate_id"];
            if (yrityslogid != null)
            {
                return RedirectToAction("YritysSisalto", "OmaSisaltos");
            }

            var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            return View(omaSisalto.ToList());
        }

        //Haetaan tietty opiskelija_id
        //*******************************************************
        public ActionResult OpisSisalto()
        {
            var opislogid = Session["student_id"];
            var opisTeksti = (from m in db.OmaSisalto
                              where m.opiskelija_Id.ToString() == opislogid.ToString()
                              select m);

            if (opislogid == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            return View(opisTeksti.ToList());
        }

        //haetaan tietty yritys_id
        //**********************************************************
        public ActionResult YritysSisalto()
        {
            var yrityslogid = Session["corporate_id"];
            if (yrityslogid == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            return View(omaSisalto.ToList());
        }

        // GET: OmaSisaltos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OmaSisalto omaSisalto = db.OmaSisalto.Find(id);
            if (omaSisalto == null)
            {
                return HttpNotFound();
            }
            return View(omaSisalto);
        }

        // GET: OmaSisaltos/Create
        public ActionResult Create()
        {
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi");
            return View();
        }

        // POST: OmaSisaltos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "omaSisalto_Id,omatAsetukset,omaKuva,omaTeksti,opiskelija_Id")] OmaSisalto omaSisalto)
        {
            if (ModelState.IsValid)
            {
                db.OmaSisalto.Add(omaSisalto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", omaSisalto.opiskelija_Id);
            return View(omaSisalto);
        }

        // GET: OmaSisaltos/Edit/5
        public ActionResult Edit(int? id)
        {
            //käyttäjä ei pääse muokkaamaan omasisältöä, jos loginissa saatu Session["student_id"] ei vastaa edittiin menossa olevaa opiskelija_id:tä
            if (Session["student_id"].ToString() != id.ToString())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OmaSisalto omaSisalto = db.OmaSisalto.Find(id);
            if (omaSisalto == null)
            {
                return HttpNotFound();
            }
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", omaSisalto.opiskelija_Id);

            return View(omaSisalto);



        }

        // POST: OmaSisaltos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "omaSisalto_Id,omatAsetukset,omaKuva,omaTeksti,opiskelija_Id")] OmaSisalto omaSisalto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(omaSisalto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", omaSisalto.opiskelija_Id);
            return View(omaSisalto);
        }

        // GET: OmaSisaltos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OmaSisalto omaSisalto = db.OmaSisalto.Find(id);
            if (omaSisalto == null)
            {
                return HttpNotFound();
            }
            return View(omaSisalto);
        }

        // POST: OmaSisaltos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OmaSisalto omaSisalto = db.OmaSisalto.Find(id);
            db.OmaSisalto.Remove(omaSisalto);
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
