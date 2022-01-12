using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class EnvioMasivoBo
    {
        public void Guardar(DateTime fechaLiquidacion, int idHotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                HistorialEnvioMasivo oHistorial = Contexto.HistorialEnvioMasivo.
                                                  Where(H => H.FechaPeriodoLiquidacion.Year == fechaLiquidacion.Year &&
                                                             H.FechaPeriodoLiquidacion.Month == fechaLiquidacion.Month &&
                                                             H.Hotel.IdHotel == idHotel).FirstOrDefault();
                if (oHistorial == null)
                {

                    HistorialEnvioMasivo oHistorialEnvio = new HistorialEnvioMasivo();
                    oHistorialEnvio.FechaPeriodoLiquidacion = fechaLiquidacion;
                    oHistorialEnvio.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                    oHistorialEnvio.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);

                    Contexto.AddToHistorialEnvioMasivo(oHistorialEnvio);
                    Contexto.SaveChanges();
                }
            }
        }

        public bool EsMasivoEnviado(DateTime fechaLiquidacion, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.HistorialEnvioMasivo.
                          Where(H => H.FechaPeriodoLiquidacion.Year == fechaLiquidacion.Year &&
                                     H.FechaPeriodoLiquidacion.Month == fechaLiquidacion.Month &&
                                     H.Hotel.IdHotel == idHotel).Count();

                return (con > 0) ? true : false;
            }
        }
    }
}
