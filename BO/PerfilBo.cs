using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class PerfilBo
    {
        ModuloPerfilBo moduloPerfilBo = null;
        public Perfil ObtenerPerfil(int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Perfil perfilTmp = Contexto.Perfil.Where(P => P.IdPerfil == idPerfil).FirstOrDefault();
                return perfilTmp;
            }
        }

        public List<Perfil> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Perfil> listaPerfil = new List<Perfil>();
                listaPerfil = Contexto.Perfil.Where(P => P.IdPerfil != 2).ToList();
                return listaPerfil;
            }
        }

        public int Guardar(Perfil perfilTmp, string permisos, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = perfilTmp.Nombre;
                auditoriaTmp.NombreTabla = "Perfil";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre Perfil";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = perfilTmp.Nombre + " : " + permisos;
                auditoriaTmp.NombreTabla = "Perfil";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Permisos";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                Contexto.AddToPerfil(perfilTmp);
                Contexto.SaveChanges();

                return perfilTmp.IdPerfil;
            }
        }

        public void Actualizar(int idPerfil, string nombre, string descripcion, string permisos, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                moduloPerfilBo = new ModuloPerfilBo();
                Perfil perfilTmp = Contexto.Perfil.Where(P => P.IdPerfil == idPerfil).FirstOrDefault();                

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre;
                auditoriaTmp.ValorAnterior = perfilTmp.Nombre;
                auditoriaTmp.NombreTabla = "Perfil";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre Perfil";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombre + " : " + permisos;
                auditoriaTmp.ValorAnterior = perfilTmp.Nombre + " : " + moduloPerfilBo.Permisos(idPerfil);
                auditoriaTmp.NombreTabla = "Perfil";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Permisos";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                #endregion

                perfilTmp.Nombre = nombre;
                perfilTmp.Descripcion = descripcion;

                Contexto.SaveChanges();
            }
        }

        public bool Eliminar(int idPerfil)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                bool esValido = true;

                if (idPerfil == 2)
                    return false;

                if (Contexto.Usuario.Where(U => U.Perfil.IdPerfil == idPerfil).Count() > 0)
                    return false;
                else
                {
                    Perfil perfil = Contexto.Perfil.Where(P => P.IdPerfil == idPerfil).FirstOrDefault();
                    Contexto.DeleteObject(perfil);
                    Contexto.SaveChanges();
                    return esValido;
                }                
            }
        }
    }
}
