using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace WebOwner
{
    public partial class Salir : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();

            //se borra la cookie de autenticacion
            FormsAuthentication.SignOut();
            //se redirecciona al usuario a la pagina de login
            //Response.Redirect(Request.UrlReferrer.ToString());
            //Response.Redirect("http://www.hotelesestelar.com/", true);
            Response.Redirect("~/Default.aspx", true);
        }
    }
}
