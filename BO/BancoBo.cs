using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class BancoBo
    {
        /// <summary>
        /// Lista todos los bancos.
        /// </summary>
        /// <returns></returns>
        public List<Banco> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Banco> listaBanco = new List<Banco>();
                listaBanco = Contexto.Banco.OrderBy(B => B.Nombre).ToList();
                return listaBanco;
            }
        }

        /// <summary>
        /// Obtengo un banco por el nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public int Obtener(string nombre)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Banco.Where(B => B.Nombre.ToUpper() == nombre.ToUpper()).Select(B => B.IdBanco).FirstOrDefault();
            }
        }

        /// <summary>
        /// Guardar banco
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public Banco Guardar(string nombre)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Banco bancoTmp = new Banco();
                bancoTmp.Nombre = nombre;

                Contexto.AddToBanco(bancoTmp);
                Contexto.SaveChanges();

                return bancoTmp;
            }
        }
    }
}
