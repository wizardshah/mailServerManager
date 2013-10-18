using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mailServerManager.Models;
using mailServerManager.Data;

using System.Text.RegularExpressions;


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
            MyMailServer server = db.MyMailServers.Find(mymail.MyMailServerId);

            if (mymail.checkEmail(server.DomainName, mymail.EmailAddress + "@" + server.DomainName))
            {
                ModelState.AddModelError("EmailAddress", "Email Address already Exists");
            }            
            
            if (mymail.MaxSize > server.DomainMaxAccountSize)
            {
                ModelState.AddModelError("MaxSize", "Email max size increases the limit of " + server.DomainMaxAccountSize + "MB" );
                
            }

            if (mymail.MaxSize <= 0)
            {
                ModelState.AddModelError("MaxSize", "Email Box Size can't be zero or negative");
            }

            string pattern = @"^([a-zA-Z0-9_.-]+)$";//valid email reg expression

            Match m = Regex.Match(mymail.EmailAddress, pattern);//check for invalid characters

            if (!m.Success)
            {
                ModelState.AddModelError("EmailAddress", "Email Address contains invalid character");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mymail.EmailAddress = mymail.EmailAddress + "@" + server.DomainName;
                    db.MyMails.Add(mymail);
                    db.SaveChanges();
                    //add email in the mail server
                    mymail.createNewEmailAccount(server.DomainName);
                }
                catch
                { }

                return RedirectToAction("Edit", "MailServerPanel", new { id = mymail.MyMailServerId });
            }            
                        
            ViewBag.MyMailServerId = mymail.MyMailServerId;
            ViewBag.DomainName = server.DomainName;

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

            ViewBag.MyMailServerId = mymail.MyMailServerId;
            

            return View(mymail);
        }

        //
        // POST: /EmailPanel/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyMail mymail)
        {
            MyMailServer server = db.MyMailServers.Find(mymail.MyMailServerId);

            if (mymail.MaxSize > server.DomainMaxAccountSize)
            {
                ModelState.AddModelError("MaxSize", "Email max size increases the limit of " + server.DomainMaxAccountSize + "MB");

            }

            if (mymail.MaxSize <= 0)
            {
                ModelState.AddModelError("MaxSize", "Email Box Size can't be zero or negative");
            }

            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Match m = Regex.Match(mymail.EmailAddress, pattern);

            if (!m.Success)
            {
                ModelState.AddModelError("EmailAddress", "Email Address contains invalid character");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(mymail).State = EntityState.Modified;
                    db.SaveChanges();
                    //edit email in the mail server
                    mymail.editEmailAccount(server.DomainName);
                }
                catch { }

                return RedirectToAction("Edit", "MailServerPanel", new { id = mymail.MyMailServerId });
            }

            ViewBag.MyMailServerId = mymail.MyMailServerId;
            ViewBag.DomainName = server.DomainName;

            return View(mymail);
        }

        //
        // GET: /EmailPanel/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MyMail mymail = db.MyMails.Find(id);

            ViewBag.MyMailServerId = mymail.MyMailServerId;

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

            try
            {                
                db.MyMails.Remove(mymail);
                db.SaveChanges();

                MyMailServer server = db.MyMailServers.Find(mymail.MyMailServerId);
                //delete email from the mail server
                mymail.deleteEmailAccount(server.DomainName);
            }
            catch
            { }

            return RedirectToAction("Edit", "MailServerPanel", new { id = mymail.MyMailServerId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}