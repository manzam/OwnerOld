using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class AuditoriaBo
    {
        /// <summary>
        /// Lista los modulos de Owners
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerModulos()
        {            
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<string> listaTablas = new List<string>();
                listaTablas = Contexto.Auditoria.Select(A => A.NombreTabla).Distinct().ToList();

                return listaTablas;
            }
        }

        /// <summary>
        /// Lista los campos de las tablas, segun el parametro de entrada.
        /// </summary>
        /// <param name="nombreTabla"></param>
        /// <returns></returns>
        public List<string> ObtenerCampos(string nombreTabla)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<string> listaCampos = new List<string>();
                listaCampos = Contexto.Auditoria.
                              Where(A => A.NombreTabla == nombreTabla).
                              Select(A => A.Campo).
                              Distinct().
                              ToList();

                return listaCampos;
            }
        }

        /// <summary>
        /// Guarda una auditoria
        /// </summary>
        /// <param name="detalleNuevo"></param>
        /// <param name="detalleAnterior"></param>
        /// <param name="nombreTabla"></param>
        /// <param name="accion"></param>
        /// <param name="campo"></param>
        /// <param name="fecha"></param>
        /// <param name="idUsuario"></param>
        public void Guardar(string detalleNuevo, string detalleAnterior, string nombreTabla, string accion, string campo, DateTime fecha, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Auditoria auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = detalleNuevo;
                auditoriaTmp.ValorAnterior = detalleAnterior;
                auditoriaTmp.NombreTabla = nombreTabla;
                auditoriaTmp.Accion = accion;
                auditoriaTmp.Campo = campo;
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);
                Contexto.SaveChanges();
            }
        }
    }
}
