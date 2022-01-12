using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.Paginas
{
    public partial class CambioClave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            bool res = propietarioBoTmp.ActualizarClave(((ObjetoGenerico)Session["usuarioLogin"]).Id, txtClaveActual.Text, txtClaveNueva.Text, ((ObjetoGenerico)Session["usuarioLogin"]).Tipo);

            if (res)
            {
                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
                divExito.Visible = true;
                divError.Visible = false;

                ((ObjetoGenerico)Session["usuarioLogin"]).Cambio = false;

                Response.Redirect("~/PerfilAdmon.aspx", true);
            }
            else
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_11;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PerfilAdmon.aspx", true);
        }
    }
}
