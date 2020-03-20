using ProjetASPMVC1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjetASPMVC1.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index(Candidat objUser)
        {

            return View();
                
               
        }
        [HttpPost]
        public ActionResult Authorise(Candidat user)
        {
            if (ModelState.IsValid)
            {
                using (Projet_ContextBD db = new Projet_ContextBD())
                {

                    var userDetail = db.Candidats.Where(x => x.CIN == user.CIN && x.password == user.password).FirstOrDefault();
                    if ((userDetail == null) && (ModelState.IsValid))
                    {

                        // user.LoginErrorMsg = "Invalid UserName or Password";
                        Response.Write("<script>alert(\'données incorrect\');</" + "script>");
                        return View("Index", user);
                    }
                    else
                    {
                        CandidatsController.id = (string)Session["CIN"];
                        Session["CIN"] = userDetail.CIN;
                        Session["prenom"] = userDetail.prenom;
                        
                        return RedirectToAction("Edit", new RouteValueDictionary(new { Controller="Candidats",Action="Edit",id= Session["CIN"] }));
                    }

                }
            }else return  View("Index");

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
public ActionResult Next_page()//your view page
    {
            //Read the value
            var username = (string)Session["prenom"];
            return View();
        }

        
       


    }
    
}