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
    public class corbeilsController : Controller
    {
        private Projet_ContextBD db = new Projet_ContextBD();

        // GET: corbeils
        public ActionResult Index()
        {
            var corbeils = db.corbeils.Include(c => c.Diplome).Include(c => c.Filiere).Include(c => c.Notes);
            return View(corbeils.ToList());
        }

        // GET: corbeils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            corbeil corbeil = db.corbeils.Find(id);
            if (corbeil == null)
            {
                return HttpNotFound();
            }
            return View(corbeil);
        }

        // GET: corbeils/Create
        public ActionResult Create()
        {
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome");
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil");
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note");
            return View();
        }

        // POST: corbeils/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CIN,CNE,prenom,nom,ville,addresse,GSM,type_bac,annee_bac,note_bac,mention_bac,n_dossier,nationnalite,sexe,cont_sup,cont_ajout,password,password_conf,photo,email,EmailConfirmed,statut,convocu,niveau,date_naiss,id_fil,id_note,id_diplome")] corbeil corbeil)
        {
            if (ModelState.IsValid)
            {
                db.corbeils.Add(corbeil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", corbeil.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", corbeil.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", corbeil.id_note);
            return View(corbeil);
        }

        // GET: corbeils/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            corbeil corbeil = db.corbeils.Find(id);
            if (corbeil == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", corbeil.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", corbeil.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", corbeil.id_note);
            return View(corbeil);
        }

        // POST: corbeils/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CIN,CNE,prenom,nom,ville,addresse,GSM,type_bac,annee_bac,note_bac,mention_bac,n_dossier,nationnalite,sexe,cont_sup,cont_ajout,password,password_conf,photo,email,EmailConfirmed,statut,convocu,niveau,date_naiss,id_fil,id_note,id_diplome")] corbeil corbeil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corbeil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_diplome = new SelectList(db.Diplomes, "id_diplome", "nom_diplome", corbeil.id_diplome);
            ViewBag.id_fil = new SelectList(db.Filieres, "id_fil", "nom_fil", corbeil.id_fil);
            ViewBag.id_note = new SelectList(db.Notes, "id_note", "id_note", corbeil.id_note);
            return View(corbeil);
        }

        // GET: corbeils/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            corbeil corbeil = db.corbeils.Find(id);
            if (corbeil == null)
            {
                return HttpNotFound();
            }
            return View(corbeil);
        }

        // POST: corbeils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            corbeil corbeil = db.corbeils.Find(id);
            db.corbeils.Remove(corbeil);
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
