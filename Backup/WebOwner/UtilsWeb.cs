using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOwner
{
    public class UtilsWeb
    {
        public void GetEmailByHotel(int idHotel, ref string email, ref string pass)
        {
            if (Properties.Settings.Default.Bogota.Contains(idHotel.ToString()))
            {
                email = Properties.Settings.Default.CorreoRemitenteBogota;
                pass = Properties.Settings.Default.ClaveRemitenteBogota;
            }
            else if (Properties.Settings.Default.Costa.Contains(idHotel.ToString()))
            {
                email = Properties.Settings.Default.CorreoRemitenteCosta;
                pass = Properties.Settings.Default.ClaveRemitenteCosta;
            }
            else if (Properties.Settings.Default.Medellin.Contains(idHotel.ToString()))
            {
                email = Properties.Settings.Default.CorreoRemitenteMedellin;
                pass = Properties.Settings.Default.ClaveRemitenteMedellin;
            }
            else
            {
                email = Properties.Settings.Default.CorreoRemitente;
                pass = Properties.Settings.Default.ClaveRemitente;
            }
        }
    }
}
