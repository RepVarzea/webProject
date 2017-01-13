using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public bool possuiPermissao(string IDMorador)
        {
            if (string.IsNullOrEmpty(IDMorador))
                return false;

            MORADOR morador = db.MORADOR.FirstOrDefault(u => u.id_morador.ToString() == IDMorador);

            return morador != null && morador.ativo == 1;
        }

        public bool podeRegistar(MORADOR user)
        {
            return user.login == getLoginAdmin().login && user.senha == getLoginAdmin().senha;
        }

        private MORADOR getLoginAdmin()
        {
            MORADOR admin = db.MORADOR.FirstOrDefault(u => u.id_morador == 24);
            return admin;
        }
    }
}