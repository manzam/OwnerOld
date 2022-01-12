using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.Paginas
{
    public partial class ReporteHotelSuite : System.Web.UI.Page
    {
        HotelBo hotelBoTmp = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hotelBoTmp = new HotelBo();
                List<DM.Hotel> listaHotel = null;

                if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                    listaHotel = hotelBoTmp.VerTodos();
                else
                    listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

                ddlHotelSuite.DataSource = listaHotel;
                ddlHotelSuite.DataValueField = "IdHotel";
                ddlHotelSuite.DataTextField = "Nombre";
                ddlHotelSuite.DataBind();
            }

            string ctrlTrigger = Page.Request.Params["__EVENTTARGET"];

            if (ctrlTrigger == "ctl00$Contenidoprincipal$ReportViewerReporte")
                btnAceptarDos_Click(null, null);
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
                case "1": // Consolidado por suite
                    Response.Redirect("ReporteConsolidadoPorSuite.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "2": // Consolidado por Propietario
                    Response.Redirect("ReporteConsolidadoPorPropietario.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "P": // Consolidado por Propietario
                    Response.Redirect("ReportePerfilPermiso.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                case "EC": // Consolidado por Propietario
                    Response.Redirect("ReporteExtractoAcumulado.aspx?idMenu=ctl00_grupo_4", true);
                    break;
                default:
                    break;
            }
        }

        protected void btnAceptarDos_Click(object sender, EventArgs e)
        {
            string condicion = string.Empty;
            condicion = " WHERE Hotel.IdHotel = " + ddlHotelSuite.SelectedValue;

            if (txtNumSuite.Text != string.Empty)
                condicion += " and Suit.NumSuit like '" + txtNumSuite.Text + "%' ";

            Reportes.XtraReport_ReporteDos reporteDos = new WebOwner.Reportes.XtraReport_ReporteDos(condicion);
            ReportViewerReporte.Report = reporteDos;
        }
    }
}
