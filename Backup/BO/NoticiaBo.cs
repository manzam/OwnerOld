using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class NoticiaBo
    {
        public List<Noticia> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Noticia> listaNoticia = new List<Noticia>();
                listaNoticia = Contexto.Noticia.ToList();
                return listaNoticia;
            }
        }

        public void Guardar(string titulo, string urlImagen, bool esActivo, int IdUsuario, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Noticia noticiaTmp = new Noticia();
                noticiaTmp.Titulo = titulo;
                noticiaTmp.Imagen = urlImagen;
                noticiaTmp.Activo = esActivo;
                noticiaTmp.Fecha = DateTime.Now;
                noticiaTmp.FechaCaducidad = fecha;
                noticiaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", IdUsuario);

                Contexto.AddToNoticia(noticiaTmp);
                Contexto.SaveChanges();
            }
        }

        public Noticia Obtener(int idNoticia)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Noticia noticiaTmp = Contexto.Noticia.Where(N => N.IdNoticia == idNoticia).FirstOrDefault();

                return noticiaTmp;
            }
        }

        public void Actualizar(int idNoticia, string titulo, string urlImagen, bool esActivo, string UsuarioActualiza, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Noticia noticiaTmp = Contexto.Noticia.Where(N => N.IdNoticia == idNoticia).FirstOrDefault();

                noticiaTmp.Titulo = titulo;
                noticiaTmp.Activo = esActivo;
                noticiaTmp.FechaCaducidad = fecha;
                noticiaTmp.Imagen = urlImagen;
                Contexto.SaveChanges();
            }
        }

        public List<Noticia> ObtenerNoticiasAMostrar()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Noticia> listaNoticia = new List<Noticia>();
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
                listaNoticia = Contexto.Noticia.Where(N => N.Activo == true && N.FechaCaducidad >= fechaActual).ToList();
                return listaNoticia;
            }
        }
    }
}
