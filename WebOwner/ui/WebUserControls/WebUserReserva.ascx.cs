using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM;
using BO;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserReserva : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.CargarCombos();
                CargarGrilla();
            }
        }

        #region Propiedades

        public List<ObjetoGenerico> ListaSuit 
        {
            get { return (List<ObjetoGenerico>)ViewState["ListaSuit"]; }
            set { ViewState["ListaSuit"] = value; }
        }

        #endregion

        #region Metodo

        private void CargarGrilla()
        {
            //ReservasBo reservasBoTmp = new ReservasBo();
            //gvwReservas.DataSource = reservasBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).IdUsuarioPropietario);
            //gvwReservas.DataBind();
        }

        private void CargarCombos()
        {
            SuitPropietarioBo suitPropietarioTmp = new SuitPropietarioBo();            
            ddlHotel.DataSource = suitPropietarioTmp.HotelesPorPropietario(((ObjetoGenerico)Session["usuarioLogin"]).Id);
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();

            ddlHotel_SelectedIndexChanged(null, null);
        }

        private void LimpiarFormulario()
        {
            txtFechaLlegada.Value = "";
            txtFechaSalida.Value = "";
            txtNumAdultos.Text = "1";
            txtNumNinos.Text = "0";
            txtObervacion.Text = "";
        }

        #endregion

        #region Boton

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

            this.btnNuevo.Visible = false;
            this.GrillaHotel.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoReserva.Visible = true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ReservasBo resevasBoTmp = new ReservasBo();

                DateTime fechaLlegada = DateTime.Parse(txtFechaLlegada.Value);
                DateTime fechaSalida = DateTime.Parse(txtFechaSalida.Value);

                int numNinos = 0;
                if (txtNumNinos.Text != string.Empty)
                    numNinos = int.Parse(txtNumNinos.Text);

                int numAdultos = 0;
                if (txtNumAdultos.Text != string.Empty)
                    numAdultos = int.Parse(txtNumAdultos.Text);

                resevasBoTmp.Guardar(((ObjetoGenerico)Session["usuarioLogin"]).Id, int.Parse(ddlSuit.SelectedValue),
                                     numAdultos, numNinos, fechaLlegada, fechaSalida, txtObervacion.Text);

                LimpiarFormulario();
                CargarGrilla();

                this.btnNuevo.Visible = true;
                this.GrillaHotel.Visible = true;

                this.btnGuardar.Visible = false;
                this.NuevoReserva.Visible = false;

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

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            CargarGrilla();

            this.btnNuevo.Visible = true;
            this.GrillaHotel.Visible = true;

            this.btnGuardar.Visible = false;
            this.NuevoReserva.Visible = false;
        }

        #endregion

        #region Eventos

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuitBo suitBoTmp = new SuitBo();
            this.ListaSuit = suitBoTmp.ObtenerSuitsPorHotelFull(int.Parse(ddlHotel.SelectedValue));
            ddlSuit.DataSource = this.ListaSuit;
            ddlSuit.DataTextField = "NumSuit";
            ddlSuit.DataValueField = "IdSuit";
            ddlSuit.DataBind();

            lblCiudad.Text = this.ListaSuit[0].NombreCiudad;
            lblDescripcionSuit.Text = this.ListaSuit[0].Descripcion;
        }

        protected void ddlSuit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjetoGenerico suitTmp = this.ListaSuit.Where(S => S.IdSuit == int.Parse(ddlSuit.SelectedValue)).FirstOrDefault();
            lblDescripcionSuit.Text = suitTmp.Descripcion;
        }

        #endregion

    }
}