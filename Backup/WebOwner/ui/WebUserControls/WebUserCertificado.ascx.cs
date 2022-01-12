using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using System.Text;
using DM;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserCertificado1 : System.Web.UI.UserControl
    {
        PropietarioBo propietarioBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.Request.Params["__EVENTTARGET"] == "ctl00$Contenidoprincipal$WebUserEstracto1$ddlHotel")
            //{
            //    this.EsVistaEstracto = false;
            //}

            divError.Visible = false;
            divExito.Visible = false;

            uc_WebUserBuscadorPropietarioSuite.AlAceptar += new EventHandler(uc_WebUserBuscadorPropietarioSuite_AlAceptar);

            if (!Page.IsPostBack)
            {
                this.txtAno.Text = DateTime.Now.Year.ToString();

                ParametroBo parametroBoTmp = new ParametroBo();
                gvwPropietarios.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));

                //this.EsVistaEstracto = false;
                CargarCombos();
                ddlHotel_SelectedIndexChanged(null, null);

                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                Session["listaIdPropietario"] = new List<int>();
            }

            if (Page.Request.Params["__EVENTTARGET"] == "ctl00$Contenidoprincipal$WebUserCertificado1$ReportViewer_Reporte")//(this.EsVistaEstracto)
            {
                btnVerExtracto_Click(null, null);
            }
        }

        #region Propiedades

        //public bool EsVistaEstracto 
        //{
        //    get { return (bool)ViewState["EsVistaEstracto"]; }
        //    set { ViewState["EsVistaEstracto"] = value; }
        //}

        public int IdPropietario
        {
            get { return (int)ViewState["IdPropietario"]; }
            set { ViewState["IdPropietario"] = value; }
        }

        #endregion

        #region Metodos

        private void ValidarCorreos()
        {

        }

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                ddlHotel.DataSource = hotelBoTmp.VerTodos();
            else
                ddlHotel.DataSource = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();

            ddlHotel.Items.Insert(0, new ListItem("Seleccione...", "-1"));
        }

        private void CargarGrilla()
        {
            DateTime fechaLiquidacion = new DateTime(int.Parse(this.txtAno.Text), 1, 1, 00, 00, 00);

            propietarioBoTmp = new PropietarioBo();
            gvwPropietarios.DataSource = propietarioBoTmp.VerTodosPorHotelPropietarioConDistinc(int.Parse(ddlHotel.SelectedValue));
            gvwPropietarios.DataBind();
        }

        private void CrearReporte(bool esReporteWeb)
        {
            //DateTime fecha = DateTime.Parse(txtFecha.Text);
            //int idHotel = int.Parse(ddlHotel.SelectedValue);            

            //reportes.XtraReport_Extracto xr_Extracto = new WebOwner.reportes.XtraReport_Extracto( fecha);

            //string nombrePdf = propietarioTmp.IdPropietario + "_" + propietarioTmp.IdSuit + "_" + fecha.Month + "_" + fecha.Year;
            //string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");

            //// Opciones de pdf exportar.
            //PdfExportOptions pdfOptions = xr_Extracto.ExportOptions.Pdf;
            //pdfOptions.Compressed = true;
            //pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
            //pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
            //pdfOptions.DocumentOptions.Application = "Test Application";
            //pdfOptions.DocumentOptions.Author = "WebOwner";
            //pdfOptions.DocumentOptions.Keywords = "XtraReports, XtraPrinting";
            //pdfOptions.DocumentOptions.Subject = "Test Subject";
            //pdfOptions.DocumentOptions.Title = "Test Title";

            //xr_Extracto.ExportToPdf(rutaTmp);
            ////ReportViewer_Reporte.Report = xr_Extracto;

            //Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
            //                       Properties.Settings.Default.ClaveRemitente,
            //                       propietarioTmp.Correo,
            //                       Properties.Settings.Default.NombreRemitente,
            //                       "Extracto del mes.",
            //                       "Extracto " + fecha.ToString("MM-yyyy"),
            //                       Properties.Settings.Default.HostSMTP,
            //                       Properties.Settings.Default.PuertoSMTP,
            //                       Properties.Settings.Default.EnableSsl,
            //                       rutaTmp);            
        }

        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }

        public void ValidarEnvioMasivo(DateTime fechaLiquidacion, int idHotel)
        {
            EnvioMasivoBo oEnvioMasivoBo = new EnvioMasivoBo();
            if (oEnvioMasivoBo.EsMasivoEnviado(fechaLiquidacion, idHotel))
            {
                this.divExito.Visible = true;
                this.divError.Visible = false;
                this.lbltextoExito.Text = "Ya se ha enviado el correo masivo.";
                return;
            }
        }

        #endregion

        #region Eventos

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["listaIdSuitePropietario"] = new Dictionary<int, int>();
            CargarGrilla();
        }
        
        protected void gvwPropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwPropietarios.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        void uc_WebUserBuscadorPropietarioSuite_AlAceptar(object sender, EventArgs e)
        {
            if (uc_WebUserBuscadorPropietarioSuite.IdPropietarioBuscado == -1)
                return;

            int idPropietario = uc_WebUserBuscadorPropietarioSuite.IdPropietarioBuscado;
            int idSuite = uc_WebUserBuscadorPropietarioSuite.IdSuiteBuscado;

            ddlHotel.SelectedValue = uc_WebUserBuscadorPropietarioSuite.IdHotelBuscado.ToString();

            propietarioBoTmp = new PropietarioBo();
            List<ObjetoGenerico> listaPropBuscado = new List<ObjetoGenerico>();
            listaPropBuscado.Add(propietarioBoTmp.ObtenerPropietario(idPropietario, idSuite));
            gvwPropietarios.DataSource = listaPropBuscado;
            gvwPropietarios.DataBind();
        }

        protected void imgBtnSeleccion_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgBtnTmp = (ImageButton)sender;
            List<int> listaIdPropietario = (List<int>)Session["listaIdPropietario"];
            string id = imgBtnTmp.CommandArgument;

            if (imgBtnTmp.ImageUrl == "~/img/117.png")
            {
                imgBtnTmp.ImageUrl = "~/img/95.png";
                listaIdPropietario.Add(int.Parse(id));
            }
            else
            {
                imgBtnTmp.ImageUrl = "~/img/117.png";
                listaIdPropietario.Remove(int.Parse(id));
            }

            Session["listaIdPropietario"] = listaIdPropietario;
        }

        protected void gvwPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton imgBtnTmp = (ImageButton)e.Row.FindControl("imgBtnSeleccion");

            //    Dictionary<int, int> listaIdSuite = (Dictionary<int, int>)Session["listaIdSuitePropietario"];
            //    string[] ids = imgBtnTmp.CommandArgument.Split(',');

            //    if (listaIdSuite.Count > 0)
            //    {
            //        if (listaIdSuite.Where(I => I.Key == idSuite).Count() > 0)
            //        {
            //            imgBtnTmp.ImageUrl = "~/img/95.png";
            //            if (listaIdSuite.Where(L => L.Key == int.Parse(ids[0])).Count() == 0)
            //                listaIdSuite.Add(int.Parse(ids[0]), int.Parse(ids[1])); // IdSuite, IdPropietario
            //        }
            //        else
            //        {
            //            imgBtnTmp.ImageUrl = "~/img/117.png";
            //            listaIdSuite.Remove(int.Parse(ids[0]));
            //        }
            //    }
            //}
        }

        #endregion

        #region Boton

        protected void btnEnviarExtractoIndividual_Click(object sender, EventArgs e)
        {
            ObjetoGenerico usuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
            StringBuilder listaErrores = new StringBuilder();

            try
            {
                Button btnTmp = (Button)sender;
                this.IdPropietario = int.Parse(btnTmp.CommandArgument);
            }
            catch (Exception) { }
                        
            DateTime fecha = new DateTime(int.Parse(this.txtAno.Text), 1, 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            string nombrePdf = "Certificado_" + this.IdPropietario + "_" + fecha.Month + "_" + fecha.Year + "_" + fecha.Hour + "_" + fecha.Minute;
            string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");

            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = new WebOwner.reportes.XtraReport_Certificado(rutaTmp, fecha, idHotel, this.IdPropietario, ref listaErrores);

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(this.IdPropietario);

            List<string> listaAdjunto = new List<string>();
            listaAdjunto.Add(rutaTmp);

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
                                   "",
                                   "Certificado " + fecha.ToString("yyyy") + " Hotel: " + propietarioTmp.NombreHotel,
                                   Properties.Settings.Default.HostSMTP,
                                   Properties.Settings.Default.PuertoSMTP,
                                   Properties.Settings.Default.EnableSsl,
                                   listaAdjunto,
                                   listaCorreoDestino,
                                   false,
                                   ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas);
        }

        protected void btnVerExtracto_Click(object sender, EventArgs e)
        {
            StringBuilder listaErrores = new StringBuilder();

            try
            {
                Button btnTmp = (Button)sender;
                this.IdPropietario = int.Parse(btnTmp.CommandArgument);
            }
            catch (Exception) { }


            DateTime fecha = new DateTime(int.Parse(this.txtAno.Text), 1, 1, 00, 00, 00);
            string nombrePdf = this.IdPropietario + "_" + fecha.Month + "_" + fecha.Year + "_" + fecha.Hour + "_" + fecha.Minute;
            string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");
            int idHotel = int.Parse(ddlHotel.SelectedValue);

            DevExpress.XtraReports.UI.XtraReport extractoTmp = new WebOwner.reportes.XtraReport_Certificado(rutaTmp, fecha, idHotel, this.IdPropietario, ref listaErrores);

            if (listaErrores.ToString() != string.Empty)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaErrores.ToString();
            }
            else
                ReportViewer_Reporte.Report = extractoTmp;
        }

        protected void btnEnviarExtracto_Click(object sender, EventArgs e)
        {
            ObjetoGenerico usuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
            StringBuilder listaErrores = new StringBuilder();

            DateTime fecha = new DateTime(int.Parse(this.txtAno.Text), 1, 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);            
            short conEmail = 0;
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;
            List<string> listaAdjuntos = new List<string>();

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            short tipoExtracto = configReporteTmp.ObtenerTipoExtracto(idHotel);

            #region valido si este hotel tiene configurado el certificado
            ConfiguracionCertificadoBo configCertificadoBoTmp = new ConfiguracionCertificadoBo();

            if (String.IsNullOrEmpty(configCertificadoBoTmp.ObtenerTexto(idHotel)))
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No se ha configurado el certificado para este Hotel";
                return;
            }
            #endregion

            #region Valido si los propietarios tienen los correos correctos
            StringBuilder listaValidacion = propietarioBoTmp.ValidarCorreos(idHotel);
            if (listaValidacion.Length > 0)
            {
                listaValidacion.Insert(0, "Los siguientes propietarios, no tienen sus correos (Correo Personal ó Correo Contacto) correctamente.");

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaValidacion.ToString();
                return;
            }
            #endregion

            List<ObjetoGenerico> listaPropietarioTmp = propietarioBoTmp.VerTodosPorHotelPropietarioSinDistinc(idHotel, Properties.Settings.Default.IdPropietario);

            int idPropietarioTmp = listaPropietarioTmp[0].IdPropietario;
            int conTmp = 0;
            foreach (ObjetoGenerico propietarioTmp in listaPropietarioTmp)
            {

                #region Genero un retardo para no bloquerar el servicio SMTP
                conEmail++;
                //if (conEmail == 25)
                //{
                //    System.Threading.Thread.Sleep(5000);
                //    conEmail = 0;
                //}
                #endregion

                conTmp++;
                //string rutaExtracto = rutaTmp + "Certificado_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".pdf";
                string nombrePdf = "Certificado_" + propietarioTmp.IdPropietario + "_" + propietarioTmp.IdSuit + "_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");
                listaAdjuntos.Add(rutaTmp);

                try
                {
                    extractoTmp = new WebOwner.reportes.XtraReport_Certificado(rutaTmp, fecha, idHotel, propietarioTmp.IdPropietario, ref listaErrores);

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
                                               "",
                                               "Certificado " + fecha.ToString("yyyy") + " Hotel: " + propietarioTmp.NombreHotel,
                                               Properties.Settings.Default.HostSMTP,
                                               Properties.Settings.Default.PuertoSMTP,
                                               Properties.Settings.Default.EnableSsl,
                                               listaAdjuntos,
                                               listaCorreoDestino,
                                               false,
                                               ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas);

                        if (esEnvioCorrecto == false)
                            listaErrores.AppendLine("Propietario: " + propietarioTmp.PrimeroNombre + " " + propietarioTmp.SegundoNombre + " " + propietarioTmp.PrimerApellido + " " + propietarioTmp.SegundoApellido + " Identificación: " + propietarioTmp.NumIdentificacion);

                        listaAdjuntos = new List<string>();
                        //listaSuits = new List<string>();
                    }
                }
                catch (Exception ex)
                {
                }

                idPropietarioTmp = listaPropietarioTmp[conTmp].IdPropietario;
            }

            if (listaErrores.ToString() != string.Empty)
            {
                listaErrores.Insert(0, "Los siguientes propietarios, no fueron enviados los extractos, faltan sus correos.");

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaErrores.ToString();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            uc_WebUserBuscadorPropietarioSuite.PageSize = this.PageSize;
            uc_WebUserBuscadorPropietarioSuite.CargarGrilla();
        }

        protected void btnEnviarSeleccionados_Click(object sender, EventArgs e)
        {
            ObjetoGenerico usuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
            List<int> listaIdPropietario = (List<int>)Session["listaIdPropietario"];
            if (listaIdPropietario.Count == 0)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No hay propietarios seleccionados.";
                return;
            }

            StringBuilder listaErrores = new StringBuilder();

            DateTime fecha = new DateTime(int.Parse(this.txtAno.Text), 1, 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);            
            short conEmail = 0;
            SuitBo oSuiteBo = new SuitBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;
            List<string> listaAdjuntos = new List<string>();

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            short tipoExtracto = configReporteTmp.ObtenerTipoExtracto(idHotel);

            foreach (int idPropietario in listaIdPropietario)
            {
                #region Genero un retardo para no bloquerar el servicio SMTP
                conEmail++;
                if (conEmail == 25)
                {
                    System.Threading.Thread.Sleep(5000);
                    conEmail = 0;
                }
                #endregion

                //string rutaExtracto = rutaTmp + "Extracto_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".pdf";
                string nombrePdf = "Certificado_" + idPropietario + "_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");

                listaAdjuntos.Add(rutaTmp);
                ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(idPropietario);

                try
                {
                    extractoTmp = new WebOwner.reportes.XtraReport_Certificado(rutaTmp, fecha, idHotel, idPropietario, ref listaErrores);

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
                                           "",
                                           "Certificado " + fecha.ToString("yyyy") + " Hotel: " + propietarioTmp.NombreHotel,
                                           Properties.Settings.Default.HostSMTP,
                                           Properties.Settings.Default.PuertoSMTP,
                                           Properties.Settings.Default.EnableSsl,
                                           listaAdjuntos,
                                           listaCorreoDestino,
                                           false,
                                           ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas);

                    listaAdjuntos = new List<string>();

                }
                catch (Exception ex)
                {
                }
            }

            if (listaErrores.ToString() != string.Empty)
            {
                listaErrores.Insert(0, "Los siguientes propietarios, no fueron enviados los extractos, faltan sus correos.");

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaErrores.ToString();
            }
        }

        #endregion
    }
}