using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Servicios;
using System.Collections.Specialized;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidarExpresion mEval = new ValidarExpresion();
            string expresion = "((X + Y)/X)*Y";

            StringCollection mParameters = new StringCollection();
            mParameters.Add("double X");
            mParameters.Add("double Y");

            //StringCollection mNameSpaces = new StringCollection();
            //mNameSpaces.Add("System");
            //mNameSpaces.Add("System.Text");

            if (mEval.PrecompilarAssembly(expresion, mParameters))//, mNameSpaces))
            {
                Object[] mParam = { 100, 3 };
                Console.WriteLine(mEval.Evaluar(mParam));
                Console.ReadKey();
            }
            else
                Console.WriteLine("No se ha generado el Assembly");
        }
    }
}
