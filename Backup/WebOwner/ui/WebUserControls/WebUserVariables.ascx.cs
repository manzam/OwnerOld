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
    public partial class WebUserVariables : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.CargarCombos();
                ddlHotel_SelectedIndexChanged(null, null);
            }
        }

        #region Propiedades
        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }
        public int IdVariable { get { return (int)ViewState["IdVariable"]; } set { ViewState["IdVariable"] = value; } }
        #endregion

        #region Metodos

        private void CargarGrilla()
        {
            VariableBo variableBoTmp = new VariableBo();
            gvwVariables.DataSource = variableBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue));
            gvwVariables.DataBind();
        }

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel=new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataSource = listaHotel;
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();
        }

        private void limpiar()
        {
            chActivo.Checked = true;
            txtDescripcion.Text = string.Empty;
            txtNombre.Text = string.Empty;
        }

        private void SetValorSuitePropietario(int idHotel, int idVariable)
        {
            SuitPropietarioBo suitPropietarioBoTmp = new SuitPropietarioBo();
            List<int> listaIdSuitPropietario = suitPropietarioBoTmp.ListaIdsuitPropietario(idHotel);

            List<Valor_Variable_Suit> listaValorVariable = new List<Valor_Variable_Suit>();
            foreach (int itemIdSuitPropietario in listaIdSuitPropietario)
            {
                Valor_Variable_Suit valorVariableSuitTmp = new Valor_Variable_Suit();
                valorVariableSuitTmp.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", itemIdSuitPropietario);
                valorVariableSuitTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", idVariable);
                valorVariableSuitTmp.Valor = -1;
                listaValorVariable.Add(valorVariableSuitTmp);
            }

            ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
            valorVariableBoTmp.Guardar(listaValorVariable);
        }

        #endregion

        #region Eventos

        protected void gvwVariables_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvwVariables.EditIndex = -1;
            CargarGrilla();
        }

        protected void gvwVariables_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.IdVariable = int.Parse(gvwVariables.DataKeys[e.NewEditIndex]["IdVariable"].ToString());
            gvwVariables.EditIndex = e.NewEditIndex;
            CargarGrilla();
        }

        protected void gvwVariables_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    ObjetoGenerico variableTmp = (ObjetoGenerico)e.Row.DataItem;
                    Label lbl = (Label)e.Row.FindControl("lblTipoVariable");

                    lbl.Text = ddlTipoVariable.Items.FindByValue(variableTmp.Tipo).Text;
                }
                catch (Exception)
                {
                }
            }
        }

        protected void gvwVariables_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                VariableBo variableBoTmp = new VariableBo();
                int idVariable = int.Parse(gvwVariables.DataKeys[e.RowIndex]["IdVariable"].ToString());
                int idHotel = int.Parse(gvwVariables.DataKeys[e.RowIndex]["IdHotel"].ToString());
                string tipo = ((DropDownList)(gvwVariables.Rows[e.RowIndex].FindControl("ddlTipoVariable"))).SelectedValue;
                string nombreOld = gvwVariables.DataKeys[e.RowIndex]["Nombre"].ToString();
                string nombreNew = ((TextBox)(gvwVariables.Rows[e.RowIndex].FindControl("txtNombreGrilla"))).Text;
                bool activo = ((CheckBox)(gvwVariables.Rows[e.RowIndex].FindControl("chActivo"))).Checked;
                string descripcion = ((TextBox)(gvwVariables.Rows[e.RowIndex].FindControl("txtDescripcionGrilla"))).Text;

                bool esValidacion = ((CheckBox)(gvwVariables.Rows[e.RowIndex].FindControl("cbConValidacion"))).Checked;
                short valMax = short.Parse(((TextBox)(gvwVariables.Rows[e.RowIndex].FindControl("txtValMaximo"))).Text);

                if (nombreOld != nombreNew)
                {
                    if (variableBoTmp.EsRepetida(nombreNew, idHotel, idVariable))
                    {
                        divError.Visible = true;
                        divExito.Visible = false;

                        lbltextoError.Text = Resources.Resource.lblMensajeError_4;
                        return;
                    }
                }

                double valorConstante = 0;
                try
                {
                    valorConstante = double.Parse(((TextBox)(gvwVariables.Rows[e.RowIndex].FindControl("txtValorConstante"))).Text);
                }
                catch (Exception ex)
                {
                    valorConstante = 0;
                }

                variableBoTmp.Actualizar(idVariable, nombreNew, descripcion, activo, tipo, idHotel, ddlHotel.SelectedItem.Text, ddlTipoVariable.SelectedItem.Text, esValidacion, valMax, this.UsuarioLogin.Id, valorConstante);

                // Si es de tipo Variable propietario, seteo en valor 1 todos los propietarios, para evitar resultados infinitos 
                //if (tipo == "P")
                //    this.SetValorSuitePropietario(idHotel, idVariable);

                gvwVariables.EditIndex = -1;
                this.CargarGrilla();

                divError.Visible = false;
                divExito.Visible = true;

                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;

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
                ImageButton imgButton = (ImageButton)sender;
                this.IdVariable = int.Parse(imgButton.CommandArgument);

                VariableBo variableBoTmp = new VariableBo();

                if (variableBoTmp.EsEliminar(this.IdVariable))
                {
                    variableBoTmp.Eliminar(this.IdVariable, ddlHotel.SelectedItem.Text, this.UsuarioLogin.Id);

                    this.divExito.Visible = true;
                    this.divError.Visible = false;
                    this.lbltextoExito.Text = Resources.Resource.lblMensajeEliminar;

                    this.CargarGrilla();
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Esta variable hace parte de una regla de liquidación.";
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

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }

        protected void chActivo_CheckedChanged(object sender, EventArgs e)
        {            
            if (((CheckBox)sender).Checked == false)
            {
                VariableBo variableBoTmp = new VariableBo();
                if (!variableBoTmp.EsEliminar(this.IdVariable))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Esta variable hace parte de una regla de liquidación.";

                    gvwVariables_RowCancelingEdit(null, null);
                }
            }
        }

        protected void ddlTipoVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoVariable.SelectedValue == "C")
            {
                txtValorConstante.Enabled = true;
                rfv_ValorConstante.Enabled = true;
            }
            else
            {
                txtValorConstante.Enabled = false;
                rfv_ValorConstante.Enabled = false;
            }
        }

        #endregion

        #region Botones

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            this.limpiar();

            btnNuevo.Visible = false;
            GrillaVariables.Visible = false;

            NuevoVariable.Visible = true;
            btnGuardar.Visible = true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                VariableBo variableBoTmp = new VariableBo();
                if (variableBoTmp.EsRepetida(txtNombre.Text, int.Parse(ddlHotel.SelectedValue), -1))
                {
                    divError.Visible = true;
                    divExito.Visible = false;

                    lbltextoError.Text = Resources.Resource.lblMensajeError_4;
                    return;
                }
                else
                {
                    int idHotel = int.Parse(ddlHotel.SelectedValue);
                    double valorConstante = 0;

                    try
                    {
                        valorConstante = double.Parse(txtValorConstante.Text);
                    }
                    catch (Exception ex)
                    {
                        valorConstante = 0;
                    }


                    int idVariable = variableBoTmp.Guardar(txtNombre.Text, txtDescripcion.Text, chActivo.Checked,
                                                           ddlTipoVariable.SelectedValue, idHotel, ddlHotel.SelectedItem.Text,
                                                           ddlTipoVariable.SelectedItem.Text, cbEsConValidacion.Checked, short.Parse(txtNumMaximo.Text), UsuarioLogin.Id, valorConstante);

                    // Si es de tipo Variable propietario, seteo en valor 1 todos los propietarios, para evitar resultados infinitos 
                    if (ddlTipoVariable.SelectedValue == "P")
                        this.SetValorSuitePropietario(idHotel, idVariable);

                    NuevoVariable.Visible = false;
                    btnGuardar.Visible = false;
                    btnNuevo.Visible = true;
                    GrillaVariables.Visible = true;

                    this.divExito.Visible = true;
                    this.divError.Visible = false;
                    this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;

                    CargarGrilla();
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
            gvwVariables.EditIndex = -1;
            CargarGrilla();
            NuevoVariable.Visible = false;
            btnNuevo.Visible = true;
            GrillaVariables.Visible = true;

            btnGuardar.Visible = false;
            divError.Visible = false;
            divExito.Visible = false;
        }

        #endregion
    }
}