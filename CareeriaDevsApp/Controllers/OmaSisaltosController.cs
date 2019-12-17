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
using PagedList;
using PagedList.Mvc;


namespace CareeriaDevsApp.Controllers
{
    public class OmaSisaltosController : BaseController
    {
        private Stud1Entities db = new Stud1Entities();

        public ActionResult Index()
        {

            //****HOX!**** tätä voit kutsua BaseControllerista ja muuttaa redirectit mieluiseksi ks. Controllers -> Basecontroller ****HOX!****
            if (TryGetRedirectUrl(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
                                  RedirectToAction("YritysSisalto", "OmaSisaltos"),
                                  out var redirectResult))
            { 
                return redirectResult;
            }
            var omaSisalto = db.OmaSisalto.Include(o => o.Opiskelija);
            return View(omaSisalto.ToList());
        }



        //Haetaan tietty opiskelija_id ja listataan ko. id:n näkymä OpisSisalto.cshtml
        //****************************************************************************
        public ActionResult OpisSisalto()
        {

            if (TryGetRedirectUrlWhereStudent(RedirectToAction("Login", "Logins", null), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos", null),
            out var redirectResult))
            {
                return redirectResult;
            }


            var opiskelijaid = Convert.ToInt32(Session["student_id"]);
            var opiskelijan = db.Opiskelija.FirstOrDefault(y => y.opiskelija_Id == opiskelijaid);
            ViewBag.opiskelijanimi = opiskelijan.etunimi + " " + opiskelijan.sukunimi;


            //vain se opiskelija kenen opiskelija_id on sama kun muokattava id, pystyy tätä id:tä muokkaamaan
            var opislogid = Session["student_id"];
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
        public ActionResult YritysSisalto(string searchBy, string search, int? page)  //Tommi, hakumetodi
        {

            if (TryGetRedirectUrlWhereYritys(RedirectToAction("Login", "Logins"), //BaseControllerilta saadaan käyttäjätodennus
          RedirectToAction("OpisSisalto", "OmaSisaltos"),
          out var redirectResult))
            {
                return redirectResult;
            }

            var corporate = Convert.ToInt32(Session["corporate_id"]);
            var corporatename = db.Yritys.FirstOrDefault(y => y.yritys_Id == corporate);
            ViewBag.yritysnimi = corporatename.yrityksenNimi;


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
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (search== "")
            {
                return View(db.OmaSisalto.OrderBy(x => x.opiskelija_Id).ToPagedList(pageNumber, pageSize));
            }
            else return View(db.OmaSisalto.Where(x => x.omaTeksti.Contains(search) || search == null).OrderBy(x => x.opiskelija_Id).ToPagedList(pageNumber, pageSize));  //Tommi, listaa profiilit joiden omaTeksti sisältää hakusanan tai sen osan. Jos haku = null, näytetään kaikki profiilit.

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
                    RedirectToAction("OpisSisalto", "OmaSisaltos", null);
                }
                if (Session["corporate_id"] != null)
                {
                    RedirectToAction("YritysSisalto", "OmaSisaltos", null);
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




        // Omasisällön poisto-oikeus vain adminilla ****************
        //**********************************************************
        public ActionResult Delete(int? id)
        {
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                RedirectToAction("Index", "Home", null);
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
