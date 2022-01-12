using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class HotelUsuarioBo
    {
        public void Guardar(int idHotel, int IdUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel_Usuario hotelUsuarioTmp = new Hotel_Usuario();
                hotelUsuarioTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                hotelUsuarioTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", IdUsuario);
                
                Contexto.AddToHotel_Usuario(hotelUsuarioTmp);
                Contexto.SaveChanges();
            }
        }

        public void Guardar(List<Hotel> listaHotel, int IdUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {                
                foreach (Hotel hotelTmp in listaHotel)
                {
                    Hotel_Usuario hotelUsuarioTmp = new Hotel_Usuario();
                    hotelUsuarioTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", hotelTmp.IdHotel);
                    hotelUsuarioTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", IdUsuario);
                    Contexto.AddToHotel_Usuario(hotelUsuarioTmp);
                    Contexto.SaveChanges();
                }                
            }
        }

        public void Guardar(List<int> listaIdHotel, int IdUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                this.Eliminar(IdUsuario);

                foreach (int idHotelTmp in listaIdHotel)
                {
                    Hotel_Usuario hotelUsuarioTmp = new Hotel_Usuario();
                    hotelUsuarioTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotelTmp);
                    hotelUsuarioTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", IdUsuario);
                    Contexto.AddToHotel_Usuario(hotelUsuarioTmp);
                    Contexto.SaveChanges();
                }
            }
        }

        public List<Hotel_Usuario> VerTodos(int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel_Usuario> listaHoteles = Contexto.Hotel_Usuario.Include("Hotel").Where(HU => HU.Usuario.IdUsuario == idUsuario).ToList();
                return listaHoteles;
            }
        }

        public void Eliminar(int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                foreach (Hotel_Usuario hotelUsuarioTmp in Contexto.Hotel_Usuario.Where(HU => HU.Usuario.IdUsuario == idUsuario).ToList())
                {
                    Contexto.DeleteObject(hotelUsuarioTmp);
                    Contexto.SaveChanges();
                }
            }
        }
    }
}
