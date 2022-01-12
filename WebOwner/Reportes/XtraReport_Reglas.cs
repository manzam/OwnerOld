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
    public partial class XtraReport_Reglas : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_Reglas(int idHotel, string nombreHotel)
        {
            InitializeComponent();

            xrTitulo.Text = nombreHotel;

            ConceptoBo conceptoBoTmp = new ConceptoBo();
            List<Concepto> listaConceptoTmp = conceptoBoTmp.VerTodos(idHotel);
            List<ObjetoGenerico> listaConcepto = new List<ObjetoGenerico>();

            foreach (Concepto itemConcepto in listaConceptoTmp)
            {
                ObjetoGenerico concepto = new ObjetoGenerico();
                concepto.IdConcepto = itemConcepto.IdConcepto;
                concepto.Nombre = itemConcepto.Nombre;
                concepto.Regla = conceptoBoTmp.ObtenerRegla(itemConcepto.IdConcepto);
                listaConcepto.Add(concepto);
            }

            xrTableReglas.Rows.Clear();
            foreach (ObjetoGenerico itemConcepto in listaConcepto)
            {
                XRTableRow nuevaFila = new XRTableRow();
                nuevaFila.HeightF = float.Parse("40");
                nuevaFila.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                nuevaFila.BorderColor = Color.FromArgb(117, 153, 169);

                XRTableCell nuevaCeldaNombre = new XRTableCell();
                nuevaCeldaNombre.Text = itemConcepto.Nombre;
                nuevaCeldaNombre.WidthF = float.Parse("405.82");
                nuevaCeldaNombre.HeightF = float.Parse("40");
                nuevaCeldaNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

                XRTableCell nuevaCeldaRegla = new XRTableCell();
                nuevaCeldaRegla.Text = itemConcepto.Regla;
                nuevaCeldaRegla.WidthF = float.Parse("672.18");
                nuevaCeldaRegla.HeightF = float.Parse("40");
                nuevaCeldaRegla.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

                nuevaFila.Cells.Add(nuevaCeldaNombre);
                nuevaFila.Cells.Add(nuevaCeldaRegla);

                xrTableReglas.Rows.Add(nuevaFila);
            }
        }
    }
}
