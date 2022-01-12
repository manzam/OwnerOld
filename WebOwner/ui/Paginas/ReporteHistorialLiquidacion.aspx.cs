using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using DM;
using System.Data;

namespace WebOwner.ui.Paginas
{
    public partial class ReporteHistorialLiquidacion : System.Web.UI.Page
    {
        HotelBo hotelBoTmp = null;
        CierreBo cierreBo;
        LiquidacionBo liquidacionBo;
        ParametroBo parametroBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            uc_WebUserBuscadorPropietarioSuite.AlAceptar += new EventHandler(uc_WebUserBuscadorPropietarioSuite_AlAceptar);

            if (!IsPostBack)
            {
                parametroBoTmp = new ParametroBo();
                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));

                this.EsReporteLiquidacionHotel = false;
                this.EsReporteLiquidacionPropietario = false;

                this.txtFechaDesde.Text = DateTime.Now.Year.ToString();
                this.txtFechaHasta.Text = DateTime.Now.Year.ToString();

                hotelBoTmp = new HotelBo();
                List<DM.Hotel> listaHotel = null;

                if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                    listaHotel = hotelBoTmp.VerTodos();
                else
                    listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

                ddlHotelReporte.DataSource = listaHotel;
                ddlHotelReporte.DataValueField = "IdHotel";
                ddlHotelReporte.DataTextField = "Nombre";
                ddlHotelReporte.DataBind();                
            }

            string ctrlTrigger = Page.Request.Params["__EVENTTARGET"];

            if (this.EsReporteLiquidacionHotel)
                if (ctrlTrigger == "ctl00$Contenidoprincipal$ReportViewer_Liquidacion")
                    btnReporteLiquidacionHotel_Click(null, null);

            if (this.EsReporteLiquidacionPropietario)
                if (ctrlTrigger == "ctl00$Contenidoprincipal$ReportViewer_Liquidacion")
                    btnReporteLiquidacionPropietario_Click(null, null);
        }

        public bool EsReporteLiquidacionHotel
        {
            get { return (bool)ViewState["EsReporteLiquidacionHotel"]; }
            set { ViewState["EsReporteLiquidacionHotel"] = value; }
        }
        public bool EsReporteLiquidacionPropietario
        {
            get { return (bool)ViewState["EsReporteLiquidacionPropietario"]; }
            set { ViewState["EsReporteLiquidacionPropietario"] = value; }
        }
        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
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
                case "H": // Consolidado por suite
                    Response.Redirect("ReporteHistorialLiquidacion.aspx?idMenu=ctl00_grupo_4", true);
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

        protected void btnReporteLiquidacionHotel_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = new DateTime(int.Parse(this.txtFechaDesde.Text), int.Parse(ddlMesDesde.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = Utilities.ObtenerUltimoDiaMes(int.Parse(this.txtFechaHasta.Text), int.Parse(ddlMesHasta.SelectedValue));
            int idHotel = int.Parse(ddlHotelReporte.SelectedValue);

            liquidacionBo = new LiquidacionBo();
            List<DM.Liquidacion> listaLiquidacionHotel = liquidacionBo.ReporteLiquidacionHotel(idHotel, fechaInicio, fechaHasta);

            Reportes.XtraReport_LiquidacionHotel reporteTmp = new WebOwner.Reportes.XtraReport_LiquidacionHotel(fechaInicio,
                                                                                                                ddlHotelReporte.SelectedItem.Text,
                                                                                                                listaLiquidacionHotel);
            ReportViewer_Liquidacion.Report = reporteTmp;

            this.EsReporteLiquidacionHotel = true;
            this.EsReporteLiquidacionPropietario = false;
        }

        protected void btnReporteLiquidacionPropietario_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = new DateTime(int.Parse(this.txtFechaDesde.Text), int.Parse(ddlMesDesde.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = Utilities.ObtenerUltimoDiaMes(int.Parse(this.txtFechaHasta.Text), int.Parse(ddlMesHasta.SelectedValue));

            int idHotel = int.Parse(ddlHotelReporte.SelectedValue);
            int numVariablesPropietarios = 0;

            liquidacionBo = new LiquidacionBo();
            List<ObjetoGenerico> listaLiquidacion = liquidacionBo.ReporteLiquidacionPropietario(idHotel, fechaInicio, fechaHasta, ref numVariablesPropietarios, -1);

            if (listaLiquidacion.Count > 0)
            {
                Reportes.XtraReport_LiquidacionPropietario reporteTmp = new WebOwner.Reportes.XtraReport_LiquidacionPropietario(fechaInicio,
                                                                                                                                fechaHasta,
                                                                                                                                ddlHotelReporte.SelectedItem.Text,
                                                                                                                                listaLiquidacion,
                                                                                                                                numVariablesPropietarios);
                ReportViewer_Liquidacion.Report = reporteTmp;

                this.EsReporteLiquidacionHotel = false;
                this.EsReporteLiquidacionPropietario = true;
            }
        }

        protected void btnBuscarHistorial_Click(object sender, EventArgs e)
        {
            uc_WebUserBuscadorPropietarioSuite.PageSize = this.PageSize;
            uc_WebUserBuscadorPropietarioSuite.CargarGrilla();
        }

        void uc_WebUserBuscadorPropietarioSuite_AlAceptar(object sender, EventArgs e)
        {
            DateTime fechaInicio = new DateTime(int.Parse(this.txtFechaDesde.Text), int.Parse(ddlMesDesde.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = Utilities.ObtenerUltimoDiaMes(int.Parse(this.txtFechaHasta.Text), int.Parse(ddlMesHasta.SelectedValue));
            int idHotel = int.Parse(ddlHotelReporte.SelectedValue);
            int numVariablesPropietarios = 0;

            liquidacionBo = new LiquidacionBo();
            //List<DM.Liquidacion> listaLiquidacionHotel = liquidacionBo.ReporteLiquidacionPropietario(idHotel, fechaDesde, ref numVariablesPropietarios, uc_WebUserBuscadorPropietarioSuite.IdPropietarioBuscado);

            //Reportes.XtraReport_LiquidacionPropietario reporteTmp = new WebOwner.Reportes.XtraReport_LiquidacionPropietario(fechaDesde,
            //                                                                                                                ddlHotelReporte.SelectedItem.Text,
            //                                                                                                                uc_WebUserBuscadorPropietarioSuite.IdHotelBuscado, 
            //                                                                                                                uc_WebUserBuscadorPropietarioSuite.IdPropietarioBuscado, 
            //                                                                                                                uc_WebUserBuscadorPropietarioSuite.IdSuiteBuscado,
            //                                                                                                                numVariablesPropietarios);

            List<ObjetoGenerico> listaLiquidacion = liquidacionBo.ReporteLiquidacionPropietario(idHotel, fechaInicio, fechaHasta, ref numVariablesPropietarios, uc_WebUserBuscadorPropietarioSuite.IdPropietarioBuscado);

            Reportes.XtraReport_LiquidacionPropietario reporteTmp = new WebOwner.Reportes.XtraReport_LiquidacionPropietario(fechaInicio,
                                                                                                                            fechaHasta,
                                                                                                                            ddlHotelReporte.SelectedItem.Text,
                                                                                                                            listaLiquidacion,
                                                                                                                            numVariablesPropietarios);
            ReportViewer_Liquidacion.Report = reporteTmp;

            this.EsReporteLiquidacionHotel = false;
            this.EsReporteLiquidacionPropietario = true;
        }
    }
}
