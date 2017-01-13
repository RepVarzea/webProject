using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiteVarzea.Models;
using System.Configuration;
using SiteVarzea.Classes;


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

        #region mostraExtras
        public ActionResult Extras()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RepVarzeaWin"].ConnectionString;
            string sql = "SELECT  id_gasto,data ,pagou.nome,valor,descricao " +
                         "FROM GASTO g " +
                         "LEFT OUTER JOIN MORADOR pagou ON pagou.id_morador = g.id_morador;";
            MORADOR aux = new MORADOR();
            var model = new List<GASTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var gasto = new GASTO
                        {
                            data = DateTime.Parse(dr["data"].ToString()),
                            descricao = dr["descricao"].ToString(),
                            valor = double.Parse(dr["valor"].ToString()),
                            nomeMorador = dr["nome"].ToString(),
                            id_gasto = Convert.ToInt32(dr["id_gasto"].ToString())
                        };
                        model.Add(gasto);
                    }
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("","Erro!");
            }
            return View(model);
        }
        #endregion

        public ActionResult Novo()
        {
            CollectionVM collectionVM = new CollectionVM();
            List<ChoiceViewModel> choiceList = db.MORADOR.Where(user => user.ativo == 1).OrderBy(n => n.nome).Select(user => new ChoiceViewModel() {SNo = user.id_morador, Text = user.nome}).ToList();

            collectionVM.ChoicesVM = choiceList;
            collectionVM.SelectedChoices = new List<long>();
            ViewBag.MoradorList = collectionVM;

            return View();
        }

        [HttpPost]
        public ActionResult Novo(CollectionVM collectionVM, GASTO gASTO)
        {
            var selecionados = collectionVM.SelectedChoices;
            //int idPagou = (int)Session["id_morador"];
            int idPagou = 25;
            //Cria novo gasto
            GASTO gasto = new GASTO
            {
                MORADOR = db.MORADOR.FirstOrDefault(u => u.id_morador == idPagou),
                id_morador = idPagou ,
                valor = gASTO.valor,
                descricao = gASTO.descricao,
                data = DateTime.Now
            };
            db.GASTO.Add(gasto);
            db.SaveChanges();
            foreach (var id in selecionados)
            {
                int id_morador = (int)id;
                GASTO_MORADOR gm = new GASTO_MORADOR
                {
                    MORADOR = db.MORADOR.FirstOrDefault(u => u.id_morador == id_morador),
                    GASTO = db.GASTO.FirstOrDefault(u => u.id_gasto == gasto.id_gasto )
                };
                db.GASTO_MORADOR.Add(gm);
                db.SaveChanges();
            }
            return View();
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
