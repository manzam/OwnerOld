using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Servicios;

namespace WebOwner.Reportes
{
    public partial class XtraReport_ReporteTres : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_ReporteTres(string condicion)
        {
            InitializeComponent();

            string sql = "SELECT " +
                         "Hotel.Nombre, " +
                         "Variable.Nombre AS NombreVariable, " +
                         "Variable.Descripcion, " +
                         "Variable.Tipo, " +
                         "Variable.Activo, " +
                         "Hotel.Nit " +
                         "FROM Variable " +
                         "INNER JOIN Hotel ON Variable.IdHotel = Hotel.IdHotel" +
                         condicion;

            DataTable tabla = Utilities.Select(sql, "reporteTres");

            //tabla.WriteXmlSchema("C:\\Users\\manuel\\Desktop\\reporteTres.xml");
            this.DataSource = tabla;
        }

    }
}
