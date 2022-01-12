using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DM;
using Servicios;

namespace BO
{
    public class CentroCostoHotelBo
    {
        /// <summary>
        /// Lista todos los centro costo - hotel por id del hotel
        /// </summary>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public DataTable VerTodos(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string sql = "SELECT " +
                             "  Hotel.Nombre AS NombreHotel, " +
                             "  CentroCosto_Hotel.CodigoTercero AS Codigo, " +
                             "  Cuenta_Contable.Codigo + ' - ' + Cuenta_Contable.Nombre AS NombreCuentaContable, " +
                             "  Centro_Costo.Nombre AS NombreCentroCosto, " +
                             "  CentroCosto_Hotel.IdCentroCosto_Hotel, " +
                             "  CentroCosto_Hotel.EsConBase, " +
                             "  CentroCosto_Hotel.IdConcepto, " +
                             "  Cuenta_Contable.IdCuentaContable, " +                             
                             "  Centro_Costo.IdCentroCosto, " +
                             "  CentroCosto_Hotel.EsTerceroVariable " +                             
                             "FROM  CentroCosto_Hotel " +
                             "INNER JOIN  Hotel ON CentroCosto_Hotel.IdHotel = Hotel.IdHotel " +
                             "INNER JOIN  Cuenta_Contable ON CentroCosto_Hotel.IdCuentaContable = Cuenta_Contable.IdCuentaContable " +
                             "FULL JOIN Centro_Costo ON CentroCosto_Hotel.IdCentroCosto = Centro_Costo.IdCentroCosto " +
                             "WHERE (Hotel.IdHotel = " + idHotel + ")";

                DataTable dtTmp = Utilities.Select(sql,"CentroCosto");
                return dtTmp;
            }
        }

        /// <summary>
        /// guarda un centro costo - hotel
        /// </summary>
        /// <param name="centroCostoHotel"></param>
        /// <param name="nombreHotel"></param>
        /// <param name="cuentaContable"></param>
        /// <param name="ceCo"></param>
        /// <param name="codigoTercero"></param>
        /// <param name="esConBase"></param>
        /// <param name="nombreConcepto"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public int Guardar(CentroCosto_Hotel centroCostoHotel, string nombreHotel, string cuentaContable, string ceCo, string codigoTercero, bool esConBase, string nombreConcepto, int idUsuario, int idCuentaContable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToCentroCosto_Hotel(centroCostoHotel);

                Cuenta_Contable oCuenta_Contable = Contexto.Cuenta_Contable.Where(CC => CC.IdCuentaContable == idCuentaContable).FirstOrDefault();
                if (oCuenta_Contable != null)
                {
                    if (ceCo != "-1")
                        oCuenta_Contable.IncluyeCeCo = true;
                    else
                        oCuenta_Contable.IncluyeCeCo = false;
                }

                #region auditoria
                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Cuenta Contable : " + cuentaContable + " Centro Costo : " + ceCo + " Codigo Tercero : " + codigoTercero + " Tiene Base: " + ((esConBase) ? "Si" : "No") + " Concepto: " + nombreConcepto;
                auditoriaTmp.NombreTabla = "Centro Costo - Hotel - Cuenta Contable";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Relacion";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.SaveChanges();

                return centroCostoHotel.IdCentroCosto_Hotel;
            }
        }

        /// <summary>
        /// Actualiza un centro costo - hotel
        /// </summary>
        /// <param name="idCentroCosto_Hotel"></param>
        /// <param name="idHotel"></param>
        /// <param name="idCentroCosto"></param>
        /// <param name="codigoTercero"></param>
        /// <param name="nombreHotel"></param>
        /// <param name="cuentaContable"></param>
        /// <param name="ceCo"></param>
        /// <param name="esConBase"></param>
        /// <param name="nombreConcepto"></param>
        /// <param name="idConcepto"></param>
        /// <param name="esTerceroVariable"></param>
        /// <param name="idUsuario"></param>
        public void Actualizar(int idCentroCosto_Hotel, int idHotel, int idCentroCosto, string codigoTercero, string nombreHotel, string cuentaContable, string ceCo, bool esConBase, string nombreConcepto, int idConcepto, bool esTerceroVariable, int idUsuario, int idCuentaContable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                CentroCosto_Hotel centroCostoHotel = Contexto.CentroCosto_Hotel.Include("Hotel").Include("Cuenta_Contable").Include("Centro_Costo").Include("Concepto").Where(CCH => CCH.IdCentroCosto_Hotel == idCentroCosto_Hotel).FirstOrDefault();

                Cuenta_Contable oCuenta_Contable = Contexto.Cuenta_Contable.Where(CC => CC.IdCuentaContable == idCuentaContable).FirstOrDefault();
                if (oCuenta_Contable != null)
                {
                    if (ceCo != "-1")
                        oCuenta_Contable.IncluyeCeCo = true;
                    else
                        oCuenta_Contable.IncluyeCeCo = false;
                }

                #region auditoria
                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = "Hotel : " + nombreHotel + " Cuenta Contable : " + cuentaContable + " Centro Costo : " + ceCo + " Codigo Tercero : " + codigoTercero + " Tiene Base: " + ((esConBase) ? "Si" : "No") + " Concepto: " + nombreConcepto + " Tercero Variable: " + ((esTerceroVariable) ? "Si" : "No");
                auditoriaTmp.ValorAnterior = "Hotel : " + centroCostoHotel.Hotel.Nombre + " Cuenta Contable : " + centroCostoHotel.Cuenta_Contable.Nombre + " Centro Costo : " + ((centroCostoHotel.Centro_Costo != null) ? centroCostoHotel.Centro_Costo.Nombre : "") + " Codigo Tercero : " + centroCostoHotel.CodigoTercero + " Tiene Base: " + ((centroCostoHotel.EsConBase) ? "Si" : "No") + " Concepto: " + ((centroCostoHotel.EsConBase) ? centroCostoHotel.Concepto.Nombre : "" + " Tercero Variable: " + ((esTerceroVariable) ? "Si" : "No"));
                auditoriaTmp.NombreTabla = "Centro Costo - Hotel - Cuenta Contable";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Relacion";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                centroCostoHotel.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                centroCostoHotel.CodigoTercero = codigoTercero;
                centroCostoHotel.EsTerceroVariable = esTerceroVariable;

                if (esConBase)
                {
                    centroCostoHotel.EsConBase = true;
                    centroCostoHotel.ConceptoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", idConcepto);
                }
                else
                {
                    centroCostoHotel.EsConBase = false;
                    centroCostoHotel.Concepto = null;
                }

                if (idCentroCosto != -1)
                    centroCostoHotel.Centro_CostoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Centro_Costo", "IdCentroCosto", idCentroCosto);
                else
                    centroCostoHotel.Centro_Costo = null;

                Contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Obtengo un centro costo - hotel por id cuenta contable y is hotel
        /// </summary>
        /// <param name="idCuentaContable"></param>
        /// <param name="idHotel"></param>
        /// <returns></returns>
        public CentroCosto_Hotel Obtener(int idCuentaContable, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.
                       CentroCosto_Hotel.
                       Include("Cuenta_Contable").
                       Include("Centro_Costo").
                       Where(CCH => CCH.Cuenta_Contable.IdCuentaContable == idCuentaContable && CCH.Hotel.IdHotel == idHotel).
                       FirstOrDefault();
            }
        }

        /// <summary>
        /// Elimina un centro consto - hotel por id cuenta contable y is hotel
        /// </summary>
        /// <param name="idCuentaContable"></param>
        /// <param name="idHotel"></param>
        public void Eliminar(int idCuentaContable, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                CentroCosto_Hotel CentroCosto_HotelTmp = Contexto.CentroCosto_Hotel.Where(CCH => CCH.IdCentroCosto_Hotel == idCuentaContable && CCH.Hotel.IdHotel == idHotel).FirstOrDefault();
                Contexto.DeleteObject(CentroCosto_HotelTmp);
                Contexto.SaveChanges();
            }
        }
    }
}
