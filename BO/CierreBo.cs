using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class CierreBo
    {
        /// <summary>
        /// Guarda un cierre de liquidacion
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="observacion"></param>
        /// <param name="idHotel"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public bool Guardar(DateTime fechaInicio, DateTime fechaFin, string observacion, int idHotel, 
                            int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                if (this.ValidarCierre(fechaInicio, idHotel))
                {
                    Cierre cierreTmp = new Cierre();
                    cierreTmp.FechaInicio = fechaInicio;
                    cierreTmp.FechaFin = fechaFin;
                    cierreTmp.Observacion = observacion;
                    cierreTmp.Estado = false;
                    cierreTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                    cierreTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);

                    Contexto.AddToCierre(cierreTmp);
                    Contexto.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Actualiza un cierre, para cambiar el estado del cierre
        /// </summary>
        /// <param name="observacion"></param>
        /// <param name="estado"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idCierre"></param>
        public void Actualizar(string observacion, bool estado, int idUsuario, int idCierre)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Cierre cierreTmp = Contexto.Cierre.Include("Usuario").Where(C => C.IdCierre == idCierre).FirstOrDefault();

                Cierre_Historial cierreHistorial = new Cierre_Historial();
                cierreHistorial.Observacion = cierreTmp.Observacion;
                cierreHistorial.Estado = cierreTmp.Estado;
                cierreHistorial.Fecha = DateTime.Now;
                cierreHistorial.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", cierreTmp.Usuario.IdUsuario);
                cierreHistorial.CierreReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Cierre", "IdCierre", cierreTmp.IdCierre);
                
                Contexto.AddToCierre_Historial(cierreHistorial);

                cierreTmp.Observacion = observacion;
                cierreTmp.Estado = !estado;
                cierreTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);                               

                Contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Valida el estado del cierre segun la fecha y hotel
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public bool ValidarCierre(DateTime fechaInicio, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Cierre cierreTmp = Contexto.Cierre.Where(C => C.FechaInicio.Year == fechaInicio.Year &&
                                                         C.FechaInicio.Month == fechaInicio.Month &&
                                                         C.Hotel.IdHotel == idHotel).FirstOrDefault();
                if (cierreTmp == null)
                    return true;
                else
                    return cierreTmp.Estado;
            }
        }

        /// <summary>
        /// Lista todos los estados de los cierres.
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public List<ObjetoGenerico> ListarTodos(int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = new List<ObjetoGenerico>();
                lista = (from C in Contexto.Cierre
                         join H in Contexto.Hotel on C.Hotel.IdHotel equals H.IdHotel
                         select new ObjetoGenerico()
                         {
                             NombreHotel = H.Nombre,
                             Fecha = C.FechaInicio,
                             FechaSalida = C.FechaFin,
                             Descripcion = C.Observacion,
                             Activo = C.Estado,
                             IdHotel = H.IdHotel,
                             IdCierre = C.IdCierre
                         }).OrderBy(X => X.Fecha).Skip(inicio).Take(fin).ToList();

                foreach (ObjetoGenerico item in lista)
                {
                    item.Periodo = item.Fecha.ToString("dd/MM/yyyy") + "  -  " + item.FechaSalida.ToString("dd/MM/yyyy");
                    item.RutaLogo = (item.Activo) ? "../../img/39.png" : "../../img/37.png";
                }

                return lista;
            }
        }

        /// <summary>
        /// Paginacion
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public int CountListarTodos(int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Cierre.Count();
            }
        }

        /// <summary>
        /// Lista historial de los cierres
        /// </summary>
        /// <param name="idCierre"></param>
        /// <returns></returns>
        public List<ObjetoGenerico> ListaHistorial(int idCierre)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = new List<ObjetoGenerico>();
                lista = (from C in Contexto.Cierre_Historial
                         join U in Contexto.Usuario on C.Usuario.IdUsuario equals U.IdUsuario
                         select new ObjetoGenerico()
                         {
                             Descripcion = C.Observacion,
                             Activo = C.Estado,
                             Nombre = U.Login,
                             Fecha = C.Fecha
                         }).OrderBy(X => X.Fecha).ToList();

                foreach (ObjetoGenerico item in lista)
                {
                    item.RutaLogo = (item.Activo) ? "../../img/39.png" : "../../img/37.png";
                }

                return lista;
            }
        }
    }
}
