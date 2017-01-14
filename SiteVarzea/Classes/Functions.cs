using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using SiteVarzea.Models;

namespace SiteVarzea.Classes { 
    public class Functions
    {
        private readonly RepVarzeaEntities db = new RepVarzeaEntities();

        public string existeLogin(MORADOR USER)
        {
            //Verifica username
            MORADOR morador = db.MORADOR.Where(u => u.login == USER.login).FirstOrDefault();
            if (morador != null)
                return "Login";

            //Verifica email
            morador = db.MORADOR.Where(u => u.email == USER.email).FirstOrDefault();
            if (morador != null)
                return "Email";

            //Verifica nome
            morador = db.MORADOR.Where(u => u.nome == USER.nome).FirstOrDefault();
            if (morador != null)
                return "Nome";

            return string.Empty;
        }

        public bool possuiPermissao(Object id)
        {
            if (id == null)
                return false;

            string idMorador = id.ToString();
            MORADOR morador = db.MORADOR.FirstOrDefault(u => u.id_morador.ToString() == idMorador);

            return morador != null && morador.ativo == 1;
        }

        public bool podeRegistar(int iduser)
        {
            return iduser == getLoginAdmin().id_morador;
        }

        private MORADOR getLoginAdmin()
        {
            MORADOR admin = db.MORADOR.FirstOrDefault(u => u.id_morador == 1);
            return admin;
        }

        public string getDevedores(int idGasto)
        {
            string Devedores = "";
            foreach (var GM in db.GASTO_MORADOR.Where(u => u.GASTO.id_gasto == idGasto))
            {
                Devedores += GM.MORADOR.nome + ", ";
            }
            if (Devedores.Length <= 2)
                return Devedores;
            //Gambix para retirar última vírgula
            char[] array = Devedores.ToCharArray();
            array[array.Length - 2] = Convert.ToChar("\0");
            Devedores = new string(array);
            return Devedores;
        }
    }
}