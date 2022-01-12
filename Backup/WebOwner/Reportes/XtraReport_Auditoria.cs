using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Servicios;

namespace WebOwner.Reportes
{
    public partial class XtraReport_Auditoria : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_Auditoria(string nombreModulo, string fechaDesde, string fechaHasta, string condicion)
        {
            InitializeComponent();

            this.lblModulo.Text = nombreModulo;
            this.lblFechaDesde.Text = fechaDesde;
            this.lblFechaHasta.Text = fechaHasta;

            string sql = "SELECT " +
                         "Auditoria.ValorNuevo, " + 
                         "Auditoria.ValorAnterior, " + 
                         "Auditoria.NombreTabla, " +  
                         "Auditoria.Accion, " + 
                         "Auditoria.Campo, " + 
                         "Auditoria.Fechahora, " +  
                         "Usuario.Nombre, " + 
                         "Usuario.Apellido, " + 
                         "Usuario.Identificacion, " + 
                         "Usuario.Login " + 
                         "FROM " + 
                         "Auditoria " + 
                         "INNER JOIN Usuario ON Auditoria.IdUsuario = Usuario.IdUsuario " +
                         condicion +
                         " order by Auditoria.Fechahora,Auditoria.Accion,Auditoria.Campo";

            DataTable tabla = Utilities.Select(sql, "reporteAuditoria");

            //tabla.WriteXmlSchema("C:\\Users\\manuel\\Desktop\\reporteAuditoria.xml");
            this.DataSource = tabla;
        }
    }
}
