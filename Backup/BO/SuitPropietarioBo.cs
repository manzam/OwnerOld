using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class SuitPropietarioBo
    {
        public void Guardar(List<ObjetoGenerico> listaSuitPropietario, int idPropietario)
        {
            ValorVariableBo valorVariableBoTmp = new ValorVariableBo();

            using (ContextoOwner contexto = new ContextoOwner())
            {
                foreach (ObjetoGenerico item in listaSuitPropietario)
                {
                    Suit_Propietario suitPropietarioTmp = new Suit_Propietario();
                    suitPropietarioTmp.NumCuenta = item.NumCuenta;
                    suitPropietarioTmp.NumEstadias = item.NumEstadias;
                    suitPropietarioTmp.TipoCuenta = item.TipoCuenta;
                    suitPropietarioTmp.Titular = item.Titular;
                    suitPropietarioTmp.SuitReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit", "IdSuit", item.IdSuit);
                    suitPropietarioTmp.PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Propietario", "IdPropietario", idPropietario);
                    suitPropietarioTmp.BancoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Banco", "IdBanco", item.IdBanco);

                    contexto.AddToSuit_Propietario(suitPropietarioTmp);
                    contexto.SaveChanges();

                    foreach (ObjetoGenerico itemValor in item.ListaVariables)
                    {
                        valorVariableBoTmp.GuardarSuit(suitPropietarioTmp.IdSuitPropietario, itemValor.IdVariable.Value, itemValor.Valor);
                    }
                }
            }
        }

        public void Guardar(List<Suit_Propietario> listaSuitPropietario)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                foreach (Suit_Propietario item in listaSuitPropietario)
                {
                    contexto.AddToSuit_Propietario(item);                    
                }
                contexto.SaveChanges();
            }
        }
        
        public int Guardar(Suit_Propietario suitPropietario, string nombreBanco, string nombrePropietario, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                #region auditoria

                string valor = string.Empty;
                string nomVariable = string.Empty;
                DateTime fecha = DateTime.Now;
                Auditoria auditoriaTmp;

                int idSuite = (int)suitPropietario.SuitReference.EntityKey.EntityKeyValues[0].Value;
                var nombreHotelSuite = Contexto.Suit.Where(S => S.IdSuit == idSuite).Select(S => new { S.NumSuit, S.Hotel.Nombre }).FirstOrDefault();

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + nombrePropietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Num. Cuenta = " + suitPropietario.NumCuenta;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Num. Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + nombrePropietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Tipo Cuenta = " + suitPropietario.TipoCuenta;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Tipo Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + nombrePropietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Banco = " + nombreBanco;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Banco";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + nombrePropietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Titular Cuenta = " + suitPropietario.Titular;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Titular Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                #endregion

                Contexto.AddToSuit_Propietario(suitPropietario);
                Contexto.SaveChanges();

                return suitPropietario.IdSuitPropietario;
            }
        }

        public int Guardar(Suit_Propietario suitPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToSuit_Propietario(suitPropietario);
                Contexto.SaveChanges();

                return suitPropietario.IdSuitPropietario;
            }
        }

        public void Actualizar(int idSuitPropietario, int idBanco, string numCuenta, int numEstadias, string titular, string tipoCuenta, string nombreBanco, string propietario, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Suit_Propietario suitPropietarioTmp = Contexto.Suit_Propietario.Where(SP => SP.IdSuitPropietario == idSuitPropietario).FirstOrDefault();

                #region auditoria

                string valor = string.Empty;
                string nomVariable = string.Empty;
                DateTime fecha = DateTime.Now;
                Auditoria auditoriaTmp;

                int idBancoOld = (int)suitPropietarioTmp.BancoReference.EntityKey.EntityKeyValues[0].Value;
                string nombreBancoOld = Contexto.Banco.Where(B => B.IdBanco == idBancoOld).Select(B => B.Nombre).FirstOrDefault();

                var nombreHotelSuite = (from SP in Contexto.Suit_Propietario
                                        join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                        join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                        select new { S.NumSuit, H.Nombre }).FirstOrDefault();

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Num. Cuenta = " + numCuenta;
                auditoriaTmp.ValorAnterior = "Propietario: " + propietario + " Hotel : " + nombreBancoOld + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Num. Cuenta = " + suitPropietarioTmp.NumCuenta;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Num. Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Tipo Cuenta = " + tipoCuenta;
                auditoriaTmp.ValorAnterior = "Propietario: " + propietario + " Hotel : " + nombreBancoOld + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Tipo Cuenta = " + suitPropietarioTmp.TipoCuenta;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Tipo Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Banco = " + nombreBanco;
                auditoriaTmp.ValorAnterior = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Banco = " + nombreBancoOld;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Banco";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Titular Cuenta = " + titular;
                auditoriaTmp.ValorAnterior = "Propietario: " + propietario + " Hotel : " + nombreBancoOld + " Num. Suite : " + nombreHotelSuite.NumSuit + " : Titular Cuenta = " + suitPropietarioTmp.Titular;
                auditoriaTmp.NombreTabla = "Detalle Suite";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Titular Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                #endregion

                suitPropietarioTmp.NumCuenta = numCuenta;
                suitPropietarioTmp.NumEstadias = numEstadias;
                suitPropietarioTmp.Titular = titular;
                suitPropietarioTmp.TipoCuenta = tipoCuenta;
                suitPropietarioTmp.BancoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Banco", "IdBanco", idBanco);

                Contexto.SaveChanges();
            }
        }

        public void Eliminar(int idSuitPropietario, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Suit_Propietario suitPropietarioTmp = Contexto.Suit_Propietario.Where(SP => SP.IdSuitPropietario == idSuitPropietario).FirstOrDefault();

                #region auditoria

                int idSuite = (int)suitPropietarioTmp.SuitReference.EntityKey.EntityKeyValues[0].Value;
                Suit suiteTmp = Contexto.Suit.Include("Hotel").Where(S => S.IdSuit == idSuite).FirstOrDefault();

                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + suiteTmp.Hotel.Nombre + " Num. Suite : " + suiteTmp.NumSuit + " Num. Escritura : " + suiteTmp.NumEscritura + " Registro Notaria : " + suiteTmp.RegistroNotaria;
                auditoriaTmp.NombreTabla = "Suite";
                auditoriaTmp.Accion = "Eliminar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.DeleteObject(suitPropietarioTmp);
                Contexto.SaveChanges();
            }
        }

        public bool EsSuitEliminada(int idSuitPropietario, int idPropietario)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                int idSuiteTmp = contexto.Suit_Propietario.Where(SP => SP.IdSuitPropietario == idSuitPropietario).Select(SP => SP.Suit.IdSuit).FirstOrDefault();

                int con = contexto.Liquidacion.Where(L => L.Suit.IdSuit == idSuiteTmp && L.Propietario.IdPropietario == idPropietario).Count();


                //int con = (from VC in contexto.Variables_Concepto
                //           join V in contexto.Variable on VC.Variable.IdVariable equals V.IdVariable
                //           join VVS in contexto.Valor_Variable_Suit on V.IdVariable equals VVS.Variable.IdVariable
                //           join C in contexto.Concepto on VC.Concepto.IdConcepto equals C.IdConcepto
                //           join L in contexto.Liquidacion on C.IdConcepto equals L.Concepto.IdConcepto
                //           where VVS.Suit_Propietario.IdSuitPropietario == idSuitPropietario
                //           select VC).Count();

                return (con == 0) ? true : false;
            }
        }

        public List<Hotel> HotelesPorPropietario(int idPropietario)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<Hotel> listaHotel = new List<Hotel>();
                listaHotel = (from P in contexto.Propietario
                              join SP in contexto.Suit_Propietario on P.IdPropietario equals SP.Propietario.IdPropietario
                              join S in contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                              join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                              join C in contexto.Ciudad on H.Ciudad.IdCiudad equals C.IdCiudad
                              where P.IdPropietario == idPropietario
                              select H).Distinct().ToList();

                return listaHotel;
            }
        }

        public List<int> ListaIdsuitPropietario(int idHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                List<int> listaId = (from SP in contexto.Suit_Propietario
                                     join S in contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                     join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                     where H.IdHotel == idHotel
                                     select SP.IdSuitPropietario).ToList();

                listaId = listaId.Distinct().ToList();
                return listaId;
            }
        }

        public Suit_Propietario Obtener(int idPropietario, int idSuit)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                return contexto.
                       Suit_Propietario.
                       Include("Suit").
                       Where(SP => SP.Suit.IdSuit == idSuit && SP.Propietario.IdPropietario == idPropietario).
                       FirstOrDefault();
            }
        }

        public Suit_Propietario Obtener(string numIdentificacion, string NumSuiteEscritura, string codHotel)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                return (from SP in contexto.Suit_Propietario
                        join S in contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                        join P in contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                        join H in contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                        where P.NumIdentificacion == numIdentificacion &&
                              S.NumEscritura == NumSuiteEscritura &&
                              H.Codigo == codHotel
                        select SP).FirstOrDefault();
            }
        }

        public void SetEstadoSuitPropietario(int idSuitePropietario, bool esActivo)
        {
            using (ContextoOwner contexto = new ContextoOwner())
            {
                Suit_Propietario suitePropietario = contexto.Suit_Propietario.Where(SP => SP.IdSuitPropietario == idSuitePropietario).FirstOrDefault();
                suitePropietario.EsActivo = esActivo;
                contexto.SaveChanges();
            }
        }
    }
}
