using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class CuentaContableBo
    {
        public List<ObjetoGenerico> VerTodos(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaCuentaContable = new List<ObjetoGenerico>();
                List<int> listaCuentaContableHotel = new List<int>();

                listaCuentaContable = (from CC in Contexto.Cuenta_Contable
                                       select new ObjetoGenerico()
                                       {
                                           IdCuentaContable = CC.IdCuentaContable,
                                           Nombre = CC.Codigo + " - " + CC.Nombre,
                                           NombreCuentaContable = CC.Codigo + " - " + CC.Nombre,
                                           Descripcion = CC.Nombre,
                                           TipoCuenta = CC.Tipo_Cuenta_Contable.Nombre,
                                           Codigo = CC.Codigo
                                       }).ToList();

                listaCuentaContableHotel = Contexto.
                                           CentroCosto_Hotel.
                                           Where(C => C.Hotel.IdHotel == idHotel).
                                           Select(C => C.Cuenta_Contable.IdCuentaContable).ToList();

                foreach (int itemIdCuCo in listaCuentaContableHotel)
                {
                    listaCuentaContable.RemoveAll(C => C.IdCuentaContable == itemIdCuCo);
                }

                return listaCuentaContable;
            }
        }

        public List<ObjetoGenerico> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaCuentaContable = (from CC in Contexto.Cuenta_Contable
                                                            select new ObjetoGenerico()
                                                            {
                                                                IdCuentaContable = CC.IdCuentaContable,
                                                                Nombre = CC.Codigo + " - " + CC.Nombre,
                                                                NombreCuentaContable = CC.Codigo + " - " + CC.Nombre,
                                                                Descripcion = CC.Nombre,
                                                                TipoCuenta = CC.Tipo_Cuenta_Contable.Nombre,
                                                                Codigo = CC.Codigo
                                                            }).ToList();
                return listaCuentaContable;
            }
        }

        public List<ObjetoGenerico> VerTodos(bool esCentroCostoVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaCuentaContable = (from CC in Contexto.Cuenta_Contable
                                                            where CC.EsCentroCostoVariable == esCentroCostoVariable
                                                            select new ObjetoGenerico()
                                                            {
                                                                IdCuentaContable = CC.IdCuentaContable,
                                                                Nombre = CC.Codigo + " - " + CC.Nombre,
                                                                Descripcion = CC.Nombre,
                                                                TipoCuenta = CC.Tipo_Cuenta_Contable.Nombre,
                                                                Codigo = CC.Codigo
                                                            }).ToList();
                return listaCuentaContable;
            }
        }

        public Cuenta_Contable Obtener(int idCuentaContable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Cuenta_Contable cuentaContableTmp = Contexto.
                                                  Cuenta_Contable.
                                                  Include("Tipo_Cuenta_Contable").
                                                  Where(CC => CC.IdCuentaContable == idCuentaContable).FirstOrDefault();
                return cuentaContableTmp;
            }
        }

        public int Guardar(string codigo, string nombre, bool esCentroCostoVariable, bool esDocumentoCruce,
                           string encabezadoDocCruce, string naturalezaCruce, int idTipoCuenta, string unidadNegocio, 
                           string nombreTipoCuenta, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Cuenta_Contable cuentaContableTmp = new Cuenta_Contable();
                cuentaContableTmp.Codigo = codigo;
                cuentaContableTmp.Nombre = nombre;
                cuentaContableTmp.EsCentroCostoVariable = esCentroCostoVariable;
                cuentaContableTmp.TieneDocumentoCruce = esDocumentoCruce;
                cuentaContableTmp.EncabezadoDocCruce = encabezadoDocCruce;
                cuentaContableTmp.NaturalezaCuenta = naturalezaCruce;
                cuentaContableTmp.UnidadNegocio = unidadNegocio;
                cuentaContableTmp.Tipo_Cuenta_ContableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Tipo_Cuenta_Contable", "IdTipoCuentaContable", idTipoCuenta);




                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + codigo;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Codigo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + ((esCentroCostoVariable) ? "Si" : "No");
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Es Centro Costo Variable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + ((esDocumentoCruce) ? "Si" : "No");
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Tiene Documento Cruce";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + naturalezaCruce;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Naturaleza Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + unidadNegocio;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Unidad Negocio";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + nombreTipoCuenta;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Tipo Cuenta Contable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.AddToCuenta_Contable(cuentaContableTmp);
                Contexto.SaveChanges();

                return cuentaContableTmp.IdCuentaContable;
            }
        }

        public void Actualizar(int idCuentaContable, string codigo, string nombre, bool esCentroCostoVariable, bool esDocumentoCruce,
                               string encabezadoDocCruce, string naturalezaCruce, int idTipoCuenta, string unidadNegocio,
                               string nombreTipoCuenta, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Cuenta_Contable cuentaContableTmp = Contexto.
                                                  Cuenta_Contable.
                                                  Where(CC => CC.IdCuentaContable == idCuentaContable).FirstOrDefault();
                
                #region auditoria
                int idTipoCuentaOld = (int)cuentaContableTmp.Tipo_Cuenta_ContableReference.EntityKey.EntityKeyValues[0].Value;
                string nombreTipoCuentaOld = Contexto.Tipo_Cuenta_Contable.Where(T => T.IdTipoCuentaContable == idTipoCuentaOld).Select(T => T.Nombre).FirstOrDefault();

                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre;
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + codigo;
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre + " : " + cuentaContableTmp.Codigo;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Codigo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + ((esCentroCostoVariable) ? "Si" : "No");
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre + " : " + ((cuentaContableTmp.EsCentroCostoVariable) ? "Si" : "No");
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Es Centro Costo Variable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + ((esDocumentoCruce) ? "Si" : "No");
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre + " : " + ((cuentaContableTmp.TieneDocumentoCruce.Value) ? "Si" : "No");
                auditoriaTmp.NombreTabla = "Usuario";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Tiene Documento Cruce";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + naturalezaCruce;
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre + " : " + cuentaContableTmp.NaturalezaCuenta;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Naturaleza Cuenta";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + unidadNegocio;
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre + " : " + cuentaContableTmp.UnidadNegocio;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Unidad Negocio";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + nombreTipoCuenta;
                auditoriaTmp.ValorAnterior = cuentaContableTmp.Nombre + " : " + nombreTipoCuentaOld;
                auditoriaTmp.NombreTabla = "Cuenta Contable";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Tipo Cuenta Contable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                #endregion

                cuentaContableTmp.Codigo = codigo;
                cuentaContableTmp.Nombre = nombre;
                cuentaContableTmp.EsCentroCostoVariable = esCentroCostoVariable;
                cuentaContableTmp.TieneDocumentoCruce = esDocumentoCruce;
                cuentaContableTmp.EncabezadoDocCruce = encabezadoDocCruce;
                cuentaContableTmp.NaturalezaCuenta = naturalezaCruce;
                cuentaContableTmp.UnidadNegocio = unidadNegocio;
                cuentaContableTmp.Tipo_Cuenta_ContableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Tipo_Cuenta_Contable", "IdTipoCuentaContable", idTipoCuenta);

                Contexto.SaveChanges();
            }
        }

        public bool EsRepetido(string codigo, int idCuentaContable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Cuenta_Contable> listaCuentaContable = Contexto.Cuenta_Contable.Where(CC => CC.Codigo == codigo).ToList();

                if (idCuentaContable != -1)
                    listaCuentaContable.RemoveAll(CC => CC.IdCuentaContable == idCuentaContable);

                return (listaCuentaContable.Count == 0) ? true : false;
            }
        }

        public bool EliminarCuentaContable(int idCuentaContable, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int i = 0;
                i = Contexto.Concepto.Where(C => C.Cuenta_Contable.IdCuentaContable == idCuentaContable).Count();
                i += Contexto.CentroCosto_Hotel.Where(C => C.Cuenta_Contable.IdCuentaContable == idCuentaContable).Count();

                if (i == 0)
                {
                    Cuenta_Contable cuentaContableTmp = Contexto.Cuenta_Contable.Where(C => C.IdCuentaContable == idCuentaContable).FirstOrDefault();
                    
                    #region auditoria
                    Auditoria auditoriaTmp;

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = cuentaContableTmp.Nombre;
                    auditoriaTmp.NombreTabla = "Cuenta Contable";
                    auditoriaTmp.Accion = "Eliminar";
                    auditoriaTmp.Campo = "Nombre";
                    auditoriaTmp.Fechahora = DateTime.Now;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    Contexto.DeleteObject(cuentaContableTmp);
                    Contexto.SaveChanges();

                    return true;
                }
                else
                    return false;
                
            }
        }
    }
}
