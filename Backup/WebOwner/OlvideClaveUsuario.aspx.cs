using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DM;
using Servicios;

namespace WebOwner
{
    public partial class OlvideClaveUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string login = Request.QueryString["l"];
                login = Utilities.DesencriptarEnVocal(login, Properties.Settings.Default.Passphrase);
                string clave = Utilities.EncodePassword(String.Concat(login, login));

                UsuarioBo usuarioBotmp = new UsuarioBo();

                if (login != string.Empty)
                    usuarioBotmp.ResetPass(login, clave);
                else
                    lblInfo.InnerText = "Usuario denegado.";
            }
            catch (Exception ex)
            {
                lblInfo.InnerText = "Usuario denegado.";
            }
        }
    }
}
