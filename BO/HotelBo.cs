using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class HotelBo
    {
        public ObjetoGenerico ObtenerHotel(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico hotelTmp = new ObjetoGenerico();
                hotelTmp = (from H in Contexto.Hotel
                            join C in Contexto.Ciudad on H.Ciudad.IdCiudad equals C.IdCiudad
                            join D in Contexto.Departamento on C.Departamento.IdDepartamento equals D.IdDepartamento
                            where H.IdHotel == idHotel
                            select new ObjetoGenerico
                            {
                                IdHotel = H.IdHotel,
                                IdCiudad = C.IdCiudad,
                                IdDepto = D.IdDepartamento,
                                Nombre = H.Nombre,
                                Nit = H.Nit,
                                Direccion = H.Direccion,
                                Correo = H.Correo,
                                RutaLogo = H.Logo,
                                Codigo = H.Codigo,
                                UnidadNegocioHotel = H.UnidadNegocio
                            }).FirstOrDefault();

                return hotelTmp;
            }
        }

        public Hotel Obtener(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel hotelTmp = new Hotel();
                hotelTmp = Contexto.Hotel.Where(H => H.IdHotel == idHotel).FirstOrDefault();

                return hotelTmp;
            }
        }

        public Hotel Obtener(string codigo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel hotelTmp = new Hotel();
                hotelTmp = Contexto.Hotel.Where(H => H.Codigo == codigo).FirstOrDefault();

                return hotelTmp;
            }
        }

        public List<Hotel> VerTodos(int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHoteles = new List<Hotel>();

                listaHoteles = (from HU in Contexto.Hotel_Usuario
                                join H in Contexto.Hotel on HU.Hotel.IdHotel equals H.IdHotel
                                where HU.Usuario.IdUsuario == idUsuario
                                orderby H.Nombre
                                select H).ToList();
                return listaHoteles;
            }
        }

        public List<Hotel> ObtenerHotelPorPerfil(int idUsuario, int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHoteles = new List<Hotel>();

                if (idPerfil == 1)
                    listaHoteles = Contexto.Hotel.ToList();
                else
                    listaHoteles = (from HU in Contexto.Hotel_Usuario
                                    join H in Contexto.Hotel on HU.Hotel.IdHotel equals H.IdHotel
                                    where HU.Usuario.IdUsuario == idUsuario
                                    orderby H.Nombre
                                    select H).ToList();

                return listaHoteles;
            }
        }

        public List<Hotel> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHoteles = new List<Hotel>();
                listaHoteles = Contexto.Hotel.OrderBy(H => H.Nombre).ToList();
                return listaHoteles;
            }
        }

        public List<Hotel> VerTodos(int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHoteles = new List<Hotel>();
                listaHoteles = Contexto.Hotel.OrderBy(H => H.Nombre).OrderBy(H => H.Nombre).Skip(inicio).Take(fin).ToList();
                return listaHoteles;
            }
        }

        public int VerTodosCount(int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Hotel.Count();
            }
        }

        public int Guardar(string nombre, string direccion, string nit, int idCiudad)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel hotelTmp = new Hotel();
                hotelTmp.Nombre = nombre;
                hotelTmp.Direccion = direccion;
                hotelTmp.Nit = nit;

                hotelTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);

                Contexto.AddToHotel(hotelTmp);
                Contexto.SaveChanges();

                return hotelTmp.IdHotel;
            }
        }

        public int Guardar(Hotel hotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToHotel(hotel);
                Contexto.SaveChanges();

                return hotel.IdHotel;
            }
        }

        public void Actualizar(int idHotel, string nombre, string direccion, string nit, 
                               string correo, string codigo, string unidadNegocio, int idCiudad)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel hotelTmp = Contexto.Hotel.Where(H => H.IdHotel == idHotel).FirstOrDefault();
                hotelTmp.Nombre = nombre;
                hotelTmp.Direccion = direccion;
                hotelTmp.Nit = nit;
                hotelTmp.Correo = correo;
                hotelTmp.Codigo = codigo;
                hotelTmp.UnidadNegocio = unidadNegocio;
                hotelTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);

                Contexto.SaveChanges();
            }
        }

        public void GuardarRutaLogo(int idHotel, string rutaLogo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel hotelTmp = Contexto.Hotel.Where(H => H.IdHotel == idHotel).FirstOrDefault();
                hotelTmp.Logo = rutaLogo;
                Contexto.SaveChanges();
            }
        }

        public bool EsNitValido(string nit, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHotel = Contexto.Hotel.Where(H => H.Nit == nit).ToList();

                if (idHotel != -1)
                    listaHotel.RemoveAll(H => H.IdHotel == idHotel);

                return (listaHotel.Count == 0) ? true : false;
            }
        }

        public int ObtenerConsecutivo(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Hotel.Where(H => H.IdHotel == idHotel).Select(H => H.Secuencial).FirstOrDefault();
            }
        }

        public void GuardarConsecutivo(int idHotel, int secuencial)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Hotel hotelTmp = Contexto.Hotel.Where(H => H.IdHotel == idHotel).FirstOrDefault();
                hotelTmp.Secuencial = secuencial;

                Contexto.SaveChanges();
            }
        }

        public bool EsCodigoHotelValido(string codigo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int i = Contexto.Hotel.Where(H => H.Codigo == codigo).Count();
                return (i == 0) ? false : true;
            }
        }
    }
}
