using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Servicios;
using System.Linq;
using System.Windows.Forms;
using DM;
using System.Collections.Generic;
using BO;

namespace WebOwner.Reportes
{
    public partial class XtraReport_ReporteUno : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_ReporteUno(string condicion, string nombreHotel)
        {
            InitializeComponent();

            this.lblHoteles.Text = nombreHotel;

            string sql = "SELECT " +
                         "Suit_Propietario.IdSuitPropietario, " + 
                         "Propietario.FechaIngreso, " +
                         "(CASE Propietario.Activo WHEN 1 THEN 'Si' ELSE 'No' END) as 'Estado Propietario', " +
                         "Propietario.NombrePrimero + ' ' + Propietario.NombreSegundo + ' ' + Propietario.ApellidoPrimero + ' ' + Propietario.ApellidoSegundo AS Propietario, " +
                         "Propietario.TipoDocumento, " +
                         "Propietario.NumIdentificacion, " +
                         "Propietario.TipoPersona, " +
                         "(CASE Suit.Activo WHEN 1 THEN 'Si' ELSE 'No' END) as 'Estado Suite', " +
                         "Suit.NumSuit, " +
                         "Suit.NumEscritura, " +
                         "Suit.RegistroNotaria, " +
                         "Propietario.Correo, " +
                         "Propietario.Direccion, " +
                         "Propietario.Telefono_1, " +
                         "Propietario.Telefono_2, " +
                         "C1.Nombre AS CiudadPropietario, " +
                         "Banco.Nombre, " +
                         "Suit_Propietario.NumCuenta, " +
                         "Suit_Propietario.TipoCuenta " +
                         "FROM Propietario " +
                         "INNER JOIN Suit_Propietario ON Propietario.IdPropietario = Suit_Propietario.IdPropietario " +
                         "INNER JOIN Suit ON Suit_Propietario.IdSuit = Suit.IdSuit " +
                         "INNER JOIN Hotel ON Suit.IdHotel = Hotel.IdHotel " +
                         "INNER JOIN Ciudad AS C1 ON Propietario.IdCiudad = C1.IdCiudad " +
                         "INNER JOIN Banco ON Suit_Propietario.IdBanco = Banco.IdBanco" +
                         condicion +
                         " order by Propietario.NombrePrimero";

            DataTable tabla = Utilities.Select(sql, "reporteUno");
            List<ObjetoGenerico> listaVariable = null;

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int idSuitePropietario = (int)tabla.Rows[0]["IdSuitPropietario"];

                listaVariable = (from VVS in Contexto.Valor_Variable_Suit
                                 join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                                 where V.Activo == true &&
                                       VVS.Suit_Propietario.IdSuitPropietario == idSuitePropietario
                                 select new ObjetoGenerico()
                                 {
                                     IdSuitPropietario = VVS.Suit_Propietario.IdSuitPropietario,
                                     Nombre = V.Nombre,
                                     Valor = VVS.Valor
                                 }).ToList();

                // Nuevas columnas de las variables
                foreach (ObjetoGenerico itemVariable in listaVariable)
                {
                    tabla.Columns.Add(itemVariable.Nombre, typeof(double));
                }

                foreach (DataRow itemFila in tabla.Rows)
                {
                    idSuitePropietario = (int)itemFila["IdSuitPropietario"];

                    listaVariable = (from VVS in Contexto.Valor_Variable_Suit
                                      join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                                      where V.Activo == true &&
                                            VVS.Suit_Propietario.IdSuitPropietario == idSuitePropietario
                                      select new ObjetoGenerico()
                                      {
                                          IdSuitPropietario = VVS.Suit_Propietario.IdSuitPropietario,
                                          Nombre = V.Nombre,
                                          Valor = VVS.Valor
                                      }).ToList();

                    foreach (ObjetoGenerico itemVariable in listaVariable)
                    {
                        try
                        {
                            itemFila[itemVariable.Nombre] = itemVariable.Valor;
                        }
                        catch (Exception ex)
                        {
                        }                        
                    }
                }
            }

            // Pinto las columnas en el informe

            float wVariables = 1070 / listaVariable.Count;

            tblNombreTitulos.Rows[0].Cells.Clear();
            float[] anchos = { 100, 100, 300, 130, 135, 100, 100, 80, 130, 125, 250, 250, 100, 100, 150, 200, 150 };

            XRTableRow fila = new XRTableRow();
            fila.Font = new Font("Arial", 8);

            for (int i = 1; i < tabla.Columns.Count; i++)
            {
                XRTableCell celda = new XRTableCell();
                celda.Text = tabla.Columns[i].ToString().ToUpper();

                if ((i - 1) >= anchos.Length)
                    celda.WidthF = wVariables;
                else
                    celda.WidthF = anchos[i - 1];

                celda.BorderColor = Color.White;
                celda.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
                celda.Borders = DevExpress.XtraPrinting.BorderSide.Right;

                fila.Cells.Add(celda);
            }
            tblNombreTitulos.Rows.Add(fila);

            // Pinto los datos
            tblDatos.Rows[0].Cells.Clear();
            tblDatos.Rows.Clear();
            tblDatos.BeginInit();

            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                fila = new XRTableRow();

                for (int c = 1; c < tabla.Columns.Count; c++)
                {
                    XRTableCell celda = new XRTableCell();
                    celda.Text = tabla.Rows[i][c].ToString();

                    if ((c - 1) >= anchos.Length)
                        celda.WidthF = wVariables;
                    else
                        celda.WidthF = anchos[c - 1];

                    celda.BorderColor = Color.Black;
                    celda.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
                    celda.Borders = DevExpress.XtraPrinting.BorderSide.Right;

                    fila.Cells.Add(celda);
                }
                tblDatos.Rows.Add(fila);                
            }
            tblDatos.EndInit();
        }
    }
}
