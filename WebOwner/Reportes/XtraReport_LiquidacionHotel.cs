using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DM;
using System.Collections.Generic;

namespace WebOwner.Reportes
{
    public partial class XtraReport_LiquidacionHotel : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_LiquidacionHotel(DateTime periodo, string nombreHotel, List<Liquidacion> listaLiquidacion)
        {
            InitializeComponent();

            lblTituloLiquidacion.Text = ("Liquidacion Conceptos " + nombreHotel + " Periodo " + periodo.Month + "-" + periodo.Year).ToUpper();

            foreach (Liquidacion itemLiquidacion in listaLiquidacion)
            {
                XRTableRow miFila = new XRTableRow();
                miFila.Borders = DevExpress.XtraPrinting.BorderSide.All;
                miFila.Padding = 2;

                XRTableCell miCeldaNombre = new XRTableCell();
                miCeldaNombre.Text = itemLiquidacion.Concepto.Nombre;
                miCeldaNombre.WidthF = 546;
                miCeldaNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                miFila.Cells.Add(miCeldaNombre);

                XRTableCell miCeldaValor = new XRTableCell();
                miCeldaValor.Text = itemLiquidacion.Valor.ToString("N");
                miCeldaValor.WidthF = 200;
                miCeldaValor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                miFila.Cells.Add(miCeldaValor);

                xrTableLiquidacion.Rows.Add(miFila);
            }
        }

    }
}
