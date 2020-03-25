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
    public class BoitMessagesController : Controller
    {
        private Projet_ContextBD db = new Projet_ContextBD();

        // GET: BoitMessages
        public ActionResult Index()
        {
            List<BoitMessage> msgs = new List<BoitMessage>();
            foreach (var msg in db.message.ToList())
            {
                if (msg.vue == 0)
                {
                    msgs.Add(msg);
                }

            }
            return View(msgs);
        }

      


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoitMessage boitMessage = db.message.Find(id);
            boitMessage.vue = 1;
            db.SaveChanges();
            List<BoitMessage> msgs = new List<BoitMessage>();
            foreach (var msg in db.message.ToList())
            {
                if (msg.vue == 0)
                {
                    msgs.Add(msg);
                }

            }
            return RedirectToAction("Index",msgs);
        }

   

    
    }
}
