using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace Servicios
{
    public class PaginaBase : System.Web.UI.Page
    {
        public void MiIdioma(string idioma)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(idioma);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(idioma);
        }

        protected override void InitializeCulture()
        {
        }
        
    }
}
