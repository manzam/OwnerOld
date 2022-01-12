using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class CiudadBo
    {
        /// <summary>
        /// Lista todas las ciudades
        /// </summary>
        /// <returns></returns>
        public List<Ciudad> ObtenerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Ciudad> listaCiudad = new List<Ciudad>();
                listaCiudad = Contexto.Ciudad.ToList();

                return listaCiudad;
            }
        }

        /// <summary>
        /// Obtiene las ciudades filtrado por el departamento
        /// </summary>
        /// <param name="idDepartamento"></param>
        /// <returns></returns>
        public List<Ciudad> ObtenerPorDepto(int idDepartamento)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Ciudad> listaCiudad = new List<Ciudad>();
                listaCiudad = Contexto.Ciudad.Where(C => C.Departamento.IdDepartamento == idDepartamento).ToList();

                return listaCiudad;
            }
        }

        /// <summary>
        /// Obtiene una ciudad por nombre
        /// </summary>
        /// <param name="nombreCiudad"></param>
        /// <returns></returns>
        public int ObtenerIdByNombre(string nombreCiudad)
        {
            // byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(texto);
            // return System.Text.Encoding.UTF8.GetString(tempBytes);

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.
                       Ciudad.
                       Where(C => C.Nombre.Contains(nombreCiudad.ToUpper())).
                       Select(C => C.IdCiudad).
                       FirstOrDefault();
            }
        }
    }
}
