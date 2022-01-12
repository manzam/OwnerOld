using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using System.Data;
using Servicios;

namespace BO
{
    public class ConfiguracionCertificadoBo
    {
        public void Guardar(int idHotel, string descripcion, List<int> listaIdConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Certificado certificadoTmp = Contexto.Certificado.Where(C => C.Hotel.IdHotel == idHotel).FirstOrDefault();

                if (certificadoTmp != null)
                {
                    Contexto.DeleteObject(certificadoTmp);
                    Contexto.SaveChanges();
                }

                certificadoTmp = new Certificado();
                certificadoTmp.Descripcion = descripcion;
                certificadoTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);

                for (int i = 0; i < listaIdConcepto.Count; i++)
                {
                    Certificado_Detalle certificadoDetalleTmp = new Certificado_Detalle();
                    certificadoDetalleTmp.ConceptoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", listaIdConcepto[i]);

                    certificadoTmp.Certificado_Detalle.Add(certificadoDetalleTmp);
                }

                Contexto.AddToCertificado(certificadoTmp);
                Contexto.SaveChanges();
            }
        }

        public List<ObjetoGenerico> listaIdsConcepto(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> certificadoTmp = (from C in Contexto.Certificado
                                                        join CD in Contexto.Certificado_Detalle on C.IdCertificado equals CD.Certificado.IdCertificado
                                                        join CO in Contexto.Concepto on CD.Concepto.IdConcepto equals CO.IdConcepto
                                                        where C.Hotel.IdHotel == idHotel
                                                        select new ObjetoGenerico()
                                                        {
                                                            Descripcion = C.Descripcion,
                                                            IdConcepto = CO.IdConcepto
                                                        }).ToList();
                return certificadoTmp;
            }
        }

        public string ObtenerTexto(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Certificado.Where(C => C.Hotel.IdHotel == idHotel).Select(C => C.Descripcion).FirstOrDefault();
            }
        }

        public DataTable CalculoConceptos(int idpropietario, int idHotel, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<int> ids = (from C in Contexto.Certificado
                                 join CD in Contexto.Certificado_Detalle on C.IdCertificado equals CD.Certificado.IdCertificado
                                 join CO in Contexto.Concepto on CD.Concepto.IdConcepto equals CO.IdConcepto
                                 where C.Hotel.IdHotel == idHotel
                                 select CO.IdConcepto).ToList();

                string idsConceptos = String.Join(",", ids.Select(x => x.ToString()).ToArray());

                string sqlCalculo = "select sum(L.Valor) Valor, upper(C.Nombre) Concepto from Liquidacion L " +
                                    "inner join Concepto C on L.IdConcepto = C.IdConcepto " +
                                    "where L.IdHotel = " + idHotel + " and L.IdConcepto in (" + idsConceptos + ") and L.FechaPeriodoLiquidado >= '" + fecha.Year + "-01-01 00:00:00' and " +
                                    "L.FechaPeriodoLiquidado <= '" + fecha.Year + "-12-01 23:59:59' and L.IdPropietario = " + idpropietario + "  and EsLiquidacionHotel = 0 " +
                                    "group by C.Nombre " +
                                    "union " +
                                    "select sum(L.Valor) Valor, upper(C.Nombre) Concepto from Liquidacion L " +
                                    "inner join Concepto C on L.IdConcepto = C.IdConcepto " +
                                    "where L.IdHotel = " + idHotel + " and L.IdConcepto in (" + idsConceptos + ") and L.FechaPeriodoLiquidado >= '" + fecha.Year + "-01-01 00:00:00' and " +
                                    "L.FechaPeriodoLiquidado <= '" + fecha.Year + "-12-01 23:59:59' and EsLiquidacionHotel = 1 " +
                                    "group by C.Nombre " +
                                    "union " +
                                    "select Valor, upper(NombreConcepto) from Informacion_Certificado " +
                                    "where Anio = " + fecha.Year + " and IdHotel = " + idHotel + " and IdPropietario = " + idpropietario;

                DataTable dtCalculos = Utilities.Select(sqlCalculo, "conceptos");
                return dtCalculos;
            }
        }


        public List<Informacion_Certificado> ObtenerConceptos(int idHotel, int anio)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Informacion_Certificado.Where(C => C.Anio == anio && C.Hotel.IdHotel == idHotel).ToList();
            }
        }

        public bool Guardar(List<ObjetoGenerico> listaInfoCertificado)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                try
                {
                    Informacion_Certificado infoCertificadoTmp = null;

                    foreach (ObjetoGenerico itemInfoCertificadoArchivo in listaInfoCertificado)
                    {
                        infoCertificadoTmp = Contexto.Informacion_Certificado.Where(C => C.Anio == itemInfoCertificadoArchivo.Anio &&
                                                                                         C.Hotel.IdHotel == itemInfoCertificadoArchivo.IdHotel &&
                                                                                         C.Propietario.IdPropietario == itemInfoCertificadoArchivo.IdPropietario).FirstOrDefault();
                        if (infoCertificadoTmp != null)
                        {
                            infoCertificadoTmp.Valor = itemInfoCertificadoArchivo.Valor;
                            infoCertificadoTmp.NombreConcepto = itemInfoCertificadoArchivo.NombreConcepto;

                            Contexto.SaveChanges();
                        }
                        else
                        {
                            Informacion_Certificado infoCertificadoNuevoTmp = new Informacion_Certificado();
                            infoCertificadoNuevoTmp.Anio = itemInfoCertificadoArchivo.Anio;
                            infoCertificadoNuevoTmp.Valor = itemInfoCertificadoArchivo.Valor;
                            infoCertificadoNuevoTmp.NombreConcepto = itemInfoCertificadoArchivo.NombreConcepto;
                            infoCertificadoNuevoTmp.HotelReference.EntityKey = new EntityKey("ContextoOwner.Hotel", "IdHotel", itemInfoCertificadoArchivo.IdHotel);
                            infoCertificadoNuevoTmp.PropietarioReference.EntityKey = new EntityKey("ContextoOwner.Propietario", "IdPropietario", itemInfoCertificadoArchivo.IdPropietario);

                            Contexto.AddToInformacion_Certificado(infoCertificadoNuevoTmp);
                        }
                    }

                    Contexto.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
