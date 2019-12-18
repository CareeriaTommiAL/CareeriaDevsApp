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
using System.Globalization;

namespace CareeriaDevsApp.Controllers
{
    public class LoginsController : BaseController
    {
        private Stud1Entities db = new Stud1Entities();

        // GET: Logins
        public ActionResult Index() //admin näkymä
        {
            if (TryGetRedirectUrlWhereAdmin(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos"),
            out var redirectResult))
            {
                return redirectResult;
            }

            var login = db.Login.Include(l => l.Opiskelija).Include(l => l.Yritys).Include(l => l.PaaKayttaja);
            return View(login.ToList());
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            if (TryGetRedirectUrlWhereAdmin(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos"),
            out var redirectResult))
            {
                return redirectResult;
            }

            ViewBag.opiskelija_Id = new SelectList(db.Opiskelija, "opiskelija_Id", "etunimi");
            ViewBag.yritys_Id = new SelectList(db.Yritys, "yritys_Id", "yrityksenNimi");
            ViewBag.paaKayttaja_Id = new SelectList(db.PaaKayttaja, "paaKayttaja_Id", "nimi");
            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka", "postinumero");
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
            if (TryGetRedirectUrlWhereAdmin(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos"),
            out var redirectResult))
            {
                return redirectResult;
            }

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
            if (TryGetRedirectUrlWhereAdmin(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos"),
            out var redirectResult))
            {
                return redirectResult;
            }

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
        //*****************https://www.youtube.com/watch?v=gSJFjuWFTdA&feature=youtu.be&t=1472***********************
        //*******************************thank you sourav mondal*****************************************************
        //***********************************************************************************************************





        //Oppilaan rekisteröinti***************************************************************
        //*************************************************************************************
        [HttpGet]
        public ActionResult OppilasRekisterointi()
        {
            if (TryGetRedirectDefault(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos"),
             out var redirectResult))
            {
                return redirectResult;
            }

            ViewBag.postitoimipaikka_Id = new SelectList(db.Postitoimipaikka, "postitoimipaikka_Id", "postinumero");
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]


        //OppilasRekisterointi.cshtml käyttää Tuple -määritystä modeleihin(luokkiin) kirjoittamista varten ja ne ovar array -muodossa(Item1,Item2,Item3 jne.)
        //Määritellään OppilasRekisterointi.cshtml syötettyjen tietojen tallennuspaikka (luokkaan)...
        public ActionResult OppilasRekisterointi(
            [Bind(Prefix = "Item1")] LoginModel oppkirjautuminen,
            [Bind(Prefix = "Item2")] OpiskelijaModel opiskelija,
            [Bind(Prefix = "Item3")] PostitoimipaikkaModel pstmp,
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
                    uusiOpis.etunimi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uusiOpis.etunimi.ToLower()); //muutetaan ensimmäinen kirjain isoksi, koska käyttäjä
                    uusiOpis.sukunimi = opiskelija.sukunimi;
                    uusiOpis.sukunimi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uusiOpis.sukunimi.ToLower()); //muutetaan ensimmäinen kirjain isoksi, koska käyttäjä
                    uusiOpis.opiskelija_Id = opiskelija.opiskelija_Id;

                    string opiskelijanpostinumero = pstmp.postinumero;
                    //Postitoimipaikka id:n haku postinumeron perusteella
                    uusiOpis.postitoimipaikka_Id = (from x in db.Postitoimipaikka where x.postinumero == opiskelijanpostinumero select x.postitoimipaikka_Id).First();

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
                    uusiOmasis.omaKuva = "/Content/Images/avatar.png";


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
            if (TryGetRedirectDefault(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
            RedirectToAction("YritysSisalto", "OmaSisaltos"),
            out var redirectResult))
            {
                return redirectResult;
            }
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]

        //Määritellään YritysRekisterointi.cshtml syötettyjen tietojen tallennuspaikka (luokkaan)...
        public ActionResult YritysRekisterointi(
            [Bind(Prefix = "Item1")] LoginModel yriKirjautuminen,
            [Bind(Prefix = "Item2")] YritysModel yritys,
            [Bind(Prefix = "Item3")] PostitoimipaikkaModel pstmp,
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
                yriKirjautuminen.onkoEmailAktivoitu = true;//muutettu trueksi koska helpompi kehittäessä...


                #region Tallennus kantaan
                using (Stud1Entities dc = new Stud1Entities())
                {
                    //Tietojen tallennus modelista tietokantaan...
                    Yritys uusiYrit = new Yritys();
                    uusiYrit.yrityksenNimi = yritys.yrityksenNimi;
                    uusiYrit.yrityksenNimi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uusiYrit.yrityksenNimi.ToLower()); //muutetaan ensimmäinen kirjain isoksi, koska käyttäjä
                    uusiYrit.Y_tunnus = yritys.Y_tunnus;
                    uusiYrit.lahiosoite = yritys.lahiosoite;

                    string yrityksenpostinumero = pstmp.postinumero;

                    uusiYrit.postitoimipaikka_Id = (from x in db.Postitoimipaikka where x.postinumero == yrityksenpostinumero select x.postitoimipaikka_Id).First(); //Postitoimipaikka id:n haku postinumeron perusteella



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
            //mikäli käyttäjä on jo kirjautunut hänet ohjataan suoraan etusivulle, jos hän jostain syystä koittaa päästä kirjautumissivulle...
            if (Session["student_id"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["corporate_id"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["admin_id"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
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
                    //**************************************************
                    //******käyttäjän tunnnistus loginista**************
                    //**************************************************
                    if (v.opiskelija_Id != null) //jos käyttäjällä on login -taulun opiskelija_id:ssä tietoa, niin käyttäjä tunnistetaan opiskelijaksi.
                    {
                        System.Diagnostics.Debug.WriteLine("tämä käyttäjä on opiskelija");
                        Session["student_id"] = v.opiskelija_Id;
                        Session["student_kayttajaNimi"] = v.kayttajaNimi;
                        Session["islogged"] = 1; //käyttäjälle lisätään apumuuttuja, jota voi käyttää todentamaan onko jokin käyttäjistä kirjautunut.
                    }
                    if (v.yritys_Id != null) //jos käyttäjällä on login -taulun yritys_id:ssä tietoa, niin käyttäjä tunnistetaan yritykseksi.
                    {
                        System.Diagnostics.Debug.WriteLine("tämä käyttäjä on yritys");
                        Session["corporate_id"] = v.yritys_Id;
                        Session["corporate_kayttajaNimi"] = v.kayttajaNimi;
                        Session["islogged"] = 1;
                    }
                    if (v.paaKayttaja_Id != null) //jos käyttäjällä on login -taulun pääkäyttäjä_id:ssä tietoa, niin käyttäjä tunnistetaan pääkäyttäjäksi.
                    {
                        System.Diagnostics.Debug.WriteLine("tämä käyttäjä on pääkäyttäjä");
                        Session["admin_id"] = v.paaKayttaja_Id;
                        Session["admin_kayttajaNimi"] = v.kayttajaNimi;
                        Session["islogged"] = 1;
                    }

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
                            if (v.opiskelija_Id != null) //kirjautunut opiskelija ohjataan omaan näkymäänsä
                            {
                                return RedirectToAction("OpisSisalto", "Omasisaltos");
                            }
                            if (v.yritys_Id != null) //kirjautunut yritys ohjataan omaan näkymäänsä
                            {
                                return RedirectToAction("YritysSisalto", "Omasisaltos");
                            }
                            if (v.paaKayttaja_Id != null) //kirjautunut pääkäyttäjä(admin) ohjataan omaan näkymäänsä
                            {
                                return RedirectToAction("Index", "Omasisaltos");
                            }

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
        //[Authorize]
        //[HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            FormsAuthentication.SignOut();

            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();

            return RedirectToAction("Login", "Logins");
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




        //*****************************************************************************************
        //Oppilaan tietojen päivitys***************************************************************
        //*****************************************************************************************

        [HttpGet]
        public ActionResult OppilasTiedotUpdate(int? id)
        {
            if (TryGetRedirectUrlWhereStudent(RedirectToAction("OpisSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
                      RedirectToAction("YritysSisalto", "OmaSisaltos"),
                      out var redirectResult))
            {
                return redirectResult;
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Where(l => l.opiskelija_Id == id).FirstOrDefault();
            if (login == null)
            {
                return HttpNotFound();
            }

            var puhnrotiedot = db.PuhelinNumero.FirstOrDefault(p => p.opiskelija_Id == id);
            if (puhnrotiedot != null)
            {
                var numero = puhnrotiedot.numero;
                ViewBag.puhnro = numero;
            }

            ViewBag.kayttajanimi = login.kayttajaNimi;
            ViewBag.etunimi = login.Opiskelija.etunimi;
            ViewBag.sukunimi = login.Opiskelija.sukunimi;
            ViewBag.postinro = (from a in db.Postitoimipaikka where a.postitoimipaikka_Id == login.Opiskelija.postitoimipaikka_Id select a.postinumero).FirstOrDefault();/*"Kirjoita postinumerosi tähän";*/



            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OppilasTiedotUpdate(
            [Bind(Prefix = "Item1")] LoginModel oppkirjautuminen,
            [Bind(Prefix = "Item2")] OpiskelijaModel opiskelija,
            [Bind(Prefix = "Item3")] PostitoimipaikkaModel pstmp,
            [Bind(Prefix = "Item4")] PuhelinNumeroModel puhelinnro,
            int? id
            )
        {

            bool Status = false;

            // Validation 
            if (id != null)
            {
                #region Opiskelijan päivitettyjen tietojen tallennus tietokantaan

                //etunimen, sukunimen, postitoimipaikan päivitys
                using (Stud1Entities dc = new Stud1Entities())
                {
                    var paivitaOpis = dc.Opiskelija.Where(c => c.opiskelija_Id == id).FirstOrDefault(); //päivitetään sitä opiskelijaa joka omaa saman opiskelija_Id:n mikä (id)viewistä ohjataan tänne
                    paivitaOpis.etunimi = opiskelija.etunimi;
                    paivitaOpis.etunimi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(paivitaOpis.etunimi.ToLower());
                    paivitaOpis.sukunimi = opiskelija.sukunimi;
                    paivitaOpis.sukunimi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(paivitaOpis.sukunimi.ToLower());

                    string opiskelijanpostinumero = pstmp.postinumero;
                    //Postitoimipaikka id:n haku postinumeron perusteella
                    paivitaOpis.postitoimipaikka_Id = (from x in db.Postitoimipaikka where x.postinumero == opiskelijanpostinumero select x.postitoimipaikka_Id).First();

                    dc.Entry(paivitaOpis).State = EntityState.Modified;
                    dc.SaveChanges();
                    dc.Entry(paivitaOpis).State = EntityState.Detached; //en tiedä onko tarpeellinen, mutta toimii
                }


                //salasanan päivitys, jokainen entitymäärityksen tulee olla eriniminen muuten tulee erroria...HOX!
                using (Stud1Entities dcc = new Stud1Entities())
                {
                    var paivitaSa = dcc.Login.Where(d => d.opiskelija_Id == id).FirstOrDefault();
                    paivitaSa.salasana = oppkirjautuminen.salasana;

                    dcc.Entry(paivitaSa).State = EntityState.Modified;
                    dcc.SaveChanges();
                    dcc.Entry(paivitaSa).State = EntityState.Detached; //en tiedä onko tarpeellinen, mutta toimii
                }


                //puhelinnumeron päivitys
                using (Stud1Entities dccc = new Stud1Entities())
                {
                    var paivitaPuh = dccc.PuhelinNumero.Where(e => e.opiskelija_Id == id).FirstOrDefault();
                    paivitaPuh.numero = puhelinnro.numero;

                    dccc.Entry(paivitaPuh).State = EntityState.Modified;
                    dccc.SaveChanges();
                    dccc.Entry(paivitaPuh).State = EntityState.Detached; //en tiedä onko tarpeellinen, mutta toimii
                }

                #endregion

            }

            else
            {
                TempData["tallennusEpaonnistui"] = "Virhe käsiteltäessä pyyntöä!";
            }

            TempData["tallennusOnnistui"] = "Tiedot tallennettiin onnistuneesti!";
            ViewBag.Status = Status;
            return RedirectToAction("OpisSisalto", "OmaSisaltos", null);
        }

        //*****************************************************************************************
        //Yrityksen tietojen päivitys***************************************************************
        //*****************************************************************************************

        [HttpGet]
        public ActionResult YritysTiedotUpdate(int? id)
        {
            if (TryGetRedirectUrlWhereYritys(RedirectToAction("YritysSisalto", "OmaSisaltos"), //BaseControllerilta saadaan käyttäjätodennus
                      RedirectToAction("OpisSisalto", "OmaSisaltos"),
                      out var redirectResult))
            {
                return redirectResult;
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login2 = db.Login.Where(l => l.yritys_Id == id).FirstOrDefault();
            if (login2 == null)
            {
                return HttpNotFound();
            }

            var puhnrotiedot = db.PuhelinNumero.FirstOrDefault(p => p.yritys_Id == id);
            if (puhnrotiedot != null)
            {
                var numero = puhnrotiedot.numero;
                ViewBag.puhnro = numero;
            }

            var yritystiedot = db.Yritys.FirstOrDefault(y => y.yritys_Id == id);
            if (yritystiedot != null)
            {
                ViewBag.yritysnimi = login2.kayttajaNimi;
                ViewBag.yrityksennimi = yritystiedot.yrityksenNimi;
                ViewBag.ytunnus = yritystiedot.Y_tunnus;
                ViewBag.lahiosoite = yritystiedot.lahiosoite;
                ViewBag.postinro = (from a in db.Postitoimipaikka where a.postitoimipaikka_Id == yritystiedot.postitoimipaikka_Id select a.postinumero).FirstOrDefault();/*"Kirjoita postinumerosi tähän";*/
            }


            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YritysTiedotUpdate(
            [Bind(Prefix = "Item1")] LoginModel yrityskirjautuminen,
            [Bind(Prefix = "Item2")] YritysModel yritys,
            [Bind(Prefix = "Item3")] PostitoimipaikkaModel pstmp,
            [Bind(Prefix = "Item4")] PuhelinNumeroModel puhelinnro,
            int? id
            )
        {

            bool Status = false;
            //
            // Model Validation 
            if (id != null)
            {
                #region Yrityksen päivitettyjen tietojen tallennus tietokantaan

                //etunimen, sukunimen, postitoimipaikan päivitys
                using (Stud1Entities dc = new Stud1Entities())
                {
                    var paivitaYritys = dc.Yritys.Where(c => c.yritys_Id == id).FirstOrDefault(); //päivitetään sitä opiskelijaa joka omaa saman opiskelija_Id:n mikä (id)viewistä ohjataan tänne
                    paivitaYritys.yrityksenNimi = yritys.yrityksenNimi;
                    paivitaYritys.Y_tunnus = yritys.Y_tunnus;
                    paivitaYritys.lahiosoite = yritys.lahiosoite;

                    string yrityksenpostinumero = pstmp.postinumero;
                    //Postitoimipaikka id:n haku postinumeron perusteella
                    paivitaYritys.postitoimipaikka_Id = (from x in db.Postitoimipaikka where x.postinumero == yrityksenpostinumero select x.postitoimipaikka_Id).First();

                    dc.Entry(paivitaYritys).State = EntityState.Modified;
                    dc.SaveChanges();
                    dc.Entry(paivitaYritys).State = EntityState.Detached; //en tiedä onko tarpeellinen, mutta toimii
                }


                //salasanan päivitys, jokainen entitymäärityksen tulee olla eriniminen muuten tulee erroria...HOX!
                using (Stud1Entities dcc = new Stud1Entities())
                {
                    var paivitaSaY = dcc.Login.Where(d => d.yritys_Id == id).FirstOrDefault();
                    paivitaSaY.salasana = yrityskirjautuminen.salasana;

                    dcc.Entry(paivitaSaY).State = EntityState.Modified;
                    dcc.SaveChanges();
                    dcc.Entry(paivitaSaY).State = EntityState.Detached; //en tiedä onko tarpeellinen, mutta toimii
                }


                //puhelinnumeron päivitys
                using (Stud1Entities dccc = new Stud1Entities())
                {
                    var paivitaPuhY = dccc.PuhelinNumero.Where(e => e.yritys_Id == id).FirstOrDefault();
                    paivitaPuhY.numero = puhelinnro.numero;

                    dccc.Entry(paivitaPuhY).State = EntityState.Modified;
                    dccc.SaveChanges();
                    dccc.Entry(paivitaPuhY).State = EntityState.Detached; //en tiedä onko tarpeellinen, mutta toimii
                }

                #endregion

            }

            else
            {
                TempData["tallennusEpaonnistui"] = "Virhe käsiteltäessä pyyntöä!";
            }

            TempData["tallennusOnnistui"] = "Tiedot tallennettiin onnistuneesti!";
            ViewBag.Status = Status;
            return RedirectToAction("YritysSisalto", "OmaSisaltos", null);
        }





    }
}

