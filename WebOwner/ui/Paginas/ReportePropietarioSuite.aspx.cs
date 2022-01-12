using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.Paginas
{
    public partial class ReportePropietarioSuite : System.Web.UI.Page
    {
        HotelBo hotelBoTmp = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
            }

            string ctrlTrigger = Page.Request.Params["__EVENTTARGET"];

            if (ctrlTrigger == "ctl00$Contenidoprincipal$ReportViewerReporte")
                btnAceptarUno_Click(null, null);
        }

        private void CargarCombos()
        {
            hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = null;

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotelPropietarios.DataSource = listaHotel;
            ddlHotelPropietarios.DataValueField = "IdHotel";
            ddlHotelPropietarios.DataTextField = "Nombre";
            ddlHotelPropietarios.DataBind();
        }

        protected void btnAceptarUno_Click(object sender, EventArgs e)
        {
            string condicion = string.Empty;

            if (txtNombre.Text != string.Empty)
                condicion = " Propietario.NombrePrimero like '" + txtNombre.Text + "%' or Propietario.NombreSegundo like '" + txtNombre.Text + "%' OR ";

            if (txtNumSuite.Text != string.Empty)
                condicion += "Suit.NumSuit Like '" + txtNumSuite.Text + "%' AND ";

            condicion += " Hotel.IdHotel = " + ddlHotelPropietarios.SelectedValue;

            if (condicion != string.Empty)
                condicion = " WHERE " + condicion;

            Reportes.XtraReport_ReporteUno reporteUno = new WebOwner.Reportes.XtraReport_ReporteUno(condicion, ddlHotelPropietarios.SelectedItem.Text);
            ReportViewerReporte.Report = reporteUno;
        }

        protected void ddlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlReporte.SelectedValue)
            {
                case "uno": // Propietarios - Suite
                    Response.Redirect("ReportePropietarioSuite.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "dos": // Hotel - Suite
                    Response.Redirect("ReporteHotelSuite.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "tres": // Reglas
                    Response.Redirect("ReporteReglas.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "cuatro": // Usuarios
                    Response.Redirect("ReporteUsuario.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "H": // Consolidado por suite
                    Response.Redirect("ReporteHistorialLiquidacion.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "1": // Consolidado por suite
                    Response.Redirect("ReporteConsolidadoPorSuite.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "2": // Consolidado por Propietario
                    Response.Redirect("ReporteConsolidadoPorPropietario.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "P": // Consolidado por Propietario
                    Response.Redirect("ReportePerfilPermiso.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                default:
                    break;
            }
        }
    }
}
