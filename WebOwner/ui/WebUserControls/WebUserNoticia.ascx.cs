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
    public partial class WebUserNoticia : System.Web.UI.UserControl
    {
        NoticiaBo noticiaBoTmp;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.divExito.Visible = false;
            this.divError.Visible = false;

            if (!IsPostBack)
            {
                this.CargarGrilla();               
            }
        }

        public int IdNoticiaSeleccionada 
        {
            get { return (int)ViewState["IdNoticiaSeleccionada"]; }
            set { ViewState["IdNoticiaSeleccionada"] = value; }
        }

        #region Metodo

        private void LimpiarFormulario()
        {
            txtTitulo.Text = "";
            txtFecha.Text = "";
            cbEstado.Checked = true;
        }

        private void CargarDatosNoticia(int idNoticia)
        {
            NoticiaBo noticiaBoTmp = new NoticiaBo();
            Noticia noticiaTmp = noticiaBoTmp.Obtener(this.IdNoticiaSeleccionada);
            txtTitulo.Text = noticiaTmp.Titulo;
            cbEstado.Checked = noticiaTmp.Activo;
            txtFecha.Text = noticiaTmp.FechaCaducidad.ToString("dd/MM/yyyy");
        }

        #endregion

        #region Eventos

        private void CargarGrilla()
        {
            NoticiaBo noticiaBoTmp = new NoticiaBo();
            gvwNoticias.DataSource = noticiaBoTmp.VerTodos();
            gvwNoticias.DataBind();
        }

        protected void gvwNoticias_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwNoticias.SelectedRow;
            this.IdNoticiaSeleccionada = int.Parse(gvwNoticias.DataKeys[filaSeleccionada.RowIndex].Value.ToString());

            this.CargarDatosNoticia(this.IdNoticiaSeleccionada);

            this.btnNuevo.Visible = false;
            this.GrillaHotel.Visible = false;
            this.btnGuardar.Visible = false;

            this.btnActualizar.Visible = true;
            this.NuevoNoticia.Visible = true; 
        }

        protected void txtNoticia_HtmlEditorExtender_ImageUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            //string rutaImg = "~/img/imgNoticias/" + e.FileName;
            //txtNoticia_HtmlEditorExtender.AjaxFileUpload.SaveAs(Server.MapPath(rutaImg));
            //e.PostedUrl = rutaImg;
        }

        protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
        }

        #endregion

        #region Boton

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IdNoticiaSeleccionada = -1; //limpiamos el id por precaucion.

            LimpiarFormulario();

            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.GrillaHotel.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoNoticia.Visible = true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (AsyncFileUpload1.HasFile)
                {
                    if (AsyncFileUpload1.ContentType.ToLower() == "image/jpeg" ||
                       AsyncFileUpload1.ContentType.ToLower() == "image/gif" ||
                       AsyncFileUpload1.ContentType.ToLower() == "image/png")
                    {
                        noticiaBoTmp = new NoticiaBo();
                        DateTime fecha = DateTime.Parse(txtFecha.Text);
                        string filename = "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
                        AsyncFileUpload1.SaveAs(Server.MapPath("../../img/imgNoticias/") + filename);

                        noticiaBoTmp.Guardar(txtTitulo.Text, "~/img/imgNoticias/" + filename, cbEstado.Checked, ((ObjetoGenerico)Session["usuarioLogin"]).Id, fecha);

                        LimpiarFormulario();
                        CargarGrilla();

                        this.btnNuevo.Visible = true;
                        this.GrillaHotel.Visible = true;

                        this.btnActualizar.Visible = false;
                        this.btnGuardar.Visible = false;
                        this.NuevoNoticia.Visible = false;

                        this.divExito.Visible = true;
                        this.divError.Visible = false;
                        this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                    }
                    else
                    {
                        this.divExito.Visible = false;
                        this.divError.Visible = true;
                        this.lbltextoError.Text = "El archivo no es una imagen.";
                    }
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "No se slecciono ninguna imagen a cargar.";
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

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (AsyncFileUpload1.HasFile)
                {
                    if (AsyncFileUpload1.ContentType.ToLower() == "image/jpeg" ||
                       AsyncFileUpload1.ContentType.ToLower() == "image/gif" ||
                       AsyncFileUpload1.ContentType.ToLower() == "image/png")
                    {
                        noticiaBoTmp = new NoticiaBo();
                        DateTime fecha = DateTime.Parse(txtFecha.Text);
                        string filename = "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
                        AsyncFileUpload1.SaveAs(Server.MapPath("../../img/imgNoticias/") + filename);

                        noticiaBoTmp.Actualizar(this.IdNoticiaSeleccionada, txtTitulo.Text, "~/img/imgNoticias/" + filename, cbEstado.Checked, ((ObjetoGenerico)Session["usuarioLogin"]).Login, fecha);

                        LimpiarFormulario();
                        CargarGrilla();

                        this.btnNuevo.Visible = true;
                        this.GrillaHotel.Visible = true;

                        this.btnActualizar.Visible = false;
                        this.btnGuardar.Visible = false;
                        this.NuevoNoticia.Visible = false;

                        this.divExito.Visible = true;
                        this.divError.Visible = false;
                        this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                    }
                    else
                    {
                        this.divExito.Visible = false;
                        this.divError.Visible = true;
                        this.lbltextoError.Text = "El archivo no es una imagen.";
                    }
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "No se slecciono ninguna imagen a cargar.";
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
            LimpiarFormulario();
            CargarGrilla();

            this.btnNuevo.Visible = true;
            this.GrillaHotel.Visible = true;

            this.btnActualizar.Visible = false;
            this.btnGuardar.Visible = false;
            this.NuevoNoticia.Visible = false;
        }
        #endregion
    }
}