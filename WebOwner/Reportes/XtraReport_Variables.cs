using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BO;
using DM;
using System.Collections.Generic;

namespace WebOwner.Reportes
{
    public partial class XtraReport_Variables : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_Variables(int idHotel, string nombreHotel, DateTime fecha)
        {
            InitializeComponent();

            xrTitulo.Text = nombreHotel;

            ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
            List<ObjetoGenerico> listaVariablesHotel = valorVariableBoTmp.ObtenerVariableValorPorHotel(idHotel, "H", fecha);
            List<ObjetoGenerico> listaVariablesPropietario = valorVariableBoTmp.ObtenerVariableValorPorPropietario(idHotel);

            xrTableVariablesHotel.Rows.Clear();
            xrTablaVariablePropietario.Rows.Clear();

            XRTableRow filaTmp = null;
            XRTableCell celdaTmp = null;

            foreach (ObjetoGenerico itemVariable in listaVariablesHotel)
            {
                filaTmp = new XRTableRow();

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.NombreVariable;
                celdaTmp.WidthF = 278.43F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                filaTmp.Cells.Add(celdaTmp);

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.Valor.ToString();
                celdaTmp.WidthF = 37.5F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                filaTmp.Cells.Add(celdaTmp);

                xrTableVariablesHotel.Rows.Add(filaTmp);
            }

            foreach (ObjetoGenerico itemVariable in listaVariablesPropietario)
            {
                filaTmp = new XRTableRow();

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.Nombre;
                celdaTmp.WidthF = 347.18F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                filaTmp.Cells.Add(celdaTmp);

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.NumSuit;
                celdaTmp.WidthF = 101.78F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                filaTmp.Cells.Add(celdaTmp);

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.NumEscritura;
                celdaTmp.WidthF = 100.62F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                filaTmp.Cells.Add(celdaTmp);

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.NombreVariable;
                celdaTmp.WidthF = 298.59F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                filaTmp.Cells.Add(celdaTmp);

                celdaTmp = new XRTableCell();
                celdaTmp.Text = itemVariable.Valor.ToString();
                celdaTmp.WidthF = 229.84F;
                celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                filaTmp.Cells.Add(celdaTmp);

                xrTablaVariablePropietario.Rows.Add(filaTmp);
            }
        }

    }
}
