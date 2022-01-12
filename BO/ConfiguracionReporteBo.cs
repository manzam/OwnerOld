using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class ConfiguracionReporteBo
    {
        public List<ObjetoGenerico> ListaVariables(int idHotel, int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = new List<ObjetoGenerico>();
                lista = ((from V in Contexto.Variable
                          where V.Hotel.IdHotel == idHotel
                          select new ObjetoGenerico()
                          {
                              IdVariable = V.IdVariable,
                              Nombre = V.Nombre,
                              Tipo = V.Tipo
                          }).Union(
                        from C in Contexto.Concepto
                        where C.Hotel.IdHotel == idHotel
                        select new ObjetoGenerico()
                        {
                            IdVariable = C.IdConcepto,
                            Nombre = C.Nombre,
                            Tipo = "C"
                        })).OrderBy(O => O.Nombre).Skip(inicio).Take(fin).ToList();
                return lista;
            }
        }

        public int ListaVariablesCount(int idHotel, int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = 0;
                con=((from V in Contexto.Variable
                          where V.Hotel.IdHotel == idHotel
                          select new ObjetoGenerico()
                          {
                              IdVariable = V.IdVariable,
                              Nombre = V.Nombre,
                              Tipo = V.Tipo
                          }).Union(
                        from C in Contexto.Concepto
                        where C.Hotel.IdHotel == idHotel
                        select new ObjetoGenerico()
                        {
                            IdVariable = C.IdConcepto,
                            Nombre = C.Nombre,
                            Tipo = "C"
                        })).OrderBy(O => O.Nombre).Count();

                return con;
            }
        }

        public List<Reporte> ListaReporte()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Reporte> lista = new List<Reporte>();
                lista = Contexto.Reporte.ToList();

                return lista;
            }
        }

        public void EliminarVariableReporte(int idReporteDetalle)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Reporte_Detalle reporteDetalleTmp = Contexto.
                                                    Reporte_Detalle.
                                                    Where(D => D.IdReporteDetalle == idReporteDetalle).
                                                    FirstOrDefault();
                if (reporteDetalleTmp != null)
                {
                    Contexto.DeleteObject(reporteDetalleTmp);
                    Contexto.SaveChanges();
                }
            }
        }

        public List<ObjetoGenerico> ListaDetalleReporte(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = new List<ObjetoGenerico>();

                lista = ((from D in Contexto.Reporte_Detalle
                          join V in Contexto.Variable on D.Variable.IdVariable equals V.IdVariable
                          where D.Hotel.IdHotel == idHotel && V.Activo == true
                          select new ObjetoGenerico()
                          {
                              IdReporteDetalle = D.IdReporteDetalle,
                              IdVariable = V.IdVariable,
                              Nombre = V.Nombre,
                              Orden = D.Orden.Value,
                              Tipo = V.Tipo
                          }).Union(
                          from D in Contexto.Reporte_Detalle
                          join C in Contexto.Concepto on D.Concepto.IdConcepto equals C.IdConcepto
                          where D.Hotel.IdHotel == idHotel
                          select new ObjetoGenerico()
                          {
                              IdReporteDetalle = D.IdReporteDetalle,
                              IdVariable = C.IdConcepto,
                              Nombre = C.Nombre,
                              Orden = D.Orden.Value,
                              Tipo = "C"
                          })
                          ).OrderBy(O => O.Orden).ToList();

                return lista;
            }
        }

        public void Guardar(List<ObjetoGenerico> listaDetalle, int idReporte, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                // Se borra todos sus detalles
                List<Reporte_Detalle> listaDetalleTmp = Contexto.Reporte_Detalle.Where(R => R.Hotel.IdHotel == idHotel && R.Reporte.IdReporte == idReporte).ToList();
                foreach (Reporte_Detalle itemDetalle in listaDetalleTmp)
                {
                    Contexto.DeleteObject(itemDetalle);
                }

                Contexto.SaveChanges();

                // Insertamos todo los detalles de nuevo
                foreach (ObjetoGenerico itemDetalle in listaDetalle)
                {
                    Reporte_Detalle reporteDetalle = new Reporte_Detalle();
                    reporteDetalle.Orden = itemDetalle.Orden;
                    reporteDetalle.Tipo = itemDetalle.Tipo;
                    reporteDetalle.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                    reporteDetalle.ReporteReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Reporte", "IdReporte", idReporte);

                    if (itemDetalle.Tipo == "P" || itemDetalle.Tipo == "H")
                        reporteDetalle.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", itemDetalle.IdVariable);
                    else
                        reporteDetalle.ConceptoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", itemDetalle.IdVariable);

                    Contexto.AddToReporte_Detalle(reporteDetalle);
                }
                
                Contexto.SaveChanges();
            }
        }

        public List<Reporte_Detalle> ObtenerOrden(int idReporte, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Reporte_Detalle> lista = Contexto.Reporte_Detalle.Include("Variable").Include("Concepto").Where(R => R.Reporte.IdReporte == idReporte && R.Hotel.IdHotel == idHotel).OrderBy(R => R.Orden).ToList();
                return lista;
            }
        }

        public List<ObjetoGenerico> ListaGrupo()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = new List<ObjetoGenerico>();
                lista = (from R in Contexto.Reporte_Grupo
                         select new ObjetoGenerico()
                         {
                             Id = R.IdGrupo,
                             Nombre = R.Nombre,
                             Orden = R.Orden
                         }).OrderBy(O => O.Orden).ToList();
                return lista;
            }
        }

        public List<ObjetoGenerico> ListaItemGrupo(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = new List<ObjetoGenerico>();
                lista = ((from G in Contexto.Reporte_Grupo
                          join RG in Contexto.Reporte_Grupo_Detalle on G.IdGrupo equals RG.Reporte_Grupo.IdGrupo
                          join V in Contexto.Variable on RG.Variable.IdVariable equals V.IdVariable
                          where RG.Hotel.IdHotel == idHotel
                          select new ObjetoGenerico()
                          {
                              IdReporteGrupoDetalle = RG.IdReporteGrupoDetalle,
                              IdVariable = V.IdVariable,
                              Nombre = V.Nombre,
                              Descripcion = G.Nombre,
                              Tipo = RG.Tipo,
                              Orden = G.Orden
                          }).Union(
                         from G in Contexto.Reporte_Grupo
                         join RG in Contexto.Reporte_Grupo_Detalle on G.IdGrupo equals RG.Reporte_Grupo.IdGrupo
                         join C in Contexto.Concepto on RG.Concepto.IdConcepto equals C.IdConcepto
                         where RG.Hotel.IdHotel == idHotel
                         select new ObjetoGenerico()
                         {
                             IdReporteGrupoDetalle = RG.IdReporteGrupoDetalle,
                             IdVariable = C.IdConcepto,
                             Nombre = C.Nombre,
                             Descripcion = G.Nombre,
                             Tipo = RG.Tipo,
                             Orden = G.Orden
                         }
                         )).OrderBy(G => G.Orden).ToList();
                return lista;
            }
        }

        public void EliminarItemGrupo(int IdReporteGrupoDetalle)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Reporte_Grupo_Detalle item = Contexto.Reporte_Grupo_Detalle.Where(R => R.IdReporteGrupoDetalle == IdReporteGrupoDetalle).FirstOrDefault();
                Contexto.DeleteObject(item);
                Contexto.SaveChanges();
            }
        }

        public void GuardarItemGrupo(int idVariable, string tipo, int idGrupo, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = 0;
                Reporte_Grupo_Detalle grupoDetalle = new Reporte_Grupo_Detalle();
                grupoDetalle.Tipo = tipo;
                grupoDetalle.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                grupoDetalle.Reporte_GrupoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Reporte_Grupo", "IdGrupo", idGrupo);

                if (tipo == "P" || tipo == "H")
                {
                    con = Contexto.Reporte_Grupo_Detalle.Where(R => R.Variable.IdVariable == idVariable && R.Reporte_Grupo.IdGrupo == idGrupo).Count();
                    grupoDetalle.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", idVariable);
                }
                else
                {
                    con = Contexto.Reporte_Grupo_Detalle.Where(R => R.Concepto.IdConcepto == idVariable && R.Reporte_Grupo.IdGrupo == idGrupo).Count();
                    grupoDetalle.ConceptoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", idVariable);
                }


                if (con == 0)
                {
                    Contexto.AddToReporte_Grupo_Detalle(grupoDetalle);
                    Contexto.SaveChanges();
                }
            }
        }

        public void GuardarGrupo(string nombreGrupo, int orden)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Reporte_Grupo reporteGrupo = new Reporte_Grupo();
                reporteGrupo.Nombre = nombreGrupo;
                reporteGrupo.Orden = orden;

                Contexto.AddToReporte_Grupo(reporteGrupo);
                Contexto.SaveChanges();
            }
        }
        public void EditarGrupo(string nombreGrupo, int orden, int idGrupo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Reporte_Grupo reporteGrupo = Contexto.Reporte_Grupo.Where(R => R.IdGrupo == idGrupo).FirstOrDefault();
                reporteGrupo.Nombre = nombreGrupo;
                reporteGrupo.Orden = orden;

                Contexto.SaveChanges();
            }
        }
        public void EliminarGrupo(int idGrupo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Reporte_Grupo reporteGrupo = Contexto.Reporte_Grupo.Where(R => R.IdGrupo == idGrupo).FirstOrDefault();
                Contexto.DeleteObject(reporteGrupo);
                Contexto.SaveChanges();
            }
        }

        public short ObtenerTipoExtracto(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Extracto.Where(E => E.Hotel.IdHotel == idHotel).Select(E => E.TipoExtracto).FirstOrDefault();
            }
        }
    }
}
