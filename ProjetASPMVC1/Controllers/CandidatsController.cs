using ProjetASPMVC1.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ProjetASPMVC1.Controllers
{
    public class CandidatsController : Controller
    {
        public static String id;
        private Projet_ContextBD db = new Projet_ContextBD();
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

        [HttpPost]
        public ActionResult Creation(

           string CIN, string CNE, string  prenom, 
           string  nom, string  ville, string  addresse, string  tel,
           string GSM, string  type_bac,
           string annee_bac, string note_bac, string mention_bac, 
           string  nationnalite, string  sexe, string  password, string  password_conf, string  photo,
           string  email,  string  niveau, string  date_naiss,
           string  id_fil, string s1, string s2, string s3, string s4, string s5, string s6,string nom_dip,
           string villeDip, string  etab
            )
        {
            
            if(ModelState.IsValid)
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
            
            cand.id_fil =Convert.ToInt32(id_fil);
           
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

            cand.cont_ajout= 0;
            cand.password = password;
            cand.password_conf = password_conf;
            cand.photo = photo;
            cand.email = email;
            cand.statut = "Candidat";
            cand.niveau = niveau;
            cand.date_naiss =Convert.ToDateTime(date_naiss);
            db.Notes.Add(note);
            db.SaveChanges();
            db.Diplomes.Add(dip);
            db.SaveChanges();
            cand.id_diplome = dip.id_diplome;
            cand.id_note = note.id_note;
            db.Candidats.Add(cand);
            db.SaveChanges();

            }

            return RedirectToAction("Index","Home");
        }
        


        public ActionResult Next_page(String id)

        {


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

        public ActionResult Edit()//your view page
        {
            return View();
        }
    }
}