using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM;
using BO;
using Servicios;
using AjaxControlToolkit;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserPropietario : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.IdPropietarioSeleccionado = -1;

                HotelBo hotelBoTmp = new HotelBo();
                List<DM.Hotel> listaHotel = new List<DM.Hotel>();

                if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                {
                    listaHotel = hotelBoTmp.VerTodos();
                    this.EsAdmon = true;
                }
                else
                    listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);
                
                ddlHotelFiltro.DataSource = listaHotel;
                ddlHotelFiltro.DataTextField = "Nombre";
                ddlHotelFiltro.DataValueField = "IdHotel";
                ddlHotelFiltro.DataBind();

                CargarGrilla();

                ParametroBo parametroBoTmp = new ParametroBo();
                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                gvwPropietario.PageSize = this.PageSize;               
            }
        }

        #region Propiedades

        public int IdPropietarioSeleccionado
        {
            get { return (int)ViewState["IdPropietarioSeleccionado"]; }
            set { ViewState["IdPropietarioSeleccionado"] = value; }
        }

        public int IdSuitPropietarioSeleccionado
        {
            get { return (int)ViewState["IdSuitPropietarioSeleccionado"]; }
            set { ViewState["IdSuitPropietarioSeleccionado"] = value; }
        }

        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }

        public ObjetoGenerico PropietarioTmp
        {
            get { return (ObjetoGenerico)ViewState["Propietario"]; }
            set { ViewState["Propietario"] = value; }
        }

        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }

        public bool EsAdmon
        {
            get
            {
                try
                {
                    return (bool)ViewState["EsAdmon"];
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set { ViewState["EsAdmon"] = value; }
        }

        #endregion

        #region Metodos

        private void CargarReporte()
        {
            //if (ddlHotel.Items.Count > 0)
            //{
            //    Reportes.XtraReport_Propietarios reporte = new WebOwner.Reportes.XtraReport_Propietarios(int.Parse(ddlHotel.SelectedValue), ddlHotel.SelectedItem.Text);
            //    ReportViewer_ReportePropietarios.Report = reporte;
            //}
        }

        private void CargarGrilla()
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            int idHotel = int.Parse(ddlHotelFiltro.SelectedValue);

            gvwPropietario.DataSource = propietarioBoTmp.VerTodosPorHotelPropietarioGrilla(idHotel);
            gvwPropietario.DataBind();
        }


        #endregion

        #region Eventos

        protected void ddlHotelFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   this.CargarGrilla();
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }
        protected void gvwPropietario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwPropietario.PageIndex = e.NewPageIndex;
            this.CargarGrilla();
        }

        //void uc_WebUserBuscadorPropietario_AlAceptar(object sender, EventArgs e)
        //{
        //    if (uc_WebUserBuscadorPropietario.IdPropietarioBuscado == -1)
        //        return;

        //    this.IdPropietarioSeleccionado = uc_WebUserBuscadorPropietario.IdPropietarioBuscado;

        //    this.CargarDatosPropietario(this.IdPropietarioSeleccionado);
        //    this.CargarGrillaSuits(this.IdPropietarioSeleccionado);

        //    this.btnNuevo.Visible = false;
        //    this.GrillaPropietario.Visible = false;
        //    //this.btnGuardar.Visible = false;

        //    //this.btnActualizar.Visible = true;
        //    this.NuevoHotel.Visible = true;
        //}

        #endregion

        #region Botones

        //protected void btnVerTodos_Click(object sender, EventArgs e)
        //{
        //    gvwSuits.SelectedIndex = -1;

        //    LimpiarFormulario();
        //    CargarGrilla();

        //    this.GrillaPropietario.Visible = true;

        //    this.btnNuevo.Visible = true;
        //    //this.btnActualizar.Visible = false;
        //    //this.btnGuardar.Visible = false;
        //    this.btnEliminar.Visible = false;
        //    this.NuevoHotel.Visible = false;
        //    //this.suitDetalle.Visible = false;
        //}


        //protected void btnNuevo_Click(object sender, EventArgs e)
        //{
        //    //ReportViewer_ReportePropietarios.Report = null;

        //    Propietario propetarioTmp = new Propietario();
        //    Session["NuevoPropietario"] = propetarioTmp;
        //    Session["ListaSuit"] = new List<ObjetoGenerico>();
        //    Session["ListaVariablesSuit"] = new List<ObjetoGenerico>();

        //    txtIdPropietario.Text = "";
        //    txtErrorVar.Text = "";

        //    this.IdPropietarioSeleccionado = -1; //limpiamos el id por precaucion.
        //    hiddenIdSuitPropietarioSeleccionado.Value = "-1";
        //    hiddenIdUsuario.Value = this.UsuarioLogin.Id.ToString();
        //    HiddenIdPropietario.Value = "-1";

        //    LimpiarFormulario();

        //    this.btnNuevo.Visible = false;
        //    //this.btnActualizar.Visible = false;
        //    this.GrillaPropietario.Visible = false;

        //    //this.btnGuardar.Visible = true;
        //    this.NuevoHotel.Visible = true;
        //}

        //protected void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    //this.suitDetalle.Visible = false;

        //    uc_WebUserBuscadorPropietario.PageSize = this.PageSize;
        //    uc_WebUserBuscadorPropietario.CargarGrilla();
        //}

        protected void btnEstado_Click(object sender, EventArgs e)
        {
            try
            {
                //if (HiddenIdPropietario.Value != "-1")
                //    return;

                SuitPropietarioBo suitePropietarioBo = new SuitPropietarioBo();
                Button btnActivo = (Button)sender;

                if (btnActivo.Text == "Inactivar")
                {
                    suitePropietarioBo.SetEstadoSuitPropietario(int.Parse(btnActivo.CommandArgument), false);
                    this.lbltextoExito.Text = "Suite inactivada con exito.";
                }
                else
                {
                    suitePropietarioBo.SetEstadoSuitPropietario(int.Parse(btnActivo.CommandArgument), true);
                    this.lbltextoExito.Text = "Suite activada con exito.";
                }

                //this.CargarGrillaSuits(this.IdPropietarioSeleccionado);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                PropietarioBo propietarioBo = new PropietarioBo();

                if (propietarioBo.EliminarPropietario(this.IdPropietarioSeleccionado))
                {
                    this.lbltextoExito.Text = "Propietario eliminado con exito.";
                }
                else
                {
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_9;
                }

                //btnVerTodos_Click(null, null);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        #endregion

        //protected void btnEdit_Click(object sender, EventArgs e)
        //{
        //    GridViewRow filaSeleccionada = gvwPropietario.SelectedRow;
        //    this.IdPropietarioSeleccionado = int.Parse(txtIdPropietario.Text);

        //    this.CargarDatosPropietario(this.IdPropietarioSeleccionado);
        //    this.CargarGrillaSuits(this.IdPropietarioSeleccionado);

        //    this.btnNuevo.Visible = false;
        //    this.GrillaPropietario.Visible = false;
        //    //this.btnGuardar.Visible = false;

        //    //this.btnActualizar.Visible = true;
        //    this.NuevoHotel.Visible = true;
        //    this.btnEliminar.Visible = true;
        //}

        protected void gvwPropietario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Click to highlight row
                Control lnkSelect = e.Row.FindControl("linkSelect");
                if (lnkSelect != null)
                {
                    string idPropietario = gvwPropietario.DataKeys[e.Row.RowIndex]["IdPropietario"].ToString();

                    LinkButton linkButton = (LinkButton)lnkSelect;
                    linkButton.OnClientClick = string.Format("loadOwner('{0}');", idPropietario);
                }
            }
        }
    }
}

