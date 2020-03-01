using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetASPMVC1.Models;

namespace ProjetASPMVC1.Controllers
{
    public class CandidatsController : Controller
    {
        private Projet_ContextBD db = new Projet_ContextBD();

        // GET: Candidats
        public ActionResult Index()
        {
            var etudiants = db.Etudiants.Include(c => c.Diplome).Include(c => c.Filiere).Include(c => c.Notes);
            return View(etudiants.ToList());
        }


      
        public ActionResult Edit(string id)

        {
           
            Candidat candidat = db.Etudiants.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", candidat.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", candidat.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", candidat.id_note);
            return View(candidat);
        }

        

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CIN,CNE,prenom,sexe,password,photo,statut,niveau,date_naiss,id_fil,id_note,id_diplome")] Candidat candidat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", candidat.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", candidat.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", candidat.id_note);
            return View(candidat);
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
