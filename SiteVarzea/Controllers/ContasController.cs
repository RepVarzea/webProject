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
    public class ContasController : Controller
    {
        private RepVarzeaEntities db = new RepVarzeaEntities();
        private Functions verifica = new Functions();

        // GET: Contas
        public ActionResult Index()
        {
            var gASTO = db.GASTO.Include(g => g.MORADOR);
            return View(gASTO.ToList());
        }

        // GET: Contas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GASTO gASTO = db.GASTO.Find(id);
            if (gASTO == null)
            {
                return HttpNotFound();
            }
            return View(gASTO);
        }

        // GET: Contas/Create
        public ActionResult Create()
        {
            if(!verifica.possuiPermissao(Session["id_morador"].ToString()))
                return Redirect("~/Error/Erro401");

            ViewBag.id_gasto = new SelectList(db.MORADOR, "id_morador", "nome");
            return View();
        }

        // POST: Contas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_gasto,id_morador,descricao,data,valor")] GASTO gASTO)
        {
            if (ModelState.IsValid)
            {
                db.GASTO.Add(gASTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_gasto = new SelectList(db.MORADOR, "id_morador", "nome", gASTO.id_gasto);
            return View(gASTO);
        }

        // GET: Contas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GASTO gASTO = db.GASTO.Find(id);
            if (gASTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_gasto = new SelectList(db.MORADOR, "id_morador", "nome", gASTO.id_gasto);
            return View(gASTO);
        }

        // POST: Contas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_gasto,id_morador,descricao,data,valor")] GASTO gASTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gASTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_gasto = new SelectList(db.MORADOR, "id_morador", "nome", gASTO.id_gasto);
            return View(gASTO);
        }

        // GET: Contas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GASTO gASTO = db.GASTO.Find(id);
            if (gASTO == null)
            {
                return HttpNotFound();
            }
            return View(gASTO);
        }

        // POST: Contas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GASTO gASTO = db.GASTO.Find(id);
            db.GASTO.Remove(gASTO);
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
