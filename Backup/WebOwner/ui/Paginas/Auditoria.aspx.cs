using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;

namespace WebOwner.ui.Paginas
{
    public partial class Auditoria : System.Web.UI.Page
    {
        AuditoriaBo auditoriaBo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaDesde.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");

                this.CargarModulos();
                ddlModulo_SelectedIndexChanged(null, null);
            }

            if (Page.Request.Params["__EVENTTARGET"] == "ctl00$Contenidoprincipal$ReportViewer_Auditoria")
                btnFiltrar_Click(null, null);
        }

        #region Variables
        #endregion

        #region Propiedades
        #endregion

        #region Metodos
        private void CargarModulos()
        {
            auditoriaBo = new AuditoriaBo();

            foreach (string itemModulo in auditoriaBo.ObtenerModulos())
            {
                ddlModulo.Items.Add(new ListItem(itemModulo, itemModulo));
            }            
            ddlModulo.DataBind();
        }
        private void CargarCampos()
        {
            auditoriaBo = new AuditoriaBo();
        }
        #endregion

        #region Eventos
        protected void ddlModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            auditoriaBo = new AuditoriaBo();

            ddlCampos.Items.Clear();

            ddlCampos.Items.Add(new ListItem("Todos", "T"));

            foreach (string itemCampos in auditoriaBo.ObtenerCampos(ddlModulo.SelectedValue))
            {
                ddlCampos.Items.Add(new ListItem(itemCampos, itemCampos));
            }
            ddlCampos.DataBind();
        }
        #endregion

        #region Boton
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string condicion = "where Auditoria.NombreTabla = '" + ddlModulo.SelectedValue + "' ";

            if (ddlCampos.SelectedValue != "T")
                condicion += " and Auditoria.Campo = '" + ddlCampos.SelectedValue + "' ";

            if (ddlAccion.SelectedValue != "T")
                condicion += " and Auditoria.Accion = '" + ddlAccion.SelectedValue + "' ";

            condicion += " and (Auditoria.Fechahora between '" + txtFechaDesde.Text + " 00:00:00' and '" + txtFechaHasta.Text + " 23:59:59')";

            Reportes.XtraReport_Auditoria reporteAuditoria = new WebOwner.Reportes.XtraReport_Auditoria(ddlModulo.SelectedItem.Text, txtFechaDesde.Text, txtFechaHasta.Text, condicion);
            ReportViewer_Auditoria.Report = reporteAuditoria;
        }
        #endregion
    }
}
