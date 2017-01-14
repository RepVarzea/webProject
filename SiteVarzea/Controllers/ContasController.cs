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
        private readonly RepVarzeaEntities db = new RepVarzeaEntities();
        private readonly Functions functions = new Functions();

        #region mostraExtrasGeral
        public ActionResult Extras()
        {
            //Verifica se é morador ativo
            if (!functions.possuiPermissao(Session["id_morador"]))
                return Redirect("~/Error/Erro401");

            string connectionString = ConfigurationManager.ConnectionStrings["RepVarzeaWin"].ConnectionString;
            const string sql = "SELECT  id_gasto,data ,pagou.nome,valor,descricao " +
                               "FROM GASTO g " +
                               "LEFT OUTER JOIN MORADOR pagou ON pagou.id_morador = g.id_morador;";

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
                            id_gasto = (int)dr["id_gasto"],
                            nomeDevedores = functions.getDevedores((int)dr["id_gasto"])
                        };
                        model.Add(gasto);
                    }
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Erro!");
            }
            return View(model);
        }

        public ActionResult ExtrasPessoal()
        {
            double totalPagar = 0;
            double totalRebeber = 0;

            //Verifica se é morador ativo
            if (!functions.possuiPermissao(Session["id_morador"]))
                return Redirect("~/Error/Erro401");

            int idMorador = (int)Session["id_morador"];
            //Calcula quanto deve(Valor de cada gasto_morador relacionado à seu ID)
            foreach (var gm in db.GASTO_MORADOR.Where(u => u.MORADOR.id_morador == idMorador))
            {

                int count = db.GASTO_MORADOR.Count(u => u.GASTO.id_gasto == gm.GASTO.id_gasto);
                GASTO gasto = db.GASTO.FirstOrDefault(u => u.id_gasto == gm.GASTO.id_gasto);
                if (gasto == null) continue;

                double valor = gasto.valor / count;
                totalPagar += valor;
            }

            //Calcula quanto gastou(Valor de cada Gasto relacionado à seu ID)
            foreach (var g in db.GASTO.Where(u => u.id_morador == idMorador))
            {
                totalRebeber += g.valor;
            }
            double totalLiquido = totalRebeber - totalPagar;

            GASTO_MORADOR morador = new GASTO_MORADOR
            {
                nome = (from u in db.MORADOR
                        where u.id_morador == idMorador
                        select u.nome).SingleOrDefault(),
                totalPagar = totalPagar,
                totalReceber = totalRebeber,
                totalLiquido = totalLiquido
            };

            return View(morador);
        }
        #endregion

        #region incluiExtras
        public ActionResult Novo()
        {
            Object i = null;
            string a = i.ToString();
            //Verifica se é morador ativo
            if (!functions.possuiPermissao(Session["id_morador"]))
                return Redirect("~/Error/Erro401");

            CollectionVM collectionVM = new CollectionVM();
            List<ChoiceViewModel> choiceList =
                db.MORADOR.Where(user => user.ativo == 1)
                    .OrderBy(n => n.nome)
                    .Select(user => new ChoiceViewModel() { SNo = user.id_morador, Text = user.nome })
                    .ToList();

            collectionVM.ChoicesVM = choiceList;
            collectionVM.SelectedChoices = new List<long>();
            ViewBag.MoradorList = collectionVM;
            return View();
        }

        [HttpPost]
        public ActionResult Novo(CollectionVM collectionVM, GASTO gASTO)
        {
            //Verifica se é morador ativo
            if (!functions.possuiPermissao(Session["id_morador"]))
                return Redirect("~/Error/Erro401");

            var selecionados = collectionVM.SelectedChoices;
            int idPagou = (int)Session["id_morador"];

            //Cria novo gasto
            GASTO gasto = new GASTO
            {
                MORADOR = db.MORADOR.FirstOrDefault(u => u.id_morador == idPagou),
                id_morador = idPagou,
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
                    GASTO = db.GASTO.FirstOrDefault(u => u.id_gasto == gasto.id_gasto)
                };
                db.GASTO_MORADOR.Add(gm);
                db.SaveChanges();
            }
            return RedirectToAction("Extras");
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
