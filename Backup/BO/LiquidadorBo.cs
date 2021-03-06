using DM;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Transactions;
using System.Data;
using System;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace BO
{
    public class LiquidadorBo
    {
        public List<LiquidadorOwners> ObtenerPropietariosConSuiteActivas(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<LiquidadorOwners> listaPropietarios = new List<LiquidadorOwners>();
                listaPropietarios = (from SP in Contexto.Suit_Propietario
                                     join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                     join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                     where S.Hotel.IdHotel == idHotel && SP.EsActivo == true
                                     select new LiquidadorOwners()
                                     {
                                         IdOwner = P.IdPropietario,
                                         IdSuite = S.IdSuit,
                                         Nombre = P.NombrePrimero,
                                         FullNombre = P.NombrePrimero + " " + P.NombreSegundo + " " + P.ApellidoPrimero + " " + P.ApellidoSegundo,
                                         NumSuite = S.NumSuit,
                                         NumEscritura = S.NumEscritura,
                                         NumIdentificacion = P.NumIdentificacion,
                                         NomConcepto = ""
                                     }).OrderBy(P => new { P.Nombre, P.NumSuite }).ToList();
                return listaPropietarios;
            }
        }

        public bool LiquidacionHotel(int idHotel, int yyyy, int mm)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Liquidacion.Where(L => L.FechaPeriodoLiquidado.Year == yyyy &&
                                                      L.FechaPeriodoLiquidado.Month == mm &&
                                                      L.Hotel.IdHotel == idHotel).Count();

                return (con == 0) ? false : true;
            }
        }

        public List<LiquidadorRegla> ObtenerReglaHotel(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<LiquidadorRegla> listaRegla = new List<LiquidadorRegla>();

                List<Concepto> listaConcepto = Contexto.Concepto.Where(C => C.Hotel.IdHotel == idHotel && C.NivelConcepto == 1 && C.EsActiva == true).ToList();
                foreach (Concepto itemConcepto in listaConcepto)
                {
                    LiquidadorRegla reglaTmp = new LiquidadorRegla();
                    reglaTmp.NombreConcepto = itemConcepto.Nombre;
                    reglaTmp.IdConcepto = itemConcepto.IdConcepto;
                    reglaTmp.NumDecimales = itemConcepto.NumDecimales;
                    reglaTmp.Orden = itemConcepto.Orden;
                    string regla = string.Empty;

                    List<Variables_Concepto> listaVariableConcpeto = Contexto.Variables_Concepto.Include("Concepto1").Include("Variable").Where(VC => VC.Concepto.IdConcepto == itemConcepto.IdConcepto).OrderBy(VC => VC.Orden).ToList();
                    reglaTmp.ListaConceptos = new List<LiquidadorReglaConceptos>();
                    foreach (Variables_Concepto itemVariableConcpeto in listaVariableConcpeto)
                    {
                        regla = regla + string.Format("{0}{1}{2} ", ((itemVariableConcpeto.Concepto1 != null) ? itemVariableConcpeto.Concepto1.Nombre : ""), ((itemVariableConcpeto.Variable != null) ? itemVariableConcpeto.Variable.Nombre : ""), itemVariableConcpeto.Operador);

                        LiquidadorReglaConceptos reglaConceptoTmp = new LiquidadorReglaConceptos();
                        reglaConceptoTmp.IdVariable = ((itemVariableConcpeto.Variable != null) ? itemVariableConcpeto.Variable.IdVariable : -1);
                        reglaConceptoTmp.IdConcepto = ((itemVariableConcpeto.Concepto1 != null) ? itemVariableConcpeto.Concepto1.IdConcepto : -1);
                        reglaConceptoTmp.NomVariable = string.Format("{0}{1}", ((itemVariableConcpeto.Concepto1 != null) ? itemVariableConcpeto.Concepto1.Nombre : ""), ((itemVariableConcpeto.Variable != null) ? itemVariableConcpeto.Variable.Nombre : ""));
                        reglaConceptoTmp.Operador = itemVariableConcpeto.Operador;
                        reglaConceptoTmp.NomClass = itemVariableConcpeto.NomClass;

                        reglaTmp.ListaConceptos.Add(reglaConceptoTmp);
                    }
                    reglaTmp.Regla = regla;
                    listaRegla.Add(reglaTmp);
                }
                return listaRegla;
            }
        }

        public List<LiquidadorRegla> ObtenerReglaPorHotel(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<LiquidadorRegla> listaRegla = new List<LiquidadorRegla>();

                List<Concepto> listaConcepto = Contexto.Concepto.Where(C => C.Hotel.IdHotel == idHotel && C.EsActiva == true).ToList();
                foreach (Concepto itemConcepto in listaConcepto)
                {
                    if (itemConcepto.NivelConcepto == 2) // Solo necesitamos las reglas que aplican para propietarios
                    {
                        LiquidadorRegla reglaTmp = new LiquidadorRegla();
                        reglaTmp.NombreConcepto = itemConcepto.Nombre;
                        reglaTmp.IdConcepto = itemConcepto.IdConcepto;
                        reglaTmp.NumDecimales = itemConcepto.NumDecimales;
                        reglaTmp.Orden = itemConcepto.Orden;
                        string regla = string.Empty;

                        List<Variables_Concepto> listaVariableConcpeto = Contexto.Variables_Concepto.Include("Concepto1").Include("Variable").Where(VC => VC.Concepto.IdConcepto == itemConcepto.IdConcepto).OrderBy(VC => VC.Orden).ToList();
                        reglaTmp.ListaConceptos = new List<LiquidadorReglaConceptos>();
                        foreach (Variables_Concepto itemVariableConcpeto in listaVariableConcpeto)
                        {
                            regla = regla + string.Format("{0}{1}{2} ", ((itemVariableConcpeto.Concepto1 != null) ? itemVariableConcpeto.Concepto1.Nombre : ""), ((itemVariableConcpeto.Variable != null) ? itemVariableConcpeto.Variable.Nombre : ""), itemVariableConcpeto.Operador);

                            LiquidadorReglaConceptos reglaConceptoTmp = new LiquidadorReglaConceptos();
                            reglaConceptoTmp.IdVariable = ((itemVariableConcpeto.Variable != null) ? itemVariableConcpeto.Variable.IdVariable : -1);
                            reglaConceptoTmp.IdConcepto = ((itemVariableConcpeto.Concepto1 != null) ? itemVariableConcpeto.Concepto1.IdConcepto : -1);
                            reglaConceptoTmp.NomVariable = string.Format("{0}{1}", ((itemVariableConcpeto.Concepto1 != null) ? itemVariableConcpeto.Concepto1.Nombre : ""), ((itemVariableConcpeto.Variable != null) ? itemVariableConcpeto.Variable.Nombre : ""));
                            reglaConceptoTmp.Operador = itemVariableConcpeto.Operador;
                            reglaConceptoTmp.NomClass = itemVariableConcpeto.NomClass;

                            Concepto oConceptoFind = listaConcepto.Find(R => R.IdConcepto == reglaConceptoTmp.IdConcepto);
                            if (oConceptoFind != null && oConceptoFind.NivelConcepto == 1)
                                reglaConceptoTmp.NomClass = "varH";

                            reglaTmp.ListaConceptos.Add(reglaConceptoTmp);
                        }
                        reglaTmp.Regla = regla;
                        listaRegla.Add(reglaTmp);
                    }
                }
                return listaRegla;
            }
        }

        public List<LiquidadorValorVariable> ObtenerVariablesValor(int idHotel, int ano, int mes)
        {
            List<LiquidadorValorVariable> listaValorVariableHotel = new List<LiquidadorValorVariable>();

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                listaValorVariableHotel = (from L in Contexto.Liquidacion
                                           join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                           where L.Hotel.IdHotel == idHotel && L.EsLiquidacionHotel == true && (L.FechaPeriodoLiquidado.Year == ano && L.FechaPeriodoLiquidado.Month == mes)
                                           select new LiquidadorValorVariable()
                                           { // Variables valor de hotel de liquidacion
                                               IdOwner = -1,
                                               IdSuit = -1,
                                               IdVariable = C.IdConcepto,
                                               Nombre = C.Nombre,
                                               Valor = L.Valor,
                                               Tipo = "varH"
                                           }).Union(
                                            from V in Contexto.Variable
                                            where V.Hotel.IdHotel == idHotel && V.Activo == true && V.Tipo == "C"
                                            select new LiquidadorValorVariable()
                                            { // Variables valor constantes
                                                IdOwner = -1,
                                                IdSuit = -1,
                                                IdVariable = V.IdVariable,
                                                Nombre = V.Nombre,
                                                Valor = V.ValorConstante,
                                                Tipo = "varCO"
                                            }).Union(
                                            from SP in Contexto.Suit_Propietario
                                            join VVS in Contexto.Valor_Variable_Suit on SP.IdSuitPropietario equals VVS.Suit_Propietario.IdSuitPropietario
                                            join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                                            join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                            join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                            where S.Hotel.IdHotel == idHotel && SP.EsActivo == true
                                            select new LiquidadorValorVariable()
                                            {
                                                IdOwner = P.IdPropietario,
                                                IdSuit = S.IdSuit,
                                                IdVariable = VVS.Variable.IdVariable,
                                                Nombre = V.Nombre,
                                                Valor = VVS.Valor,
                                                Tipo = "varV"
                                            }).ToList();

                List<Variable> lstVariable = Contexto.Variable.Where(V => V.Hotel.IdHotel == idHotel && V.Activo == true && V.Tipo == "H").ToList();
                foreach (Variable itemVariable in lstVariable)
                {
                    Valor_Variable Valor_VariableTmp = Contexto.Valor_Variable.Where(VV => VV.Variable.IdVariable == itemVariable.IdVariable && (VV.Fecha.Year == ano && VV.Fecha.Month == mes)).FirstOrDefault();

                    LiquidadorValorVariable LiquidadorValorVariableTmp = new LiquidadorValorVariable();
                    LiquidadorValorVariableTmp.IdOwner = -1;
                    LiquidadorValorVariableTmp.IdSuit = -1;
                    LiquidadorValorVariableTmp.IdVariable = itemVariable.IdVariable;
                    LiquidadorValorVariableTmp.Nombre = itemVariable.Nombre;
                    LiquidadorValorVariableTmp.Valor = (Valor_VariableTmp == null) ? 0 : Valor_VariableTmp.Valor;
                    LiquidadorValorVariableTmp.Tipo = "varH";
                    listaValorVariableHotel.Add(LiquidadorValorVariableTmp);
                }
            }
            return listaValorVariableHotel;
        }

        public bool EliminarLiquidacion(int yyyy, int mm, int idHotel, ref string error)
        {
            bool isOK = true;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    using (ContextoOwner Contexto = new ContextoOwner())
                    {

                        string ConnectionString = (Contexto.Connection as EntityConnection).StoreConnection.ConnectionString;
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                        builder.ConnectTimeout = 2500;
                        SqlConnection con = new SqlConnection(builder.ConnectionString);
                        int res = 0;
                        con.Open();
                        using (SqlCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "DeleteLiquidacionPorHotel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.AddWithValue("@IdHotel", idHotel);
                            cmd.Parameters.AddWithValue("@FechaPeriodoMM", mm);
                            cmd.Parameters.AddWithValue("@FechaPeriodoYY", yyyy);
                            cmd.Parameters.AddWithValue("@All", "S");

                            res = cmd.ExecuteNonQuery();
                        }
                        Contexto.SaveChanges();
                    }
                    scope.Complete();
                }
                catch (System.Exception ex)
                {
                    error = string.Format("InnerException: {0} \n Message: {1} \n ToString: {2}", ex.InnerException, ex.Message, ex.ToString());
                    isOK = false;
                }
            }
            return isOK;
        }


        public bool GuardarLiquidacionProp(List<LiquidacionProp> lstLiquidacionProp, int idUsuario, int yyyy, int mm, int idHotel, bool isSel, ref string error)
        {
            bool isOK = true;

            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                using (ContextoOwner Contexto = new ContextoOwner())
                {

                    string ConnectionString = (Contexto.Connection as EntityConnection).StoreConnection.ConnectionString;
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                    builder.ConnectTimeout = 2500;
                    SqlConnection con = new SqlConnection(builder.ConnectionString);
                    int res = 0;
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "DeleteLiquidacionPorHotel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.AddWithValue("@IdHotel", idHotel);
                        cmd.Parameters.AddWithValue("@FechaPeriodoMM", mm);
                        cmd.Parameters.AddWithValue("@FechaPeriodoYY", yyyy);

                        if (isSel)
                        {
                            foreach (LiquidacionProp itemLiqProp in lstLiquidacionProp)
                            {
                                cmd.Parameters.AddWithValue("@IdProp", itemLiqProp.IdPropietario);
                                res = cmd.ExecuteNonQuery();
                                cmd.Parameters.RemoveAt("@IdProp");
                            }
                        }
                        else
                        {
                            res = cmd.ExecuteNonQuery();
                        }

                    }

                    Liquidacion oLiquidacion = null;
                    foreach (LiquidacionProp itemLiqProp in lstLiquidacionProp)
                    {
                        foreach (LiquidacionConceptoProp itemConcepto in itemLiqProp.ListaConceptos)
                        {
                            oLiquidacion = new Liquidacion();
                            oLiquidacion.FechaElabaoracion = DateTime.Now;
                            oLiquidacion.FechaPeriodoLiquidado = new DateTime(yyyy, mm, 1); //itemLiqProp.FechaPeriodoLiquidado;
                            oLiquidacion.EsLiquidacionHotel = false;
                            oLiquidacion.HotelReference.EntityKey = new EntityKey("ContextoOwner.Hotel", "IdHotel", itemLiqProp.IdHotel);
                            oLiquidacion.SuitReference.EntityKey = new EntityKey("ContextoOwner.Suit", "IdSuit", itemLiqProp.IdSuit);
                            oLiquidacion.PropietarioReference.EntityKey = new EntityKey("ContextoOwner.Propietario", "IdPropietario", itemLiqProp.IdPropietario);
                            oLiquidacion.UsuarioReference.EntityKey = new EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);

                            oLiquidacion.Regla = itemConcepto.Regla;
                            oLiquidacion.Valor = itemConcepto.Valor;
                            oLiquidacion.ConceptoReference.EntityKey = new EntityKey("ContextoOwner.Concepto", "IdConcepto", itemConcepto.IdConcepto);
                            Contexto.AddToLiquidacion(oLiquidacion);
                        }
                    }

                    Contexto.SaveChanges();
                }
                //    scope.Complete();
            }
            catch (System.Exception ex)
            {
                error = string.Format("InnerException: {0} \n Message: {1} \n ToString: {2}", ex.InnerException, ex.Message, ex.ToString());
                isOK = false;
            }
            //}
            return isOK;
        }

        public bool GuardarLiquidacionHotel(List<LiquidacionHotel> lstLiquidacionHotel, int idUsuario, int yyyy, int mm, int idHotel, ref string error)
        {
            bool isOK = true;

            //using (TransactionScope scope = new TransactionScope())
            //{
                try
                {
                    using (ContextoOwner Contexto = new ContextoOwner())
                    {
                        List<Liquidacion> lstLiquidacionDelete = Contexto.Liquidacion.Where(L => L.EsLiquidacionHotel == true && (L.FechaPeriodoLiquidado.Year == yyyy && L.FechaPeriodoLiquidado.Month == mm) && L.Hotel.IdHotel == idHotel).ToList();
                        foreach (Liquidacion itemLiqDelete in lstLiquidacionDelete)
                        {
                            Contexto.DeleteObject(itemLiqDelete);
                        }

                        Liquidacion oLiquidacion = null;
                        foreach (LiquidacionHotel itemLiqHotel in lstLiquidacionHotel)
                        {
                            oLiquidacion = new Liquidacion();
                            oLiquidacion.Valor = itemLiqHotel.Valor;
                            oLiquidacion.Regla = itemLiqHotel.Regla;
                            oLiquidacion.FechaPeriodoLiquidado = new DateTime(yyyy, mm, 1); //itemLiqHotel.FechaPeriodoLiquidado;
                            oLiquidacion.FechaElabaoracion = DateTime.Now;
                            oLiquidacion.EsLiquidacionHotel = true;
                            oLiquidacion.HotelReference.EntityKey = new EntityKey("ContextoOwner.Hotel", "IdHotel", itemLiqHotel.IdHotel);
                            oLiquidacion.ConceptoReference.EntityKey = new EntityKey("ContextoOwner.Concepto", "IdConcepto", itemLiqHotel.IdConcepto);
                            oLiquidacion.UsuarioReference.EntityKey = new EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                            Contexto.AddToLiquidacion(oLiquidacion);
                        }
                        Contexto.SaveChanges();
                    }
                    //scope.Complete();
                }
                catch (System.Exception ex)
                {
                    error = string.Format("InnerException: {0} \n Message: {1} \n ToString: {2}", ex.InnerException, ex.Message, ex.ToString());
                    isOK = false;
                }
            //}
            return isOK;
        }
    }
}
