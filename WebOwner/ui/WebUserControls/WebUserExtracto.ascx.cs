using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DevExpress.XtraPrinting;
using Servicios;
using System.IO;
using DM;
using System.Text;
using Ionic.Zip;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserEstracto : System.Web.UI.UserControl
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
                this.txtAnoHasta.Text = DateTime.Now.Year.ToString();

                ParametroBo parametroBoTmp = new ParametroBo();
                gvwPropietarios.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));

                //this.EsVistaEstracto = false;
                CargarCombos();
                ddlHotel_SelectedIndexChanged(null, null);

                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                Session["listaIdSuitePropietario"] = new Dictionary<int, int>();
            }

            if (Page.Request.Params["__EVENTTARGET"] == "ctl00$Contenidoprincipal$WebUserEstracto1$ReportViewer_Reporte")//(this.EsVistaEstracto)
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

        public int IdSuit
        {
            get { return (int)ViewState["IdSuit"]; }
            set { ViewState["IdSuit"] = value; }
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
            DateTime fechaLiquidacion = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

            propietarioBoTmp = new PropietarioBo();
            //gvwPropietarios.DataSource = propietarioBoTmp.ObtenerPropietariosConSuite(int.Parse(ddlHotel.SelectedValue));
            // Se cambio a este metodo, porque los extractos se cruzaban con los propietarios anteriores de sus suite que habian vendido.
            gvwPropietarios.DataSource = propietarioBoTmp.ObtenerPropietariosConSuite(int.Parse(ddlHotel.SelectedValue), fechaLiquidacion);
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
            #region Valido si ya se envio el correo masivo
            DateTime fechaDesde = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            ValidarEnvioMasivo(fechaDesde, idHotel);
            #endregion

            Session["listaIdSuitePropietario"] = new Dictionary<int, int>();

            CargarGrilla();
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Valido si ya se envio el correo masivo
            DateTime fechaDesde = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            ValidarEnvioMasivo(fechaDesde, idHotel);
            #endregion

            gvwPropietarios.PageIndex = 0;
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
            Dictionary<int, int> listaIdSuite = (Dictionary<int, int>)Session["listaIdSuitePropietario"];
            string[] ids = imgBtnTmp.CommandArgument.Split(',');

            if (imgBtnTmp.ImageUrl == "~/img/117.png")
            {
                imgBtnTmp.ImageUrl = "~/img/95.png";
                listaIdSuite.Add(int.Parse(ids[0]), int.Parse(ids[1])); // IdSuite, IdPropietario
            }
            else
            {
                imgBtnTmp.ImageUrl = "~/img/117.png";
                listaIdSuite.Remove(int.Parse(ids[0]));
            }

            Session["listaIdSuitePropietario"] = listaIdSuite;
        }

        protected void gvwPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idSuite = int.Parse(gvwPropietarios.DataKeys[e.Row.RowIndex]["IdSuit"].ToString());
                ImageButton imgBtnTmp = (ImageButton)e.Row.FindControl("imgBtnSeleccion");

                Dictionary<int, int> listaIdSuite = (Dictionary<int, int>)Session["listaIdSuitePropietario"];
                string[] ids = imgBtnTmp.CommandArgument.Split(',');

                if (listaIdSuite.Count > 0)
                {
                    if (listaIdSuite.Where(I => I.Key == idSuite).Count() > 0)
                    {
                        imgBtnTmp.ImageUrl = "~/img/95.png";
                        if (listaIdSuite.Where(L => L.Key == int.Parse(ids[0])).Count() == 0)
                            listaIdSuite.Add(int.Parse(ids[0]), int.Parse(ids[1])); // IdSuite, IdPropietario
                    }
                    else
                    {
                        imgBtnTmp.ImageUrl = "~/img/117.png";
                        listaIdSuite.Remove(int.Parse(ids[0]));
                    }
                }
            }
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
                string[] idTmp = btnTmp.CommandArgument.Split(',');

                this.IdPropietario = int.Parse(idTmp[0]);
                this.IdSuit = int.Parse(idTmp[1]);
            }
            catch (Exception) { }

            List<string> listaAdjuntos = new List<string>();
            List<string> listaSuits = new List<string>();
            DateTime fechaDesde = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = new DateTime(int.Parse(this.txtAnoHasta.Text), int.Parse(ddlMesHasta.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            bool esAcumulado = cbEsAcumulado.Checked;
            string rutaTmp = Server.MapPath(Properties.Settings.Default.RutaExtracto);

            if (!esAcumulado)
                fechaHasta = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

            #region Valido si tiene la informacion estadistica

            InformacionEstadisticaBo infoEstadisticaBoTmp = new InformacionEstadisticaBo();
            if (!infoEstadisticaBoTmp.ValidarInfoEstadistica(fechaDesde, idHotel))
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No hay información Estadistica para este Hotel.";
                return;
            }
            #endregion

            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;

            AsuntoAdjuntoBo asuntoAdjuntoBo = new AsuntoAdjuntoBo();
            string archivoAdjunto = asuntoAdjuntoBo.ObtenerArchivoAdjunto(idHotel, fechaDesde);
            string textoAdjunto = asuntoAdjuntoBo.ObtenerTextoAdjunto(idHotel, fechaDesde);

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(this.IdPropietario, this.IdSuit);

            string nameFile = "Extracto_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss____") + propietarioTmp.Codigo + "_" + propietarioTmp.NumEscritura + "_" + propietarioTmp.NumIdentificacion + ".pdf";
            string rutaExtracto = rutaTmp + "/" + idHotel + "/" + nameFile;
            listaAdjuntos.Add(rutaExtracto);
            if (!string.IsNullOrEmpty(archivoAdjunto))
            {
                listaAdjuntos.Add(Server.MapPath(archivoAdjunto));
            }

            listaSuits.Add(propietarioTmp.NumEscritura);

            switch (configReporteTmp.ObtenerTipoExtracto(idHotel))
            {
                case 1:
                    extractoTmp = new WebOwner.reportes.XtraReport_Extracto(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 2:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 3:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 4:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCuatro(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 5:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 6:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSeis(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 7:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSiete(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 8:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoOcho(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                case 9:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoNueve(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                    break;

                default:
                    break;
            }

            List<string> listaCorreoDestino = new List<string>();
            listaCorreoDestino.Add(propietarioTmp.Correo);
            listaCorreoDestino.Add(propietarioTmp.Correo2);
            listaCorreoDestino.Add(propietarioTmp.Correo3);
            listaCorreoDestino.Add(propietarioTmp.CorreoContacto);

            bool esEnvioCorrecto = true;

            LiquidacionBo oLiquidacionBo = new LiquidacionBo();
            oLiquidacionBo.GuardarUrlExtracto(fechaDesde, this.IdPropietario, this.IdSuit, nameFile);

            Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                   Properties.Settings.Default.ClaveRemitente,
                                   Properties.Settings.Default.NombreRemitente,
                                   usuarioLogin.Correo.Trim(),
                                   textoAdjunto,
                                   "Extracto " + fechaDesde.ToString("MM-yyyy") + " Hotel: " + propietarioTmp.NombreHotel + " N° Suite: " + string.Join(",", listaSuits.ToArray()),
                                   Properties.Settings.Default.HostSMTP,
                                   Properties.Settings.Default.PuertoSMTP,
                                   Properties.Settings.Default.EnableSsl,
                                   listaAdjuntos,
                                   listaCorreoDestino,
                                   false,
                                   ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas, idHotel);

            //reportes.XtraReport_Extracto xr_Extracto = new WebOwner.reportes.XtraReport_Extracto(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad,this.IdPropietario, this.IdSuit, ref listaErrores);
        }

        protected void btnVerExtracto_Click(object sender, EventArgs e)
        {
            StringBuilder listaErrores = new StringBuilder();

            try
            {
                Button btnTmp = (Button)sender;
                string[] idTmp = btnTmp.CommandArgument.Split(',');

                this.IdPropietario = int.Parse(idTmp[0]);
                this.IdSuit = int.Parse(idTmp[1]);
            }
            catch (Exception) { }


            DateTime fechaDesde = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = new DateTime(int.Parse(this.txtAnoHasta.Text), int.Parse(ddlMesHasta.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            bool esAcumulado = cbEsAcumulado.Checked;

            if (!esAcumulado)
                fechaHasta = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;

            #region Valido si tiene la informacion estadistica

            InformacionEstadisticaBo infoEstadisticaBoTmp = new InformacionEstadisticaBo();
            if (!infoEstadisticaBoTmp.ValidarInfoEstadistica(fechaDesde, idHotel))
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No hay información Estadistica para este Hotel.";
                return;
            }
            #endregion

            switch (configReporteTmp.ObtenerTipoExtracto(idHotel))
            {
                case 1:
                    extractoTmp = new WebOwner.reportes.XtraReport_Extracto(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;

                case 2:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado, ref listaErrores);
                    break;

                case 3:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado, ref listaErrores);
                    break;

                case 4:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCuatro(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;

                case 5:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;

                case 6:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSeis(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;

                case 7:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSiete(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;

                case 8:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoOcho(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;

                case 9:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoNueve(this.IdPropietario, this.IdSuit, fechaDesde, fechaHasta, Properties.Settings.Default.IdPorcentajePropiedad, idHotel, esAcumulado);
                    break;


                default:
                    break;
            }

            if (listaErrores.ToString() != string.Empty)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaErrores.ToString();
            }
            else
            {
                string nombrePdf = this.IdPropietario + "_" + this.IdSuit + "_" + fechaDesde.Month + "_" + fechaDesde.Year;
                string rutaTmp = Server.MapPath("../../extractos/" + nombrePdf + ".pdf");

                ReportViewer_Reporte.Report = extractoTmp;
                //this.EsVistaEstracto = true;
            }
        }

        protected void btnEnviarExtracto_Click(object sender, EventArgs e)
        {
            ObjetoGenerico usuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
            StringBuilder listaErrores = new StringBuilder();

            DateTime fechaDesde = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = new DateTime(int.Parse(this.txtAnoHasta.Text), int.Parse(ddlMesHasta.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            bool esAcumulado = cbEsAcumulado.Checked;
            string rutaTmp = Server.MapPath(Properties.Settings.Default.RutaExtracto);
            short conEmail = 0;
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;
            List<string> listaAdjuntos = new List<string>();

            if (!esAcumulado)
                fechaHasta = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            short tipoExtracto = configReporteTmp.ObtenerTipoExtracto(idHotel);

            AsuntoAdjuntoBo asuntoAdjuntoBo = new AsuntoAdjuntoBo();
            string archivoAdjunto = asuntoAdjuntoBo.ObtenerArchivoAdjunto(idHotel, fechaDesde);
            string textoAdjunto = asuntoAdjuntoBo.ObtenerTextoAdjunto(idHotel, fechaDesde);

            #region valido si este hotel tiene configurado la plantilla del extracto
            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();
            Extracto configExtractoTmp = configExtractoBoTmp.Obtener(idHotel);

            if (configExtractoTmp == null)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No se ha configurado la plantilla del extracto \"Configuracion Extracto\" para este Hotel";
                return;
            }
            #endregion

            #region Valido si tiene la informacion estadistica

            InformacionEstadisticaBo infoEstadisticaBoTmp = new InformacionEstadisticaBo();
            if (!infoEstadisticaBoTmp.ValidarInfoEstadistica(fechaDesde, idHotel))
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No hay información Estadistica para este Hotel.";
                return;
            }
            #endregion

            #region Guardo registro de que ya se genero el envio masivo
            EnvioMasivoBo oEnvioMasivoBo = new EnvioMasivoBo();
            oEnvioMasivoBo.Guardar(fechaDesde, idHotel, ((ObjetoGenerico)Session["usuarioLogin"]).Id);
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

            int index = 0;
            int idPropietarioCurrent = listaPropietarioTmp[index].IdPropietario;
            int idPropietarioNext = listaPropietarioTmp[index + 1].IdPropietario;

            foreach (ObjetoGenerico propietarioTmp in listaPropietarioTmp)
            {
                string nameFile = "Extracto_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss____") + propietarioTmp.Codigo + "_" + propietarioTmp.NumEscritura + "_" + propietarioTmp.NumIdentificacion + ".pdf";
                string rutaExtracto = rutaTmp + "/" + idHotel + "/" + nameFile;
                listaAdjuntos.Add(rutaExtracto);

                try
                {
                    switch (tipoExtracto)
                    {
                        case 1:
                            extractoTmp = new WebOwner.reportes.XtraReport_Extracto(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 2:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 3:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 4:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCuatro(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 5:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 6:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSeis(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 7:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSiete(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 8:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoOcho(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 9:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoNueve(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;


                        default:
                            break;
                    }

                    if (idPropietarioCurrent == idPropietarioNext)
                    {
                        index++;
                        idPropietarioCurrent = idPropietarioNext;
                        try
                        {
                            idPropietarioNext = listaPropietarioTmp[index + 1].IdPropietario;
                        }
                        catch (Exception ex)
                        {
                            idPropietarioNext = -1;
                        }
                        continue;
                    }
                    index++;
                    idPropietarioCurrent = idPropietarioNext;
                    try
                    {
                        idPropietarioNext = listaPropietarioTmp[index + 1].IdPropietario;
                    }
                    catch (Exception ex)
                    {
                    }

                    if (!string.IsNullOrEmpty(archivoAdjunto))
                    {
                        listaAdjuntos.Add(Server.MapPath(archivoAdjunto));
                    }

                    List<string> listaCorreoDestino = new List<string>();
                    listaCorreoDestino.Add(propietarioTmp.Correo);
                    listaCorreoDestino.Add(propietarioTmp.Correo2);
                    listaCorreoDestino.Add(propietarioTmp.Correo3);
                    listaCorreoDestino.Add(propietarioTmp.CorreoContacto);

                    bool esEnvioCorrecto = true;

                    LiquidacionBo oLiquidacionBo = new LiquidacionBo();
                    oLiquidacionBo.GuardarUrlExtracto(fechaDesde, propietarioTmp.IdPropietario, propietarioTmp.IdSuit, nameFile);

                    Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                           Properties.Settings.Default.ClaveRemitente,
                                           Properties.Settings.Default.NombreRemitente,
                                           usuarioLogin.Correo.Trim(),
                                           textoAdjunto,
                                           "Extracto " + fechaDesde.ToString("MM-yyyy") + " Hotel: " + propietarioTmp.NombreHotel + " N° Suite: " + propietarioTmp.NumEscritura,
                                           Properties.Settings.Default.HostSMTP,
                                           Properties.Settings.Default.PuertoSMTP,
                                           Properties.Settings.Default.EnableSsl,
                                           listaAdjuntos,
                                           listaCorreoDestino,
                                           false,
                                           ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas, idHotel);

                    if (esEnvioCorrecto == false)
                        listaErrores.AppendLine("Propietario: " + propietarioTmp.PrimeroNombre + " " + propietarioTmp.SegundoNombre + " " + propietarioTmp.PrimerApellido + " " + propietarioTmp.SegundoApellido + " Identificación: " + propietarioTmp.NumIdentificacion);

                    listaAdjuntos = new List<string>();
                }
                catch (Exception ex)
                {
                    listaAdjuntos = new List<string>();
                }
            }

            //reportes.XtraReport_Extracto xr_Extracto = new WebOwner.reportes.XtraReport_Extracto(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores);

            if (listaErrores.ToString() != string.Empty)
            {
                listaErrores.Insert(0, "Los siguientes propietarios, no fueron enviados los extractos, faltan sus correos.");

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = listaErrores.ToString();
            }
        }

        protected void bntGuardarTodos_Click(object sender, EventArgs e)
        {
            StringBuilder listaErrores = new StringBuilder();

            DateTime fecha = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);

            string nombrePdf = fecha.Month + "_" + fecha.Year;
            string rutaTmp = Server.MapPath(Properties.Settings.Default.RutaExtracto);
            string nombreHotel = ddlHotel.SelectedItem.Text;

            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;

            switch (configReporteTmp.ObtenerTipoExtracto(idHotel))
            {
                case 1:
                    extractoTmp = new WebOwner.reportes.XtraReport_Extracto(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 2:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 3:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 4:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 5:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 6:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSeis(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 7:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSiete(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 8:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoOcho(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;

                case 9:
                    extractoTmp = new WebOwner.reportes.XtraReport_ExtractoNueve(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
                    break;
                default:
                    break;
            }

            //reportes.XtraReport_Extracto xr_Extracto = new WebOwner.reportes.XtraReport_Extracto(rutaTmp, fecha, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, ref listaErrores, ((ObjetoGenerico)Session["usuarioLogin"]).Correo, nombreHotel);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            uc_WebUserBuscadorPropietarioSuite.PageSize = this.PageSize;
            uc_WebUserBuscadorPropietarioSuite.CargarGrilla();
        }

        protected void btnEnviarSeleccionados_Click(object sender, EventArgs e)
        {
            ObjetoGenerico usuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
            Dictionary<int, int> listaIdSuitePropietario = (Dictionary<int, int>)Session["listaIdSuitePropietario"];
            if (listaIdSuitePropietario.Count == 0)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No hay propietarios seleccionados.";
                return;
            }

            StringBuilder listaErrores = new StringBuilder();

            DateTime fechaDesde = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
            DateTime fechaHasta = new DateTime(int.Parse(this.txtAnoHasta.Text), int.Parse(ddlMesHasta.SelectedValue), 1, 00, 00, 00);
            int idHotel = int.Parse(ddlHotel.SelectedValue);
            bool esAcumulado = cbEsAcumulado.Checked;
            string rutaTmp = Server.MapPath(Properties.Settings.Default.RutaExtracto);
            short conEmail = 0;
            SuitBo oSuiteBo = new SuitBo();
            DevExpress.XtraReports.UI.XtraReport extractoTmp = null;
            List<string> listaAdjuntos = new List<string>();
            List<string> listaSuits = new List<string>();

            if (esAcumulado)
                fechaHasta = new DateTime(int.Parse(this.txtAno.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ConfiguracionReporteBo configReporteTmp = new ConfiguracionReporteBo();
            short tipoExtracto = configReporteTmp.ObtenerTipoExtracto(idHotel);

            AsuntoAdjuntoBo asuntoAdjuntoBo = new AsuntoAdjuntoBo();
            string archivoAdjunto = asuntoAdjuntoBo.ObtenerArchivoAdjunto(idHotel, fechaDesde);
            string textoAdjunto = asuntoAdjuntoBo.ObtenerTextoAdjunto(idHotel, fechaDesde);

            #region valido si este hotel tiene configurado la plantilla del extracto
            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();
            Extracto configExtractoTmp = configExtractoBoTmp.Obtener(idHotel);

            if (configExtractoTmp == null)
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No se ha configurado la plantilla del extracto \"Configuracion Extracto\" para este Hotel";
                return;
            }
            #endregion

            #region Valido si tiene la informacion estadistica

            InformacionEstadisticaBo infoEstadisticaBoTmp = new InformacionEstadisticaBo();
            if (!infoEstadisticaBoTmp.ValidarInfoEstadistica(fechaDesde, idHotel))
            {
                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = "No hay información Estadistica para este Hotel.";
                return;
            }
            #endregion

            foreach (KeyValuePair<int, int> llaveValor in listaIdSuitePropietario)
            {
                #region Genero un retardo para no bloquerar el servicio SMTP
                conEmail++;
                if (conEmail == 25)
                {
                    System.Threading.Thread.Sleep(5000);
                    conEmail = 0;
                }
                #endregion

                ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(llaveValor.Value, llaveValor.Key);
                string nameFile = "Extracto_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss____") + propietarioTmp.Codigo + "_" + propietarioTmp.NumEscritura + "_" + propietarioTmp.NumIdentificacion + ".pdf";
                string rutaExtracto = rutaTmp + "/" + idHotel + "/" + nameFile;
                listaAdjuntos.Add(rutaExtracto);
                listaSuits.Add(oSuiteBo.ObtenerSuit(llaveValor.Key).NumEscritura);

                try
                {
                    switch (tipoExtracto)
                    {
                        case 1:
                            extractoTmp = new WebOwner.reportes.XtraReport_Extracto(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 2:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoDos(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 3:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoTres(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 4:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 5:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoCinco(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 6:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSeis(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 7:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoSiete(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 8:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoOcho(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;

                        case 9:
                            extractoTmp = new WebOwner.reportes.XtraReport_ExtractoNueve(rutaExtracto, fechaDesde, fechaHasta, idHotel, Properties.Settings.Default.IdPorcentajePropiedad, esAcumulado, propietarioTmp);
                            break;


                        default:
                            break;
                    }

                    List<string> listaCorreoDestino = new List<string>();
                    listaCorreoDestino.Add(propietarioTmp.Correo);
                    listaCorreoDestino.Add(propietarioTmp.Correo2);
                    listaCorreoDestino.Add(propietarioTmp.Correo3);
                    listaCorreoDestino.Add(propietarioTmp.CorreoContacto);

                    bool esEnvioCorrecto = true;

                    LiquidacionBo oLiquidacionBo = new LiquidacionBo();
                    oLiquidacionBo.GuardarUrlExtracto(fechaDesde, propietarioTmp.IdPropietario, propietarioTmp.IdSuit, nameFile);

                    Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                           Properties.Settings.Default.ClaveRemitente,
                                           Properties.Settings.Default.NombreRemitente,
                                           usuarioLogin.Correo.Trim(),
                                           textoAdjunto,
                                           "Extracto " + fechaDesde.ToString("MM-yyyy") + " Hotel: " + propietarioTmp.NombreHotel + " N° Suite: " + string.Join(",", listaSuits.ToArray()),
                                           Properties.Settings.Default.HostSMTP,
                                           Properties.Settings.Default.PuertoSMTP,
                                           Properties.Settings.Default.EnableSsl,
                                           listaAdjuntos,
                                           listaCorreoDestino,
                                           false,
                                           ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas, idHotel);

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