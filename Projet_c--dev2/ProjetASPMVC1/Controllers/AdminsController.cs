using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ClosedXML.Excel;
using Newtonsoft.Json;
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
                    ViewBag.sup = Convert.ToString(db.corbeils.Count());
                    return View("Index");

                }

            }






        }
        /*
         *
         *Partie Deliberation
         * 
         */
        public ActionResult Deliberation3eme()
        {
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            return View();
        }
        public ActionResult Deliberation4eme()
        {
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            return View();
        }
        public List<Candidat> sortCandidat(List<Candidat> cd, int choix)
        {
            for (int i = 0; i < cd.Count - 1; i++)
            {
                int index = i;
                for (int j = i + 1; j < cd.Count; j++)
                {
                    if (cd[j].Notes.note_concours == cd[index].Notes.note_concours)
                    {
                        //Tester sur le choix de deuxième critere
                        if (choix == 2)
                        {
                            if (cd[j].Notes.notemath > cd[index].Notes.notemath)
                            {
                                index = j;
                                break;
                            }
                            if(cd[j].Notes.notemath == cd[index].Notes.notemath)
                            {
                                if (Int32.Parse(cd[j].note_bac) > Int32.Parse(cd[index].note_bac))
                                {
                                    index = j;
                                    break;
                                }
                            }
                          
                        }
                        //Tester sur le choix de troisième critere
                        if (choix == 3)
                        {
                            if (Int32.Parse(cd[j].note_bac) > Int32.Parse(cd[index].note_bac))
                            {
                                index = j;
                                  break;
                            }
                            if (Int32.Parse(cd[j].note_bac) == Int32.Parse(cd[index].note_bac))
                            {
                                if (cd[j].Notes.notemath > cd[index].Notes.notemath)
                                {
                                    index = j;
                                    break;
                                }
                            }
                        }

                    }
                }
                //echange
                //candidat avec critere de valeur plus grand <=> celui le plus petit
                var HighetCand = cd[index];
                cd[index] = cd[i];
                cd[i] = HighetCand;
             
            }
            return cd;
        }


        public ActionResult ResultatDelib3eme(int id_fil,int coeff_math,int coeff_specialite,int nbr_places,int list_att,int note_min,int choix1, int choix2, int choix3)
        {
             List<Candidat> admis_principal = new List<Candidat>();
             List<Candidat> admis_att = new List<Candidat>();
            List<Candidat> candidats = db.Candidats.Where(p => p.niveau == "3eme").Where(p=>p.statut== "pres").Where(p => p.id_fil == id_fil
            ).ToList();
             if (candidats.Count() != 0)
            {
               foreach(var cand in candidats)
                 {
                     cand.Notes.note_concours = ((double)cand.Notes.notemath * coeff_math+ (double)cand.Notes.notespec*coeff_specialite)/(coeff_math+coeff_specialite);
                     db.SaveChanges();
                 }
                candidats = candidats.AsQueryable().Where(p => p.Notes.note_concours > note_min).ToList();
                //1eme critere à choisir
                 switch (choix1)
                 {
                     case 1:
                         candidats=candidats.AsQueryable().OrderByDescending(p => p.Notes.note_concours).ToList();
                         break;
                     case 2:
                        candidats = candidats.AsQueryable().OrderByDescending(p => p.Notes.notemath).ToList();
                         break;
                     case 3:
                        candidats = candidats.AsQueryable().OrderByDescending(p => p.Notes.notespec).ToList();
                         break;

                     default:
                         break;
                 };
                //
                candidats = sortCandidat(candidats, choix2);
                Session["candidatsDelib"] = candidats;

             

                 for (int i = 0; i < nbr_places; i++)
                  {
                    Candidat cand = null;
                    try
                    {
                        cand = candidats.ElementAt(i);
                       

                    }
                    catch (System.ArgumentOutOfRangeException e)  
                    {
                        break;
                    }
                    
                    admis_principal.Add(cand);
                      cand.statut = "ADMIS_PR";
                      db.SaveChanges();

                }
                  for (int i = nbr_places; i < list_att+nbr_places; i++)
                  {
                    Candidat cand = null;
                    try
                    {
                        cand = candidats.ElementAt(i);


                    }
                    catch (System.ArgumentOutOfRangeException e) 
                    {
                        break;
                    }
                    admis_att.Add(cand);
                      cand.statut = "ADMIS_Att";
                       db.SaveChanges();

                }
                Session["admispr"] = admis_principal;
                Session["admisatt"] = admis_att;
                ViewBag.annee = candidats.ElementAt(0).niveau;
                ViewBag.id_fil = id_fil;
                ViewBag.Filiere = db.Filieres.Find(id_fil).nom_fil;
                ViewBag.att = admis_att.Count;
                ViewBag.attratio = (int)(((double)admis_att.Count/ (double)candidats.Count)*100);
                ViewBag.principal = admis_principal.Count;
                ViewBag.principalratio = (int)(((double)admis_principal.Count / (double)candidats.Count) * 100);
                ViewBag.total = candidats.Count;
                ViewBag.coefMath = coeff_math;
                ViewBag.coefspec = coeff_specialite;
                ViewBag.noteMin = note_min;
                ViewBag.error = "";
                return View("ResultatDeliberation", candidats);
            }
            else
            {
                ViewBag.error = "empty";
                return View("ResultatDeliberation");
            }

        


        }
        
        public ActionResult candidatNonConforme(string cin)
        {
            List<Candidat>  candidats = (List < Candidat > )Session["candidatsDelib"];
            Candidat candidat = null;
            foreach(var cnd in candidats)
            {
               if(cnd.CIN == cin)
                {
                    var admis = db.Candidats.Find(cin);
                    admis.statut = "NON_Conforme";
                    db.SaveChanges();
                    candidat = cnd;
                    break;
                }
            }
            List<Candidat> admis_principal = (List<Candidat>)Session["admispr"];
            List<Candidat> admis_att = (List<Candidat>)Session["admisatt"];
            admis_att.Remove(candidat);
            admis_principal.Remove(candidat);
            ViewBag.error = "";
            candidats.Remove(candidat);
            return View("ResultatDeliberation", candidats);
        }
        public ActionResult ListAdmis3emeDelib()
        {
            List<Candidat> admisprincipal = (List<Candidat>)Session["admispr"];
            ViewBag.filiere = admisprincipal.ElementAt(0).Filiere.nom_fil;
            ViewBag.annee = admisprincipal.ElementAt(0).niveau;
            ViewBag.admisPR = admisprincipal;
            ViewBag.admisATT = (List<Candidat>)Session["admisatt"];
            
            return View("ListAdmisDelib");
        }
        public ActionResult ListAdmission3eme()
        {
            ViewBag.list = "";
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");
            return View();
        }
        [HttpPost]
        public ActionResult ListAdmission3emeResult(int? id_fil)
        {
            ViewBag.list = "list";
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");
            ViewBag.filiere= db.Filieres.Find(id_fil).nom_fil;
            ViewBag.admisPR = db.Candidats.Where(p => p.niveau == "3eme").Where(p => p.statut == "ADMIS_PR").Where(p => p.id_fil == id_fil
            ).ToList();
            ViewBag.admisATT = db.Candidats.Where(p => p.niveau == "3eme").Where(p => p.statut == "ADMIS_Att").Where(p => p.id_fil == id_fil
           ).ToList();
            return View("ListAdmission3eme");
        }
        public ActionResult deleberationResult4eme()
        {

            return View();
        }
        public ActionResult ResultatDelib4eme(int id_fil, int coeff_math, int coeff_specialite, int nbr_places, int list_att, int note_min, int choix1, int choix2, int choix3)
        {
            List<Candidat> admis_principal = new List<Candidat>();
            List<Candidat> admis_att = new List<Candidat>();
            List<Candidat> candidats = db.Candidats.Where(p => p.niveau == "4eme").Where(p => p.statut == "pres").Where(p => p.id_fil == id_fil
            ).ToList();
            if (candidats.Count() != 0)
            {
                foreach (var cand in candidats)
                {
                    cand.Notes.note_concours = ((double)cand.Notes.notemath * coeff_math + (double)cand.Notes.notespec * coeff_specialite) / (coeff_math + coeff_specialite);
                    db.SaveChanges();
                }
                candidats = candidats.AsQueryable().Where(p => p.Notes.note_concours > note_min).ToList();
                switch (choix1)
                {
                    case 1:
                        candidats = candidats.AsQueryable().OrderByDescending(p => p.Notes.note_concours).ToList();
                        break;
                    case 2:
                        candidats = candidats.AsQueryable().OrderByDescending(p => p.Notes.notemath).ToList();
                        break;
                    case 3:
                        candidats = candidats.AsQueryable().OrderByDescending(p => p.Notes.notespec).ToList();
                        break;

                    default:
                        break;
                };

                candidats = sortCandidat(candidats, choix2);
                Session["candidatsDelib"] = candidats;


                for (int i = 0; i < nbr_places; i++)
                {
                    Candidat cand = null;
                    try
                    {
                        cand = candidats.ElementAt(i);


                    }
                    catch (System.ArgumentOutOfRangeException e)
                    {
                        break;
                    }

                    admis_principal.Add(cand);
                    cand.statut = "ADMIS_PR";
                    db.SaveChanges();

                }
                for (int i = nbr_places; i < list_att + nbr_places; i++)
                {
                    Candidat cand = null;
                    try
                    {
                        cand = candidats.ElementAt(i);
                    }
                    catch (System.ArgumentOutOfRangeException e) 
                    {
                        break;
                    }
                    admis_att.Add(cand);
                    cand.statut = "ADMIS_Att";
                    db.SaveChanges();

                }
                Session["admispr"] = admis_principal;
                Session["admisatt"] = admis_att;
                ViewBag.id_fil = id_fil;
                ViewBag.Filiere = db.Filieres.Find(id_fil).nom_fil;
                ViewBag.att = admis_att.Count;
                ViewBag.attratio = (int)(((double)admis_att.Count / (double)candidats.Count) * 100);
                ViewBag.principal = admis_principal.Count;
                ViewBag.principalratio = (int)(((double)admis_principal.Count / (double)candidats.Count) * 100);
                ViewBag.total = candidats.Count;
                ViewBag.coefMath = coeff_math;
                ViewBag.coefspec = coeff_specialite;
                ViewBag.noteMin = note_min;
                ViewBag.error = "";
                return View("ResultatDeliberation",candidats);
            }
            else
            {
                ViewBag.error = "empty";
                return View("ResultatDeliberation");
            }




        }

        public ActionResult ListAdmis4emeDelib()
        {
            List<Candidat> admisprincipal = (List<Candidat>)Session["admispr"];
            ViewBag.filiere = admisprincipal.ElementAt(0).Filiere.nom_fil;
            ViewBag.annee = admisprincipal.ElementAt(0).niveau;
            ViewBag.admisPR = admisprincipal;
            ViewBag.admisATT = (List<Candidat>)Session["admisatt"];

            return View("ListAdmisDelib");

        }
        public ActionResult ListAdmission4eme()
        {
            ViewBag.list = "";
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");
            return View();
        }
        [HttpPost]
        public ActionResult ListAdmission4emeResult(int? id_fil)
        {
            ViewBag.list = "list";
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");
            ViewBag.filiere = db.Filieres.Find(id_fil).nom_fil;
            ViewBag.admisPR = db.Candidats.Where(p => p.niveau == "4eme").Where(p => p.statut == "ADMIS_PR").Where(p => p.id_fil == id_fil
            ).ToList();
            ViewBag.admisATT = db.Candidats.Where(p => p.niveau == "4eme").Where(p => p.statut == "ADMIS_Att").Where(p => p.id_fil == id_fil
           ).ToList();
            return View("ListAdmission4eme");
        }




        /*
        *
        * Partie Deliberation__END
        */

        /*
          * begin
          * Partie statistique des admis de 3 annes
          */
        public ActionResult statistiqueDesAdmis3Annee()
        {
            //pour les admis
            List<DataPoint> dataPointsForInfoAdmis = new List<DataPoint>();
            List<DataPoint> dataPointsForIndusAdmis = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmcAdmis = new List<DataPoint>();
            List<DataPoint> dataPointsForGtrAdmis = new List<DataPoint>();
            //pour les atente
            List<DataPoint> dataPointsForInfoAttente = new List<DataPoint>();
            List<DataPoint> dataPointsForIndusAttente = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmcAttente = new List<DataPoint>();
            List<DataPoint> dataPointsForGtrAttente = new List<DataPoint>();
            //pour les admis
            var infoAdmis = new SortedList<string, int>();
            var indusAdmis = new SortedList<string, int>();
            var gpmcAdmis = new SortedList<string, int>();
            var gtrAdmis = new SortedList<string, int>();
            //pour les attent
            var infoAttente = new SortedList<string, int>();
            var indusAttente = new SortedList<string, int>();
            var gpmcAttente = new SortedList<string, int>();
            var gtrAttente = new SortedList<string, int>();

            var query = (from candidat in db.Candidats
                         join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                         join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                         where candidat.statut.Equals("ADMIS_PR") && candidat.niveau.Equals("3eme")
                         select new
                         {
                             nomFiliere = filiere.nom_fil,
                             nomDiplome = diplome.nom_diplome,

                         }).ToList();
            var queryAttente = (from candidat in db.Candidats
                                join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                                join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                                where candidat.statut.Equals("ADMIS_Att") && candidat.niveau.Equals("3eme")
                                select new
                                {
                                    nomFiliere = filiere.nom_fil,
                                    nomDiplome = diplome.nom_diplome,

                                }).ToList();

            foreach (var candidat in query)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (infoAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        infoAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        infoAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indusAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        indusAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indusAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtrAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        gtrAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtrAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.ToLower().Contains("gpmc")))
                {
                    if (gpmcAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        gpmcAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmcAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            // pour les attents
            foreach (var candidat in queryAttente)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (infoAttente.ContainsKey(candidat.nomDiplome))
                    {
                        infoAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        infoAttente.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indusAttente.ContainsKey(candidat.nomDiplome))
                    {
                        indusAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indusAttente.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtrAttente.ContainsKey(candidat.nomDiplome))
                    {
                        gtrAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtrAttente.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.Contains("gpmc")))
                {
                    if (gpmcAttente.ContainsKey(candidat.nomDiplome))
                    {
                        gpmcAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmcAttente.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            foreach (var i in infoAdmis)
            {
                dataPointsForInfoAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indusAdmis)
            {
                dataPointsForIndusAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmcAdmis)
            {
                dataPointsForGpmcAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtrAdmis)
            {
                dataPointsForGtrAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            //pour la liste d'attente
            foreach (var i in infoAttente)
            {
                dataPointsForInfoAttente.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indusAttente)
            {
                dataPointsForIndusAttente.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmcAttente)
            {
                dataPointsForGpmcAttente.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtrAttente)
            {
                dataPointsForGtrAttente.Add(new DataPoint(i.Key, i.Value));
            }
            //passer les donnees aux views

            ViewBag.dataPointsForInfoAdmis = JsonConvert.SerializeObject(dataPointsForInfoAdmis);
            ViewBag.dataPointsForIndusAdmis = JsonConvert.SerializeObject(dataPointsForIndusAdmis);
            ViewBag.dataPointsForGpmcAdmis = JsonConvert.SerializeObject(dataPointsForGpmcAdmis);
            ViewBag.dataPointsForGtrAdmis = JsonConvert.SerializeObject(dataPointsForGtrAdmis);
            // attent
            ViewBag.dataPointsForInfoAttente = JsonConvert.SerializeObject(dataPointsForInfoAttente);
            ViewBag.dataPointsForIndusAttente = JsonConvert.SerializeObject(dataPointsForIndusAttente);
            ViewBag.dataPointsForGpmcAttente = JsonConvert.SerializeObject(dataPointsForGpmcAttente);
            ViewBag.dataPointsForGtrAttente = JsonConvert.SerializeObject(dataPointsForGtrAttente);
            return View("statistique_Des_Admis_En_3_Anees");
        }
        public ActionResult statistiqueDesAdmis4Annee()
        {
            List<DataPoint> dataPointsForInfoAdmis = new List<DataPoint>();
            List<DataPoint> dataPointsForIndusAdmis = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmcAdmis = new List<DataPoint>();
            List<DataPoint> dataPointsForGtrAdmis = new List<DataPoint>();
            //pour les atente
            List<DataPoint> dataPointsForInfoAttente = new List<DataPoint>();
            List<DataPoint> dataPointsForIndusAttente = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmcAttente = new List<DataPoint>();
            List<DataPoint> dataPointsForGtrAttente = new List<DataPoint>();
            //pour les admis
            var infoAdmis = new SortedList<string, int>();
            var indusAdmis = new SortedList<string, int>();
            var gpmcAdmis = new SortedList<string, int>();
            var gtrAdmis = new SortedList<string, int>();
            //pour les attent
            var infoAttente = new SortedList<string, int>();
            var indusAttente = new SortedList<string, int>();
            var gpmcAttente = new SortedList<string, int>();
            var gtrAttente = new SortedList<string, int>();

            var query = (from candidat in db.Candidats
                         join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                         join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                         where candidat.statut.Equals("ADMIS_PR") && candidat.niveau.Equals("4eme")
                         select new
                         {
                             nomFiliere = filiere.nom_fil,
                             nomDiplome = diplome.nom_diplome,

                         }).ToList();
            var queryAttente = (from candidat in db.Candidats
                                join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                                join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                                where candidat.statut.Equals("ADMIS_Att") && candidat.niveau.Equals("4eme")
                                select new
                                {
                                    nomFiliere = filiere.nom_fil,
                                    nomDiplome = diplome.nom_diplome,

                                }).ToList();

            foreach (var candidat in query)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (infoAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        infoAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        infoAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indusAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        indusAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indusAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtrAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        gtrAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtrAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.ToLower().Contains("gpmc")))
                {
                    if (gpmcAdmis.ContainsKey(candidat.nomDiplome))
                    {
                        gpmcAdmis[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmcAdmis.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            // pour les attents
            foreach (var candidat in queryAttente)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (infoAttente.ContainsKey(candidat.nomDiplome))
                    {
                        infoAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        infoAttente.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indusAttente.ContainsKey(candidat.nomDiplome))
                    {
                        indusAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indusAttente.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtrAttente.ContainsKey(candidat.nomDiplome))
                    {
                        gtrAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtrAttente.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.Contains("gpmc")))
                {
                    if (gpmcAttente.ContainsKey(candidat.nomDiplome))
                    {
                        gpmcAttente[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmcAttente.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            foreach (var i in infoAdmis)
            {
                dataPointsForInfoAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indusAdmis)
            {
                dataPointsForIndusAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmcAdmis)
            {
                dataPointsForGpmcAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtrAdmis)
            {
                dataPointsForGtrAdmis.Add(new DataPoint(i.Key, i.Value));
            }
            //pour la liste d'attente
            foreach (var i in infoAttente)
            {
                dataPointsForInfoAttente.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indusAttente)
            {
                dataPointsForIndusAttente.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmcAttente)
            {
                dataPointsForGpmcAttente.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtrAttente)
            {
                dataPointsForGtrAttente.Add(new DataPoint(i.Key, i.Value));
            }
            //passer les donnees aux views

            ViewBag.dataPointsForInfoAdmis = JsonConvert.SerializeObject(dataPointsForInfoAdmis);
            ViewBag.dataPointsForIndusAdmis = JsonConvert.SerializeObject(dataPointsForIndusAdmis);
            ViewBag.dataPointsForGpmcAdmis = JsonConvert.SerializeObject(dataPointsForGpmcAdmis);
            ViewBag.dataPointsForGtrAdmis = JsonConvert.SerializeObject(dataPointsForGtrAdmis);
            // attent
            ViewBag.dataPointsForInfoAttente = JsonConvert.SerializeObject(dataPointsForInfoAttente);
            ViewBag.dataPointsForIndusAttente = JsonConvert.SerializeObject(dataPointsForIndusAttente);
            ViewBag.dataPointsForGpmcAttente = JsonConvert.SerializeObject(dataPointsForGpmcAttente);
            ViewBag.dataPointsForGtrAttente = JsonConvert.SerializeObject(dataPointsForGtrAttente);
            return View("statistique_Des_Admis_En_4_Anees");
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
            int nb4eme = 0;
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
            ViewBag.sup = Convert.ToString(db.corbeils.Count());
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
                if (et.n_dossier.Equals("0") || et.n_dossier.Equals(null))
                {
                    Random rnd = new Random();

                    rnd.Next(1, 100);
                    et.n_dossier = "M" + Convert.ToString(et.CNE) + "end";
                    db.SaveChanges();
                    return PartialView("_ConvoqueDossier", et);

                }
                else
                {
                    ViewBag.msgE = "Numero de dossier déja attribué  " + et.n_dossier;
                    Response.Write("<script>alert(\'Erreur le Candidat déja convoquer CIN non valide !!!!!\');</" + "script>");

                    return PartialView("_ConvoqueDossier", et);
                }
            }
            Response.Write("<script>alert(\'ce candidat n'existe pas en 3eme annee  attention!!!!!\');</" + "script>");
            return PartialView("_ConvoqueDossierErr", et);


        }




        // enregitrer candidat 4eme annnée

        [HttpPost]
        public PartialViewResult ConvoqueDossier4eme(string CIN)
        {

            Candidat et = db.Candidats.Find(CIN);
            if (et != null && et.niveau.Equals("4eme"))
            {
                if (et.n_dossier.Equals("0") || et.n_dossier.Equals(null))
                {
                    Random rnd = new Random();

                    rnd.Next(1, 100);
                    et.n_dossier = "M" + Convert.ToString(et.CNE) + "end";
                    db.SaveChanges();

                    return PartialView("_ConvoqueDossier", et);

                }
                else
                {
                    ViewBag.msgE = "Numero de dossier déja attribué  " + et.n_dossier;
                    Response.Write("<script>alert(\'Erreur le Candidat déja convoquer CIN non valide !!!!!\');</" + "script>");
                    return PartialView("_ConvoqueDossier", et);
                }
            }
            Response.Write("<script>alert(\'ce candidat n'existe pas en 4 eme annee!!!!!\');</" + "script>");
            return PartialView("_ConvoqueDossierErr", et);


        }


        public ActionResult preselection()
        {
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "id_diplome", "nom_diplome");


            return View();

        }


        // preselection de 3 eme année

        [HttpPost]
        public ActionResult preselectionCalcule(int id_fil, string nom_dip, double n1, int n2, int n3, int n4, int n5, int n6, int bac, double seuil)
        {

            List<Candidat> list = new List<Candidat>();


            Session["id_fil"] = id_fil;
            Session["nom_dip"] = nom_dip;
            Session["n1"] = n1;
            Session["n2"] = n2;
            Session["n3"] = n3;
            Session["n4"] = n4;
            Session["n5"] = n5;
            Session["n6"] = n6;
            Session["bac"] = bac;
            Session["seuil"] = seuil;
            Session["id_fil3"] = id_fil;
            foreach (var etu in db.Candidats.ToList().Where(p => p.niveau == "3eme"))
            {
                double? somme = 0.0;
                if (etu.id_fil == id_fil)
                {
                    foreach (var dep in db.Diplomes.ToList())
                    {
                        if (etu.id_diplome == dep.id_diplome)
                        {
                            if (dep.nom_diplome.Equals(nom_dip))
                            {
                                foreach (var nts in db.Notes.ToList())
                                {
                                    if (etu.id_note == nts.id_note)
                                    {

                                        somme = ((n1 * nts.s1) + (n2 * nts.s2) + (nts.s3 * n3) + (nts.s4 * n4) + (nts.s5 * n5) + (nts.s6 * n6) + (bac * Convert.ToDouble(etu.note_bac))) / (n1 + n2 + n3 + n4 + n5 + n6);
                                        if (somme >= seuil)
                                        {

                                            //db.Candidats.Find(etu.CIN).convocu = true;
                                            db.Candidats.Find(etu.CIN).statut = "pres";
                                            db.SaveChanges();

                                            list.Add(etu);
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
            }

            return View(list);

        }


        //fnt pour la selection de preselection 'excel' 3eme annee
        public IList<Candidat> preselectionCalculeget(int id_fil, string nom_dip, double n1, int n2, int n3, int n4, int n5, int n6, int bac, double seuil)
        {

            IList<Candidat> list = new List<Candidat>();

            //  IList <Candidat> etuli;
            Session["id_fil"] = id_fil;
            Session["nom_dip"] = nom_dip;
            Session["n1"] = n1;
            Session["n2"] = n2;
            Session["n3"] = n3;
            Session["n4"] = n4;
            Session["n5"] = n5;
            Session["n6"] = n6;
            Session["bac"] = bac;
            Session["seuil"] = seuil;

            foreach (var etu in db.Candidats.Where(p => p.niveau == "3eme"))
            {
                double? somme = 0.0;
                if (etu.id_fil == id_fil)
                {
                    foreach (var dep in db.Diplomes.ToList())
                    {
                        if (etu.id_diplome == dep.id_diplome)
                        {
                            if (dep.nom_diplome.Equals(nom_dip))
                            {
                                foreach (var nts in db.Notes.ToList())
                                {
                                    if (etu.id_note == nts.id_note)
                                    {

                                        somme = ((n1 * nts.s1) + (n2 * nts.s2) + (nts.s3 * n3) + (nts.s4 * n4) + (nts.s5 * n5) + (nts.s6 * n6) + (bac * Convert.ToDouble(etu.note_bac))) / (n1 + n2 + n3 + n4 + n5 + n6);
                                        if (somme >= seuil)
                                        {
                                            //db.Candidats.Find(etu.CIN).convocu = true;
                                            db.Candidats.Find(etu.CIN).statut = "pres";
                                            db.SaveChanges();
                                            list.Add(etu);
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
            }
         
            return list.ToList();

        }


        //export file excel 
        [HttpPost]
        public FileResult Export()
        {
            Projet_ContextBD entities = new Projet_ContextBD();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("CIN"),
                                            new DataColumn("CNE"),
                                            new DataColumn("prenom"),
                                            new DataColumn("nom"),
                                            new DataColumn("n_dossier")
            });

            var list_can = this.preselectionCalculeget(Convert.ToInt32(Session["id_fil"]), Convert.ToString(Session["nom_dip"]), Convert.ToDouble(Session["n1"]), Convert.ToInt32(Session["n2"]), Convert.ToInt32(Session["n3"]), Convert.ToInt32(Session["n4"]), Convert.ToInt32(Session["n5"]), Convert.ToInt32(Session["n6"]), Convert.ToInt32(Session["bac"]), Convert.ToDouble(Session["seuil"]));


            foreach (var cand3 in list_can)
            {
                dt.Rows.Add(cand3.CIN, cand3.CNE, cand3.prenom, cand3.nom, cand3.n_dossier);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Results3eme.xlsx");
                }
            }
        }
        /////////////////////4 eme /////////////////////////////////////////
        public ActionResult preselection4eme()
        {
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "id_diplome", "nom_diplome");

            return View();
        }

        [HttpPost]
        public ActionResult preselectionCalcule4eme(int id_fil, string nom_dip, double n1, int n2, int n3, int n4, int n5, int n6, int bac, double seuil)
        {

            List<Candidat> list = new List<Candidat>();


            Session["id_fil"] = id_fil;
            Session["nom_dip"] = nom_dip;
            Session["n1"] = n1;
            Session["n2"] = n2;
            Session["n3"] = n3;
            Session["n4"] = n4;
            Session["n5"] = n5;
            Session["n6"] = n6;
            Session["bac"] = bac;
            Session["seuil"] = seuil;
            Session["id_fil4"] = id_fil;
            foreach (var etu in db.Candidats.ToList().Where(p => p.niveau == "4eme"))
            {
                double? somme = 0.0;
                if (etu.id_fil == id_fil)
                {
                    foreach (var dep in db.Diplomes.ToList())
                    {
                        if (etu.id_diplome == dep.id_diplome)
                        {
                            if (dep.nom_diplome.Equals(nom_dip))
                            {
                                foreach (var nts in db.Notes.ToList())
                                {
                                    if (etu.id_note == nts.id_note)
                                    {

                                        somme = ((n1 * nts.s1) + (n2 * nts.s2) + (nts.s3 * n3) + (nts.s4 * n4) + (nts.s5 * n5) + (nts.s6 * n6) + (bac * Convert.ToDouble(etu.note_bac))) / (n1 + n2 + n3 + n4 + n5 + n6);
                                        if (somme >= seuil)
                                        {
                                            //db.Candidats.Find(etu.CIN).convocu = true;
                                            db.Candidats.Find(etu.CIN).statut = "pres";
                                            db.SaveChanges();
                                            list.Add(etu);
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
            }

            return View(list);

        }
        //fnt pour la selection de preselection 'excel' 4eme annee
        public IList<Candidat> preselectionCalculeget4eme(int id_fil, string nom_dip, double n1, int n2, int n3, int n4, int n5, int n6, int bac, double seuil)
        {

            IList<Candidat> list = new List<Candidat>();

            //  IList <Candidat> etuli;
            Session["id_fil"] = id_fil;
            Session["nom_dip"] = nom_dip;
            Session["n1"] = n1;
            Session["n2"] = n2;
            Session["n3"] = n3;
            Session["n4"] = n4;
            Session["n5"] = n5;
            Session["n6"] = n6;
            Session["bac"] = bac;
            Session["seuil"] = seuil;



            foreach (var etu in db.Candidats.Where(p => p.niveau == "4eme"))
            {
                double? somme = 0.0;
                if (etu.id_fil == id_fil)
                {
                    foreach (var dep in db.Diplomes.ToList())
                    {
                        if (etu.id_diplome == dep.id_diplome)
                        {
                            if (dep.nom_diplome.Equals(nom_dip))
                            {
                                foreach (var nts in db.Notes.ToList())
                                {
                                    if (etu.id_note == nts.id_note)
                                    {

                                        somme = ((n1 * nts.s1) + (n2 * nts.s2) + (nts.s3 * n3) + (nts.s4 * n4) + (nts.s5 * n5) + (nts.s6 * n6) + (bac * Convert.ToDouble(etu.note_bac))) / (n1 + n2 + n3 + n4 + n5 + n6);
                                        if (somme >= seuil)
                                        {
                                            //db.Candidats.Find(etu.CIN).convocu = true;
                                            db.Candidats.Find(etu.CIN).statut = "pres";
                                            db.SaveChanges();
                                            list.Add(etu);
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
            }

            return list.ToList();

        }


        //export file excel 
        [HttpPost]
        public FileResult Export4eme()
        {
            Projet_ContextBD entities = new Projet_ContextBD();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("CIN"),
                                            new DataColumn("CNE"),
                                            new DataColumn("prenom"),
                                            new DataColumn("nom"),
                                            new DataColumn("n_dossier")
            });

            var list_can = this.preselectionCalculeget4eme(Convert.ToInt32(Session["id_fil"]), Convert.ToString(Session["nom_dip"]), Convert.ToDouble(Session["n1"]), Convert.ToInt32(Session["n2"]), Convert.ToInt32(Session["n3"]), Convert.ToInt32(Session["n4"]), Convert.ToInt32(Session["n5"]), Convert.ToInt32(Session["n6"]), Convert.ToInt32(Session["bac"]), Convert.ToDouble(Session["seuil"]));


            foreach (var cand3 in list_can)
            {
                dt.Rows.Add(cand3.CIN, cand3.CNE, cand3.prenom, cand3.nom, cand3.n_dossier);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Results4eme.xlsx");
                }
            }
        }

        public ActionResult AfficheConvoque3()
        {
            List<Candidat> conv = new List<Candidat>();
            foreach (var v in db.Candidats.ToList())
            {

                if (v.id_fil == Convert.ToInt32(Session["id_fil3"]) && v.statut.Equals("pres") && v.niveau.Equals("3eme"))
                {
                    conv.Add(v);
                }
            }
            Filiere f = db.Filieres.Find(Convert.ToInt32(Session["id_fil3"]));
            ViewBag.fil = f.nom_fil;
            return View(conv);
        }

        public ActionResult AfficheConvoque4()
        {
            List<Candidat> conv = new List<Candidat>();
            foreach (var v in db.Candidats.ToList())
            {

                if (v.id_fil == Convert.ToInt32(Session["id_fil4"]) && v.statut.Equals("pres") && v.niveau.Equals("4eme"))
                {
                    conv.Add(v);
                }
            }
            Filiere f = db.Filieres.Find(Convert.ToInt32(Session["id_fil4"]));
            ViewBag.fil4 = f.nom_fil;
            return View(conv);
        }
        public ActionResult LogOut()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult getChartForPreselction3annee()
        {
            //for 4 annnee
            List<DataPoint> dataPointsForInfo = new List<DataPoint>();
            List<DataPoint> dataPointsForIndus = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmc = new List<DataPoint>();
            List<DataPoint> dataPointsForGtr = new List<DataPoint>();

            var info = new SortedList<string, int>();
            var indus = new SortedList<string, int>();
            var gpmc = new SortedList<string, int>();
            var gtr = new SortedList<string, int>();

            var query = (from candidat in db.Candidats
                         join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                         join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                         where candidat.statut.Equals("pres") && candidat.niveau.Equals("3eme")
                         select new
                         {
                             nomFiliere = filiere.nom_fil,
                             nomDiplome = diplome.nom_diplome,

                         }).ToList();

            foreach (var candidat in query)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (info.ContainsKey(candidat.nomDiplome))
                    {
                        info[candidat.nomDiplome]++;
                    }
                    else
                    {
                        info.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indus.ContainsKey(candidat.nomDiplome))
                    {
                        indus[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indus.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtr.ContainsKey(candidat.nomDiplome))
                    {
                        gtr[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtr.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.ToLower().Contains("gpmc")))
                {
                    if (gpmc.ContainsKey(candidat.nomDiplome))
                    {
                        gpmc[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmc.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            foreach (var i in info)
            {
                dataPointsForInfo.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indus)
            {
                dataPointsForIndus.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmc)
            {
                dataPointsForGpmc.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtr)
            {
                dataPointsForGtr.Add(new DataPoint(i.Key, i.Value));
            }

            ViewBag.dataPointsForInfo = JsonConvert.SerializeObject(dataPointsForInfo);
            ViewBag.dataPointsForIndus = JsonConvert.SerializeObject(dataPointsForIndus);
            ViewBag.dataPointsForGpmc = JsonConvert.SerializeObject(dataPointsForGpmc);
            ViewBag.dataPointsForGtr = JsonConvert.SerializeObject(dataPointsForGtr);
            return View("StatistiqueView");
        }
        //chart for 4 anneee


        public ActionResult getChartForPreselction4annee()
        {
            //for 4 annnee
            List<DataPoint> dataPointsForInfo = new List<DataPoint>();
            List<DataPoint> dataPointsForIndus = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmc = new List<DataPoint>();
            List<DataPoint> dataPointsForGtr = new List<DataPoint>();

            var info = new SortedList<string, int>();
            var indus = new SortedList<string, int>();
            var gpmc = new SortedList<string, int>();
            var gtr = new SortedList<string, int>();

            var query = (from candidat in db.Candidats
                         join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                         join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                         where candidat.statut.Equals("pres") && candidat.niveau.Equals("4eme")
                         select new
                         {
                             nomFiliere = filiere.nom_fil,
                             nomDiplome = diplome.nom_diplome,

                         }).ToList();

            foreach (var candidat in query)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (info.ContainsKey(candidat.nomDiplome))
                    {
                        info[candidat.nomDiplome]++;
                    }
                    else
                    {
                        info.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indus.ContainsKey(candidat.nomDiplome))
                    {
                        indus[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indus.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtr.ContainsKey(candidat.nomDiplome))
                    {
                        gtr[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtr.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.ToLower().Contains("gpmc")))
                {
                    if (gpmc.ContainsKey(candidat.nomDiplome))
                    {
                        gpmc[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmc.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            foreach (var i in info)
            {
                dataPointsForInfo.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indus)
            {
                dataPointsForIndus.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmc)
            {
                dataPointsForGpmc.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtr)
            {
                dataPointsForGtr.Add(new DataPoint(i.Key, i.Value));
            }

            ViewBag.dataPointsForInfo = JsonConvert.SerializeObject(dataPointsForInfo);
            ViewBag.dataPointsForIndus = JsonConvert.SerializeObject(dataPointsForIndus);
            ViewBag.dataPointsForGpmc = JsonConvert.SerializeObject(dataPointsForGpmc);
            ViewBag.dataPointsForGtr = JsonConvert.SerializeObject(dataPointsForGtr);
            return View("Statistique");
        }
        public ActionResult statistique_Des_Presents_3annee()
        {
            List<DataPoint> dataPointsForInfo = new List<DataPoint>();
            List<DataPoint> dataPointsForIndus = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmc = new List<DataPoint>();
            List<DataPoint> dataPointsForGtr = new List<DataPoint>();

            var info = new SortedList<string, int>();
            var indus = new SortedList<string, int>();
            var gpmc = new SortedList<string, int>();
            var gtr = new SortedList<string, int>();

            var query = (from candidat in db.Candidats
                         join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                         join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                         where (candidat.statut.Equals("ADMIS_PR") || candidat.statut.Equals("ADMIS_Att") || candidat.statut.Equals("present_au_cncrs")) && candidat.niveau.Equals("3eme")
                         select new
                         {
                             nomFiliere = filiere.nom_fil,
                             nomDiplome = diplome.nom_diplome,

                         }).ToList();

            foreach (var candidat in query)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (info.ContainsKey(candidat.nomDiplome))
                    {
                        info[candidat.nomDiplome]++;
                    }
                    else
                    {
                        info.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indus.ContainsKey(candidat.nomDiplome))
                    {
                        indus[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indus.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtr.ContainsKey(candidat.nomDiplome))
                    {
                        gtr[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtr.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.ToLower().Contains("gpmc")))
                {
                    if (gpmc.ContainsKey(candidat.nomDiplome))
                    {
                        gpmc[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmc.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            foreach (var i in info)
            {
                dataPointsForInfo.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indus)
            {
                dataPointsForIndus.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmc)
            {
                dataPointsForGpmc.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtr)
            {
                dataPointsForGtr.Add(new DataPoint(i.Key, i.Value));
            }

            ViewBag.dataPointsForInfo = JsonConvert.SerializeObject(dataPointsForInfo);
            ViewBag.dataPointsForIndus = JsonConvert.SerializeObject(dataPointsForIndus);
            ViewBag.dataPointsForGpmc = JsonConvert.SerializeObject(dataPointsForGpmc);
            ViewBag.dataPointsForGtr = JsonConvert.SerializeObject(dataPointsForGtr);
            return View("statistique_Des_Presents_3annee");
        }
        public ActionResult statistique_Des_Presents_4annee()
        {
            List<DataPoint> dataPointsForInfo = new List<DataPoint>();
            List<DataPoint> dataPointsForIndus = new List<DataPoint>();
            List<DataPoint> dataPointsForGpmc = new List<DataPoint>();
            List<DataPoint> dataPointsForGtr = new List<DataPoint>();

            var info = new SortedList<string, int>();
            var indus = new SortedList<string, int>();
            var gpmc = new SortedList<string, int>();
            var gtr = new SortedList<string, int>();

            var query = (from candidat in db.Candidats
                         join filiere in db.Filieres on candidat.id_fil equals filiere.id_fil
                         join diplome in db.Diplomes on candidat.id_diplome equals diplome.id_diplome
                         where (candidat.statut.Equals("ADMIS_PR") || candidat.statut.Equals("ADMIS_Att") || candidat.statut.Equals("present_au_cncrs")) && candidat.niveau.Equals("4eme")
                         select new
                         {
                             nomFiliere = filiere.nom_fil,
                             nomDiplome = diplome.nom_diplome,

                         }).ToList();

            foreach (var candidat in query)
            {

                if (candidat.nomFiliere.ToLower().Contains("info"))
                {
                    if (info.ContainsKey(candidat.nomDiplome))
                    {
                        info[candidat.nomDiplome]++;
                    }
                    else
                    {
                        info.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("indus"))
                {
                    if (indus.ContainsKey(candidat.nomDiplome))
                    {
                        indus[candidat.nomDiplome]++;
                    }
                    else
                    {
                        indus.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("telec") || candidat.nomFiliere.ToLower().Contains("téléc"))
                {
                    if (gtr.ContainsKey(candidat.nomDiplome))
                    {
                        gtr[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gtr.Add(candidat.nomDiplome, 1);
                    }
                }
                else if (candidat.nomFiliere.ToLower().Contains("proc") || (candidat.nomFiliere.ToLower().Contains("gpmc")))
                {
                    if (gpmc.ContainsKey(candidat.nomDiplome))
                    {
                        gpmc[candidat.nomDiplome]++;
                    }
                    else
                    {
                        gpmc.Add(candidat.nomDiplome, 1);
                    }
                }
            }
            foreach (var i in info)
            {
                dataPointsForInfo.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in indus)
            {
                dataPointsForIndus.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gpmc)
            {
                dataPointsForGpmc.Add(new DataPoint(i.Key, i.Value));
            }
            foreach (var i in gtr)
            {
                dataPointsForGtr.Add(new DataPoint(i.Key, i.Value));
            }

            ViewBag.dataPointsForInfo = JsonConvert.SerializeObject(dataPointsForInfo);
            ViewBag.dataPointsForIndus = JsonConvert.SerializeObject(dataPointsForIndus);
            ViewBag.dataPointsForGpmc = JsonConvert.SerializeObject(dataPointsForGpmc);
            ViewBag.dataPointsForGtr = JsonConvert.SerializeObject(dataPointsForGtr);
            return View("statistique_Des_Presents_4annee");
        }



        public ActionResult rechercher()
        {
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "id_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View();
        }


        public ActionResult delete(string CIN)
        {
            Candidat can = db.Candidats.Find(CIN);
            int id = can.id_fil;
            corbeil cor = new corbeil();
            cor.CNE = can.CNE;
            cor.CIN = can.CIN;
            cor.prenom = can.prenom;
            cor.nom = can.nom;
            cor.ville = can.ville;
            cor.addresse = can.addresse;
            cor.nom_dip = can.nom_dip;
            cor.GSM = can.GSM;
            cor.type_bac = can.type_bac;
            cor.annee_bac = can.annee_bac;
            cor.note_bac = can.note_bac;
            cor.mention_bac = can.mention_bac;
            cor.n_dossier = can.n_dossier;

            cor.nationnalite = can.nationnalite;
            cor.sexe = can.sexe;
            cor.cont_sup = can.cont_sup;

            cor.cont_ajout = can.cont_ajout;
            cor.password = can.password;
            cor.password_conf = can.password_conf;
            cor.photo = can.photo;
            cor.email = can.email;
            cor.statut = can.statut;


            cor.niveau = can.niveau;
            cor.date_naiss = can.date_naiss;
            cor.id_fil = id;
            cor.id_diplome = can.id_diplome;
            cor.id_note = can.id_note;
            db.corbeils.Add(cor);
            db.SaveChanges();
            db.Candidats.Remove(can);
            db.SaveChanges();

            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "nom_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View("rechercher");
        }


        public ActionResult DeleteCor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            corbeil corb = db.corbeils.Find(id);
            db.corbeils.Remove(corb);

            db.SaveChanges();
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "nom_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View("rechercherCorbeil", db.corbeils.ToList());
        }


        public ActionResult rechercherCorbeil()
        {

            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "nom_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View();
        }


        public JsonResult GetEtudiantByIdMatricule(string n_dossier)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.Candidats.Where(p => p.n_dossier == n_dossier && p.niveau.Equals("3eme")), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEtudiantByIdDip(string nom_diplome)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.Candidats.Where(p => p.nom_dip == nom_diplome && p.niveau.Equals("3eme")), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEtudiantById(int id_fil)
        {



            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.Candidats.Where(p => p.id_fil == id_fil && p.niveau.Equals("3eme")), JsonRequestBehavior.AllowGet);
        }


        //dans corbeil


        public JsonResult GetEtudiantByIdMatriculeCor(string n_dossier)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.corbeils.Where(p => p.n_dossier == n_dossier && p.niveau.Equals("3eme")), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEtudiantByIdDipCor(string nom_diplome)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.corbeils.Where(p => p.nom_dip == nom_diplome && p.niveau.Equals("3eme")), JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetEtudiantByIdCor(int id_fil)
        {



            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.corbeils.Where(p => p.id_fil == id_fil && p.niveau.Equals("3eme")), JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// /////////////////////////////////////pour 4eme annee////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        /// 


        public ActionResult rechercher4eme()
        {
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "id_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View();
        }


        public ActionResult delete4eme(string CIN)
        {
            Candidat can = db.Candidats.Find(CIN);
            int id = can.id_fil;
            corbeil cor = new corbeil();
            cor.CNE = can.CNE;
            cor.CIN = can.CIN;
            cor.prenom = can.prenom;
            cor.nom = can.nom;
            cor.ville = can.ville;
            cor.addresse = can.addresse;
            cor.nom_dip = can.nom_dip;
            cor.GSM = can.GSM;
            cor.type_bac = can.type_bac;
            cor.annee_bac = can.annee_bac;
            cor.note_bac = can.note_bac;
            cor.mention_bac = can.mention_bac;
            cor.n_dossier = can.n_dossier;

            cor.nationnalite = can.nationnalite;
            cor.sexe = can.sexe;
            cor.cont_sup = can.cont_sup;

            cor.cont_ajout = can.cont_ajout;
            cor.password = can.password;
            cor.password_conf = can.password_conf;
            cor.photo = can.photo;
            cor.email = can.email;
            cor.statut = can.statut;


            cor.niveau = can.niveau;
            cor.date_naiss = can.date_naiss;
            cor.id_fil = id;
            cor.id_diplome = can.id_diplome;
            cor.id_note = can.id_note;
            db.corbeils.Add(cor);
            db.SaveChanges();
            db.Candidats.Remove(can);
            db.SaveChanges();

            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "nom_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View("rechercher4eme");
        }


        public ActionResult DeleteCor4eme(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            corbeil corb = db.corbeils.Find(id);
            db.corbeils.Remove(corb);

            db.SaveChanges();
            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "nom_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View("rechercherCorbeil4eme", db.corbeils.ToList());
        }


        public ActionResult rechercherCorbeil4eme()
        {

            ViewBag.f = new SelectList(db.Filieres, "id_fil", "nom_fil");

            ViewBag.d = new SelectList(db.Diplomes, "nom_diplome", "nom_diplome");


            ViewBag.m = new SelectList(db.Candidats, "n_dossier", "n_dossier");
            return View();
        }








        public JsonResult GetEtudiantByIdMatricule4eme(string n_dossier)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.Candidats.Where(p => p.n_dossier == n_dossier && p.niveau.Equals("4eme")), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEtudiantByIdDip4eme(string nom_diplome)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.Candidats.Where(p => p.nom_dip == nom_diplome && p.niveau.Equals("4eme")), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEtudiantById4eme(int id_fil)
        {



            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.Candidats.Where(p => p.id_fil == id_fil && p.niveau.Equals("4eme")), JsonRequestBehavior.AllowGet);
        }


        //dans corbeil


        public JsonResult GetEtudiantByIdMatriculeCor4eme(string n_dossier)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.corbeils.Where(p => p.n_dossier == n_dossier && p.niveau.Equals("4eme")), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEtudiantByIdDipCor4eme(string nom_diplome)
        {


            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.corbeils.Where(p => p.nom_dip == nom_diplome && p.niveau.Equals("4eme")), JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetEtudiantByIdCor4eme(int id_fil)
        {



            db.Configuration.ProxyCreationEnabled = false;

            return Json(db.corbeils.Where(p => p.id_fil == id_fil && p.niveau.Equals("4eme")), JsonRequestBehavior.AllowGet);
        }


        public ActionResult correction(String niveau, String idfil, String msg)
        {
            int id = Convert.ToInt32(idfil);
            var candidats = db.Candidats.Include(c => c.Diplome).Include(c => c.Filiere).Include(c => c.Notes);
            candidats = candidats.Where(c => c.niveau == niveau && c.id_fil == id && c.statut == "pres");
            ViewBag.msg = msg;
            return View(candidats.ToList());

        }

        public ActionResult correction2()
        {

            var Notes1 = Request["item.Notes.notemath"].ToString().Split(',');
            var Notes2 = Request["item.Notes.notespec"].ToString().Split(',');
            var CINS = Request["item.CIN"].ToString().Split(',');
            String d;

            for (int i = 0; i < CINS.Length; i++)
            {
                d = CINS[i].ToString();
                Candidat cand = db.Candidats.Find(d);
                int idnote = cand.id_note;
                Notes n = db.Notes.Find(idnote);
                n.notemath = Double.Parse(Notes1[i]);
                n.notespec = Double.Parse(Notes2[i]);
                 if(n.notemath==0 && n.notespec == 0)
                {
                    cand.statut = "absent_au_cncrs";
                }
                else
                {
                    cand.statut = "present_au_cncrs";
                    n.note_concours = (n.notemath + n.notespec) / 2;
                }
                
                db.SaveChanges();

            }
            String cin = CINS[0];
            Candidat c = db.Candidats.Find(cin);
            String niveau = c.niveau;
            String idfil = c.id_fil.ToString();
            String msg = "affectation des notes du concours avec succés";


            return RedirectToAction("correction", new { niveau = niveau, idfil = idfil, msg = msg });
        }



    }
    }
