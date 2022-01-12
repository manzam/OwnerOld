using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM;
using BO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserMisNoticias : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {        
            NoticiaBo noticiaBoTmp = new NoticiaBo();
            this.RpMisNoticias.DataSource = noticiaBoTmp.ObtenerNoticiasAMostrar();
            this.RpMisNoticias.DataBind();        
        }
    }
}