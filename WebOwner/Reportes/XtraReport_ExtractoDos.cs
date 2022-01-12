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
    public partial class XtraReport_ExtractoDos : DevExpress.XtraReports.UI.XtraReport
    {
        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};

        public XtraReport_ExtractoDos(string rutaPdf, DateTime fechaDesde, DateTime fechaHasta, int idHotel, int idPorcentajeSuit, 
                                      bool esAcumulado, ref StringBuilder listaErrores)
        {
            InitializeComponent();
            
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            List<ObjetoGenerico> listaPropietarioTmp = propietarioBoTmp.
                                                       VerTodosPorHotelPropietario(idHotel, 
                                                                                   Properties.Settings.Default.IdPropietario);

            foreach (ObjetoGenerico propietarioTmp in listaPropietarioTmp)
            {
                string ruta = string.Empty;
                CrearExtracto(propietarioTmp, fechaDesde, fechaHasta, idPorcentajeSuit, idHotel, propietarioTmp.IdSuit, esAcumulado);

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
                string textoAdjunto = asuntoAdjuntoBo.ObtenerTextoAdjunto(idHotel);

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
                                       Properties.Settings.Default.IsPruebas, idPorcentajeSuit);

                if (esEnvioCorrecto == false)
                    listaErrores.AppendLine("Propietario: " + propietarioTmp.PrimeroNombre + " " + propietarioTmp.SegundoNombre + " " + propietarioTmp.PrimerApellido + " " + propietarioTmp.SegundoApellido + " Identificación: " + propietarioTmp.NumIdentificacion);
            }
        }

        public XtraReport_ExtractoDos(string rutaPdf, DateTime fechaDesde, DateTime fechaHasta, int idHotel, int idPorcentajeSuit, bool esAcumulado, ObjetoGenerico propietarioTmp)
        {
            InitializeComponent();            

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            CrearExtracto(propietarioTmp, fechaDesde, fechaHasta, idPorcentajeSuit, idHotel, propietarioTmp.IdSuit, esAcumulado);

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

        public XtraReport_ExtractoDos(int idpropietario, int idSuit, DateTime fechaDesde, DateTime fechaHasta,
                                      int idPorcentajeSuit, int idHotel, bool esAcumulado, ref StringBuilder listaErrores)
        {
            InitializeComponent();
            
            PropietarioBo propietarioBoTmp = new PropietarioBo();            

            ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietario(idpropietario, idSuit);
            CrearExtracto(propietarioTmp, fechaDesde, fechaHasta, idPorcentajeSuit, idHotel, idSuit, esAcumulado);
        }

        public XtraReport_ExtractoDos(string rutaPdf, DateTime fecha, int idHotel, int idPorcentajeSuit,
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
             * */
        }

        public void CrearExtracto(ObjetoGenerico propietarioTmp, DateTime fechaDesde, DateTime fechaHasta, int idVariable,
                                  int idHotel, int idSuite, bool esAcumulado)
        {
            this.pbLogo.ImageUrl = propietarioTmp.RutaLogo;
            this.lblNombreHotel.Text = propietarioTmp.NombreHotel.ToUpper();            
            this.lblNit.Text = propietarioTmp.Nit;
            this.lblFechaExpedicion.Text = DateTime.Now.Day.ToString() + " de " + meses[DateTime.Now.Month - 1] + " de " + DateTime.Now.Year.ToString();
            this.lblPropietario.Text = propietarioTmp.PrimeroNombre + " " + propietarioTmp.SegundoNombre + " " + propietarioTmp.PrimerApellido + " " + propietarioTmp.SegundoApellido;
            this.lblDireccion.Text = propietarioTmp.Direccion;
            this.lblNumSuite.Text = propietarioTmp.NumSuit;

            //this.lblNomHotel.Text = propietarioTmp.NombreHotel.ToUpper();
            //this.lblMesAtras.Text = meses[fechaDesde.Month - 1];
            //this.lblMesAtras2.Text = meses[fechaDesde.Month - 1];            
            //this.lblMesAtras3.Text = meses[fechaDesde.Month - 1];
            //this.lblMesAtras4.Text = meses[fechaDesde.Month - 1];
            //this.lblFechaActual.Text = meses[fechaDesde.Month] + " de " + fechaDesde.Year + ".";

            lblInfoExtracto1.Text = this.lblInfoExtracto1.Text.Replace("lblNomHotel", propietarioTmp.NombreHotel.ToUpper());
            lblInfoExtracto1.Text = this.lblInfoExtracto1.Text.Replace("lblFechaActual", meses[fechaDesde.Month - 1] + " de " + fechaDesde.Year + ".");

            lblInfoExtracto1.Text = this.lblInfoExtracto1.Text.Replace("lblMesAtras", meses[fechaDesde.Month - 1]);
            lblInfoExtracto2.Text = this.lblInfoExtracto2.Text.Replace("lblMesAtras", meses[fechaDesde.Month - 1]);

            if (esAcumulado)
                lblNombreExtracto.Text = "Extracto acumulado : " + meses[fechaDesde.Month - 1] + " de " + fechaDesde.Year + " \n " + meses[fechaHasta.Month - 1] + " de " + fechaHasta.Year;
            else
                lblNombreExtracto.Text = "";

            this.ObtenerInfoLiquidacion(propietarioTmp.IdPropietario, idHotel, idSuite, fechaDesde, fechaHasta, esAcumulado);
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

            txtPieExtracto.Text = extractoTmp.PieExtracto;
            imgFirma.ImageUrl = extractoTmp.FirmaLogo;

            ConceptoBo conceptoBoTmp = new ConceptoBo();
            ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
            VariableBo variableBoTmp = new VariableBo();
            InformacionEstadisticaBo infoEstadisticaBo = new InformacionEstadisticaBo();
            InformacionEstadisticaBo infoEstadistica = new InformacionEstadisticaBo();

            foreach (Detalle_Extracto itemDetalle in extractoTmp.Detalle_Extracto)
            {
                double valor = 0;
                XRLabel lblTmp = (XRLabel)this.FindControl(itemDetalle.IdControl, true);

                switch (itemDetalle.NombreTabla)
                {
                    case "Concepto": // Hace referencia a un calculo                        
                        valor = conceptoBoTmp.ObtenerValorLiquidacion(idPropietario, itemDetalle.IdVariable.Value, idSuite, idHotel, fechaDesde, fechaHasta, esAcumulado);
                        break;
                    case "Información Estadstica":
                        valor = (infoEstadisticaBo.Obtener2(idHotel, fechaDesde, itemDetalle.IdVariable.Value)).Valor;
                        break;

                    case "Ninguno":
                        valor = 0;
                        break;

                    default:

                        string tipo = itemDetalle.NombreTabla.Split(' ')[1].Trim();
                        switch (tipo)
                        {
                            case "Hotel":
                                    valor = valorVariableBoTmp.Obtener(itemDetalle.IdVariable.Value, fechaDesde, fechaHasta);
                                break;
                            case "Propietario":
                                valor = variableBoTmp.Obtener(idPropietario, itemDetalle.IdVariable.Value, idSuite, idHotel, fechaDesde, fechaHasta, esAcumulado);
                                break;
                            case "Estadstica":
                                valor = (infoEstadistica.Obtener2(idHotel, fechaDesde, itemDetalle.IdVariable.Value)).Valor;
                                break;
                            default: // Constante
                                valor = (variableBoTmp.Obtener(itemDetalle.IdVariable.Value)).ValorConstante;
                                break;
                        }
                        break;
                }

                switch (itemDetalle.IdControl)
                {
                    // Si es el porcentaje propietario, para pasarlo a decimal (79,75)
                    case "ddlPorcentajeIngresoPropietario":
                        lblTmp.Text = "INGRESO DEL PRETARIO (" + (valor * 100).ToString("N0") + "%) A DISTRIBUIR";
                        break;

                    case "ddlCoeficiente":
                    case "ddlCoeficienteSuite":
                    case "ddlCoeficienteParticipacion":
                        lblTmp.Text = (valor * 100).ToString("N5");
                        break;

                    case "ddlCoeficiente3":
                        lblTmp.Text = Math.Round((valor * 100), 3).ToString();
                        break;

                    case "ddlParticipacionPropietario":
                    case "ddlParticipacionPropietario3":
                        lblTmp.Text = (valor * 100).ToString("N2");
                        break;

                    case "ddlFaraAdicional":
                        lblTmp.Text = valor.ToString("N");
                        break;

                    default:
                        if (lblTmp == null)
                        {
                            lblInfoExtracto1.Text = lblInfoExtracto1.Text.Replace(itemDetalle.IdControl, valor.ToString("N0"));
                            lblInfoExtracto2.Text = lblInfoExtracto2.Text.Replace(itemDetalle.IdControl, valor.ToString("N0"));
                        }
                        else
                            lblTmp.Text = valor.ToString("N0");
                        break;
                }
            }

            totalCo.Text = (double.Parse(ddlUtilidadOperativaCop.Text) - (double.Parse(ddlFaraContractualCop.Text) + double.Parse(ddlFaraAdicionalCop.Text) + double.Parse(ddlRetencionFuenteCop.Text))).ToString("N0");
            totalSuite.Text = (double.Parse(ddlUtilidadOperativaSuite.Text) - (double.Parse(ddlFaraContractualSuite.Text) + double.Parse(ddlFaraAdicionalSuite.Text) + double.Parse(ddlRetencionFuenteSuite.Text))).ToString("N0");
        }

    }
}
