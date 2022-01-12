using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using Servicios;

namespace WebOwner
{
    public partial class Hotel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void InitializeCulture()
        {
            PaginaBase paginaBase = new PaginaBase();
            //paginaBase.MiIdioma(Session["Idioma"].ToString());
        }
    }
}
