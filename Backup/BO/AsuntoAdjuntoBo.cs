using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class AsuntoAdjuntoBo
    {
        /// <summary>
        /// Obtiene un asunto de correo por hotel, trae informacion del adjuntado.
        /// </summary>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public Asunto_Correo Obtener(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Asunto_Correo.Include("Asunto_Adjunto").Where(AC => AC.Hotel.IdHotel == idHotel).FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene la ruta del archivo adjunto, filtrando por hotel y por fecha.
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string ObtenerArchivoAdjunto(int idHotel, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                //List<DM.Asunto_Correo> listaCorreo = Contexto.Asunto_Correo.ToList();
                //List<DM.Asunto_Adjunto> listaAdjuntos = Contexto.Asunto_Adjunto.ToList();
                //string ss = "";
                //foreach (DM.Asunto_Adjunto adjunto in listaAdjuntos)
                //{
                //    ss += adjunto.AdjuntoRuta + "\r\n";
                //}

                //object o = (from AC in Contexto.Asunto_Correo
                // join AA in Contexto.Asunto_Adjunto on AC.IdAsuntoCorreo equals AA.Asunto_Correo.IdAsuntoCorreo
                // where AC.Hotel.IdHotel == idHotel && AA.Fecha.Year == fecha.Year && AA.Fecha.Month == fecha.Month
                // select AA.AdjuntoRuta);

                //string aRetornar = (from AC in Contexto.Asunto_Correo
                // join AA in Contexto.Asunto_Adjunto on AC.IdAsuntoCorreo equals AA.Asunto_Correo.IdAsuntoCorreo
                // where AC.Hotel.IdHotel == idHotel && AA.Fecha.Year == fecha.Year && AA.Fecha.Month == fecha.Month
                // select AA.AdjuntoRuta).FirstOrDefault();

                return (from AC in Contexto.Asunto_Correo
                        join AA in Contexto.Asunto_Adjunto on AC.IdAsuntoCorreo equals AA.Asunto_Correo.IdAsuntoCorreo
                        where AC.Hotel.IdHotel == idHotel && AA.Fecha.Year == fecha.Year && AA.Fecha.Month == fecha.Month
                        select AA.AdjuntoRuta).FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el texto ó la observación adjunto por hotel
        /// </summary>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public string ObtenerTextoAdjunto(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return (from AC in Contexto.Asunto_Correo
                        where AC.Hotel.IdHotel == idHotel
                        select AC.Asunto).FirstOrDefault();
            }
        }

        /// <summary>
        /// Guarda ó actualiza, validando si ya existe un registro adjunto para ese hotel.
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="asunto"></param>
        /// <param name="rutaAdjunto"></param>
        /// <param name="fecha"></param>
        public void GuardarActualizar(int idHotel, string asunto, string rutaAdjunto, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Asunto_Correo asuntoCorreoTmp = Contexto.Asunto_Correo.Include("Asunto_Adjunto").Where(AC => AC.Hotel.IdHotel == idHotel).FirstOrDefault();

                if (asuntoCorreoTmp == null)
                {
                    asuntoCorreoTmp = new Asunto_Correo();
                    asuntoCorreoTmp.Asunto = asunto;
                    asuntoCorreoTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);

                    asuntoCorreoTmp.Asunto_Adjunto.Add(new Asunto_Adjunto() { Fecha = fecha, AdjuntoRuta = rutaAdjunto });
                    Contexto.AddToAsunto_Correo(asuntoCorreoTmp);
                }
                else
                {
                    asuntoCorreoTmp.Asunto = asunto;

                    if (rutaAdjunto != string.Empty)
                    {
                        Asunto_Adjunto adjunto = Contexto.Asunto_Adjunto.Where(AA => AA.Fecha.Year == fecha.Year && AA.Fecha.Month == fecha.Month && AA.Asunto_Correo.IdAsuntoCorreo == asuntoCorreoTmp.IdAsuntoCorreo).FirstOrDefault();

                        if (adjunto == null)
                            asuntoCorreoTmp.Asunto_Adjunto.Add(new Asunto_Adjunto() { Fecha = fecha, AdjuntoRuta = rutaAdjunto });
                        else
                            adjunto.AdjuntoRuta = rutaAdjunto;
                    }
                }

                Contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene una lista de adjunots, filtrando por hotel y fecha.
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<Asunto_Adjunto> ObtenerAdjuntosPorHotel(int idHotel, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return (from AC in Contexto.Asunto_Correo
                        join AA in Contexto.Asunto_Adjunto on AC.IdAsuntoCorreo equals AA.Asunto_Correo.IdAsuntoCorreo
                        where AC.Hotel.IdHotel == idHotel && AA.Fecha.Year == fecha.Year && AA.Fecha.Month == fecha.Month
                        select AA).ToList();
            }
        }

        public void EliminarAdjunto(int id)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Asunto_Adjunto oAsunto_Adjunto = Contexto.Asunto_Adjunto.Where(A => A.IdAsuntoAdjunto == id).FirstOrDefault();
                Contexto.DeleteObject(oAsunto_Adjunto);
                Contexto.SaveChanges();
            }
        }
    }
}
