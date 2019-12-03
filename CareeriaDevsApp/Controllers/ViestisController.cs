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
            //jos admin ei ole kirjautunut, niin opiskelijaprofiilien adminsivua ei näytetä
            if (Convert.ToInt32(Session["admin_id"]) != 1) //muista muuttaa Session["admin_id"]) != 1 kaikkialle jos muutetaan paakayttaja_id Login-tauluun,
                                                           //tämä on databasessa kiinteänä id:nä (eli vain 1 admin tällä hetkellä)!!!!!!!!!!!!!!!!!!!
            {
                //jos käyttäjällä on Session["student_id"], niin opiskelija ohjataan suoraan omaan profiiliin
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    return RedirectToAction("OpisSisalto", "OmaSisaltos");
                }
                //jos käyttäjällä on Session["corporate_id"], niin yritys ohjataan suoraan samanlaiseen näkymään kuin adminilla, mutta ilman toimintoja
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid != null)
                {
                    return RedirectToAction("YritysSisalto", "OmaSisaltos");
                }
                return RedirectToAction("Login", "Logins");//jos mikään ylläolevista ei toteudu niin käyttäjä ohjataan login -viewiin.
            }
            var viesti = db.Viesti.Include(v => v.Opiskelija).Include(v => v.PaaKayttaja).Include(v => v.Yritys);
            return View(viesti.ToList());
        }


        // GET: Viestis/Create
        public ActionResult Create()
        {
            //jos admin ei ole kirjautunut, niin opiskelijaprofiilien adminsivua ei näytetä
            if (Convert.ToInt32(Session["admin_id"]) != 1) //muista muuttaa Session["admin_id"]) != 1 kaikkialle jos muutetaan paakayttaja_id Login-tauluun,
                                                           //tämä on databasessa kiinteänä id:nä (eli vain 1 admin tällä hetkellä)!!!!!!!!!!!!!!!!!!!
            {
                //jos käyttäjällä on Session["student_id"], niin opiskelija ohjataan suoraan omaan profiiliin
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    return RedirectToAction("OpisSisalto", "OmaSisaltos");
                }
                //jos käyttäjällä on Session["corporate_id"], niin yritys ohjataan suoraan samanlaiseen näkymään kuin adminilla, mutta ilman toimintoja
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid != null)
                {
                    return RedirectToAction("YritysSisalto", "OmaSisaltos");
                }
                return RedirectToAction("Login", "Logins");//jos mikään ylläolevista ei toteudu niin käyttäjä ohjataan login -viewiin.
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi");
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi");
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi");
            return View();
        }

        // POST: Viestis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti)
        {
            if (ModelState.IsValid)
            {
                db.Viesti.Add(viesti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi", viesti.opiskelija_Id);
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi", viesti.paaKayttaja_Id);
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi", viesti.yritys_Id);
            return View(viesti);
        }


        //****************************************************************
        //luodaan id:n avulla uusi viesti opiskelijalle
        //****************************************************************

        public ActionResult CreateOffer(int? opisId)
        {
            //tarkastetaan kuka actionia kutsuu
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    return RedirectToAction("OpisSisalto", "OmaSisaltos"); //opiskelija ohjataan omalle sivulleen
                }
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid == null)
                {
                    return RedirectToAction("Login", "Logins"); //jos yritys ei ole kirjautuneena, niin ohjataan yritys kirjautumissivulle
                                                                //(ei pitäisi olla mahdollista, että näin käy koska tähän viitataan yrityksen YritysSisalto -viewistä)
                }
            }
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
        public ActionResult CreateOffer([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti, int? opisId)
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
            //tarkastetaan kuka actionia kutsuu
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                var opislogid = Session["student_id"];
                if (opislogid == null)
                {
                    return RedirectToAction("Login", "Logins"); //jos opiskelija ei ole kirjautuneena, niin ohjataan opiskelija kirjautumissivulle
                                                                //(ei pitäisi olla mahdollista, että näin käy koska tähän viitataan opiskelijan HaeOpiskelijanViestit -viewistä)
                }
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid != null)
                {
                    return RedirectToAction("YritysSisalto", "OmaSisaltos"); //yritys ohjataan omalle sivulleen
                }
            }
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
            //jos admin ei ole kirjautunut, niin opiskelijaprofiilien adminsivua ei näytetä
            if (Convert.ToInt32(Session["admin_id"]) != 1) //muista muuttaa Session["admin_id"]) != 1 kaikkialle jos muutetaan paakayttaja_id Login-tauluun,
                                                           //tämä on databasessa kiinteänä id:nä (eli vain 1 admin tällä hetkellä)!!!!!!!!!!!!!!!!!!!
            {
                //jos käyttäjällä on Session["student_id"], niin opiskelija ohjataan suoraan omaan profiiliin
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    return RedirectToAction("OpisSisalto", "OmaSisaltos");
                }
                //jos käyttäjällä on Session["corporate_id"], niin yritys ohjataan suoraan samanlaiseen näkymään kuin adminilla, mutta ilman toimintoja
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid != null)
                {
                    return RedirectToAction("YritysSisalto", "OmaSisaltos");
                }
                return RedirectToAction("Login", "Logins");//jos mikään ylläolevista ei toteudu niin käyttäjä ohjataan login -viewiin.
            }

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
