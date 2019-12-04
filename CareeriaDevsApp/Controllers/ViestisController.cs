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

        // POST: Viestis/CreateOffer
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
                    uusiViesti.onkoLuettu = false;
                    uusiViesti.viestinPaivays = DateTime.Now;

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
        //luodaan id:n avulla vastaus opiskelijalle v2
        //****************************************************************
        public ActionResult ReplyToStudent(int? opiskelijaId, string vastausAihe)
        {
            //tarkastetaan kuka actionia kutsuu
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    return RedirectToAction("OpisSisalto", "OmaSisaltos");
                }
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid == null)
                {
                    return RedirectToAction("Login","Logins");
                }
            }
            Viesti replyToStudModel = new Viesti();
            replyToStudModel.opiskelija_Id = opiskelijaId;
            replyToStudModel.inbox = vastausAihe;

            return View(replyToStudModel);
        }

        // POST: Viestis/ReplyToStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ReplyTostudent([Bind(Include = "viesti_Id,inbox,viestiLoki,opiskelija_Id,yritys_Id,paaKayttaja_Id")] Viesti viesti, int? opiskelijaId, string vastausAihe)
        {

            if (ModelState.IsValid)
            {
                using (Stud1Entities dc = new Stud1Entities())
                {
                    Viesti uusiVastausViesti = new Viesti();
                    uusiVastausViesti.inbox = vastausAihe;
                    uusiVastausViesti.viestiLoki = viesti.viestiLoki;
                    uusiVastausViesti.yritys_Id = Convert.ToInt32(Session["corporate_id"]);
                    uusiVastausViesti.opiskelija_Id = opiskelijaId;
                    uusiVastausViesti.onkoVastaus = false;
                    uusiVastausViesti.onkoLuettu = false;
                    uusiVastausViesti.viestinPaivays = DateTime.Now;

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




        //****************************************************************
        //luodaan id:n avulla vastaus yritykselle v1
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
                    uusiVastausViesti.onkoLuettu = false;
                    uusiVastausViesti.onkoVastaus = true;
                    uusiVastausViesti.viestinPaivays = DateTime.Now;

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

        //****************************************************************
        //viestiluetuksi
        //****************************************************************
        //**HOX! tällä hetkellä mahdollisesti pystyy suoraan selaimen osoiteriville kirjoitettuaan kirjoittaa viestin luetuksi. ei paha.
        public ActionResult ViestiLuetuksi(int id) //id tulee suoraan osoitekenttään linkistä saatavalla item.viesti_Id:llä
        {
            using (Stud1Entities update = new Stud1Entities())
            {
                Viesti viestiLuetuksi = (from c in update.Viesti
                                         where c.viesti_Id == id
                                         select c).FirstOrDefault();
                viestiLuetuksi.onkoLuettu = true;
                update.SaveChanges();
            }

            //Viesti f = db.Viesti.FirstOrDefault(x => x.viesti_Id == id);
            //f.onkoLuettu = true;
            //db.SaveChanges();
            
            //uudelleenlinkkaus takaisin käyttäjän sivulle onnistuneen tietokantatallennuksen jälkeen:
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                if (Session["student_id"] != null)
                {
                    return RedirectToAction("HaeOpiskelijanViestit", "Viestis", null);
                }
                if (Session["corporate_id"] != null)
                {
                    return RedirectToAction("HaeYrityksenViestit", "Viestis", null);
                }
                return RedirectToAction("Index", "Viestis", null);
            }
            else
            {
                return RedirectToAction("Login", "Logins", null);
            }
            
        }

        //************************************************************************
        //************************************************************************


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
            //jos admin ei ole kirjautunut, niin opiskelijaprofiilien adminsivua ei näytetä
            if (Convert.ToInt32(Session["admin_id"]) != 1) //jos joku muu kuin admin niin tarkastetaan ketä
            {
                if (Session["student_id"] == null && Session["corporate_id"] == null) //jos käyttäjä ei ole kirjautunut niin hänet ohjataan login sivulle kutsuessaan tätä
                {
                    return RedirectToAction("Login", "Logins");
                }
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


        //************************************************************************
        //Viestin haku listaan
        //************************************************************************

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

        //Haetaan tietty corporate_id ja listataan ko. id:n viestit
        //****************************************************************************
        public ActionResult HaeYrityksenViestit()
        {
            var yritysid = Session["corporate_id"];
            //opiskelijalle näkyy vain omat viestit
            var yritysViestit = (from m in db.Viesti
                               where m.yritys_Id.ToString() == yritysid.ToString()
                               select m);

            if (yritysid == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(yritysViestit.ToList().Where(m => m.onkoVastaus == true));
        }


    }
}
