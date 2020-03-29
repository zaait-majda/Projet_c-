using ProjetASPMVC1.Models;
using Rotativa;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Web.Hosting;

namespace ProjetASPMVC1.Controllers
{
    public class CandidatsController : Controller
    {
        public static String id;
        private Projet_ContextBD db = new Projet_ContextBD();
        private readonly object[] cin;

        public ActionResult Index(Candidat objUser)
        {

            return View();


        }
        [HttpPost]
        public ActionResult Authorise(Candidat user)
        {
            using (Projet_ContextBD db = new Projet_ContextBD())
            {
                var userDetail = db.Candidats.Where(x => x.CIN == user.CIN && x.password == user.password).FirstOrDefault();
                if (userDetail == null)
                {
                    // user.LoginErrorMsg = "Invalid UserName or Password";
                    return View("Index", user);
                }
                else
                {
                    Session["CIN"] = user.CIN;
                    ViewBag.cin = user.CIN;
                    Session["prenom"] = user.prenom;
                    return RedirectToAction("Edit", "Candidats");
                }
            }

        }
        public ActionResult LogOut()
        {
            int userId = (int)Session["CIN"];



            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Creation()
        {

            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");
            return View();
        }
        public ActionResult Creation2()
        {

            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");
            return View();
        }


        [HttpPost]
        public ActionResult Creation(

           string CIN, string CNE, string prenom,
           string nom, string ville, string addresse, string tel,
           string GSM, string type_bac,
           string annee_bac, string note_bac, string mention_bac,
           string nationnalite, string sexe, string password, string password_conf, string photo,
           string email, string niveau, string date_naiss,
           string id_fil, string s1, string s2, string s3, string s4, string s5, string s6, string nom_dip,
           string villeDip, string etab


            )
        {

            Candidat cand = new Candidat();
            Notes note = new Notes();
            Diplome dip = new Diplome();
            note.s1 = Convert.ToDouble(s1);
            note.s2 = Convert.ToDouble(s2);
            note.s3 = Convert.ToDouble(s3);
            note.s4 = Convert.ToDouble(s4);
            note.s5 = Convert.ToDouble(s5);
            note.s6 = Convert.ToDouble(s6);
            dip.nom_diplome = nom_dip;
            dip.ville_diplome = ville;
            dip.etablissement = etab;

            cand.id_fil = Convert.ToInt32(id_fil);

            cand.CNE = CNE;
            cand.CIN = CIN;
            cand.prenom = prenom;
            cand.nom = nom;
            cand.ville = ville;
            cand.addresse = addresse;
            cand.tel = tel;
            cand.GSM = GSM;
            cand.type_bac = type_bac;
            cand.annee_bac = annee_bac;
            cand.note_bac = note_bac;
            cand.mention_bac = mention_bac;
            cand.n_dossier = "";

            cand.nationnalite = nationnalite;
            cand.sexe = sexe;
            cand.cont_sup = 0;

            cand.cont_ajout = 0;
            cand.password = password;
            cand.password_conf = password_conf;
            cand.photo = photo;
            cand.email = email;
            cand.statut = "inscrit";
            cand.EmailConfirmed = true;

            cand.niveau = niveau;
            cand.date_naiss = Convert.ToDateTime(date_naiss);
            db.Notes.Add(note);
            db.SaveChanges();
            db.Diplomes.Add(dip);
            db.SaveChanges();
            cand.id_diplome = dip.id_diplome;
            cand.id_note = note.id_note;
            db.Candidats.Add(cand);
            db.SaveChanges();
            sendmail(cand);
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult Creation2(

            string CIN, string CNE, string prenom,
            string nom, string ville, string addresse, string tel,
            string GSM, string type_bac,
            string annee_bac, string note_bac, string mention_bac,
            string nationnalite, string sexe, string password, string password_conf, string photo,
            string email, string niveau, string date_naiss,
            string id_fil, string s1, string s2, string s3, string s4, string s5, string s6, string nom_dip,
            string villeDip, string etab


             )
        {

            Candidat cand = new Candidat();
            Notes note = new Notes();
            Diplome dip = new Diplome();
            note.s1 = Convert.ToDouble(s1);
            note.s2 = Convert.ToDouble(s2);
            note.s3 = Convert.ToDouble(s3);
            note.s4 = Convert.ToDouble(s4);
            note.s5 = Convert.ToDouble(s5);
            note.s6 = Convert.ToDouble(s6);
            dip.nom_diplome = nom_dip;
            dip.ville_diplome = ville;
            dip.etablissement = etab;

            cand.id_fil = Convert.ToInt32(id_fil);

            cand.CNE = CNE;
            cand.CIN = CIN;
            cand.prenom = prenom;
            cand.nom = nom;
            cand.ville = ville;
            cand.addresse = addresse;
            cand.tel = tel;
            cand.GSM = GSM;
            cand.type_bac = type_bac;
            cand.annee_bac = annee_bac;
            cand.note_bac = note_bac;
            cand.mention_bac = mention_bac;
            cand.n_dossier = "0";

            cand.nationnalite = nationnalite;
            cand.sexe = sexe;
            cand.cont_sup = 0;

            cand.cont_ajout = 0;
            cand.password = password;
            cand.password_conf = password_conf;
            cand.photo = photo;
            cand.email = email;
            cand.statut = "Candidat";
            cand.niveau = niveau;
            cand.date_naiss = Convert.ToDateTime(date_naiss);
            db.Notes.Add(note);
            db.SaveChanges();
            db.Diplomes.Add(dip);
            db.SaveChanges();
            cand.id_diplome = dip.id_diplome;
            cand.id_note = note.id_note;
            cand.nom_dip = dip.nom_diplome;
            db.Candidats.Add(cand);
            db.SaveChanges();

            sendmail(cand);

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Next_page(String id)

        {
            ViewBag.CIN = id;

            Candidat candidat = db.Candidats.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }

            return View(candidat);
        }
        public ActionResult EXportPDF()
        {
            id = (string)Session["CIN"];

            return new ActionAsPdf("fiche2")
            {
                FileName = Server.MapPath("~/Content/fiche.pdf")
            };
            //frm
        }

        public ActionResult fiche2()

        {
            String s = CandidatsController.id;
            Candidat candidat = db.Candidats.Find(s);

            return View(candidat);
        }
        public void sendmail(Candidat ca)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");

            var fromAddress = new MailAddress("testensa2020@gmail.com", "ENSAS");
            var toAddress = new MailAddress(ca.email, "To Name");
            const string fromPassword = "majda2018";
            const string subject = "Subject";
            var url = "http://localhost:56154/" + "Candidats/Confirm?regId=" + ca.CIN;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            var nom = "PASS  HI " + ca.nom;
            body += body.Replace("@ViewBag.message1", nom);
            var PASS = "votre mot de passe " + ca.password;
            body += body.Replace("@ViewBag.message", PASS);
            body = body.ToString();
            //  BuildEmailTemplate(ca.CIN);


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);

            }
        }

        public ActionResult Confirm(String regId)
        {
            ViewBag.regID = regId;
            return View();
        }
        public JsonResult RegisterConfirm(String regId)
        {

            using (var ddb = new Projet_ContextBD())
            {
                Candidat Data = ddb.Candidats.Where(x => x.CIN == regId).FirstOrDefault();
                Data.EmailConfirmed = true;

                db.SaveChanges();
                var msg = "Your Email Is Verified!";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Change(String LangugeAbbrevation)
        {
            if (LangugeAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LangugeAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LangugeAbbrevation);

            }
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LangugeAbbrevation;
            Response.Cookies.Add(cookie);



            return View("Index");

        }

        public ActionResult update(String cin, String msg)
        {
            
            Candidat candidat = db.Candidats.Find(Session["CIN"]);
            ViewBag.CIN = cin;
            ViewBag.messageconfirm = msg;
            return View(candidat);
        }

    
        public ActionResult Home()
        {

            Candidat candidat = db.Candidats.Find(Session["CIN"]);
  
            return View(candidat);
        }

        public ActionResult updateinfo(
            String cin, String cne, String prenom, String nom, String ville,
            String adresse, String numéro, String GSM, String nationalité,
            String sexe, String email, String date_naiss, String photo)
        {


            Candidat candidat = db.Candidats.Find(cin);
            candidat.CIN = cin;
            candidat.CNE = cne;
            candidat.prenom = prenom;
            candidat.nom = nom;
            candidat.ville = ville;
            candidat.addresse = adresse;
            candidat.tel = numéro;
            candidat.GSM = GSM;
            candidat.nationnalite = nationalité;
            candidat.email = email;
            candidat.date_naiss = Convert.ToDateTime(date_naiss);

            candidat.sexe = sexe;
            string filename = System.IO.Path.GetFileNameWithoutExtension(photo);
            string extension = System.IO.Path.GetExtension(photo);
            filename = filename + extension;
            candidat.photo = filename;
            db.SaveChanges();

            ViewBag.CIN = cin;
            String msg = "modification des informations personnels avec succés";

            return RedirectToAction("update", new { cin = cin, msg = msg });






        }


        public ActionResult update2(String cin, String msg)
        {

            ViewBag.messageconfirm = msg;

            Candidat candidat = db.Candidats.Find(Session["CIN"]);
            int iddip = candidat.id_diplome;
            Diplome dip = db.Diplomes.Find(iddip);
            ViewBag.nomdip = dip.nom_diplome;
            ViewBag.villedip = dip.ville_diplome;
            ViewBag.etab = dip.etablissement;



            int idnote = candidat.id_note;
            Notes n = db.Notes.Find(idnote);
            ViewBag.s1 = n.s1;
            ViewBag.s2 = n.s2;
            ViewBag.s3 = n.s3;
            ViewBag.s4 = n.s4;
            ViewBag.s5 = n.s5;
            ViewBag.s6 = n.s6;
            ViewBag.CIN = cin;
            return View(candidat);
        }


        public ActionResult updatediplome(String data, String type_bac, String annee_bac, String note_bac,
            String mention_bac, String nom_diplome, String ville_diplome, String etablissment,
            String niveau, String s1, String s2, String s3, String s4, String s5, String s6)
        {
            Candidat Candidat = db.Candidats.Find(data);
            Candidat.type_bac = type_bac;
            Candidat.annee_bac = annee_bac;
            Candidat.note_bac = note_bac;
            Candidat.mention_bac = mention_bac;
            Candidat.niveau = niveau;
            Candidat.nom_dip= nom_diplome;
            db.SaveChanges();

            int iddip = Candidat.id_diplome;
            Diplome dip = db.Diplomes.Find(iddip);
            dip.nom_diplome = nom_diplome;
            dip.ville_diplome = ville_diplome;
            dip.etablissement = etablissment;

            db.SaveChanges();



            int idnote = Candidat.id_note;
            Notes n = db.Notes.Find(idnote);
            n.s1 = Convert.ToDouble(s1);
            n.s2 = Convert.ToDouble(s2);
            n.s3 = Convert.ToDouble(s3);
            n.s4 = Convert.ToDouble(s4);
            n.s5 = Convert.ToDouble(s5);
            n.s6 = Convert.ToDouble(s6);
            db.SaveChanges();
            String id = data;
            ViewBag.CIN = id;

            String msg = "informations sur les diplomes obtenus sont changés avec succés";

            return RedirectToAction("update2", new { cin = id, msg = msg });




        }


        public ActionResult update3(String cin, String msg)
        {

            Candidat candidat = db.Candidats.Find(Session["CIN"]);
            ViewBag.CIN = cin;
            ViewBag.messageconfirm = msg;
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            return View(candidat);

        }


        public ActionResult updatefiliere(String data, String id_fil)
        {



            Candidat candidat = db.Candidats.Find(data);
            candidat.id_fil = Convert.ToInt32(id_fil);
            db.SaveChanges();

            String msg = "modification de choix de filières avec succés";
            String id = data;

            return RedirectToAction("update3", new { cin = id, msg = msg });




        }

        public ActionResult Telecharger()
        {   
            return View();
        }
        public ActionResult Troisiemegtr()
        {
            return View();
        }
        public ActionResult Quatriemegtr()
        {
            return View();
        }
        public ActionResult Troisiemeinfo()
        {
            return View();
        }
        public ActionResult Quatriemeinfo()
        {
            return View();
        }
        public ActionResult Troisiemeindus()
        {
            return View();
        }
        public ActionResult Quatriemindus()
        {
            return View();
        }
        public ActionResult deleteCompte()
        {

         
            return View();
        }

        [HttpPost]
        public ActionResult deleteCompteConfirm()
        {

            Candidat can = db.Candidats.Find(Session["CIN"]);
            db.Candidats.Remove(can);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }



    }
}