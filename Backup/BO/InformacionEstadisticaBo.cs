using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class InformacionEstadisticaBo
    {
        public List<Informacion_Estadistica> VerTodos(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.
                       Informacion_Estadistica.
                       Include("Hotel").
                       Where(I => I.Hotel.IdHotel == idHotel).
                       OrderBy(I => I.Orden).
                       ToList();
            }
        }

        public List<Informacion_Estadistica> VerTodosAcumuladas(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.
                       Informacion_Estadistica.
                       Where(I => I.Hotel.IdHotel == idHotel && I.EsAcumulada == true).
                       ToList();
            }
        }

        public Informacion_Estadistica Obtener(int idInformacionEstadistica)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.
                       Informacion_Estadistica.
                       Include("Hotel").
                       Where(I => I.IdInformacionEstadistica == idInformacionEstadistica).
                       OrderBy(I => I.Nombre).
                       FirstOrDefault();
            }
        }

        public bool EsNombreVariableValido(string nombreVariable, string codigoHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int i = Contexto.Informacion_Estadistica.Where(I => I.Nombre == nombreVariable && I.Hotel.Codigo == codigoHotel).Count();
                return (i == 0) ? false : true;
            }
        }

        public List<ObjetoGenerico> VerDetalleHistorial(int idInformacionEstadistica, int ano)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return (from I in Contexto.Informacion_Estadistica
                        join H in Contexto.Historial_Informacion_Estadistica on I.IdInformacionEstadistica equals H.Informacion_Estadistica.IdInformacionEstadistica
                        where I.IdInformacionEstadistica == idInformacionEstadistica &&
                              H.Fecha.Year == ano
                        select new ObjetoGenerico()
                        {
                            IdInformacionEstadistica = I.IdInformacionEstadistica,
                            IdHistorialInformacionEstadistica = H.IdHistorialInformacionEstadistica,
                            IdHotel = I.Hotel.IdHotel,                            
                            Nombre = I.Nombre,
                            Valor = H.Valor,
                            Fecha = H.Fecha,
                            NombreHotel = I.Hotel.Nombre
                        }).OrderBy(I => I.Fecha).ToList();
            }  
        }

        public double ObtenerValorInfoEstadistica(int idInformacionEstadistica, int ano, int mes)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return (from I in Contexto.Informacion_Estadistica
                        join H in Contexto.Historial_Informacion_Estadistica on I.IdInformacionEstadistica equals H.Informacion_Estadistica.IdInformacionEstadistica
                        where I.IdInformacionEstadistica == idInformacionEstadistica &&
                              H.Fecha.Year == ano &&
                              H.Fecha.Month == mes
                        select H.Valor).FirstOrDefault();
            }
        }

        public void Eliminar(int idInfoEstadistica)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Informacion_Estadistica infoEstadistica = Contexto.Informacion_Estadistica.Where(I => I.IdInformacionEstadistica == idInfoEstadistica).FirstOrDefault();
                Contexto.DeleteObject(infoEstadistica);
                Contexto.SaveChanges();
            }
        }

        public void EliminarHistorial(int idInfoEstadisticaHistorial)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Historial_Informacion_Estadistica infoEstadisticaHistorial = Contexto.Historial_Informacion_Estadistica.Where(H => H.IdHistorialInformacionEstadistica == idInfoEstadisticaHistorial).FirstOrDefault();
                Contexto.DeleteObject(infoEstadisticaHistorial);
                Contexto.SaveChanges();
            }
        }

        public void Guardar(Informacion_Estadistica infoEstadistica)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToInformacion_Estadistica(infoEstadistica);
                Contexto.SaveChanges();
            }
        }

        public void GuardarPlano(List<ObjetoGenerico> listaInfoEstadistica)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHotel = Contexto.Hotel.ToList();

                foreach (ObjetoGenerico itemInfo in listaInfoEstadistica)
                {
                    Informacion_Estadistica infoEstadisticaTmp = Contexto.Informacion_Estadistica.Include("Historial_Informacion_Estadistica").Where(I => I.Hotel.Codigo == itemInfo.Codigo && I.Nombre == itemInfo.NombreVariable).FirstOrDefault();

                    if (infoEstadisticaTmp != null)
                    {
                        Historial_Informacion_Estadistica historialTmp = infoEstadisticaTmp.Historial_Informacion_Estadistica.Where(H => H.Fecha.Year == itemInfo.Fecha.Year && H.Fecha.Month == itemInfo.Fecha.Month).FirstOrDefault();
                        if (historialTmp != null)
                        {
                            historialTmp.Fecha = itemInfo.Fecha;
                            historialTmp.Valor = itemInfo.Valor;
                        }
                        else
                        {
                            Historial_Informacion_Estadistica historialNuevo = new Historial_Informacion_Estadistica();
                            historialNuevo.Fecha = itemInfo.Fecha;
                            historialNuevo.Valor = itemInfo.Valor;
                            infoEstadisticaTmp.Historial_Informacion_Estadistica.Add(historialNuevo);
                        }
                    }
                    else
                    {
                        Informacion_Estadistica infoEstadisticaNuevoTmp = new Informacion_Estadistica();
                        infoEstadisticaNuevoTmp.Nombre = itemInfo.NombreVariable;
                        infoEstadisticaNuevoTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", listaHotel.Where(H => H.Codigo == itemInfo.Codigo).Select(H => H.IdHotel).FirstOrDefault());

                        Historial_Informacion_Estadistica historialNuevo = new Historial_Informacion_Estadistica();
                        historialNuevo.Fecha = itemInfo.Fecha;
                        historialNuevo.Valor = itemInfo.Valor;
                        infoEstadisticaNuevoTmp.Historial_Informacion_Estadistica.Add(historialNuevo);

                        Contexto.AddToInformacion_Estadistica(infoEstadisticaNuevoTmp);
                    }
                }
                Contexto.SaveChanges();
            }
        }

        public void ActualizarValorInfoEstadistica(int idInfoEstadistica, double valor, DateTime fecha, string nombre, bool esAcumulada, short orden, string sufijo, short valorAcumulado)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Informacion_Estadistica infoEstadistica = Contexto.Informacion_Estadistica.Where(I => I.IdInformacionEstadistica == idInfoEstadistica).FirstOrDefault();
                infoEstadistica.Nombre = nombre;
                infoEstadistica.EsAcumulada = esAcumulada;
                infoEstadistica.Orden = orden;
                infoEstadistica.Sufijo = sufijo;
                infoEstadistica.ValorAcumulado = valorAcumulado;

                if (!esAcumulada)
                {
                    Historial_Informacion_Estadistica historialInfoEstadisticaTmp = (from H in Contexto.Historial_Informacion_Estadistica
                                                                                     where H.Informacion_Estadistica.IdInformacionEstadistica == idInfoEstadistica &&
                                                                                           H.Fecha.Year == fecha.Year &&
                                                                                           H.Fecha.Month == fecha.Month
                                                                                     select H).FirstOrDefault();

                    if (historialInfoEstadisticaTmp == null)
                    {
                        Historial_Informacion_Estadistica historialInfoEstadistica = new Historial_Informacion_Estadistica();
                        historialInfoEstadistica.Fecha = fecha;
                        historialInfoEstadistica.Valor = valor;
                        historialInfoEstadistica.Informacion_EstadisticaReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Informacion_Estadistica", "IdInformacionEstadistica", idInfoEstadistica);

                        Contexto.AddToHistorial_Informacion_Estadistica(historialInfoEstadistica);
                    }
                    else
                        historialInfoEstadisticaTmp.Valor = valor;
                }

                Contexto.SaveChanges();
            }
        }

        public List<ObjetoGenerico> Obtener(int idHotel, DateTime fecha, int idPropietario, int idSuite)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariablesInfo = new List<ObjetoGenerico>();

                listaVariablesInfo = (from I in Contexto.Informacion_Estadistica
                                      where I.Hotel.IdHotel == idHotel && I.Orden > 0
                                      select new ObjetoGenerico()
                                      {
                                          IdInformacionEstadistica = I.IdInformacionEstadistica,
                                          Nombre = I.Nombre,
                                          EsVarAcumulada = I.EsAcumulada,
                                          Valor = 0,
                                          Orden = I.Orden,
                                          Sufijo = I.Sufijo,
                                          ValorAcumulado = I.ValorAcumulado
                                      }).OrderBy(I => I.Orden).ToList();

                foreach (ObjetoGenerico itemInfo in listaVariablesInfo.Where(V => V.EsVarAcumulada == false).ToList())
                {
                    itemInfo.Valor = Contexto.Historial_Informacion_Estadistica.
                                     Where(H => H.Informacion_Estadistica.IdInformacionEstadistica == itemInfo.IdInformacionEstadistica &&
                                           H.Fecha.Year == fecha.Year &&
                                           H.Fecha.Month == fecha.Month).
                                     Select(H => H.Valor).
                                     FirstOrDefault();
                }

                foreach (ObjetoGenerico itemInfo in listaVariablesInfo.Where(V => V.EsVarAcumulada == true).ToList())
                {
                    itemInfo.Valor = 0;
                    List<double> listValues = (from C in Contexto.Concepto
                                               join L in Contexto.Liquidacion on C.IdConcepto equals L.Concepto.IdConcepto
                                               where C.Informacion_Estadistica.IdInformacionEstadistica == itemInfo.IdInformacionEstadistica &&
                                                     L.Propietario.IdPropietario == idPropietario &&
                                                     L.Suit.IdSuit == idSuite &&
                                                     L.FechaPeriodoLiquidado.Year >= fecha.Year && L.FechaPeriodoLiquidado.Month >= 1 &&
                                                     L.FechaPeriodoLiquidado.Year <= fecha.Year && L.FechaPeriodoLiquidado.Month <= fecha.Month
                                               select L.Valor).ToList();

                    if (listValues.Count > 0)
                    {
                        itemInfo.Valor = listValues.Sum();
                    }
                }

                return listaVariablesInfo;
            }
        }

        public ObjetoGenerico Obtener2(int idHotel, DateTime fecha, int idInfoEstadistica)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico Info = new ObjetoGenerico();

                Info = (from I in Contexto.Informacion_Estadistica
                        join H in Contexto.Historial_Informacion_Estadistica on I.IdInformacionEstadistica equals H.Informacion_Estadistica.IdInformacionEstadistica
                        where I.Hotel.IdHotel == idHotel && 
                              I.IdInformacionEstadistica == idInfoEstadistica &&
                              H.Fecha.Year == fecha.Year &&
                              H.Fecha.Month == fecha.Month
                        select new ObjetoGenerico()
                        {
                            IdInformacionEstadistica = I.IdInformacionEstadistica,
                            Nombre = I.Nombre,
                            EsVarAcumulada = I.EsAcumulada,
                            Valor = H.Valor
                        }).FirstOrDefault();

                return Info;
            }
        }

        public bool ValidarInfoEstadistica(DateTime fecha, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = (from IE in Contexto.Informacion_Estadistica
                           join HE in Contexto.Historial_Informacion_Estadistica on IE.IdInformacionEstadistica equals HE.Informacion_Estadistica.IdInformacionEstadistica
                           where IE.Hotel.IdHotel == idHotel && 
                                 HE.Fecha.Year == fecha.Year &&
                                 HE.Fecha.Month == fecha.Month
                           select IE).Count();
                return (con == 0) ? false : true;
            }
        }
    }
}
