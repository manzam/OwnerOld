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
    public partial class SesionAdmin : System.Web.UI.Page
    {
        UsuarioBo usuarioBo;

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblUserName.Text = Environment.UserName;
        }

        protected void loginOwner_Authenticate(object sender, AuthenticateEventArgs e)
        {
            ////string adPath = "LDAP://10.1.100.18/DC=Hoteles_estelar,DC=com"; 
            //LdapAuthentication adAuth = new LdapAuthentication(Properties.Settings.Default.LDAP);

            ////Hoteles_estelar\Sop_soft
            ////Estelar10

            //try
            //{
            //    if (adAuth.IsAuthenticated(loginOwner.UserName, loginOwner.Password))
            //    {
            //        //// Retrieve the user's groups 
            //        //string groups = adAuth.GetGroups();
            //        //// Create the authetication ticket 
            //        //FormsAuthenticationTicket authTicket =
            //        //new FormsAuthenticationTicket(1, // version 
            //        //loginOwner.UserName,
            //        //DateTime.Now,
            //        //DateTime.Now.AddMinutes(60),
            //        //false, groups);
            //        //// Now encrypt the ticket. 
            //        //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            //        //// Create a cookie and add the encrypted ticket to the 
            //        //// cookie as data. 
            //        //HttpCookie authCookie =
            //        //new HttpCookie(FormsAuthentication.FormsCookieName,
            //        //encryptedTicket);
            //        //// Add the cookie to the outgoing cookies collection. 
            //        //Response.Cookies.Add(authCookie);

            //        // Redirect the user to the originally requested page 
            //        //loginOwner.
            //        FormsAuthentication.RedirectFromLoginPage(loginOwner.UserName, loginOwner.RememberMeSet);
            //        //Response.Redirect(FormsAuthentication.GetRedirectUrl(loginOwner.UserName, false));
            //    }
            //    else
            //    {
            //        loginOwner.FailureText = "Authentication failed, check username and password.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    loginOwner.FailureText = "Error authenticating. " + ex.Message;
            //}

            UsuarioBo usuarioBoTmp = new UsuarioBo();
            ObjetoGenerico userLogin = usuarioBoTmp.Autenticar(loginOwner.UserName, loginOwner.Password);            
            //ObjetoGenerico userLogin = usuarioBoTmp.Autenticar(loginOwner.UserName);

            if (userLogin != null)
            {
                //if (rbEspanol.Checked)
                //    Session["Idioma"] = "es-CO";
                //else
                //    Session["Idioma"] = "en-CA";

                Session["usuarioLogin"] = userLogin;
                Response.Redirect("PerfilAdmon.aspx", true);
                //FormsAuthentication.RedirectFromLoginPage(loginOwner.UserName, loginOwner.RememberMeSet);
            }
        }

        protected void lbOlvideContrasena_Click(object sender, EventArgs e)
        {
            usuarioBo = new UsuarioBo();
            ObjetoGenerico user = usuarioBo.Obtener(loginOwner.UserName);

            if (user != null)
            {
                string url = Properties.Settings.Default.UrlOlvidoClaveUsuario + "?l=" + Utilities.EncriptarEnVocal(user.Login, Properties.Settings.Default.Passphrase);
                string msg = Properties.Settings.Default.mesajeCorreoOlvidoClave.Replace("%url%", url);

                msg = msg + "<br /><br />" + Properties.Settings.Default.PieCorreo;

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
                                       new List<string>() { user.Correo }, 
                                       true,
                                       ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas);

                lblRes.Text = "Ha sido enviado un correo a: " + user.Correo.ToLower();
            }
            else
            {
                lblRes.Text = "Este usuario no existe.";
            }
        }
    }
}