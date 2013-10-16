using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mailServerManager.Models;
using mailServerManager.Data;

namespace mailServerManager.Controllers
{
    public class EmailPanelController : Controller
    {
        private MyMailContext db = new MyMailContext();

        

        //
        // GET: /EmailPanel/Create

        public ActionResult Create(int MyMailServerId)
        {
            var dom = db.MyMailServers.Find(MyMailServerId);
            ViewBag.DomainName = dom.DomainName;
            ViewBag.MyMailServerId = MyMailServerId ;
            return View();
        }

        //
        // POST: /EmailPanel/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyMail mymail)
        {
            if (ModelState.IsValid)
            {
                MyMailServer server = db.MyMailServers.Find(mymail.MyMailServerId);
                mymail.EmailAddress = mymail.EmailAddress + "@" + server.DomainName;

                db.MyMails.Add(mymail);
                db.SaveChanges();

                
                mymail.createNewEmailAccount(server.DomainName);

                return RedirectToAction("Edit", "MailServerPanel", new { id = mymail.MyMailServerId });
            }

            //ViewBag.MyMailServerId = new SelectList(db.MyMailServers, "Id", "DomainName", mymail.MyMailServerId);
      
            return View(mymail);
        }

        //
        // GET: /EmailPanel/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MyMail mymail = db.MyMails.Find(id);
            if (mymail == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MyMailServerId = new SelectList(db.MyMailServers, "Id", "DomainName", mymail.MyMailServerId);
            return View(mymail);
        }

        //
        // POST: /EmailPanel/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyMail mymail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MyMailServer server = db.MyMailServers.Find(mymail.MyMailServerId);

                    db.Entry(mymail).State = EntityState.Modified;
                    db.SaveChanges();

                    

                    mymail.editEmailAccount(server.DomainName);
                }
                catch { }

                return RedirectToAction("Edit", "MailServerPanel", new { id = mymail.MyMailServerId });
            }
            //ViewBag.MyMailServerId = new SelectList(db.MyMailServers, "Id", "DomainName", mymail.MyMailServerId);
            return View(mymail);
        }

        //
        // GET: /EmailPanel/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MyMail mymail = db.MyMails.Find(id);
            if (mymail == null)
            {
                return HttpNotFound();
            }
            return View(mymail);
        }

        //
        // POST: /EmailPanel/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyMail mymail = db.MyMails.Find(id);
            db.MyMails.Remove(mymail);
            db.SaveChanges();

            MyMailServer server = db.MyMailServers.Find(mymail.MyMailServerId);
            mymail.deleteEmailAccount(server.DomainName);

            return RedirectToAction("Edit", "MailServerPanel", new { id = mymail.MyMailServerId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}