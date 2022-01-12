using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;
using DM;

namespace BO
{
    public class ModuloBo
    {
        public List<Modulo> VerTodos(int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Modulo> listaModulo = new List<Modulo>();
                listaModulo = (from MP in Contexto.Modulo_Perfil
                               join M in Contexto.Modulo on MP.Modulo.IdModulo equals M.IdModulo
                               where MP.Perfil.IdPerfil == idPerfil
                               select M).OrderBy(M => new { M.Grupo, M.Nombre }).ToList();
                return listaModulo;
            }
        }

        public List<Modulo> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Modulo> listaModulo = new List<Modulo>();
                listaModulo = Contexto.Modulo.Where(M => M.Tipo == "U" || M.Tipo == "UP").OrderBy(M=>M.Nombre).ToList();
                return listaModulo;
            }
        }
    }
}
