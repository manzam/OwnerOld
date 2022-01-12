using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class DeptoBo
    {

        public List<Departamento> ObtenerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Departamento> listaDepto = new List<Departamento>();
                listaDepto = Contexto.Departamento.ToList();

                return listaDepto;
            }
        }
    }
}
