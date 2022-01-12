using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using DM;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserCertificado : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.listaIdConceptos = new List<int>();
                CargarCombos();
            }
        }

        #region Propiedades
        public List<int> listaIdConceptos
        {
            set { Session["listaIdConceptos"] = value; }
            get { return (List<int>)Session["listaIdConceptos"]; }
        }
        #endregion

        #region Metodos

        private void LimpiarFormulario()
        {
        }

        private void CargarGrilla()
        {
            ConceptoBo conceptoBoTmp = new ConceptoBo();
            gvwConceptos.DataSource = conceptoBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue));
            gvwConceptos.DataBind();
        }

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();
            ddlHotel.DataSource = hotelBoTmp.ObtenerHotelPorPerfil(((ObjetoGenerico)Session["usuarioLogin"]).Id, ((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil);
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();

            ddlHotel_SelectedIndexChanged(null, null);
        }
        #endregion

        #region Eventos        

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescripcion.Text = string.Empty;

            ConfiguracionCertificadoBo configBoTmp = new ConfiguracionCertificadoBo();
            List<ObjetoGenerico> certificadoTmp = configBoTmp.listaIdsConcepto(int.Parse(ddlHotel.SelectedValue));

            if (certificadoTmp.Count > 0)
            {
                txtDescripcion.Text = certificadoTmp[0].Descripcion;
                this.listaIdConceptos = certificadoTmp.Select(C => C.IdConcepto.Value).ToList();
            }
            else
                this.listaIdConceptos = new List<int>();

            CargarGrilla();
        }

        protected void gvwConceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.listaIdConceptos.Count > 0)
                {
                    int idConcepto = int.Parse(gvwConceptos.DataKeys[e.Row.RowIndex]["IdConcepto"].ToString());
                    ImageButton imgBtnTmp = (ImageButton)e.Row.FindControl("imgBtnSeleccion");

                    if (this.listaIdConceptos.Contains(idConcepto))
                        imgBtnTmp.ImageUrl = "~/img/95.png";
                    else
                        imgBtnTmp.ImageUrl = "~/img/117.png";
                }
            }
        }

        protected void imgBtnSeleccion_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgBtnTmp = (ImageButton)sender;

            List<int> listaIdConceptos = (List<int>)Session["listaIdConceptos"];
            int idConcepto = int.Parse(imgBtnTmp.CommandArgument);

            if (imgBtnTmp.ImageUrl == "~/img/117.png")
            {
                imgBtnTmp.ImageUrl = "~/img/95.png";
                listaIdConceptos.Add(idConcepto);
            }
            else
            {
                imgBtnTmp.ImageUrl = "~/img/117.png";
                listaIdConceptos.RemoveAll(id => id == idConcepto);
            }

            Session["listaIdConceptos"] = listaIdConceptos;
        }

        #endregion

        #region Botones

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listaIdConceptos.Count == 0)
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Debes seleccionar al menos un concepto.";
                    return;
                }

                ConfiguracionCertificadoBo configBoTmp = new ConfiguracionCertificadoBo();
                configBoTmp.Guardar(int.Parse(ddlHotel.SelectedValue), txtDescripcion.Text, this.listaIdConceptos);

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_8;
            }
        }

        #endregion
    }
}