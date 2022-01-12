using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using DM;
using BO;

namespace WebOwner
{
    public partial class PerfilAdmon : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (((ObjetoGenerico)Session["usuarioLogin"]).Cambio)
                Response.Redirect("~/CambioClave.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {       
        }

        protected override void InitializeCulture()
        {
            //PaginaBase paginaBase = new PaginaBase();
            //paginaBase.MiIdioma(Session["Idioma"].ToString());
        }
    }
}
