using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using System.Configuration;
using System.Data;

namespace BO
{
    public class VariablesConceptoBo
    {
        public void Guardar(int idConceptoPadre, int? idvariable, int? idConcepto, 
                            string nomClass, int orden, string operador)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Variables_Concepto variableConceptoTmp = new Variables_Concepto();
                variableConceptoTmp.NomClass = nomClass;
                variableConceptoTmp.Orden = orden;

                //if (operador == "Ʃ")
                //    variableConceptoTmp.Operador = "&Sigma;";
                //else

                variableConceptoTmp.Operador = operador;
                variableConceptoTmp.ConceptoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", idConceptoPadre);

                if (idConcepto != null)
                    variableConceptoTmp.Concepto1Reference.EntityKey = new System.Data.EntityKey("ContextoOwner.Concepto", "IdConcepto", idConcepto);

                if (idvariable != null)
                    variableConceptoTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", idvariable);

                Contexto.AddToVariables_Concepto(variableConceptoTmp);
                Contexto.SaveChanges();
            }
        }

        public List<ObjetoGenerico> Obtener(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string cnn = Contexto.Connection.ConnectionString;

                string queryString = "SELECT " +
                                     "Variables_Concepto.IdVariable, " +
                                     "Concepto.IdConcepto,  " +                                     
                                     "Variables_Concepto.Orden,  " +
                                     "Variables_Concepto.Operador,  " +
                                     "Variables_Concepto.NomClass, " +
                                     "Concepto.NombreExtracto AS NombreExtracto, " +
                                     "Concepto.Nombre AS NombreConcepto,  " +
                                     "Variable.Nombre AS NombreVariable "+
                                     "FROM Variables_Concepto  " +
                                     "LEFT JOIN Variable ON Variables_Concepto.IdVariable = Variable.IdVariable " +
                                     "LEFT JOIN Concepto ON Variables_Concepto.IdConcepto = Concepto.IdConcepto  " +
                                     "WHERE Variables_Concepto.IdConceptoPadre = " + idConcepto + " ORDER BY Variables_Concepto.Orden";

                DataTable dt = Servicios.Utilities.Select(queryString, "Variables_Concepto");

                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                foreach (DataRow filaTmp in dt.Rows)
                {
                    ObjetoGenerico varTmp = new ObjetoGenerico();
                    varTmp.IdVariable = (filaTmp["IdVariable"].ToString() != string.Empty) ? int.Parse(filaTmp["IdVariable"].ToString()) : 0;
                    varTmp.IdConcepto = (filaTmp["IdConcepto"].ToString() != string.Empty) ? int.Parse(filaTmp["IdConcepto"].ToString()) : 0;
                    varTmp.Orden = int.Parse(filaTmp["Orden"].ToString());
                    varTmp.Operador = filaTmp["Operador"].ToString();
                    varTmp.NomClass = filaTmp["NomClass"].ToString();
                    varTmp.NombreVariable = filaTmp["NombreVariable"].ToString();
                    varTmp.NombreExtracto = filaTmp["NombreExtracto"].ToString();
                    //varTmp.NombreSuit = filaTmp["NombreSuit"].ToString();
                    varTmp.NombreConcepto = filaTmp["NombreConcepto"].ToString();

                    listaVariables.Add(varTmp);
                }

                return listaVariables;
            }
        }

        public void Eliminar(int idConcepto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Variables_Concepto> listaVariablesConcepto = Contexto.Variables_Concepto.Where(V => V.Concepto.IdConcepto == idConcepto).ToList();
                
                foreach (Variables_Concepto item in listaVariablesConcepto)
                {
                    Contexto.DeleteObject(item);
                    Contexto.SaveChanges();
                }                
            }
        }
    }
}
