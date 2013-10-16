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
    public class MailServerPanelController : Controller
    {
        private MyMailContext db = new MyMailContext();

        //
        // GET: /MailServerPanel/

        public ActionResult Index()
        {
            return View(db.MyMailServers.ToList());
        }

        //
        // GET: /MailServerPanel/Details/5

        public ActionResult Details(int id = 0)
        {
            MyMailServer mymailserver = db.MyMailServers.Find(id);
            if (mymailserver == null)
            {
                return HttpNotFound();
            }
            return View(mymailserver);
        }

        //
        // GET: /MailServerPanel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MailServerPanel/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyMailServer mymailserver)
        {
            if (ModelState.IsValid)
            {
                db.MyMailServers.Add(mymailserver);
                db.SaveChanges();

                mymailserver.createNewDomain();

                return RedirectToAction("Index");
            }

            return View(mymailserver);
        }

        //
        // GET: /MailServerPanel/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MyMailServer mymailserver = db.MyMailServers.Find(id);
            if (mymailserver == null)
            {
                return HttpNotFound();
            }
            return View(mymailserver);
        }

        //
        // POST: /MailServerPanel/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyMailServer mymailserver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mymailserver).State = EntityState.Modified;
                db.SaveChanges();

                mymailserver.editDomain();

                return RedirectToAction("Index");
            }
            return View(mymailserver);
        }

        //
        // GET: /MailServerPanel/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MyMailServer mymailserver = db.MyMailServers.Find(id);
            if (mymailserver == null)
            {
                return HttpNotFound();
            }
            return View(mymailserver);
        }

        //
        // POST: /MailServerPanel/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyMailServer mymailserver = db.MyMailServers.Find(id);
            db.MyMailServers.Remove(mymailserver);
            db.SaveChanges();

            mymailserver.deleteDomain();


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}