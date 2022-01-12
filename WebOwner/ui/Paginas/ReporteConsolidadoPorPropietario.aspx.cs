using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.Paginas
{
    public partial class ReporteConsolidadoPorPropietario : System.Web.UI.Page
    {
        HotelBo hotelBoTmp = null;
        PropietarioBo propietarioBo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtFechaPropietario.Text = DateTime.Now.Year.ToString();

                propietarioBo = new PropietarioBo();
                ddlPropietario.DataSource = propietarioBo.VerTodos();
                ddlPropietario.DataValueField = "IdPropietario";
                ddlPropietario.DataTextField = "NombreCompleto";
                ddlPropietario.DataBind();
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
            DateTime fechaInicio = new DateTime(int.Parse(txtFechaPropietario.Text), int.Parse(ddlMesInicioP.SelectedValue), 1);
            DateTime fechaFin = new DateTime(int.Parse(txtFechaPropietario.Text), int.Parse(ddlMesFinP.SelectedValue), 1);

            Reportes.XtraReport_ReporteConsolidadoPorPropietario reporteSeis = new WebOwner.Reportes.XtraReport_ReporteConsolidadoPorPropietario(int.Parse(ddlPropietario.SelectedValue), fechaInicio, fechaFin);
            ReportViewerReporte.Report = reporteSeis;
        }
    }
}
