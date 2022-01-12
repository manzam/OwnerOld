using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserBuscadorSuite : System.Web.UI.UserControl
    {
        public event EventHandler AlAceptar;
        SuitBo suiteBo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.IdSuiteBuscado = -1;
                this.IdHotel = -1;
            }
        }

        #region Propiedades

        public int IdSuiteBuscado
        {
            get { return (int)ViewState["IdSuiteBuscado"]; }
            set { ViewState["IdSuiteBuscado"] = value; }
        }
        public int IdHotel
        {
            get { return (int)ViewState["IdHotel"]; }
            set { ViewState["IdHotel"] = value; }
        }
        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }

        #endregion

        #region Metodo
        public void Limpiar()
        {
            gvwSuitePorHotel.SelectedIndex = -1;
            txtBusqueda.Text = string.Empty;
            gvwSuitePorHotel.DataSource = null;
            gvwSuitePorHotel.DataBind();
        }
        #endregion

        #region Boton
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            suiteBo = new SuitBo();
            gvwSuitePorHotel.DataSource = suiteBo.ObtenerSuit(txtBusqueda.Text, this.IdHotel);
            gvwSuitePorHotel.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            AlAceptar(null, null);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Eventos
        protected void gvwPropietariosBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.IdSuiteBuscado = (int)gvwSuitePorHotel.DataKeys[gvwSuitePorHotel.SelectedRow.RowIndex]["IdSuit"];
        }

        protected void gvwPropietariosBuscar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        #endregion
    }
}