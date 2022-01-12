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
    public partial class WebUserCentroCosto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        #region Propiedades

        public int IdCeCoSeleccionado
        {
            get { return (int)ViewState["IdCeCoSeleccionado"]; }
            set { ViewState["IdCeCoSeleccionado"] = value; }
        }
        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }

        #endregion


        #region Metodos

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtCodigo.Text = "";
        }

        private void CargarGrilla()
        {
            CentroCostoBo centroCostoBoTmp = new CentroCostoBo();
            gvwCeCo.DataSource = centroCostoBoTmp.VerTodos();
            gvwCeCo.DataBind();
        }

        private void CargarDatosCeCo()
        {
            CentroCostoBo centroCostoBoTmp = new CentroCostoBo();
            Centro_Costo centroCostoTmp = centroCostoBoTmp.Obtener(this.IdCeCoSeleccionado);
            
            txtNombre.Text = centroCostoTmp.Nombre;
            txtCodigo.Text = centroCostoTmp.Codigo;
        }

        #endregion

        #region Eventos

        protected void gvwCeCo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwCeCo.SelectedRow;
            this.IdCeCoSeleccionado = int.Parse(gvwCeCo.DataKeys[filaSeleccionada.RowIndex].Value.ToString());

            this.CargarDatosCeCo();

            this.btnNuevo.Visible = false;
            this.GrillaCeCo.Visible = false;
            this.btnGuardar.Visible = false;

            this.btnActualizar.Visible = true;
            this.NuevoCeCo.Visible = true;
        }

        #endregion

        #region Botones

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CentroCostoBo centroCostoBoTmp = new CentroCostoBo();

                if (centroCostoBoTmp.EsRepetido(txtCodigo.Text, this.IdCeCoSeleccionado))
                {
                    centroCostoBoTmp.Actualizar(this.IdCeCoSeleccionado, txtNombre.Text, txtCodigo.Text, this.UsuarioLogin.Id);

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
            this.GrillaCeCo.Visible = true;

            this.btnActualizar.Visible = false;
            this.btnGuardar.Visible = false;
            this.NuevoCeCo.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                CentroCostoBo centroCostoBoTmp = new CentroCostoBo();
                if (centroCostoBoTmp.EsRepetido(txtCodigo.Text, -1))
                {
                    centroCostoBoTmp.Guardar(txtNombre.Text, txtCodigo.Text, this.UsuarioLogin.Id);
                    CargarGrilla();

                    btnVerTodos_Click(null, null);

                    this.divExito.Visible = true;
                    this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IdCeCoSeleccionado = -1; //limpiamos el id por precaucion.

            LimpiarFormulario();

            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.GrillaCeCo.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoCeCo.Visible = true;
        }

        #endregion
    }
}