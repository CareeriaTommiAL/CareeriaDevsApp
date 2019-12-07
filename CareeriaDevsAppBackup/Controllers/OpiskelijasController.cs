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
using CareeriaDevsApp.Controllers.CareeriaDevsApp.Filters;

namespace CareeriaDevsApp.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PreventFromUrl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
        filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
            }
        }
    }
}
namespace CareeriaDevsApp.Controllers
{
    namespace CareeriaDevsApp.Filters
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class PreventFromUrl : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null ||
            filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Dashboard", action = "Index", area = "" }));
                }
            }
        }
    }
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
            
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero");
            return View(opiskelija);
        }

        // GET: Opiskelijas/Edit/5
        [PreventFromUrl]
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
        [PreventFromUrl]
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
        [PreventFromUrl]
        public ActionResult DeleteConfirmed(int id)
        {
            Opiskelija opiskelija = db.Opiskelija.Find(id);
            
            var poistettavaOpiskelija = from l in db.Login where l.opiskelija_Id == id select l; //Hakee login taulusta poistettavan opiskelijan loginin
            var poistettavaSisalto = from x in db.OmaSisalto where x.opiskelija_Id == id select x; //Hakee opiskelijan id:n perusteella oman sisällön kannasta
            var poistettavaPuhelinnumero = from p in db.PuhelinNumero where p.opiskelija_Id == id select p; //Hakee opiskelijan puhelinnumeron PuhelinNumero -taulusta
            var poistettavaSahkoposti = from s in db.Sahkoposti where s.opiskelija_Id == id select s; //Hakee opiskelijan sahkopostin sahkoposti -taulusta;
            var opiskelijanViestit = from m in db.Viesti where m.opiskelija_Id == id select m; //Hakee opiskelijan viestit
            
            foreach (var l in poistettavaOpiskelija)
            {
                db.Login.Remove(l);  //Poistaa opiskelijan rivin login talulusta opiskelija_Id -perusteella
            }
            foreach (var x in poistettavaSisalto)
            {
                db.OmaSisalto.Remove(x); //Poistaa opiskelijan sisällön OmaSisalto -taulusta
            }
            foreach (var p in poistettavaPuhelinnumero)
            {
                db.PuhelinNumero.Remove(p); //Poistaa opiskelijan puhelinnumeron kannasta
            }
            foreach (var s in poistettavaSahkoposti)
            {
                db.Sahkoposti.Remove(s); //Poistaa opiskelijan sähköpostin
            }
            foreach (var m in opiskelijanViestit)
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
