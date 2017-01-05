using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiteVarzea.Models;
namespace SiteVarzea.Controllers
{
    public class Functions
    {
        private RepVarzeaEntities db = new RepVarzeaEntities();
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
            MORADOR morador = db.MORADOR.Where(u => u.id_morador.ToString() == IDMorador).FirstOrDefault();

            if (morador == null || morador.ativo != 1)
                return false;
            else
                return true;
        }
    }
}