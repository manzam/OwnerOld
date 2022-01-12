using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebOwner.handlers
{
    /// <summary>
    /// Summary description for HandlerExtracto
    /// </summary>
    public class HandlerExtracto : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            DataExtracto dataSave = js.Deserialize<DataExtracto>(HttpContext.Current.Request.Params["data"]);

            DateTime fecha = new DateTime(dataSave.yyyy, dataSave.mm, 1);

            LiquidacionBo oLiquidacionBo = new LiquidacionBo();
            string urlPDF = oLiquidacionBo.ObtenerUrlExtracto(fecha, dataSave.IdProp, dataSave.IdSuite);
            if (!string.IsNullOrEmpty(urlPDF))
            {
                string fileName = urlPDF;
                string filePath = context.Server.MapPath(string.Format("~/extractos/{0}/{1}", dataSave.IdHotel, urlPDF));
                context.Response.Clear();
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                context.Response.TransmitFile(filePath);
                context.Response.End();
            }
        }

        public class DataExtracto
        {
            public int IdProp { get; set; }
            public int IdSuite { get; set; }
            public string IdHotel { get; set; }
            public int mm { get; set; }
            public int yyyy { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}