using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using DM;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserCuentaContable : System.Web.UI.UserControl
    {
        CuentaContableBo cuentaContableBo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.CargarCombo();
                this.CargarGrilla();
                chbCentroCosto_CheckedChanged(null, null);
            }
        }

        #region Propiedades

        public int IdCuentaContableSeleccionado
        {
            get { return (int)ViewState["IdCuentaContableSeleccionado"]; }
            set { ViewState["IdCuentaContableSeleccionado"] = value; }
        }

        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }

        #endregion

        #region Metodos

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtEncabezadoCruce.Text = string.Empty;
            txtUnidadNegocio.Text = string.Empty;
            chbCentroCosto.Checked = false;
            chbDocCruce.Checked = false;
            ddlNaturaleza.SelectedIndex = -1;
            ddlTipoCuenta.SelectedIndex = -1;
        }

        private void CargarGrilla()
        {
            CuentaContableBo CuentaContableBoTmp = new CuentaContableBo();
            gvwCuentaContable.DataSource = CuentaContableBoTmp.VerTodos();
            gvwCuentaContable.DataBind();
        }

        private void CargarCombo()
        {
            TipoCuentaContableBo tipoCuentaContableBoTmp = new TipoCuentaContableBo();
            ddlTipoCuenta.DataSource = tipoCuentaContableBoTmp.VerTodos();
            ddlTipoCuenta.DataValueField = "IdTipoCuentaContable";
            ddlTipoCuenta.DataTextField = "Nombre";
            ddlTipoCuenta.DataBind();

            CentroCostoBo centroCostoBoTmp = new CentroCostoBo();
            ddlCentroCosto.DataSource = centroCostoBoTmp.VerTodos();
            ddlCentroCosto.DataValueField = "IdCentroCosto";
            ddlCentroCosto.DataTextField = "Nombre";
            ddlCentroCosto.DataBind();
        }

        private void CargarDatosCeCo()
        {
            CuentaContableBo CuentaContableBoTmp = new CuentaContableBo();
            Cuenta_Contable cuentaContableTmp = CuentaContableBoTmp.Obtener(this.IdCuentaContableSeleccionado);

            txtNombre.Text = cuentaContableTmp.Nombre;
            txtCodigo.Text = cuentaContableTmp.Codigo;            
            chbCentroCosto.Checked = cuentaContableTmp.EsCentroCostoVariable;            
            txtEncabezadoCruce.Text = cuentaContableTmp.EncabezadoDocCruce;
            ddlNaturaleza.SelectedValue = cuentaContableTmp.NaturalezaCuenta;
            txtUnidadNegocio.Text = cuentaContableTmp.UnidadNegocio;

            if (cuentaContableTmp.Tipo_Cuenta_Contable != null)
                ddlTipoCuenta.SelectedValue = cuentaContableTmp.Tipo_Cuenta_Contable.IdTipoCuentaContable.ToString();

            if (cuentaContableTmp.TieneDocumentoCruce != null)
                chbDocCruce.Checked = cuentaContableTmp.TieneDocumentoCruce.Value;

            chbCentroCosto_CheckedChanged(null, null);
        }

        #endregion

        #region Eventos

        protected void gvwCuentaContable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwCuentaContable.SelectedRow;
            this.IdCuentaContableSeleccionado = int.Parse(gvwCuentaContable.DataKeys[filaSeleccionada.RowIndex].Value.ToString());

            this.CargarDatosCeCo();

            this.btnNuevo.Visible = false;
            this.GrillaCuentaContable.Visible = false;
            this.btnGuardar.Visible = false;

            this.btnActualizar.Visible = true;
            this.NuevoCuentaContable.Visible = true;
        }

        protected void chbCentroCosto_CheckedChanged(object sender, EventArgs e)
        {
            ddlCentroCosto.Enabled = chbCentroCosto.Checked;
        }

        #endregion

        #region Botones

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CuentaContableBo CuentaContableBoTmp = new CuentaContableBo();

                if (CuentaContableBoTmp.EsRepetido(txtCodigo.Text, this.IdCuentaContableSeleccionado))
                {
                    CuentaContableBoTmp.Actualizar(this.IdCuentaContableSeleccionado, txtCodigo.Text, txtNombre.Text, 
                                                   chbCentroCosto.Checked, chbDocCruce.Checked, txtEncabezadoCruce.Text, 
                                                   ddlNaturaleza.SelectedValue, int.Parse(ddlTipoCuenta.SelectedValue),
                                                   txtUnidadNegocio.Text, ddlTipoCuenta.SelectedItem.Text, UsuarioLogin.Id);

                    btnVerTodos_Click(null, null);

                    this.divExito.Visible = true;
                    this.lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
                    this.divError.Visible = false;
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_16;
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

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            CargarGrilla();

            this.btnNuevo.Visible = true;
            this.GrillaCuentaContable.Visible = true;

            this.btnActualizar.Visible = false;
            this.btnGuardar.Visible = false;
            this.NuevoCuentaContable.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                CuentaContableBo CuentaContableBoTmp = new CuentaContableBo();
                if (CuentaContableBoTmp.EsRepetido(txtCodigo.Text, -1))
                {
                    CuentaContableBoTmp.Guardar(txtCodigo.Text, txtNombre.Text, chbCentroCosto.Checked, chbDocCruce.Checked,
                                                txtEncabezadoCruce.Text, ddlNaturaleza.SelectedValue,
                                                int.Parse(ddlTipoCuenta.SelectedValue), txtUnidadNegocio.Text, ddlTipoCuenta.SelectedItem.Text, UsuarioLogin.Id);

                    //if (chbTerceroVariable.Checked)
                    //{
                    //    Response.Redirect("RelacionCuentaHotel.aspx");
                    //}
                    //else
                    //{
                        CargarGrilla();

                        btnVerTodos_Click(null, null);

                        this.divExito.Visible = true;
                        this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                        this.divError.Visible = false;
                    //}
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_16;
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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IdCuentaContableSeleccionado = -1; //limpiamos el id por precaucion.

            LimpiarFormulario();

            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.GrillaCuentaContable.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoCuentaContable.Visible = true;
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgButton = (ImageButton)sender;
                cuentaContableBo = new CuentaContableBo();

                if (cuentaContableBo.EliminarCuentaContable(int.Parse(imgButton.CommandArgument), UsuarioLogin.Id))
                {
                    this.divExito.Visible = true;
                    this.divError.Visible = false;
                    this.lbltextoExito.Text = "Cuenta contable eliminada con exito.";

                    btnVerTodos_Click(null, null);
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_9;
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

        #endregion
    }
}