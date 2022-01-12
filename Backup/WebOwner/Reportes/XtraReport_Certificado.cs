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
using System.Data;

namespace WebOwner.reportes
{
    public partial class XtraReport_Certificado : DevExpress.XtraReports.UI.XtraReport
    {
        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};

        public XtraReport_Certificado(string rutaPdf, DateTime fecha, int idHotel, int idPropietario, ref StringBuilder listaErrores)
        {
            InitializeComponent();            

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            ObjetoGenerico propietarioTmp = propietarioBoTmp.ObtenerPropietarioCertificado(idPropietario, idHotel);

            CrearCertificado(propietarioTmp, fecha, idHotel);

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


        public void CrearCertificado(ObjetoGenerico propietarioTmp, DateTime fecha, int idHotel)
        {
            ConfiguracionCertificadoBo configCertificadoBoTmp = new ConfiguracionCertificadoBo();
            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();

            Extracto extractoTmp = configExtractoBoTmp.Obtener(idHotel);

            this.txtPieExtracto.Text = extractoTmp.PieExtracto;
            this.imgFirma.ImageUrl = extractoTmp.FirmaLogo;
            this.pbLogo.ImageUrl = propietarioTmp.RutaLogo;
            this.lblNombreHotel.Text = propietarioTmp.NombreHotel.ToUpper();            
            this.lblNit.Text = propietarioTmp.Nit;
            
            DataTable dtCalculos = configCertificadoBoTmp.CalculoConceptos(propietarioTmp.IdPropietario, idHotel, fecha);

            xrTableConceptos.Rows.Clear();
            XRTableCell celda = null;
            XRTableRow fila = null;
            double total = 0;

            foreach (DataRow itemCalculo in dtCalculos.Rows)
            {
                fila = new XRTableRow();
                fila.BorderColor = Color.White;

                celda = new XRTableCell();
                celda.Text = itemCalculo["Concepto"].ToString().Replace("_", " ");
                celda.TextAlignment = TextAlignment.MiddleLeft;
                fila.Cells.Add(celda);

                celda = new XRTableCell();
                celda.Text = "$ " + ((double)itemCalculo["Valor"]).ToString("N0");
                celda.TextAlignment = TextAlignment.MiddleRight;
                fila.Cells.Add(celda);

                total += ((double)itemCalculo["Valor"]);

                xrTableConceptos.Rows.Add(fila);
            }

            Numalet numaLetTmp = new Numalet();
            numaLetTmp.LetraCapital = true;
            numaLetTmp.Decimales = 0;
            string valorLetras = numaLetTmp.ToCustomString(total);

            this.lblInfo.Text = "Que durante el año gravable " + fecha.Year + " " + propietarioTmp.Nombre + " identificado(a) con " + ((propietarioTmp.TipoPersona.ToUpper() == "NATURAL") ? "CC" : "NIT") + " " + propietarioTmp.NumIdentificacion + ". Recibo de Nuestra Compañía la suma de : $ " + total.ToString("N0") + "  (" + valorLetras + ") por concepto de: " + configCertificadoBoTmp.ObtenerTexto(idHotel);
            lblExpide.Text = "Expide en la ciudad de " + propietarioTmp.NombreCiudad + " a los " + DateTime.Now.Day + " del mes de " + meses[DateTime.Now.Month - 1] + " del año" + DateTime.Now.Year;
        }
    }
}
