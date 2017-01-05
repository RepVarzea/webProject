using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiteVarzea.Models;

namespace SiteVarzea.Controllers
{
    public class Casa : Controller
    {
        private RepVarzeaEntities db = new RepVarzeaEntities();

        // GET: Casa
        public ActionResult Index()
        {
            return View(db.GASTO_MORADOR.ToList());
        }

        // GET: Casa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GASTO_MORADOR gASTO_MORADOR = db.GASTO_MORADOR.Find(id);
            if (gASTO_MORADOR == null)
            {
                return HttpNotFound();
            }
            return View(gASTO_MORADOR);
        }

        // GET: Casa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Casa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_gasto_morador")] GASTO_MORADOR gASTO_MORADOR)
        {
            if (ModelState.IsValid)
            {
                db.GASTO_MORADOR.Add(gASTO_MORADOR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gASTO_MORADOR);
        }

        // GET: Casa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GASTO_MORADOR gASTO_MORADOR = db.GASTO_MORADOR.Find(id);
            if (gASTO_MORADOR == null)
            {
                return HttpNotFound();
            }
            return View(gASTO_MORADOR);
        }

        // POST: Casa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_gasto_morador")] GASTO_MORADOR gASTO_MORADOR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gASTO_MORADOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gASTO_MORADOR);
        }

        // GET: Casa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GASTO_MORADOR gASTO_MORADOR = db.GASTO_MORADOR.Find(id);
            if (gASTO_MORADOR == null)
            {
                return HttpNotFound();
            }
            return View(gASTO_MORADOR);
        }

        // POST: Casa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GASTO_MORADOR gASTO_MORADOR = db.GASTO_MORADOR.Find(id);
            db.GASTO_MORADOR.Remove(gASTO_MORADOR);
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
