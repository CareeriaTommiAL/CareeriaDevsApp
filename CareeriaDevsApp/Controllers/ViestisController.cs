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
    public class ViestisController : Controller
    {
        private Stud1Entities db = new Stud1Entities();

        // GET: Viestis
        public ActionResult Index()
        {
            var viesti = db.Viesti.Include(v => v.Opiskelija).Include(v => v.PaaKayttaja).Include(v => v.Yritys);
            return View(viesti.ToList());
        }

        // GET: Viestis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Viesti viesti = db.Viesti.Find(id);
            if (viesti == null)
            {
                return HttpNotFound();
            }
            return View(viesti);
        }

        //// GET: Viestis/Create
        //public ActionResult Create()
        //{
        //    ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi");
        //    ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi");
        //    ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi");
        //    return View();
        //}

        //// POST: Viestis/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult Create([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Viesti.Add(viesti);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", viesti.opiskelija_Id);
        //    ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", viesti.paaKayttaja_Id);
        //    ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", viesti.yritys_Id);
        //    return View(viesti);
        //}

        //****************************************************************
        //luodaan id:n avulla uusi viesti opiskelijalle
        //****************************************************************

        public ActionResult Create(int? opisId)
        {
            Viesti model = new Viesti();
            model.opiskelija_Id = opisId;

            return View(model);
        }

        // POST: Viestis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti, int? opisId)
        {

            if (ModelState.IsValid)
            {
                using (Stud1Entities dc = new Stud1Entities())
                {
                    Viesti uusiViesti = new Viesti();
                    uusiViesti.inbox = viesti.inbox;
                    uusiViesti.viestiLoki = viesti.viestiLoki;
                    uusiViesti.opiskelija_Id = opisId;
                    uusiViesti.yritys_Id = Convert.ToInt32(Session["corporate_id"]);
                    uusiViesti.onkoVastaus = false;

                    db.Viesti.Add(uusiViesti);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", viesti.opiskelija_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", viesti.paaKayttaja_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", viesti.yritys_Id);
            return View(viesti);
        }

        //****************************************************************
        //luodaan id:n avulla vastaus yritykselle
        //****************************************************************
        public ActionResult Reply(int? yritysId, string vastausAihe)
        {
            Viesti replyModel = new Viesti();
            replyModel.yritys_Id = yritysId;
            replyModel.inbox = vastausAihe;

            return View(replyModel);
        }

        // POST: Viestis/Reply
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Reply([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti, int? yritysId, string vastausAihe)
        {

            if (ModelState.IsValid)
            {
                using (Stud1Entities dc = new Stud1Entities())
                {
                    Viesti uusiVastausViesti = new Viesti();
                    uusiVastausViesti.inbox = vastausAihe;
                    uusiVastausViesti.viestiLoki = viesti.viestiLoki;
                    uusiVastausViesti.opiskelija_Id = Convert.ToInt32(Session["student_id"]);
                    uusiVastausViesti.yritys_Id = yritysId;
                    uusiVastausViesti.onkoVastaus = true;

                    db.Viesti.Add(uusiVastausViesti);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", viesti.opiskelija_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", viesti.paaKayttaja_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", viesti.yritys_Id);
            return View(viesti);
        }



        // GET: Viestis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Viesti viesti = db.Viesti.Find(id);
            if (viesti == null)
            {
                return HttpNotFound();
            }
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", viesti.opiskelija_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", viesti.paaKayttaja_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", viesti.yritys_Id);
            return View(viesti);
        }

        // POST: Viestis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viesti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", viesti.opiskelija_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", viesti.paaKayttaja_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", viesti.yritys_Id);
            return View(viesti);
        }

        // GET: Viestis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Viesti viesti = db.Viesti.Find(id);
            if (viesti == null)
            {
                return HttpNotFound();
            }
            return View(viesti);
        }

        // POST: Viestis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Viesti viesti = db.Viesti.Find(id);
            db.Viesti.Remove(viesti);
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



        //Haetaan tietty opiskelija_id ja listataan ko. id:n viestit
        //****************************************************************************
        public ActionResult HaeOpiskelijanViestit()
        {
            var opislogid = Session["student_id"];
            //opiskelijalle näkyy vain omat viestit
            var opisViestit = (from m in db.Viesti
                              where m.opiskelija_Id.ToString() == opislogid.ToString()
                              select m);

            if (opislogid == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(opisViestit.ToList().Where(m => m.onkoVastaus == false));
        }


    }
}
