using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CareeriaDevsApp;
using CareeriaDevsApp.Models;
using System.Web.Security;

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


        //***********************************************************************************************************
        //**************************TÄSTÄ ALKAA REKISTERÖITYMINEN JA LOGIN*******************************************
        //***********************************************************************************************************
        //***********************************************************************************************************





        //Oppilaan rekisteröinti***************************************************************
        //*************************************************************************************
        [HttpGet]
        public ActionResult OppilasRekisterointi()
        {
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]

        //Määritellään OppilasRekisterointi.cshtml syötettyjen tietojen tallennuspaikka (luokkaan)...
        public ActionResult OppilasRekisterointi(
            [Bind(Prefix = "Item1")] LoginModel oppkirjautuminen,
            [Bind(Prefix = "Item2")] OpiskelijaModel opiskelija,
            [Bind(Prefix = "Item4")] PuhelinNumeroModel puhelinnro
            )
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email already Exist 


                //Login oppTarkastus = new Login();
                var isExist = IsEmailExist(oppkirjautuminen.kayttajaNimi);
                if (isExist)
                {
                    //TempData["EmailExist"] = "Tämä sähköpostiosoite on jo rekisteröity";
                    //ModelState.AddModelError("EmailExist", "Tämä sähköpostiosoite on jo rekisteröity");
                    ModelState.AddModelError("", "Tämä sähköpostiosoite on jo rekisteröity");
                    return View(); //jos sähköposti löytyy niin käyttäjä palautetaan samaan näkymään. Tämän ei pitäisi poistaa jo syötettyjä tietoja.
                }
                #endregion

                #region Generate Activation Code 
                oppkirjautuminen.aktivointiKoodi = Guid.NewGuid();
                #endregion

                //#region Password Hashing 
                //user.salasana = Crypto.Hash(user.salasana);
                ////salasanan tarkastus user.cs -tiedostosta (en tiedä miten toimii)
                //confirm.ConfirmPassword = Crypto.Hash(confirm.ConfirmPassword); //
                //#endregion
                oppkirjautuminen.onkoEmailAktivoitu = true;//muutettu trueksi koska helpompi kehittäessä...


                #region Tallennus tietokantaan
                using (Stud1Entities dc = new Stud1Entities())
                {
                    //Tietojen tallennus modelista tietokantaan...
                    Opiskelija uusiOpis = new Opiskelija();
                    uusiOpis.etunimi = opiskelija.etunimi;
                    uusiOpis.sukunimi = opiskelija.sukunimi;
                    uusiOpis.opiskelija_Id = opiskelija.opiskelija_Id;

                    Login uusiKirj = new Login();
                    uusiKirj.opiskelija_Id = opiskelija.opiskelija_Id;
                    uusiKirj.kayttajaNimi = oppkirjautuminen.kayttajaNimi;
                    uusiKirj.salasana = oppkirjautuminen.salasana;
                    uusiKirj.aktivointiKoodi = oppkirjautuminen.aktivointiKoodi;
                    uusiKirj.onkoEmailAktivoitu = oppkirjautuminen.onkoEmailAktivoitu;

                    PuhelinNumero uusiPuhO = new PuhelinNumero();
                    uusiPuhO.numero = puhelinnro.numero;
                    uusiPuhO.opiskelija_Id = opiskelija.opiskelija_Id;

                    Sahkoposti uusiSpostiO = new Sahkoposti();
                    uusiSpostiO.sahkopostiOsoite = oppkirjautuminen.kayttajaNimi;
                    uusiSpostiO.opiskelija_Id = opiskelija.opiskelija_Id;

                    OmaSisalto uusiOmasis = new OmaSisalto();
                    uusiOmasis.opiskelija_Id = opiskelija.opiskelija_Id;




                    dc.Opiskelija.Add(uusiOpis);
                    dc.PuhelinNumero.Add(uusiPuhO);
                    dc.Sahkoposti.Add(uusiSpostiO);
                    dc.OmaSisalto.Add(uusiOmasis);
                    dc.Login.Add(uusiKirj);
                    dc.SaveChanges();

                    ////Kutsutaan aktivointisähköpostin lähetysmetodia...
                    //SendVerificationLinkEmail(user.kayttajaNimi, user.aktivointiKoodi.ToString());
                    //message = "Rekisteröityminen onnistui. Rekisteröitymisen aktivointilinkki " + 
                    //    " on lähetetty sähköpostiinne:" + user.kayttajaNimi;
                    //Status = true;
                }
                #endregion
            }
            else
            {
                message = "Virhe käsiteltäessä pyyntöä!";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return RedirectToAction("Login"); //palautuu tällä hetkellä login viewiin, pitäisi varmaan käyttää sähköpostiosoitteen validointia tai rekisteröityminen onnistui sivua jos aikaa jää....
        }

        //Yrityksen rekisteröinti**************************************************************
        //*************************************************************************************
        [HttpGet]
        public ActionResult YritysRekisterointi()
        {
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]

        //Määritellään YritysRekisterointi.cshtml syötettyjen tietojen tallennuspaikka (luokkaan)...
        public ActionResult YritysRekisterointi(
            [Bind(Prefix = "Item1")] LoginModel yriKirjautuminen,
            [Bind(Prefix = "Item2")] YritysModel yritys,
            [Bind(Prefix = "Item4")] PuhelinNumeroModel puhelinnro
            )
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email already Exist 
                var isExist = IsEmailExist(yriKirjautuminen.kayttajaNimi);
                if (isExist)
                {
                    //TempData["EmailExist"] = "Tämä sähköpostiosoite on jo rekisteröity";
                    //ModelState.AddModelError("EmailExist", "Tämä sähköpostiosoite on jo rekisteröity");
                    ModelState.AddModelError("", "Tämä sähköpostiosoite on jo rekisteröity");
                    return View(); //jos sähköposti löytyy niin käyttäjä palautetaan samaan näkymään. Tämän ei pitäisi poistaa jo syötettyjä tietoja.
                }
                #endregion

                #region Generate Activation Code 
                yriKirjautuminen.aktivointiKoodi = Guid.NewGuid();
                #endregion

                //#region Password Hashing 
                //user.salasana = Crypto.Hash(user.salasana);
                ////salasanan tarkastus user.cs -tiedostosta (en tiedä miten toimii)
                //confirm.ConfirmPassword = Crypto.Hash(confirm.ConfirmPassword); //
                //#endregion
                //yriKirjautuminen.onkoEmailAktivoitu = true;//muutettu trueksi koska helpompi kehittäessä...


                #region Save to Database
                using (Stud1Entities dc = new Stud1Entities())
                {
                    //Tietojen tallennus modelista tietokantaan...
                    Yritys uusiYrit = new Yritys();
                    uusiYrit.yrityksenNimi = yritys.yrityksenNimi;
                    uusiYrit.Y_tunnus = yritys.Y_tunnus;
                    uusiYrit.lahiosoite = yritys.lahiosoite;

                    Login uusiKirjY = new Login();
                    uusiKirjY.yritys_Id = yritys.yritys_Id;
                    uusiKirjY.kayttajaNimi = yriKirjautuminen.kayttajaNimi;
                    uusiKirjY.salasana = yriKirjautuminen.salasana;
                    uusiKirjY.aktivointiKoodi = yriKirjautuminen.aktivointiKoodi;
                    uusiKirjY.onkoEmailAktivoitu = yriKirjautuminen.onkoEmailAktivoitu;

                    PuhelinNumero uusiPuhY = new PuhelinNumero();
                    uusiPuhY.numero = puhelinnro.numero;
                    uusiPuhY.yritys_Id = yritys.yritys_Id;

                    Sahkoposti uusiSpostiY = new Sahkoposti();
                    uusiSpostiY.sahkopostiOsoite = yriKirjautuminen.kayttajaNimi;
                    uusiSpostiY.yritys_Id = yritys.yritys_Id;


                    dc.Yritys.Add(uusiYrit);
                    dc.PuhelinNumero.Add(uusiPuhY);
                    dc.Sahkoposti.Add(uusiSpostiY);
                    dc.Login.Add(uusiKirjY);
                    dc.SaveChanges();

                    ////Kutsutaan aktivointisähköpostin lähetysmetodia...
                    //SendVerificationLinkEmail(user.kayttajaNimi, user.aktivointiKoodi.ToString());
                    //message = "Rekisteröityminen onnistui. Rekisteröitymisen aktivointilinkki " + 
                    //    " on lähetetty sähköpostiinne:" + user.kayttajaNimi;
                    //Status = true;
                }
                #endregion
            }
            else
            {
                message = "Virhe käsiteltäessä pyyntöä!";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return RedirectToAction("Login"); //palautuu tällä hetkellä login viewiin, pitäisi varmaan käyttää sähköpostiosoitteen validointia tai rekisteröityminen onnistui sivua jos aikaa jää....
        }


        //Tilin aktivointi*******************************************************************************************
        //***********************************************************************************************************
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (Stud1Entities dc = new Stud1Entities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //SaveChanges();

                var v = dc.Login.Where(a => a.aktivointiKoodi == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.onkoEmailAktivoitu = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Virhe käsiteltäessä pyyntöä!";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Kirjautuminen**************************************************************************************************
        //***************************************************************************************************************
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (Stud1Entities dc = new Stud1Entities())
            {
                var v = dc.Login.Where(a => a.kayttajaNimi == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    //if (!v.onkoEmailAktivoitu) //jos email-osoitetta ei ole aktivoitu niin:
                    //{
                    //    ViewBag.Message = "Olkaa hyvä ja vahvistakaa sähköpostiosoitteenne"; //ei välttämätön, mutta aika yleinen, jos jää aikaa niin toteutetaan
                    //    return View();
                    //}

                    //tämä tarkastaa generoidun passwordin ja syötetyn passwordin eron
                    //ei välttämättä tarvitse tehdä erikoista passwordia, mutta yllä oleva sähköpostivahvistus olisi varmasti hyvä olla
                    //original:
                    //if (string.Compare(Crypto.Hash(login.Password), v.salasana) == 0)
                    if (string.Compare(login.Password, v.salasana) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Tarkista käyttäjänimi ja salasana.";
                    }
                }
                else
                {
                    message = "Tarkista käyttäjänimi ja salasana.";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout****************************************************************************************
        //**********************************************************************************************
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


        //tarkastetaan onko syötetty sähköpostiosoite jo rekisteröity?...
        //***************************************************************
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (Stud1Entities dc = new Stud1Entities())
            {
                var v = dc.Login.Where(a => a.kayttajaNimi == emailID).FirstOrDefault();
                return v != null;
            }
        }

        //aktivointisähköpostin lähetys käyttäjän rekisteröidyttyä*********jos jää aikaa****************
        //**********************************************************************************************
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("Tähän sähköpostiosoite mistä lähetetään", "Lähettäjän nimi?"); //lähettäjän "email","nimi"
            var toEmail = new MailAddress(emailID); //juuri luodun profiilin, eli vastaanottajan sähköpostiosoite.
            var fromEmailPassword = ""; // Tähän sen emailin salasana mistä data lähtee käyttäjälle.
            string subject = "Tilinne on luotu onnistuneesti!";

            string body = "<br/><br/>Tilinne on onnistuneesti luotu." +
                "  Olkaa hyvä ja vahvistakaa tilinne alla olevasta linkistä." +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        //***********************************************************************************************************
        //**************************TÄHÄN LOPPUU REKISTERÖITYMINEN JA LOGIN******************************************
        //***********************************************************************************************************
        //***********************************************************************************************************


    }
}

