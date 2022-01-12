using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class CentroCostoBo
    {
        /// <summary>
        /// Guardar centro de costo
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="codigo"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public int Guardar(string nombre, string codigo, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Centro_Costo centroCostoTmp = new Centro_Costo();
                centroCostoTmp.Nombre = nombre;
                centroCostoTmp.Codigo = codigo;

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre;
                auditoriaTmp.NombreTabla = "Centro Costo";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Nombre CeCo : " + nombre + " Codigo = " + codigo;
                auditoriaTmp.NombreTabla = "Centro Costo";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Codigo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.AddToCentro_Costo(centroCostoTmp);
                Contexto.SaveChanges();

                return centroCostoTmp.IdCentroCosto;
            }
        }

        /// <summary>
        /// Lista todos los centros de costo
        /// </summary>
        /// <returns></returns>
        public List<Centro_Costo> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Centro_Costo> listaConcepto = Contexto.Centro_Costo.OrderBy(CC => CC.Nombre).ToList();
                return listaConcepto;
            }
        }

        /// <summary>
        /// Obtiene un centro costo por el id
        /// </summary>
        /// <param name="idCentroCosto"></param>
        /// <returns></returns>
        public Centro_Costo Obtener(int idCentroCosto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Centro_Costo centroCostoTmp = Contexto.Centro_Costo.Where(CC => CC.IdCentroCosto == idCentroCosto).FirstOrDefault();
                return centroCostoTmp;
            }
        }

        /// <summary>
        /// Actualiza un centro de costo
        /// </summary>
        /// <param name="idCentroCosto"></param>
        /// <param name="nombre"></param>
        /// <param name="codigo"></param>
        /// <param name="idUsuario"></param>
        public void Actualizar(int idCentroCosto, string nombre, string codigo, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Centro_Costo centroCostoTmp = Contexto.Centro_Costo.Where(CC => CC.IdCentroCosto == idCentroCosto).FirstOrDefault();

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre;
                auditoriaTmp.ValorAnterior = centroCostoTmp.Nombre;
                auditoriaTmp.NombreTabla = "Centro Costo";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Nombre CeCo : " + nombre + " Codigo = " + codigo;
                auditoriaTmp.ValorAnterior = "Nombre CeCo : " + centroCostoTmp.Nombre + " Codigo = " + centroCostoTmp.Codigo;
                auditoriaTmp.NombreTabla = "Centro Costo";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Codigo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                centroCostoTmp.Nombre = nombre;
                centroCostoTmp.Codigo = codigo;
                Contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Valida si el centro de costo esta duplicado
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="idCentroCosto"></param>
        /// <returns></returns>
        public bool EsRepetido(string codigo, int idCentroCosto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Centro_Costo> listaCeCo = Contexto.Centro_Costo.Where(CC => CC.Codigo == codigo).ToList();

                if (idCentroCosto != -1)
                    listaCeCo.RemoveAll(CC => CC.IdCentroCosto == idCentroCosto);

                return (listaCeCo.Count == 0) ? true : false;
            }
        }

        /// <summary>
        /// Obtiene un codigo de centro de costo, segun las condiciones del cdigo escrito en el metodo
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idCuentaContable"></param>
        /// <param name="idCentroCosto"></param>
        /// <param name="esCentroCostoVariable"></param>
        /// <param name="incluyeCeCo"></param>
        /// <returns></returns>
        public string ObtenerCodigoCentroCosto(int idHotel, int idCuentaContable, int idCentroCosto, bool esCentroCostoVariable, bool incluyeCeCo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                if (incluyeCeCo)
                {
                    if (esCentroCostoVariable)
                        return (from CC_H in Contexto.CentroCosto_Hotel
                                join CeCo in Contexto.Centro_Costo on CC_H.Centro_Costo.IdCentroCosto equals CeCo.IdCentroCosto
                                where CC_H.Hotel.IdHotel == idHotel && CC_H.Cuenta_Contable.IdCuentaContable == idCuentaContable
                                select CeCo.Codigo).FirstOrDefault();
                    else
                        return (from CC in Contexto.Cuenta_Contable
                                join CeCo in Contexto.Centro_Costo on CC.Centro_Costo.IdCentroCosto equals CeCo.IdCentroCosto
                                where CeCo.IdCentroCosto == idCentroCosto
                                select CeCo.Codigo).FirstOrDefault();
                }
                else
                    return "";
            }
        }
    }
}
