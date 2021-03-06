﻿using ProjetASPMVC1.Models;
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
        Projet_ContextBD db = new Projet_ContextBD();
        public static String id;
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
                    id = (string)Session["CIN"];
                    return RedirectToAction("Edit", "Candidats");
                }
            }

        }
        public ActionResult Creation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Creation(string CIN, string CNE, string prenom, string sexe, string password, string photo, string statut, string niveau) 
        {
            Candidat e = new Candidat();
            e.CIN = CIN;
            e.CNE = CNE;
            e.prenom = prenom;
            e.sexe = sexe;
            e.password = password;
            e.photo = photo;
            e.statut = statut;
            e.niveau = niveau;
            db.Candidats.Add(e);

            db.SaveChanges();
            ViewBag.msg = "Bien Ajoutee";
            return View();
        }
        public ActionResult LogOut()
        {
            int userId = (int)Session["CIN"];
            Session.Abandon();
            return RedirectToAction("Index", "Home");
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

        public ActionResult Edit(String id)

        {
            
            
            Candidat candidat = db.Candidats.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", candidat.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", candidat.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", candidat.id_note);
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
    }
}