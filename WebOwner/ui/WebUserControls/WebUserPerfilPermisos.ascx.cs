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
    public partial class WebUserPerfilPermisos : System.Web.UI.UserControl
    {
        PerfilBo perfilBo;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.divExito.Visible = false;
            this.divError.Visible = false;

            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        #region Propiedades

        public int IdPerfilSeleccionado
        {
            get { return (int)ViewState["IdPerfilSeleccionado"]; }
            set { ViewState["IdPerfilSeleccionado"] = value; }
        }

        public ObjetoGenerico UsuarioLogin 
        {
            get { return (ObjetoGenerico)((ObjetoGenerico)Session["usuarioLogin"]); }
        }

        #endregion

        #region Metodo

        private void CargarGrilla()
        {
            ModuloBo moduloBoTmp = new ModuloBo();
            gvwModulo.DataSource = moduloBoTmp.VerTodos();
            gvwModulo.DataBind();

            PerfilBo perfilBoTmp = new PerfilBo();
            gvwPerfil.DataSource = perfilBoTmp.VerTodos();
            gvwPerfil.DataBind();
        }

        private void CargarDatosPermiso()
        {
            PerfilBo perfilBoTmp = new PerfilBo();
            Perfil perfilTmp = perfilBoTmp.ObtenerPerfil(this.IdPerfilSeleccionado);
            txtNombre.Text = perfilTmp.Nombre;
            txtDescripcion.Text = perfilTmp.Descripcion;

            ModuloPerfilBo moduloPerfilBoTmp = new ModuloPerfilBo();
            List<Modulo_Perfil> listaModuloPerfilTmp = moduloPerfilBoTmp.VerTodos(this.IdPerfilSeleccionado);

            foreach (GridViewRow filaTmp in this.gvwModulo.Rows)
            {
                int idModulo = int.Parse(gvwModulo.DataKeys[filaTmp.RowIndex].Value.ToString());
                //CheckBox chPermiso = (CheckBox)filaTmp.FindControl("chEsPermitido");
                System.Web.UI.HtmlControls.HtmlInputCheckBox chPermiso = (System.Web.UI.HtmlControls.HtmlInputCheckBox)filaTmp.FindControl("chEsPermitido");

                if (listaModuloPerfilTmp.Where(MP => MP.Modulo.IdModulo == idModulo).Count() > 0)
                    chPermiso.Checked = true;
                else
                    chPermiso.Checked = false;
            }
        }

        private void LimpiarCheckGrilla()
        {
            foreach (GridViewRow filaTmp in this.gvwModulo.Rows)
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox chPermiso = (System.Web.UI.HtmlControls.HtmlInputCheckBox)filaTmp.FindControl("chEsPermitido");
                chPermiso.Checked = false;
            }
        }

        #endregion

        #region Boton

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCheckGrilla();

            NuevoModulo.Visible = true;
            btnGuardar.Visible = true;
            btnVerTodos.Visible = true;
            txtNombre.Enabled = true;
            txtDescripcion.Enabled = true;

            btnActualizar.Visible = false;
            btnNuevo.Visible = false;
            GrillaPerfil.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Perfil perfilTmp = new Perfil();
                perfilTmp.Nombre = txtNombre.Text;
                perfilTmp.Descripcion = txtDescripcion.Text;

                string permisos = string.Empty;
                foreach (GridViewRow filaTmp in this.gvwModulo.Rows)
                {                   
                    int idModuloTmp = int.Parse(gvwModulo.DataKeys[filaTmp.RowIndex].Value.ToString());

                    System.Web.UI.HtmlControls.HtmlInputCheckBox chPermiso = (System.Web.UI.HtmlControls.HtmlInputCheckBox)filaTmp.FindControl("chEsPermitido");
                    //CheckBox chPermiso = (CheckBox)filaTmp.FindControl("chEsPermitido");

                    if (chPermiso.Checked)
                    {
                        Modulo_Perfil moduloPerfilTmp = new Modulo_Perfil();
                        moduloPerfilTmp.ModuloReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Modulo", "IdModulo", idModuloTmp);
                        perfilTmp.Modulo_Perfil.Add(moduloPerfilTmp);
                        permisos += filaTmp.Cells[0].Text + ",";
                    }
                }

                permisos = permisos.TrimEnd(new char[] { ',' });

                PerfilBo perfilBoTmp = new PerfilBo();
                perfilBoTmp.Guardar(perfilTmp, permisos, UsuarioLogin.Id);

                btnVerTodos_Click(null, null);

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

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Modulo_Perfil> listaModuloPerfil = new List<Modulo_Perfil>();

                string permisos = string.Empty;
                foreach (GridViewRow filaTmp in this.gvwModulo.Rows)
                {                    
                    int idModuloTmp = int.Parse(gvwModulo.DataKeys[filaTmp.RowIndex].Value.ToString());
                    System.Web.UI.HtmlControls.HtmlInputCheckBox chPermiso = (System.Web.UI.HtmlControls.HtmlInputCheckBox)filaTmp.FindControl("chEsPermitido");

                    if (chPermiso.Checked)
                    {
                        Modulo_Perfil moduloPerfilTmp = new Modulo_Perfil();
                        moduloPerfilTmp.ModuloReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Modulo", "IdModulo", idModuloTmp);
                        moduloPerfilTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", this.IdPerfilSeleccionado);
                        listaModuloPerfil.Add(moduloPerfilTmp);
                        permisos += filaTmp.Cells[0].Text + ",";
                    }
                }
                permisos = permisos.TrimEnd(new char[] { ',' });

                PerfilBo perfilBoTmp = new PerfilBo();
                perfilBoTmp.Actualizar(this.IdPerfilSeleccionado, txtNombre.Text, txtDescripcion.Text, permisos, this.UsuarioLogin.Id);

                ModuloPerfilBo moduloPerfilBoTmp = new ModuloPerfilBo();
                moduloPerfilBoTmp.Actualizar(listaModuloPerfil, this.IdPerfilSeleccionado);

                btnVerTodos_Click(null, null);

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

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            this.CargarGrilla();

            this.btnNuevo.Visible = true;
            this.GrillaPerfil.Visible = true;

            this.NuevoModulo.Visible = false;
            this.btnGuardar.Visible = false;
            this.btnActualizar.Visible = false;

            txtDescripcion.Text = "";
            txtNombre.Text = "";
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                perfilBo = new PerfilBo();
                ImageButton imgBtn = (ImageButton)sender;

                if (perfilBo.Eliminar(int.Parse(imgBtn.CommandArgument)))
                {
                    this.CargarGrilla();

                    this.divExito.Visible = true;
                    this.divError.Visible = false;
                    this.lbltextoExito.Text = "Registro eliminado con exito.";
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_9;
                    btnVerTodos_Click(null, null);
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

        #region Evento

        protected void gvwPerfil_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwPerfil.SelectedRow;
            this.IdPerfilSeleccionado = int.Parse(gvwPerfil.DataKeys[filaSeleccionada.RowIndex].Value.ToString());

            this.CargarDatosPermiso();

            this.btnNuevo.Visible = false;
            this.GrillaPerfil.Visible = false;
            this.btnGuardar.Visible = false;

            this.btnActualizar.Visible = true;
            this.NuevoModulo.Visible = true;

            txtDescripcion.Enabled = true;
            txtNombre.Enabled = true;
        }

        #endregion
    }
}