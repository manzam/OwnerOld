using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Servicios;

namespace WebOwner.Reportes
{
    public partial class XtraReport_ReporteCuatro : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_ReporteCuatro(string condicion)
        {
            InitializeComponent();

            string sql = "SELECT distinct " +
                         "Usuario.Nombre, " +
                         "Usuario.Apellido, " +
                         "Usuario.Identificacion, " +
                         "Usuario.Login, " +
                         "Usuario.Correo, " +
                         "Usuario.Telefono_1, " +
                         "Usuario.Telefono_2, " +
                         "Usuario.Activo, " +
                         "Hotel.Nombre AS Hotel, " +
                         "Hotel.Nit, " +
                         "Perfil.Nombre AS Perfil " +
                         "FROM Usuario " +
                         "INNER JOIN Hotel_Usuario ON Usuario.IdUsuario = Hotel_Usuario.IdUsuario " +
                         "INNER JOIN Hotel ON Hotel_Usuario.IdHotel = Hotel.IdHotel " +
                         "INNER JOIN Perfil ON Usuario.IdPerfil = Perfil.IdPerfil " +
                         condicion +
                         " UNION ( " +
                         "SELECT Usuario.Nombre, Usuario.Apellido, Usuario.Identificacion, Usuario.Login, Usuario.Correo, " +
                         "Usuario.Telefono_1, Usuario.Telefono_2, Usuario.Activo, 'Todos los Hoteles', '', Perfil.Nombre AS Perfil " +
                         "FROM Usuario " +
                         "INNER JOIN Perfil ON Usuario.IdPerfil = Perfil.IdPerfil " +
                         "where Perfil.IdPerfil = " + Properties.Settings.Default.IdSuperUsuario + ") " + 
                         "UNION ( " +
                         "SELECT Usuario.Nombre, Usuario.Apellido, Usuario.Identificacion, Usuario.Login, Usuario.Correo, " +
                         "Usuario.Telefono_1, Usuario.Telefono_2, Usuario.Activo, 'Todos los Hoteles', '', 'Sin Perfil' " +
                         "FROM Usuario " +
                         "where Usuario.IdPerfil is null)";

            DataTable tabla = Utilities.Select(sql, "reporteCuatro");

            //tabla.WriteXmlSchema("C:\\Users\\manuel\\Desktop\\reporteCuatro.xml");
            this.DataSource = tabla;
        }

    }
}
