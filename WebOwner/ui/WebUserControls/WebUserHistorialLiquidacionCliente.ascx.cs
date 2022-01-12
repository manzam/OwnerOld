using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DM;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserHistorialLiquidacionCliente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.txtAnoDesde.Text = DateTime.Now.Year.ToString();
                this.txtAnoHasta.Text = DateTime.Now.Year.ToString();
                this.EsReporte = false;

                PropietarioBo propietarioBoTmp = new PropietarioBo();
                List<ObjetoGenerico> listaHotelSuite = propietarioBoTmp.ObtenerPropietarioConSuiteAndHotel(((ObjetoGenerico)Session["usuarioLogin"]).Id);

                this.ListaHotel = (from H in listaHotelSuite
                                   group H by new { H.NombreHotel, H.IdHotel } into grupo
                                   select new ObjetoGenerico()
                                   {
                                       IdHotel = grupo.Key.IdHotel,
                                       NombreHotel = grupo.Key.NombreHotel
                                   }).ToList();
                
                this.ListaSuite = (from S in listaHotelSuite
                                   select new ObjetoGenerico()
                                   {
                                       IdHotel = S.IdHotel,
                                       IdSuit = S.IdSuit,
                                       NumSuit = S.NumSuit

                                   }).Distinct().ToList();

                this.CargarCombos();
                ddlHotel_SelectedIndexChanged(null, null);
            }

            if (this.EsReporte)
                btnAceptar_Click(null, null);
        }

        #region Propiedad

        public List<ObjetoGenerico> ListaSuite 
        {
            get { return (List<ObjetoGenerico>)ViewState["ListaSuite"]; }
            set { ViewState["ListaSuite"] = value; }
        }

        public List<ObjetoGenerico> ListaHotel
        {
            get { return (List<ObjetoGenerico>)ViewState["ListaHotel"]; }
            set { ViewState["ListaHotel"] = value; }
        }

        public bool EsReporte 
        {
            get { return (bool)ViewState["EsReporte"]; }
            set { ViewState["EsReporte"] = value; }
        }

        #endregion

        #region Metodo

        private void CargarCombos()
        {
            ddlHotel.DataSource = this.ListaHotel;
            ddlHotel.DataTextField = "NombreHotel";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();
        }

        #endregion

        #region Boton

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaDesde = new DateTime(int.Parse(this.txtAnoDesde.Text), int.Parse(ddlMesDesde.SelectedValue), 1, 00, 00, 00);
                DateTime fechaHasta = new DateTime(int.Parse(this.txtAnoHasta.Text), int.Parse(ddlMesHasta.SelectedValue), 1, 00, 00, 00);

                LiquidacionBo liquidacionBoTmp = new LiquidacionBo();
                List<ObjetoGenerico> listaLiquidacionHotel = liquidacionBoTmp.
                                                          ReporteLiquidacionPropietario(int.Parse(ddlHotel.SelectedValue),
                                                                                        fechaDesde,
                                                                                        fechaHasta,
                                                                                        ((ObjetoGenerico)Session["usuarioLogin"]).Id,
                                                                                        int.Parse(ddlSuite.SelectedValue));

                string nombreTitulo = "LIQUIDACION - " + ddlHotel.SelectedItem.Text + " N° SUITE " + ddlSuite.SelectedItem.Text;

                Reportes.XtraReport_LiquidacionPropietario reporte = new WebOwner.Reportes.
                                                                         XtraReport_LiquidacionPropietario(fechaDesde, fechaHasta,
                                                                                                           nombreTitulo,
                                                                                                           listaLiquidacionHotel,3);
                ReportViewer_HistorialLiquidacion.Report = reporte;
                this.EsReporte = true;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        #endregion

        #region Eventos

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idHotel = int.Parse(ddlHotel.SelectedValue);

            ddlSuite.DataSource = (from S in this.ListaSuite
                                   where S.IdHotel == idHotel
                                   select S).ToList();
            ddlSuite.DataTextField = "NumSuit";
            ddlSuite.DataValueField = "IdSuit";
            ddlSuite.DataBind();
        }

        #endregion
    }
}