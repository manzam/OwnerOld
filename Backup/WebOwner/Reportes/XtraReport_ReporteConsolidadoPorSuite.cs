using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Servicios;
using System.Data;
using System.Collections.Generic;
using DM;
using BO;

namespace WebOwner.Reportes
{
    public partial class XtraReport_ReporteConsolidadoPorSuite : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_ReporteConsolidadoPorSuite(int idHotel, int idSuite, int idReporte, DateTime fechaDesde, DateTime fechaHasta )
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
                                         "Suit.IdSuit, " +
                                         "Propietario.IdPropietario, 'MES' as MES " +
                                         "FROM Propietario " +
                                         "INNER JOIN Suit_Propietario ON Propietario.IdPropietario = Suit_Propietario.IdPropietario " +
                                         "INNER JOIN Suit ON Suit_Propietario.IdSuit = Suit.IdSuit " +
                                         "WHERE Suit.IdHotel = " + idHotel + " AND Suit.IdSuit = " + idSuite + "  ORDER BY Suit_Propietario.IdPropietario";

            DataTable tablaDatos = Utilities.Select(sqlDatosPropietaris, "reporteConsolidadoPorSuite");


            bool entraVP = true, entraVH = true, entraVC = true;
            int p = tablaDatos.Rows.Count;

            for (int pp = 0; pp < p; pp++)
            {
                tablaDatos.Rows[pp]["MES"] = meses[fechaDesdeTmp.Month - 1];

                do
                {
                    #region Obteniendo datos de tipo propietario
                    string sqlDatosVariablePropietario = "SELECT " +
                                                         "Variable.Nombre, " +
                                                         "Valor_Variable_Suit.Valor, " +
                                                         "Suit_Propietario.IdSuit AS IdSuiteP, " +
                                                         "Variable.IdVariable AS IdVariableP, " +
                                                         "Suit_Propietario.IdPropietario AS IdPropietarioP " +
                                                         "FROM Valor_Variable_Suit " +
                                                         "INNER JOIN Variable ON Valor_Variable_Suit.IdVariable = Variable.IdVariable " +
                                                         "INNER JOIN Suit_Propietario ON Valor_Variable_Suit.IdSuitPropietario = Suit_Propietario.IdSuitPropietario " +
                                                         "WHERE (Suit_Propietario.IdSuit = " + idSuite + ") AND " +
                                                         "(Variable.Activo = 1) AND " +
                                                         "(Suit_Propietario.IdPropietario = " + tablaDatos.Rows[pp]["IdPropietario"].ToString() + ") AND " +
                                                         "(Variable.IdHotel = " + idHotel + ") AND " +
                                                         "(Variable.IdVariable IN " +
                                                         "(SELECT IdVariable " +
                                                         "FROM  Reporte_Detalle " +
                                                         "WHERE (IdReporte = " + idReporte + ") AND (Tipo = 'P') AND (IdHotel = " + idHotel + "))) ORDER BY Suit_Propietario.IdPropietario";
                    DataTable tablaDatosVariableP = Utilities.Select(sqlDatosVariablePropietario, "reporteConsolidadoPorSuiteP");

                    // Nuevas columnas 
                    if (entraVP)
                    {
                        DataView view = new DataView(tablaDatosVariableP);
                        DataTable tablaVariables = view.ToTable(true, "Nombre");

                        foreach (DataRow itemFilaVariable in tablaVariables.Rows)
                        {
                            DataColumn nuevaColumna = new DataColumn();
                            nuevaColumna.Caption = itemFilaVariable["Nombre"].ToString();
                            nuevaColumna.ColumnName = itemFilaVariable["Nombre"].ToString();
                            nuevaColumna.DataType = typeof(double);

                            tablaDatos.Columns.Add(nuevaColumna);
                        }
                        entraVP = false;
                    }

                    string exp = "Mes = '" + meses[fechaDesdeTmp.Month - 1] + "' and IdSuit = " + tablaDatos.Rows[pp]["IdSuit"] + " and IdPropietario = " + tablaDatos.Rows[pp]["IdPropietario"];
                    DataRow[] itemFilaTmp = tablaDatos.Select(exp);

                    foreach (DataRow itemFilaVP in tablaDatosVariableP.Rows)
                    {
                        itemFilaTmp[0][itemFilaVP["Nombre"].ToString()] = itemFilaVP["Valor"].ToString();
                    }
                    #endregion

                    #region Obteniendo datos de tipo Hotel
                    string sqlDatosVariableHotel = "SELECT Variable.Nombre, Valor_Variable.Valor " +
                                                   "FROM Variable " +
                                                   "INNER JOIN Valor_Variable ON Variable.IdVariable = Valor_Variable.IdVariable " +
                                                   "WHERE Variable.Activo = 1 AND " +
                                                   "Variable.IdHotel = " + idHotel + " AND " +
                                                   "(MONTH(Valor_Variable.Fecha) = " + fechaDesdeTmp.Month + " AND YEAR(Valor_Variable.Fecha) = " + fechaDesdeTmp.Year + ") AND " +
                                                   "Variable.IdVariable IN ( " +
                                                   "SELECT Reporte_Detalle.IdVariable " +
                                                   "FROM Reporte_Detalle " +
                                                   "WHERE Reporte_Detalle.IdReporte = " + idReporte + " AND " +
                                                   "Reporte_Detalle.Tipo = 'H' AND " +
                                                   "Reporte_Detalle.IdHotel = " + idHotel + ")";
                    DataTable tablaDatosVariableH = Utilities.Select(sqlDatosVariableHotel, "reporteConsolidadoPorSuite");

                    // Nuevas columnas 
                    if (entraVH)
                    {
                        DataView view = new DataView(tablaDatosVariableH);
                        DataTable tablaVariables = view.ToTable(true, "Nombre");

                        foreach (DataRow itemFilaVariable in tablaVariables.Rows)
                        {
                            DataColumn nuevaColumna = new DataColumn();
                            nuevaColumna.Caption = itemFilaVariable["Nombre"].ToString();
                            nuevaColumna.ColumnName = itemFilaVariable["Nombre"].ToString();
                            nuevaColumna.DataType = typeof(double);

                            tablaDatos.Columns.Add(nuevaColumna);
                        }
                        entraVH = false;
                    }

                    exp = "Mes = '" + meses[fechaDesdeTmp.Month - 1] + "' and IdSuit = " + tablaDatos.Rows[pp]["IdSuit"] + " and IdPropietario = " + tablaDatos.Rows[pp]["IdPropietario"];
                    itemFilaTmp = tablaDatos.Select(exp);

                    foreach (DataRow itemFilaVH in tablaDatosVariableH.Rows)
                    {
                        itemFilaTmp[0][itemFilaVH["Nombre"].ToString()] = itemFilaVH["Valor"].ToString();
                    }
                    #endregion

                    #region Obteniendo datos de tipo Concepto
                    string sqlDatosVariableConcepto = "SELECT  Concepto.Nombre, Liquidacion.Valor, Liquidacion.IdSuit,Liquidacion.IdConcepto,Liquidacion.IdPropietario " +
                                                      "FROM Liquidacion " +
                                                      "INNER JOIN Concepto ON Liquidacion.IdConcepto = Concepto.IdConcepto " +
                                                      "WHERE " +
                                                      "Liquidacion.IdHotel = " + idHotel + " AND " +
                                                      "(MONTH(Liquidacion.FechaPeriodoLiquidado) = " + fechaDesdeTmp.Month + " AND " +
                                                      "YEAR(Liquidacion.FechaPeriodoLiquidado) = " + fechaDesdeTmp.Year + ") AND " +
                                                      "Liquidacion.IdSuit = " + idSuite + " AND " +
                                                      "IdPropietario = " + tablaDatos.Rows[pp]["IdPropietario"].ToString() + " AND " +
                                                      "Liquidacion.IdConcepto IN " +
                                                      "(SELECT Reporte_Detalle.IdConcepto " +
                                                      "FROM Reporte_Detalle " +
                                                      "WHERE Reporte_Detalle.IdReporte = " + idReporte + " AND " +
                                                      "Reporte_Detalle.Tipo = 'C' AND " +
                                                      "Reporte_Detalle.IdHotel = " + idHotel + ") " +
                                                      "ORDER BY Liquidacion.FechaPeriodoLiquidado,Liquidacion.IdPropietario";

                    DataTable tablaDatosVariableC = Utilities.Select(sqlDatosVariableConcepto, "reporteConsolidadoPorSuiteC");

                    // Nuevas columnas 
                    if (entraVC)
                    {
                        DataView view = new DataView(tablaDatosVariableC);
                        DataTable tablaVariables = view.ToTable(true, "Nombre");

                        foreach (DataRow itemFilaVariable in tablaDatosVariableC.Rows)
                        {
                            DataColumn nuevaColumna = new DataColumn();
                            nuevaColumna.Caption = itemFilaVariable["Nombre"].ToString();
                            nuevaColumna.ColumnName = itemFilaVariable["Nombre"].ToString();
                            nuevaColumna.DataType = typeof(double);

                            tablaDatos.Columns.Add(nuevaColumna);
                        }
                        entraVC = false;
                    }

                    exp = "Mes = '" + meses[fechaDesdeTmp.Month - 1] + "' and IdSuit = " + tablaDatos.Rows[pp]["IdSuit"] + " and IdPropietario = " + tablaDatos.Rows[pp]["IdPropietario"];
                    itemFilaTmp = tablaDatos.Select(exp);

                    foreach (DataRow itemFilaVC in tablaDatosVariableC.Rows)
                    {
                        itemFilaTmp[0][itemFilaVC["Nombre"].ToString()] = itemFilaVC["Valor"].ToString();
                    }

                    #endregion

                    fechaDesdeTmp = fechaDesdeTmp.AddMonths(1);

                    if (fechaDesdeTmp.Month <= fechaHasta.Month)
                    {
                        DataRow filaNueva = tablaDatos.NewRow();
                        filaNueva["MES"] = meses[fechaDesdeTmp.Month - 1];
                        filaNueva["Propietario"] = tablaDatos.Rows[pp]["Propietario"].ToString();
                        filaNueva["TipoPersona"] = tablaDatos.Rows[pp]["TipoPersona"].ToString();
                        filaNueva["TipoDocumento"] = tablaDatos.Rows[pp]["TipoDocumento"].ToString();
                        filaNueva["NumIdentificacion"] = tablaDatos.Rows[pp]["NumIdentificacion"].ToString();
                        filaNueva["IdPropietario"] = tablaDatos.Rows[pp]["IdPropietario"].ToString();
                        filaNueva["IdSuit"] = tablaDatos.Rows[pp]["IdSuit"].ToString();
                        tablaDatos.Rows.Add(filaNueva);
                    }

                } while (fechaDesdeTmp.Month <= fechaHasta.Month);

                fechaDesdeTmp = new DateTime(fechaDesde.Year, fechaDesde.Month, 1);
            }

            tablaDatos.Columns.Remove("IdPropietario");
            tablaDatos.Columns.Remove("IdSuit");

            #region Ordenacion tabla
            DataTable tablaFinal = new DataTable();
            List<Reporte_Detalle> listaReporteDetalle = new List<Reporte_Detalle>();
            ConfiguracionReporteBo configuracionReporteBoTmp = new ConfiguracionReporteBo();
            listaReporteDetalle = configuracionReporteBoTmp.ObtenerOrden(idReporte, idHotel);

            tablaFinal.Columns.Add("MES", typeof(string));
            tablaFinal.Columns.Add("Propietario", typeof(string));
            tablaFinal.Columns.Add("TipoPersona", typeof(string));
            tablaFinal.Columns.Add("TipoDocumento", typeof(string));
            tablaFinal.Columns.Add("NumIdentificacion", typeof(string));

            foreach (Reporte_Detalle item in listaReporteDetalle)
            {
                if (item.Tipo == "C")
                    tablaFinal.Columns.Add(item.Concepto.Nombre);
                else
                    tablaFinal.Columns.Add(item.Variable.Nombre);
            }

            for (int i = 0; i < tablaDatos.Rows.Count; i++)
            {
                DataRow filaNueva = tablaFinal.NewRow();

                for (int c = 0; c < tablaDatos.Columns.Count; c++)
                {
                    filaNueva[tablaDatos.Columns[c].ColumnName] = tablaDatos.Rows[i][c];
                }
                tablaFinal.Rows.Add(filaNueva);
            }
            #endregion


            DataView viewDatos = new DataView(tablaFinal);
            viewDatos.Sort = "Propietario desc";
            tablaFinal = viewDatos.ToTable();


            // Llenado de los datos
            tblNombreTitulos.Rows.Clear();
            float ancho = 2575 / tablaFinal.Columns.Count;            

            filaTmp = new XRTableRow();
            foreach (DataColumn itemColumna in tablaFinal.Columns)
            {
                celdaTmp = new XRTableCell();
                celdaTmp.WidthF = ancho;
                celdaTmp.Text = itemColumna.ColumnName.ToUpper();

                filaTmp.Cells.Add(celdaTmp);
            }
            tblNombreTitulos.Rows.Add(filaTmp);

            // Llenado del cuerpo
            tblDatos.Rows.Clear();
            foreach (DataRow itemFila in tablaFinal.Rows)
            {
                filaTmp = new XRTableRow();
                for (int i = 0; i < tablaFinal.Columns.Count; i++)
                {
                    celdaTmp = new XRTableCell();
                    celdaTmp.WidthF = ancho;
                    celdaTmp.Text = itemFila[i].ToString();

                    filaTmp.Cells.Add(celdaTmp);
                }
                tblDatos.Rows.Add(filaTmp);
            }            
        }

    }
}
