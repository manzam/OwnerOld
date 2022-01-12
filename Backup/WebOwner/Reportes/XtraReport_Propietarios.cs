using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BO;
using DM;

namespace WebOwner.Reportes
{
    public partial class XtraReport_Propietarios : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_Propietarios(int idHotel, string nombreHotel)
        {
            InitializeComponent();

            VariableBo variableBoTmp = new VariableBo();
            List<ObjetoGenerico> listaVariable = variableBoTmp.VerTodos("P", idHotel);

            xrTableTitulos.Rows.Clear();

            XRTableRow miFila = new XRTableRow();
            XRTableCell miCelda = new XRTableCell();

            miCelda.Text = "Propietario".ToUpper();
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font("Times New Roman", 14);
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "Hotel".ToUpper();
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font("Times New Roman", 14);
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "Num. Suite".ToUpper();
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font("Times New Roman", 14);
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miFila.Cells.Add(miCelda);

            miCelda = new XRTableCell();
            miCelda.Text = "Num. Escritura".ToUpper();
            miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            miCelda.ForeColor = Color.White;
            miCelda.Font = new Font("Times New Roman", 14);
            miCelda.BackColor = Color.FromArgb(117, 153, 169);
            miFila.Cells.Add(miCelda);

            foreach (ObjetoGenerico itemVariable in listaVariable)
            {
                miCelda = new XRTableCell();
                miCelda.Text = itemVariable.Nombre.ToUpper().Replace('_', ' ');
                miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                miCelda.ForeColor = Color.White;
                miCelda.Font = new Font("Times New Roman", 14);
                miCelda.BackColor = Color.FromArgb(117, 153, 169);
                miFila.Cells.Add(miCelda);
            }

            xrTableTitulos.Rows.Add(miFila);

            PropietarioBo propietarioBoTmp = new PropietarioBo();
            List<ObjetoGenerico> listaPropietario = propietarioBoTmp.ListaPropietario(idHotel);

            string numSuiteTmp = "";
            bool esNuevo = false;

            foreach (ObjetoGenerico itemPropietario in listaPropietario)
            {
                if (numSuiteTmp != itemPropietario.NumSuit)
                {
                    numSuiteTmp = itemPropietario.NumSuit;

                    if (esNuevo)
                        xrTableTitulos.Rows.Add(miFila);

                    esNuevo = false;
                    miFila = new XRTableRow();

                    miCelda = new XRTableCell();
                    miCelda.Text = itemPropietario.PrimeroNombre + " " + itemPropietario.SegundoNombre + " " + itemPropietario.PrimerApellido + " " + itemPropietario.SegundoApellido;
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    miFila.Cells.Add(miCelda);

                    miCelda = new XRTableCell();
                    miCelda.Text = itemPropietario.NombreHotel;
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    miFila.Cells.Add(miCelda);

                    miCelda = new XRTableCell();
                    miCelda.Text = itemPropietario.NumSuit;
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    miFila.Cells.Add(miCelda);

                    miCelda = new XRTableCell();
                    miCelda.Text = itemPropietario.NumEscritura;
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    miFila.Cells.Add(miCelda);

                    miCelda = new XRTableCell();
                    miCelda.Text = itemPropietario.Valor.ToString("N4");
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    miFila.Cells.Add(miCelda);
                }
                else
                {
                    esNuevo = true;

                    miCelda = new XRTableCell();
                    miCelda.Text = itemPropietario.Valor.ToString("N4");
                    miCelda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    miFila.Cells.Add(miCelda);
                }
            }
        }

    }
}
