using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class HistorialEstadiasBo
    {
        public List<ObjetoGenerico> VerTodos(int idSuit, int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaHistorial = new List<ObjetoGenerico>();

                listaHistorial = (from H in Contexto.Historial_Estadias
                                  join P in Contexto.Propietario on H.Propietario.IdPropietario equals P.IdPropietario
                                  join U in Contexto.Usuario on H.Usuario.IdUsuario equals U.IdUsuario
                                  where P.IdPropietario == idPropietario && H.Suit.IdSuit == idSuit
                                  select new ObjetoGenerico()
                                  {
                                      FechaLlegada = H.FechaLlegada,
                                      FechaSalida = H.FechaSalida,
                                      Descripcion = H.Observacion,
                                      Nombre = U.Login
                                  }).OrderBy(H => H.FechaLlegada).ToList();

                return listaHistorial;
            }
        }

        public void Guardar(DateTime fechaLlegada, DateTime fechaSalida, string observacion,int idSuit, int idPropietario, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Historial_Estadias historialEstadiasTmp = new Historial_Estadias();
                historialEstadiasTmp.FechaLlegada = fechaLlegada;
                historialEstadiasTmp.FechaSalida = fechaSalida;
                historialEstadiasTmp.Observacion = observacion;
                historialEstadiasTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                historialEstadiasTmp.PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Propietario", "IdPropietario", idPropietario);
                historialEstadiasTmp.SuitReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit", "IdSuit", idSuit);

                Contexto.AddToHistorial_Estadias(historialEstadiasTmp);
                Contexto.SaveChanges();
            }
        }

        public List<ObjetoGenerico> ObtenerHistorial(int ano, int idSuit, int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaHistorial = new List<ObjetoGenerico>();

                listaHistorial = (from H in Contexto.Historial_Estadias
                                  join P in Contexto.Propietario on H.Propietario.IdPropietario equals P.IdPropietario
                                  join U in Contexto.Usuario on H.Usuario.IdUsuario equals U.IdUsuario
                                  where P.IdPropietario == idPropietario && H.Suit.IdSuit == idSuit && 
                                        H.FechaSalida.Year == ano && H.FechaLlegada.Year == ano
                                  select new ObjetoGenerico()
                                  {
                                      FechaLlegada = H.FechaLlegada,
                                      FechaSalida = H.FechaSalida,
                                      Descripcion = H.Observacion,
                                      Nombre = U.Login
                                  }).OrderBy(H => H.FechaLlegada).ToList();

                return listaHistorial;
            }
        }

    }
}
