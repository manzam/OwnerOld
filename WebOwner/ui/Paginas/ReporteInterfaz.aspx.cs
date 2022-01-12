using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Servicios;

namespace WebOwner.ui.Paginas
{
    public partial class ReporteInterfaz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //List<ObjetoGenerico> listaIdSuiteIdPropietario = new List<ObjetoGenerico>();
                //Session["listaIdSuiteIdPropietario"] = listaIdSuiteIdPropietario;
                Session.Remove("listaIdSuiteIdPropietario");

                FileInfo fileInfo = (FileInfo)Session["fileInfo"];
                string ruta = (string)Session["ruta"];

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());                

                Response.WriteFile(ruta);                
                Response.End();

                Response.Redirect(Server.MapPath("~/ui/Paginas/Interfaz.aspx"));
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
            }
            
        }
    }
}
