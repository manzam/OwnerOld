using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using DM;
using BO;
using System.Linq;
using System.Data;

namespace WebOwner.Reportes
{
    public partial class XtraReport_LiquidacionPropietario : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_LiquidacionPropietario(DateTime periodoDesde, DateTime periodoHasta, string nombreTitulo, List<ObjetoGenerico> listaLiquidacion, int numVariablesPropietarios)
        {
            InitializeComponent();

            lblTituloLiquidacion.Text = (nombreTitulo + "   Periodo [ 1-" + periodoDesde.Month + "-" + periodoDesde.Year + " * 30-" + periodoHasta.Month + "-" + periodoHasta.Year + " ]").ToUpper();

            //var listaConceptos = (from L in listaLiquidacion
            //                      group new { L.Concepto.IdConcepto, L.Concepto.Nombre }
            //                      by new { L.Concepto.IdConcepto, L.Concepto.Nombre } into miGrupo
            //                      select new { Nombre = miGrupo.Key.Nombre });

            xrTableLiquidacion.Rows.Clear();
            xrTableLiquidacion.BeginInit();

            XRTableRow miFila = null;
            XRTableCell miCelda = null;

            miFila = new XRTableRow();

            miCelda = new XRTableCell();
            miCelda.Text = "Nombre Propietario".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 150;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "Identificacion".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 150; // 130;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "N° Suit".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 150; // 70;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "N° Escritura".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 150; // 70;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            List<int?> listaConceptos = listaLiquidacion.Select(C => C.IdConcepto).Distinct().ToList();
            int numConceptos = listaConceptos.Count;

            short numModulo = 0;

            foreach (int? idConcepto in listaConceptos)
            {
                miCelda = new XRTableCell();
                ObjetoGenerico itemConcepto = listaLiquidacion.Where(C => C.IdConcepto == idConcepto).FirstOrDefault();
                miCelda.Text = itemConcepto.NombreConcepto.Replace('_', ' ').ToUpper();
                miCelda.BackColor = Color.FromArgb(117, 153, 169);
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                miCelda.Padding = 2;
                miCelda.ForeColor = Color.White;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 150;// ancho;
                miFila.Cells.Add(miCelda);

                numModulo++;
            }
            xrTableLiquidacion.Rows.Add(miFila);

            int numFila = 0;
            for (int i = 0; i < listaLiquidacion.Count - 1; i = i + numConceptos)
            {
                miFila = new XRTableRow();

                miCelda = new XRTableCell();
                string nombre = listaLiquidacion[i].PrimeroNombre + " " + listaLiquidacion[i].SegundoNombre + " " + listaLiquidacion[i].PrimerApellido + " " + listaLiquidacion[i].SegundoApellido;
                miCelda.Text = nombre;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 150;
                miFila.Cells.Add(miCelda);

                miCelda = new XRTableCell();
                miCelda.Text = listaLiquidacion[i].NumIdentificacion;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 150; //130;
                miFila.Cells.Add(miCelda);

                miCelda = new XRTableCell();
                miCelda.Text = listaLiquidacion[i].NumSuit;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 150; // 70;
                miFila.Cells.Add(miCelda);

                miCelda = new XRTableCell();
                miCelda.Text = listaLiquidacion[i].NumEscritura;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 150; // 70;
                miFila.Cells.Add(miCelda);

                short conVar = 1;
                for (int k = 0; k < numConceptos; k++)
                {
                    miCelda = new XRTableCell();

                    if (conVar <= numVariablesPropietarios)
                        miCelda.Text = listaLiquidacion[i + k].Valor.ToString("N7");
                    else
                        miCelda.Text = listaLiquidacion[i + k].Valor.ToString("N0");

                    //miCelda.Summary.FormatString = "N";
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    miCelda.Padding = 2;
                    miCelda.Font = new Font(new FontFamily("Arial"), 7);
                    miCelda.WidthF = 150; // ancho;
                    miFila.Cells.Add(miCelda);

                    conVar++;
                }

                xrTableLiquidacion.Rows.Add(miFila);
                numFila++;
                if (numFila == 230)
                {
                    //break;
                }
            }

            // Sumatorias por columna

            miFila = new XRTableRow();
            miCelda = new XRTableCell();

            miCelda.Text = "Total";
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            miCelda.Padding = 2;
            miCelda.Font = new Font(new FontFamily("Arial"), 8, FontStyle.Bold);
            miCelda.WidthF = 600;
            miFila.Cells.Add(miCelda);            

            for (int i = 4; i < xrTableLiquidacion.Rows[0].Cells.Count; i++)
            {
                double suma = 0;

                for (int j = 1; j < xrTableLiquidacion.Rows.Count; j++)
                {
                    suma = suma + double.Parse(xrTableLiquidacion.Rows[j].Cells[i].Text);
                }

                miCelda = new XRTableCell();
                miCelda.Text = suma.ToString();
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 8, FontStyle.Bold);
                miCelda.WidthF = 150; // ancho;
                miFila.Cells.Add(miCelda);
            }

            xrTableLiquidacion.Rows.Add(miFila);
            xrTableLiquidacion.EndInit();

        }

        public XtraReport_LiquidacionPropietario(DateTime periodo, string nombreTitulo, int idHotel, int idPropietario, int idSuite, int numVariablesPropietarios)
        {
            InitializeComponent();

            LiquidacionBo liquidacionBoTmp = new LiquidacionBo();
            List<Liquidacion> listaLiquidacion = liquidacionBoTmp.ReporteLiquidacionPropietario(idHotel, periodo, idPropietario, idSuite);
            
            lblTituloLiquidacion.Text = (nombreTitulo + "   Periodo " + periodo.Month + "-" + periodo.Year).ToUpper();

            var listaConceptos = (from L in listaLiquidacion
                                  group new { L.Concepto.IdConcepto, L.Concepto.Nombre }
                                  by new { L.Concepto.IdConcepto, L.Concepto.Nombre } into miGrupo
                                  select miGrupo);

            xrTableLiquidacion.Rows.Clear();

            XRTableRow miFila = null;
            XRTableCell miCelda = null;

            miFila = new XRTableRow();

            miCelda = new XRTableCell();
            miCelda.Text = "Nombre Propietario".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 150;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "Identificacion".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 130;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "N° Suit".ToUpper();
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.Padding = 2;
            miCelda.WidthF = 70;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font(new FontFamily("Arial"), 7);
            miFila.Cells.Add(miCelda);

            int numConceptos = listaConceptos.Count();
            float ancho = (xrTableLiquidacion.WidthF - 350) / numConceptos;
            short numModulo = 0;

            foreach (var itemConcepto in listaConceptos)
            {
                miCelda = new XRTableCell();
                miCelda.Text = itemConcepto.Key.Nombre.Replace('_', ' ').ToUpper();
                miCelda.BackColor = Color.FromArgb(117, 153, 169);
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                miCelda.Padding = 2;
                miCelda.ForeColor = Color.White;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = ancho;
                miFila.Cells.Add(miCelda);

                numModulo++;
            }
            xrTableLiquidacion.Rows.Add(miFila);

            //short contadorGrupo = 0;

            for (int i = 0; i < listaLiquidacion.Count; i = i + numConceptos)
            {
                miFila = new XRTableRow();

                miCelda = new XRTableCell();
                string nombre = listaLiquidacion[i].Propietario.NombrePrimero + " " + listaLiquidacion[i].Propietario.NombreSegundo + " " + listaLiquidacion[i].Propietario.ApellidoPrimero + " " + listaLiquidacion[i].Propietario.ApellidoSegundo;
                miCelda.Text = nombre;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 150;
                miFila.Cells.Add(miCelda);

                miCelda = new XRTableCell();
                miCelda.Text = listaLiquidacion[i].Propietario.NumIdentificacion;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 130;
                miFila.Cells.Add(miCelda);

                miCelda = new XRTableCell();
                miCelda.Text = listaLiquidacion[i].Suit.NumSuit;
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                miCelda.Padding = 2;
                miCelda.Font = new Font(new FontFamily("Arial"), 7);
                miCelda.WidthF = 70;
                miFila.Cells.Add(miCelda);

                short conVar = 1;
                for (int k = 0; k < numConceptos; k++)
                {
                    miCelda = new XRTableCell();

                    if (conVar <= numVariablesPropietarios)
                        miCelda.Text = listaLiquidacion[i + k].Valor.ToString("N10");
                    else
                        miCelda.Text = listaLiquidacion[i + k].Valor.ToString("N0");

                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    miCelda.Padding = 2;
                    miCelda.Font = new Font(new FontFamily("Arial"), 7);
                    miCelda.WidthF = ancho;
                    miFila.Cells.Add(miCelda);
                }

                xrTableLiquidacion.Rows.Add(miFila);
            }
        }
    }
}
