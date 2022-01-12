using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using Servicios;
using System.Text.RegularExpressions;

namespace BO
{
    public class PropietarioBo
    {
        public ObjetoGenerico Autenticar(string login, string pass)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string passTmp = Utilities.EncodePassword(string.Concat(login, pass));
                ObjetoGenerico user = (from U in Contexto.Propietario
                                       join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                       where U.Login == login && 
                                             U.Pass == passTmp && 
                                             U.Activo == true
                                       select new ObjetoGenerico()
                                       {
                                           Id = U.IdPropietario,
                                           IdPerfil = P.IdPerfil,
                                           IdCiudad = U.Ciudad.IdCiudad,
                                           IdDepto = U.Ciudad.Departamento.IdDepartamento,
                                           PrimeroNombre = U.NombrePrimero,
                                           SegundoNombre = U.NombreSegundo,
                                           PrimerApellido = U.ApellidoPrimero,
                                           SegundoApellido = U.ApellidoSegundo,
                                           NumIdentificacion = U.NumIdentificacion,
                                           TipoPersona = U.TipoPersona,
                                           TipoDocumento = U.TipoDocumento,
                                           Login = U.Login,
                                           Correo = U.Correo,
                                           Correo2 = U.Correo2,
                                           Correo3 = U.Correo3,
                                           Activo = U.Activo,
                                           Cambio = U.Cambio,
                                           Direccion = U.Direccion,
                                           Telefono1 = U.Telefono_1,
                                           Telefono2 = U.Telefono_2,
                                           NombreContacto = U.NombreContacto,
                                           TelContacto = U.TelefonoContacto,
                                           CorreoContacto = U.CorreoContacto,
                                           Tipo = "P"
                                       }).FirstOrDefault();
                return user;
            }
        }

        public List<ObjetoGenerico> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropiestario = new List<ObjetoGenerico>();
                listaPropiestario = (from U in Contexto.Propietario
                                     join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                     join C in Contexto.Ciudad on U.Ciudad.IdCiudad equals C.IdCiudad
                                     select new ObjetoGenerico()
                                     {
                                         IdPropietario = U.IdPropietario,
                                         IdPerfil = P.IdPerfil,
                                         PrimeroNombre = U.NombrePrimero,
                                         SegundoNombre = U.NombreSegundo,
                                         PrimerApellido = U.ApellidoPrimero,
                                         SegundoApellido = U.ApellidoSegundo,
                                         NombreCompleto = U.NombrePrimero + " " + U.NombreSegundo + " " + U.ApellidoPrimero + " " + U.ApellidoSegundo,
                                         TipoPersona = U.TipoPersona,
                                         Login = U.Login,
                                         NumIdentificacion = U.NumIdentificacion,
                                         NombreCiudad = C.Nombre,
                                         NombrePerfil = P.Nombre,
                                         Direccion = U.Direccion,
                                         Telefono1 = U.Telefono_1,
                                         Telefono2 = U.Telefono_2,
                                         Telefono3 = U.Telefono_3,
                                         EsRetenedor = U.EsRetenedor
                                     }).OrderBy(U => U.PrimeroNombre).ToList();
                return listaPropiestario;
            }
        }

        public List<Propietario> VerTodos2()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Propietario> listaPropietario = new List<Propietario>();
                listaPropietario = Contexto.Propietario.ToList();

                return listaPropietario;
            }
        }

        public List<ObjetoGenerico> VerTodosPorHotelPropietarioGrilla(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropiestario = (from SP in Contexto.Suit_Propietario
                                                          join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                                          join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                                          where S.Hotel.IdHotel == idHotel
                                                          select new ObjetoGenerico()
                                                          {
                                                              PrimeroNombre = P.NombrePrimero,
                                                              SegundoNombre = P.NombreSegundo,
                                                              PrimerApellido = P.ApellidoPrimero,
                                                              SegundoApellido = P.ApellidoSegundo,
                                                              NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                              TipoPersona = P.TipoPersona,
                                                              IdPropietario = P.IdPropietario,
                                                              IdSuit = S.IdSuit,
                                                              NumIdentificacion = P.NumIdentificacion,
                                                              NumSuit = S.NumSuit,
                                                              NumEscritura = S.NumEscritura
                                                          }).OrderBy(P => new { P.NombreCompleto, P.IdSuit }).ToList();
                return listaPropiestario;
            }
        }

        public List<ObjetoGenerico> VerTodosPorHotelPropietarioSinDistinc(int idHotel, int idPerfilPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                //Contexto.CreateQuery<ObjetoGenerico>("", null);
                List<ObjetoGenerico> listaPropiestario = (from SP in Contexto.Suit_Propietario
                                                          join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                                          join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                                          join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel                                                          
                                                          join C in Contexto.Ciudad on P.Ciudad.IdCiudad equals C.IdCiudad
                                                          join B in Contexto.Banco on SP.Banco.IdBanco equals B.IdBanco
                                                          where H.IdHotel == idHotel &&
                                                                SP.EsActivo == true &&
                                                                P.Perfil.IdPerfil == idPerfilPropietario
                                                          select new ObjetoGenerico()
                                                          {
                                                              IdPropietario = P.IdPropietario,
                                                              IdPerfil = P.Perfil.IdPerfil,
                                                              IdSuit = S.IdSuit,
                                                              PrimeroNombre = P.NombrePrimero,
                                                              SegundoNombre = P.NombreSegundo,
                                                              PrimerApellido = P.ApellidoPrimero,
                                                              SegundoApellido = P.ApellidoSegundo,
                                                              Nombre = P.NombrePrimero + " " + P.NombreSegundo,
                                                              Apellido = P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                              TipoPersona = P.TipoPersona,
                                                              Nit = H.Nit,
                                                              NumIdentificacion = P.NumIdentificacion,
                                                              NombreCiudad = C.Nombre,
                                                              NumSuit = S.NumSuit,
                                                              NumEscritura = S.NumEscritura,
                                                              Login = P.Login,
                                                              Correo = P.Correo,
                                                              Correo2 = P.Correo2,
                                                              Correo3 = P.Correo3,
                                                              CorreoContacto = P.CorreoContacto,
                                                              NombreHotel = H.Nombre,
                                                              RutaLogo = H.Logo,
                                                              EsRetenedor = P.EsRetenedor,
                                                              Direccion = P.Direccion,
                                                              NumCuenta = SP.NumCuenta,
                                                              NombreBanco = B.Nombre,
                                                              Codigo = H.Codigo
                                                          }).OrderBy(P => new { P.PrimeroNombre, P.IdPropietario }).ToList();
                return listaPropiestario;
            }
        }

        public List<ObjetoGenerico> VerTodosPorHotelPropietarioConDistinc(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropiestario = (from H in Contexto.Hotel
                                                          join S in Contexto.Suit on H.IdHotel equals S.Hotel.IdHotel
                                                          join SP in Contexto.Suit_Propietario on S.IdSuit equals SP.Suit.IdSuit
                                                          join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                                          where H.IdHotel == idHotel
                                                          select new ObjetoGenerico()
                                                          {
                                                              IdPropietario = P.IdPropietario,
                                                              Nombre = P.NombrePrimero,
                                                              NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                              NumIdentificacion = P.NumIdentificacion
                                                          }).Distinct().OrderBy(P => new { P.Nombre, P.IdPropietario }).ToList();
                return listaPropiestario;
            }
        }

        public List<ObjetoGenerico> VerTodosPorHotelPropietario(int idHotel, int idPerfilPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropiestario = (from SP in Contexto.Suit_Propietario
                                                          join U in Contexto.Propietario on SP.Propietario.IdPropietario equals U.IdPropietario
                                                          join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                                          join C in Contexto.Ciudad on U.Ciudad.IdCiudad equals C.IdCiudad
                                                          join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                                          join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                                          where H.IdHotel == idHotel && P.IdPerfil == idPerfilPropietario
                                                          select new ObjetoGenerico()
                                                          {
                                                              PrimeroNombre = U.NombrePrimero,
                                                              SegundoNombre = U.NombreSegundo,
                                                              PrimerApellido = U.ApellidoPrimero,
                                                              SegundoApellido = U.ApellidoSegundo,
                                                              Nombre = U.NombrePrimero + " " + U.NombreSegundo,
                                                              Apellido = U.ApellidoPrimero + " " + U.ApellidoSegundo,
                                                              TipoPersona = U.TipoPersona,
                                                              Nit = U.NumIdentificacion,
                                                              NumIdentificacion = U.NumIdentificacion,
                                                              IdPropietario = U.IdPropietario,
                                                              IdPerfil = P.IdPerfil,
                                                              NombrePerfil = P.Nombre,
                                                              NombreCiudad = C.Nombre,
                                                              NumSuit = S.NumSuit,
                                                              NumEscritura = S.NumEscritura,
                                                              Login = U.Login,
                                                              IdSuit = S.IdSuit,
                                                              Correo = U.Correo,
                                                              Correo2 = U.Correo2,
                                                              Correo3 = U.Correo3,
                                                              CorreoContacto = U.CorreoContacto,
                                                              NombreHotel = H.Nombre,
                                                              RutaLogo = H.Logo,
                                                              EsRetenedor = U.EsRetenedor
                                                          }).Distinct().ToList();
                return listaPropiestario;
            }
        }
        
        public List<ObjetoGenerico> VerTodos(int idPerfilPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropiestario = new List<ObjetoGenerico>();
                listaPropiestario = (from U in Contexto.Propietario
                                     join P in Contexto.Perfil on U.Perfil.IdPerfil equals P.IdPerfil
                                     join C in Contexto.Ciudad on U.Ciudad.IdCiudad equals C.IdCiudad
                                     where U.Perfil.IdPerfil == idPerfilPropietario
                                     select new ObjetoGenerico()
                                     {
                                         IdPropietario = U.IdPropietario,
                                         IdPerfil = P.IdPerfil,
                                         PrimeroNombre = U.NombrePrimero,
                                         SegundoNombre = U.NombreSegundo,
                                         PrimerApellido = U.ApellidoPrimero,
                                         SegundoApellido = U.ApellidoSegundo,
                                         TipoPersona = U.TipoPersona,
                                         Login = U.Login,
                                         NumIdentificacion = U.NumIdentificacion,
                                         NombreCiudad = C.Nombre,
                                         NombrePerfil = P.Nombre,
                                         Direccion = U.Direccion,
                                         Telefono1 = U.Telefono_1,
                                         Telefono2 = U.Telefono_2,
                                         Telefono3 = U.Telefono_3,
                                         Correo = U.Correo,
                                         Correo2 = U.Correo2,
                                         Correo3 = U.Correo3,
                                         EsRetenedor = U.EsRetenedor
                                     }).OrderBy(U => U.NombreCompleto).ToList();
                return listaPropiestario;
            }
        }

        public int Guardar(Propietario propietario, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToPropietario(propietario);
                Contexto.SaveChanges();

                #region auditoria

                string valor = string.Empty;
                string nomVariable = string.Empty;
                DateTime fecha = DateTime.Now;
                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = propietario.NombrePrimero + " " + propietario.NombreSegundo + " " + propietario.ApellidoPrimero + " " + propietario.ApellidoSegundo;
                auditoriaTmp.NombreTabla = "Propietario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = propietario.NumIdentificacion;
                auditoriaTmp.NombreTabla = "Propietario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Num. Identificacion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = propietario.Correo;
                auditoriaTmp.NombreTabla = "Propietario";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Correo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                foreach (Suit_Propietario itemSuite in propietario.Suit_Propietario)
                {
                    int idSuite = (int)itemSuite.SuitReference.EntityKey.EntityKeyValues[0].Value;
                    int idBanco = (int)itemSuite.BancoReference.EntityKey.EntityKeyValues[0].Value;

                    var nombreHotelSuite = Contexto.Suit.Where(S => S.IdSuit == idSuite).Select(S => new { S.NumSuit, S.Hotel.Nombre }).FirstOrDefault();
                    string nombreBanco = Contexto.Banco.Where(B => B.IdBanco == idBanco).Select(B => B.Nombre).FirstOrDefault();

                    // Detalle de la suite
                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Propietario: " + propietario.NombrePrimero + " " + propietario.NombreSegundo + " " + propietario.ApellidoPrimero + " " + propietario.ApellidoSegundo + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Num. Cuenta = " + itemSuite.NumCuenta;
                    auditoriaTmp.NombreTabla = "Detalle Suite";
                    auditoriaTmp.Accion = "Insertar";
                    auditoriaTmp.Campo = "Num. Cuenta";
                    auditoriaTmp.Fechahora = fecha;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Propietario: " + propietario.NombrePrimero + " " + propietario.NombreSegundo + " " + propietario.ApellidoPrimero + " " + propietario.ApellidoSegundo + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Tipo Cuenta = " + itemSuite.TipoCuenta;
                    auditoriaTmp.NombreTabla = "Detalle Suite";
                    auditoriaTmp.Accion = "Insertar";
                    auditoriaTmp.Campo = "Tipo Cuenta";
                    auditoriaTmp.Fechahora = fecha;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Propietario: " + propietario.NombrePrimero + " " + propietario.NombreSegundo + " " + propietario.ApellidoPrimero + " " + propietario.ApellidoSegundo + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Banco = " + nombreBanco;
                    auditoriaTmp.NombreTabla = "Detalle Suite";
                    auditoriaTmp.Accion = "Insertar";
                    auditoriaTmp.Campo = "Banco";
                    auditoriaTmp.Fechahora = fecha;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Propietario: " + propietario.NombrePrimero + " " + propietario.NombreSegundo + " " + propietario.ApellidoPrimero + " " + propietario.ApellidoSegundo + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Titular Cuenta = " + itemSuite.Titular;
                    auditoriaTmp.NombreTabla = "Detalle Suite";
                    auditoriaTmp.Accion = "Insertar";
                    auditoriaTmp.Campo = "Titular Cuenta";
                    auditoriaTmp.Fechahora = fecha;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);

                    foreach (Valor_Variable_Suit itemValor in itemSuite.Valor_Variable_Suit.Where(VVS=>VVS.Suit_Propietario.IdSuitPropietario==itemSuite.IdSuitPropietario).ToList())
                    {
                        int idVariable = (int)itemValor.VariableReference.EntityKey.EntityKeyValues[0].Value;
                        nomVariable = Contexto.Variable.Where(V => V.IdVariable == idVariable).Select(V => V.Nombre).FirstOrDefault();
                        valor = itemValor.Valor.ToString("N");

                        auditoriaTmp = new Auditoria();
                        auditoriaTmp.ValorNuevo = "Propietario: " + propietario.NombrePrimero + " " + propietario.NombreSegundo + " " + propietario.ApellidoPrimero + " " + propietario.ApellidoSegundo + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : " + nomVariable + " = " + valor;
                        auditoriaTmp.NombreTabla = "Valor Variable Suite";
                        auditoriaTmp.Accion = "Insertar";
                        auditoriaTmp.Campo = "Valor";
                        auditoriaTmp.Fechahora = fecha;
                        auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                        Contexto.AddToAuditoria(auditoriaTmp);                        
                    }                    
                }                
                #endregion

                Contexto.SaveChanges();
                return propietario.IdPropietario;
            }
        }

        public int Guardar(string nombrePrimero, string nombreSegundo, string apellidoPrimero, string apellidoSegundo, string tipoPersona,
                           string numIdentificacion, string login, string clave, string correo, string correo2, string correo3,
                           bool esActivo, int idCiudad, int idPerfil, string direccion, string tel1, string tel2, string tel3,
                           string nombreContacto, string telContacto, string correoContacto, string tipoDocumento, bool esRetenedor)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Propietario propietarioTmp = new Propietario();
                propietarioTmp.NombrePrimero = nombrePrimero;
                propietarioTmp.NombreSegundo = nombreSegundo;
                propietarioTmp.ApellidoPrimero = apellidoPrimero;
                propietarioTmp.ApellidoSegundo = apellidoSegundo;
                propietarioTmp.TipoPersona = tipoPersona;
                propietarioTmp.TipoDocumento = tipoDocumento;

                propietarioTmp.NumIdentificacion = numIdentificacion;
                propietarioTmp.Pass = clave;
                propietarioTmp.Login = login;
                propietarioTmp.Activo = esActivo;
                propietarioTmp.Correo = correo;
                propietarioTmp.Correo2 = correo2;
                propietarioTmp.Correo3 = correo3;
                propietarioTmp.EsRetenedor = esRetenedor;
                propietarioTmp.FechaIngreso = DateTime.Now;
                propietarioTmp.Direccion = direccion;
                propietarioTmp.Telefono_1 = tel1;
                propietarioTmp.Telefono_2 = tel2;
                propietarioTmp.Telefono_3 = tel3;
                propietarioTmp.Cambio = true;
                propietarioTmp.NombreContacto = nombreContacto;
                propietarioTmp.TelefonoContacto = telContacto;
                propietarioTmp.CorreoContacto = correoContacto;
                propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);
                propietarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", idPerfil);

                Contexto.AddToPropietario(propietarioTmp);
                Contexto.SaveChanges();

                return propietarioTmp.IdPropietario;
            }
        }

        public void Actualizar(int idPropietario, string nombrePrimero, string nombreSegundo, string apellidoPrimero, string apellidoSegundo, string tipoPersona,
                               string numIdentificacion, string correo, string correo2, string correo3, bool esActivo, int idCiudad, int idPerfil, string direccion,
                               string tel1, string tel2, string nombreContacto, string telContacto, string correoContacto, string tipoDocumento, bool esRetenedor, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Propietario propietarioTmp = Contexto.Propietario.Where(P => P.IdPropietario == idPropietario).FirstOrDefault();

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombrePrimero + " " + nombreSegundo + " " + apellidoPrimero + " " + apellidoSegundo;
                auditoriaTmp.ValorAnterior = propietarioTmp.NombrePrimero + " " + propietarioTmp.NombreSegundo + " " + propietarioTmp.ApellidoPrimero + " " + propietarioTmp.ApellidoSegundo;
                auditoriaTmp.NombreTabla = "Propietario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = numIdentificacion;
                auditoriaTmp.ValorAnterior = propietarioTmp.NumIdentificacion;
                auditoriaTmp.NombreTabla = "Propietario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Num. Identificacion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = correo;
                auditoriaTmp.ValorAnterior = propietarioTmp.Correo;
                auditoriaTmp.NombreTabla = "Propietario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Correo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                propietarioTmp.NombrePrimero = nombrePrimero;
                propietarioTmp.NombreSegundo = nombreSegundo;
                propietarioTmp.ApellidoPrimero = apellidoPrimero;
                propietarioTmp.ApellidoSegundo = apellidoSegundo;
                propietarioTmp.TipoPersona = tipoPersona;
                propietarioTmp.TipoDocumento = tipoDocumento;
                propietarioTmp.NumIdentificacion = numIdentificacion;
                propietarioTmp.Activo = esActivo;
                propietarioTmp.Direccion = direccion;
                propietarioTmp.Telefono_1 = tel1;
                propietarioTmp.Telefono_2 = tel2;
                propietarioTmp.Correo = correo;
                propietarioTmp.Correo2 = correo2;
                propietarioTmp.Correo3 = correo3;
                propietarioTmp.EsRetenedor = esRetenedor;
                propietarioTmp.NombreContacto = nombreContacto;
                propietarioTmp.TelefonoContacto = telContacto;
                propietarioTmp.CorreoContacto = correoContacto;
                propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);
                propietarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", idPerfil);

                Contexto.SaveChanges();
            }
            
        }

        public void Actualizar(int idPropietario, string nombrePrimero, string nombreSegundo, 
                               string apellidoPrimero, string apellidoSegundo, string tipoPersona,
                               string numIdentificacion, string correo, string correo2, string correo3, int idCiudad, string direccion,
                               string tel1, string tel2, string tipoDocumento, bool esRetenedor)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Propietario propietarioTmp = Contexto.Propietario.Where(P => P.IdPropietario == idPropietario).FirstOrDefault();
                propietarioTmp.NombrePrimero = nombrePrimero;
                propietarioTmp.NombreSegundo = nombreSegundo;
                propietarioTmp.ApellidoPrimero = apellidoPrimero;
                propietarioTmp.ApellidoSegundo = apellidoSegundo;
                propietarioTmp.TipoPersona = tipoPersona;
                propietarioTmp.TipoDocumento = tipoDocumento;
                propietarioTmp.NumIdentificacion = numIdentificacion;
                propietarioTmp.Direccion = direccion;
                propietarioTmp.Telefono_1 = tel1;
                propietarioTmp.Telefono_2 = tel2;
                propietarioTmp.Correo = correo;
                propietarioTmp.Correo2 = correo2;
                propietarioTmp.Correo3 = correo3;
                propietarioTmp.EsRetenedor = esRetenedor;
                propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);

                Contexto.SaveChanges();
            }

        }

        public void Actualizar(int idPropietario, string correo, string correo2, string correo3, int idCiudad, string direccion,
                               string tel1, string tel2, string nombreContacto, string telContacto, string correoContacto, bool esRetenedor)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Propietario propietarioTmp = Contexto.Propietario.Where(P => P.IdPropietario == idPropietario).FirstOrDefault();
                //propietarioTmp.TipoDocumento = tipoDocumento;
                propietarioTmp.Direccion = direccion;
                propietarioTmp.Telefono_1 = tel1;
                propietarioTmp.Telefono_2 = tel2;
                propietarioTmp.Correo = correo;
                propietarioTmp.Correo2 = correo2;
                propietarioTmp.Correo3 = correo3;
                propietarioTmp.EsRetenedor = esRetenedor;
                propietarioTmp.NombreContacto = nombreContacto;
                propietarioTmp.TelefonoContacto = telContacto;
                propietarioTmp.CorreoContacto = correoContacto;
                propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);
                Contexto.SaveChanges();
            }
        }

        public ObjetoGenerico ObtenerPropietario(int idPropietario, int idSuit)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico propietarioTmp = new ObjetoGenerico();
                propietarioTmp = (from P in Contexto.Propietario
                                  join C in Contexto.Ciudad on P.Ciudad.IdCiudad equals C.IdCiudad
                                  join D in Contexto.Departamento on C.Departamento.IdDepartamento equals D.IdDepartamento
                                  join SP in Contexto.Suit_Propietario on P.IdPropietario equals SP.Propietario.IdPropietario
                                  join B in Contexto.Banco on SP.Banco.IdBanco equals B.IdBanco
                                  join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                  join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                  join CH in Contexto.Ciudad on H.Ciudad.IdCiudad equals CH.IdCiudad
                                  where P.IdPropietario == idPropietario && SP.Suit.IdSuit == idSuit
                                  select new ObjetoGenerico
                                  {
                                      IdPropietario = P.IdPropietario,
                                      IdCiudad = C.IdCiudad,
                                      IdDepto = D.IdDepartamento,
                                      IdPerfil = P.Perfil.IdPerfil,
                                      IdSuit = S.IdSuit,
                                      IdHotel = H.IdHotel,
                                      PrimeroNombre = P.NombrePrimero,
                                      SegundoNombre = P.NombreSegundo,
                                      PrimerApellido = P.ApellidoPrimero,
                                      SegundoApellido = P.ApellidoSegundo,
                                      Nombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                      TipoPersona = P.TipoPersona,
                                      NumIdentificacion = P.NumIdentificacion,
                                      Login = P.Login,
                                      Correo = P.Correo,
                                      Correo2 = P.Correo2,
                                      Correo3 = P.Correo3,
                                      Activo = P.Activo,
                                      Direccion = P.Direccion,
                                      Telefono1 = P.Telefono_1,
                                      Telefono2 = P.Telefono_2,
                                      Telefono3 = P.Telefono_3,
                                      NombreContacto = P.NombreContacto,
                                      TelContacto = P.TelefonoContacto,
                                      CorreoContacto = P.CorreoContacto,
                                      NumSuit = S.NumSuit,
                                      NumEscritura = S.NumEscritura,
                                      NumEstadias = SP.NumEstadias,
                                      RutaLogo = H.Logo,
                                      NombreHotel = H.Nombre,
                                      NombreCiudad = CH.Nombre,
                                      Nit = H.Nit,
                                      NumCuenta = SP.NumCuenta,
                                      NombreBanco = B.Nombre,
                                      EsRetenedor = P.EsRetenedor,
                                      Codigo = H.Codigo
                                  }).FirstOrDefault();

                return propietarioTmp;
            }
        }

        public ObjetoGenerico ObtenerPropietarioCertificado(int idPropietario, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico propietarioTmp = new ObjetoGenerico();
                propietarioTmp = (from P in Contexto.Propietario
                                  join C in Contexto.Ciudad on P.Ciudad.IdCiudad equals C.IdCiudad
                                  join D in Contexto.Departamento on C.Departamento.IdDepartamento equals D.IdDepartamento
                                  join SP in Contexto.Suit_Propietario on P.IdPropietario equals SP.Propietario.IdPropietario
                                  join B in Contexto.Banco on SP.Banco.IdBanco equals B.IdBanco
                                  join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                  join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                  join CH in Contexto.Ciudad on H.Ciudad.IdCiudad equals CH.IdCiudad
                                  where P.IdPropietario == idPropietario
                                  select new ObjetoGenerico
                                  {
                                      IdPropietario = P.IdPropietario,
                                      IdCiudad = C.IdCiudad,
                                      IdDepto = D.IdDepartamento,
                                      IdPerfil = P.Perfil.IdPerfil,
                                      IdSuit = S.IdSuit,
                                      IdHotel = H.IdHotel,
                                      PrimeroNombre = P.NombrePrimero,
                                      SegundoNombre = P.NombreSegundo,
                                      PrimerApellido = P.ApellidoPrimero,
                                      SegundoApellido = P.ApellidoSegundo,
                                      Nombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                      TipoPersona = P.TipoPersona,
                                      NumIdentificacion = P.NumIdentificacion,
                                      Login = P.Login,
                                      Correo = P.Correo,
                                      Correo2 = P.Correo2,
                                      Correo3 = P.Correo3,
                                      Activo = P.Activo,
                                      Direccion = P.Direccion,
                                      Telefono1 = P.Telefono_1,
                                      Telefono2 = P.Telefono_2,
                                      Telefono3 = P.Telefono_3,
                                      NombreContacto = P.NombreContacto,
                                      TelContacto = P.TelefonoContacto,
                                      CorreoContacto = P.CorreoContacto,
                                      NumSuit = S.NumSuit,
                                      NumEscritura = S.NumEscritura,
                                      NumEstadias = SP.NumEstadias,
                                      RutaLogo = H.Logo,
                                      NombreHotel = H.Nombre,
                                      NombreCiudad = CH.Nombre,
                                      Nit = H.Nit,
                                      NumCuenta = SP.NumCuenta,
                                      NombreBanco = B.Nombre,
                                      EsRetenedor = P.EsRetenedor
                                  }).FirstOrDefault();

                return propietarioTmp;
            }
        }

        public ObjetoGenerico ObtenerPropietario(int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                ObjetoGenerico propietarioTmp = new ObjetoGenerico();
                propietarioTmp = (from P in Contexto.Propietario
                                    join C in Contexto.Ciudad on P.Ciudad.IdCiudad equals C.IdCiudad
                                    join D in Contexto.Departamento on C.Departamento.IdDepartamento equals D.IdDepartamento
                                          where P.IdPropietario == idPropietario
                                    select new ObjetoGenerico
                                    {
                                        IdPropietario = P.IdPropietario,
                                        IdCiudad = C.IdCiudad,
                                        IdDepto = D.IdDepartamento,
                                        IdPerfil = P.Perfil.IdPerfil,
                                        PrimeroNombre = P.NombrePrimero,
                                        SegundoNombre = P.NombreSegundo,
                                        PrimerApellido = P.ApellidoPrimero,
                                        SegundoApellido = P.ApellidoSegundo,
                                        TipoPersona = P.TipoPersona,
                                        TipoDocumento = P.TipoDocumento,
                                        NumIdentificacion = P.NumIdentificacion,
                                        Login = P.Login,
                                        Correo = P.Correo,
                                        Correo2 = P.Correo2,
                                        Correo3 = P.Correo3,
                                        EsRetenedor = P.EsRetenedor,
                                        Activo = P.Activo,
                                        Direccion = P.Direccion,
                                        Telefono1 = P.Telefono_1,
                                        Telefono2 = P.Telefono_2,
                                        Telefono3 = P.Telefono_3,
                                        NombreContacto = P.NombreContacto,
                                        TelContacto = P.TelefonoContacto,
                                        CorreoContacto = P.CorreoContacto
                                    }).FirstOrDefault();

                return propietarioTmp;
            }
        }

        public List<ObjetoGenerico> ObtenerPropietarioConSuiteAndHotel(int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaSuitePropietario = new List<ObjetoGenerico>();

                listaSuitePropietario = (from SP in Contexto.Suit_Propietario
                                         join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                         join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                         join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                         where P.IdPropietario == idPropietario
                                         select new ObjetoGenerico()
                                         {
                                             IdHotel = H.IdHotel,
                                             NombreHotel = H.Nombre,
                                             IdPropietario = P.IdPropietario,
                                             IdSuit = S.IdSuit,
                                             PrimeroNombre = P.NombrePrimero,
                                             SegundoNombre = P.NombreSegundo,
                                             PrimerApellido = P.ApellidoPrimero,
                                             SegundoApellido = P.ApellidoSegundo,
                                             TipoPersona = P.TipoPersona,
                                             Nombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                             NumIdentificacion = P.NumIdentificacion,
                                             Direccion = P.Direccion,
                                             NumSuit = S.NumSuit,
                                             NumEscritura = S.NumEscritura,
                                             RegistroNotaria = S.RegistroNotaria,
                                             Correo = P.Correo,
                                             Correo2 = P.Correo2,
                                             Correo3 = P.Correo3,
                                             EsRetenedor = P.EsRetenedor,
                                             NombreCiudad = P.Ciudad.Nombre,
                                             NumEstadias = SP.NumEstadias
                                         }).OrderBy(H => H.NombreHotel).ToList();

                return listaSuitePropietario;
            }
        }

        public Propietario ObtenerPropietario(string numIdentificacionNit)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Propietario.Where(U => U.NumIdentificacion == numIdentificacionNit).FirstOrDefault();
            }
        }

        public void ResetClave(string login)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Propietario propietarioTmp = Contexto.Propietario.Where(P => P.Login == login).FirstOrDefault();
                propietarioTmp.Pass = Utilities.EncodePassword(String.Concat(propietarioTmp.Login, propietarioTmp.Login));
                propietarioTmp.Cambio = true;
                Contexto.SaveChanges();
            }
        }

        public bool ActualizarClave(int idPersona, string claveActual, string claveNueva, string tipo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                if (tipo == "U")
                {
                    Usuario usuarioTmp = Contexto.Usuario.Where(P => P.IdUsuario == idPersona).FirstOrDefault();

                    string claveTmp = Utilities.EncodePassword(String.Concat(usuarioTmp.Login, claveActual));
                    if (claveTmp == usuarioTmp.Pass)
                    {
                        string claveNueva_ = Utilities.EncodePassword(String.Concat(usuarioTmp.Login, claveNueva));
                        usuarioTmp.Pass = claveNueva_;
                        usuarioTmp.EsCambio = false;
                        Contexto.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    Propietario propietarioTmp = Contexto.Propietario.Where(P => P.IdPropietario == idPersona).FirstOrDefault();

                    string claveTmp = Utilities.EncodePassword(String.Concat(propietarioTmp.Login, claveActual));
                    if (claveTmp == propietarioTmp.Pass)
                    {
                        string claveNueva_ = Utilities.EncodePassword(String.Concat(propietarioTmp.Login, claveNueva));
                        propietarioTmp.Pass = claveNueva_;
                        propietarioTmp.Cambio = false;
                        Contexto.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        public List<ObjetoGenerico> ObtenerPropietariosConSuite(int idHotel, DateTime fechaPeriodoLiquidacion)
        {
            //SELECT distinct dbo.Propietario.NombrePrimero, dbo.Propietario.NombreSegundo, dbo.Propietario.NumIdentificacion, dbo.Suit.Descripcion, dbo.Suit.NumSuit, dbo.Suit.NumEscritura, dbo.Liquidacion.FechaPeriodoLiquidado, dbo.Hotel.Nombre
            //FROM dbo.Liquidacion 
            //INNER JOIN dbo.Suit ON dbo.Liquidacion.IdSuit = dbo.Suit.IdSuit 
            //INNER JOIN dbo.Hotel ON dbo.Liquidacion.IdHotel = dbo.Hotel.IdHotel 
            //INNER JOIN dbo.Propietario ON dbo.Liquidacion.IdPropietario = dbo.Propietario.IdPropietario
            //where dbo.Hotel.IdHotel = 352 and dbo.Liquidacion.FechaPeriodoLiquidado = '2016-03-01'

            List<ObjetoGenerico> listaPropietarios = new List<ObjetoGenerico>();
            if (idHotel == -1)
                return listaPropietarios;

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                
                listaPropietarios = (from L in Contexto.Liquidacion
                                     join S in Contexto.Suit on L.Suit.IdSuit equals S.IdSuit
                                     join H in Contexto.Hotel on L.Hotel.IdHotel equals H.IdHotel
                                     join P in Contexto.Propietario on L.Propietario.IdPropietario equals P.IdPropietario
                                     where S.Hotel.IdHotel == idHotel && L.FechaPeriodoLiquidado.Year == fechaPeriodoLiquidacion.Year && L.FechaPeriodoLiquidado.Month == fechaPeriodoLiquidacion.Month && L.FechaPeriodoLiquidado.Day == fechaPeriodoLiquidacion.Day
                                     select new ObjetoGenerico()
                                     {
                                         IdPropietario = P.IdPropietario,
                                         IdHotel = H.IdHotel,
                                         IdSuit = S.IdSuit,
                                         PrimeroNombre = P.NombrePrimero,
                                         SegundoNombre = P.NombreSegundo,
                                         PrimerApellido = P.ApellidoPrimero,
                                         SegundoApellido = P.ApellidoSegundo,
                                         TipoPersona = P.TipoPersona,
                                         Nombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                         NumIdentificacion = P.NumIdentificacion,
                                         Direccion = P.Direccion,
                                         NumSuit = S.NumSuit,
                                         Correo = P.Correo,
                                         Correo2 = P.Correo2,
                                         Correo3 = P.Correo3,
                                         EsRetenedor = P.EsRetenedor,
                                         NumEscritura = S.NumEscritura,
                                         NombreCiudad = P.Ciudad.Nombre
                                     }).Distinct().OrderBy(P => P.PrimeroNombre).ToList();
                return listaPropietarios;
            }
        }

        public List<ObjetoGenerico> ObtenerPropietariosConSuite(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropietarios = new List<ObjetoGenerico>();
                listaPropietarios = (from SP in Contexto.Suit_Propietario
                                     join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                     join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                     where S.Hotel.IdHotel == idHotel &&
                                           SP.EsActivo == true
                                     select new ObjetoGenerico()
                                     {
                                         IdPropietario = P.IdPropietario,
                                         IdSuit = S.IdSuit,
                                         PrimeroNombre = P.NombrePrimero,
                                         SegundoNombre = P.NombreSegundo,
                                         PrimerApellido = P.ApellidoPrimero,
                                         SegundoApellido = P.ApellidoSegundo,
                                         TipoPersona = P.TipoPersona,
                                         Nombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                         NumIdentificacion = P.NumIdentificacion,
                                         Direccion = P.Direccion,
                                         NumSuit = S.NumSuit,
                                         Correo = P.Correo,
                                         Correo2 = P.Correo2,
                                         Correo3 = P.Correo3,
                                         EsRetenedor = P.EsRetenedor,
                                         NumEscritura = S.NumEscritura,
                                         NombreCiudad = P.Ciudad.Nombre,
                                         NumEstadias = SP.NumEstadias
                                     }).OrderBy(P => P.PrimeroNombre).ToList();
                return listaPropietarios;
            }
        }

        public List<ObjetoGenerico> ObtenerPropietariosConSuiteActivas(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropietarios = new List<ObjetoGenerico>();
                listaPropietarios = (from SP in Contexto.Suit_Propietario
                                     join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                     join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                     where S.Hotel.IdHotel == idHotel && SP.EsActivo == true
                                     select new ObjetoGenerico()
                                     {
                                         IdPropietario = P.IdPropietario,
                                         IdSuit = S.IdSuit,
                                         PrimeroNombre = P.NombrePrimero,
                                         SegundoNombre = P.NombreSegundo,
                                         PrimerApellido = P.ApellidoPrimero,
                                         SegundoApellido = P.ApellidoSegundo,
                                         TipoPersona = P.TipoPersona,
                                         Nombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                         NumIdentificacion = P.NumIdentificacion,
                                         Direccion = P.Direccion,
                                         NumSuit = S.NumSuit,
                                         Correo = P.Correo,
                                         Correo2 = P.Correo2,
                                         Correo3 = P.Correo3,
                                         EsRetenedor = P.EsRetenedor,
                                         NumEscritura = S.NumEscritura,
                                         NombreCiudad = P.Ciudad.Nombre,
                                         NumEstadias = SP.NumEstadias
                                     }).OrderBy(P => P.PrimeroNombre).ToList();
                return listaPropietarios;
            }
        }

        public List<ObjetoGenerico> ObtenerPropietariosByFiltro(int idHotel, string filtro, string comodin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropietarios = new List<ObjetoGenerico>();

                switch (filtro)
                {
                    case "N":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             where S.Hotel.IdHotel == idHotel && (P.NombrePrimero.ToUpper().Contains(comodin.ToUpper()) || P.NombreSegundo.ToUpper().Contains(comodin.ToUpper()))
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "A":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             where S.Hotel.IdHotel == idHotel && (P.ApellidoPrimero.ToUpper().Contains(comodin.ToUpper()) || P.ApellidoSegundo.ToUpper().Contains(comodin.ToUpper()))
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "S":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             where S.Hotel.IdHotel == idHotel && S.NumSuit.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "E":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             where S.Hotel.IdHotel == idHotel && S.NumEscritura.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "I":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             where S.Hotel.IdHotel == idHotel && P.NumIdentificacion.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    default:
                        break;
                }
                
                return listaPropietarios;
            }
        }

        public List<ObjetoGenerico> ObtenerPropietariosByFiltro(string filtro, string comodin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropietarios = new List<ObjetoGenerico>();

                switch (filtro)
                {
                    case "N":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                             where (P.NombrePrimero.ToUpper().Contains(comodin.ToUpper()) || P.NombreSegundo.ToUpper().Contains(comodin.ToUpper()))
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 IdHotel = H.IdHotel,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 NombreHotel = H.Nombre,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "A":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                             where (P.ApellidoPrimero.ToUpper().Contains(comodin.ToUpper()) || P.ApellidoSegundo.ToUpper().Contains(comodin.ToUpper()))
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 IdHotel = H.IdHotel,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 NombreHotel = H.Nombre,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "S":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                             where S.NumSuit.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 IdHotel = H.IdHotel,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 NombreHotel = H.Nombre,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "E":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                             where S.NumEscritura.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 IdHotel = H.IdHotel,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 NombreHotel = H.Nombre,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "I":
                        listaPropietarios = (from SP in Contexto.Suit_Propietario
                                             join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                             join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                             join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                             where P.NumIdentificacion.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 IdSuit = S.IdSuit,
                                                 IdHotel = H.IdHotel,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumEscritura = S.NumEscritura,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 NumSuit = S.NumSuit,
                                                 Correo = P.Correo,
                                                 NombreHotel = H.Nombre,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    default:
                        break;
                }

                return listaPropietarios;
            }
        }

        public List<ObjetoGenerico> ObtenerPropietarios(string filtro, string comodin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaPropietarios = new List<ObjetoGenerico>();

                switch (filtro)
                {
                    case "N":
                        listaPropietarios = (from P in Contexto.Propietario
                                             where (P.NombrePrimero.ToUpper().Contains(comodin.ToUpper()) || P.NombreSegundo.ToUpper().Contains(comodin.ToUpper()))
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "A":
                        listaPropietarios = (from P in Contexto.Propietario
                                             where (P.ApellidoPrimero.ToUpper().Contains(comodin.ToUpper()) || P.ApellidoSegundo.ToUpper().Contains(comodin.ToUpper()))
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    case "I":
                        listaPropietarios = (from P in Contexto.Propietario
                                             where P.NumIdentificacion.Contains(comodin)
                                             select new ObjetoGenerico()
                                             {
                                                 IdPropietario = P.IdPropietario,
                                                 PrimeroNombre = P.NombrePrimero,
                                                 SegundoNombre = P.NombreSegundo,
                                                 PrimerApellido = P.ApellidoPrimero,
                                                 SegundoApellido = P.ApellidoSegundo,
                                                 NombreCompleto = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                                 TipoPersona = P.TipoPersona,
                                                 NumIdentificacion = P.NumIdentificacion,
                                                 Direccion = P.Direccion,
                                                 Correo = P.Correo,
                                                 Correo2 = P.Correo2,
                                                 Correo3 = P.Correo3,
                                                 EsRetenedor = P.EsRetenedor
                                             }).OrderBy(P => new { P.NombreCompleto }).ToList();
                        break;

                    default:
                        break;
                }

                return listaPropietarios;
            }
        }

        /// <summary>
        /// Obtiene los propietaraios, con sus suites y sus varaibles de las mismas filtrando por hotel
        /// </summary>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public List<ObjetoGenerico> ListaPropietario(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaProietario = new List<ObjetoGenerico>();

                return listaProietario = (from H in Contexto.Hotel
                                          join V in Contexto.Variable on H.IdHotel equals V.Hotel.IdHotel
                                          join VVS in Contexto.Valor_Variable_Suit on V.IdVariable equals VVS.Variable.IdVariable
                                          join SP in Contexto.Suit_Propietario on VVS.Suit_Propietario.IdSuitPropietario equals SP.IdSuitPropietario
                                          join UP in Contexto.Propietario on SP.Propietario.IdPropietario equals UP.IdPropietario
                                          join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                          where H.IdHotel == idHotel
                                          orderby new { UP.NombrePrimero, UP.ApellidoSegundo,
                                                        UP.NombreSegundo, UP.ApellidoPrimero,
                                                        S.NumSuit, NombreVariable = V.Nombre }
                                          select new ObjetoGenerico()
                                              {
                                                  PrimeroNombre = UP.NombrePrimero,
                                                  SegundoNombre = UP.NombreSegundo,
                                                  PrimerApellido = UP.ApellidoPrimero,
                                                  SegundoApellido = UP.ApellidoSegundo,
                                                  NombreHotel = H.Nombre,
                                                  NumSuit = S.NumSuit,
                                                  NumEscritura = S.NumEscritura,
                                                  Valor = VVS.Valor,
                                                  NombreVariable = V.Nombre
                                              }).ToList();
            }
        }

        public List<Propietario> ListarTodos(int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Propietario> listaPropeitario = new List<Propietario>();
                listaPropeitario = Contexto.Propietario.OrderBy(P => P.NombrePrimero).Skip(inicio).Take(fin).ToList();

                return listaPropeitario;
            }
        }

        public int CountListarTodos(int inicio, int fin)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Propietario.OrderBy(P => P.NombrePrimero).Count();
            }
        }

        public bool EliminarPropietario(int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int p = Contexto.Liquidacion.Where(L => L.Propietario.IdPropietario == idPropietario).Count();

                if (p == 0)
                {
                    Propietario propietarioTmp = Contexto.Propietario.Where(P => P.IdPropietario == idPropietario).FirstOrDefault();
                    Contexto.DeleteObject(propietarioTmp);
                    Contexto.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
        }

        public StringBuilder ValidarCorreos(int idHotel)
        {
            StringBuilder lista = new StringBuilder();
            List<ObjetoGenerico> listaPropietarioTmp = this.VerTodosPorHotelPropietario(idHotel, 2);

            foreach (ObjetoGenerico item in listaPropietarioTmp)
            {
                //if (item.NumIdentificacion == "9412050008")
                //{ 
                //}

                if (!string.IsNullOrEmpty(item.Correo))
                {
                    if (!Utilities.EsCorreoValido(item.Correo.Trim().ToLower()))
                    {
                        lista.AppendLine(item.NumIdentificacion + " - " + item.PrimeroNombre + " " + item.SegundoNombre + " " + item.PrimerApellido + " " + item.SegundoApellido);
                    }
                }

                if (!string.IsNullOrEmpty(item.Correo2))
                {
                    if (!Utilities.EsCorreoValido(item.Correo2.Trim().ToLower()))
                    {
                        lista.AppendLine(item.NumIdentificacion + " - " + item.PrimeroNombre + " " + item.SegundoNombre + " " + item.PrimerApellido + " " + item.SegundoApellido);
                    }
                }

                if (!string.IsNullOrEmpty(item.Correo3))
                {
                    if (!Utilities.EsCorreoValido(item.Correo3.Trim().ToLower()))
                    {
                        lista.AppendLine(item.NumIdentificacion + " - " + item.PrimeroNombre + " " + item.SegundoNombre + " " + item.PrimerApellido + " " + item.SegundoApellido);
                    }
                }

                if (!string.IsNullOrEmpty(item.CorreoContacto))
                {
                    if (!Utilities.EsCorreoValido(item.CorreoContacto.Trim().ToLower()))
                    {
                        lista.AppendLine(item.NumIdentificacion + " - " + item.PrimeroNombre + " " + item.SegundoNombre + " " + item.PrimerApellido + " " + item.SegundoApellido);
                    }
                }
            }

            return lista;
        }
    }
}
