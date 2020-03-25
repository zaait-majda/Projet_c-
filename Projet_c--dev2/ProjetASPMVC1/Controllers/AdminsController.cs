using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjetASPMVC1.Models;

namespace ProjetASPMVC1.Controllers
{
    public class AdminsController : Controller
    {
        private Projet_ContextBD db = new Projet_ContextBD();

        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorise(string login, string password)
        {
            //if (ModelState.IsValid)
            //    {
            using (Projet_ContextBD db = new Projet_ContextBD())
            {

                var userDetail = db.Admins.Where(x => x.Login == login && x.password == password).FirstOrDefault();
                if (userDetail == null)
                {

                    // user.LoginErrorMsg = "Invalid UserName or Password";
                    Response.Write("<script>alert(\'données incorrect\');</" + "script>");
                    return View("LoginAdmin");
                }
                else
                {

                    Session["login"] = userDetail.Login;
                    Session["password"] = userDetail.password;
                    int nb3eme = 0;
                    int nb4eme = 0;
                    int nbIns = 0;
                    foreach (var cand in db.Candidats.ToArray())
                    {
                        if (cand.niveau.Equals("4eme"))
                        {
                            nb4eme++;
                        }
                        else if (cand.niveau.Equals("3eme"))
                        {
                            nb3eme++;
                        }

                        nbIns++;
                    }
                    int msgnonVue = 0;
                    foreach (var msg in db.message.ToList())
                    {
                        if (msg.vue.Equals(0))
                        {
                            msgnonVue++;
                        }

                    }
                    ViewBag.Msg = Convert.ToString(msgnonVue);
                    ViewBag.niv3 = Convert.ToString(nb3eme);
                    ViewBag.niv4 = Convert.ToString(nb4eme);
                    ViewBag.ut = Convert.ToString(nbIns.ToString());

                    return View("Index");

                }

            }






        }

        // chart 4eme
        public ActionResult MyChartNiveau4()
        {

            var x = from p in db.Filieres select p.nom_fil;
            var x2 = from p in db.Filieres select p;
            var y = from p in db.Candidats select p;

            List<int> cmpt = new List<int>();
            List<string> fil = new List<string>();




            foreach (var t2 in x2)
            {
                int c = 0;
                foreach (var t in y)
                {

                    if (t2.id_fil == t.id_fil && t.niveau.Equals("4eme"))
                    {

                        c++;
                    }

                }


                cmpt.Add(c);
            }

            foreach (var t2 in x)
            {

                fil.Add(t2.ToString());

            }

            new System.Web.Helpers.Chart(width: 800, height: 200).AddTitle("Nombre des Candidats de 4eme année par filiere").AddSeries(chartType: "Column", xValue: fil, yValues: cmpt).Write("png");

            return null;
        }
        //chart de 3eme année

        public ActionResult MyChartNiveau3()
        {

            var x = from p in db.Filieres select p.nom_fil;
            var x2 = from p in db.Filieres select p;
            var y = from p in db.Candidats select p;

            List<int> cmpt = new List<int>();
            List<string> fil = new List<string>();




            foreach (var t2 in x2)
            {
                int c = 0;
                foreach (var t in y)
                {

                    if (t2.id_fil == t.id_fil && t.niveau.Equals("3eme"))
                    {

                        c++;
                    }

                }


                cmpt.Add(c);
            }

            foreach (var t2 in x)
            {

                fil.Add(t2.ToString());

            }

            new System.Web.Helpers.Chart(width: 800, height: 200).AddTitle("Nombre des Candidats de 3eme année par filiere").AddSeries(chartType: "Column", xValue: fil, yValues: cmpt).Write("png");
            return null;
        }

        // GET: Admins
        public ActionResult Index(string id)
        {
            int nb3eme = 0;
            int nb4eme = 10;
            int nbIns = 0;
            int msgnonVue = 0;
            foreach (var cand in db.Candidats.ToArray())
            {
                if (cand.niveau.Equals("4eme"))
                {
                    nb4eme++;
                }
                else if (cand.niveau.Equals("3eme"))
                {
                    nb3eme++;
                }

                nbIns++;
            }
            
            foreach(var msg in db.message.ToList())
            {
                if (msg.vue.Equals(0))
                {
                    msgnonVue++;
                }

            }
            ViewBag.Msg = Convert.ToString(msgnonVue);
            ViewBag.niv3 = Convert.ToString(nb3eme);
            ViewBag.niv4 = Convert.ToString(nb4eme);
            ViewBag.ut = Convert.ToString(nbIns.ToString());
            return View();
        }
    
        public ActionResult Convoquer()
        {
            int msgnonVue = 0;
            foreach (var msg in db.message.ToList())
            {
                if (msg.vue.Equals(0))
                {
                    msgnonVue++;
                }

            }
            ViewBag.Msg = Convert.ToString(msgnonVue);
            return View();
        }

        public ActionResult Convoquer4eme()
        {
            int msgnonVue = 0;
            foreach (var msg in db.message.ToList())
            {
                if (msg.vue.Equals(0))
                {
                    msgnonVue++;
                }

            }
            ViewBag.Msg = Convert.ToString(msgnonVue);

            return View();
        }



        // enregitrer candidat 3eme annnée

        [HttpPost]
        public PartialViewResult ConvoqueDossier(string CIN)
        {

            Candidat et = db.Candidats.Find(CIN);
            if (et != null && et.niveau.Equals("3eme"))
            {
                if (et.n_dossier.Equals(""))
                {
                    Random rnd = new Random();

                    rnd.Next(1, 100);
                    et.n_dossier = "M" + Convert.ToString(et.CNE) + "end";
                    db.SaveChanges();
                    return PartialView("_ConvoqueDossier", et);

                }
                else
                {
                    Response.Write("<script>alert(\'Erreur le Candidat déja convoquer CIN non valide !!!!!\');</" + "script>");
                    return PartialView("_ConvoqueDossierErr", et);
                }
            }
            Response.Write("<script>alert(\'ce candidat en 4eme année  attention!!!!!\');</" + "script>");
            return PartialView("_ConvoqueDossierErr", et);


        }
   



       // enregitrer candidat 4eme annnée

       [HttpPost]
        public PartialViewResult ConvoqueDossier4eme(string CIN)
        {

            Candidat et = db.Candidats.Find(CIN);
            if (et != null && et.niveau.Equals("4eme"))
            {
                if (et.n_dossier.Equals(""))
                {
                    Random rnd = new Random();

                    rnd.Next(1, 100);
                    et.n_dossier = "M" + Convert.ToString(et.CNE) + "end";
                    db.SaveChanges();
                    return PartialView("_ConvoqueDossier", et);

                }
                else
                {
                    Response.Write("<script>alert(\'Erreur le Candidat déja convoquer CIN non valide !!!!!\');</" + "script>");
                    return PartialView("_ConvoqueDossierErr", et);
                }
            }
            Response.Write("<script>alert(\'ce candidat en 3eme année attention !!!!!\');</" + "script>");
            return PartialView("_ConvoqueDossierErr", et);


        }

     
        public ActionResult LogOut()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
