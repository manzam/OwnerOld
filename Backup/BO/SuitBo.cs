using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class SuitBo
    {
        public int Guardar(string descripcion, bool activo, string numSuit, string escritura, 
                           string registroNotaria, int idHotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string nombreHotel = Contexto.Hotel.Where(H => H.IdHotel == idHotel).Select(H => H.Nombre).FirstOrDefault();

                Suit suitTmp = new Suit();
                suitTmp.Descripcion = descripcion;
                suitTmp.Activo = activo;
                suitTmp.NumSuit = numSuit;
                suitTmp.NumEscritura = escritura;
                suitTmp.RegistroNotaria = registroNotaria;
                suitTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " : " + numSuit;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Num. Suite";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Num. Suite : " + numSuit + " : " + escritura;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Num. Escritura";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Num. Suite : " + numSuit + " : " + registroNotaria;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Registro Notaria";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Num. Suite : " + numSuit + " : " + ((activo) ? "Si" : "No");
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Activo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.AddToSuit(suitTmp);
                Contexto.SaveChanges();

                return suitTmp.IdSuit;
            }
        }

        public int Guardar(Suit suit, int idUsuario)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                contexto.AddToSuit(suit);
                contexto.SaveChanges();

                return suit.IdSuit;
            }
        }        

        public void Guardar(List<Suit> listaSuit)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                foreach (Suit suitTmp in listaSuit)
                {
                    contexto.AddToSuit(suitTmp);                    
                }
                contexto.SaveChanges();
            }
        }

        public void Actualizar(int idSuit, string descripcion, bool activo, string numSuit, string escritura,
                               string registroNotaria, int idHotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Suit suitTmp = Contexto.Suit.Where(S => S.IdSuit == idSuit).FirstOrDefault();                

                #region auditoria
                int idHotelOld = (int)suitTmp.HotelReference.EntityKey.EntityKeyValues[0].Value;
                string nombreHotel = Contexto.Hotel.Where(H => H.IdHotel == idHotel).Select(H => H.Nombre).FirstOrDefault();
                string nombreHotelOld = Contexto.Hotel.Where(H => H.IdHotel == idHotelOld).Select(H => H.Nombre).FirstOrDefault();

                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " : " + numSuit;
                auditoriaTmp.ValorAnterior = "Hotel : " + nombreHotelOld + " : " + suitTmp.NumSuit;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Num. Suite";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Num. Suite : " + numSuit + " : " + escritura;
                auditoriaTmp.ValorAnterior = "Hotel : " + nombreHotelOld + " Num. Suite : " + suitTmp.NumSuit + " : " + suitTmp.NumEscritura;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Num. Escritura";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Num. Suite : " + numSuit + " : " + registroNotaria;
                auditoriaTmp.ValorAnterior = "Hotel : " + nombreHotelOld + " Num. Suite : " + suitTmp.NumSuit + " : " + suitTmp.RegistroNotaria;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Registro Notaria";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Num. Suite : " + numSuit + " : " + ((activo) ? "Si" : "No");
                auditoriaTmp.ValorAnterior = "Hotel : " + nombreHotelOld + " Num. Suite : " + suitTmp.NumSuit + " : " + ((suitTmp.Activo) ? "Si" : "No");
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Activo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                suitTmp.Descripcion = descripcion;
                suitTmp.Activo = activo;
                suitTmp.NumSuit = numSuit;
                suitTmp.NumEscritura = escritura;
                suitTmp.RegistroNotaria = registroNotaria;

                Contexto.SaveChanges();
            }
        }

        public List<Suit> ObtenerSuitsPorHotel(int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<Suit> listaSuit = new List<Suit>();
                listaSuit = contexto.Suit.Include("Hotel").Where(S => S.Hotel.IdHotel == idHotel).OrderBy(S => S.NumSuit).ToList();
                
                return listaSuit;
            }
        }

        public List<ObjetoGenerico> ObtenerSuitsPorHotel2(int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaSuit = new List<ObjetoGenerico>();
                listaSuit = (from S in contexto.Suit
                             where S.Hotel.IdHotel == idHotel
                             select new ObjetoGenerico()
                             {
                                 IdSuit = S.IdSuit,
                                 NumSuit = "Escritura: " + S.NumEscritura + " N° Suite: " + S.NumSuit
                             }).OrderBy(S => S.NumSuit).ToList();

                return listaSuit;
            }
        }

        public List<ObjetoGenerico> ObtenerSuitsPorHotelCargaMasiva(int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaSuit = new List<ObjetoGenerico>();

                listaSuit = (from H in contexto.Hotel
                             join S in contexto.Suit on H.IdHotel equals S.Hotel.IdHotel
                             select new ObjetoGenerico() 
                             {
                                 Codigo = H.Codigo,
                                 NumEscritura = S.NumEscritura
                             }).ToList();

                return listaSuit;
            }
        }

        public List<Suit> ObtenerSuitsPorHotel(int idHotel, int inicio, int fin)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<Suit> listaSuit = new List<Suit>();
                listaSuit = contexto.Suit.Where(S => S.Hotel.IdHotel == idHotel).Skip(inicio).Take(fin).ToList();

                return listaSuit;
            }
        }

        public List<ObjetoGenerico> ObtenerSuitsPorHotelFull(int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaSuit = new List<ObjetoGenerico>();
                listaSuit = (from S in contexto.Suit
                             join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                             join C in contexto.Ciudad on H.Ciudad.IdCiudad equals C.IdCiudad
                             select new ObjetoGenerico()
                             {
                                 IdSuit = S.IdSuit,
                                 NumSuit = S.NumSuit,
                                 NombreCiudad = C.Nombre,
                                 Descripcion = S.Descripcion
                             }).ToList();

                return listaSuit;
            }
        }

        public List<ObjetoGenerico> ObtenerSuitsPorPropietario(int idPropietario)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaSuit = new List<ObjetoGenerico>();
                listaSuit = (from S in contexto.Suit
                             join SP in contexto.Suit_Propietario on S.IdSuit equals SP.Suit.IdSuit
                             join B in contexto.Banco on SP.Banco.IdBanco equals B.IdBanco
                             join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                             where SP.Propietario.IdPropietario == idPropietario
                             select new ObjetoGenerico()
                             {
                                 IdSuit = S.IdSuit,
                                 IdHotel = H.IdHotel,
                                 IdBanco = B.IdBanco,
                                 IdSuitPropietario = SP.IdSuitPropietario,
                                 Descripcion = S.Descripcion,
                                 NumCuenta = SP.NumCuenta,
                                 NombreBanco = B.Nombre,
                                 NumSuit = S.NumSuit,
                                 NumEscritura = S.NumEscritura,
                                 NombreHotel = H.Nombre,
                                 NumEstadias = SP.NumEstadias,
                                 Titular = SP.Titular,
                                 TipoCuenta = SP.TipoCuenta,
                                 Estado = (SP.EsActivo) ? "Inactivar" : "Activar"
                             }).ToList();

                return listaSuit;
            }
        }

        public ObjetoGenerico ObtenerSuitPorPropietario(int idSuitPropietario)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                ObjetoGenerico suitTmp = new ObjetoGenerico();
                suitTmp = (from S in contexto.Suit
                             join SP in contexto.Suit_Propietario on S.IdSuit equals SP.Suit.IdSuit
                             join B in contexto.Banco on SP.Banco.IdBanco equals B.IdBanco
                             join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                             where SP.IdSuitPropietario == idSuitPropietario
                             select new ObjetoGenerico()
                             {
                                 IdSuit = S.IdSuit,
                                 IdHotel = H.IdHotel,
                                 IdBanco = B.IdBanco,
                                 IdSuitPropietario = SP.IdSuitPropietario,
                                 Descripcion = S.Descripcion,
                                 NumCuenta = SP.NumCuenta,
                                 NombreBanco = B.Nombre,
                                 NumSuit = S.NumSuit,
                                 NumEscritura = S.NumEscritura,
                                 NombreHotel = H.Nombre,
                                 NumEstadias = SP.NumEstadias,
                                 Titular = SP.Titular,
                                 TipoCuenta = SP.TipoCuenta
                             }).FirstOrDefault();

                return suitTmp;
            }
        }

        public List<Suit> ObtenerSuitesPorPropietario(int idPropietario, int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<Suit> listaSuit = new List<Suit>();
                listaSuit = (from SP in contexto.Suit_Propietario
                             join S in contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                             join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                             where H.IdHotel == idHotel && SP.Propietario.IdPropietario == idPropietario
                             select S).ToList();

                return listaSuit;
            }
        }

        public Suit ObtenerSuit(int idSuit)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                Suit suitTmp = contexto.Suit.Where(S => S.IdSuit == idSuit).FirstOrDefault();
                return suitTmp;
            }
        }

        public Suit ObtenerSuitByNumSuite(string numSuit, string codigoHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                Suit suitTmp = (from S in contexto.Suit
                                join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                where S.NumSuit == numSuit && H.Codigo == codigoHotel
                                select S).FirstOrDefault();
                return suitTmp;
            }
        }

        public Suit ObtenerSuitByEscritura(string numEscritura, string codigoHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                Suit suitTmp = (from S in contexto.Suit
                                join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                where S.NumEscritura == numEscritura && H.Codigo == codigoHotel
                                select S).FirstOrDefault();
                return suitTmp;
            }
        }

        public List<Suit> VerTodos()
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<Suit> listaSuite = new List<Suit>();
                listaSuite = contexto.Suit.Include("Hotel").ToList();
                return listaSuite;
            }
        }

        public Suit ObtenerSuit(string numEscritura)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                Suit suitTmp = contexto.Suit.Where(S => S.NumEscritura == numEscritura).FirstOrDefault();
                return suitTmp;
            }
        }

        public bool Eliminar(int idSuit, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                if (Contexto.Suit_Propietario.Where(S => S.Suit.IdSuit == idSuit).Count() > 0)
                    return false;
                else
                {
                    Suit suitTmp = Contexto.Suit.Where(S => S.IdSuit == idSuit).FirstOrDefault();

                    #region auditoria
                    int idhotel = (int)suitTmp.HotelReference.EntityKey.EntityKeyValues[0].Value;
                    string nombreHotel = Contexto.Hotel.Where(H => H.IdHotel == idhotel).Select(H => H.Nombre).FirstOrDefault();

                    Auditoria auditoriaTmp;

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " : " + suitTmp.NumSuit;
                    auditoriaTmp.NombreTabla = "Suite";
                    auditoriaTmp.Accion = "Eliminar";
                    auditoriaTmp.Campo = "Num. Suite";
                    auditoriaTmp.Fechahora = DateTime.Now;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    Contexto.DeleteObject(suitTmp);
                    Contexto.SaveChanges();
                    return true;
                }
            }
        }

        public bool EsRepetido(string numSuit, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int count = Contexto.Suit.Where(S => S.NumSuit == numSuit && S.Hotel.IdHotel == idHotel).Count();
                return (count > 0) ? true : false;
            }
        }

        public List<Suit> ObtenerSuit(string numSuit, int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<Suit> listaSuite = new List<Suit>();
                listaSuite = contexto.Suit.Where(S => S.Hotel.IdHotel == idHotel && S.NumSuit.Contains(numSuit)).ToList();
                return listaSuite;
            }
        }
    }
}
