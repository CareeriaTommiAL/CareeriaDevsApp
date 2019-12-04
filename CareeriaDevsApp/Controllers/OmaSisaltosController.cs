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

        //Adminin näkymä Index.cshtml, ja vain adminin
        //********************************************
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


            var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            return View(omaSisalto.ToList());
        }




        //Haetaan tietty opiskelija_id ja listataan ko. id:n näkymä OpisSisalto.cshtml
        //****************************************************************************
        public ActionResult OpisSisalto()
        {
            var opislogid = Session["student_id"];
            //vain se opiskelija kenen opiskelija_id on sama kun muokattava id, pystyy tätä id:tä muokkaamaan
            var opisTeksti = (from m in db.OmaSisalto
                              where m.opiskelija_Id.ToString() == opislogid.ToString()
                              select m);

            if (opislogid == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            var i = 0;

            foreach (var item in db.Viesti) //haetaan lukemattomat viestit viewbagiin
            {
                if (item.onkoLuettu == false)
                {
                    if (item.opiskelija_Id == Convert.ToInt32(Session["student_id"]))
                    {
                        if (item.onkoVastaus == false)
                        {
                            i++;
                        }

                    }

                }
            };


            ViewBag.lukemattomatViestit = i;

            var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            return View(opisTeksti.ToList());
        }




        //haetaan tietty yritys_id ja listataan ko. id:n näkymä YritysSisalto.cshtml
        //**************************************************************************
        public ActionResult YritysSisalto(string search)  //Tommi, hakumetodi
        {
            var yrityslogid = Session["corporate_id"];
            if (yrityslogid == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            var i = 0;

            foreach (var item in db.Viesti) //haetaan lukemattomat viestit viewbagiin
            {
                if (item.onkoLuettu == false)
                {
                    if (item.yritys_Id == Convert.ToInt32(Session["corporate_id"]))
                    {
                        if (item.onkoVastaus == true)
                        {
                            i++;
                        }

                    }

                }
            };


            ViewBag.lukemattomatViestit = i;



            //var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            //return View(omaSisalto.ToList());
            return View(db.OmaSisalto.Where(x => x.omaTeksti.Contains(search) || search == null).ToList());  //Tommi, listaa profiilit joiden omaTeksti sisältää hakusanan tai sen osan. Jos haku = null, näytetään kaikki profiilit.

        }



        // Uuden omasisällön luonti. (Joutuu luomaan olemassa olevaan id:hen, mahdollisesti kaatuu jos tietoja jo olemassa) EI TESTATTU HOX! EI TESTATTU!
        //********************************************************************************************************************************************
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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




        // Omansisällön muokkaus...missa (int? id) = opiskelija_Id
        //*********************************************************
        public ActionResult Edit(int? id)
        {
            //adminin pitäisi aina päästä muokkaamaan...
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                //käyttäjä ei pääse muokkaamaan omasisältöä, jos loginissa saatu Session["student_id"] ei vastaa edittiin menossa olevaa opiskelija_id:tä
                if (Convert.ToInt32(Session["student_id"]) != id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OmaSisalto omaSisalto = db.OmaSisalto.Where(c => c.opiskelija_Id == id).SingleOrDefault();
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




        // Omasisällön poisto-oikeus vain adminilla (taulujen linkkaukset tuo varmasti omat ongelmansa) EI TESTATTU
        //**********************************************************************************************************
        public ActionResult Delete(int? id)
        {
            if (Convert.ToInt32(Session["admin_id"]) != 1)
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
