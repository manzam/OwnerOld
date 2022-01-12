using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class ModuloPerfilBo
    {
        public List<Modulo_Perfil> VerTodos(int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Modulo_Perfil> listaModuloPerfil = new List<Modulo_Perfil>();
                listaModuloPerfil = Contexto.Modulo_Perfil.Include("Modulo").Where(MP => MP.Perfil.IdPerfil == idPerfil).ToList();
                return listaModuloPerfil;
            }
        }

        public void Actualizar(List<Modulo_Perfil> listaModuloPerfil, int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Modulo_Perfil> listaPerfilTmp = Contexto.Modulo_Perfil.Where(MP => MP.Perfil.IdPerfil == idPerfil).ToList();
                foreach (Modulo_Perfil moduloPerfilTmp in listaPerfilTmp)
                {
                    Contexto.DeleteObject(moduloPerfilTmp);
                }

                foreach (Modulo_Perfil moduloPerfilTmp in listaModuloPerfil)
                {
                    Contexto.AddToModulo_Perfil(moduloPerfilTmp);                    
                }
                Contexto.SaveChanges();
            }
        }

        public string Permisos(int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string permiso = string.Empty;
                List<Modulo_Perfil> listaModuloPerfil = new List<Modulo_Perfil>();
                listaModuloPerfil = Contexto.Modulo_Perfil.Include("Modulo").Where(MP => MP.Perfil.IdPerfil == idPerfil).ToList();

                foreach (Modulo_Perfil itemModulo in listaModuloPerfil)
                {
                    permiso += itemModulo.Modulo.Nombre + ",";
                }

                permiso = permiso.TrimEnd(new char[] { ',' });

                return permiso;
            }
        }
    }
}
