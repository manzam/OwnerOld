using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Servicios;
using System.Data;

namespace WebOwner.Reportes
{
    public partial class XtraReport_ReporteConsolidadoPorPropietario : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_ReporteConsolidadoPorPropietario(int idPropietario, DateTime fechaDesde, DateTime fechaHasta)
        {
            InitializeComponent();

            XRTableRow filaTmp = null;
            XRTableCell celdaTmp = null;

            DateTime fechaDesdeTmp = new DateTime(fechaDesde.Year, fechaDesde.Month, 1);

            string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            string sqlDatosPropietaris = "SELECT " +
                                         "(Propietario.NombrePrimero +' '+ Propietario.NombreSegundo +' '+ Propietario.ApellidoPrimero +' '+ Propietario.ApellidoSegundo) AS Propietario, " +
                                         "Propietario.TipoPersona, " +
                                         "Propietario.TipoDocumento, " +
                                         "Propietario.NumIdentificacion, " +
                                         "Propietario.IdPropietario, " +
                                         "Hotel.Nombre AS 'Nombre Hotel', " +
                                         "Suit.NumSuit, " +
                                         "'Participacion Copropietario' AS 'Participacion Copropietario', " +
                                         "'ddlCoeficiente' AS 'Coeficiente', " +
                                         "Suit_Propietario.IdSuitPropietario, " +
                                         "Suit.IdHotel, " +
                                         "Suit.IdSuit " +                                         
                                         "FROM Propietario " +
                                         "INNER JOIN Suit_Propietario ON Propietario.IdPropietario = Suit_Propietario.IdPropietario " +
                                         "INNER JOIN Suit ON Suit_Propietario.IdSuit = Suit.IdSuit " +
                                         "INNER JOIN Hotel ON Suit.IdHotel = Hotel.IdHotel " +
                                         "WHERE Propietario.IdPropietario = " + idPropietario;
            DataTable tablaDatos = Utilities.Select(sqlDatosPropietaris, "reporteConsolidadoPorSuite");


            foreach (DataRow itemSuite in tablaDatos.Rows)
            {

                string sqlParticipacion = "SELECT (Valor_Variable_Suit.Valor * 100) as Valor " +
                                          "FROM Detalle_Extracto " +
                                          "INNER JOIN Variable ON Detalle_Extracto.IdVariable = Variable.IdVariable " +
                                          "INNER JOIN Valor_Variable_Suit ON Variable.IdVariable = Valor_Variable_Suit.IdVariable " +
                                          "WHERE (Detalle_Extracto.IdControl = 'ddlParticipacionPropietario') AND (Variable.IdHotel = " + itemSuite["IdHotel"].ToString() + ") AND (Valor_Variable_Suit.IdSuitPropietario = " + itemSuite["IdSuitPropietario"].ToString() + ")";
                DataTable tablaParticipacion = Utilities.Select(sqlParticipacion, "Participacion");
                itemSuite["Participacion Copropietario"] = tablaParticipacion.Rows[0]["Valor"];

                string sqlCoeficiente = "SELECT (Valor_Variable_Suit.Valor * 100) as Valor " +
                                        "FROM Detalle_Extracto " +
                                        "INNER JOIN Variable ON Detalle_Extracto.IdVariable = Variable.IdVariable " +
                                        "INNER JOIN Valor_Variable_Suit ON Variable.IdVariable = Valor_Variable_Suit.IdVariable " +
                                        "WHERE (Detalle_Extracto.IdControl = 'ddlCoeficiente') AND (Variable.IdHotel = " + itemSuite["IdHotel"].ToString() + ") AND (Valor_Variable_Suit.IdSuitPropietario = " + itemSuite["IdSuitPropietario"].ToString() + ")";
                DataTable tablaCoeficiente = Utilities.Select(sqlCoeficiente, "Coeficiente");
                itemSuite["Coeficiente"] = tablaCoeficiente.Rows[0]["Valor"];
                

                string sqlGrupos = "SELECT IdGrupo,Nombre FROM Reporte_Grupo order by Orden";
                DataTable tablaGrupos = Utilities.Select(sqlGrupos, "grupos");
                
                foreach (DataRow itemGrupo in tablaGrupos.Rows)
                {
                    if (!tablaDatos.Columns.Contains(itemGrupo["Nombre"].ToString()))
                    {
                        tablaDatos.Columns.Add(new DataColumn(itemGrupo["Nombre"].ToString(), typeof(double)));
                        tablaDatos.AcceptChanges();
                    }

                    string sqlIngresos = "SELECT SUM(Valor) AS Valor FROM ( " +
                                         "SELECT Valor " +
                                         "FROM Valor_Variable " +
                                         "WHERE " +
                                         "(MONTH(Valor_Variable.Fecha) >= " + fechaDesde.Month + " AND " +
                                         "YEAR(Valor_Variable.Fecha) >= " + fechaDesde.Year + ") AND " +
                                         "(MONTH(Valor_Variable.Fecha) <= " + fechaHasta.Month + " AND " +
                                         "YEAR(Valor_Variable.Fecha) <= " + fechaHasta.Year + ") AND " +
                                         "Valor_Variable.IdVariable IN (SELECT " +
                                         "Reporte_Grupo_Detalle.IdVariable " +
                                         "FROM Reporte_Grupo_Detalle " +
                                         "INNER JOIN Variable ON Reporte_Grupo_Detalle.IdVariable = Variable.IdVariable " +
                                         "WHERE Reporte_Grupo_Detalle.IdGrupo = " + itemGrupo["IdGrupo"].ToString() + " AND Reporte_Grupo_Detalle.Tipo = 'H' AND Reporte_Grupo_Detalle.IdHotel = " + itemSuite["IdHotel"].ToString() + ") " +
                                         "UNION " +
                                         "SELECT Valor " +
                                         "FROM Valor_Variable_Suit " +
                                         "INNER JOIN Suit_Propietario ON Valor_Variable_Suit.IdSuitPropietario = Suit_Propietario.IdSuitPropietario " +
                                         "WHERE " +
                                         "Suit_Propietario.IdSuitPropietario = " + itemSuite["IdSuitPropietario"].ToString() + " AND " +
                                         "Valor_Variable_Suit.IdVariable IN (SELECT " +
                                         "Reporte_Grupo_Detalle.IdVariable " +
                                         "FROM Reporte_Grupo_Detalle " +
                                         "INNER JOIN Variable ON Reporte_Grupo_Detalle.IdVariable = Variable.IdVariable " +
                                         "WHERE Reporte_Grupo_Detalle.IdGrupo = " + itemGrupo["IdGrupo"].ToString() + " AND Reporte_Grupo_Detalle.Tipo = 'P' AND Reporte_Grupo_Detalle.IdHotel = " + itemSuite["IdHotel"].ToString() + ") " +
                                         "UNION " +
                                         "SELECT Valor " +
                                         "FROM Liquidacion " +
                                         "WHERE " +
                                         "Liquidacion.IdSuit = " + itemSuite["IdSuit"].ToString() + " AND " +
                                         "(MONTH(Liquidacion.FechaPeriodoLiquidado) >= " + fechaDesde.Month + " AND " +
                                         "YEAR(Liquidacion.FechaPeriodoLiquidado) >= " + fechaDesde.Year + ") AND " +
                                         "(MONTH(Liquidacion.FechaPeriodoLiquidado) <= " + fechaHasta.Month + " AND " +
                                         "YEAR(Liquidacion.FechaPeriodoLiquidado) <= " + fechaHasta.Year + ") AND " +
                                         "Liquidacion.IdConcepto IN (SELECT " +
                                         "Reporte_Grupo_Detalle.IdConcepto " +
                                         "FROM Reporte_Grupo_Detalle " +
                                         "INNER JOIN Concepto ON Reporte_Grupo_Detalle.IdConcepto = Concepto.IdConcepto " +
                                         "WHERE Reporte_Grupo_Detalle.IdGrupo = " + itemGrupo["IdGrupo"].ToString() + " AND Reporte_Grupo_Detalle.Tipo = 'C' AND Reporte_Grupo_Detalle.IdHotel = " + itemSuite["IdHotel"].ToString() + ")) " +
                                         "as T";
                    DataTable tablaIngresos = Utilities.Select(sqlIngresos, "sumatorias");
                    itemSuite[itemGrupo["Nombre"].ToString()] = tablaIngresos.Rows[0]["Valor"];
                }
            }            

            tablaDatos.Columns.Remove("IdPropietario");
            tablaDatos.Columns.Remove("IdSuitPropietario");
            tablaDatos.Columns.Remove("IdSuit");
            tablaDatos.Columns.Remove("IdHotel");
            
            tablaDatos.AcceptChanges();

            DataView viewDatos = new DataView(tablaDatos);
            viewDatos.Sort = "NumSuit desc";
            tablaDatos = viewDatos.ToTable();


            // Llenado de los datos
            tblNombreTitulos.Rows.Clear();
            float ancho = 1974 / tablaDatos.Columns.Count;

            filaTmp = new XRTableRow();
            foreach (DataColumn itemColumna in tablaDatos.Columns)
            {
                celdaTmp = new XRTableCell();
                celdaTmp.WidthF = ancho;
                celdaTmp.Text = itemColumna.ColumnName.ToUpper();

                filaTmp.Cells.Add(celdaTmp);
            }
            tblNombreTitulos.Rows.Add(filaTmp);

            // Llenado del cuerpo
            tblDatos.Rows.Clear();
            foreach (DataRow itemFila in tablaDatos.Rows)
            {
                filaTmp = new XRTableRow();
                for (int i = 0; i < tablaDatos.Columns.Count; i++)
                {
                    celdaTmp = new XRTableCell();
                    celdaTmp.WidthF = ancho;
                    celdaTmp.Text = itemFila[i].ToString();

                    filaTmp.Cells.Add(celdaTmp);
                }
                tblDatos.Rows.Add(filaTmp);
            }

            //// Sumatorias
            //filaTmp = new XRTableRow();
            //filaTmp.BackColor = Color.FromArgb(117, 153, 169);
            //filaTmp.ForeColor = Color.White;

            //celdaTmp = new XRTableCell();
            //celdaTmp.WidthF = ancho * 7;
            //celdaTmp.Text = "Total";
            //celdaTmp.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
            //celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //filaTmp.Cells.Add(celdaTmp);

            //celdaTmp = new XRTableCell();
            //celdaTmp.WidthF = ancho;
            //celdaTmp.Text = (valParticipacion * 100).ToString("R");
            ////celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //filaTmp.Cells.Add(celdaTmp);

            //celdaTmp = new XRTableCell();
            //celdaTmp.WidthF = ancho;
            //celdaTmp.Text = valIngresos.ToString("R");
            ////celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //filaTmp.Cells.Add(celdaTmp);

            //celdaTmp = new XRTableCell();
            //celdaTmp.WidthF = ancho;
            //celdaTmp.Text = valDeducciones.ToString("R");
            ////celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //filaTmp.Cells.Add(celdaTmp);

            //celdaTmp = new XRTableCell();
            //celdaTmp.WidthF = ancho;
            //celdaTmp.Text = valTotales.ToString("R");
            ////celdaTmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //filaTmp.Cells.Add(celdaTmp);

            //tblDatos.Rows.Add(filaTmp);
        }

    }
}
