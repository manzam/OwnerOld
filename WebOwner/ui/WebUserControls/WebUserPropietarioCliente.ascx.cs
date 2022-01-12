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
    public partial class WebUserPropietarioCliente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.divExito.Visible = false;
            this.divError.Visible = false;

            if (!IsPostBack)
            {
                CargarCombos();
                CargarDatos();
                this.BloquearControl();
            }
        }

        public ObjetoGenerico PropietarioTmp
        {
            get { return (ObjetoGenerico)ViewState["Propietario"]; }
            set { ViewState["Propietario"] = value; }
        }

        #region Metodos

        private void CargarDatos()
        {
            this.PropietarioTmp = (ObjetoGenerico)Session["usuarioLogin"];
            txtNombre.Text = this.PropietarioTmp.PrimeroNombre;
            txtNombreSegundo.Text = this.PropietarioTmp.SegundoNombre;
            txtApellidoPrimero.Text = this.PropietarioTmp.PrimerApellido;
            txtApellidoSegundo.Text = this.PropietarioTmp.SegundoApellido;
            txtNumIdentificacion.Text = this.PropietarioTmp.NumIdentificacion;
            txtTel1.Text = this.PropietarioTmp.Telefono1;
            txtTel2.Text = this.PropietarioTmp.Telefono2;
            txtCorreo.Text = this.PropietarioTmp.Correo;
            txtCorreo2.Text = this.PropietarioTmp.Correo2;
            txtCorreo3.Text = this.PropietarioTmp.Correo3;
            ddlDepto.SelectedValue = this.PropietarioTmp.IdDepto.ToString();
            ddlDepto_SelectedIndexChanged(null, null);
            ddlCiudad.SelectedValue = this.PropietarioTmp.IdCiudad.ToString();
            lblTipoPersona.Text = this.PropietarioTmp.TipoPersona;
            ddlTipoDocumento.SelectedValue = this.PropietarioTmp.TipoDocumento;
            txtDireccion.Text = this.PropietarioTmp.Direccion;
            txtNombreContacto.Text = this.PropietarioTmp.NombreContacto;
            txtTelefonoContacto.Text = this.PropietarioTmp.TelContacto;
            txtCorreoContacto.Text = this.PropietarioTmp.CorreoContacto;
            cbEsRetenedor.Checked = this.PropietarioTmp.EsRetenedor;

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            gvwSuites.DataSource = propietarioBoTmp.ObtenerPropietarioConSuiteAndHotel(this.PropietarioTmp.Id);
            gvwSuites.DataBind();
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

        private void BloquearControl()
        {
            txtNombre.Enabled = false;
            txtNombreSegundo.Enabled = false;
            txtApellidoPrimero.Enabled = false;
            txtApellidoSegundo.Enabled = false;
            txtNumIdentificacion.Enabled = false;
            txtCorreo.Enabled = false;
            txtDireccion.Enabled = false;
            txtTel1.Enabled = false;
            txtTel2.Enabled = false;
            ddlDepto.Enabled = false;
            ddlCiudad.Enabled = false;
            txtNombreContacto.Enabled = false;
            txtTelefonoContacto.Enabled = false;
            txtCorreoContacto.Enabled = false;

            btnGuardar.Visible = false;
            btnCancelar.Visible = false;
            btnActualizar.Visible = true;
        }

        #endregion

        #region Eventos

        protected void ddlDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CiudadBo ciudadBoTmp = new CiudadBo();
            ddlCiudad.DataSource = ciudadBoTmp.ObtenerPorDepto(int.Parse(ddlDepto.SelectedValue));
            ddlCiudad.DataTextField = "Nombre";
            ddlCiudad.DataValueField = "IdCiudad";
            ddlCiudad.DataBind();
        }
        
        #endregion

        #region Boton

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                PropietarioBo propietarioBoTmp = new PropietarioBo();
                propietarioBoTmp.Actualizar(((ObjetoGenerico)Session["usuarioLogin"]).Id, txtCorreo.Text, txtCorreo2.Text, txtCorreo3.Text, int.Parse(ddlCiudad.SelectedValue),
                                            txtDireccion.Text, txtTel1.Text, txtTel2.Text, txtNombreContacto.Text, txtTelefonoContacto.Text, txtCorreoContacto.Text,cbEsRetenedor.Checked);


                this.PropietarioTmp = (ObjetoGenerico)Session["usuarioLogin"];
                this.PropietarioTmp.Correo = txtCorreo.Text;
                this.PropietarioTmp.IdCiudad = int.Parse(ddlCiudad.SelectedValue);
                this.PropietarioTmp.Direccion = txtDireccion.Text;
                this.PropietarioTmp.Telefono1 = txtTel1.Text;
                this.PropietarioTmp.Telefono2 = txtTel2.Text;
                this.PropietarioTmp.NombreContacto = txtNombreContacto.Text;
                this.PropietarioTmp.TelContacto = txtTelefonoContacto.Text;
                this.PropietarioTmp.CorreoContacto = txtCorreoContacto.Text;
                Session["usuarioLogin"] = this.PropietarioTmp;

                this.BloquearControl();

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

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            txtNombre.Enabled = true;
            txtNombreSegundo.Enabled = true;
            txtApellidoPrimero.Enabled = true;
            txtApellidoSegundo.Enabled = true;
            txtNumIdentificacion.Enabled = true;
            txtCorreo.Enabled = true;
            txtDireccion.Enabled = true;
            txtTel1.Enabled = true;
            txtTel2.Enabled = true;
            ddlDepto.Enabled = true;
            ddlCiudad.Enabled = true;
            txtNombreContacto.Enabled = true;
            txtTelefonoContacto.Enabled = true;
            txtCorreoContacto.Enabled = true;

            btnGuardar.Visible = true;
            btnCancelar.Visible = true;
            btnActualizar.Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.BloquearControl();

            btnGuardar.Visible = false;
            btnCancelar.Visible = false;
            btnActualizar.Visible = true;
        }

        #endregion
    }
}