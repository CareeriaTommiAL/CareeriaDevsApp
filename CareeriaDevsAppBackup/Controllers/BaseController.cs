using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareeriaDevsApp.Controllers
{
    public class BaseController : Controller
    {
        //*********************************************************************************************************************
        // käyttäjien tarkistusmetodi (jos eri kuin admin, ja jos student_id tai corporate_id annettuna sessionissa niin...)
        //*********************************************************************************************************************
        protected bool TryGetRedirectUrl(ActionResult student, ActionResult corporate, out ActionResult redirectResult)
        {
            if (Convert.ToInt32(Session["admin_id"]) != 1) //muista muuttaa Session["admin_id"]) != 1 kaikkialle jos muutetaan paakayttaja_id Login-tauluun,
                                                           //tämä on databasessa kiinteänä id:nä (eli vain 1 admin tällä hetkellä)!!!!!!!!!!!!!!!!!!!
            {

                //jos käyttäjällä on Session["student_id"], niin opiskelija ohjataan suoraan omaan profiiliin
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    redirectResult = student;
                    return true;
                }
                //jos käyttäjällä on Session["corporate_id"], niin yritys ohjataan suoraan samanlaiseen näkymään kuin adminilla, mutta ilman toimintoja
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid != null)
                {
                    redirectResult = corporate;
                    return true;
                }

                redirectResult = RedirectToAction("Login", "Logins");
                return true;
            }

            redirectResult = null;
            return false;
        }

        //***********************************************
        //Jos kyseessä on yritys
        //***********************************************
        protected bool TryGetRedirectUrlWhereYritys(ActionResult studentCorporate, ActionResult corporateCorporate, out ActionResult redirectResultCorporate)
        {
            //tarkastetaan kuka actionia kutsuu
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                var opislogid = Session["student_id"];
                if (opislogid != null)
                {
                    redirectResultCorporate = studentCorporate;
                    return true;
                }
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid == null)
                {
                    redirectResultCorporate = corporateCorporate;
                    return true;
                }

            }
            redirectResultCorporate = null;
            return false;
        }

        //***********************************************
        //Jos kyseessä on opiskelija
        //***********************************************
        protected bool TryGetRedirectUrlWhereStudent(ActionResult studentStudent, ActionResult corporateStudent, out ActionResult redirectResultStudent)
        {

            //tarkastetaan kuka actionia kutsuu
            if (Convert.ToInt32(Session["admin_id"]) != 1)
            {
                var opislogid = Session["student_id"];
                if (opislogid == null)
                {
                    redirectResultStudent = studentStudent;
                    return true;
                }
                var yrityslogid = Session["corporate_id"];
                if (yrityslogid != null)
                {
                    redirectResultStudent = corporateStudent;
                    return true;
                }
            }
            redirectResultStudent = null;
            return false;
        }


        //***********************************************
        //Jos kyseessä pelkkä admin
        //***********************************************

    }
}