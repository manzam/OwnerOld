using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserCierre : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.divExito.Visible = false;
            this.divError.Visible = false;

            if (!IsPostBack)
            {
                this.txtAno.Text = DateTime.Now.Year.ToString();
                this.IdCierre = -1;

                ParametroBo parametroBoTmp = new ParametroBo();
                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                gvwCierres.PageSize = this.PageSize;

                HotelBo hotelBoTmp = new HotelBo();

                List<DM.Hotel> listaHotel = new List<DM.Hotel>();
                

                if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                {
                    listaHotel = hotelBoTmp.VerTodos();
                    ddlHotel.DataSource = listaHotel;
                }
                else
                {
                    listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);
                    ddlHotel.DataSource = listaHotel;
                }
                
                ddlHotel.DataTextField = "Nombre";
                ddlHotel.DataValueField = "IdHotel";
                ddlHotel.DataBind();

                ddlHotel.Items.Insert(0, new ListItem("Seleccione...", "0", true));
            }
        }

        #region Propiedades

        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }
        public int IdCierre 
        {
            get { return (int)ViewState["IdCierre"]; }
            set { ViewState["IdCierre"] = value; }
        }
        public int IdHotel
        {
            get { return (int)ViewState["IdHotel"]; }
            set { ViewState["IdHotel"] = value; }
        }
        public bool Activo
        {
            get { return (bool)ViewState["Activo"]; }
            set { ViewState["Activo"] = value; }
        }
        #endregion

        #region Metodos
        private void Limpiar()
        {
            txtAno.Text = DateTime.Now.Year.ToString();
            txtObservacion.Text = string.Empty;
            this.IdCierre = -1;
            gvwCierres.SelectedIndex = -1;

            gvwCierres.DataSource = null;
            gvwCierres.DataBind();
        }
        #endregion

        #region Eventos

        protected void gvwHistorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwCierres.SelectedRow;
            this.IdCierre = (int)gvwCierres.DataKeys[filaSeleccionada.RowIndex]["IdCierre"];
            this.IdHotel = (int)gvwCierres.DataKeys[filaSeleccionada.RowIndex]["IdHotel"];
            this.Activo = (bool)gvwCierres.DataKeys[filaSeleccionada.RowIndex]["Activo"];

            string fecha = ((LinkButton)filaSeleccionada.FindControl("lkbFecha")).Text.Split('-')[0].Trim();
            txtAno.Text = fecha.Split('/')[2].Trim();
            ddlMes.SelectedValue = (int.Parse(fecha.Split('/')[1].Trim())).ToString();
            ddlHotel.SelectedValue = this.IdHotel.ToString();

            CierreBo cierreBoTmp = new CierreBo();
            gvwHistorial.DataSource = cierreBoTmp.ListaHistorial(this.IdCierre);
            gvwHistorial.DataBind();

            if (this.Activo) // True abierto
            {
                btnCerrar.Visible = true;
                btnAbrir.Visible = false;                
            }
            else
            {
                btnCerrar.Visible = false;
                btnAbrir.Visible = true;
            }

            btnCancelar.Visible = true;
        }
        #endregion

        #region Boton
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                CierreBo cierreBoTmp = new CierreBo();

                if (this.IdCierre != -1)
                {
                    cierreBoTmp.Actualizar(txtObservacion.Text, this.Activo, ((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdCierre);
                }
                else
                {
                    DateTime fechaFin = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
                    DateTime fechaInicio = new DateTime(fechaFin.Year, fechaFin.Month, 1);

                    fechaFin = Utilities.ObtenerUltimoDiaMes(fechaInicio.Year, fechaInicio.Month);

                    if (!cierreBoTmp.Guardar(fechaInicio, fechaFin, txtObservacion.Text, int.Parse(ddlHotel.SelectedValue), ((ObjetoGenerico)Session["usuarioLogin"]).Id))
                    {
                        this.divExito.Visible = false;
                        this.lbltextoError.Text = Resources.Resource.lblMensajeError_19;
                        this.divError.Visible = true;
                        return;
                    }
                }

                this.Limpiar();

                gvwCierres.DataSource = null;
                gvwCierres.PageIndex = 0;

                odsCierre.SelectParameters["fin"].DefaultValue = this.PageSize.ToString();

                odsCierre.Select();
                odsCierre.DataBind();                

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }            
        }
        protected void btnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                CierreBo cierreBoTmp = new CierreBo();
                cierreBoTmp.Actualizar(txtObservacion.Text, this.Activo, ((ObjetoGenerico)Session["usuarioLogin"]).Id, this.IdCierre);

                this.txtAno.Text = DateTime.Now.Year.ToString();
                txtObservacion.Text = string.Empty;
                ddlHotel.SelectedIndex = -1;
                this.IdCierre = -1;

                gvwHistorial_SelectedIndexChanged(null, null);

                gvwCierres.DataSource = null;
                gvwCierres.PageIndex = 0;

                odsCierre.SelectParameters["fin"].DefaultValue = this.PageSize.ToString();

                odsCierre.Select();
                odsCierre.DataBind();

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Limpiar();

            btnCerrar.Visible = true;
            btnCancelar.Visible = false;
            btnAbrir.Visible = false;
        }
        #endregion
    }
}