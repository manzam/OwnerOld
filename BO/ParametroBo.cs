using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class ParametroBo
    {
        public string ObtenerValor(string llave)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Parametros.Where(P => P.Llave == llave).Select(P => P.Valor).FirstOrDefault();
            }
        }
    }
}