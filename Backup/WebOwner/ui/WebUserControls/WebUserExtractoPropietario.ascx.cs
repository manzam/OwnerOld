using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserExtractoPropietario : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.txtFecha.Text = DateTime.Now.Year.ToString();
                this.txtFechaDesde.Text = DateTime.Now.Year.ToString();

                this.EsVistaEstracto = false;
                CargarGrilla();
            }

            if (this.EsVistaEstracto)
            {
                btnVerExtracto_Click(null, null);
            }
        }

        #region Propiedades
        public int IdHotel
        {
            get { return (int)ViewState["IdHotel"]; }
            set { ViewState["IdHotel"] = value; }
        }
        public int IdSuit
        {
            get { return (int)ViewState["IdSuit"]; }
            set { ViewState["IdSuit"] = value; }
        }
        public bool EsVistaEstracto
        {
            get { return (bool)ViewState["EsVistaEstracto"]; }
            set { ViewState["EsVistaEstracto"] = value; }
        }
        #endregion

        #region Metodos
        public void CargarGrilla()
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            gvwSuitsHotel.DataSource = propietarioBoTmp.ObtenerPropietarioConSuiteAndHotel(((ObjetoGenerico)Session["usuarioLogin"]).Id);
            gvwSuitsHotel.DataBind();
        }
        #endregion

        #region Eventos
        protected void btnVerExtracto_Click(object sender, EventArgs e)
        {
            StringBuilder listaErrores = new StringBuilder();

            try
            {
                Button btnTmp = (Button)sender;
                string[] idTmp = btnTmp.CommandArgument.Split(',');

                this.IdHotel = int.Parse(idTmp[0]);
                this.IdSuit = int.Parse(idTmp[1]);
            }
            catch (Exception) { }

            DateTime fechaDesde = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = new DateTime(int.Parse(this.txtFechaDesde.Text), int.Parse(ddlMesHasta.SelectedValue), 1, 00, 00, 00);
            bool esAcumulado = cbEsAcumulado.Checked;
            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;

            switch (configReporteTmp.ObtenerTipoExtracto(this.IdHotel))
            {
                case 1:
                    extractoTmp = new WebOwner.reportes.XtraReport_Extracto(((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, this.IdHotel, esAcumulado);
                    break;

                case 2:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, this.IdHotel, esAcumulado, ref listaErrores);
                    break;

                case 3:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, this.IdHotel, esAcumulado, ref listaErrores);
                    break;

                case 4:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCuatro(((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, this.IdHotel, esAcumulado);
                    break;

                default:
                    break;
            } 

            //reportes.XtraReport_Extracto xr_Extracto = new WebOwner.reportes.XtraReport_Extracto(((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdSuit, fecha, Properties.Settings.Default.IdPorcentajePropiedad, IdHotel, ref listaErrores);

            if (listaErrores.ToString() != string.Empty)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaErrores.ToString();
            }
            else
            {
                string nombrePdf = ((ObjetoGenerico)Session["usuarioLogin"]).Id + "_" + this.IdSuit + "_" + fechaDesde.Month + "_" + fechaDesde.Year;
                string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");

                ReportViewer_Reporte.Report = extractoTmp;
                this.EsVistaEstracto = true;
            }
        }
        #endregion

        #region Boton
        #endregion
    }
}