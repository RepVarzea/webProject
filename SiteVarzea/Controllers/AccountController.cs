using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteVarzea.Models;

namespace SiteVarzea.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using(OurDbContext db = new OurDbContext())
            {
                return View(db.morador.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(MORADOR account)
        {
            if(ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.morador.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.nome + "/" + account.ano +" " +  "registrado com sucesso.";
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MORADOR user)
        {
            using (OurDbContext db = new OurDbContext())
            {
                var usr = db.morador.Where(u => u.login == user.login && u.senha == user.senha).FirstOrDefault();
                if(usr != null)
                {
                    Session["idmorador"] = usr.id_morador.ToString();
                    Session["login"] = usr.nome.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("","Login or password is wrong.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["idmorador"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
    }
}