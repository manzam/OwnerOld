using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class ReservasBo
    {
        public void Guardar(int idUsuario, int idSuit, int numAdultos, int numNinos, DateTime fechaLlegada, DateTime fechaSalida, string observacion)
        {
            //using (ContextoOwner Contexto = new ContextoOwner())
            //{
            //    Reservas reservasTmp = new Reservas();
            //    reservasTmp.NumAdultos = numAdultos;
            //    reservasTmp.NumNinos = numNinos;
            //    reservasTmp.FechaLlegada = fechaLlegada;
            //    reservasTmp.FechaSalida = fechaSalida;
            //    reservasTmp.FechaReserva = DateTime.Now;
            //    reservasTmp.Observacion = observacion;
            //    reservasTmp.Usuario_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario_Propietario", "IdUsuarioPropietario", idUsuario);
            //    reservasTmp.SuitReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit", "IdSuit", idSuit);

            //    Contexto.AddToReservas(reservasTmp);
            //    Contexto.SaveChanges();
            //}
        }

        //public List<ObjetoGenerico> VerTodos(int IdUsuarioPropietario)
        //{
            //using (ContextoOwner Contexto = new ContextoOwner())
            //{
            //    List<ObjetoGenerico> listaReservas = new List<ObjetoGenerico>();
            //    listaReservas = (from R in Contexto.Reservas
            //                     join S in Contexto.Suit on R.Suit.IdSuit equals S.IdSuit
            //                     join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
            //                     join C in Contexto.Ciudad on H.Ciudad.IdCiudad equals C.IdCiudad
            //                     where R.Usuario_Propietario.IdUsuarioPropietario == IdUsuarioPropietario
            //                     select new ObjetoGenerico()
            //                     {
            //                         IdReserva = R.IdReserva,
            //                         NombreHotel = H.Nombre,
            //                         NombreCiudad = C.Nombre,
            //                         NumSuit = S.NumSuit,
            //                         NumAdultos = R.NumAdultos,
            //                         NumNinos = R.NumNinos,
            //                         Descripcion = R.Observacion,
            //                         Fecha = R.FechaReserva,
            //                         FechaLlegada = R.FechaLlegada,
            //                         FechaSalida = R.FechaSalida
            //                     }).ToList();
            //    return listaReservas;
            //}
        //}
    }
}
