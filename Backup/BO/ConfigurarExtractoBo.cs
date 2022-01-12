using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class ConfigurarExtractoBo
    {
        public List<ObjetoGenerico> ObtenerListaVariables(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();

                listaVariables = ((from V in Contexto.Variable
                                   where V.Hotel.IdHotel == idHotel
                                   select new ObjetoGenerico()
                                   {
                                       IdVariable = V.IdVariable,
                                       //Nombre = (V.Tipo == "P") ? V.Nombre + " - " + "Variable Propietario" : V.Nombre + " - " + "Variable Hotel"
                                       Nombre = (
                                                 V.Tipo == "P" ? V.Nombre + " - " + "Variable Propietario" :
                                                 V.Tipo == "H" ? V.Nombre + " - " + "Variable Hotel" :
                                                 V.Nombre + " - " + "Variable Constante"
                                                )
                                   }).Union(
                                  from C in Contexto.Concepto
                                  where C.Hotel.IdHotel == idHotel
                                  select new ObjetoGenerico()
                                  {
                                      IdVariable = C.IdConcepto,
                                      Nombre = C.Nombre + " - " + "Concepto"
                                  }).Union(
                                  from IE in Contexto.Informacion_Estadistica
                                  where IE.Hotel.IdHotel == idHotel
                                  select new ObjetoGenerico() 
                                  {
                                      IdVariable = IE.IdInformacionEstadistica,
                                      Nombre = IE.Nombre + " - " + "Información Estadstica"
                                  }
                                  )).OrderBy(V => V.Nombre).ToList();

                return listaVariables;
            }
        }

        public List<ObjetoGenerico> ObtenerListaVariablesSinVariablesConcepto(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();

                listaVariables = (from V in Contexto.Variable
                                  where V.Hotel.IdHotel == idHotel
                                  select new ObjetoGenerico()
                                  {
                                      IdVariable = V.IdVariable,
                                      Nombre = (V.Tipo == "P") ? V.Nombre + " - " + "Variable Propietario" : V.Nombre + " - " + "Variable Hotel"                                      
                                  }).OrderBy(V => V.Nombre).ToList();

                return listaVariables;
            }
        }

        public Extracto Obtener(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Extracto extractoTmp = new Extracto();
                extractoTmp = Contexto.
                              Extracto.
                              Include("Detalle_Extracto").
                              Where(E => E.Hotel.IdHotel == idHotel).
                              FirstOrDefault();
                return extractoTmp;
            }
        }

        public void GuardarActualizar(int idHotel, string txtDescripcion, string pieExtracto, List<ObjetoGenerico> listaVariable, string rutaFirmaLogo, short tipoExtracto)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Extracto extractoTmp = Contexto.
                                       Extracto.
                                       Where(E => E.Hotel.IdHotel == idHotel).
                                       FirstOrDefault();
                if (extractoTmp != null)
                    Contexto.DeleteObject(extractoTmp);

                Extracto extracto = new Extracto();
                extracto.DescripcionExtracto = txtDescripcion;
                extracto.PieExtracto = pieExtracto;
                extracto.TipoExtracto = tipoExtracto;
                extracto.FirmaLogo = rutaFirmaLogo;
                extracto.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);

                foreach (ObjetoGenerico itemDetalle in listaVariable)
                {
                    Detalle_Extracto detalleExt = new Detalle_Extracto();
                    detalleExt.IdVariable = itemDetalle.IdVariable;
                    detalleExt.NombreTabla = itemDetalle.NombreVariable;
                    detalleExt.IdControl = itemDetalle.Nombre;

                    extracto.Detalle_Extracto.Add(detalleExt);
                }

                Contexto.AddToExtracto(extracto);
                Contexto.SaveChanges();
            }
        }
    }
}
