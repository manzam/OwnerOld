using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Servicios;

namespace WebOwner.Reportes
{
    public partial class XtraReport_PerfilPermiso : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport_PerfilPermiso()
        {
            InitializeComponent();

            string sql = "SELECT " +
                         "Perfil.IdPerfil, " +
                         "Perfil.Nombre, " +
                         "Perfil.Descripcion, " +
                         "Modulo.Nombre AS Modulo " +
                         "FROM Modulo_Perfil " +
                         "INNER JOIN Modulo ON Modulo_Perfil.IdModulo = Modulo.IdModulo " +
                         "INNER JOIN Perfil ON Modulo_Perfil.IdPerfil = Perfil.IdPerfil " +
                         "order by Nombre";

            DataTable tabla = Utilities.Select(sql, "reportePerfil");

            DataView view = new DataView(tabla);
            DataTable tablaPerfil = view.ToTable(true, "IdPerfil");

            tblTabla.Rows.Clear();

            XRTableCell celda = null;
            XRTableRow fila = null;

            foreach (DataRow itemPerfil in tablaPerfil.Rows)
            {
                DataRow[] filasPerfil = tabla.Select("IdPerfil = " + itemPerfil[0]);

                fila = new XRTableRow();
                fila.HeightF = 20F;

                celda = new XRTableCell();
                celda.WidthF = 100F;
                celda.Text = "Perfil";
                celda.BackColor = Color.FromArgb(117, 153, 169);
                celda.ForeColor = Color.White;
                fila.Cells.Add(celda);

                celda = new XRTableCell();
                celda.Text = filasPerfil[0][1].ToString().ToUpper();
                fila.Cells.Add(celda);

                tblTabla.Rows.Add(fila);

                fila = new XRTableRow();
                fila.HeightF = 20F;

                celda = new XRTableCell();
                celda.Text = "Descripción";
                celda.WidthF = 100F;
                celda.BackColor = Color.FromArgb(117, 153, 169);
                celda.ForeColor = Color.White;
                fila.Cells.Add(celda);

                celda = new XRTableCell();
                celda.Text = filasPerfil[0][2].ToString().ToUpper();
                fila.Cells.Add(celda);

                tblTabla.Rows.Add(fila);

                fila = new XRTableRow();
                fila.HeightF = 20F;

                celda = new XRTableCell();
                celda.Text = "Permisos";
                celda.BackColor = Color.FromArgb(117, 153, 169);
                celda.ForeColor = Color.White;
                fila.Cells.Add(celda);
                tblTabla.Rows.Add(fila);

                for (int i = 0; i < filasPerfil.Length; i++)
                {
                    fila = new XRTableRow();
                    fila.HeightF = 20F;

                    celda = new XRTableCell();
                    celda.Text = filasPerfil[i][3].ToString().ToUpper();
                    fila.Cells.Add(celda);
                    tblTabla.Rows.Add(fila);
                }

                fila = new XRTableRow();
                fila.HeightF = 20F;

                celda = new XRTableCell();
                fila.Cells.Add(celda);
                tblTabla.Rows.Add(fila);
            }
        }

    }
}
