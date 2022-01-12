using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM;
using BO;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserRelacionCuentaHotel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CargarCombos();
                this.CargarGrilla();
                this.CargarConceptos();                
            }
        }

        #region Propiedades

        public int IdCentroCostoHotel 
        {
            get { return (int)ViewState["IdCentroCostoHotel"]; }
            set { ViewState["IdCentroCostoHotel"] = value; }
        }

        public int IdCuentaContable
        {
            get { return (int)ViewState["IdCuentaContable"]; }
            set { ViewState["IdCuentaContable"] = value; }
        }

        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }

        #endregion

        #region Metodos

        private void CargarConceptos()
        {
            ConceptoBo conceptoBoTmp = new ConceptoBo();
            ddlConceptos.DataSource = conceptoBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue));
            ddlConceptos.DataTextField = "Nombre";
            ddlConceptos.DataValueField = "IdConcepto";
            ddlConceptos.DataBind();
            ddlConceptos.Items.Insert(0, new ListItem("Seleccione...", "-1"));
        }

        private void CargarCombos()
        {
            CentroCostoBo centroCostoBoTmp = new CentroCostoBo();
            ddlCentroCosto.DataSource = centroCostoBoTmp.VerTodos();
            ddlCentroCosto.DataTextField = "Nombre";
            ddlCentroCosto.DataValueField = "IdCentroCosto";
            ddlCentroCosto.DataBind();

            ddlCentroCosto.Items.Insert(0, new ListItem() { Text = "Ninguno", Value = "-1", Selected = true });

            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataSource = listaHotel;
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();
        }

        private void CargarGrilla()
        {
            int idHotel = int.Parse(ddlHotel.SelectedValue);

            CuentaContableBo cuentaContableBoTmp = new CuentaContableBo();
            ddlCuentaContable.DataSource = cuentaContableBoTmp.VerTodos(idHotel);
            ddlCuentaContable.DataTextField = "Nombre";
            ddlCuentaContable.DataValueField = "IdCuentaContable";
            ddlCuentaContable.DataBind();

            if (ddlCuentaContable.Items.Count > 0)
            {
                this.divInfo.Visible = false;
                btnNuevo.Enabled = true;
            }
            else
            {
                btnNuevo.Enabled = false;
                this.divInfo.Visible = true;
                this.lbltextoInfo.Text = Resources.Resource.lblMensajeCuentasContables;
            }

            CentroCostoHotelBo centroCostoHotelBoTmp = new CentroCostoHotelBo();
            gvwHotelesCuenta.DataSource = centroCostoHotelBoTmp.VerTodos(idHotel);
            gvwHotelesCuenta.DataBind();             
        }

        private void Limpiar()
        {
            txtCodigoTercero.Text = "";
            ddlCentroCosto.SelectedIndex = -1;
            ddlCuentaContable.SelectedIndex = -1;
            ddlHotel.SelectedIndex = -1;
        }

        #endregion

        #region Boton

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                CentroCostoHotelBo centroCostoHotelBoTmp = new CentroCostoHotelBo();

                CentroCosto_Hotel centroCostoHotelTmp = new CentroCosto_Hotel();
                centroCostoHotelTmp.CodigoTercero = txtCodigoTercero.Text;
                centroCostoHotelTmp.EsTerceroVariable = cbEsTerceroVariable.Checked;
                
                if (cbEsConBase.Checked)
                {
                    centroCostoHotelTmp.EsConBase = true;
                    centroCostoHotelTmp.ConceptoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", int.Parse(ddlConceptos.SelectedValue));
                }

                centroCostoHotelTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", int.Parse(ddlHotel.SelectedValue));
                centroCostoHotelTmp.Cuenta_ContableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Cuenta_Contable", "IdCuentaContable", int.Parse(ddlCuentaContable.SelectedValue));

                if (ddlCentroCosto.SelectedValue != "-1")
                    centroCostoHotelTmp.Centro_CostoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Centro_Costo", "IdCentroCosto", int.Parse(ddlCentroCosto.SelectedValue));

                centroCostoHotelBoTmp.Guardar(centroCostoHotelTmp, ddlHotel.SelectedItem.Text, ddlCuentaContable.SelectedItem.Text, ddlCentroCosto.SelectedItem.Text, txtCodigoTercero.Text, cbEsConBase.Checked, ((cbEsConBase.Checked) ? ddlConceptos.SelectedItem.Text : string.Empty), this.UsuarioLogin.Id, int.Parse(ddlCuentaContable.SelectedValue));

                btnVerTodos_Click(null, null);

                this.divExito.Visible = true;
                this.divError.Visible = false;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
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
            btnGuardar.Visible = false;
            btnNuevo.Visible = true;
            btnActualizar.Visible = false;
            pnlGrilla.Visible = true;
            pnlNuevoEditar.Visible = false;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IdCentroCostoHotel = -1; //limpiamos el id por precaucion.

            lblCuentaContable.Visible = false;
            ddlCuentaContable.Visible = true;
            cbEsConBase.Checked = false;
            ddlConceptos.Enabled = false;
            ddlConceptos.SelectedIndex = -1;

            this.Limpiar();

            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.pnlGrilla.Visible = false;

            this.btnGuardar.Visible = true;
            this.pnlNuevoEditar.Visible = true;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CentroCostoHotelBo centroCostoHotelBoTmp = new CentroCostoHotelBo();
                centroCostoHotelBoTmp.Actualizar(this.IdCentroCostoHotel, int.Parse(ddlHotel.SelectedValue),
                                                 int.Parse(ddlCentroCosto.SelectedValue), txtCodigoTercero.Text, ddlHotel.SelectedItem.Text,
                                                 ddlCuentaContable.SelectedItem.Text, ddlCentroCosto.SelectedItem.Text, 
                                                 cbEsConBase.Checked, ((cbEsConBase.Checked) ? ddlConceptos.SelectedItem.Text : string.Empty),
                                                 int.Parse(ddlConceptos.SelectedValue), cbEsTerceroVariable.Checked, this.UsuarioLogin.Id, this.IdCuentaContable);

                btnVerTodos_Click(null, null);

                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
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

        #region Eventos

        protected void gvwHotelesCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.IdCentroCostoHotel = (int)gvwHotelesCuenta.DataKeys[gvwHotelesCuenta.SelectedRow.RowIndex]["IdCentroCosto_Hotel"];
            string IdCentroCosto = gvwHotelesCuenta.DataKeys[gvwHotelesCuenta.SelectedRow.RowIndex]["IdCentroCosto"].ToString();
            txtCodigoTercero.Text = ((Label)gvwHotelesCuenta.Rows[gvwHotelesCuenta.SelectedRow.RowIndex].FindControl("lblCodigoTercero")).Text;
            string idConcepto = gvwHotelesCuenta.DataKeys[gvwHotelesCuenta.SelectedRow.RowIndex]["IdConcepto"].ToString();

            this.IdCuentaContable = (int)gvwHotelesCuenta.DataKeys[gvwHotelesCuenta.SelectedRow.RowIndex]["IdCuentaContable"];

            if (idConcepto != string.Empty)
                ddlConceptos.SelectedValue = idConcepto;
            else
                ddlConceptos.SelectedIndex = -1;

            cbEsTerceroVariable.Checked = bool.Parse(gvwHotelesCuenta.DataKeys[gvwHotelesCuenta.SelectedRow.RowIndex]["EsTerceroVariable"].ToString());

            cbEsConBase.Checked = bool.Parse(gvwHotelesCuenta.DataKeys[gvwHotelesCuenta.SelectedRow.RowIndex]["EsConBase"].ToString());
            cbEsConBase_CheckedChanged(null, null);

            ddlCuentaContable.Visible = false;

            lblCuentaContable.Text = ((LinkButton)gvwHotelesCuenta.Rows[gvwHotelesCuenta.SelectedRow.RowIndex].FindControl("lblCuentaContable")).Text;
            lblCuentaContable.Visible = true;

            if (IdCentroCosto != string.Empty)
                ddlCentroCosto.SelectedValue = IdCentroCosto;
            else
                ddlCentroCosto.SelectedIndex = -1;            

            btnGuardar.Visible = false;
            btnActualizar.Visible = true;
            pnlGrilla.Visible = false;
            pnlNuevoEditar.Visible = true;
        }

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarGrilla();
            this.CargarConceptos();
        }

        protected void cbEsConBase_CheckedChanged(object sender, EventArgs e)
        {
            ddlConceptos.Enabled = cbEsConBase.Checked;
            rfv_Conceptos.Enabled = cbEsConBase.Checked;
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;

                CentroCostoHotelBo centroCostoHotelBoTmp = new CentroCostoHotelBo();
                centroCostoHotelBoTmp.Eliminar(int.Parse(btn.CommandArgument), int.Parse(ddlHotel.SelectedValue));

                CargarGrilla();

                this.divExito.Visible = true;
                this.lbltextoExito.Text = "El Centro Costo - Hotel, fue eliminada con exito.";
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

        #endregion
    }
}