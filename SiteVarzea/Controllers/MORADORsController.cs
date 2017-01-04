﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiteVarzea.Models;
using System.Threading;

namespace SiteVarzea.Controllers
{
    public class MORADORsController : Controller
    {
        private RepVarzeaEntities db = new RepVarzeaEntities();
        private VerificaSQL verifica = new VerificaSQL();

        // GET: MORADORs
        public ActionResult Index()
        {
            var mORADOR = db.MORADOR.Include(m => m.GASTO);
            return View(mORADOR.ToList());
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MORADOR user)
        {
            var usr = db.MORADOR.Where(u => u.login == user.login && u.senha == user.senha).FirstOrDefault();
            if(usr != null)
            {
                Session["id_morador"] = usr.id_morador;
                Session["nome"] = usr.nome.ToString();
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Username or password is wrong.");
            }
            return View();
        }

        //LoggedIn
        public ActionResult LoggedIn()
        {
            if (Session["id_morador"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MORADOR mORADOR = db.MORADOR.Find(Session["id_morador"]);
            if (mORADOR == null)
            {
                return HttpNotFound();
            }
            return View(mORADOR);
        }

        // GET: ContaDetails/5
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

        // GET: ContaCreate
        public ActionResult Create()
        {
            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao");
            return View();
        }

        // POST: ContaCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_morador,nome,ano,email,aux,ativo,login,senha,confirmaSenha")] MORADOR USER)
        {
            if (ModelState.IsValid)
            {
                USER.ativo = USER.aux ? 1 : 0;
                string check = verifica.existeLogin(USER);
                if (!string.IsNullOrEmpty(check))
                {
                    ModelState.AddModelError("",check + " já cadastrado.");
                    return View();
                }

                db.MORADOR.Add(USER);
                db.SaveChanges();
            }

            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao", USER.id_morador);
            ViewBag.Message = USER.nome + "/" + USER.ano + " " + "registrado com sucesso.";
            Thread.Sleep(3000);
            return RedirectToAction("Login");
        }

        // GET: ContaEdit/5
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

        // POST: ContaEdit/5
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

        // GET: ContaDelete/5
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

        // POST: ContaDelete/5
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
