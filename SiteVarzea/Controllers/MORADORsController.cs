using System;
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
    public class MORADORsController : Controller
    {
        private readonly RepVarzeaEntities db = new RepVarzeaEntities();
        private Functions verifica = new Functions();

        // GET: MORADORs
        public ActionResult Index()
        {
            var mORADOR = db.MORADOR.Include(m => m.GASTO);
            return View(mORADOR.ToList());
        }

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
            var usr = db.MORADOR.Where(u => u.login == user.login && u.senha == user.senha).FirstOrDefault();
            if(usr != null)
            {
                Session["id_morador"] = usr.id_morador;
                Session["nome"] = usr.nome.ToString();
                if (verifica.podeRegistar(user))
                    Session["todos"] = true;
                return RedirectToAction("LoggedIn");
            }
            ModelState.AddModelError("", "Username or password is wrong.");
            
            return View();
        }
        #endregion

        #region LoggedIn

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

                if (verifica.podeRegistar(USER))
                {
                    db.MORADOR.Add(USER);
                    db.SaveChanges();
                }
                else
                {
                    return Redirect("~/Error/Erro401");
                }
            }
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
