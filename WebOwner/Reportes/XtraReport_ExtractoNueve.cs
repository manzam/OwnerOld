using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using BO;
using DM;
using System.Collections.Generic;
using DevExpress.XtraPrinting;
using Servicios;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace WebOwner.reportes
{
    public partial class XtraReport_ExtractoNueve : DevExpress.XtraReports.UI.XtraReport
    {
        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};

        public XtraReport_ExtractoNueve(string rutaPdf, DateTime fechaDesde, DateTime fechaHasta, int idHotel, int idPorcentajeSuit, 
                                   bool esAcumulado)
        {
            InitializeComponent();

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            List<ObjetoGenerico> listaPropietarioTmp = propietarioBoTmp.
                                                       VerTodosPorHotelPropietario(idHotel, 
                                                                                   Properties.Settings.Default.IdPropietario);

            foreach (ObjetoGenerico propietarioTmp in listaPropietarioTmp)
            {
                string ruta = string.Empty;
                CrearExtracto(propietarioTmp, fechaDesde, fechaHasta, idPorcentajeSuit, idHotel, esAcumulado);

                // Opciones de pdf exportar.
                PdfExportOptions pdfOptions = this.ExportOptions.Pdf;
                pdfOptions.Compressed = true;
                pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
                pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
                pdfOptions.DocumentOptions.Application = "Owners";
                pdfOptions.DocumentOptions.Author = "WebOwner";
                pdfOptions.DocumentOptions.Keywords = "XtraReports, XtraPrinting";
                pdfOptions.DocumentOptions.Subject = "Owners";
                pdfOptions.DocumentOptions.Title = "Extracto";

                List<string> listaAdjuntos = new List<string>();
                AsuntoAdjuntoBo asuntoAdjuntoBo = new AsuntoAdjuntoBo();
                string archivoAdjunto = asuntoAdjuntoBo.ObtenerArchivoAdjunto(idHotel, fechaDesde);
                string textoAdjunto = asuntoAdjuntoBo.ObtenerTextoAdjunto(idHotel, fechaDesde);

                string rutaAdjuntos = rutaPdf;
                ruta = rutaPdf + "envioExtracto_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".pdf";
                this.ExportToPdf(ruta);
                listaAdjuntos.Add(ruta);

                if (!string.IsNullOrEmpty(archivoAdjunto))
                {
                    archivoAdjunto = archivoAdjunto.Replace("~/", "").Trim();
                    archivoAdjunto = archivoAdjunto.Replace("/", "\\").Trim();

                    archivoAdjunto = rutaAdjuntos.Replace("extractos\\", archivoAdjunto);
                    //asuntoAdjunto.RutaLogo = asuntoAdjunto.RutaLogo + rutaPdf.Replace("//", asuntoAdjunto.RutaLogo);
                    listaAdjuntos.Add(archivoAdjunto);
                }

                List<string> listaCorreoDestino = new List<string>();
                listaCorreoDestino.Add(propietarioTmp.Correo);
                listaCorreoDestino.Add(propietarioTmp.CorreoContacto);

                bool esEnvioCorrecto = true;

                Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                       Properties.Settings.Default.ClaveRemitente,
                                       Properties.Settings.Default.NombreRemitente,
                                       string.Empty,
                                       textoAdjunto,
                                       "Extracto " + fechaDesde.ToString("MM-yyyy") + " Hotel: " + propietarioTmp.NombreHotel + " N° Suite: " + propietarioTmp.NumSuit,
                                       Properties.Settings.Default.HostSMTP,
                                       Properties.Settings.Default.PuertoSMTP,
                                       Properties.Settings.Default.EnableSsl,
                                       listaAdjuntos,
                                       listaCorreoDestino,
                                       false,
                                       ref esEnvioCorrecto,
                                       Properties.Settings.Default.IsPruebas, idHotel);

                //if (esEnvioCorrecto == false)
                //    listaErrores.AppendLine("Propietario: " + propietarioTmp.PrimeroNombre + " " + propietarioTmp.SegundoNombre + " " + propietarioTmp.PrimerApellido + " " + propietarioTmp.SegundoApellido + " Identificación: " + propietarioTmp.NumIdentificacion);
            }
        }

        public XtraReport_ExtractoNueve(string rutaPdf, DateTime fechaDesde, DateTime fechaHasta, int idHotel, int idPorcentajeSuit, bool esAcumulado, ObjetoGenerico propietarioTmp)
        {
            InitializeComponent();

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            CrearExtracto(propietarioTmp, fechaDesde, fechaHasta, idPorcentajeSuit, idHotel, esAcumulado);

            PdfExportOptions pdfOptions = this.ExportOptions.Pdf;
            pdfOptions.Compressed = true;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
            pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
            pdfOptions.DocumentOptions.Application = "Owners";
            pdfOptions.DocumentOptions.Author = "WebOwner";
            pdfOptions.DocumentOptions.Keywords = "XtraReports, XtraPrinting";
            pdfOptions.DocumentOptions.Subject = "Owners";
            pdfOptions.DocumentOptions.Title = "Extracto";

            this.ExportToPdf(rutaPdf);
        }

        public XtraReport_ExtractoNueve(int idpropietario, int idSuit, DateTime mesDesdeLiquidacion, DateTime mesHastaLiquidacion,
                                   int idPorcentajeSuit, int idHotel, bool esAcumulado)
        {
            InitializeComponent();            

            PropietarioBo propietarioBoTmp = new PropietarioBo();            

            ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(idpropietario, idSuit);
            CrearExtracto(propietarioTmp, mesDesdeLiquidacion, mesHastaLiquidacion, idPorcentajeSuit, idHotel, esAcumulado);
        }

        public XtraReport_ExtractoNueve(string rutaPdf, DateTime fecha, int idHotel, int idPorcentajeSuit,
                                   ref StringBuilder listaErrores, string correo, string nombreHotel)
        {
            InitializeComponent();
            /*
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            List<ObjetoGenerico> listaPropietarioTmp = propietarioBoTmp.
                                                       VerTodosPorHotelPropietario(idHotel,
                                                                                   Properties.Settings.Default.IdPropietario);

            ZipFile zip = new ZipFile();
            int i = 0;
            short partes = 1;
            int maxCorreos = 70;
            string rutaZip = "";
            List<string> listaCorreoDestino = new List<string>();
            List<string> listaCorreo = new List<string>();
            listaCorreo.Add(correo);

            foreach (ObjetoGenerico propietarioTmp in listaPropietarioTmp)
            {
                string ruta = rutaPdf + propietarioTmp.PrimeroNombre + "_" + propietarioTmp.SegundoNombre + "_" + propietarioTmp.PrimerApellido + "_" + propietarioTmp.SegundoApellido + "_NumSuite_" + propietarioTmp.NumSuit + "_" + i + "_" + ".pdf";

                CrearExtracto(propietarioTmp, fecha, idPorcentajeSuit, idHotel, propietarioTmp.IdSuit, ref listaErrores);

                // Opciones de pdf exportar.
                PdfExportOptions pdfOptions = this.ExportOptions.Pdf;
                pdfOptions.Compressed = true;
                pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
                pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
                pdfOptions.DocumentOptions.Application = "Owners";
                pdfOptions.DocumentOptions.Author = "WebOwner";
                pdfOptions.DocumentOptions.Keywords = "XtraReports, XtraPrinting";
                pdfOptions.DocumentOptions.Subject = "Owners";
                pdfOptions.DocumentOptions.Title = "Extracto";

                this.ExportToPdf(ruta);
                
                listaCorreoDestino.Add(propietarioTmp.Correo);
                listaCorreoDestino.Add(propietarioTmp.CorreoContacto);

                zip.AddFile(ruta, "");
                i++;

                if (i >= maxCorreos)
                {
                    rutaZip = rutaPdf + "comprimidos/" + nombreHotel + "_" + fecha.Month + "_" + fecha.Year + "_" + partes + "_Parte" + ".zip";
                    zip.Save(rutaZip);                    

                    Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                           Properties.Settings.Default.ClaveRemitente,
                                           Properties.Settings.Default.NombreRemitente,
                                           "Extractos " + nombreHotel,
                                           "Extracto " + fecha.ToString("MM-yyyy") + " Parte " + partes + ".",
                                           Properties.Settings.Default.HostSMTP,
                                           Properties.Settings.Default.PuertoSMTP,
                                           Properties.Settings.Default.EnableSsl,
                                           rutaZip,
                                           listaCorreo);

                    listaCorreoDestino = new List<string>();
                    zip = new ZipFile();
                    maxCorreos = maxCorreos * 2;
                    rutaZip = "";
                    partes++;
                }
            }

            if (zip.Count > 0)
            {
                rutaZip = rutaPdf + "comprimidos/" + nombreHotel + "_" + fecha.Month + "_" + fecha.Year + "_" + partes + "_Parte" + ".zip";
                zip.Save(rutaZip);

                listaCorreo.Add(correo);

                Utilities.EnviarCorreo(Properties.Settings.Default.CorreoRemitente,
                                       Properties.Settings.Default.ClaveRemitente,
                                       Properties.Settings.Default.NombreRemitente,
                                       "Extractos " + nombreHotel,
                                       "Extracto " + fecha.ToString("MM-yyyy") + " Parte " + partes + ".",
                                       Properties.Settings.Default.HostSMTP,
                                       Properties.Settings.Default.PuertoSMTP,
                                       Properties.Settings.Default.EnableSsl,
                                       rutaZip,
                                       listaCorreo);
            }
             */
        }

        public void CrearExtracto(ObjetoGenerico propietarioTmp, DateTime fechaDesde, DateTime fechaHasta, int idVariable,
                                  int idHotel, bool esAcumulado)
        {
            this.pbLogo.ImageUrl = propietarioTmp.RutaLogo;
            this.lblNombreHotel.Text = propietarioTmp.NombreHotel.ToUpper();            
            this.lblFechaExpedicion.Text = DateTime.Now.Day.ToString() + " de " + meses[DateTime.Now.Month - 1] + " de " + DateTime.Now.Year.ToString();
            this.lblMesLiquidacion.Text = meses[fechaDesde.Month - 1] + " de " + fechaDesde.Year;
            this.lblNit.Text = propietarioTmp.Nit;
            this.lblPropietario.Text = propietarioTmp.PrimeroNombre + " " + propietarioTmp.SegundoNombre + " " + propietarioTmp.PrimerApellido + " " + propietarioTmp.SegundoApellido;            
            this.lblDireccion.Text = propietarioTmp.Direccion;
            this.lblNumSuite.Text = propietarioTmp.NumEscritura;
            this.lblNumCuenta.Text = propietarioTmp.NumCuenta;
            this.lblNombreBanco.Text = (propietarioTmp.NombreBanco == "SIN BANCO") ? string.Empty : propietarioTmp.NombreBanco;

            if (esAcumulado)
            {
                lblNombreExtracto.Text = "Extracto acumulado :";
                lblMesLiquidacion.Text = meses[fechaDesde.Month - 1] + " de " + fechaDesde.Year + " - " + meses[fechaHasta.Month - 1] + " de " + fechaHasta.Year;
            }

            try
            {
                if (propietarioTmp.NumIdentificacion.Contains('-'))
                {
                    string[] nit = propietarioTmp.NumIdentificacion.Split('-');
                    this.lblNitPropietario.Text = (double.Parse(nit[0].Trim())).ToString("N0") + "-" + nit[1].Trim();
                }
                else
                    this.lblNitPropietario.Text = (double.Parse(propietarioTmp.NumIdentificacion)).ToString("N0");
            }
            catch (Exception ex)
            {
                this.lblNitPropietario.Text = propietarioTmp.NumIdentificacion;
            }


            this.ObtenerInfoEstadistica(propietarioTmp.IdPropietario, idHotel, propietarioTmp.IdSuit, fechaDesde, fechaHasta, esAcumulado);
            this.ObtenerInfoLiquidacion(propietarioTmp.IdPropietario, idHotel, propietarioTmp.IdSuit, fechaDesde, fechaHasta, esAcumulado);
        }

        private void ObtenerInfoEstadistica(int idPropietario, int idHotel, int idSuite, DateTime fechaDesde, DateTime fechaHasta, bool esAcumulado)
        {
            this.xrTblinfoEstadistica.Rows.Clear();

            InformacionEstadisticaBo infoEstadisticaBoTmp = new InformacionEstadisticaBo();
            List<ObjetoGenerico> listaInfoEstadistica = null;

            if (esAcumulado)
                listaInfoEstadistica = infoEstadisticaBoTmp.Obtener(idHotel, fechaHasta, idPropietario, idSuite); // Para que coja el valor del ultimo mes siempre
            else
                listaInfoEstadistica = infoEstadisticaBoTmp.Obtener(idHotel, fechaDesde, idPropietario, idSuite);

            XRTableRow miFila = null;

            for (int i = 0; i < listaInfoEstadistica.Count; i++)
            {
                if (i % 2 == 0)
                {
                    miFila = new XRTableRow();
                    miFila.WidthF = 730;

                    XRTableCell celdaNombre = new XRTableCell();
                    celdaNombre.Text = listaInfoEstadistica[i].Nombre;
                    celdaNombre.TextAlignment = TextAlignment.MiddleLeft;
                    celdaNombre.Padding = new PaddingInfo(3, 0, 0, 0);
                    celdaNombre.WidthF = float.Parse("182.5");
                    celdaNombre.HeightF = float.Parse("28");
                    miFila.Cells.Add(celdaNombre);

                    XRTableCell celdaValor = new XRTableCell();
                    celdaValor.Text = listaInfoEstadistica[i].Valor.ToString("N0") + " " + ((listaInfoEstadistica[i].Sufijo == "-1") ? "" : listaInfoEstadistica[i].Sufijo);

                    //if (listaInfoEstadistica[i].EsVarAcumulada)
                    //    celdaValor.Text = this.ObtenerValorAcumulado(idPropietario, idHotel, listaInfoEstadistica[i].IdInformacionEstadistica, fechaDesde, fechaHasta, idSuite).ToString("N0") + " " + ((listaInfoEstadistica[i].Sufijo == "-1") ? "" : listaInfoEstadistica[i].Sufijo);
                    //else
                    //{
                    //    // por lo general es la ultima del mes seleccionado
                    //    celdaValor.Text = listaInfoEstadistica[i].Valor.ToString("N0") + " " + ((listaInfoEstadistica[i].Sufijo == "-1") ? "" : listaInfoEstadistica[i].Sufijo);

                    //    if (esAcumulado)
                    //    {
                    //        if (listaInfoEstadistica[i].ValorAcumulado == 1)
                    //            celdaValor.Text = "0";
                    //    }
                    //}                        

                    celdaValor.TextAlignment = TextAlignment.MiddleLeft;
                    celdaValor.Padding = new PaddingInfo(0, 3, 0, 0);
                    celdaValor.WidthF = float.Parse("182.5");
                    celdaValor.HeightF = float.Parse("28");
                    miFila.Cells.Add(celdaValor);
                }
                else
                {
                    XRTableCell celdaNombre = new XRTableCell();
                    celdaNombre.Text = listaInfoEstadistica[i].Nombre;
                    celdaNombre.TextAlignment = TextAlignment.MiddleLeft;
                    celdaNombre.Padding = new PaddingInfo(3, 0, 0, 0);
                    celdaNombre.WidthF = float.Parse("182.5");
                    celdaNombre.HeightF = float.Parse("28");
                    miFila.Cells.Add(celdaNombre);

                    XRTableCell celdaValor = new XRTableCell();
                    celdaValor.Text = listaInfoEstadistica[i].Valor.ToString("N0") + " " + ((listaInfoEstadistica[i].Sufijo == "-1") ? "" : listaInfoEstadistica[i].Sufijo);

                    //if (listaInfoEstadistica[i].EsVarAcumulada)
                    //    celdaValor.Text = this.ObtenerValorAcumulado(idPropietario, idHotel, listaInfoEstadistica[i].IdInformacionEstadistica, fechaDesde, fechaHasta, idSuite).ToString("N0") + " " + ((listaInfoEstadistica[i].Sufijo == "-1") ? "" : listaInfoEstadistica[i].Sufijo);
                    //else
                    //{
                    //    // por lo general es la ultima del mes seleccionado
                    //    celdaValor.Text = listaInfoEstadistica[i].Valor.ToString("N0") + " " + ((listaInfoEstadistica[i].Sufijo == "-1") ? "" : listaInfoEstadistica[i].Sufijo);

                    //    if (esAcumulado)
                    //    {
                    //        if (listaInfoEstadistica[i].ValorAcumulado == 1)
                    //            celdaValor.Text = "0";
                    //    }
                    //}

                    celdaValor.TextAlignment = TextAlignment.MiddleLeft;
                    celdaValor.Padding = new PaddingInfo(0, 3, 0, 0);
                    celdaValor.WidthF = float.Parse("182.5");
                    celdaValor.HeightF = float.Parse("28");
                    miFila.Cells.Add(celdaValor);

                    this.xrTblinfoEstadistica.Rows.Add(miFila);
                }                                
            }

            this.xrTblinfoEstadistica.Rows.Add(miFila);
        }

        private double ObtenerValorAcumulado(int idPropietario, int idHotel, int idConceptoAcumular, DateTime fechaDesde, DateTime fechaHasta, int idSuite)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = 0;
                Concepto concepto = Contexto.Concepto.Where(C => C.Informacion_Estadistica.IdInformacionEstadistica == idConceptoAcumular).FirstOrDefault();
                if (concepto != null)
                {
                    try
                    {
                        if (concepto.NivelConcepto == 1)
                            valor = Contexto.Liquidacion.
                                    Where(L => L.FechaPeriodoLiquidado.Year >= fechaDesde.Year &&
                                               L.FechaPeriodoLiquidado.Month >= fechaDesde.Month &&
                                               L.FechaPeriodoLiquidado.Year <= fechaHasta.Year &&
                                               L.FechaPeriodoLiquidado.Month <= fechaHasta.Month &&
                                               L.Hotel.IdHotel == idHotel &&
                                               L.Concepto.IdConcepto == concepto.IdConcepto).
                                    Sum(L => L.Valor);
                        else
                            valor = Contexto.Liquidacion.
                                Where(L => L.FechaPeriodoLiquidado.Year >= fechaDesde.Year &&
                                           L.FechaPeriodoLiquidado.Month >= fechaDesde.Month &&
                                           L.FechaPeriodoLiquidado.Year <= fechaHasta.Year &&
                                           L.FechaPeriodoLiquidado.Month <= fechaHasta.Month &&
                                           L.Propietario.IdPropietario == idPropietario &&
                                           L.Suit.IdSuit == idSuite &&
                                           L.Concepto.IdConcepto == concepto.IdConcepto).
                                Sum(L => L.Valor);
                    }
                    catch (Exception ex)
                    {
                        return valor;
                    }

                }
                return valor;
            }
        }

        private void ObtenerInfoLiquidacion(int idPropietario, int idHotel, int idSuite, DateTime fechaDesde, DateTime fechaHasta, bool esAcumulado)
        {
            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();
            Extracto extractoTmp = configExtractoBoTmp.Obtener(idHotel);

            lblInfoExtracto.Text = extractoTmp.DescripcionExtracto;
            lblInfoExtracto.Text = lblInfoExtracto.Text.Replace("%fecha%", meses[fechaDesde.Month - 1] + " de " + fechaDesde.ToString("yyyy"));
            txtPieExtracto.Text = extractoTmp.PieExtracto;
            imgFirma.ImageUrl = extractoTmp.FirmaLogo;

            ConceptoBo conceptoBoTmp = new ConceptoBo();
            ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
            VariableBo variableBoTmp = new VariableBo();

            foreach (Detalle_Extracto itemDetalle in extractoTmp.Detalle_Extracto)
            {
                double valor = 0;
                XRLabel lblTmp = (XRLabel)this.FindControl(itemDetalle.IdControl, true);

                if (itemDetalle.IdControl == "ddlUtilidadOperacional")
                { 
                }

                // Esta validacion se hace para validar si eligio ninguna variable
                if (itemDetalle.IdVariable.Value == -1)
                {
                    lblTmp.Text = "0";
                    continue;
                }

                if (itemDetalle.NombreTabla == "Concepto") // Hace referencia a un calculo
                    valor = conceptoBoTmp.ObtenerValorLiquidacion(idPropietario, itemDetalle.IdVariable.Value, idSuite, idHotel, fechaDesde, fechaHasta, esAcumulado);
                else if (itemDetalle.NombreTabla == "Ninguno")
                    valor = 0;
                else
                {
                    switch (itemDetalle.NombreTabla.Split(' ')[1].Trim())
                    {
                        case "Hotel":
                                valor = valorVariableBoTmp.Obtener(itemDetalle.IdVariable.Value, fechaDesde, fechaHasta);
                            break;
                        case "Propietario":
                            valor = variableBoTmp.Obtener(idPropietario, itemDetalle.IdVariable.Value, idSuite, idHotel, fechaDesde, fechaHasta, esAcumulado);
                            break;
                        default: // Constante
                            valor = (variableBoTmp.Obtener(itemDetalle.IdVariable.Value)).ValorConstante;
                            break;
                    }                  
                }

                switch (itemDetalle.IdControl)
                {
                    // Si es el porcentaje propietario, para pasarlo a decimal (79,75)
                    case "ddlPorcentajeIngresoPropietario":
                        lblTmp.Text = "INGRESO DEL PROPIETARIO (" + (valor * 100).ToString("N0") + "%) A DISTRIBUIR";
                        break;

                    case "ddlCoeficiente9":
                        lblTmp.Text = Math.Round((valor * 100), 3).ToString();
                        break;

                    case "ddlParticipacionPropietario9":
                        lblTmp.Text = (valor * 100).ToString("N2");
                        break;

                    case "ddlParticipacionPropietario3":
                        lblTmp.Text = (valor * 100).ToString("N2");
                        break;

                    default:
                        lblTmp.Text = valor.ToString("N0");
                        break;
                }
            }
        }
    }
}