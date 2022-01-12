using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using System.IO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebValorVariable3 : System.Web.UI.UserControl
    {
        VariableBo variableBoTmp;
        HotelBo hotelBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.EsReporte = false;

                txtFecha.Text = DateTime.Now.Year.ToString();

                this.CargarCombo();
                this.CargarGrilla();

                gvwDetalleVariable.DataSource = null;
                gvwDetalleVariable.DataBind();

                for (int i = DateTime.Now.Year; i > (DateTime.Now.Year - 20); i--)
                {
                    ListItem itemAno = new ListItem(i.ToString(), i.ToString());
                    ddlAno.Items.Add(itemAno);
                }

                this.IdVariableSeleccionado = -1;
            }

            if (this.EsReporte)
                btnAceptarReporte_Click(null, null);
        }

        #region Propiedades

        public int IdVariableSeleccionado
        {
            get { return (int)ViewState["IdVariableSeleccionado"]; }
            set { ViewState["IdVariableSeleccionado"] = value; }
        }
        public bool EsReporte 
        {
            get { return (bool)ViewState["EsReporte"]; }
            set { ViewState["EsReporte"] = value; }
        }
        public ObjetoGenerico UsuarioLogin 
        {
            get { return (ObjetoGenerico)((ObjetoGenerico)Session["usuarioLogin"]); }
        }

        #endregion

        #region Metodos

        private void CargarGrilla()
        {
            variableBoTmp = new VariableBo();
            gvwVariable.DataSource = variableBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue), true, "H");
            gvwVariable.DataBind();
        }

        private void CargarCombo()
        {
            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataSource = listaHotel;
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();

            ddlHotelReporte.DataSource = listaHotel;
            ddlHotelReporte.DataTextField = "Nombre";
            ddlHotelReporte.DataValueField = "IdHotel";
            ddlHotelReporte.DataBind();
        }

        private void VerDetalleVariable(int idVariable, int ano)
        {
            VariableBo variableBoTmp = new VariableBo();
            List<ObjetoGenerico> listaVariable = variableBoTmp.VerDetalleVariable(idVariable, ano);

            gvwDetalleVariable.DataSource = listaVariable;
            gvwDetalleVariable.DataBind();
        }
        
        #endregion

        #region Boton
        protected void btnAceptarReporte_Click(object sender, EventArgs e)
        {
            if (ddlHotelReporte.Items.Count > 0)
            {
                this.EsReporte = true;
                DateTime fecha = new DateTime(int.Parse(txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1);
                Reportes.XtraReport_Variables reporteTmp = new WebOwner.Reportes.XtraReport_Variables(int.Parse(ddlHotel.SelectedValue), ddlHotel.SelectedItem.Text, fecha);
                ReportViewer_ReporteVariables.Report = reporteTmp;
            }
        }
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                variableBoTmp = new VariableBo();
                hotelBoTmp = new HotelBo();

                string filename = System.IO.Path.GetFileName(AsyncFileUpload.FileName);
                string linea = string.Empty;
                List<ObjetoGenerico> litsaValorVariable = new List<ObjetoGenerico>();
                bool esValido = true;
                short l = 0;
                string html = "<ul>";

                if (filename.Split(new char[] { '.' })[1] == "txt")
                {
                    if (AsyncFileUpload.HasFile)
                    {
                        AsyncFileUpload.SaveAs(Server.MapPath("../../interfases/") + filename);

                        using (StreamReader sr = new StreamReader(Server.MapPath("../../interfases/") + filename))
                        {
                            while ((linea = sr.ReadLine()) != null)
                            {
                                if (linea.Trim() != string.Empty)
                                {
                                    //Codigo Hotel ; Nombre variable estadistica ; Fecha ; Valor
                                    string[] lineaSplit = linea.Split(new char[] { ';' });

                                    ObjetoGenerico oInfoEstadistica = new ObjetoGenerico();
                                    if (hotelBoTmp.EsCodigoHotelValido(lineaSplit[0]))
                                        oInfoEstadistica.Codigo = lineaSplit[0];
                                    else
                                    {
                                        html += "<li>El codigo del hotel no existe, fila: [ " + l + " ]</li>";
                                        esValido = esValido && false;
                                    }

                                    if (variableBoTmp.EsNombreVariableValido(lineaSplit[1], lineaSplit[0]))
                                        oInfoEstadistica.NombreVariable = lineaSplit[1];
                                    else
                                    {
                                        html += "<li>La variable no existe para ese Hotel, fila: [ " + l + " ]</li>";
                                        esValido = esValido && false;
                                    }

                                    try
                                    {
                                        oInfoEstadistica.Valor = double.Parse(lineaSplit[3]);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.divExito.Visible = false;
                                        this.divError.Visible = true;
                                        this.lbltextoError.Text = "Valor no válido, fila: [ " + l + " ] punto sólo para decimales.";
                                    }
                                    

                                    try
                                    {
                                        oInfoEstadistica.Fecha = DateTime.Parse(lineaSplit[2]);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.divExito.Visible = false;
                                        this.divError.Visible = true;
                                        this.lbltextoError.Text = "Fecha no válida, fila: [ " + l + " ] \"dia/Mes/año\" ";
                                    }

                                    l++;
                                    litsaValorVariable.Add(oInfoEstadistica);
                                }
                            }
                        }

                        html += "</ul>";

                        if (esValido)
                        {
                            variableBoTmp.GuardarPlano(litsaValorVariable);
                            ddlHotel_SelectedIndexChanged(null, null);
                            this.divExito.Visible = true;
                            this.divError.Visible = false;
                            this.lbltextoExito.Text = "El archivo subio con exito";
                        }
                        else
                        {
                            this.divExito.Visible = false;
                            this.divError.Visible = true;
                            this.lbltextoError.Text = html;
                        }

                        File.Delete(Server.MapPath("../../interfases/") + filename);
                    }
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "El archivo debe tener extension .txt";
                }
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }
        protected void btnDescarPlano_Click(object sender, EventArgs e)
        {
            variableBoTmp = new VariableBo();

            string ruta = Server.MapPath("../../interfases/PlanoValorVariable_" + ddlHotel.SelectedItem.Text.ToUpper() + ".txt");

            System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta);

            foreach (DM.Variable itemVariable in variableBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue), true, "H"))
            {
                //Codigo Hotel;Nombre Variable;Fecha;Valor
                sw.WriteLine(String.Concat(itemVariable.Hotel.Codigo + ";", itemVariable.Nombre + ";", DateTime.Now.ToString("dd/MM/yyyy") + ";", "0"));
            }

            sw.Close();
            FileInfo fileInfo = new FileInfo(ruta);

            Response.Clear();
            Response.ClearHeaders();
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());

            Response.WriteFile(ruta);
            Response.End();
        }
        #endregion

        #region Eventos

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.IdVariableSeleccionado = -1;
            gvwDetalleVariable.DataSource = null;
            gvwDetalleVariable.DataBind();
            this.CargarGrilla();
        }

        protected void gvwVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwVariable.SelectedRow;            
            this.IdVariableSeleccionado = int.Parse(gvwVariable.DataKeys[filaSeleccionada.RowIndex].Value.ToString());

            this.VerDetalleVariable(this.IdVariableSeleccionado, int.Parse(txtFecha.Text));
            CargarGrilla();
        }

        protected void gvwVariable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvwVariable.EditIndex = e.NewEditIndex;
            int idVariable = int.Parse(gvwVariable.DataKeys[e.NewEditIndex].Value.ToString());

            this.VerDetalleVariable(idVariable, DateTime.Now.Year);
            CargarGrilla();
        }

        protected void gvwVariable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvwVariable.EditIndex = -1;
            CargarGrilla();
        }

        protected void gvwVariable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int idVariable = int.Parse(gvwVariable.DataKeys[e.RowIndex].Value.ToString());
                int ano = int.Parse(((TextBox)(gvwVariable.Rows[e.RowIndex].FindControl("txtFecha"))).Text);
                int mes = int.Parse(((DropDownList)(gvwVariable.Rows[e.RowIndex].FindControl("ddlMes"))).SelectedValue);
                string nombreVariable = ((Label)(gvwVariable.Rows[e.RowIndex].FindControl("Label3"))).Text;
                string valorTmp = ((TextBox)(gvwVariable.Rows[e.RowIndex].FindControl("txtValor"))).Text;

                DateTime fecha = new DateTime(ano, mes, 1);
                double valor = 0;//double.Parse(valorTmp);

                if (valorTmp.Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
                {
                    valor = Convert.ToDouble(valorTmp, System.Globalization.CultureInfo.InvariantCulture);

                }
                else // aca quiere decir que puso una coma y lo reemplaza por un punto
                {
                    valorTmp.Replace(',', '.');
                    valor = Convert.ToDouble(valorTmp);
                }

                VariableBo variableBoTmp = new VariableBo();
                variableBoTmp.ActualizarValorVariable(idVariable, valor, fecha, ddlHotel.SelectedItem.Text, nombreVariable, this.UsuarioLogin.Id);

                gvwVariable.EditIndex = -1;
                CargarGrilla();
                this.VerDetalleVariable(idVariable, DateTime.Now.Year);

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvwDetalleVariable.DataSource = null;
            gvwDetalleVariable.DataBind();

            if (this.IdVariableSeleccionado != -1)
                this.VerDetalleVariable(this.IdVariableSeleccionado, int.Parse(ddlAno.SelectedValue));
        }

        #endregion 
    }
}