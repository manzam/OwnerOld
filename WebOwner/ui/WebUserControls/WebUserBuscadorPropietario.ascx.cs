using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserBuscadorPropietario : System.Web.UI.UserControl
    {
        public event EventHandler AlAceptar;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.IdPropietarioBuscado = -1;
            }
        }

        #region Propiedades

        public int IdPropietarioBuscado
        {
            get { return (int)ViewState["IdPropietarioBuscado"]; }
            set { ViewState["IdPropietarioBuscado"] = value; }
        }
        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }

        #endregion

        #region Metodo
        public void CargarGrilla()
        {
            //gvwPropietariosBuscar.SelectedIndex = -1;
            //gvwPropietariosBuscar.PageSize = this.PageSize;

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            gvwPropietariosBuscar.DataSource = propietarioBoTmp.ObtenerPropietarios(ddlFiltro.SelectedValue, txtBusqueda.Text);
            gvwPropietariosBuscar.DataBind();
        }
        #endregion

        #region Evento
        protected void gvwPropietariosBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.IdPropietarioBuscado = (int)gvwPropietariosBuscar.DataKeys[gvwPropietariosBuscar.SelectedRow.RowIndex]["IdPropietario"];
        }

        protected void gvwPropietariosBuscar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwPropietariosBuscar.SelectedIndex = -1;
            gvwPropietariosBuscar.PageIndex = e.NewPageIndex;
            this.CargarGrilla();
        }
        #endregion

        #region Boton

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            gvwPropietariosBuscar.DataSource = propietarioBoTmp.ObtenerPropietariosByFiltro(ddlFiltro.SelectedValue, txtBusqueda.Text);
            gvwPropietariosBuscar.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            AlAceptar(null, null);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}