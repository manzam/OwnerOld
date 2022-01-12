using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using Servicios;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data;

namespace BO
{
    public class InterfazBo
    {        
        PropietarioBo propietarioBoTmp;
        ParametroBo parametroBoTmp;
        CentroCostoBo centroCostoBo;
        private List<ObjetoGenerico> ListaInterfaz;
        private int IdHotel;
        private string separador;
        private SqlConnection sqlConection;
        public StringBuilder ListaErrores { get; set; }

        #region Propiedades
        public string F_TIPO_REG_Inicio { get; set; }
        public string F_SUBTIPO_REG_Inicio { get; set; }
        public string F_VERSION_REG_Inicio { get; set; }
        public string F_CIA_Inicio { get; set; }
        public string F_TIPO_REG_Final { get; set; }
        public string F_SUBTIPO_REG_Final { get; set; }
        public string F_VERSION_REG_Final { get; set; }
        public string F_CIA_Final { get; set; }
        public string F_TIPO_REG { get; set; }
        public string F_SUBTIPO_REG { get; set; }
        public string F_VERSION_REG { get; set; }
        public string F_CIA { get; set; }
        public string F351_ID_UN { get; set; }
        public string F353_ID_SUCURSAL { get; set; }
        public string F353_NRO_CUOTA_CRUCE { get; set; }
        public string F353_ID_FE { get; set; }
        public string F_SUBTIPO_REG_MXP { get; set; }
        public string F_CONSEC_AUTO_REG { get; set; }
        public string F_TIPO_REG_ENCABEZADO { get; set; }
        public string F350_ID_TIPO_DOCTO { get; set; } 
        public string CodigoHotel { get; set; }
        public string NitHotelEstelar { get; set; }
        public string F350_ID_CLASE_DOCTO { get; set; }
        public string F350_IND_ESTADO { get; set; }
        public string F350_IND_IMPRESION { get; set; }

        #endregion

        public string ObtenerBaseGravable(int idCuentaContable, int idHotel, int idPropietario, int idSuite, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                var centroCosto = Contexto.
                                  CentroCosto_Hotel.Include("Concepto").
                                  Where(CCH => CCH.Hotel.IdHotel == idHotel &&
                                        CCH.Cuenta_Contable.IdCuentaContable == idCuentaContable).
                                  FirstOrDefault();

                if (centroCosto == null)
                    return "+000000000000000.0000";

                if (centroCosto.EsConBase)
                {
                    double valor = Contexto.
                                    Liquidacion.
                                    Where(L => L.Concepto.IdConcepto == centroCosto.Concepto.IdConcepto &&
                                          L.Hotel.IdHotel == idHotel &&
                                          L.Suit.IdSuit == idSuite &&
                                          L.Propietario.IdPropietario == idPropietario &&
                                          L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                          L.FechaPeriodoLiquidado.Month == fecha.Month).
                                    Select(L => L.Valor).
                                    FirstOrDefault();

                    return "+" + Utilities.PadLeft(((int)Math.Round(valor)) + ".0000", 20, '0');
                }
                else
                {
                    return "+000000000000000.0000";
                }
            }
        }

        public string ObtenerCodigoTercero(int idCuentaContable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.CentroCosto_Hotel.Where(CC_H => CC_H.Hotel.IdHotel == IdHotel && CC_H.Cuenta_Contable.IdCuentaContable == idCuentaContable).Select(CC_H => CC_H.CodigoTercero).FirstOrDefault();
            }
        }

        public InterfazBo(DateTime fechaPeriodo, int idHotel, List<ObjetoGenerico> listaIdSuiteIdPropietario, string cnn)
        {
            ListaErrores = new StringBuilder();
            sqlConection = new SqlConnection(cnn);
            propietarioBoTmp = new PropietarioBo();
            parametroBoTmp = new ParametroBo();
            centroCostoBo = new CentroCostoBo();
            ListaInterfaz = new List<ObjetoGenerico>();

            IdHotel = idHotel;
            separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                if (listaIdSuiteIdPropietario.Count > 0)
                {
                    foreach (ObjetoGenerico itemIdSuiteIdPropietario in listaIdSuiteIdPropietario)
                    {
                        ObjetoGenerico item = (from L in Contexto.Liquidacion
                                               join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                               join CC in Contexto.Cuenta_Contable on C.Cuenta_Contable.IdCuentaContable equals CC.IdCuentaContable
                                               join H in Contexto.Hotel on C.Hotel.IdHotel equals H.IdHotel
                                               join TCC in Contexto.Tipo_Cuenta_Contable on CC.Tipo_Cuenta_Contable.IdTipoCuentaContable equals TCC.IdTipoCuentaContable
                                               join UP in Contexto.Propietario on L.Propietario.IdPropietario equals UP.IdPropietario
                                               join S in Contexto.Suit on L.Suit.IdSuit equals S.IdSuit
                                               where L.FechaPeriodoLiquidado.Month == fechaPeriodo.Month &&
                                                     L.FechaPeriodoLiquidado.Year == fechaPeriodo.Year &&
                                                     H.IdHotel == idHotel &&
                                                     S.IdSuit == itemIdSuiteIdPropietario.IdSuit &&
                                                     UP.IdPropietario == itemIdSuiteIdPropietario.IdPropietario
                                               select new ObjetoGenerico()
                                               {
                                                   NombreTipoCuentaContable = TCC.Llave,
                                                   NombreConcepto = C.Nombre,
                                                   Valor = L.Valor,
                                                   Codigo = H.Codigo,
                                                   EncabezadoDocCruce = CC.EncabezadoDocCruce,
                                                   Fecha = L.FechaPeriodoLiquidado,
                                                   CodigoCuentaContable = CC.Codigo,
                                                   NaturalezaCuenta = CC.NaturalezaCuenta,
                                                   EsCentroCostoVariable = CC.EsCentroCostoVariable,
                                                   //EsTerceroVariable = CC.EsTerceroVariable,
                                                   //EsTerceroVariable = 
                                                   NumIdentificacion = UP.NumIdentificacion,
                                                   NumSuit = S.NumSuit,
                                                   NumEscritura = S.NumEscritura,
                                                   UnidadNegocioHotel = H.UnidadNegocio,
                                                   UnidadNegocioCuentaContable = CC.UnidadNegocio,
                                                   IncluyeCeCo = CC.IncluyeCeCo,
                                                   IdCuentaContable = CC.IdCuentaContable,
                                                   IdCentroCosto = (CC.Centro_Costo.IdCentroCosto == null) ? -1 : CC.Centro_Costo.IdCentroCosto,
                                                   IdConcepto = C.IdConcepto,
                                                   CodigoTercero = C.CodigoTercero,
                                                   IdSuit = S.IdSuit,
                                                   IdHotel = H.IdHotel,
                                                   IdPropietario = UP.IdPropietario,
                                                   // Variables cuando el concepto tiene amarrada una segunda cuenta contable
                                                   EsConSegundaCuenta = C.EsConSegundaCuentaContable,
                                                   Condicion = C.Condicion,
                                                   ValorCondicion = C.ValorCondicion,
                                                   IdVariableCondicion = C.IdVariableCondicion,
                                                   TipoVariableCondicion = C.TipoVariableCondicion
                                               }).FirstOrDefault();

                        this.ListaInterfaz.Add(item);
                    }
                }
                else
                {
                    this.ListaInterfaz = (from L in Contexto.Liquidacion
                                          join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                          join CC in Contexto.Cuenta_Contable on C.Cuenta_Contable.IdCuentaContable equals CC.IdCuentaContable
                                          join H in Contexto.Hotel on C.Hotel.IdHotel equals H.IdHotel
                                          join TCC in Contexto.Tipo_Cuenta_Contable on CC.Tipo_Cuenta_Contable.IdTipoCuentaContable equals TCC.IdTipoCuentaContable
                                          join UP in Contexto.Propietario on L.Propietario.IdPropietario equals UP.IdPropietario
                                          join S in Contexto.Suit on L.Suit.IdSuit equals S.IdSuit
                                          where L.FechaPeriodoLiquidado.Month == fechaPeriodo.Month &&
                                                L.FechaPeriodoLiquidado.Year == fechaPeriodo.Year &&
                                                H.IdHotel == idHotel
                                          select new ObjetoGenerico()
                                          {
                                              NombreTipoCuentaContable = TCC.Llave,
                                              NombreConcepto = C.Nombre,
                                              Valor = L.Valor,
                                              Codigo = H.Codigo,
                                              EncabezadoDocCruce = CC.EncabezadoDocCruce,
                                              Fecha = L.FechaPeriodoLiquidado,
                                              CodigoCuentaContable = CC.Codigo,
                                              NaturalezaCuenta = CC.NaturalezaCuenta,
                                              EsCentroCostoVariable = CC.EsCentroCostoVariable,
                                              NumIdentificacion = UP.NumIdentificacion,
                                              NumSuit = S.NumSuit,
                                              NumEscritura = S.NumEscritura,
                                              UnidadNegocioHotel = H.UnidadNegocio,
                                              UnidadNegocioCuentaContable = CC.UnidadNegocio,
                                              IncluyeCeCo = CC.IncluyeCeCo,
                                              IdCuentaContable = CC.IdCuentaContable,
                                              IdCentroCosto = (CC.Centro_Costo.IdCentroCosto == null) ? -1 : CC.Centro_Costo.IdCentroCosto,
                                              IdConcepto = C.IdConcepto,
                                              CodigoTercero = C.CodigoTercero,
                                              IdSuit = S.IdSuit,
                                              IdHotel = H.IdHotel,
                                              IdPropietario = UP.IdPropietario,
                                              PrimeroNombre = UP.NombrePrimero,
                                              // Variables cuando el concepto tiene amarrada una segunda cuenta contable
                                              EsConSegundaCuenta = C.EsConSegundaCuentaContable,
                                              Condicion = C.Condicion,
                                              ValorCondicion = C.ValorCondicion,
                                              IdVariableCondicion = C.IdVariableCondicion,
                                              TipoVariableCondicion = C.TipoVariableCondicion
                                          }).OrderBy(L => L.PrimeroNombre).ToList();

                    List<ObjetoGenerico> lista = (from L in Contexto.Liquidacion
                                                  join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                                  join CC in Contexto.Cuenta_Contable on C.Cuenta_Contable.IdCuentaContable equals CC.IdCuentaContable
                                                  join H in Contexto.Hotel on C.Hotel.IdHotel equals H.IdHotel
                                                  join TCC in Contexto.Tipo_Cuenta_Contable on CC.Tipo_Cuenta_Contable.IdTipoCuentaContable equals TCC.IdTipoCuentaContable
                                                  where L.FechaPeriodoLiquidado.Month == fechaPeriodo.Month &&
                                                        L.FechaPeriodoLiquidado.Year == fechaPeriodo.Year &&
                                                        H.IdHotel == idHotel && 
                                                        C.NivelConcepto == 1
                                                  select new ObjetoGenerico()
                                                  {
                                                      NombreTipoCuentaContable = TCC.Llave,
                                                      NombreConcepto = C.Nombre,
                                                      Valor = L.Valor,
                                                      Codigo = H.Codigo,
                                                      EncabezadoDocCruce = CC.EncabezadoDocCruce,
                                                      Fecha = L.FechaPeriodoLiquidado,
                                                      CodigoCuentaContable = CC.Codigo,
                                                      NaturalezaCuenta = CC.NaturalezaCuenta,
                                                      EsCentroCostoVariable = CC.EsCentroCostoVariable,
                                                      NumIdentificacion = C.CodigoTercero,
                                                      NumSuit = "",
                                                      NumEscritura = "",
                                                      UnidadNegocioHotel = H.UnidadNegocio,
                                                      UnidadNegocioCuentaContable = CC.UnidadNegocio,
                                                      IncluyeCeCo = CC.IncluyeCeCo,
                                                      IdCuentaContable = CC.IdCuentaContable,
                                                      IdCentroCosto = (CC.Centro_Costo.IdCentroCosto == null) ? -1 : CC.Centro_Costo.IdCentroCosto,
                                                      IdConcepto = C.IdConcepto,
                                                      CodigoTercero = C.CodigoTercero,
                                                      IdSuit = -1,
                                                      IdHotel = H.IdHotel,
                                                      IdPropietario = -1,
                                                      PrimeroNombre = "",
                                                      // Variables cuando el concepto tiene amarrada una segunda cuenta contable
                                                      EsConSegundaCuenta = C.EsConSegundaCuentaContable,
                                                      Condicion = C.Condicion,
                                                      ValorCondicion = C.ValorCondicion,
                                                      IdVariableCondicion = C.IdVariableCondicion,
                                                      TipoVariableCondicion = C.TipoVariableCondicion
                                                  }).OrderBy(L => L.NumIdentificacion).ToList();

                    this.ListaInterfaz.AddRange(lista);
                }
            }
        }

        public StringBuilder GenerarInterfas(int consecutivo, DateTime fechaPeriodo)
        {
            StringBuilder planoTmp = new StringBuilder();
            //Inicio Plano
            consecutivo++;
            string basePlano = Utilities.PadLeft(consecutivo.ToString(), 7, '0') + //F_NUMERO_REG
                               this.F_TIPO_REG_Inicio +             // F_TIPO_REG 
                               this.F_SUBTIPO_REG_Inicio +          // F_SUBTIPO_REG
                               this.F_VERSION_REG_Inicio +          // F_VERSION_REG
                               this.F_CIA_Inicio;                   // F_CIA
            planoTmp.AppendLine(basePlano);

            //Encabezado Documento Contable
            consecutivo++;
            DateTime fecha = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, DateTime.DaysInMonth(fechaPeriodo.Year, fechaPeriodo.Month));

            string encabezado = Utilities.PadLeft(consecutivo.ToString(), 7, '0') + //F_NUMERO_REG
                                this.F_TIPO_REG_ENCABEZADO +                        // F_TIPO_REG_ENCABEZADO
                                this.F_SUBTIPO_REG_Inicio +                         // F_SUBTIPO_REG
                                this.F_VERSION_REG_Inicio +                         // F_VERSION_REG 
                                this.F_CIA_Inicio +                                 // F_CIA
                                this.F_CONSEC_AUTO_REG +                            // F_CONSEC_AUTO_REG
                                this.CodigoHotel +                                  // F350_ID_CO
                                this.F350_ID_TIPO_DOCTO +                           // F350_ID_TIPO_DOCTO
                                fechaPeriodo.ToString("yyyy") + fechaPeriodo.ToString("MM") + fecha.ToString("dd") +  // F350_CONSEC_DOCTO
                                fechaPeriodo.ToString("yyyy") + fechaPeriodo.ToString("MM") + fecha.ToString("dd") +  // F350_FECHA
                                Utilities.PadRight(this.NitHotelEstelar, 15, ' ') +       // F350_ID_TERCERO
                                Utilities.PadLeft(this.F350_ID_CLASE_DOCTO, 5, '0') +     // F350_ID_CLASE_DOCTO
                                this.F350_IND_ESTADO +                                    // F350_IND_ESTADO
                                this.F350_IND_IMPRESION +                                 // F350_IND_IMPRESION
                                Utilities.PadRight("LIQUIDACION DE COPROPIETARIOS", 255, ' ');  // F350_NOTAS
            planoTmp.AppendLine(encabezado);

            List<CentroCosto_Hotel> listaCeCoHotel=new List<CentroCosto_Hotel>();
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                listaCeCoHotel = Contexto.CentroCosto_Hotel.Include("Cuenta_Contable").Where(CCH => CCH.Hotel.IdHotel == this.IdHotel).ToList();
            }


            string linea = string.Empty;
            // Cuerpo Plano
            foreach (ObjetoGenerico itemInterfaz in this.ListaInterfaz)
            {
                consecutivo++;
                if (consecutivo == 87)
                { 
                }

                itemInterfaz.EsTerceroVariable = listaCeCoHotel.Where(CCH => CCH.Cuenta_Contable.IdCuentaContable == itemInterfaz.IdCuentaContable).Select(CCH => CCH.EsTerceroVariable).FirstOrDefault();

                #region Validacion de segunda cuenta contable
                if (itemInterfaz.EsConSegundaCuenta)
                {
                    using (ContextoOwner Contexto = new ContextoOwner())
                    {
                        ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
                        double valorVariable = 0;

                        if (itemInterfaz.TipoVariableCondicion == "P")                            
                            valorVariable = valorVariableBoTmp.ObtenerValorTipoPropietario(itemInterfaz.IdPropietario, itemInterfaz.IdSuit, itemInterfaz.IdVariableCondicion.Value);
                        else
                            valorVariable = valorVariableBoTmp.ObtenerValorTipoHotel(itemInterfaz.IdVariableCondicion.Value);

                        EntityConnection ec = (EntityConnection)Contexto.Connection;
                        SqlConnection sc = (SqlConnection)ec.StoreConnection;
                        string stringCnn = sc.ConnectionString;

                        SqlConnection cnn = new SqlConnection(stringCnn);

                        SqlCommand cmd = new SqlCommand("select case when(@valorVariable >= @valorCondicion) then 'true' else 'false' end");

                        SqlParameter parameter1 = cmd.Parameters.Add("@valorVariable", SqlDbType.Decimal);
                        parameter1.Value = itemInterfaz.ValorCondicion;

                        SqlParameter parameter2 = cmd.Parameters.Add("@valorCondicion", SqlDbType.Decimal);
                        parameter2.Value = itemInterfaz.ValorCondicion;

                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            if (Convert.ToBoolean(rdr[0]))
                            {
                                ObjetoGenerico oGenerico = (from CC in Contexto.Cuenta_Contable
                                                            join TC in Contexto.Tipo_Cuenta_Contable on CC.Tipo_Cuenta_Contable.IdTipoCuentaContable equals TC.IdTipoCuentaContable
                                                            select new ObjetoGenerico()
                                                            {
                                                                IdCuentaContable = CC.IdCuentaContable,
                                                                NombreTipoCuentaContable = TC.Llave,
                                                                Codigo = CC.Codigo,
                                                                NaturalezaCuenta = CC.NaturalezaCuenta,
                                                                EsCentroCostoVariable = CC.EsCentroCostoVariable,
                                                                //EsTerceroVariable = CC.EsTerceroVariable,
                                                                EncabezadoDocCruce = CC.EncabezadoDocCruce,
                                                                IncluyeCeCo = CC.IncluyeCeCo,
                                                                UnidadNegocioCuentaContable = CC.UnidadNegocio
                                                            }).FirstOrDefault();

                                oGenerico.EsTerceroVariable = listaCeCoHotel.Where(CCH => CCH.Cuenta_Contable.IdCuentaContable == oGenerico.IdCuentaContable).Select(CCH => CCH.EsTerceroVariable).FirstOrDefault();

                                itemInterfaz.NombreTipoCuentaContable = oGenerico.NombreTipoCuentaContable;
                                itemInterfaz.CodigoCuentaContable = oGenerico.Codigo;
                                itemInterfaz.NaturalezaCuenta = oGenerico.NaturalezaCuenta;
                                itemInterfaz.EsCentroCostoVariable = oGenerico.EsCentroCostoVariable;
                                itemInterfaz.EsTerceroVariable = oGenerico.EsTerceroVariable;
                                itemInterfaz.EncabezadoDocCruce = oGenerico.EncabezadoDocCruce;
                                itemInterfaz.IncluyeCeCo = oGenerico.IncluyeCeCo;
                                itemInterfaz.UnidadNegocioCuentaContable = oGenerico.UnidadNegocio;
                            }
                        }
                        rdr.Close();
                    }
                }
                #endregion

                switch (itemInterfaz.NombreTipoCuentaContable)
                {
                    case "MovimientoContable":
                        linea = this.MovimientoContable(itemInterfaz, consecutivo, fecha.ToString("dd"));
                        if (linea != string.Empty)
                            planoTmp.AppendLine(linea);
                        else
                            consecutivo--;
                        break;

                    case "MovimientoCuentasPorPagar":
                        linea = this.MovimientoCuentasPorPagar(itemInterfaz, consecutivo, fecha.Day);
                        if (linea != string.Empty)
                            planoTmp.AppendLine(linea);
                        else
                            consecutivo--;
                        break;

                    case "MovimientoCuentasPorCobrar":
                        linea = this.MovimientoCuentasPorCobrar(itemInterfaz, consecutivo, fecha.Day);
                        if (linea != string.Empty)
                            planoTmp.AppendLine(linea);
                        else
                            consecutivo--;
                        break;
                    default:
                        break;
                }
                linea = string.Empty;
            }

            consecutivo++;
            //Final Plano
            basePlano = Utilities.PadLeft(consecutivo.ToString(), 7, '0') +
                        this.F_TIPO_REG_Final +             // F_TIPO_REG 
                        this.F_SUBTIPO_REG_Final +          // F_SUBTIPO_REG
                        this.F_VERSION_REG_Final +          // F_VERSION_REG
                        this.F_CIA_Final;                   // F_CIA
            planoTmp.AppendLine(basePlano);

            return planoTmp;
        }

        private string MovimientoContable(ObjetoGenerico interfazTmp, int consecutivo, string diaFinalMes)
        {
            string tercero = "";
            string valorDebito = "";
            string valorCredito = "";
            string baseGravable = "";

            // Base Gravable
            baseGravable = this.ObtenerBaseGravable(interfazTmp.IdCuentaContable, interfazTmp.IdHotel, interfazTmp.IdPropietario, interfazTmp.IdSuit, interfazTmp.Fecha);

            if (!string.IsNullOrEmpty(interfazTmp.CodigoTercero)) // Si es vacio se coje ese como codigo
            {
                tercero = interfazTmp.CodigoTercero;
            }
            else
            {
                // Si el campo "EsTerceroVariable" se busca el valor en la tabla Centrocosto_Hotel "CodigoTercero", si no
                // se obtiene el nit o NumIdentificacion del liquidado.
                if (interfazTmp.EsTerceroVariable)
                    tercero = interfazTmp.NumIdentificacion;
                else
                    tercero = this.ObtenerCodigoTercero(interfazTmp.IdCuentaContable);
            }
                

            // Debito y Credito
            if (interfazTmp.NaturalezaCuenta == "Debito")
            {
                valorDebito = "+" + Utilities.PadLeft(((int)Math.Round(interfazTmp.Valor)) + ".0000", 20, '0');
                valorCredito = "+000000000000000.0000";
            }
            else
            {
                valorCredito = "+" + Utilities.PadLeft(((int)Math.Round(interfazTmp.Valor)) + ".0000", 20, '0');
                valorDebito = "+000000000000000.0000";
            }

            // si los valores son ceros, no debe sacarlo en el plano
            if (valorCredito == "+000000000000000.0000" && valorDebito == "+000000000000000.0000")
                return string.Empty;

            string unidadNegocio = "";
            if (interfazTmp.UnidadNegocioCuentaContable == string.Empty || interfazTmp.UnidadNegocioCuentaContable == null)
                unidadNegocio = interfazTmp.UnidadNegocioHotel;
            else
                unidadNegocio = interfazTmp.UnidadNegocioCuentaContable;

            string cc = centroCostoBo.ObtenerCodigoCentroCosto(IdHotel, interfazTmp.IdCuentaContable,
                                       interfazTmp.IdCentroCosto, interfazTmp.EsCentroCostoVariable, interfazTmp.IncluyeCeCo);

            if (cc == null)
            {
                //ListaErrores.AppendLine("Debes configurar, en el modulo Cuenta Contable - Hotel, el centro costo. Cuenta Contable : " + interfazTmp.CodigoCuentaContable);
                cc = "";
            }

            string notas = Utilities.PadRight(("LIQUIDACION PROPIETARIOS " + interfazTmp.Fecha.ToLongDateString() + " Suite " + interfazTmp.NumSuit), 255, ' ');
            //notas = Utilities.QuitarAcentuaciones(notas);

            string linea = Utilities.PadLeft(consecutivo.ToString(), 7, '0') +                         // F_NUMERO_REG
                           Utilities.PadLeft(this.F_TIPO_REG, 4, '0') +                                // F_TIPO_REG
                           this.F_SUBTIPO_REG +                                                        // F_SUBTIPO_REG
                           this.F_VERSION_REG +                                                        // F_VERSION_REG
                           this.F_CIA +                                                                // F_CIA
                           interfazTmp.Codigo +                                                        // F350_ID_CO
                           Utilities.PadRight(interfazTmp.EncabezadoDocCruce, 3, ' ') +                // F350_ID_TIPO_DOCTO
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes +  // F350_CONSEC_DOCTO
                           Utilities.PadRight(interfazTmp.CodigoCuentaContable, 20, ' ') +             // F351_ID_AUXILIAR
                           Utilities.PadRight(tercero, 15, ' ') +                                      // F351_ID_TERCERO
                           interfazTmp.Codigo +                                                        // F351_ID_CO_MOV
                           unidadNegocio +                                                             // F351_ID_UN
                           Utilities.PadRight(cc, 15, ' ') +                                           // F351_ID_CCOSTO
                           "          " +                                                              // F351_ID_FE
                           valorDebito +                                                               // F351_VALOR_DB
                           valorCredito +                                                              // F351_VALOR_CR
                           "+000000000000000.0000" +                                                   // F351_VALOR_DB_ALT
                           "+000000000000000.0000" +                                                   // F351_VALOR_CR_ALT
                           baseGravable +                                                              // F351_BASE_GRAVABLE                           
                           "  " +                                                                      // F351_DOCTO_BANCO
                           "00000000" +                                                                // F351_NRO_DOCTO_BANCO
                           notas;                                                                      // F351_NOTAS
            return linea;
        }

        private string MovimientoCuentasPorPagar(ObjetoGenerico interfazTmp, int consecutivo, int diaFinalMes)
        {
            string tercero = "";
            string valorDebito = "";
            string valorCredito = "";

            if (!string.IsNullOrEmpty(interfazTmp.CodigoTercero))// Si es vacio se coje ese como codigo
            {
                tercero = interfazTmp.CodigoTercero;
            }
            else
            {
                // Si el campo "EsTerceroVariable" se busca el valor en la tabla Centrocosto_Hotel "CodigoTercero", si no
                // se obtiene el nit o NumIdentificacion del liquidado.
                if (interfazTmp.EsTerceroVariable)
                    tercero = interfazTmp.NumIdentificacion;
                else
                    tercero = this.ObtenerCodigoTercero(interfazTmp.IdCuentaContable);
            }

            // Debito y Credito
            if (interfazTmp.NaturalezaCuenta == "Debito")
            {
                valorDebito = "+" + Utilities.PadLeft(((int)Math.Round(interfazTmp.Valor)) + ".0000", 20, '0');
                valorCredito = "+000000000000000.0000";
            }
            else
            {
                valorCredito = "+" + Utilities.PadLeft(((int)Math.Round(interfazTmp.Valor)) + ".0000", 20, '0');
                valorDebito = "+000000000000000.0000";
            }

            if (valorCredito == "+000000000000000.0000" && valorDebito == "+000000000000000.0000")
                return string.Empty;

            string unidadNegocio = "";
            if (interfazTmp.UnidadNegocioCuentaContable == string.Empty || interfazTmp.UnidadNegocioCuentaContable == null)
                unidadNegocio = interfazTmp.UnidadNegocioHotel;
            else
                unidadNegocio = interfazTmp.UnidadNegocioCuentaContable;

            string cc = centroCostoBo.ObtenerCodigoCentroCosto(IdHotel, interfazTmp.IdCuentaContable,
                                       interfazTmp.IdCentroCosto, interfazTmp.EsCentroCostoVariable, interfazTmp.IncluyeCeCo);
            if (cc == null)
            {
                //ListaErrores.AppendLine("Debes configurar, en el modulo Cuenta Contable - Hotel, el centro costo. Cuenta Contable : " + interfazTmp.CodigoCuentaContable);
                cc = "";
            }

            string notas = Utilities.PadRight(("LIQUIDACION PROPIETARIOS " + interfazTmp.Fecha.ToLongDateString() + " Suite " + interfazTmp.NumSuit), 255, ' ');
            //notas = Utilities.QuitarAcentuaciones(notas);
                        
            string linea = Utilities.PadLeft(consecutivo.ToString(), 7, '0') +                          // F_NUMERO_REG
                           Utilities.PadLeft(this.F_TIPO_REG, 4, '0') +                                 // F_TIPO_REG
                           this.F_SUBTIPO_REG_MXP +                                                     // F_SUBTIPO_REG
                           this.F_VERSION_REG +                                                         // F_VERSION_REG
                           this.F_CIA +                                                                 // F_CIA
                           interfazTmp.Codigo +                                                         // F350_ID_CO
                           Utilities.PadRight(interfazTmp.EncabezadoDocCruce, 3, ' ') +                // F350_ID_TIPO_DOCTO
                           interfazTmp.Fecha.ToString("yyyy") + "" + interfazTmp.Fecha.ToString("MM") + "" + diaFinalMes +  // F350_CONSEC_DOCTO
                           Utilities.PadRight(interfazTmp.CodigoCuentaContable, 20, ' ') +              // F351_ID_AUXILIAR
                           Utilities.PadRight(tercero, 15, ' ') +                                       // F351_ID_TERCERO
                           interfazTmp.Codigo +                                                         // F351_ID_CO_MOV
                           unidadNegocio +                                                              // F351_ID_UN
                           Utilities.PadRight(cc, 15, ' ') +                                            // F351_ID_CCOSTO
                           valorDebito +                                                                // F351_VALOR_DB
                           valorCredito +                                                               // F351_VALOR_CR
                           "+000000000000000.0000" +                                                    // F351_VALOR_DB_ALT
                           "+000000000000000.0000" +                                                    // F351_VALOR_CR_ALT
                           notas +                                                                      // F351_NOTAS
                           this.F353_ID_SUCURSAL +                                                      // F353_ID_SUCURSAL
                           //  Utilities.PadRight(interfazTmp.NumEscritura.Trim(), 4, ' ') +                // F353_PREFIJO_CRUCE
                           "LCP " +
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes + // F353_CONSEC_DOCTO_CRUCE
                           this.F353_NRO_CUOTA_CRUCE +                                                  // F353_NRO_CUOTA_CRUCE
                           Utilities.PadRight(this.F353_ID_FE, 10, ' ') +                                // F353_ID_FE
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes +  // F353_FECHA_VCTO
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes +  // F353_FECHA_DSCTO_PP
                           "+000000000000000.0000" +                                                    // F353_VLR_DSCTO_PP
                           "+000000000000000.0000" +                                                    // F354_VALOR_APLICADO_PP
                           "+000000000000000.0000" +                                                    // F354_VALOR_APLICADO_PP_ALT
                           "+000000000000000.0000" +                                                    // F354_VALOR_RETENCION
                           "+000000000000000.0000" +                                                    // F354_VALOR_RETENCION_ALT
                           notas;                                                                       // F351_NOTAS
            return linea;
        }

        private string MovimientoCuentasPorCobrar(ObjetoGenerico interfazTmp, int consecutivo, int diaFinalMes)
        {
            string tercero = "";
            string valorDebito = "";
            string valorCredito = "";

            if (!string.IsNullOrEmpty(interfazTmp.CodigoTercero))// Si es vacio se coje ese como codigo
            {
                tercero = interfazTmp.CodigoTercero;
            }
            else
            {
                // Si el campo "EsTerceroVariable" se busca el valor en la tabla Centrocosto_Hotel "CodigoTercero", si no
                // se obtiene el nit o NumIdentificacion del liquidado.
                if (interfazTmp.EsTerceroVariable)
                    tercero = interfazTmp.NumIdentificacion;
                else
                    tercero = this.ObtenerCodigoTercero(interfazTmp.IdCuentaContable);
            }

            // Debito y Credito
            if (interfazTmp.NaturalezaCuenta == "Debito")
            {
                valorDebito = "+" + Utilities.PadLeft(((int)Math.Round(interfazTmp.Valor)) + ".0000", 20, '0');
                valorCredito = "+000000000000000.0000";
            }
            else
            {
                valorCredito = "+" + Utilities.PadLeft(((int)Math.Round(interfazTmp.Valor)) + ".0000", 20, '0');
                valorDebito = "+000000000000000.0000";
            }

            if (valorCredito == "+000000000000000.0000" && valorDebito == "+000000000000000.0000")
                return string.Empty;

            string unidadNegocio = "";
            if (interfazTmp.UnidadNegocioCuentaContable == string.Empty || interfazTmp.UnidadNegocioCuentaContable == null)
                unidadNegocio = interfazTmp.UnidadNegocioHotel;
            else
                unidadNegocio = interfazTmp.UnidadNegocioCuentaContable;

            string cc = centroCostoBo.ObtenerCodigoCentroCosto(IdHotel, interfazTmp.IdCuentaContable,
                                       interfazTmp.IdCentroCosto, interfazTmp.EsCentroCostoVariable, interfazTmp.IncluyeCeCo);
            if (cc == null)
            {
                //ListaErrores.AppendLine("Debes configurar, en el modulo Cuenta Contable - Hotel, el centro costo. Cuenta Contable : " + interfazTmp.CodigoCuentaContable);
                cc = "";
            }

            string notas = Utilities.PadRight(("LIQUIDACION PROPIETARIOS " + interfazTmp.Fecha.ToLongDateString() + " Suite " + interfazTmp.NumSuit), 255, ' ');
            //notas = Utilities.QuitarAcentuaciones(notas);

            string linea = Utilities.PadLeft(consecutivo.ToString(), 7, '0') +                          // F_NUMERO_REG
                           Utilities.PadLeft(this.F_TIPO_REG, 4, '0') +                                 // F_TIPO_REG
                           "01" +                                                                       // F_SUBTIPO_REG
                           this.F_VERSION_REG +                                                         // F_VERSION_REG
                           this.F_CIA +                                                                 // F_CIA
                           interfazTmp.Codigo +                                                         // F350_ID_CO
                           Utilities.PadRight(interfazTmp.EncabezadoDocCruce, 3, ' ') +                // F350_ID_TIPO_DOCTO
                           interfazTmp.Fecha.ToString("yyyy") + "" + interfazTmp.Fecha.ToString("MM") + "" + diaFinalMes +  // F350_CONSEC_DOCTO
                           Utilities.PadRight(interfazTmp.CodigoCuentaContable, 20, ' ') +              // F351_ID_AUXILIAR
                           Utilities.PadRight(tercero, 15, ' ') +                                       // F351_ID_TERCERO
                           interfazTmp.Codigo +                                                         // F351_ID_CO_MOV
                           unidadNegocio +                                                              // F351_ID_UN
                           Utilities.PadRight(cc, 15, ' ') +                                            // F351_ID_CCOSTO
                           valorDebito +                                                                // F351_VALOR_DB
                           valorCredito +                                                               // F351_VALOR_CR
                           "+000000000000000.0000" +                                                    // F351_VALOR_DB_ALT
                           "+000000000000000.0000" +                                                    // F351_VALOR_CR_ALT
                           notas +                                                                      // F351_NOTAS
                           this.F353_ID_SUCURSAL +                                                      // F353_ID_SUCURSAL
                           this.F350_ID_TIPO_DOCTO +                                                    // F353_ID_TIPO_DOCTO_CRUCE                      
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes + // F353_CONSEC_DOCTO_CRUCE
                           this.F353_NRO_CUOTA_CRUCE +                                                  // F353_NRO_CUOTA_CRUCE                           
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes +  // F353_FECHA_VCTO
                           interfazTmp.Fecha.ToString("yyyy") + interfazTmp.Fecha.ToString("MM") + diaFinalMes +  // F353_FECHA_DSCTO_PP
                           "+000000000000000.0000" +                                                    // F353_VLR_DSCTO_PP
                           "+000000000000000.0000" +                                                    // F354_VALOR_APLICADO_PP
                           "+000000000000000.0000" +                                                    // F354_VALOR_APLICADO_PP_ALT
                           "+000000000000000.0000" +                                                    // F354_VALOR_APROVECHA
                           "+000000000000000.0000" +                                                    // F354_VALOR_APROVECHA_ALT
                           "+000000000000000.0000" +                                                    // F354_VALOR_RETENCION
                           "+000000000000000.0000" +                                                    // F354_VALOR_RETENCION_ALT
                           Utilities.PadRight("Generico", 15, ' ') +                                    // F354_TERCERO_VEND
                           notas;                                                                       // F351_NOTAS
            return linea;
        }
    }
}


// Utilities.PadRight(interfazTmp.NumEscritura, 4, ' ') +                       // F353_PREFIJO_CRUCE
// Utilities.PadRight(this.F353_ID_FE, 10, ' ') +                               // F353_ID_FE