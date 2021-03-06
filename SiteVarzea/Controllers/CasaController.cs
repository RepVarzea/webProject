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
using System.Web.Optimization;
using SiteVarzea.Classes;

namespace SiteVarzea.Controllers
{
    public class CasaController : Controller
    {
        private readonly RepVarzeaEntities db = new RepVarzeaEntities();
        private Functions verifica = new Functions();

        #region Moradores 
        public ActionResult Moradores()
        {
            var mORADOR = db.MORADOR.Include(m => m.GASTO);
            return View(mORADOR.ToList().OrderByDescending(c => c.ativo).ThenBy(a => a.ano));
        }
        #endregion

        #region Login
        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MORADOR user)
        {
            var usr = db.MORADOR.FirstOrDefault(u => u.login == user.login && u.senha == user.senha);
            if(usr != null)
            {
                Session["id_morador"] = usr.id_morador;
                Session["nome"] = usr.nome;
                
                //Habilita o botão "Novo morador" no menu
                if (verifica.podeRegistar(usr.id_morador))
                    Session["todos"] = true;

                return Redirect("~/Contas/ExtrasPessoal");
            }
            ModelState.AddModelError("", "Username or password is wrong.");
            
            return View();
        }
        #endregion

        #region Logout
        //Logout
        public ActionResult Logout()
        {
            Session["id_morador"] = null;
            Response.Redirect("../");
            return null;
        }
        #endregion

        #region Registrar
        // GET: ContaCreate
        public ActionResult Registrar()
        {
            ViewBag.id_morador = new SelectList(db.GASTO, "id_gasto", "descricao");
            return View();
        }

        // POST: ContaCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "id_morador,nome,ano,email,aux,ativo,login,senha,confirmaSenha")] MORADOR USER)
        {
            if (ModelState.IsValid)
            {
                USER.ativo = USER.aux ? 1 : 0;
                string check = verifica.existeLogin(USER);
                if (!string.IsNullOrEmpty(check))
                {
                    ModelState.AddModelError("", check + " já cadastrado.");
                    return View();
                }

                if (verifica.podeRegistar((int)Session["id_morador"]))
                {
                    db.MORADOR.Add(USER);
                    db.SaveChanges();
                }
                else
                {
                    return Redirect("~/Error/Erro401");
                }
            }
            Session["todos"] = null;
            Session["inserido"] = "Inserido com sucesso!";

            return RedirectToAction("Login");
        }
        #endregion

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
