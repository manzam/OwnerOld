using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using System.IO;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserValorVariable : System.Web.UI.UserControl
    {
        VariableBo variableBoTmp;
        HotelBo hotelBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                for (int i = (DateTime.Now.Year + 1); i >= (DateTime.Now.Year - 11); i--)
                {
                    ListItem item = new ListItem(i.ToString(), i.ToString());
                    ddlAno.Items.Add(item);
                }

                ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                ddlMes.SelectedValue = DateTime.Now.Month.ToString();

                this.CargarCombo();
                this.CargarGrilla();

                this.IdVariableSeleccionado = -1;
            }
        }

        #region Propiedades

        public int IdVariableSeleccionado
        {
            get { return (int)ViewState["IdVariableSeleccionado"]; }
            set { ViewState["IdVariableSeleccionado"] = value; }
        }
        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)((ObjetoGenerico)Session["usuarioLogin"]); }
        }

        #endregion

        #region Metodos

        private void CargarGrilla()
        {
            DateTime fecha = new DateTime(int.Parse(ddlAno.Text), int.Parse(ddlMes.SelectedValue), 1);

            variableBoTmp = new VariableBo();
            gvwVariable.DataSource = variableBoTmp.VerTodosConValor(int.Parse(ddlHotel.SelectedValue), true, "H", fecha);
            gvwVariable.DataBind();
        }

        private void CargarCombo()
        {
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
        #endregion

        #region Boton        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecha = new DateTime(int.Parse(ddlAno.Text), int.Parse(ddlMes.SelectedValue), 1);
                List<ObjetoGenerico> listaValorVariable = new List<ObjetoGenerico>();

                foreach (GridViewRow item in gvwVariable.Rows)
                {
                    ObjetoGenerico oVariableValor = new ObjetoGenerico();
                    oVariableValor.IdVariable = (int)gvwVariable.DataKeys[item.RowIndex]["IdVariable"];
                    oVariableValor.Nombre = ((Label)gvwVariable.Rows[item.RowIndex].FindControl("lblNombreVariable")).Text;
                    oVariableValor.Valor = double.Parse(((TextBox)gvwVariable.Rows[item.RowIndex].FindControl("txtValor")).Text);

                    listaValorVariable.Add(oVariableValor);
                }

                variableBoTmp = new VariableBo();
                variableBoTmp.ActualizarValorVariableLista(listaValorVariable, fecha, ddlHotel.SelectedItem.Text, this.UsuarioLogin.Id);

                this.CargarGrilla();

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
        #endregion

        #region Eventos

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }

        #endregion

    }
}