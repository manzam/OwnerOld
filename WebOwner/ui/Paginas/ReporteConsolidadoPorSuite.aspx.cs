using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.Paginas
{
    public partial class ReporteConsolidadoPorSuite : System.Web.UI.Page
    {
        HotelBo hotelBoTmp = null;
        SuitBo suitBo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtFecha.Text = DateTime.Now.Year.ToString();

                hotelBoTmp = new HotelBo();
                List<DM.Hotel> listaHotel = null;

                if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                    listaHotel = hotelBoTmp.VerTodos();
                else
                    listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

                ddlHotelConsolidadoPorsuite.DataSource = listaHotel;
                ddlHotelConsolidadoPorsuite.DataValueField = "IdHotel";
                ddlHotelConsolidadoPorsuite.DataTextField = "Nombre";
                ddlHotelConsolidadoPorsuite.DataBind();
            }

            string ctrlTrigger = Page.Request.Params["__EVENTTARGET"];

            if (ctrlTrigger == "ctl00$Contenidoprincipal$ReportViewerReporte")
                btnConsolidadoPorSuite_Click(null, null);
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

        protected void btnConsolidadoPorSuite_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = new DateTime(int.Parse(txtFecha.Text), int.Parse(ddlMesInicio.SelectedValue), 1);
            DateTime fechaFin = new DateTime(int.Parse(txtFecha.Text), int.Parse(ddlMesFin.SelectedValue), 1);

            Reportes.XtraReport_ReporteConsolidadoPorSuite reporteCinco = new WebOwner.Reportes.XtraReport_ReporteConsolidadoPorSuite(int.Parse(ddlHotelConsolidadoPorsuite.SelectedValue), int.Parse(ddlSuiteConsolidadoPorsuite.SelectedValue), 1, fechaInicio, fechaFin);
            ReportViewerReporte.Report = reporteCinco;
        }

        protected void ddlHotelConsolidadoPorsuite_SelectedIndexChanged(object sender, EventArgs e)
        {
            suitBo = new SuitBo();
            ddlSuiteConsolidadoPorsuite.DataSource = suitBo.ObtenerSuitsPorHotel(int.Parse(ddlHotelConsolidadoPorsuite.SelectedValue));
            ddlSuiteConsolidadoPorsuite.DataValueField = "IdSuit";
            ddlSuiteConsolidadoPorsuite.DataTextField = "NumSuit";
            ddlSuiteConsolidadoPorsuite.DataBind();

            ddlSuiteConsolidadoPorsuite.Items.Insert(0, new ListItem("Seleccione...", "-1"));
        }
    }
}
