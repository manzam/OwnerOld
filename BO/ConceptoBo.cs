using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using Servicios;

namespace BO
{
    public class ConceptoBo
    {
        /// <summary>
        /// Lista todos los conceptos
        /// </summary>
        /// <returns></returns>
        public List<Concepto> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Concepto> listaConcepto = new List<Concepto>();
                listaConcepto = Contexto.Concepto.ToList();
                return listaConcepto;
            }
        }

        public List<Concepto> VerTodos(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Concepto> listaConcepto = new List<Concepto>();
                listaConcepto = Contexto.Concepto.Where(C => C.Hotel.IdHotel == idHotel).OrderBy(C => C.Orden).ToList();
                return listaConcepto;
            }
        }

        public List<Concepto> VerTodos(int idHotel, int nivelConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Concepto> listaConcepto = new List<Concepto>();
                listaConcepto = Contexto.Concepto.
                                Where(C => C.Hotel.IdHotel == idHotel && C.NivelConcepto == nivelConcepto).
                                OrderBy(C => C.Nombre).ToList();

                return listaConcepto;
            }
        }

        public Concepto Obtener(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Concepto conceptoTmp = Contexto.Concepto.
                                       Include("Informacion_Estadistica").
                                       Include("Hotel").
                                       Include("Cuenta_Contable").
                                       Include("Cuenta_Contable1").
                                       Where(C => C.IdConcepto == idConcepto).FirstOrDefault();
                return conceptoTmp;
            }
        }

        public void ActivarConcepto(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Concepto oConcepto = Contexto.Concepto.Where(C => C.IdConcepto == idConcepto).FirstOrDefault();
                oConcepto.EsActiva = !oConcepto.EsActiva;
                Contexto.SaveChanges();
            }
        }

        public string ObtenerRegla(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = ((from VC in Contexto.Variables_Concepto
                                                        join V in Contexto.Variable on VC.Variable.IdVariable equals V.IdVariable
                                                        where VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = V.Nombre,
                                                            Orden = VC.Orden
                                                        }).Union(
                                                        from VC in Contexto.Variables_Concepto
                                                        join C in Contexto.Concepto on VC.Concepto1.IdConcepto equals C.IdConcepto
                                                        where VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = C.Nombre,
                                                            Orden = VC.Orden
                                                        }).Union(
                                                        from VC in Contexto.Variables_Concepto
                                                        where VC.Operador != "" && VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = VC.Operador,
                                                            Orden = VC.Orden
                                                        })).OrderBy(V => V.Orden).ToList();
                string regla = "";
                foreach (ObjetoGenerico variable in listaVariables)
                {
                    //regla += variable.Nombre;
                    if (variable.Nombre != "*" && variable.Nombre != "+" && variable.Nombre != "-" && variable.Nombre != "/" && variable.Nombre != "(" && variable.Nombre != ")" &&
                        variable.Nombre != "case" && variable.Nombre != "when" && variable.Nombre != "<" && variable.Nombre != ">" && variable.Nombre != "end" && variable.Nombre != "then" &&
                        variable.Nombre != "and" && variable.Nombre != "else" && variable.Nombre != "=")
                        regla += " @" + variable.Nombre;
                    else
                        regla += " " + variable.Nombre;
                }

                return regla;
            }
        }

        public string ObtenerRegla2(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = ((from VC in Contexto.Variables_Concepto
                                                        join V in Contexto.Variable on VC.Variable.IdVariable equals V.IdVariable
                                                        where VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = V.Nombre,
                                                            Orden = VC.Orden
                                                        }).Union(
                                                        from VC in Contexto.Variables_Concepto
                                                        join C in Contexto.Concepto on VC.Concepto1.IdConcepto equals C.IdConcepto
                                                        where VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = C.Nombre,
                                                            Orden = VC.Orden
                                                        }).Union(
                                                        from VC in Contexto.Variables_Concepto
                                                        where VC.Operador != "" && VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = VC.Operador,
                                                            Orden = VC.Orden
                                                        })).OrderBy(V => V.Orden).ToList();
                string regla = "";
                foreach (ObjetoGenerico variable in listaVariables)
                {
                    regla += variable.Nombre;
                }

                return regla;
            }
        }

        public List<ObjetoGenerico> ObtenerReglas(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = ((from VC in Contexto.Variables_Concepto
                                                        join V in Contexto.Variable on VC.Variable.IdVariable equals V.IdVariable
                                                        where VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = V.Nombre,
                                                            IdVariable = V.IdVariable,
                                                            IdConcepto = -1
                                                        }).Union(
                                                        from VC in Contexto.Variables_Concepto
                                                        join C in Contexto.Concepto on VC.Concepto1.IdConcepto equals C.IdConcepto
                                                        where VC.Concepto.IdConcepto == idConcepto
                                                        select new ObjetoGenerico()
                                                        {
                                                            Nombre = C.Nombre,
                                                            IdVariable = -1,
                                                            IdConcepto = C.IdConcepto
                                                        })).ToList();

                return listaVariables;
            }
        }

        public int Guardar(string nombreConcepto, int idHotel, int nivelConcepto, string cadenaVariables, 
                           int idCuentaContable, string nombreExtracto, bool esMuestraExtracto, int numDecimales,
                           string codigoTercero, int orden, int idInfoEstadistica, int idUsuario, string regla, 
                           string nombreHotel, string cuentaContable, string variableEstadistica, bool esConSegundaCuentaContable,
                           int idCuentaContable2, int idVariableCondicion, string condicion, double valorCondicion, string tipoVariableCondicion, 
                           bool esMotrarEnLiquidacion, bool esRetencionAplicar)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Concepto conceptoTmp = new Concepto();
                conceptoTmp.Nombre = nombreConcepto.ToUpper();
                conceptoTmp.NombreExtracto = nombreExtracto.ToUpper();
                conceptoTmp.NivelConcepto = nivelConcepto;
                conceptoTmp.EsMuestraExtracto = esMuestraExtracto;
                conceptoTmp.NumDecimales = numDecimales;
                conceptoTmp.CodigoTercero = codigoTercero;
                conceptoTmp.Orden = orden;
                conceptoTmp.EsMuestraReporteLiquidacion = esMotrarEnLiquidacion;
                conceptoTmp.EsRetencionAplicar = esRetencionAplicar;
                conceptoTmp.EsActiva = true;
                conceptoTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);

                if (idCuentaContable != -1)
                    conceptoTmp.Cuenta_ContableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Cuenta_Contable", "IdCuentaContable", idCuentaContable);

                if (idInfoEstadistica != -1)
                    conceptoTmp.Informacion_EstadisticaReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Informacion_Estadistica", "IdInformacionEstadistica", idInfoEstadistica);

                conceptoTmp.EsConSegundaCuentaContable = esConSegundaCuentaContable;

                if (esConSegundaCuentaContable)
                {                    
                    conceptoTmp.Condicion = condicion;
                    conceptoTmp.Cuenta_Contable1Reference.EntityKey = new System.Data.EntityKey("ContextoOwner.Cuenta_Contable", "IdCuentaContable", idCuentaContable2);
                    conceptoTmp.ValorCondicion = valorCondicion;
                    conceptoTmp.IdVariableCondicion = idVariableCondicion;
                    conceptoTmp.TipoVariableCondicion = tipoVariableCondicion;
                }

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre Concepto";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " = " + regla;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Regla";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + nombreExtracto;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre Extracto";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + ((nivelConcepto == 1) ? "Hotel" : "Propietario");
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nivel Concepto";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + codigoTercero;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Codigo Tercero";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + cuentaContable;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Cuenta Contable";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + numDecimales;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Numero Decimales";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + variableEstadistica;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Variable Acumular";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);                
                #endregion

                Contexto.AddToConcepto(conceptoTmp);
                Contexto.SaveChanges();

                int idConcepto = conceptoTmp.IdConcepto;

                VariablesConceptoBo variableConceptoBoTmp = new VariablesConceptoBo();
                string[] listaVariables = cadenaVariables.Split(',');

                for (int i = 0; i < listaVariables.Length; i++)
                {
                    if (Utilities.EsOperador(listaVariables[i]))
                        variableConceptoBoTmp.Guardar(idConcepto, null, null, string.Empty, i, listaVariables[i]);
                    else
                    {
                        string[] classId = listaVariables[i].Split('_');

                        if (classId.Length == 2) // Para evitar cuando es una palabra reservadas.
                        {
                            if (classId[0] == "varC")
                                variableConceptoBoTmp.Guardar(idConcepto, null, int.Parse(classId[1]), classId[0], i, string.Empty);
                            else
                                variableConceptoBoTmp.Guardar(idConcepto, int.Parse(classId[1]), null, classId[0], i, string.Empty);
                        }
                    }
                }

                return conceptoTmp.IdConcepto;
            }
        }

        public void Actualizar(int idConcepto, string nombreConcepto,int idHotel, int nivelConcepto, string cadenaVariables, int idCuentaContable,
                               string nombreExtracto, bool esMuestraExtracto, int numDecimales, string codigoTercero, int orden, int idInfoEstadistica, int idUsuario,
                               string regla, string reglaOld, string nombreHotel, string cuentaContable, string variableEstadistica, bool esConSegundaCuentaContable,
                               int idCuentaContable2, int idVariableCondicion, string condicion, double valorCondicion, string tipoVariableCondicion, bool esMotrarEnLiquidacion, bool esRetencionAplicar)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Concepto conceptoTmp = Contexto.Concepto.Include("Informacion_Estadistica").Include("Cuenta_Contable").Where(C => C.IdConcepto == idConcepto).FirstOrDefault();
                
                #region auditoria
                int idHotelOld = (int)conceptoTmp.HotelReference.EntityKey.EntityKeyValues[0].Value;

                int idCuentaContableOld = -1;
                if (conceptoTmp.Cuenta_ContableReference.EntityKey != null)
                    idCuentaContableOld = (int)conceptoTmp.Cuenta_ContableReference.EntityKey.EntityKeyValues[0].Value;

                int idInfoEstadisticaOld = -1;
                if (conceptoTmp.Informacion_EstadisticaReference.EntityKey != null)
                    idInfoEstadisticaOld = (int)conceptoTmp.Informacion_EstadisticaReference.EntityKey.EntityKeyValues[0].Value;
                
                string nombreHotelOld = Contexto.Hotel.Where(H => H.IdHotel == idHotelOld).Select(H => H.Nombre).FirstOrDefault();
                string cuentaContableOld = Contexto.Cuenta_Contable.Where(CC => CC.IdCuentaContable == idCuentaContableOld).Select(CC => CC.Codigo + " - " + CC.Nombre).FirstOrDefault();
                string infoEstadisticaOld = Contexto.Informacion_Estadistica.Where(I => I.IdInformacionEstadistica == idInfoEstadistica).Select(I => I.Nombre).FirstOrDefault();

                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto;
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre Concepto";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " = " + regla;
                auditoriaTmp.ValorAnterior = conceptoTmp.Nombre + " = " + reglaOld + " : " + nombreHotelOld; ;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Regla";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + nombreExtracto;
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre + " : " + conceptoTmp.NombreExtracto;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre Extracto";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + ((nivelConcepto == 1) ? "Hotel" : "Propietario");
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre + " : " + ((conceptoTmp.NivelConcepto == 1) ? "Hotel" : "Propietario");
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nivel Concepto";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + codigoTercero;
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre + " : " + conceptoTmp.CodigoTercero;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Codigo Tercero";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + cuentaContable;
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre + " : " + cuentaContableOld;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Cuenta Contable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + numDecimales;
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre + " : " + conceptoTmp.NumDecimales;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Numero Decimales";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotel + " : " + nombreConcepto + " : " + variableEstadistica;
                auditoriaTmp.ValorAnterior = nombreHotelOld + " : " + conceptoTmp.Nombre + " : " + infoEstadisticaOld;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Variable Acumular";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp); 
                #endregion

                conceptoTmp.Nombre = nombreConcepto.ToUpper();
                conceptoTmp.NombreExtracto = nombreExtracto.ToUpper();
                conceptoTmp.NivelConcepto = nivelConcepto;                
                conceptoTmp.EsMuestraExtracto = esMuestraExtracto;
                conceptoTmp.NumDecimales = numDecimales;
                conceptoTmp.CodigoTercero = codigoTercero;
                conceptoTmp.Orden = orden;
                conceptoTmp.EsMuestraReporteLiquidacion = esMotrarEnLiquidacion;
                conceptoTmp.EsRetencionAplicar = esRetencionAplicar;
                conceptoTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);

                if (idCuentaContable != -1)
                    conceptoTmp.Cuenta_ContableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Cuenta_Contable", "IdCuentaContable", idCuentaContable);
                else
                    conceptoTmp.Cuenta_Contable = null;

                if (idInfoEstadistica != -1)
                    conceptoTmp.Informacion_EstadisticaReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Informacion_Estadistica", "IdInformacionEstadistica", idInfoEstadistica);
                else
                    conceptoTmp.Informacion_Estadistica = null;

                conceptoTmp.EsConSegundaCuentaContable = esConSegundaCuentaContable;
                if (esConSegundaCuentaContable)
                {
                    conceptoTmp.EsConSegundaCuentaContable = esConSegundaCuentaContable;
                    conceptoTmp.Condicion = condicion;
                    conceptoTmp.Cuenta_Contable1Reference.EntityKey = new System.Data.EntityKey("ContextoOwner.Cuenta_Contable", "IdCuentaContable", idCuentaContable2);
                    conceptoTmp.ValorCondicion = valorCondicion;
                    conceptoTmp.IdVariableCondicion = idVariableCondicion;
                    conceptoTmp.TipoVariableCondicion = tipoVariableCondicion;
                }

                Contexto.SaveChanges();

                VariablesConceptoBo variableConceptoBoTmp = new VariablesConceptoBo();
                string[] listaVariables = cadenaVariables.Split(',');

                for (int i = 0; i < listaVariables.Length; i++)
                {
                    if (Utilities.EsOperador(listaVariables[i]))
                        variableConceptoBoTmp.Guardar(idConcepto, null, null, "", i, listaVariables[i]);
                    else
                    {
                        string[] classId = listaVariables[i].Split('_');

                        if (classId[0] == "varC")
                            variableConceptoBoTmp.Guardar(idConcepto, null, int.Parse(classId[1]), classId[0], i, "");
                        else
                            variableConceptoBoTmp.Guardar(idConcepto, int.Parse(classId[1]), null, classId[0], i, "");
                    }
                }                
            }
        }

        public List<ObjetoGenerico> ListaValorLiquidacionConceptos(DateTime fecha, int idHotel, bool esLiquidacionHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaValores = new List<ObjetoGenerico>();
                listaValores = (from L in Contexto.Liquidacion
                                join C in Contexto.Concepto on L.Concepto.IdConcepto equals C.IdConcepto
                                join U in Contexto.Usuario on L.Usuario.IdUsuario equals U.IdUsuario
                                where L.FechaPeriodoLiquidado.Month == fecha.Month && 
                                      L.FechaPeriodoLiquidado.Year == fecha.Year &&
                                      C.Hotel.IdHotel == idHotel &&
                                      L.EsLiquidacionHotel == esLiquidacionHotel
                                select new ObjetoGenerico()
                                {
                                    Nombre = C.Nombre,
                                    Valor = L.Valor,
                                    Fecha = L.FechaElabaoracion,
                                    Login = U.Login
                                }).ToList();

                return listaValores;
            }
        }

        public double ObtenerValorLiquidacion(int idPropietario, int idConcepto, int idSuite, int idHotel, DateTime fechaDesde, DateTime fechaHasta, bool esAcumulado)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = -1;
                // Se hizo esto para poder cojer el valor de una liquidacion que solo hace parte para el hotel, 
                // se excluye de suite y propietario.
                //ObjetoGenerico liquidacionTmp = null;
                bool esLiquidacionHotel = Contexto.
                                          Liquidacion.
                                          Where(L => L.Hotel.IdHotel == idHotel &&
                                                     L.Concepto.IdConcepto == idConcepto).
                                          Select(L => L.EsLiquidacionHotel).FirstOrDefault();

                if (esAcumulado)
                    if (esLiquidacionHotel)
                        valor = Contexto.Liquidacion.
                              Where(L =>
                              L.Hotel.IdHotel == idHotel &&
                              L.Concepto.IdConcepto == idConcepto &&
                              L.FechaPeriodoLiquidado.Month >= fechaDesde.Month &&
                              L.FechaPeriodoLiquidado.Year >= fechaDesde.Year &&
                              L.FechaPeriodoLiquidado.Month <= fechaHasta.Month &&
                              L.FechaPeriodoLiquidado.Year <= fechaHasta.Year).Sum(L => L.Valor);
                    else
                    {
                        try
                        {
                            valor = Contexto.Liquidacion.
                              Where(L =>
                              L.Hotel.IdHotel == idHotel &&
                              L.Concepto.IdConcepto == idConcepto &&
                              L.Suit.IdSuit == idSuite &&
                              L.FechaPeriodoLiquidado.Month >= fechaDesde.Month &&
                              L.FechaPeriodoLiquidado.Year >= fechaDesde.Year &&
                              L.FechaPeriodoLiquidado.Month <= fechaHasta.Month &&
                              L.FechaPeriodoLiquidado.Year <= fechaHasta.Year).Sum(L => L.Valor);
                        }
                        catch (Exception ex) // Si entra aqui, es por que no existe una liquidacion con las condiciones de esa consulta.
                        {
                            valor = 0;
                        }
                        
                    }

                else
                    if (esLiquidacionHotel)
                        valor = Contexto.Liquidacion.
                                  Where(L =>
                                  L.Hotel.IdHotel == idHotel &&
                                  L.Concepto.IdConcepto == idConcepto &&
                                  L.FechaPeriodoLiquidado.Month == fechaDesde.Month &&
                                  L.FechaPeriodoLiquidado.Year == fechaDesde.Year).Select(L => L.Valor).FirstOrDefault();
                    else
                        valor = Contexto.Liquidacion.
                                  Where(L =>
                                  L.Hotel.IdHotel == idHotel &&
                                  L.Concepto.IdConcepto == idConcepto &&
                                  L.Propietario.IdPropietario == idPropietario &&
                                  L.Suit.IdSuit == idSuite &&
                                  L.FechaPeriodoLiquidado.Month == fechaDesde.Month &&
                                  L.FechaPeriodoLiquidado.Year == fechaDesde.Year).Select(L => L.Valor).FirstOrDefault();

                if (valor != -1)
                {
                    if (esLiquidacionHotel)
                        return valor;
                    else
                    {
                        if (esAcumulado)
                        {
                            try
                            {
                                valor = Contexto.
                                    Liquidacion.
                                    Where(L => L.Suit.IdSuit == idSuite &&
                                          L.Hotel.IdHotel == idHotel &&
                                          L.Propietario.IdPropietario == idPropietario &&
                                          L.Concepto.IdConcepto == idConcepto &&
                                          L.FechaPeriodoLiquidado.Month >= fechaDesde.Month &&
                                          L.FechaPeriodoLiquidado.Year >= fechaDesde.Year &&
                                          L.FechaPeriodoLiquidado.Month <= fechaHasta.Month &&
                                          L.FechaPeriodoLiquidado.Year <= fechaHasta.Year).
                                    Sum(L => L.Valor);
                            }
                            catch (Exception ex)
                            {
                                valor = 0;
                            }
                            
                        }
                        else
                            valor = Contexto.
                                Liquidacion.
                                Where(L => L.Suit.IdSuit == idSuite &&
                                      L.Hotel.IdHotel == idHotel &&
                                      L.Propietario.IdPropietario == idPropietario &&
                                      L.Concepto.IdConcepto == idConcepto &&
                                      L.FechaPeriodoLiquidado.Month == fechaDesde.Month &&
                                      L.FechaPeriodoLiquidado.Year == fechaDesde.Year).
                                Select(L => L.Valor).
                                FirstOrDefault();

                        return valor;
                    }
                }
                else
                    return valor;
            }
        }

        public bool EsBorrableRegla(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return (Contexto.Liquidacion.Where(L => L.Concepto.IdConcepto == idConcepto).Count() > 0) ? false : true;
            }
        }

        public bool EsVariableRepetida(string nombreConcepto, int idHotel, int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Concepto.Where(C => C.Nombre.ToUpper() == nombreConcepto.ToUpper() && 
                                                       C.Hotel.IdHotel == idHotel &&
                                                       C.IdConcepto != idConcepto).Count();
                return (con > 0) ? true : false;
            }
        }

        public void Eliminar(int idConcepto, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Concepto conceptoTmp = Contexto.Concepto.Where(C => C.IdConcepto == idConcepto).FirstOrDefault();

                int idHotel = (int)conceptoTmp.HotelReference.EntityKey.EntityKeyValues[0].Value;
                string nombreHotelOld = Contexto.Hotel.Where(H => H.IdHotel == idHotel).Select(H => H.Nombre).FirstOrDefault();

                #region auditoria
                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreHotelOld + " : " + conceptoTmp.Nombre;
                auditoriaTmp.NombreTabla = "Regla";
                auditoriaTmp.Accion = "Eliminar";
                auditoriaTmp.Campo = "Nombre Concepto";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.DeleteObject(conceptoTmp);
                Contexto.SaveChanges();
            }
        }

        public double ObtenerHistorialLiquidacion(DateTime fecha, int idHotel, int idSuite, int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = 0;
                valor = Contexto.
                        Liquidacion.
                        Where(L => L.Suit.IdSuit == idSuite &&
                              L.Hotel.IdHotel == idHotel &&
                              L.Concepto.IdConcepto == idConcepto &&
                              L.FechaPeriodoLiquidado.Month == fecha.Month &&
                              L.FechaPeriodoLiquidado.Year == fecha.Year).
                        Sum(L => L.Valor);

                return valor;
            }
        }        
    }
}
