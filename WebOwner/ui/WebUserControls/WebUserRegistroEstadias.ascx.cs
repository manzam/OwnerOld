using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using DM;
using System.Drawing;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserRegistroEstadias : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            uc_WebUserBuscadorPropietario.AlAceptar += new EventHandler(uc_WebUserBuscadorPropietario_AlAceptar);

            if (!IsPostBack)
            {
                ParametroBo parametroBoTmp = new ParametroBo();
                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                gvwPropietario.PageSize = this.PageSize;

                this.CargarCombos();
                ddlhotel_SelectedIndexChanged(null, null);

                for (int i = DateTime.Now.Year - 10; i <= DateTime.Now.Year; i++)
                {
                    ListItem item = new ListItem(i.ToString(), i.ToString(), true);
                    ddlMes.Items.Add(item);
                }

                ddlMes.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;

                this.SumaEstadias = 0;
            }
        }

        #region Propiedades

        public int IdPropietario { get { return (int)ViewState["IdPropietario"]; } set { ViewState["IdPropietario"] = value; } }
        public int IdSuit { get { return (int)ViewState["IdSuit"]; } set { ViewState["IdSuit"] = value; } }
        public int SumaEstadias { get { return (int)ViewState["SumaEstadias"]; } set { ViewState["SumaEstadias"] = value; } }
        public int PageSize { get { return (int)ViewState["PageSize"]; } set { ViewState["PageSize"] = value; } }

        #endregion

        #region Metodos

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlhotel.DataSource = listaHotel;
            ddlhotel.DataTextField = "Nombre";
            ddlhotel.DataValueField = "IdHotel";
            ddlhotel.DataBind();
        }

        private void CargarPropietarios()
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            gvwPropietario.DataSource = propietarioBoTmp.ObtenerPropietariosConSuite(int.Parse(ddlhotel.SelectedValue));
            gvwPropietario.DataBind();
        }

        private void CargarHistorial()
        {
            this.SumaEstadias = 0;

            HistorialEstadiasBo historialEstadiasBoTmp = new HistorialEstadiasBo();
            gvwEstadias.DataSource = historialEstadiasBoTmp.VerTodos(this.IdSuit, this.IdPropietario);
            gvwEstadias.DataBind();
        }

        private void limpiar()
        {
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtObservacion.Text = "";
        }

        #endregion

        #region Boton

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            errorI.Visible = false;
            errorF.Visible = false;

            if (txtFechaInicio.Text == string.Empty)
            {
                errorI.Visible = true;
                return;
            }
            if (txtFechaFin.Text == string.Empty)
            {
                errorF.Visible = true;
                return;
            }

            try
            {
                DateTime fechaLlegada = DateTime.Parse(txtFechaInicio.Text);
                DateTime fechaSalida = DateTime.Parse(txtFechaFin.Text);

                int d = fechaSalida.Subtract(fechaLlegada).Days;

                if (d < 0)
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_17;
                    return;
                }

                if ((d + this.SumaEstadias) > int.Parse(lblNumEstadias.Text))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_18;
                    return;
                }

                HistorialEstadiasBo historialEstadiasBoTmp = new HistorialEstadiasBo();
                historialEstadiasBoTmp.Guardar(DateTime.Parse(txtFechaInicio.Text), DateTime.Parse(txtFechaFin.Text), txtObservacion.Text,
                                               this.IdSuit, this.IdPropietario, ((ObjetoGenerico)Session["usuarioLogin"]).Id);
                limpiar();
                CargarHistorial();

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_13;
            }

            
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            gvwPropietario.PageIndex = 0;
            this.CargarPropietarios();

            pnlDetallePropietario.Visible = false;
            pnlGrillaPropietarios.Visible = true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            uc_WebUserBuscadorPropietario.PageSize = this.PageSize;
            uc_WebUserBuscadorPropietario.IdPropietarioBuscado = -1;
            uc_WebUserBuscadorPropietario.IdSuiteBuscado = -1;
            //uc_WebUserBuscadorPropietario.CargarCombo();
            uc_WebUserBuscadorPropietario.CargarGrilla();
        }

        #endregion

        #region Eventos

        protected void ddlhotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SumaEstadias = 0;
            this.CargarPropietarios();
        }    

        protected void gvwPropietario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwPropietario.PageIndex = e.NewPageIndex;
            this.CargarPropietarios();
        }

        protected void gvwPropietario_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.IdPropietario = int.Parse(gvwPropietario.DataKeys[e.NewSelectedIndex]["IdPropietario"].ToString());
            this.IdSuit = int.Parse(gvwPropietario.DataKeys[e.NewSelectedIndex]["IdSuit"].ToString());

            string numEstadias = gvwPropietario.DataKeys[e.NewSelectedIndex]["NumEstadias"].ToString();
            string numSuit = gvwPropietario.DataKeys[e.NewSelectedIndex]["NumSuit"].ToString();
            string numEscritura = gvwPropietario.DataKeys[e.NewSelectedIndex]["NumEscritura"].ToString();
            string nombre = ((LinkButton)gvwPropietario.Rows[e.NewSelectedIndex].FindControl("lbNombre")).Text;

            this.lblNombre.Text = nombre;
            this.lblEscritura.Text = numEscritura;
            this.lblNumSuit.Text = numSuit;
            this.lblNumEstadias.Text = numEstadias;

            CargarHistorial();

            this.pnlGrillaPropietarios.Visible = false;
            this.pnlDetallePropietario.Visible = true;
        }

        protected void gvwEstadias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime fechaLlegada = DateTime.Parse(e.Row.Cells[0].Text);
                DateTime fechaSalida = DateTime.Parse(e.Row.Cells[1].Text);

                int d = fechaSalida.Subtract(fechaLlegada).Days;

                e.Row.Cells[2].Text = ((d == 0) ? 1 : d).ToString();

                this.SumaEstadias = this.SumaEstadias + int.Parse(e.Row.Cells[2].Text);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].CssClass = "textoTabla";
                e.Row.Cells[1].Text = this.SumaEstadias.ToString();
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].BorderColor = Color.FromArgb(117, 153, 169);

                e.Row.Cells.RemoveAt(2);

                if (this.SumaEstadias > int.Parse(lblNumEstadias.Text))
                    btnGuardar.Enabled = false;
            }
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SumaEstadias = 0;
            HistorialEstadiasBo historialEstadiasBoTmp = new HistorialEstadiasBo();
            gvwEstadias.DataSource = historialEstadiasBoTmp.ObtenerHistorial(int.Parse(ddlMes.SelectedValue), this.IdSuit, this.IdPropietario);
            gvwEstadias.DataBind();
        }

        protected void gvwEstadias_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells.RemoveAt(2);
                e.Row.Cells.RemoveAt(3);
            }
        }

        void uc_WebUserBuscadorPropietario_AlAceptar(object sender, EventArgs e)
        {
            if (uc_WebUserBuscadorPropietario.IdPropietarioBuscado == -1 || uc_WebUserBuscadorPropietario.IdSuiteBuscado == -1)
                return;

            this.IdPropietario = uc_WebUserBuscadorPropietario.IdPropietarioBuscado;
            this.IdSuit = uc_WebUserBuscadorPropietario.IdSuiteBuscado;

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(this.IdPropietario, this.IdSuit);

            this.IdSuit = propietarioTmp.IdSuit;
            this.lblNombre.Text = propietarioTmp.Nombre;
            this.lblEscritura.Text = propietarioTmp.NumEscritura;
            this.lblNumSuit.Text = propietarioTmp.NumSuit;
            this.lblNumEstadias.Text = propietarioTmp.NumEstadias.ToString();

            CargarHistorial();

            this.pnlGrillaPropietarios.Visible = false;
            this.pnlDetallePropietario.Visible = true;
        }

        #endregion        
    }
}