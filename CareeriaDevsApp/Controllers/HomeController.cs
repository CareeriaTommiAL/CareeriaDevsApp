using CareeriaDevsApp.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareeriaDevsApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //login tiedot näkyville
            if (Session["Username"] == null)
            {
                Session["LoggedStatus"] = "uloskirjautuneena";
            }
            else
            {
                Session["LoggedStatus"] = "sisäänkirjautuneena";

            }

            return View();
        }
        //**********************************************************************************************************
        //tämän joutuu lisäämään joka controlleriin erikseen, jotta saadaan näkyviin ja jotta se toimisi oikein
        /*
         * 
            if (Session["Username"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                Session["LoggedStatus"] = "in";
                return View();
            }

         */
        //**********************************************************************************************************


        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Login-----------------------------------------------------------------------------------------------------------------------
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Login LoginModel)
        {
            CareeriaDevsApp.Databases.CareeriaDevAppEntities db = new CareeriaDevsApp.Databases.CareeriaDevAppEntities();
            //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä
            var LoggedUser = db.Login.SingleOrDefault(x => x.kayttajaNimi == LoginModel.kayttajaNimi && x.salasana == LoginModel.salasana);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Sisäänkirjautuminen onnistui";
                Session["LoggedStatus"] = "sisäänkirjautuneena";
                Session["Username"] = LoggedUser.kayttajaNimi;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Kirjautuminen epäonnistui";
                Session["LoggedStatus"] = "uloskirjautuneena";
                LoginModel.LoginVirhe = "Tuntematon käyttäjänimi tai salasana.";
                return View("Home", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "uloskirjautuneena";
            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen pääsivulle
        }



    }
}