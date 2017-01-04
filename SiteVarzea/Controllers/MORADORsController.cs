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
    public class MORADORsController : Controller
    {
        private RepVarzeaEntities db = new RepVarzeaEntities();

        // GET: MORADORs
        public ActionResult Index()
        {
            var mORADOR = db.MORADOR.Include(m => m.GASTO);
            return View(mORADOR.ToList());
        }

        // GET: MORADORs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MORADOR mORADOR = db.MORADOR.Find(id);
            if (mORADOR == null)
            {
                return HttpNotFound();
            }
            return View(mORADOR);
        }

        // GET: MORADORs/Create
        public ActionResult Create()
        {
            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao");
            return View();
        }

        // POST: MORADORs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_morador,nome,ano,email,aux,ativo,login,senha,confirmaSenha")] MORADOR mORADOR)
        {
            if (ModelState.IsValid)
            {
                mORADOR.ativo = mORADOR.aux ? 1 : 0;
                db.MORADOR.Add(mORADOR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao", mORADOR.id_morador);
            return View(mORADOR);
        }

        // GET: MORADORs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MORADOR mORADOR = db.MORADOR.Find(id);
            if (mORADOR == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao", mORADOR.id_morador);
            return View(mORADOR);
        }

        // POST: MORADORs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_morador,nome,ano,email,ativo,login,senha")] MORADOR mORADOR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mORADOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao", mORADOR.id_morador);
            return View(mORADOR);
        }

        // GET: MORADORs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MORADOR mORADOR = db.MORADOR.Find(id);
            if (mORADOR == null)
            {
                return HttpNotFound();
            }
            return View(mORADOR);
        }

        // POST: MORADORs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MORADOR mORADOR = db.MORADOR.Find(id);
            db.MORADOR.Remove(mORADOR);
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
