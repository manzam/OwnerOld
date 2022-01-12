using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BO;
using DM;
using Servicios;

namespace WebOwner
{
    public partial class SesionPropietario : System.Web.UI.Page
    {
        PropietarioBo propBo;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginOwner_Authenticate(object sender, AuthenticateEventArgs e)
        {
            propBo = new PropietarioBo();
            ObjetoGenerico userLogin = propBo.Autenticar(loginOwner.UserName, loginOwner.Password);

            if (userLogin != null)
            {
                Session["usuarioLogin"] = userLogin;
                Response.Redirect("PerfilAdmon.aspx", true);
                //FormsAuthentication.RedirectFromLoginPage(loginOwner.UserName, loginOwner.RememberMeSet);
            }
        }

        protected void lbOlvideContrasena_Click(object sender, EventArgs e)
        {
            propBo = new PropietarioBo();
            Propietario prop = propBo.ObtenerPropietario(loginOwner.UserName);

            if (prop != null)
            {
                //string url = Properties.Settings.Default.UrlOlvidoCalve + "?l=" + Utilities.EncriptarEnVocal(prop.Login, Properties.Settings.Default.Passphrase);

                string url = Properties.Settings.Default.UrlOlvidoCalve + "?l=" + Utilities.EncriptarEnVocal(prop.Login, Properties.Settings.Default.Passphrase);
                string msg = Properties.Settings.Default.mesajeCorreoOlvidoClave.Replace("%url%", url);

                msg += msg + "<br /><br />" + Properties.Settings.Default.PieCorreo;

                bool esEnvioCorrecto = true;

                Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                      Properties.Settings.Default.ClaveRemitente,
                                      Properties.Settings.Default.NombreRemitente,
                                      string.Empty,
                                      msg,
                                      Properties.Settings.Default.AsuntoOlvidoClave,                                      
                                      Properties.Settings.Default.HostSMTP,
                                      Properties.Settings.Default.PuertoSMTP,
                                      Properties.Settings.Default.EnableSsl,
                                      new List<string>(),
                                      new List<string>() { prop.Correo },
                                      true,
                                      ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas, -1);

                lblRespuesta.Text = "Ha sido enviado un correo a: " + prop.Correo;
            }
            else
            {
                lblRespuesta.Text = "Este usuario no existe.";
            }
        }
    }
}
