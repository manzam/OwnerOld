using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Microsoft.JScript;

namespace Servicios
{
    public class ValidarExpresion
    {
        System.Reflection.Assembly oEnsamblado = null;

        public bool PrecompilarAssembly(string funcion, StringCollection listaParametros)//, StringCollection listaNameSpace)
        {
            string mParametros = "";
            StringBuilder CodigoFuente = new StringBuilder();

            CodigoFuente.Append("using System;");
            CodigoFuente.Append("using System.Text;");

            foreach (string itemParametro in listaParametros)
            {
                mParametros += itemParametro + ",";
            }

            mParametros = mParametros.Substring(0, mParametros.Length - 1);
            
            CodigoFuente.Append("public class EvalClase ");
            CodigoFuente.Append("{");
                CodigoFuente.Append("public static Object Eval(" + mParametros + ")");
                CodigoFuente.Append("{");
                    CodigoFuente.Append(" return " + funcion + ";");
                CodigoFuente.Append("}");
            CodigoFuente.Append("}");


            CSharpCodeProvider oCProvider = new CSharpCodeProvider();
            ICodeCompiler oCompiler = oCProvider.CreateCompiler();

            CompilerParameters oCParam = new CompilerParameters();
            oCParam.GenerateInMemory = true;

            CompilerResults oCResult = null;
            oCResult = oCompiler.CompileAssemblyFromSource(oCParam, CodigoFuente.ToString());

            if (oCResult.Errors.Count > 0)
            {
                this.ListaError = oCResult.Errors;
                return false;
            }
            else
            {
                oEnsamblado = oCResult.CompiledAssembly;
                return true;
            }            
        }

        public Object Evaluar(object[] ParamArray)
        {
            if (oEnsamblado == null)
                return null;
            else
            {
                Type oClass = oEnsamblado.GetType("EvalClase");
                return oClass.GetMethod("Eval").Invoke(null, ParamArray);
            }
        }

        public bool ValidaExpression(string reglaTmp, List<string> listaVariables)
        {
            StringCollection mParameters = new StringCollection();

            foreach (string varTmp in listaVariables)
            {
                mParameters.Add("double " + varTmp);
            }

            return this.PrecompilarAssembly(reglaTmp, mParameters);
        }

        public CompilerErrorCollection ListaError { get; set; }


        public object Eval(string regla, Dictionary<string, string> listaParametros)
        {
            try
            {
                foreach (var item in listaParametros)
                {
                    regla = regla.Replace(item.Key, item.Value);
                }

                object Result = null;
                Microsoft.JScript.Vsa.VsaEngine Engine = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                Result = Microsoft.JScript.Eval.JScriptEvaluate(regla, Engine);

                return Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

    
}
