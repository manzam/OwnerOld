using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Servicios;

namespace WebOwner.Reportes
{
    public partial class XtraReport_ReporteDos : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_ReporteDos(string condicion)
        {
            InitializeComponent();

            string sql = "SELECT " +    
                         "Hotel.Nombre, " +
                         "Hotel.Nit, " +
                         "Hotel.Direccion, " +
                         "Hotel.Correo, " +
                         "Hotel.CorreoReservas, " +
                         "Hotel.Codigo, " +
                         "Hotel.UnidadNegocio, " +
                         "Suit.NumSuit, " +
                         "Suit.NumEscritura, " +
                         "Suit.RegistroNotaria, " +
                         "Suit.Descripcion, " +
                         "Suit.Activo " +
                         "FROM Hotel " +
                         "INNER JOIN Suit ON Hotel.IdHotel = Suit.IdHotel " +
                         condicion;

            DataTable tabla = Utilities.Select(sql, "reporteDos");

            //tabla.WriteXmlSchema("C:\\Users\\manuel\\Desktop\\reporteDos.xml");
            this.DataSource = tabla;
        }

    }
}
