using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using System.Text;

namespace WebOwner.ui.Paginas
{
    public partial class ReporteExtractoAcumulado : System.Web.UI.Page
    {
        PropietarioBo propietarioBo = null;
        LiquidacionBo liquidacionBo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtAnoInicio.Text = (DateTime.Now.Year - 1).ToString();
                this.txtAnoFin.Text = DateTime.Now.Year.ToString();

                propietarioBo = new PropietarioBo();
                ddlPropietario.DataSource = propietarioBo.VerTodos();
                ddlPropietario.DataValueField = "IdPropietario";
                ddlPropietario.DataTextField = "NombreCompleto";
                ddlPropietario.DataBind();

                ddlPropietario_SelectedIndexChanged(null, null);
            }

            string ctrlTrigger = Page.Request.Params["__EVENTTARGET"];

            if (ctrlTrigger == "ctl00$Contenidoprincipal$ReportViewerReporte")
                btnConsolidadoPorPropietario_Click(null, null);
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

        protected void btnConsolidadoPorPropietario_Click(object sender, EventArgs e)
        {
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;
            StringBuilder listaErrores = new StringBuilder();
            DateTime fechaDesde = new DateTime(int.Parse(txtAnoInicio.Text), int.Parse(ddlMesInicioP.SelectedValue), 1);
            DateTime fechaHasta = new DateTime(int.Parse(txtAnoFin.Text), int.Parse(ddlMesFinP.SelectedValue), 1);
            string rutaTmp = Server.MapPath(Properties.Settings.Default.RutaExtracto);
            string rutaExtracto = rutaTmp + "Extracto_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".pdf";

            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();

            string[] ids = ddlSuite.SelectedValue.Split(new char[] { '%' });

            switch (configReporteTmp.ObtenerTipoExtracto(int.Parse(ids[1])))
            {
                case 1:
                    extractoTmp = new WebOwner.reportes.XtraReport_Extracto(int.Parse(ddlPropietario.SelectedValue), int.Parse(ids[0]), fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, int.Parse(ids[1]), true);
                    break;

                case 2:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(int.Parse(ddlPropietario.SelectedValue), int.Parse(ids[0]), fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, int.Parse(ids[1]), true, ref listaErrores);
                    break;

                case 3:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(int.Parse(ddlPropietario.SelectedValue), int.Parse(ids[0]), fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, int.Parse(ids[1]), true, ref listaErrores);
                    break;

                default:
                    break;
            }

            ReportViewerReporte.Report = extractoTmp;
        }

        protected void ddlPropietario_SelectedIndexChanged(object sender, EventArgs e)
        {
            liquidacionBo = new LiquidacionBo();

            ddlSuite.DataSource = liquidacionBo.ObtenerSuitPorPropietarioEnLiquidacion(int.Parse(ddlPropietario.SelectedValue));
            ddlSuite.DataValueField = "NumSuit";
            ddlSuite.DataTextField = "Nombre";
            ddlSuite.DataBind();
        }
    }
}
