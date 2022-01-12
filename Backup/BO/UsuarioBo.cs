using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using Servicios;
using System.Web;

namespace BO
{
    public class UsuarioBo
    {
        HotelBo hotelBo;

        public ObjetoGenerico Autenticar(string login, string pass)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string passTmp = Utilities.EncodePassword(string.Concat(login, pass));

                ObjetoGenerico user = (from U in Contexto.Usuario
                                       join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                       where U.Login == login && 
                                       U.Pass == passTmp && 
                                       U.Activo == true
                                       select new ObjetoGenerico()
                                       {
                                           Id = U.IdUsuario,
                                           IdPerfil = P.IdPerfil,
                                           Nombre = U.Nombre,
                                           Apellido = U.Apellido,
                                           PrimeroNombre = U.Nombre,
                                           PrimerApellido = U.Apellido,
                                           NumIdentificacion = U.Identificacion,
                                           Login = U.Login,
                                           Correo = U.Correo,
                                           Activo = U.Activo,
                                           Cambio = U.EsCambio,
                                           Tipo = "U"
                                       }).FirstOrDefault();
                return user;
            }
        }

        public ObjetoGenerico Autenticar(string login)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico user = (from U in Contexto.Usuario
                                       join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                       where U.Login == login && U.Activo == true
                                       select new ObjetoGenerico()
                                       {
                                           Id = U.IdUsuario,
                                           IdPerfil = P.IdPerfil,
                                           Nombre = U.Nombre,
                                           Apellido = U.Apellido,
                                           NumIdentificacion = U.Identificacion,
                                           Login = U.Login,
                                           Correo = U.Correo,
                                           Activo = U.Activo,
                                           Cambio = U.EsCambio,
                                           Tipo = "U"
                                       }).FirstOrDefault();
                return user;
            }
        }

        public List<ObjetoGenerico> VerTodos(int idPerfilPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropiestario = new List<ObjetoGenerico>();
                listaPropiestario = (from U in Contexto.Usuario
                                     join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                     where U.Perfil.IdPerfil != idPerfilPropietario
                                     select new ObjetoGenerico()
                                     {
                                         IdUsuario = U.IdUsuario,
                                         IdPerfil = P.IdPerfil,
                                         Nombre = U.Nombre,
                                         Apellido = U.Apellido,
                                         NumIdentificacion = U.Identificacion,
                                         Login = U.Login,
                                         Correo = U.Correo,
                                         Activo = U.Activo,
                                         Cambio = U.EsCambio
                                     }).OrderBy(U => U.Nombre).ToList();
                return listaPropiestario;
            }
        }

        public void Guardar(string nombre, string apellido, string identificacion, string correo,
                            string telefono1, string telefono2, bool activo, int idPerfil, 
                            List<int> listaIdHotel, string login, string clave, int idUsuario, string nombrePerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Usuario usuarioTmp = new Usuario();
                usuarioTmp.Nombre = nombre;
                usuarioTmp.Apellido = apellido;
                usuarioTmp.Identificacion = identificacion;
                usuarioTmp.Correo = correo;
                usuarioTmp.Telefono_1 = telefono1;
                usuarioTmp.Telefono_2 = telefono2;
                usuarioTmp.Activo = activo;
                usuarioTmp.EsCambio = true;
                usuarioTmp.Login = login;
                usuarioTmp.Pass = clave;

                if (idPerfil != -1)
                    usuarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", idPerfil);
                
                Contexto.AddToUsuario(usuarioTmp);               

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Nombre;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Apellido;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Apellido";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Identificacion;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Identificacion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombrePerfil;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Perfil";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = (usuarioTmp.Activo) ? "Si" : "No";
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Activo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);                

                hotelBo = new HotelBo();
                string nombreHotel = string.Empty;
                foreach (int itemIdHoteles in listaIdHotel)
                {
                    Hotel hotel = hotelBo.Obtener(itemIdHoteles);
                    nombreHotel += hotel.Nombre + ",";
                }
                nombreHotel = nombreHotel.TrimEnd(new char[] { ',' });

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Permiso Hotel";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                #endregion

                Contexto.SaveChanges();

                HotelUsuarioBo hotelusuarioBoTmp = new HotelUsuarioBo();
                hotelusuarioBoTmp.Guardar(listaIdHotel, usuarioTmp.IdUsuario);                
            }
        }

        public void Actualizar(int idUsuario, string nombre, string apellido, string identificacion, string correo,
                               string telefono1, string telefono2, bool activo, int idPerfil, List<int> listaIdHotel, int idUsuarioSesion)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Usuario usuarioTmp = Contexto.Usuario.Include("Perfil").Where(U => U.IdUsuario == idUsuario).FirstOrDefault();
                
                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Nombre;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuarioSesion);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Apellido;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Apellido";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Identificacion;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Identificacion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = (usuarioTmp.Activo) ? "Si" : "No";
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Activo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                usuarioTmp.Nombre = nombre;
                usuarioTmp.Apellido = apellido;
                usuarioTmp.Identificacion = identificacion;
                usuarioTmp.Correo = correo;
                usuarioTmp.Telefono_1 = telefono1;
                usuarioTmp.Telefono_2 = telefono2;
                usuarioTmp.Activo = activo;

                if (idPerfil != -1)
                    usuarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", idPerfil);
                else
                {
                    usuarioTmp.Perfil = null;
                }

                Contexto.SaveChanges();

                HotelUsuarioBo hotelusuarioBoTmp = new HotelUsuarioBo();
                hotelusuarioBoTmp.Guardar(listaIdHotel, usuarioTmp.IdUsuario);
            }
        }

        public void RestablecerPass(int idUsuario, string pass, int idUsuarioSesion)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Usuario usuarioTmp = Contexto.Usuario.Where(U => U.IdUsuario == idUsuario).FirstOrDefault();
                usuarioTmp.Pass = pass;
                usuarioTmp.EsCambio = true;

                #region auditoria
                Auditoria auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Nombre + " " + usuarioTmp.Apellido;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "RestablecerContraseña";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuarioSesion);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.SaveChanges();
            }
        }

        public void ResetPass(string usuario, string clave)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Usuario usuarioTmp = Contexto.Usuario.Where(U => U.Login == usuario).FirstOrDefault();
                usuarioTmp.Pass = clave;
                usuarioTmp.EsCambio = true;

                #region auditoria
                Auditoria auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = usuarioTmp.Nombre + " " + usuarioTmp.Apellido;
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "RestablecerContraseña";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", usuarioTmp.IdUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.SaveChanges();
            }
        }

        public ObjetoGenerico Obtener(int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico usuraio = (from U in Contexto.Usuario
                                          where U.IdUsuario == idUsuario
                                          select new ObjetoGenerico()
                                          {
                                              Nombre = U.Nombre,
                                              Apellido = U.Apellido,
                                              NumIdentificacion = U.Identificacion,
                                              Activo = U.Activo,
                                              Correo = U.Correo,
                                              Telefono1 = U.Telefono_1,
                                              Telefono2 = U.Telefono_2,
                                              Login = U.Login,
                                              IdPerfil = (U.Perfil.IdPerfil == null) ? -1 : U.Perfil.IdPerfil
                                          }).FirstOrDefault();
                return usuraio;
            }
        }

        public ObjetoGenerico Obtener(string login)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico usuraio = (from U in Contexto.Usuario
                                          where U.Login == login
                                          select new ObjetoGenerico()
                                          {
                                              Nombre = U.Nombre,
                                              Apellido = U.Apellido,
                                              NumIdentificacion = U.Identificacion,
                                              Activo = U.Activo,
                                              Correo = U.Correo,
                                              Telefono1 = U.Telefono_1,
                                              Telefono2 = U.Telefono_2,
                                              Login = U.Login
                                          }).FirstOrDefault();
                return usuraio;
            }
        }

        public List<int> ObtenerHotelesPorUsuario(int idUsuario, bool isAdmon)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<int> listaId = new List<int>();
                if (isAdmon)
                {
                    listaId = Contexto.Hotel.Select(H => H.IdHotel).ToList();
                }
                else
                {
                   listaId = Contexto.Hotel_Usuario.Where(HU => HU.Usuario.IdUsuario == idUsuario).Select(HU => HU.Hotel.IdHotel).ToList();
                }
                return listaId;
            }
        }

        public List<ObjetoGenerico> VerTodosPorHotelUsuarios(int idHotel, int idSuperUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropietario = ((from HU in Contexto.Hotel_Usuario
                                                          join U in Contexto.Usuario on HU.Usuario.IdUsuario equals U.IdUsuario
                                                          join H in Contexto.Hotel on HU.Hotel.IdHotel equals H.IdHotel
                                                          join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                                          where H.IdHotel == idHotel
                                                          select new ObjetoGenerico()
                                                          {
                                                              IdUsuario = U.IdUsuario,
                                                              Nombre = U.Nombre,
                                                              Apellido = U.Apellido,
                                                              IdPerfil = P.IdPerfil,
                                                              NombrePerfil = P.Nombre,
                                                              Login = U.Login
                                                          }).Union(
                                                          from U in Contexto.Usuario
                                                          join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                                          where P.IdPerfil == idSuperUsuario
                                                          select new ObjetoGenerico()
                                                          {
                                                              IdUsuario = U.IdUsuario,
                                                              Nombre = U.Nombre,
                                                              Apellido = U.Apellido,
                                                              IdPerfil = P.IdPerfil,
                                                              NombrePerfil = P.Nombre,
                                                              Login = U.Login
                                                          }).Union(
                                                          from U in Contexto.Usuario
                                                          where U.Perfil.IdPerfil == null
                                                          select new ObjetoGenerico()
                                                          {
                                                              IdUsuario = U.IdUsuario,
                                                              Nombre = U.Nombre,
                                                              Apellido = U.Apellido,
                                                              IdPerfil = -1,
                                                              NombrePerfil = "Sin Perfil",
                                                              Login = U.Login
                                                          })).Distinct().OrderBy(U => U.Nombre).ToList();
                return listaPropietario;
            }
        }

        public bool EsLoginExistenteUsuario(string login, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Usuario.Where(U => U.Login == login && U.IdUsuario != idUsuario).Count();

                return (con > 0) ? true : false;
            }
        }
    }
}
