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
    public class Candidats1Controller : Controller
    {
        private Projet_ContextBD db = new Projet_ContextBD();

        // GET: Candidats1
        public ActionResult Index()
        {
            var candidats = db.Candidats.Include(c => c.Diplome).Include(c => c.Filiere).Include(c => c.Notes);
            return View(candidats.ToList());
        }

        // GET: Candidats1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidat candidat = db.Candidats.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            return View(candidat);
        }

        // GET: Candidats1/Create
        public ActionResult Create()
        {
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome");
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil");
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note");
            return View();
        }

        // POST: Candidats1/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CIN,CNE,prenom,nom,ville,addresse,tel,GSM,type_bac,annee_bac,note_bac,mention_bac,n_dossier,nationnalite,sexe,cont_sup,cont_ajout,password,password_conf,photo,email,EmailConfirmed,statut,niveau,date_naiss,id_fil,id_note,id_diplome")] Candidat candidat)
        {
            if (ModelState.IsValid)
            {
                db.Candidats.Add(candidat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", candidat.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", candidat.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", candidat.id_note);
            return View(candidat);
        }

        // GET: Candidats1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

        // POST: Candidats1/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CIN,CNE,prenom,nom,ville,addresse,tel,GSM,type_bac,annee_bac,note_bac,mention_bac,n_dossier,nationnalite,sexe,cont_sup,cont_ajout,password,password_conf,photo,email,EmailConfirmed,statut,niveau,date_naiss,id_fil,id_note,id_diplome")] Candidat candidat)
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

        // GET: Candidats1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidat candidat = db.Candidats.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            return View(candidat);
        }

        // POST: Candidats1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Candidat candidat = db.Candidats.Find(id);
            db.Candidats.Remove(candidat);
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
