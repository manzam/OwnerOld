using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DM;
using Servicios;
using System.IO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserAsuntoCorreo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.divExito.Visible = false;
            this.divError.Visible = false;

            if (!IsPostBack)
            {
                this.txtFecha.Text = DateTime.Now.Year.ToString();
                CargarCombos();
                ddlHotel_SelectedIndexChanged(null, null);
            }
        }

        #region Metodos
        private void CargarCombos()
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

        private void Limpiar()
        {
            txtAsunto.Text = string.Empty;
            gvwAdjuntos.DataSource = null;
            gvwAdjuntos.DataBind();
        }
        #endregion

        #region Eventos
        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Limpiar();

            try
            {
                AsuntoAdjuntoBo asuntoAdjuntoBoTmp = new AsuntoAdjuntoBo();
                Asunto_Correo asuntoCorreoTmp = asuntoAdjuntoBoTmp.Obtener(int.Parse(ddlHotel.SelectedValue));

                if (asuntoCorreoTmp != null)
                {
                    txtAsunto.Text = asuntoCorreoTmp.Asunto;

                    ddlMes_OnSelectedIndexChanged(null, null);
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

        protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
//            string tipoArchivo = (AsyncFileUpload1.FileName.Split(new char[] { '.' })[1]).ToLower();
            string tipoArchivo = AsyncFileUpload1.FileName.Substring(AsyncFileUpload1.FileName.LastIndexOf(".") + 1);

            if (tipoArchivo == "png" || tipoArchivo == "jpeg" || tipoArchivo == "pdf" || tipoArchivo == "xlsx" || tipoArchivo == "xls" ||
                tipoArchivo == "xltx" || tipoArchivo == "xlt" || tipoArchivo == "docx" || tipoArchivo == "doc")
            {
                string filename = System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
                AsyncFileUpload1.SaveAs(Server.MapPath("../../img/adjuntos/") + filename);
                imgLogo.ImageUrl = Server.MapPath("~/img/adjuntos/") + filename;
            }
            else
                lblMesg.Text = "Este tipo de archivo no es valido.";
        }

        protected void AsyncFileUpload2_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            //string tipoArchivo = (AsyncFileUpload2.FileName.Split(new char[] { '.' })[1]).ToLower();
            string tipoArchivo = AsyncFileUpload2.FileName.Substring(AsyncFileUpload2.FileName.LastIndexOf(".") + 1);

            if (tipoArchivo == "png" || tipoArchivo == "jpeg" || tipoArchivo == "pdf" || tipoArchivo == "xlsx" || tipoArchivo == "xls" ||
                tipoArchivo == "xltx" || tipoArchivo == "xlt" || tipoArchivo == "docx" || tipoArchivo == "doc")
            {
                string filename = System.IO.Path.GetFileName(AsyncFileUpload2.FileName);
                AsyncFileUpload2.SaveAs(Server.MapPath("../../img/adjuntos/") + DateTime.Now.ToString("yyyy_dd_MM_HH_mm_ss")  + filename);
            }
            else
                lblMesg.Text = "Este tipo de archivo no es valido.";
        }

        protected void ddlMes_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AsuntoAdjuntoBo asuntoAdjuntoBoTmp = new AsuntoAdjuntoBo();
            gvwAdjuntos.DataSource = asuntoAdjuntoBoTmp.ObtenerAdjuntosPorHotel(int.Parse(ddlHotel.SelectedValue), new DateTime(int.Parse(txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 0, 0, 0));
            gvwAdjuntos.DataBind();
        }

        protected void gvwAdjuntos_OnRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbn = e.Row.FindControl("lnkDownload") as LinkButton;
                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(lbn);

            }
        }

        #endregion

        #region Boton
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaAdjunto = string.Empty;
                if (AsyncFileUpload1.HasFile)
                {
                    rutaAdjunto = "~/img/adjuntos/" + AsyncFileUpload1.FileName;
                }

                DateTime fecha = new DateTime(int.Parse(txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1);

                AsuntoAdjuntoBo asuntoAdjuntoBo = new AsuntoAdjuntoBo();
                asuntoAdjuntoBo.GuardarActualizar(int.Parse(ddlHotel.SelectedValue), txtAsunto.Text, rutaAdjunto, fecha);

                ddlHotel_SelectedIndexChanged(null, null);

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

        protected void btnEnviarInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaAdjunto = string.Empty;
                if (AsyncFileUpload2.HasFile)
                {
                    rutaAdjunto = "~/img/adjuntos/" + AsyncFileUpload2.FileName;
                }

                PropietarioBo propietarioBoTmp = new PropietarioBo();
                List<ObjetoGenerico> listaPropietarioTmp = propietarioBoTmp.VerTodosPorHotelPropietarioSinDistinc(int.Parse(ddlHotel.SelectedValue), Properties.Settings.Default.IdPropietario);

                ObjetoGenerico usuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
                List<string> listaAdjuntos = new List<string>();
                List<string> listaSuits = new List<string>();
                int idPropietarioTmp = listaPropietarioTmp[0].IdPropietario;
                int conTmp = 0;

                foreach (ObjetoGenerico propietarioTmp in listaPropietarioTmp)
                {
                    conTmp++;
                    listaAdjuntos.Add(Server.MapPath(rutaAdjunto));
                    listaSuits.Add(propietarioTmp.NumSuit);

                    try
                    {
                        if (conTmp == listaPropietarioTmp.Count)
                            conTmp = 0;

                        if (idPropietarioTmp != listaPropietarioTmp[conTmp].IdPropietario)
                        {

                            List<string> listaCorreoDestino = new List<string>();
                            listaCorreoDestino.Add(propietarioTmp.Correo);
                            listaCorreoDestino.Add(propietarioTmp.Correo2);
                            listaCorreoDestino.Add(propietarioTmp.Correo3);
                            listaCorreoDestino.Add(propietarioTmp.CorreoContacto);

                            bool esEnvioCorrecto = true;

                            Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                                   Properties.Settings.Default.ClaveRemitente,
                                                   Properties.Settings.Default.NombreRemitente,
                                                   usuarioLogin.Correo.Trim(),
                                                   txtCuerpoInfo.Text + "<br /><br /><br />",
                                                   "Información",
                                                   Properties.Settings.Default.HostSMTP,
                                                   Properties.Settings.Default.PuertoSMTP,
                                                   Properties.Settings.Default.EnableSsl,
                                                   listaAdjuntos,
                                                   listaCorreoDestino,
                                                   false,
                                                   ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas);
                            
                            listaAdjuntos = new List<string>();
                            listaSuits = new List<string>();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    idPropietarioTmp = listaPropietarioTmp[conTmp].IdPropietario;
                }                

                ddlHotel_SelectedIndexChanged(null, null);

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

        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;

            string[] types = filePath.Split(new char[] { '.' });
            string type = types[types.Length-1];

            if (!string.IsNullOrEmpty(type))
            {
                string contentType = "";
                switch (type)
                {
                    case "xls":
                        contentType = "application/vnd.ms-excel";
                        break;
                    case "doc":
                        contentType = "application/msword";
                        break;
                    case "pdf":
                        contentType = "application/pdf";
                        break;
                    case "xlsx":
                        contentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case "docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    default:
                        contentType = "";
                        break;
                }

                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                Response.WriteFile(filePath);
                Response.End();
            }            
        }

        protected void RemoveClick(object sender, EventArgs e)
        {
            int idAsuntoAdjunto = int.Parse((sender as ImageButton).CommandArgument);
            AsuntoAdjuntoBo oAsuntoAdjuntoBo = new AsuntoAdjuntoBo();
            oAsuntoAdjuntoBo.EliminarAdjunto(idAsuntoAdjunto);

            ddlMes_OnSelectedIndexChanged(null, null);
        }
        #endregion
    }
}