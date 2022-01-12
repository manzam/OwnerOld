using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class TipoCuentaContableBo
    {
        public List<Tipo_Cuenta_Contable> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Tipo_Cuenta_Contable> listaTipoCuenta = Contexto.Tipo_Cuenta_Contable.ToList();
                return listaTipoCuenta;
            }
        }
    }
}
