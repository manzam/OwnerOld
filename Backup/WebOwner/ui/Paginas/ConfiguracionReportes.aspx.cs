using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using System.Drawing;
using DM;
using System.Web.UI.HtmlControls;

namespace WebOwner.ui.Paginas
{
    public partial class ConfiguracionReportes : System.Web.UI.Page
    {
        ParametroBo parametroBoTmp = null;
        HotelBo hotelBoTmp = null;
        ConfiguracionReporteBo configuracionReporteBo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            gvwVariables.SelectedIndex = -1;

            if (!IsPostBack)
            {
                parametroBoTmp = new ParametroBo();
                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                
                gvwVariables.PageSize = this.PageSize;
                gvwDetalleReporte.PageSize = this.PageSize;

                hotelBoTmp = new HotelBo();
                List<DM.Hotel> listaHotel = new List<DM.Hotel>();

                if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                    listaHotel = hotelBoTmp.VerTodos();
                else
                    listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

                ddlHotel.DataSource = listaHotel;
                ddlHotel.DataValueField = "IdHotel";
                ddlHotel.DataTextField = "Nombre";
                ddlHotel.DataBind();

                ddlHotel.Items.Insert(0, new ListItem("Seleccione...", "-1"));

                configuracionReporteBo = new ConfiguracionReporteBo();
                ddlReportes.DataSource = configuracionReporteBo.ListaReporte();
                ddlReportes.DataValueField = "IdReporte";
                ddlReportes.DataTextField = "NombreReporte";
                ddlReportes.DataBind();

                ddlReportes.Items.Insert(0, new ListItem("Seleccione...", "-1"));
            }
        }

        #region Propiedades
        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }
        public List<ObjetoGenerico> ListaDetalle
        {
            get { return (List<ObjetoGenerico>)ViewState["ListaDetalle"]; }
            set { ViewState["ListaDetalle"] = value; }
        }
        #endregion

        #region Metodos
        public void CargarVariables()
        {
            gvwDetalleReporte.DataSource = this.ListaDetalle.OrderBy(O => O.Orden);
            gvwDetalleReporte.DataBind();
            this.PintarSeleccionados();
        }
        private void PintarSeleccionados()
        {
            if (this.ListaDetalle == null || this.ListaDetalle.Count == 0 || gvwVariables.Rows.Count == 0)
                return;

            foreach (GridViewRow itemFila in gvwVariables.Rows)
            {
                int idVariable = (int)gvwVariables.DataKeys[itemFila.RowIndex]["IdVariable"];

                if (this.ListaDetalle.Where(D => D.IdVariable == idVariable).Count() > 0)
                {
                    itemFila.BackColor = Color.FromName("#FFFAAE");
                    itemFila.ForeColor = Color.FromName("#585858");
                }
                else
                {
                    itemFila.BackColor = Color.White;
                    itemFila.ForeColor = Color.Black;
                }
            }
        }
        private void CargarGrupos()
        {
            ddlGrupo.Items.Clear();
            configuracionReporteBo = new ConfiguracionReporteBo();
            List<ObjetoGenerico> lista = configuracionReporteBo.ListaGrupo();

            foreach (ObjetoGenerico item in lista)
            {
                ddlGrupo.Items.Add(new ListItem((item.Orden.ToString() + " - " + item.Nombre), item.Id.ToString()));
            }
            ddlGrupo.Items.Insert(0, new ListItem("Seleccione...", "-1"));            
        }
        private void CargarConfiguracion()
        {
            gvwConfiguracion.DataSource = configuracionReporteBo.ListaItemGrupo(int.Parse(ddlHotel.SelectedValue));
            gvwConfiguracion.DataBind();
        }
        private void CargarGrilla()
        { }
        #endregion

        #region Eventos
        protected void gvwVariables_DataBound(object sender, EventArgs e)
        {
            this.PintarSeleccionados();
        }
        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            odsVariables.Select();
            ddlReportes_SelectedIndexChanged(null, null);
        }
        protected void ddlReportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            configuracionReporteBo = new ConfiguracionReporteBo();
            pnlConsolidadoSuite.Visible = false;
            pnlConsolidadoPropietario.Visible = false;

            switch (ddlReportes.SelectedValue)
            {
                case "1": // Colsolidado Suite
                    pnlConsolidadoSuite.Visible = true;

                    this.ListaDetalle = configuracionReporteBo.ListaDetalleReporte(int.Parse(ddlHotel.SelectedValue));
                    gvwDetalleReporte.DataSource = this.ListaDetalle;
                    gvwDetalleReporte.DataBind();
                    break;

                case "2": // Colsolidado propietario
                    pnlConsolidadoPropietario.Visible = true;
                    this.CargarGrupos();
                    this.CargarConfiguracion();

                    break;

                default:
                    break;
            }           
            
        }
        protected void gvwVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                configuracionReporteBo = new ConfiguracionReporteBo();
                int idVariable = (int)gvwVariables.DataKeys[gvwVariables.SelectedRow.RowIndex]["IdVariable"];
                string tipo = gvwVariables.DataKeys[gvwVariables.SelectedRow.RowIndex]["Tipo"].ToString();

                switch (ddlReportes.SelectedValue)
                {
                    case "1": // Consolidado por Suite
                        this.ListaDetalle.Add(new ObjetoGenerico() { Orden = 0, Tipo = tipo, IdVariable = idVariable });
                        configuracionReporteBo.Guardar(this.ListaDetalle, int.Parse(ddlReportes.SelectedValue), int.Parse(ddlHotel.SelectedValue));
                        ddlReportes_SelectedIndexChanged(null, null);
                        PintarSeleccionados();
                        break;
                    case "2": // Consolidado por Propietario
                        if (ddlGrupo.SelectedValue != "-1")
                        {
                            configuracionReporteBo.GuardarItemGrupo(idVariable, tipo, int.Parse(ddlGrupo.SelectedValue), int.Parse(ddlHotel.SelectedValue));
                            this.CargarConfiguracion();
                        }
                        break;
                    default:
                        break;
                }

                

                lbltextoExito.Text = "Configuración guardada correctamente.";
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
        #endregion

        #region Boton
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            configuracionReporteBo = new ConfiguracionReporteBo();

            try
            {
                switch (ddlReportes.SelectedValue)
                {
                    case "1":
                        foreach (GridViewRow itemFila in gvwDetalleReporte.Rows)
                        {
                            int idReporteDetalle = (int)gvwDetalleReporte.DataKeys[itemFila.RowIndex]["IdReporteDetalle"];

                            ObjetoGenerico detalleTmp = this.ListaDetalle.Where(D => D.IdReporteDetalle == idReporteDetalle).FirstOrDefault();
                            detalleTmp.Orden = int.Parse(((TextBox)gvwDetalleReporte.Rows[itemFila.RowIndex].Cells[1].FindControl("txtOrden")).Text);
                        }                        
                        configuracionReporteBo.Guardar(this.ListaDetalle, int.Parse(ddlReportes.SelectedValue), int.Parse(ddlHotel.SelectedValue));

                        break;

                    case "2":
                        //configuracionReporteBo.GuardarItemGrupo(this.ListaIngresos, int.Parse(ddlHotel.SelectedValue));
                        ddlReportes_SelectedIndexChanged(null, null);
                        break;

                    default:
                        break;
                }

                
                ddlHotel_SelectedIndexChanged(null, null);

                lbltextoExito.Text = "Configuración guardada correctamente.";
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
        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                configuracionReporteBo = new ConfiguracionReporteBo();
                int idReporteDetalle = int.Parse((((ImageButton)sender).CommandArgument));
                configuracionReporteBo.EliminarVariableReporte(idReporteDetalle);

                this.ListaDetalle.RemoveAll(D => D.IdReporteDetalle == idReporteDetalle);

                this.CargarVariables();

                lbltextoExito.Text = "Registro eliminado correctamente.";
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
        protected void btnNuevoEditarGrupo_Click(object sender, EventArgs e)
        {

        }        
        protected void btnNuevoGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                configuracionReporteBo = new ConfiguracionReporteBo();
                configuracionReporteBo.GuardarGrupo(txtGrupo.Text, int.Parse(txtOrden.Text));
                this.CargarGrupos();

                txtGrupo.Text = string.Empty;
                txtOrden.Text = string.Empty;

                lbltextoExito.Text = "Nuevo grupo guardado.";
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
        protected void btnEditarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                configuracionReporteBo = new ConfiguracionReporteBo();
                configuracionReporteBo.EditarGrupo(txtGrupo.Text, int.Parse(txtOrden.Text), int.Parse(ddlGrupo.SelectedValue));
                this.CargarGrupos();

                txtGrupo.Text = string.Empty;
                txtOrden.Text = string.Empty;

                lbltextoExito.Text = "Grupo actualizado.";
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
        protected void btnEliminarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                configuracionReporteBo = new ConfiguracionReporteBo();
                configuracionReporteBo.EliminarGrupo(int.Parse(ddlGrupo.SelectedValue));

                this.CargarConfiguracion();

                txtGrupo.Text = string.Empty;
                txtOrden.Text = string.Empty;

                lbltextoExito.Text = "Grupo eliminado.";
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
        protected void imgBtnEliminar_Click1(object sender, ImageClickEventArgs e)
        {            
            try
            {
                ImageButton btn = (ImageButton)sender;
                configuracionReporteBo = new ConfiguracionReporteBo();
                configuracionReporteBo.EliminarItemGrupo(int.Parse(btn.CommandArgument));
                this.CargarConfiguracion();
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
