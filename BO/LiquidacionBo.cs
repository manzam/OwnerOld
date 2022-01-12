using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using Servicios;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Threading;

namespace BO
{
    public class LiquidacionBo
    {
        public SqlConnection cnn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();

        public DateTime Fecha { get; set; }
        public int IdHotel { get; set; }
        public int Nivel { get; set; }                
        public int NumReglasTotal { get; set; }
        public int NumReglas { get; set; }
        public int IdConcepto { get; set; }
        public string Condicion { get; set; }
        public List<string> ListaNombresConcepto { get; set; }
        public List<ObjetoGenerico> ListaResultadosConceptos { get; set; }

        public List<ObjetoGenerico> ListaAuditoria { get; set; }
        public List<ObjetoGenerico> ListaPropietario { get; set; }
        public List<ObjetoGenerico> ListaResultadosConceptosFinal { get; set; }
        public List<ObjetoGenerico> ListaResConceptoSumatorias { get; set; }

        //Nuevo
        public List<Concepto> ListaConceptoBase { get; set; }
        public List<Variable> ListaVariablesBase { get; set; }
        public StringBuilder ListaErrores { get; set; }
        //Nuevo

        public LiquidacionBo()
        {

        }
        
        /// <summary>
        /// Iniciliazamos la fecha de liquidacion, hotel, el nivel (Hotel ó propietario) y id concepto
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idHotel"></param>
        /// <param name="nivel"></param>
        /// <param name="idConcepto"></param>
        public LiquidacionBo(DateTime fecha, int idHotel, int nivel, int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ListaErrores = new StringBuilder();
                cnn.ConnectionString = ((EntityConnection)Contexto.Connection).StoreConnection.ConnectionString;

                this.Fecha = fecha;
                this.IdHotel = idHotel;
                this.Nivel = nivel;
                this.IdConcepto = IdConcepto;

                //Nuevo  
                this.ListaConceptoBase = Contexto.Concepto.Include("Informacion_Estadistica").Where(C => C.Hotel.IdHotel == IdHotel && C.NivelConcepto == 1).ToList();
                this.ListaVariablesBase = Contexto.Variable.Where(V => V.Hotel.IdHotel == IdHotel).ToList();
                //Nuevo

                this.ListaResConceptoSumatorias = new List<ObjetoGenerico>();
                this.ListaResultadosConceptosFinal = new List<ObjetoGenerico>();

                this.ListaNombresConcepto = Contexto.Concepto.
                                            Where(C => C.Hotel.IdHotel == this.IdHotel && C.NivelConcepto == this.Nivel).
                                            OrderBy(C => C.Orden).Select(C => C.Nombre).ToList();

                this.Condicion = "not";
                this.NumReglas = 0;
                this.NumReglasTotal = this.ListaNombresConcepto.Count;
            }
        }

        public bool EsValidoLiquidarPropietario(DateTime periodo, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Liquidacion.Where(L => L.FechaPeriodoLiquidado.Year == periodo.Year &&
                                                      L.FechaPeriodoLiquidado.Month == periodo.Month &&
                                                      L.Hotel.IdHotel == idHotel).Count();

                return (con == 0) ? false : true;
            }
        }

        public double ValorLiquidacion(int idConcepto, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = Contexto.Liquidacion.
                                Where(L => L.Concepto.IdConcepto == idConcepto && 
                                           L.FechaPeriodoLiquidado.Month == fecha.Month && 
                                           L.FechaPeriodoLiquidado.Year == fecha.Year).
                                Select(L => L.Valor).
                                First();
                return valor;
            }
        }

        // Propietario
        public void EliminarLiquidacionPorPropietario(int idConcepto, DateTime fecha, int idPropietario, int idSuit, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Liquidacion> listaLiquidacionTmp = Contexto.
                                                         Liquidacion.
                                                         Where(L => L.Concepto.IdConcepto == idConcepto &&
                                                               L.FechaPeriodoLiquidado.Month == fecha.Month &&
                                                               L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                                               L.Propietario.IdPropietario == idPropietario &&
                                                               L.Suit.IdSuit == idSuit &&
                                                               L.Hotel.IdHotel == idHotel).ToList();

                foreach (var liquidacionTmp in listaLiquidacionTmp)
                {
                    Contexto.DeleteObject(liquidacionTmp);    
                }

                if (listaLiquidacionTmp.Count > 0)
                    Contexto.SaveChanges();
            }
        }
        public void EliminarLiquidacionTotal(DateTime fecha, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Liquidacion> listaLiquidacionTmp = Contexto.
                                                        Liquidacion.
                                                        Where(L => L.FechaPeriodoLiquidado.Month == fecha.Month &&
                                                              L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                                              L.Hotel.IdHotel == idHotel && 
                                                              L.EsLiquidacionHotel == false).ToList();

                foreach (Liquidacion itemLiquidacion in listaLiquidacionTmp)
                {
                    Contexto.DeleteObject(itemLiquidacion);                    
                }
                Contexto.SaveChanges();
            }
        }
        // Hotel por concepto
        public void EliminarLiquidacion(int idConcepto, DateTime fecha, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Liquidacion liquidacionTmp = Contexto.
                                             Liquidacion.
                                             Where(L => L.Concepto.IdConcepto == idConcepto &&
                                                   L.FechaPeriodoLiquidado.Month == fecha.Month &&
                                                   L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                                   L.Hotel.IdHotel == idHotel).
                                             FirstOrDefault();
                if (liquidacionTmp != null)
                {
                    Contexto.DeleteObject(liquidacionTmp);
                    Contexto.SaveChanges();
                }
            }
        }
        // Hotel por lista
        public void EliminarLiquidacion(List<int> listaIdConcepto, DateTime fecha, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                foreach (int itemId in listaIdConcepto)
                {
                    Liquidacion liquidacionTmp = Contexto.
                                             Liquidacion.
                                             Where(L => L.Concepto.IdConcepto == itemId &&
                                                   L.FechaPeriodoLiquidado.Month == fecha.Month &&
                                                   L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                                   L.Hotel.IdHotel == idHotel).
                                             FirstOrDefault();
                    if (liquidacionTmp != null)
                        Contexto.DeleteObject(liquidacionTmp);
                }
                Contexto.SaveChanges();
            }
        }

        // Hotel y fecha
        /// <summary>
        /// Elimina una liquidacion de hotel
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idHotel"></param>
        /// <param name="nombreHotel"></param>
        /// <param name="idUsuario"></param>
        public void EliminarLiquidacion(DateTime fecha, int idHotel, string nombreHotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Liquidacion> listaLiquidacion = Contexto.
                                                     Liquidacion.
                                                     Where(L => L.FechaPeriodoLiquidado.Month == fecha.Month &&
                                                           L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                                           L.Hotel.IdHotel == idHotel).
                                                     ToList();
                if (listaLiquidacion.Count > 0)
                {

                    #region auditoria
                    Auditoria auditoriaTmp;

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Hotel: " + nombreHotel + " Periodo : " + fecha.Year + " - " + fecha.Month;
                    auditoriaTmp.NombreTabla = "Liquidacion";
                    auditoriaTmp.Accion = "Eliminar";
                    auditoriaTmp.Campo = "Eliminacion Periodo";
                    auditoriaTmp.Fechahora = DateTime.Now;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    foreach (Liquidacion itemLiqui in listaLiquidacion)
                    {
                        Contexto.DeleteObject(itemLiqui);
                    }

                    Contexto.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Obtiene las variables o el concepto de la regla
        /// </summary>
        /// <param name="idConcepto"></param>
        /// <param name="idSuit"></param>
        /// <param name="idPropitario"></param>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public List<ObjetoGenerico> ObtenerVariablesConcepto(int idConcepto, int idSuit, int idPropitario, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();

                // Obtengo todas las variables del id concepto
                listaVariables = (from C in Contexto.Concepto
                                  join VC in Contexto.Variables_Concepto on C.IdConcepto equals VC.Concepto.IdConcepto
                                  where C.IdConcepto == idConcepto && VC.Operador == string.Empty
                                  select new ObjetoGenerico()
                                  {
                                      IdConcepto = VC.Concepto1.IdConcepto,
                                      IdVariable = VC.Variable.IdVariable,
                                      //Tipo = V.Tipo,
                                      Valor = -1986,
                                      NivelConcepto = C.NivelConcepto,
                                      NomClass = VC.NomClass
                                  }).ToList();

                // Recorro la lista variable para obtener su valor, tipoo y nombre
                foreach (ObjetoGenerico itemVariable in listaVariables)
                {
                    ObjetoGenerico variable = new ObjetoGenerico();

                    switch (itemVariable.NomClass)
                    {
                        case "varV": // variables comunes
                            variable = (from SP in Contexto.Suit_Propietario
                                        join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                        join VVS in Contexto.Valor_Variable_Suit on SP.IdSuitPropietario equals VVS.Suit_Propietario.IdSuitPropietario
                                        join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                        where S.IdSuit == idSuit &&
                                              P.IdPropietario == idPropitario &&
                                              VVS.Variable.IdVariable == itemVariable.IdVariable
                                        select new ObjetoGenerico()
                                        {
                                            Nombre = VVS.Variable.Nombre,
                                            Valor = VVS.Valor,
                                            Tipo = VVS.Variable.Tipo
                                        }).FirstOrDefault();
                            break;

                        case "varH": // variables hotel
                            variable = (from VV in Contexto.Valor_Variable
                                        where VV.Fecha.Year == this.Fecha.Year &&
                                              VV.Fecha.Month == this.Fecha.Month &&
                                              VV.Variable.IdVariable == itemVariable.IdVariable.Value
                                        select new ObjetoGenerico()
                                        {
                                            Nombre = VV.Variable.Nombre,
                                            Valor = VV.Valor,
                                            Tipo = VV.Variable.Tipo
                                        }).FirstOrDefault();
                            break;

                        case "varC": // variables concepto ya liquidados
                            variable = (from L in Contexto.Liquidacion
                                        join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                        where L.Concepto.IdConcepto == itemVariable.IdConcepto.Value &&
                                              L.FechaPeriodoLiquidado.Month == this.Fecha.Month &&
                                              L.FechaPeriodoLiquidado.Year == this.Fecha.Year
                                        select new ObjetoGenerico()
                                        {
                                            Nombre = C.Nombre,
                                            Valor = L.Valor,
                                            Tipo = ""
                                        }).FirstOrDefault();
                            break;

                        case "varCO": // Variables de constantes
                            variable = (from V in Contexto.Variable
                                        where V.IdVariable == itemVariable.IdVariable &&
                                              V.Hotel.IdHotel == idHotel
                                        select new ObjetoGenerico()
                                        {
                                            Nombre = V.Nombre,
                                            Valor = V.ValorConstante,
                                            Tipo = V.Tipo
                                        }).FirstOrDefault();
                            break;

                        default:
                            break;
                    }

                    if (variable != null)
                    {
                        itemVariable.Nombre = variable.Nombre;
                        itemVariable.Valor = variable.Valor;
                        itemVariable.Tipo = variable.Tipo;
                        //itemVariable.IdConcepto = -1;
                    }
                }

                
                return listaVariables;
            }
        }

        public void EjecutarSumatoria(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaResConceptoSumatorias = new List<ObjetoGenerico>();

                List<int> listaIdConceptoSumatorias = (from VC in Contexto.Variables_Concepto
                                                       join C in Contexto.Concepto on VC.Concepto.IdConcepto equals C.IdConcepto
                                                       where C.Hotel.IdHotel == idHotel &&
                                                             VC.Operador == "&Sigma;"
                                                       select C.IdConcepto).Distinct().ToList();

                //idConceptosSumatorias = string.Join(",", listaResConceptoSumatorias.Select(x => x.IdConcepto.ToString()).ToArray());

                foreach (int itemIdConcepto in listaIdConceptoSumatorias)
                {
                    ObjetoGenerico variable = (from VC in Contexto.Variables_Concepto
                                                 join C in Contexto.Concepto on VC.Concepto.IdConcepto equals C.IdConcepto
                                                 join V in Contexto.Variable on VC.Variable.IdVariable equals V.IdVariable
                                                 where C.IdConcepto == itemIdConcepto
                                                 select new ObjetoGenerico()
                                                 {
                                                     IdVariable = V.IdVariable,
                                                     NombreConcepto = C.Nombre,
                                                     NombreVariable = V.Nombre,
                                                     NumDecimales = C.NumDecimales
                                                 }).FirstOrDefault();

                    double valor = (from SP in Contexto.Suit_Propietario
                                    join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                    join VVS in Contexto.Valor_Variable_Suit on SP.IdSuitPropietario equals VVS.Suit_Propietario.IdSuitPropietario
                                    join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                                    where 
                                        S.Hotel.IdHotel == idHotel &&
                                        V.IdVariable == variable.IdVariable
                                    select VVS.Valor).Sum();

                    ObjetoGenerico resConcepto = new ObjetoGenerico();
                    resConcepto.IdConcepto = itemIdConcepto;
                    resConcepto.NombreConcepto = variable.NombreConcepto;
                    resConcepto.Activo = false;
                    resConcepto.Valor = Math.Round(valor, variable.NumDecimales);
                    resConcepto.Regla = "Ʃ" + variable.NombreVariable;
                    resConcepto.IdHotel = idHotel;
                    resConcepto.IdPropietario = -1;
                    resConcepto.NumDecimales = variable.NumDecimales;

                    listaResConceptoSumatorias.Add(resConcepto);                                        
                }                
            }
        }

        /// <summary>
        /// Liquidacion propietarios - Por cada suite, para evitar redundancia
        /// </summary>
        /// <param name="listaIdSuite"></param>
        public void LiquidarConceptoPropietario(string listaIdSuite)
        {
            string resultIdSuite = listaIdSuite;
            if (listaIdSuite == string.Empty)
            {
                PropietarioBo propietarioBoTmp = new PropietarioBo();
                List<int> listaIdSuiteTmp = (propietarioBoTmp.ObtenerPropietariosConSuite(this.IdHotel)).Select(P => P.IdSuit).ToList();
                resultIdSuite = string.Join(",", listaIdSuiteTmp.Select(x => x.ToString()).ToArray());
            }

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                cnn.Open();

                //Obtengo los propietarios por sus suite
                DataTable tablaPropietarios = Utilities.Select("SELECT Propietario.IdPropietario, Suit.IdSuit, " +
                                                               "(Propietario.NombrePrimero + ' ' + Propietario.NombreSegundo + ' ' + Propietario.ApellidoPrimero + ' ' + Propietario.ApellidoSegundo) as Nombre, " +
                                                               "Propietario.NumIdentificacion, Suit.NumSuit, Suit.NumEscritura, EsRetenedor  " +
                                                               "FROM Suit_Propietario " +
                                                               "INNER JOIN Propietario ON Suit_Propietario.IdPropietario = Propietario.IdPropietario " +
                                                               "INNER JOIN Suit ON Suit_Propietario.IdSuit = Suit.IdSuit " +
                                                               "INNER JOIN Hotel ON Suit.IdHotel = Hotel.IdHotel " +
                                                               "WHERE Hotel.IdHotel = " + this.IdHotel +
                                                               " and Suit_Propietario.EsActivo = 1 AND Suit.IdSuit IN (" + resultIdSuite + ")  order by Nombre", "Suit_Propietario");                

                // Ejecutamos las sumatorias primero, porque van a depender de otras reglas de la liquidacion.
                this.EjecutarSumatoria(this.IdHotel);
                // Solo ejecutamos el metodo 'EjecutarReglas' una sola ves, para obtener el orden de las reglas a ejecutar
                bool bandera = true;
                List<ObjetoGenerico> listaReglasConOrden = null;

                //Recorremos cada propietario
                //foreach (DataRow fila in tablaPropietarios.Rows)
                for (int i = 0; i < tablaPropietarios.Rows.Count-1; i++)
                {
                    //Aqui guardo todos los valores de las reglas por cada propietario
                    this.ListaResultadosConceptos = new List<ObjetoGenerico>();

                    try
                    {
                        if (bandera)
                        {
                            // Ejecutamos todas las reglas por propietario
                            this.EjecutarReglas(int.Parse(tablaPropietarios.Rows[i]["IdPropietario"].ToString()), int.Parse(tablaPropietarios.Rows[i]["IdSuit"].ToString()),
                                                tablaPropietarios.Rows[i]["Nombre"].ToString(), tablaPropietarios.Rows[i]["NumIdentificacion"].ToString(), tablaPropietarios.Rows[i]["NumSuit"].ToString(),
                                                tablaPropietarios.Rows[i]["NumEscritura"].ToString(), (bool)tablaPropietarios.Rows[i]["EsRetenedor"]);
                            bandera = false;
                            listaReglasConOrden = (from r in this.ListaResultadosConceptos
                                                   select new ObjetoGenerico()
                                                   {
                                                       IdConcepto = r.IdConcepto,
                                                       NombreConcepto = r.NombreConcepto,
                                                       Regla = r.Regla,
                                                       NumDecimales = r.NumDecimales,
                                                       Orden = r.Orden,
                                                       OrdenEjecucion = r.OrdenEjecucion,
                                                       IdInformacionEstadistica = r.IdInformacionEstadistica,
                                                       EsRetenedor = r.EsRetenedor
                                                   }).OrderBy(r => r.OrdenEjecucion).ToList();
                        }
                        else
                        {
                            // Ejecutamos todas las reglas por propietario
                            ThreadStart _ts1 = delegate
                            {
                                EjecutarReglasConOrden(int.Parse(tablaPropietarios.Rows[i]["IdPropietario"].ToString()), int.Parse(tablaPropietarios.Rows[i]["IdSuit"].ToString()),
                                  tablaPropietarios.Rows[i]["Nombre"].ToString(), tablaPropietarios.Rows[i]["NumIdentificacion"].ToString(), tablaPropietarios.Rows[i]["NumSuit"].ToString(),
                                  tablaPropietarios.Rows[i]["NumEscritura"].ToString(), (bool)tablaPropietarios.Rows[i]["EsRetenedor"], listaReglasConOrden);
                            };
                            Thread hilo1 = new Thread(_ts1);
                            hilo1.Start();
                            //i = i + 1;
                            
                        }

                    }
                    catch (Exception ex)
                    {
                        int idProp = (int)tablaPropietarios.Rows[i]["IdPropietario"];
                    }                                      

                    //Aqui voy consolidadndo todas las reglas con su valor por cada propietario
                    this.ListaResultadosConceptosFinal.AddRange(this.ListaResultadosConceptos);

                    this.Condicion = "not";
                    this.NumReglas = 0;
                }

                cnn.Close();
            }
        }

        /// <summary>
        /// Obtengo las reglas por propietario y suit
        /// </summary>
        /// <param name="idSuit"></param>
        /// <param name="idPropitario"></param>
        /// <returns></returns>
        private List<ObjetoGenerico> ObtenerReglas(int idSuit, int idPropitario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaReglas = new List<ObjetoGenerico>();
                ConceptoBo conceptoBoTmp = new ConceptoBo();

                //Elimino los conceptos ya liquidados
                List<Concepto> listaConcepto = Contexto.Concepto.Include("Informacion_Estadistica").Where(R => R.Hotel.IdHotel == this.IdHotel && R.NivelConcepto == 2).ToList();
                foreach (ObjetoGenerico itemConcepto in this.ListaResultadosConceptos)
                {
                    listaConcepto.RemoveAll(C => C.IdConcepto == itemConcepto.IdConcepto);
                }
                
                foreach (Concepto itemConcepto in listaConcepto)
                {
                    List<ObjetoGenerico> listaReglasTmp = conceptoBoTmp.ObtenerReglas(itemConcepto.IdConcepto);

                    foreach (Variable itemVariableBase in this.ListaVariablesBase)
                    {
                        listaReglasTmp.RemoveAll(V => V.IdVariable == itemVariableBase.IdVariable);
                    }

                    foreach (Concepto itemConceptoBase in this.ListaConceptoBase)
                    {
                        listaReglasTmp.RemoveAll(V => V.IdConcepto == itemConceptoBase.IdConcepto);
                    }

                    foreach (ObjetoGenerico itemConceptoCalculado in this.ListaResultadosConceptos.Where(R => R.IdSuit == idSuit && R.IdPropietario == idPropitario).ToList())
                    {
                        listaReglasTmp.RemoveAll(V => V.IdConcepto == itemConceptoCalculado.IdConcepto);
                    }

                    if (listaReglasTmp.Count == 0)
                    {
                        ObjetoGenerico concepto = new ObjetoGenerico();
                        concepto.IdConcepto = itemConcepto.IdConcepto;
                        concepto.NombreConcepto = itemConcepto.Nombre;
                        concepto.NumDecimales = itemConcepto.NumDecimales;
                        concepto.Orden = itemConcepto.Orden;
                        concepto.EsRetenedor = itemConcepto.EsRetencionAplicar;
                        concepto.IdInformacionEstadistica = (itemConcepto.Informacion_Estadistica == null) ? -1 : itemConcepto.Informacion_Estadistica.IdInformacionEstadistica;
                        concepto.Regla = conceptoBoTmp.ObtenerRegla(itemConcepto.IdConcepto);

                        this.NumReglas += 1;
                        listaReglas.Add(concepto);
                    }
                }
                
                return listaReglas;
            }
        }

        /// <summary>
        /// Obtengo las reglas de los conceptos por el id del hotel
        /// </summary>
        /// <returns></returns>
        private List<ObjetoGenerico> ObtenerReglas()
        {
            string sql = "select Concepto.Nombre,Concepto.IdConcepto,Concepto.NumDecimales,Concepto.Orden,COALESCE(Concepto.IdInformacionEstadistica, null, 0) IdInformacionEstadistica " +
                         "from Variables_Concepto " +
                         "inner join Concepto on Variables_Concepto.IdConceptoPadre = Concepto.IdConcepto " +
                         "where (Concepto.IdHotel = " + this.IdHotel + ") and " +
                         "(Concepto.NivelConcepto = " + this.Nivel + ") and " +
                         "(Concepto.IdConcepto " + this.Condicion + " in " +
                          "( " +
                            "select IdConceptoPadre " +
                            "from Variables_Concepto " +
                            "inner join Concepto on Concepto.IdConcepto = Variables_Concepto.IdConceptoPadre " +
                            "where (Variables_Concepto.IdConcepto is not null) and " +
                            "      (Concepto.IdHotel = " + this.IdHotel + ") and " +
                            "      (NivelConcepto = " + this.Nivel + ") " +
                          ") " +
                         ") " +
                         "group by Concepto.Nombre,Concepto.IdConcepto,Concepto.NumDecimales,Concepto.Orden,Concepto.IdInformacionEstadistica";

            DataTable tablaReglas = Utilities.Select(sql, "Concepto");

            List<ObjetoGenerico> listaReglas = new List<ObjetoGenerico>();
            ConceptoBo conceptoBoTmp = new ConceptoBo();

            foreach (DataRow fila in tablaReglas.Rows)
            {
                ObjetoGenerico concepto = new ObjetoGenerico();
                concepto.IdConcepto = int.Parse(fila["IdConcepto"].ToString());
                concepto.NombreConcepto = fila["Nombre"].ToString();
                concepto.NumDecimales = int.Parse(fila["NumDecimales"].ToString());
                concepto.Orden = int.Parse(fila["Orden"].ToString());
                concepto.IdInformacionEstadistica = int.Parse(fila["IdInformacionEstadistica"].ToString());
                concepto.Regla = conceptoBoTmp.ObtenerRegla(concepto.IdConcepto.Value);

                listaReglas.Add(concepto);
            }

            this.NumReglas += listaReglas.Count;
            return listaReglas;
        }

        //private List<ObjetoGenerico> ObtenerReglas(int idHotel, string nivel, DateTime fecha)
        //{
        //    string sql = "select distinct Concepto.Nombre,Concepto.IdConcepto,Concepto.NumDecimales,Concepto.Orden " +
        //                 "from Concepto " +
        //                 "inner join Variables_Concepto on Concepto.IdConcepto = Variables_Concepto.IdConceptoPadre " +
        //                 "where Concepto.NivelConcepto = '" + nivel + "' AND " +
        //                        "Concepto.IdHotel = " + idHotel + " AND " +
        //                        "Concepto.IdConcepto " + condicion_uno + " ( " +
        //                 "select distinct Variables_Concepto.IdConceptoPadre from Concepto " +
        //                 "inner join Variables_Concepto on Concepto.IdConcepto = Variables_Concepto.IdConceptoPadre " +
        //                 "where Concepto.NivelConcepto = '" + nivel + "' AND " +
        //                        "Concepto.IdHotel = " + idHotel + " AND " +
        //                        "Variables_Concepto.IdConcepto is not null)";

        //    DataTable tablaReglas = Utilities.Select(sql,"Concepto");

        //    List<ObjetoGenerico> listaReglas = new List<ObjetoGenerico>();
        //    ConceptoBo conceptoBoTmp = new ConceptoBo();

        //    foreach (DataRow fila in tablaReglas.Rows)
        //    {
        //        ObjetoGenerico concepto = new ObjetoGenerico();
        //        concepto.IdConcepto = int.Parse(fila["IdConcepto"].ToString());
        //        concepto.NombreConcepto = fila["Nombre"].ToString();
        //        concepto.NumDecimales = int.Parse(fila["NumDecimales"].ToString());
        //        concepto.Orden = int.Parse(fila["Orden"].ToString());
        //        concepto.Regla = conceptoBoTmp.ObtenerRegla(concepto.IdConcepto.Value);

        //        if (listaResultadosConceptos.Where(C => C.IdConcepto == concepto.IdConcepto).Count() == 0)
        //            listaReglas.Add(concepto);
        //    }

        //    // Borro las reglas de las sumatorias, por que ya fueron ejecutadas.
        //    if (listaResConceptoSumatorias != null)
        //    {
        //        foreach (ObjetoGenerico itemIdConceptoSumatoria in listaResConceptoSumatorias)
        //        {
        //            if (listaReglas.RemoveAll(R => R.IdConcepto == itemIdConceptoSumatoria.IdConcepto) > 0)
        //                numReglas += 1;
        //        }
        //    }

        //    numReglas += listaReglas.Count;
        //    return listaReglas;
        //}

        /// <summary>
        /// Ejecuta las reglas dependiendo del nivel (Hotel,Propietario)
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idPropietario"></param>
        /// <param name="idSuit"></param>
        /// <param name="nombrePropietario"></param>
        /// <param name="fecha"></param>
        /// <param name="numIdentificacion"></param>
        /// <param name="numSuit"></param>
        /// <param name="numEscritura"></param>
        /// <param name="nivel">Reglas: 1 Hotel | 2 Propietario</param>
        private void EjecutarReglas(int idPropietario, int idSuit, string nombrePropietario, 
                                    string numIdentificacion, string numSuit, string numEscritura, bool esRetenedorPropietario)
        {
            int ordenEjecucion = 0;
            do
            {
                // Obtengo las reglas por propietario y suit
                List<ObjetoGenerico> listaReglas = this.ObtenerReglas(idSuit, idPropietario);

                // Recorremos cada regla
                foreach (ObjetoGenerico reglaTmp in listaReglas)
                {
                    ObjetoGenerico objResultado = null;

                    try
                    {
                        // Obtengo el valor de cada regla
                        objResultado = this.ObtenerValorConcepto(reglaTmp.IdConcepto.Value, idPropietario, idSuit,
                                                                 reglaTmp.NombreConcepto, nombrePropietario, reglaTmp.Regla,
                                                                 numIdentificacion, numSuit, numEscritura, reglaTmp.NumDecimales,
                                                                 reglaTmp.Orden, reglaTmp.IdInformacionEstadistica, esRetenedorPropietario, reglaTmp.EsRetenedor);
                    }
                    catch (Exception ex)
                    {
                        int idProp = idPropietario;
                    }

                    if (objResultado != null)
                    {
                        objResultado.OrdenEjecucion = ordenEjecucion;
                        objResultado.Regla = reglaTmp.Regla;
                        objResultado.NumDecimales = reglaTmp.NumDecimales;
                        objResultado.EsRetenedor = reglaTmp.EsRetenedor;
                        objResultado.Orden = reglaTmp.Orden;
                        objResultado.IdConcepto = reglaTmp.IdConcepto;
                        objResultado.IdInformacionEstadistica = reglaTmp.IdInformacionEstadistica;
                        this.ListaResultadosConceptos.Add(objResultado);
                        ordenEjecucion = ordenEjecucion + 1;
                    }
                    else
                        break;
                }

            } while (this.NumReglas < this.NumReglasTotal);

            this.ListaResultadosConceptos = this.ListaResultadosConceptos.OrderBy(C => C.Orden).ToList();
        }


        private void EjecutarReglasConOrden(int idPropietario, int idSuit, string nombrePropietario,
                                    string numIdentificacion, string numSuit, string numEscritura, bool esRetenedorPropietario, List<ObjetoGenerico> listaReglas)
        {
            // Ejecutamos cada regla
            foreach (ObjetoGenerico reglaTmp in listaReglas)
            {
                ObjetoGenerico objResultado = null;

                try
                {
                    if (idPropietario == 6250)
                    {
                        
                    }
                    // Obtengo el valor de cada regla
                    objResultado = this.ObtenerValorConcepto(reglaTmp.IdConcepto.Value, idPropietario, idSuit,
                                                             reglaTmp.NombreConcepto, nombrePropietario, reglaTmp.Regla,
                                                             numIdentificacion, numSuit, numEscritura, reglaTmp.NumDecimales,
                                                             reglaTmp.Orden, reglaTmp.IdInformacionEstadistica, esRetenedorPropietario, reglaTmp.EsRetenedor);
                }
                catch (Exception ex)
                {
                    int idProp = idPropietario;
                }

                if (objResultado != null)
                {
                    this.ListaResultadosConceptos.Add(objResultado);
                }
            }

            this.ListaResultadosConceptos = this.ListaResultadosConceptos.OrderBy(C => C.Orden).ToList();
        }


        private ObjetoGenerico ObtenerValorConcepto(int idConcepto, int idPropitario, int idSuit, string nombreConcepto, 
                                                    string nombrePropietario, string regla, string numIdentificacion, 
                                                    string numSuit, string numEscritura, int numDecimales, int orden, 
                                                    int idInfoEstadistica, bool esRetenedorPropietario, bool esRetenedorAplicar)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariableValor = this.ObtenerVariablesConcepto(idConcepto, idSuit, idPropitario, this.IdHotel);

                #region Formulas Sumatorias
                // Verificamos si el concepto, es una sumatoria para obtener el valor.
                //foreach (ObjetoGenerico item in listaVariableValor.Where(VV => VV.IdConcepto != -1).ToList())
                //{
                //    if (this.ListaResConceptoSumatorias.Where(R => R.IdConcepto == item.IdConcepto).Count() > 0)
                //    {
                //        item.Valor = this.ListaResConceptoSumatorias.Where(R => R.IdConcepto == item.IdConcepto).Select(R => R.Valor).First();
                //    }
                //}
                #endregion

                // Este ciclo obtiene los valores de conceptos calculados anteriormente.
                //foreach (ObjetoGenerico item in listaVariableValor.Where(VV => VV.Valor == -1986).ToList())
                foreach (ObjetoGenerico item in listaVariableValor.ToList())
                {
                    if (this.ListaResultadosConceptos.Where(R => R.IdConcepto == item.IdConcepto).Count() > 0)
                    {
                        ObjetoGenerico varConcepto = this.ListaResultadosConceptos.Where(R => R.IdConcepto == item.IdConcepto && R.IdSuit == idSuit && R.IdPropietario == idPropitario).FirstOrDefault();
                        item.Valor = varConcepto.Valor;
                        item.Nombre = varConcepto.NombreConcepto;
                    }
                }

                StringCollection listaParametros = new StringCollection();
                Dictionary<string, double> mParam = new Dictionary<string, double>(listaVariableValor.Count);

                // Lista de variables para guardarlo en la tabla historial liquidacion
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();

                // Recorro las variables, para armar el diccionario de variable - valor
                foreach (ObjetoGenerico valorVariable in listaVariableValor)
                {
                    if (valorVariable.Nombre == null)
                    {
                        Exception ex = new Exception();
                        ex.Source = "El siguiente usuario es posible que no tenga lleno los valores de sus variables : idPropietario " + idPropitario + " idSuite : " + idSuit;
                        ListaErrores.AppendLine("Es posible que este copropietario no tengas los Valores Variables Asignadas Nombre Copropietario: " + nombrePropietario + " N° Suit: " + numSuit + " N° Escritura: " + numEscritura);

                        Utilities.Log(ex);
                    }

                    if (!mParam.ContainsKey(valorVariable.Nombre))
                    {
                        listaParametros.Add("double " + valorVariable.Nombre);
                        mParam.Add(valorVariable.Nombre, Math.Round(valorVariable.Valor, 10));

                        listaVariables.Add(valorVariable);
                    }                    
                }

                double valor = 0;
                SqlCommand cmd = new SqlCommand("select " + regla + " as Valor", cnn);

                // Agrego el diccionario a la consulta sqlCommand
                foreach (var item in mParam)
                {
                    SqlParameter parameter = cmd.Parameters.Add("@" + item.Key, SqlDbType.Decimal);
                    parameter.Value = item.Value;
                }

                //Ejecuto la regla contra la base de datos
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    try
                    {
                        valor = Convert.ToDouble(rdr[0]);
                    }
                    catch (Exception ex)
                    {
                        ListaErrores.AppendLine("Es posible que este copropietario no tengas los Valores Variables Asignadas Nombre Copropietario: " + nombrePropietario + " N° Suit: " + numSuit + " N° Escritura: " + numEscritura);
                        int idP = idPropitario;
                    }
                }
                rdr.Close();

                if (esRetenedorAplicar)
                {
                    if (!esRetenedorPropietario)
                        valor = 0;
                }

                ObjetoGenerico resConcepto = new ObjetoGenerico();
                resConcepto.IdConcepto = idConcepto;
                resConcepto.IdHotel = this.IdHotel;
                resConcepto.IdPropietario = idPropitario;
                resConcepto.IdSuit = idSuit;
                resConcepto.NombreConcepto = nombreConcepto;
                resConcepto.Valor = Math.Round(valor, numDecimales);
                resConcepto.NombreCompleto = nombrePropietario;
                resConcepto.NumIdentificacion = numIdentificacion;
                resConcepto.NumSuit = numSuit;
                resConcepto.Regla = regla.Replace('@', ' ');
                resConcepto.Fecha = this.Fecha;
                resConcepto.Activo = true;
                resConcepto.NumEscritura = numEscritura;
                resConcepto.NumDecimales = numDecimales;
                resConcepto.Orden = orden;
                resConcepto.IdInformacionEstadistica = idInfoEstadistica;

                resConcepto.ListaVariables = listaVariables;

                return resConcepto;
            }
        }

        /// <summary>
        /// Liquidacion para conceptos de Hotel
        /// </summary>
        /// <returns></returns>
        
        public List<ObjetoGenerico> LiquidarConceptoHotel()
        {
            // Obtengo todos los conceptos de hotel
            this.ListaResultadosConceptos = new List<ObjetoGenerico>();

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                this.ListaAuditoria = new List<ObjetoGenerico>();
                cnn.Open();

                do
                {
                    //Obtengo las reglas de los conceptos de hotel
                    List<ObjetoGenerico> listaReglas = this.ObtenerReglas();

                    // Recorro cada regla para calcular su valor
                    foreach (ObjetoGenerico reglaTmp in listaReglas)
                    {
                        //Calculo valor del concepto
                        ObjetoGenerico objResultado = this.ObtenerValorConceptoHotel(this.IdHotel, reglaTmp.IdConcepto.Value, reglaTmp.NombreConcepto, this.Fecha, reglaTmp.Regla, reglaTmp.NumDecimales);

                        if (objResultado != null)
                            this.ListaResultadosConceptos.Add(objResultado);

                        if (objResultado.IdConcepto == this.IdConcepto)
                            return this.ListaResultadosConceptos.Where(C => C.IdConcepto == this.IdConcepto).ToList();
                    }

                    this.Condicion = "";

                } while (this.NumReglas < this.NumReglasTotal);

                cnn.Close();
                return this.ListaResultadosConceptos;
            }
        }

        /// <summary>
        /// Calcula el valor del concepto de hotel
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idConcepto"></param>
        /// <param name="nombreConcepto"></param>
        /// <param name="fecha"></param>
        /// <param name="regla"></param>
        /// <param name="numDecimal"></param>
        /// <returns></returns>
        private ObjetoGenerico ObtenerValorConceptoHotel(int idHotel, int idConcepto, string nombreConcepto, DateTime fecha,
                                                         string regla, int numDecimal)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                // Obtengo el valor de cada variable que tiene la regla
                List<ObjetoGenerico> listaVariableValor = ((from VV in Contexto.Valor_Variable
                                                            join V in Contexto.Variable on VV.Variable.IdVariable equals V.IdVariable
                                                            join VC in Contexto.Variables_Concepto on V.IdVariable equals VC.Variable.IdVariable
                                                            where VC.Concepto.IdConcepto == idConcepto &&
                                                                  VV.Fecha.Month == fecha.Month &&
                                                                  VV.Fecha.Year == fecha.Year
                                                            select new ObjetoGenerico()
                                                            {
                                                                Nombre = V.Nombre,
                                                                Valor = VV.Valor,
                                                                IdConcepto = -1
                                                            }).Union(
                                                            from VC in Contexto.Variables_Concepto
                                                            join C in Contexto.Concepto on VC.Concepto1.IdConcepto equals C.IdConcepto
                                                            where VC.Concepto.IdConcepto == idConcepto
                                                            select new ObjetoGenerico()
                                                            {
                                                                Nombre = C.Nombre,
                                                                Valor = 0,
                                                                IdConcepto = C.IdConcepto
                                                            })
                                                            .Union(
                                                            from VC in Contexto.Variables_Concepto
                                                            join V in Contexto.Variable on VC.Variable.IdVariable equals V.IdVariable
                                                            where VC.Concepto.IdConcepto == idConcepto && V.Hotel.IdHotel == idHotel && VC.NomClass == "varCO"
                                                            select new ObjetoGenerico()
                                                            {
                                                                Nombre = V.Nombre,
                                                                Valor = V.ValorConstante,
                                                                IdConcepto = -1
                                                            }
                                                            )).Distinct().ToList();

                // Obtengo el valor de alguna variable, si ya esta es un concepto y ya ha sido calculada.
                foreach (ObjetoGenerico item in listaVariableValor.Where(VV => VV.IdConcepto != -1).ToList())
                {
                    if (this.ListaResultadosConceptos.Where(R => R.IdConcepto == item.IdConcepto).Count() > 0)
                    {
                        item.Valor = this.ListaResultadosConceptos.Where(R => R.IdConcepto == item.IdConcepto).Select(R => R.Valor).First();
                    }
                }

                StringCollection listaParametros = new StringCollection();
                Dictionary<string, double> mParam = new Dictionary<string, double>(listaVariableValor.Count);

                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();

                // Armo el diccionario con su nombre y valor
                foreach (ObjetoGenerico valorVariable in listaVariableValor)
                {
                    listaParametros.Add("double " + valorVariable.Nombre);
                    mParam.Add(valorVariable.Nombre, Math.Round(valorVariable.Valor, 10));

                    listaVariables.Add(valorVariable);
                }

                double valor = 0;
                SqlCommand cmd = new SqlCommand("select " + regla + " as Valor", cnn);

                foreach (var item in mParam)
                {
                    SqlParameter parameter = cmd.Parameters.Add("@" + item.Key, SqlDbType.Decimal);
                    parameter.Value = item.Value;
                }

                // Ejecuto la regla contra la base de datos.
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    valor = Convert.ToDouble(rdr[0]);
                }
                rdr.Close();

                //Retorno un objeto con la informacion de la regla ejecutada.
                ObjetoGenerico resConcepto = new ObjetoGenerico();
                resConcepto.IdConcepto = idConcepto;
                resConcepto.NombreConcepto = nombreConcepto;
                resConcepto.Valor = Math.Round(valor, numDecimal);
                resConcepto.Regla = regla;
                resConcepto.IdHotel = idHotel;
                resConcepto.NumDecimales = numDecimal;

                return resConcepto;
            }
        }

        /// <summary>
        /// Aceptar liquidacion Hotel
        /// </summary>
        /// <param name="listaRespuesta"></param>
        /// <param name="listaAuditoria"></param>
        /// <param name="fechaPeriodoLiquidacion"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idConcepto"></param>
        /// <param name="fecha"></param>
        /// <param name="nombreHotel"></param>
        public void AceptarLiquidacionHotel(List<ObjetoGenerico> listaRespuesta, List<ObjetoGenerico> listaAuditoria, 
                                            DateTime fechaPeriodoLiquidacion, int idUsuario, int idConcepto, DateTime fecha, string nombreHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                // Reseteo la liquidacion, para guardar la nueva liquidacion
                if (idConcepto != -1)
                    this.EliminarLiquidacion(idConcepto, fechaPeriodoLiquidacion, listaRespuesta[0].IdHotel);
                else
                    this.EliminarLiquidacion(listaRespuesta.Select(C => C.IdConcepto.Value).ToList(), fechaPeriodoLiquidacion, listaRespuesta[0].IdHotel);                

                // Recorremos cada concepto de la liquidacion y vamos guardando cada valor concepto de hotel
                foreach (ObjetoGenerico itemRespuesta in listaRespuesta)
                {
                    Liquidacion liquidacionTmp = new Liquidacion();                    
                    liquidacionTmp.Valor = itemRespuesta.Valor;
                    liquidacionTmp.Regla = itemRespuesta.Regla;
                    liquidacionTmp.FechaPeriodoLiquidado = fechaPeriodoLiquidacion;
                    liquidacionTmp.FechaElabaoracion = DateTime.Now;
                    liquidacionTmp.EsLiquidacionHotel = true;
                    liquidacionTmp.HotelReference.EntityKey = new EntityKey("ContextoOwner.Hotel", "IdHotel", itemRespuesta.IdHotel);
                    liquidacionTmp.ConceptoReference.EntityKey = new EntityKey("ContextoOwner.Concepto", "IdConcepto", itemRespuesta.IdConcepto);
                    liquidacionTmp.UsuarioReference.EntityKey = new EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);

                    foreach (ObjetoGenerico itemAuditoria in listaAuditoria.Where(A => A.IdConceptoPadre == itemRespuesta.IdConcepto).ToList())
                    {
                        Historial_Liquidacion historialLiquidaciomnTmp = new Historial_Liquidacion();
                        historialLiquidaciomnTmp.NombreVariable = itemAuditoria.Nombre;
                        historialLiquidaciomnTmp.Valor = itemAuditoria.Valor;

                        liquidacionTmp.Historial_Liquidacion.Add(historialLiquidaciomnTmp);
                    }

                    #region auditoria
                    Auditoria auditoriaTmp;

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Hotel: " + nombreHotel + " Periodo : " + fecha.Year + " - " + fecha.Month;
                    auditoriaTmp.NombreTabla = "Liquidacion";
                    auditoriaTmp.Accion = "Insertar";
                    auditoriaTmp.Campo = "Liquidacion Periodo Hotel";
                    auditoriaTmp.Fechahora = DateTime.Now;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    Contexto.AddToLiquidacion(liquidacionTmp);
                    Contexto.SaveChanges();
                }                
            }
        }

        /// <summary>
        /// guarda la liquidacion de los propietarios
        /// </summary>
        /// <param name="listaRespuesta"></param>
        /// <param name="idUsuario"></param>
        /// <param name="fecha"></param>
        /// <param name="esLiquidacionTotal"></param>
        /// <param name="idHotel"></param>
        public void AceptarLiquidacionPropietario(List<ObjetoGenerico> listaRespuesta, int idUsuario, DateTime fecha, bool esLiquidacionTotal, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                //Si la liquidacion fue a todos los propietarios, elimino la liquidacion
                if (esLiquidacionTotal)
                {
                    this.EliminarLiquidacionTotal(fecha, idHotel);
                }
                else //Sino elimino la liquidacion de solo los propietarios que se liquidaron
                {
                    foreach (ObjetoGenerico itemliquidacionEliminar in listaRespuesta)
                    {
                        this.EliminarLiquidacionPorPropietario(itemliquidacionEliminar.IdConcepto.Value,
                                                 itemliquidacionEliminar.Fecha,
                                                 itemliquidacionEliminar.IdPropietario,
                                                 itemliquidacionEliminar.IdSuit,
                                                 itemliquidacionEliminar.IdHotel);
                    }
                }

                // Recorro cada liquidacion y se va guardando
                foreach (ObjetoGenerico itemliquidacion in listaRespuesta)
                {
                    Liquidacion liquidacionTmp = new Liquidacion();
                    liquidacionTmp.Valor = itemliquidacion.Valor;
                    liquidacionTmp.Regla = itemliquidacion.Regla;
                    liquidacionTmp.FechaPeriodoLiquidado = itemliquidacion.Fecha;
                    liquidacionTmp.FechaElabaoracion = DateTime.Now;
                    liquidacionTmp.EsLiquidacionHotel = false;
                    liquidacionTmp.HotelReference.EntityKey = new EntityKey("ContextoOwner.Hotel", "IdHotel", itemliquidacion.IdHotel);
                    liquidacionTmp.ConceptoReference.EntityKey = new EntityKey("ContextoOwner.Concepto", "IdConcepto", itemliquidacion.IdConcepto);
                    liquidacionTmp.UsuarioReference.EntityKey = new EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    liquidacionTmp.SuitReference.EntityKey = new EntityKey("ContextoOwner.Suit", "IdSuit", itemliquidacion.IdSuit);
                    liquidacionTmp.PropietarioReference.EntityKey = new EntityKey("ContextoOwner.Propietario", "IdPropietario", itemliquidacion.IdPropietario);

                    foreach (ObjetoGenerico itemHistorial in itemliquidacion.ListaVariables)
                    {
                        Historial_Liquidacion historialLiquidaciomnTmp = new Historial_Liquidacion();
                        historialLiquidaciomnTmp.NombreVariable = itemHistorial.Nombre;
                        historialLiquidaciomnTmp.Valor = itemHistorial.Valor;

                        liquidacionTmp.Historial_Liquidacion.Add(historialLiquidaciomnTmp);
                    }

                    Contexto.AddToLiquidacion(liquidacionTmp);                    
                }

                #region auditoria
                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Periodo : " + fecha.Year + " - " + fecha.Month;
                auditoriaTmp.NombreTabla = "Liquidacion";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Liquidacion Periodo Propietario";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.SaveChanges();
            }
        }


        /* Reportes Liquidacion */
        public List<Liquidacion> ReporteLiquidacionHotel(int idHotel, DateTime fechaInicio, DateTime fechaHasta)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {

                DataTable dtLiquidacion = Utilities.Select("SELECT C.Nombre,sum(L.Valor) Valor " +
                                                           "FROM Liquidacion L " +
                                                           "inner join Concepto C on L.IdConcepto = C.IdConcepto " +
                                                           "where L.EsLiquidacionHotel = 1 and L.IdHotel = " + idHotel + " and (FechaPeriodoLiquidado >= '" + fechaInicio.ToString("yyyy-MM-dd") + " 00:00:00' and FechaPeriodoLiquidado <= '" + fechaHasta.ToString("yyyy-MM-dd") + " 00:00:00') " +
                                                           "group by C.IdConcepto,C.Nombre", "LiquidacionHotel");

                List<Liquidacion> reporteLiquidacion = new List<Liquidacion>();
                foreach (DataRow row in dtLiquidacion.Rows)
                {
                    Liquidacion oLiquidacion = new Liquidacion();

                    Concepto oConcpeto = new Concepto();
                    oConcpeto.Nombre = row["Nombre"].ToString();

                    oLiquidacion.Concepto = oConcpeto;
                    oLiquidacion.Valor = (double)row["Valor"];

                    reporteLiquidacion.Add(oLiquidacion);
                }

                
                   // Contexto.Liquidacion.
                   //Include("Concepto").
                   //Where(L => L.Hotel.IdHotel == idHotel &&
                   //      L.EsLiquidacionHotel == true &&
                   //      L.FechaPeriodoLiquidado.Month >= fechaInicio.Month &&
                   //      L.FechaPeriodoLiquidado.Year >= fechaInicio.Year &&
                   //      L.FechaPeriodoLiquidado.Month <= fechaHasta.Month &&
                   //      L.FechaPeriodoLiquidado.Year <= fechaHasta.Year)
                   //      .ToList();

                return reporteLiquidacion;
            }
        }

        public List<ObjetoGenerico> ReporteLiquidacionPropietario(int idHotel, DateTime fechaInicio, DateTime fechaHasta, ref int numVariablesPropietarios, int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                cnn.ConnectionString = ((EntityConnection)Contexto.Connection).StoreConnection.ConnectionString;
                
                string condicionPro = string.Empty;
                if (idPropietario != -1)
                    condicionPro = "dbo.Propietario.IdPropietario = " + idPropietario + " and ";

                string sql = "SELECT sum(dbo.Liquidacion.Valor) Valor, " +
                             "dbo.Propietario.IdPropietario, " +
                             "dbo.Propietario.NombrePrimero, " +
                             "dbo.Propietario.NombreSegundo, " +
                             "dbo.Propietario.ApellidoPrimero, " +
                             "dbo.Propietario.ApellidoSegundo, " +
                             "dbo.Propietario.NumIdentificacion, " +
                             "dbo.Suit.IdSuit, " +
                             "dbo.Suit.NumSuit, " +
                             "dbo.Suit.NumEscritura, " +
                             "dbo.Suit.RegistroNotaria, " +
                             "dbo.Concepto.IdConcepto, dbo.Concepto.Nombre, dbo.Concepto.Orden " +
                             "FROM dbo.Liquidacion " +
                             "INNER JOIN dbo.Concepto ON dbo.Liquidacion.IdConcepto = dbo.Concepto.IdConcepto " +
                             "INNER JOIN dbo.Hotel ON dbo.Liquidacion.IdHotel = dbo.Hotel.IdHotel AND dbo.Concepto.IdHotel = dbo.Hotel.IdHotel " +
                             "INNER JOIN dbo.Suit ON dbo.Liquidacion.IdSuit = dbo.Suit.IdSuit AND dbo.Hotel.IdHotel = dbo.Suit.IdHotel " +
                             "INNER JOIN dbo.Usuario ON dbo.Liquidacion.IdUsuario = dbo.Usuario.IdUsuario " +
                             "INNER JOIN dbo.Propietario ON dbo.Liquidacion.IdPropietario = dbo.Propietario.IdPropietario " +
                             "where " +
                             condicionPro +
                             "dbo.Liquidacion.idhotel = " + idHotel + " and " +
                             "dbo.Liquidacion.EsLiquidacionHotel = 0 and " +
                             "(FechaPeriodoLiquidado >= '" + fechaInicio.ToString("yyyy-MM-dd") + " 00:00:00' and FechaPeriodoLiquidado <= '" + fechaHasta.ToString("yyyy-MM-dd") + " 00:00:00') and " +
                             "dbo.Concepto.EsMuestraReporteLiquidacion = 1 and dbo.Propietario.Activo = 1 " +
                             "group by dbo.Propietario.IdPropietario,dbo.Propietario.NombrePrimero,dbo.Propietario.NombreSegundo,dbo.Propietario.ApellidoPrimero, dbo.Propietario.ApellidoSegundo,dbo.Propietario.NumIdentificacion, dbo.Suit.IdSuit,dbo.Suit.NumSuit, dbo.Suit.NumEscritura,dbo.Suit.RegistroNotaria, dbo.Concepto.IdConcepto, dbo.Concepto.Nombre, dbo.Concepto.Orden  " +
                             "order by dbo.Propietario.NombrePrimero, dbo.Propietario.IdPropietario, dbo.Suit.IdSuit, dbo.Concepto.Orden,dbo.Concepto.nombre";

                SqlDataAdapter da = new SqlDataAdapter(sql, this.cnn);
                DataTable tablaReporte = new DataTable();

                da.Fill(tablaReporte);                

                List<ObjetoGenerico> reporteLiquidacion = new List<ObjetoGenerico>();
                int idPropietarioTmp = -1;
                int idSuiteTmp = -1;

                foreach (DataRow fila in tablaReporte.Rows)
                {
                    #region Get Variables Por Propietarios

                    if (fila.RowState != DataRowState.Deleted)
                    {
                        if (!(idPropietarioTmp == (int)fila["IdPropietario"] && idSuiteTmp == (int)fila["IdSuit"]))
                        {
                            idPropietarioTmp = (int)fila["IdPropietario"];
                            idSuiteTmp = (int)fila["IdSuit"];

                            sql = "SELECT " +
                                  "dbo.Propietario.IdPropietario, dbo.Valor_Variable_Suit.Valor,dbo.Propietario.NombrePrimero, dbo.Propietario.NombreSegundo, dbo.Propietario.ApellidoPrimero, " +
                                  "dbo.Propietario.ApellidoSegundo, dbo.Propietario.NumIdentificacion, dbo.Suit.IdSuit, dbo.Suit.NumSuit, dbo.Suit.NumEscritura, dbo.Suit.RegistroNotaria, " +
                                  "dbo.Variable.IdVariable, dbo.Variable.Nombre, 0 AS Orden " +
                                  "FROM dbo.Hotel " +
                                  "INNER JOIN dbo.Suit ON dbo.Hotel.IdHotel = dbo.Suit.IdHotel " +
                                  "INNER JOIN dbo.Suit_Propietario ON dbo.Suit.IdSuit = dbo.Suit_Propietario.IdSuit " +
                                  "INNER JOIN dbo.Valor_Variable_Suit ON dbo.Suit_Propietario.IdSuitPropietario = dbo.Valor_Variable_Suit.IdSuitPropietario " +
                                  "INNER JOIN dbo.Variable ON dbo.Hotel.IdHotel = dbo.Variable.IdHotel AND dbo.Valor_Variable_Suit.IdVariable = dbo.Variable.IdVariable " +
                                  "INNER JOIN dbo.Propietario ON dbo.Suit_Propietario.IdPropietario = dbo.Propietario.IdPropietario " +
                                  "WHERE (dbo.Suit_Propietario.EsActivo = 1) and (dbo.Hotel.IdHotel = " + idHotel + ") and (dbo.Propietario.IdPropietario = " + fila["IdPropietario"].ToString() + ") and (dbo.Suit.IdSuit = " + fila["IdSuit"].ToString() + ")  order by dbo.Variable.Nombre";

                            SqlDataAdapter daVariables = new SqlDataAdapter(sql, this.cnn);
                            DataTable tablaVariables = new DataTable();

                            daVariables.Fill(tablaVariables);

                            numVariablesPropietarios = tablaVariables.Rows.Count;

                            if (numVariablesPropietarios != 3)
                            {

                            }

                            if (tablaVariables.Rows.Count == 0)
                            {
                                foreach (DataRow registrosABorrar in tablaReporte.Select("IdPropietario = " + fila["IdPropietario"].ToString() + " and IdSuit = " + fila["IdSuit"].ToString()))
                                {
                                    registrosABorrar.Delete();
                                }
                                continue;
                            }

                            int ordernTmp = -100;
                            foreach (DataRow filaVariables in tablaVariables.Rows)
                            {
                                ObjetoGenerico oLiquidacionVar = new ObjetoGenerico();
                                oLiquidacionVar.Valor = (double)filaVariables["Valor"];

                                oLiquidacionVar.IdPropietario = (int)filaVariables["IdPropietario"];
                                oLiquidacionVar.PrimeroNombre = filaVariables["NombrePrimero"].ToString();
                                oLiquidacionVar.SegundoNombre = filaVariables["NombreSegundo"].ToString();
                                oLiquidacionVar.PrimerApellido = filaVariables["ApellidoPrimero"].ToString();
                                oLiquidacionVar.SegundoApellido = filaVariables["ApellidoSegundo"].ToString();
                                oLiquidacionVar.NumIdentificacion = filaVariables["NumIdentificacion"].ToString();

                                oLiquidacionVar.IdSuit = (int)filaVariables["IdSuit"];
                                oLiquidacionVar.NumSuit = filaVariables["NumSuit"].ToString();
                                oLiquidacionVar.NumEscritura = filaVariables["NumEscritura"].ToString();
                                oLiquidacionVar.RegistroNotaria = filaVariables["RegistroNotaria"].ToString();

                                oLiquidacionVar.IdConcepto = (int)filaVariables["IdVariable"];
                                oLiquidacionVar.NombreConcepto = filaVariables["Nombre"].ToString();
                                oLiquidacionVar.OrdenConcepto = ordernTmp;

                                reporteLiquidacion.Add(oLiquidacionVar);
                                ordernTmp++;
                            }                            
                        }
                    #endregion

                        ObjetoGenerico oLiquidacion = new ObjetoGenerico();
                        oLiquidacion.Valor = (double)fila["Valor"];

                        oLiquidacion.IdPropietario = (int)fila["IdPropietario"];
                        oLiquidacion.PrimeroNombre = fila["NombrePrimero"].ToString();
                        oLiquidacion.SegundoNombre = fila["NombreSegundo"].ToString();
                        oLiquidacion.PrimerApellido = fila["ApellidoPrimero"].ToString();
                        oLiquidacion.SegundoApellido = fila["ApellidoSegundo"].ToString();
                        oLiquidacion.NumIdentificacion = fila["NumIdentificacion"].ToString();

                        oLiquidacion.IdSuit = (int)fila["IdSuit"];
                        oLiquidacion.NumSuit = fila["NumSuit"].ToString();
                        oLiquidacion.NumEscritura = fila["NumEscritura"].ToString();
                        oLiquidacion.RegistroNotaria = fila["RegistroNotaria"].ToString();

                        oLiquidacion.IdConcepto = (int)fila["IdConcepto"];
                        oLiquidacion.NombreConcepto = fila["Nombre"].ToString();
                        oLiquidacion.OrdenConcepto = (int)fila["Orden"];

                        reporteLiquidacion.Add(oLiquidacion);                        
                    }
                }


                return reporteLiquidacion;
            }
        }

        public List<Liquidacion> ReporteLiquidacionPropietario(int idHotel, DateTime periodoInicio, DateTime periodoFin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Liquidacion> reporteLiquidacion = Contexto.Liquidacion.
                                                       Include("Concepto").
                                                       Include("Hotel").
                                                       Include("Suit").
                                                       Include("Usuario").
                                                       Include("Propietario").
                                                       Where(L => L.Hotel.IdHotel == idHotel &&
                                                             L.EsLiquidacionHotel == false &&
                                                             L.FechaPeriodoLiquidado.Month >= periodoInicio.Month &&
                                                             L.FechaPeriodoLiquidado.Year >= periodoInicio.Year &&
                                                             L.FechaPeriodoLiquidado.Month <= periodoFin.Month &&
                                                             L.FechaPeriodoLiquidado.Year <= periodoFin.Year).
                                                       OrderBy(L => new { L.Propietario.NombrePrimero, L.Propietario.IdPropietario, L.Suit.IdSuit, L.Concepto.Orden }).
                                                       ToList();

                return reporteLiquidacion;
            }
        }

        public List<Liquidacion> ReporteLiquidacionPropietario(int idHotel, DateTime periodo, int idPropietario, int idSuite)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Liquidacion> reporteLiquidacion = Contexto.Liquidacion.
                                                       Include("Concepto").
                                                       Include("Hotel").
                                                       Include("Suit").
                                                       Include("Propietario").
                                                       Include("Usuario").
                                                       Where(L => L.Hotel.IdHotel == idHotel &&
                                                             L.Propietario.IdPropietario == idPropietario &&
                                                             L.Suit.IdSuit == idSuite &&
                                                             L.EsLiquidacionHotel == false &&
                                                             L.FechaPeriodoLiquidado.Month == periodo.Month &&
                                                             L.FechaPeriodoLiquidado.Year == periodo.Year &&
                                                             L.Concepto.EsMuestraReporteLiquidacion == true).
                                                       OrderBy(L => new { L.Propietario.NombrePrimero, L.Propietario.IdPropietario, L.Suit.IdSuit, L.Concepto.Orden }).
                                                       ToList();
                return reporteLiquidacion;
            }
        }

        public List<ObjetoGenerico> ReporteLiquidacionPropietario(int idHotel, DateTime periodoDesde, DateTime periodoHasta, int idPropietario, int idSuite)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                // ojo tratar de ordenar los conceptos por su orden.
                //List<Liquidacion> reporteLiquidacion = Contexto.Liquidacion.
                //                                       Include("Concepto").
                //                                       Include("Hotel").
                //                                       Include("Suit").
                //                                       Include("Propietario").
                //                                       Include("Usuario").
                //                                       Where(L => L.Hotel.IdHotel == idHotel &&
                //                                             L.Propietario.IdPropietario == idPropietario &&
                //                                             L.Suit.IdSuit == idSuite &&
                //                                             L.EsLiquidacionHotel == false &&
                //                                             L.FechaPeriodoLiquidado.Month >= periodoDesde.Month &&
                //                                             L.FechaPeriodoLiquidado.Year >= periodoDesde.Year &&
                //                                             L.FechaPeriodoLiquidado.Month <= periodoHasta.Month &&
                //                                             L.FechaPeriodoLiquidado.Year <= periodoHasta.Year).
                //                                       OrderBy(L => new { L.Propietario.NombrePrimero, L.Propietario.IdPropietario, L.Suit.IdSuit, L.Concepto.Orden }).
                //                                       ToList();

                List<ObjetoGenerico> reporteLiquidacion = (from L in Contexto.Liquidacion
                                                           join P in Contexto.Propietario on L.Propietario.IdPropietario equals P.IdPropietario
                                                           join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                                           join S in Contexto.Suit on L.Suit.IdSuit equals S.IdSuit
                                                           where L.Hotel.IdHotel == idHotel && L.Propietario.IdPropietario == idPropietario && L.Suit.IdSuit == idSuite &&
                                                           L.EsLiquidacionHotel == false &&
                                                           L.FechaPeriodoLiquidado.Month >= periodoDesde.Month &&
                                                           L.FechaPeriodoLiquidado.Year >= periodoDesde.Year &&
                                                           L.FechaPeriodoLiquidado.Month <= periodoHasta.Month &&
                                                           L.FechaPeriodoLiquidado.Year <= periodoHasta.Year
                                                           select new ObjetoGenerico()
                                                           {
                                                               OrdenConcepto = C.Orden,
                                                               Valor = L.Valor,
                                                               IdPropietario = P.IdPropietario,
                                                               PrimeroNombre = P.NombrePrimero,
                                                               SegundoNombre = P.NombreSegundo,
                                                               PrimerApellido = P.ApellidoPrimero,
                                                               SegundoApellido = P.ApellidoSegundo,
                                                               NumIdentificacion = P.NumIdentificacion,
                                                               IdSuit = S.IdSuit,
                                                               NumSuit = S.NumSuit,
                                                               NumEscritura = S.NumEscritura,
                                                               RegistroNotaria = S.RegistroNotaria,
                                                               IdConcepto = C.IdConcepto,
                                                               NombreConcepto = C.Nombre
                                                           })
                                                           .OrderBy(L => new { L.PrimeroNombre, L.IdPropietario, L.IdSuit, L.OrdenConcepto })
                                                           .ToList();
                return reporteLiquidacion;
            }
        }
        
        public List<ObjetoGenerico> ObtenerSuitPorPropietarioEnLiquidacion(int idPropietario)
        {
            //SELECT DISTINCT dbo.Suit.IdSuit, dbo.Suit.NumSuit, dbo.Suit.NumEscritura, dbo.Suit.RegistroNotaria, dbo.Hotel.IdHotel
            //FROM dbo.Liquidacion 
            //INNER JOIN dbo.Suit ON dbo.Liquidacion.IdSuit = dbo.Suit.IdSuit 
            //INNER JOIN dbo.Hotel ON dbo.Suit.IdHotel = dbo.Hotel.IdHotel
            //WHERE (dbo.Liquidacion.IdPropietario = 4973) 
            //order by dbo.Suit.IdSuit, dbo.Suit.NumSuit, dbo.Suit.NumEscritura, dbo.Suit.RegistroNotaria, dbo.Hotel.IdHotel
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = (from L in Contexto.Liquidacion
                                              join S in Contexto.Suit on L.Suit.IdSuit equals S.IdSuit
                                              join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                              where L.Propietario.IdPropietario == idPropietario
                                              select new ObjetoGenerico()
                                              {
                                                  IdSuit = S.IdSuit,
                                                  IdHotel = H.IdHotel,
                                                  NumSuit = S.NumSuit,
                                                  NumEscritura = S.NumEscritura,
                                                  Nombre = "Num. Suite: " + S.NumSuit + " Num. Escritura: " + S.NumEscritura + " Hotel: " + H.Nombre
                                              }).Distinct().ToList();

                foreach (ObjetoGenerico item in lista)
                {
                    item.NumSuit = item.IdSuit + "%" + item.IdHotel;
                }

                return lista;
            }            
        }

        public string ObtenerUrlExtracto(DateTime fecha, int idPropietario, int idSuite)
        {
            try
            {
                using (ContextoOwner Contexto = new ContextoOwner())
                {
                    string urlTmp = string.Empty;
                    urlTmp = (from L in Contexto.Liquidacion
                              where L.Propietario.IdPropietario == idPropietario &&
                              L.Suit.IdSuit == idSuite &&
                              L.EsLiquidacionHotel == false &&
                              L.FechaPeriodoLiquidado.Month >= fecha.Month && L.FechaPeriodoLiquidado.Year >= fecha.Year
                              where L.UrlExtracto != ""
                              select L.UrlExtracto).FirstOrDefault();
                    return urlTmp;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }

        public void GuardarUrlExtracto(DateTime fecha, int idPropietario, int idSuite, string urlFile)
        {
            string sqlUpdate = string.Format("update Liquidacion set UrlExtracto = '{0}' where IdSuit = {1} and IdPropietario = {2} and FechaPeriodoLiquidado = '{3} 00:00:00'", urlFile, idSuite, idPropietario, fecha.ToString("yyyy-MM-dd"));
            Utilities.Insertupdate(sqlUpdate);            
        }
    }
}