using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using BO;
using DM;
using Servicios;

namespace WebOwner.UI.WebControls
{
    public partial class WebUserHotel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;
            WebUserBuscadorSuite.AlAceptar += new EventHandler(WebUserBuscadorSuite_AlAceptar);

            if (!IsPostBack)
            {
                this.IdSuiteBuscada = -1;

                //ParametroBo parametroBoTmp = new ParametroBo();
                //gvwSuits.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));

                this.btnAgregarSuit.Enabled = false;
                this.AsyncFileUpload1.Enabled = false;
                CargarGrilla();
                CargarCombos();
            }
        }

        #region Propiedades

        public int IdHotelSeleccionado 
        { 
            get { return (int)ViewState["IdHotelSeleccionado"]; }
            set { ViewState["IdHotelSeleccionado"] = value; } 
        }

        public int IdSuiteBuscada
        {
            get { return (int)ViewState["IdSuiteBuscada"]; }
            set { ViewState["IdSuiteBuscada"] = value; }
        }

        public Suit SuitSeleccionado
        {
            get { return (Suit)ViewState["SuitSeleccionado"]; }
            set { ViewState["SuitSeleccionado"] = value; }
        }

        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }

        #endregion

        #region Metodos

        private void LimpiarFormulario()
        {
            txtCorreo.Text = "";
            txtDireccion.Text = "";
            txtNit.Text = "";
            txtNombre.Text = "";
            txtCodigo.Text = "";

            AsyncFileUpload1.Enabled = true;

            gvwSuits.DataSource = null;
            gvwSuits.DataBind();
        }

        private void CargarGrilla()
        {
            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);
            
            gvwHoteles.DataSource = listaHotel;
            gvwHoteles.DataBind();
        }

        private void CargarCombos()
        {
            DeptoBo deptoBoTmp = new DeptoBo();
            ddlDepto.DataSource = deptoBoTmp.ObtenerTodos();
            ddlDepto.DataTextField = "Nombre";
            ddlDepto.DataValueField = "IdDepartamento";
            ddlDepto.DataBind();

            ddlDepto_SelectedIndexChanged(null, null);
        }

        private void CargarGrillaSuits()
        {
            SuitBo suitBoTmp = new SuitBo();
            gvwSuits.DataSource = suitBoTmp.ObtenerSuitsPorHotel(this.IdHotelSeleccionado);
            gvwSuits.DataBind();
        }

        private void CargarDatosHotel(int idHotel)
        {
            HotelBo hotelBoTmp = new HotelBo();
            ObjetoGenerico hotelTmp = hotelBoTmp.ObtenerHotel(idHotel);

            txtNombre.Text = hotelTmp.Nombre;
            txtDireccion.Text = hotelTmp.Direccion;
            txtNit.Text = hotelTmp.Nit;
            txtCorreo.Text = hotelTmp.Correo;
            txtCodigo.Text = hotelTmp.Codigo;
            txtUnidadNegocio.Text = hotelTmp.UnidadNegocioHotel;
            imgLogo.ImageUrl = hotelTmp.RutaLogo;
            ddlDepto.SelectedValue = hotelTmp.IdDepto.ToString();
            ddlDepto_SelectedIndexChanged(null, null);
            ddlCiudad.SelectedValue = hotelTmp.IdCiudad.ToString();
        }

        #endregion

        #region Eventos

        void WebUserBuscadorSuite_AlAceptar(object sender, EventArgs e)
        {
            this.IdSuiteBuscada = WebUserBuscadorSuite.IdSuiteBuscado;
            
            SuitBo suitBoTmp = new SuitBo();
            List<Suit> lista = new List<Suit>();
            lista.Add(suitBoTmp.ObtenerSuit(this.IdSuiteBuscada));
            gvwSuits.DataSource = lista;
            gvwSuits.DataBind();
        }

        protected void ddlDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CiudadBo ciudadBoTmp = new CiudadBo();
            ddlCiudad.DataSource = ciudadBoTmp.ObtenerPorDepto(int.Parse(ddlDepto.SelectedValue));
            ddlCiudad.DataTextField = "Nombre";
            ddlCiudad.DataValueField = "IdCiudad";
            ddlCiudad.DataBind();
        }

        protected void gvwHoteles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwHoteles.SelectedRow;
            this.IdHotelSeleccionado = int.Parse(gvwHoteles.DataKeys[filaSeleccionada.RowIndex].Value.ToString());

            this.CargarDatosHotel(this.IdHotelSeleccionado);
            this.CargarGrillaSuits();

            this.btnAgregarSuit.Enabled = true;
            this.AsyncFileUpload1.Enabled = true;

            this.btnNuevo.Visible = false;            
            this.GrillaHotel.Visible = false;
            this.btnGuardar.Visible = false;

            this.btnActualizar.Visible = true;            
            this.NuevoHotel.Visible = true;
            this.btnBuscar.Visible = true;
        }

        protected void gvwSuits_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwSuits.SelectedRow;
            SuitBo suitBoTmp = new SuitBo();
            int idSuit = int.Parse(gvwSuits.DataKeys[filaSeleccionada.RowIndex].Value.ToString());
            this.SuitSeleccionado = suitBoTmp.ObtenerSuit(idSuit);

            txtNombre.Text = this.SuitSeleccionado.Descripcion;
            txtDescripcionSuit.Value = this.SuitSeleccionado.Descripcion;
            chActivoSuit.Checked = this.SuitSeleccionado.Activo;            
        }

        protected void gvwSuits_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvwSuits.EditIndex = e.NewEditIndex;

            if (this.IdSuiteBuscada != -1)
            {
                SuitBo suitBoTmp = new SuitBo();
                List<Suit> lista = new List<Suit>();
                lista.Add(suitBoTmp.ObtenerSuit(this.IdSuiteBuscada));
                gvwSuits.DataSource = lista;
                gvwSuits.DataBind();
            }
            else                
                this.CargarGrillaSuits();
        }

        protected void gvwSuits_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvwSuits.EditIndex = -1;
            this.CargarGrillaSuits();
        }

        protected void gvwSuits_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                SuitBo suitBoTmp = new SuitBo();
                int idSuit = int.Parse(gvwSuits.DataKeys[e.RowIndex].Value.ToString());
                string descripcion = ((TextBox)(gvwSuits.Rows[e.RowIndex].FindControl("txtDescripcion"))).Text;
                bool activo = ((CheckBox)(gvwSuits.Rows[e.RowIndex].FindControl("chActivo"))).Checked;
                string numSuit = ((TextBox)(gvwSuits.Rows[e.RowIndex].FindControl("txtNumSuit"))).Text;
                string escritura = ((TextBox)(gvwSuits.Rows[e.RowIndex].FindControl("txtEscritura"))).Text;
                string registroNotaria = ((TextBox)(gvwSuits.Rows[e.RowIndex].FindControl("txtRegistroNotaria"))).Text;

                suitBoTmp.Actualizar(idSuit, descripcion, activo, numSuit, escritura, registroNotaria, this.IdHotelSeleccionado, UsuarioLogin.Id);

                gvwSuits.EditIndex = -1;
                this.CargarGrillaSuits();

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
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

        protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string filename = System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
            AsyncFileUpload1.SaveAs(Server.MapPath("../../img/imgLogo/") + filename);

            HotelBo hotelBoTmp = new HotelBo();
            hotelBoTmp.GuardarRutaLogo(this.IdHotelSeleccionado, "~/img/imgLogo/" + filename);

            imgLogo.ImageUrl = Server.MapPath("~/img/imgLogo/") + filename;
        }

        protected void gvwSuits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwSuits.PageIndex = e.NewPageIndex;
            this.CargarGrillaSuits();
        }

        #endregion

        #region Botones

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            this.IdSuiteBuscada = -1;

            try
            {
                ImageButton imgButton = (ImageButton)sender;
                SuitBo suitBoTmp = new SuitBo();

                if (suitBoTmp.Eliminar(int.Parse(imgButton.CommandArgument), this.UsuarioLogin.Id))
                {
                    lbltextoExito.Text = Resources.Resource.lblMensajeEliminar;
                    divExito.Visible = true;
                    divError.Visible = false;

                    gvwSuits.EditIndex = -1;
                    this.CargarGrillaSuits();
                }
                else
                {
                    lbltextoError.Text = Resources.Resource.lblMensajeError_1;
                    divError.Visible = true;
                    divExito.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnAceptarSuit_Click(object sender, EventArgs e)
        {
            Suit suitTmp = new Suit();

            try
            {
                SuitBo suitBoTmp = new SuitBo();

                if (suitBoTmp.EsRepetido(txtNumSuit.Value, this.IdHotelSeleccionado))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_12;
                }
                else
                {
                    suitBoTmp.Guardar(txtDescripcionSuit.Value, chActivoSuit.Checked, txtNumSuit.Value, txtEscritura.Value, txtRegistroNotaria.Value,
                                      this.IdHotelSeleccionado, UsuarioLogin.Id);
                    this.CargarGrillaSuits();
                }
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_8;
            }            
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            this.IdSuiteBuscada = -1;

            try
            {
                HotelBo hotelBoTmp = new HotelBo();
                  
                SuitBo suitBoTmp = new SuitBo();
                hotelBoTmp.Actualizar(this.IdHotelSeleccionado, txtNombre.Text, txtDireccion.Text, txtNit.Text, 
                                      txtCorreo.Text, txtCodigo.Text, txtUnidadNegocio.Text, int.Parse(ddlCiudad.SelectedValue));
                CargarGrillaSuits();

                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
                divExito.Visible = true;
                divError.Visible = false;
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
            this.IdSuiteBuscada = -1;

            LimpiarFormulario();
            CargarGrilla();

            this.btnNuevo.Visible = true;
            this.GrillaHotel.Visible = true;

            this.btnActualizar.Visible = false;
            this.btnGuardar.Visible = false;
            this.NuevoHotel.Visible = false;
            this.btnBuscar.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            this.IdSuiteBuscada = -1;

            try
            {
                HotelBo hotelBoTmp = new HotelBo();

                DM.Hotel hotelTmp = new DM.Hotel();
                hotelTmp.Nombre = txtNombre.Text;
                hotelTmp.Direccion = txtDireccion.Text;
                hotelTmp.Nit = txtNit.Text;
                hotelTmp.Correo = txtCorreo.Text;
                hotelTmp.Codigo = txtCodigo.Text;
                hotelTmp.UnidadNegocio = txtUnidadNegocio.Text;
                hotelTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", int.Parse(ddlCiudad.SelectedValue));

                this.IdHotelSeleccionado = hotelBoTmp.Guardar(hotelTmp, UsuarioLogin.Id);

                CargarDatosHotel(this.IdHotelSeleccionado);

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;

                this.btnAgregarSuit.Enabled = true;
                this.AsyncFileUpload1.Enabled = true;
                this.btnActualizar.Visible = true;

                this.btnGuardar.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            btnAgregarSuit.Enabled = false;
            this.IdHotelSeleccionado = -1; //limpiamos el id por precaucion.
            this.IdSuiteBuscada = -1;

            LimpiarFormulario();

            this.btnBuscar.Visible = false;
            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.GrillaHotel.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoHotel.Visible = true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            WebUserBuscadorSuite.Limpiar();
            WebUserBuscadorSuite.IdHotel = IdHotelSeleccionado;
        }

        #endregion
    }
}