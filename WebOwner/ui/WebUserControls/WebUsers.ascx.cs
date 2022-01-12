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
    public partial class WebUserUsers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.CargarCombos();
                //this.CargarGrilla();
                this.CargarHotel();

                ddlHotel_SelectedIndexChanged(null, null);
            }
        }

        #region Propiedades

        public int IdUsuarioSeleccionado
        {
            get { return (int)ViewState["IdUsuarioSeleccionado"]; }
            set { ViewState["IdUsuarioSeleccionado"] = value; }
        }
        public int IdPerfilSeleccionado
        {
            get { return (int)ViewState["IdPerfilSeleccionado"]; }
            set { ViewState["IdPerfilSeleccionado"] = value; }
        }

        #endregion

        #region Metodos

        public void CargarCombos()
        {
            PerfilBo perfilBoTmp = new PerfilBo();
            List<Perfil> listaperfil = perfilBoTmp.VerTodos();
            listaperfil.RemoveAll(P => P.IdPerfil == Properties.Settings.Default.IdPropietario);

            ddlPerfil.DataSource = listaperfil;
            ddlPerfil.DataTextField = "Nombre";
            ddlPerfil.DataValueField = "IdPerfil";
            ddlPerfil.DataBind();

            ddlPerfil.Items.Add(new ListItem("Sin perfil", "-1"));

            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = null;

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
        }

        public void CargarGrilla()
        {
            UsuarioBo usuarioBoTmp = new UsuarioBo();
            gvwUsuario.DataSource = usuarioBoTmp.VerTodosPorHotelUsuarios(int.Parse(ddlHotel.SelectedValue), Properties.Settings.Default.IdSuperUsuario);
            gvwUsuario.DataBind();
        }

        public void CargarHotel()
        {
            HotelBo hotelBoTmp = new HotelBo();
            gvwHotel.DataSource = hotelBoTmp.VerTodos();
            gvwHotel.DataBind();
        }

        public void LimpiarFormulario()
        {
            this.btnReset.Visible = false;
            txtLogin.Enabled = true;

            txtLogin.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtNumIdentificacion.Text = "";
            txtCorreo.Text = "";
            txtTel1.Text = "";
            txtTel2.Text = "";
            chActivo.Checked = true;
            ddlPerfil.SelectedIndex = -1;
            ddlPerfil.Enabled = true;

            HotelSeleccionado(false, true);
        }

        public void CargarDatosUsuario()
        {
            UsuarioBo usuarioBoTmp = new UsuarioBo();
            ObjetoGenerico propietarioTmp = usuarioBoTmp.Obtener(this.IdUsuarioSeleccionado);

            txtNombre.Text = propietarioTmp.Nombre;
            txtApellido.Text = propietarioTmp.Apellido;
            txtCorreo.Text = propietarioTmp.Correo;
            txtNumIdentificacion.Text = propietarioTmp.NumIdentificacion;
            txtTel1.Text = propietarioTmp.Telefono1;
            txtTel2.Text = propietarioTmp.Telefono2;
            chActivo.Checked = propietarioTmp.Activo;
            txtLogin.Text = propietarioTmp.Login;

            chActivo_CheckedChanged(null, null);

            //ddlPerfil.SelectedValue = this.IdPerfilSeleccionado.ToString();
            ddlPerfil.SelectedValue = propietarioTmp.IdPerfil.ToString();
        }

        public void CargarHotelesUsuario()
        {
            UsuarioBo usuarioBoTmp = new UsuarioBo();
            bool esAdmon = false;
            if (this.IdPerfilSeleccionado == Properties.Settings.Default.IdSuperUsuario)
                esAdmon = true;

            List<int> listaId = usuarioBoTmp.ObtenerHotelesPorUsuario(this.IdUsuarioSeleccionado, esAdmon);
            Session["listaIdHotel"] = listaId;

            foreach (GridViewRow fila in gvwHotel.Rows)
            {
                int idhotel = int.Parse(gvwHotel.DataKeys[fila.RowIndex].Value.ToString());
                ImageButton imgBtnTmp = (ImageButton)fila.Cells[1].FindControl("imgBtnSeleccion");

                if (listaId.Where(x => x == idhotel).Count() > 0)
                    imgBtnTmp.ImageUrl = "~/img/95.png";
                else
                    imgBtnTmp.ImageUrl = "~/img/117.png";
            }
        }

        public void HotelSeleccionado(bool EsSeleccioando, bool enable)
        {
            List<int> listaIdHotel = new List<int>();

            foreach (GridViewRow fila in gvwHotel.Rows)
            {
                ImageButton imgBtnTmp = (ImageButton)fila.Cells[1].FindControl("imgBtnSeleccion");
                imgBtnTmp.Enabled = enable;

                if (EsSeleccioando)
                {
                    imgBtnTmp.ImageUrl = "~/img/95.png";
                    listaIdHotel.Add(int.Parse(imgBtnTmp.CommandArgument));

                }
                else
                    imgBtnTmp.ImageUrl = "~/img/117.png";
            }

            Session["listaIdHotel"] = listaIdHotel;
        }

        #endregion

        #region Boton
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            List<int> listaIdHotel = new List<int>();
            Session["listaIdHotel"] = listaIdHotel;

            this.IdUsuarioSeleccionado = -1; //limpiamos el id por precaucion.

            LimpiarFormulario();
            ddlPerfil_SelectedIndexChanged(null, null);

            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.GrillaUsuario.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoUsuario.Visible = true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBo usuarioBoTmp = new UsuarioBo();

                //if (Utilities.EsIdentificacionExistenteUsuario(txtNumIdentificacion.Text))
                //{
                //    this.divExito.Visible = false;
                //    this.divError.Visible = true;
                //    this.lbltextoError.Text = Resources.Resource.lblMensajeError_10;
                //    return;
                //}

                if (Utilities.EsLoginExistenteUsuario(txtLogin.Text))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_21;
                    return;
                }

                List<int> listaIdHotel = (List<int>)Session["listaIdHotel"];

                if (listaIdHotel.Count == 0)
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Debe al menos seleccionar una Hotel.";
                    return;
                }

                string clave = Utilities.EncodePassword(String.Concat(txtLogin.Text, txtLogin.Text));
                usuarioBoTmp.Guardar(txtNombre.Text, txtApellido.Text, txtNumIdentificacion.Text,
                                     txtCorreo.Text, txtTel1.Text, txtTel2.Text, chActivo.Checked,
                                     int.Parse(ddlPerfil.SelectedValue), listaIdHotel, txtLogin.Text,
                                     clave, ((ObjetoGenerico)Session["usuarioLogin"]).Id, ddlPerfil.SelectedItem.Text);

                this.divExito.Visible = true;
                this.divError.Visible = false;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;

                btnVerTodos_Click(null, null);
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
                UsuarioBo usuarioboTmp = new UsuarioBo();

                if (usuarioboTmp.EsLoginExistenteUsuario(txtLogin.Text, this.IdUsuarioSeleccionado))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_21;
                    return;
                }

                List<int> listaIdHotel = (List<int>)Session["listaIdHotel"];

                if (listaIdHotel.Count == 0)
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Debe al menos seleccionar una Hotel.";
                    return;
                }

                if (int.Parse(ddlPerfil.SelectedValue) == Properties.Settings.Default.IdSuperUsuario)
                    listaIdHotel.Clear();

                usuarioboTmp.Actualizar(this.IdUsuarioSeleccionado, txtNombre.Text, txtApellido.Text, txtNumIdentificacion.Text,
                                        txtCorreo.Text, txtTel1.Text, txtTel2.Text, chActivo.Checked, int.Parse(ddlPerfil.SelectedValue),
                                        listaIdHotel, ((ObjetoGenerico)Session["usuarioLogin"]).Id);

                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
                divExito.Visible = true;
                divError.Visible = false;

                btnVerTodos_Click(null, null);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBo usuarioBoTmp = new UsuarioBo();
                string clave = Utilities.EncodePassword(String.Concat(txtLogin.Text, txtLogin.Text));
                usuarioBoTmp.RestablecerPass(this.IdUsuarioSeleccionado, clave, ((ObjetoGenerico)Session["usuarioLogin"]).Id);

                lbltextoExito.Text = "Contraseña restablecida con exito.";
                divExito.Visible = true;
                divError.Visible = false;

                btnVerTodos_Click(null, null);
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
            CargarGrilla();

            this.btnReset.Visible = false;

            this.GrillaUsuario.Visible = true;

            this.btnNuevo.Visible = true;
            this.btnActualizar.Visible = false;
            this.btnGuardar.Visible = false;
            this.NuevoUsuario.Visible = false;
        }

        #endregion

        #region Eventos

        protected void gvwHotel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void imgBtnSeleccion_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgBtnTmp = (ImageButton)sender;
            List<int> listaIdHotel = (List<int>)Session["listaIdHotel"];

            if (imgBtnTmp.ImageUrl == "~/img/117.png")
            {
                imgBtnTmp.ImageUrl = "~/img/95.png";
                listaIdHotel.Add(int.Parse(imgBtnTmp.CommandArgument));
            }
            else
            {
                imgBtnTmp.ImageUrl = "~/img/117.png";
                listaIdHotel.Remove(int.Parse(imgBtnTmp.CommandArgument));
            }

            Session["listaIdHotel"] = listaIdHotel;
        }

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }

        protected void gvwUsuario_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.btnReset.Visible = true;

            this.IdUsuarioSeleccionado = Convert.ToInt32(gvwUsuario.DataKeys[e.NewSelectedIndex]["IdUsuario"].ToString());
            this.IdPerfilSeleccionado = Convert.ToInt32(gvwUsuario.DataKeys[e.NewSelectedIndex]["IdPerfil"].ToString());

            this.CargarDatosUsuario();
            this.CargarHotelesUsuario();

            this.btnNuevo.Visible = false;
            this.GrillaUsuario.Visible = false;
            this.btnGuardar.Visible = false;
            this.txtLogin.Enabled = false;

            this.btnActualizar.Visible = true;
            this.NuevoUsuario.Visible = true;
        }

        protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.IdSuperUsuario == int.Parse(ddlPerfil.SelectedValue))
                HotelSeleccionado(true, false);
            else
                HotelSeleccionado(false, true);
        }

        protected void chActivo_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in ddlPerfil.Items)
            {
                item.Selected = false;
            }

            // Cuando se inactiva el usuario, su perfil cambia a "Sin perfil"
            CheckBox cb = (CheckBox)sender;
            if (!chActivo.Checked)
            {
                ddlPerfil.Items.FindByValue("-1").Selected = true;
                ddlPerfil.Enabled = false;
            }
            else
                ddlPerfil.Enabled = true;
        }

        #endregion
    }
}