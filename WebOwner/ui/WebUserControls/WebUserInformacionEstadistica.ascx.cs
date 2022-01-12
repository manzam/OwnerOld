using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DM;
using Servicios;
using System.IO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserInformacionEstadistica : System.Web.UI.UserControl
    {
        InformacionEstadisticaBo infoEstadisticaBoTmp;
        HotelBo hotelBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                this.txtFecha.Text = DateTime.Now.Year.ToString();

                this.CargarCombo();
                this.CargarGrilla();

                gvwDetalleVariable.DataSource = null;
                gvwDetalleVariable.DataBind();

                for (int i = DateTime.Now.Year; i > (DateTime.Now.Year - 20); i--)
                {
                    ListItem itemAno = new ListItem(i.ToString(), i.ToString());
                    ddlAno.Items.Add(itemAno);
                }

                this.IdInfoEstadisticaSeleccionado = -1;
            }
        }
        
        #region Propiedades

        public int IdInfoEstadisticaSeleccionado
        {
            get { return (int)ViewState["IdInfoEstadisticaSeleccionado"]; }
            set { ViewState["IdInfoEstadisticaSeleccionado"] = value; }
        }
        public bool EsAcumulada
        {
            get { return (bool)ViewState["EsAcumulada"]; }
            set { ViewState["EsAcumulada"] = value; }
        }

        #endregion

        #region Metodos
        private void CargarGrilla()
        {
            InformacionEstadisticaBo infoEstadisticasBoTmp = new InformacionEstadisticaBo();
            gvwInformacionEstadistica.DataSource = infoEstadisticasBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue));
            gvwInformacionEstadistica.DataBind();
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
        }

        private void VerDetalleVariable(int idInfoEstadistica, int ano)
        {
            InformacionEstadisticaBo infoEstadisticasBoTmp = new InformacionEstadisticaBo();
            List<ObjetoGenerico> listaVariable = infoEstadisticasBoTmp.VerDetalleHistorial(idInfoEstadistica, ano);

            gvwDetalleVariable.DataSource = listaVariable;
            gvwDetalleVariable.DataBind();
        }

        #endregion

        #region Boton
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                InformacionEstadisticaBo infoEstadisticaBoTmp = new InformacionEstadisticaBo();

                Informacion_Estadistica infoEstadisticaTmp = new Informacion_Estadistica();
                infoEstadisticaTmp.Nombre = txtNombre.Text;
                infoEstadisticaTmp.EsAcumulada = cbVariableAcumular.Checked;
                infoEstadisticaTmp.Orden = short.Parse(txtOrden.Text);
                infoEstadisticaTmp.Sufijo = ddlSufijo.SelectedValue;
                infoEstadisticaTmp.ValorAcumulado = short.Parse(ddlValorAcumulado.SelectedValue);
                infoEstadisticaTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", int.Parse(ddlHotel.SelectedValue));

                Historial_Informacion_Estadistica historial = new Historial_Informacion_Estadistica();                
                historial.Fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

                if (!cbVariableAcumular.Checked)
                {
                    historial.Valor = (txtValor.Text.Trim() == string.Empty) ? 0 : double.Parse(txtValor.Text);
                    infoEstadisticaTmp.Historial_Informacion_Estadistica.Add(historial);
                }

                infoEstadisticaBoTmp.Guardar(infoEstadisticaTmp);
                btnCancelar_Click(null, null);

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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            txtFecha.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtOrden.Text = "0";
            cbVariableAcumular.Checked = false;
            cbVariableAcumular_CheckedChanged(null, null);

            pnlGrilla.Visible = false;            
            btnNuevo.Visible = false;

            pnlNuevo.Visible = true;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
        }

        protected void btnDescarPlano_Click(object sender, EventArgs e)
        {
            infoEstadisticaBoTmp = new InformacionEstadisticaBo();            

            string ruta = Server.MapPath("../../interfases/PlanoInfoFinanciera_" + ddlHotel.SelectedItem.Text + ".txt");

            System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta);

            foreach (Informacion_Estadistica itemInfoEstadistica in infoEstadisticaBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue)))
            {
                sw.WriteLine(String.Concat(itemInfoEstadistica.Hotel.Codigo + ";", itemInfoEstadistica.Nombre + ";", DateTime.Now.ToString("dd/MM/yyyy") + ";", "0"));
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

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                infoEstadisticaBoTmp = new InformacionEstadisticaBo();
                hotelBoTmp = new HotelBo();

                string filename = System.IO.Path.GetFileName(AsyncFileUpload.FileName);
                string linea = string.Empty;                
                List<ObjetoGenerico> litsaInfoEstadistica = new List<ObjetoGenerico>();
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

                                    if (infoEstadisticaBoTmp.EsNombreVariableValido(lineaSplit[1], lineaSplit[0]))
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
                                        html += "<li>Valor no válido, fila: [ " + l + " ]</li>";
                                        esValido = esValido && false;
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
                                    litsaInfoEstadistica.Add(oInfoEstadistica);
                                }
                            }
                        }

                        html += "</ul>";

                        if (esValido)
                        {
                            infoEstadisticaBoTmp.GuardarPlano(litsaInfoEstadistica);

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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            CargarGrilla();

            pnlGrilla.Visible = true;
            btnNuevo.Visible = true;

            btnCancelar.Visible = false;
            pnlNuevo.Visible = false;
            btnGuardar.Visible = false;
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;

                infoEstadisticaBoTmp = new InformacionEstadisticaBo();
                infoEstadisticaBoTmp.Eliminar(int.Parse(btn.CommandArgument));

                CargarGrilla();

                this.divExito.Visible = true;
                this.lbltextoExito.Text = "La informacion estadistica historial, fue eliminada con exito.";
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

        #endregion

        #region Eventos

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvwDetalleVariable.DataSource = null;
            gvwDetalleVariable.DataBind();

            gvwInformacionEstadistica.EditIndex = -1;

            this.CargarGrilla();
        }

        protected void gvwInformacionEstadistica_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwInformacionEstadistica.SelectedRow;
            this.IdInfoEstadisticaSeleccionado = int.Parse(gvwInformacionEstadistica.DataKeys[filaSeleccionada.RowIndex]["IdInformacionEstadistica"].ToString());
            this.EsAcumulada = bool.Parse(gvwInformacionEstadistica.DataKeys[filaSeleccionada.RowIndex]["EsAcumulada"].ToString());            

            if (this.EsAcumulada)
            {
                gvwDetalleVariable.DataSource = null;
                gvwDetalleVariable.DataBind();
            }
            else
                this.VerDetalleVariable(this.IdInfoEstadisticaSeleccionado, int.Parse(ddlAno.SelectedValue));

            CargarGrilla();
        }

        protected void gvwInformacionEstadistica_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvwInformacionEstadistica.EditIndex = e.NewEditIndex;

            int idInfoEstadistica = int.Parse(gvwInformacionEstadistica.DataKeys[e.NewEditIndex]["IdInformacionEstadistica"].ToString());
            this.EsAcumulada = bool.Parse(gvwInformacionEstadistica.DataKeys[e.NewEditIndex]["EsAcumulada"].ToString());            
                        
            CargarGrilla();
            GridViewRow filaSeleccionada = gvwInformacionEstadistica.Rows[e.NewEditIndex];

            if (this.EsAcumulada)
            {                
                ((DropDownList)(filaSeleccionada.FindControl("ddlMes"))).Enabled = false;
                ((TextBox)(filaSeleccionada.FindControl("txtFecha"))).Enabled = false;
                ((TextBox)(filaSeleccionada.FindControl("txtValor"))).Enabled = false;

                ((RequiredFieldValidator)gvwInformacionEstadistica.Rows[filaSeleccionada.RowIndex].FindControl("RequiredFieldValidator4")).Enabled = false;

                this.divExito.Visible = true;
                this.lbltextoExito.Text = "Esta variable, solo se puede actualizar el nombre, porque es una variable de acumulacion.";
                this.divError.Visible = false;

                gvwDetalleVariable.DataSource = null;
                gvwDetalleVariable.DataBind();
            }
            else
            {
                ((RequiredFieldValidator)gvwInformacionEstadistica.Rows[filaSeleccionada.RowIndex].FindControl("RequiredFieldValidator4")).Enabled = false;
                this.VerDetalleVariable(idInfoEstadistica, DateTime.Now.Year);
            }

            InformacionEstadisticaBo infoEstadisticasBoTmp = new InformacionEstadisticaBo();
            ((TextBox)(gvwInformacionEstadistica.Rows[filaSeleccionada.RowIndex].FindControl("txtValor"))).Text = (infoEstadisticasBoTmp.ObtenerValorInfoEstadistica(idInfoEstadistica, DateTime.Now.Year, 1)).ToString();
        }

        protected void gvwInformacionEstadistica_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvwInformacionEstadistica.EditIndex = -1;
            CargarGrilla();
        }

        protected void gvwInformacionEstadistica_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int idInfoEstadistica = int.Parse(gvwInformacionEstadistica.DataKeys[e.RowIndex].Value.ToString());
                int ano = int.Parse(((TextBox)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("txtFecha"))).Text);
                int mes = int.Parse(((DropDownList)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("ddlMes"))).SelectedValue);
                string sufijo = ((DropDownList)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("ddlSufijo"))).SelectedValue;
                string nombre = ((TextBox)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("txtNombreVariable"))).Text;
                bool esAcumulada = ((CheckBox)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("cbEsAcumulada"))).Checked;

                string valor = ((TextBox)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("txtValor"))).Text.Trim();
                double valorTmp = (valor == string.Empty) ? 0 : double.Parse(valor);

                string orden = ((TextBox)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("txtOrden"))).Text.Trim();
                short ordenTmp = (orden == string.Empty) ? short.Parse("0") : short.Parse(orden);
                short valorAcumulado = short.Parse(((DropDownList)(gvwInformacionEstadistica.Rows[e.RowIndex].FindControl("ddlValorAcumulado"))).SelectedValue);

                DateTime fecha = new DateTime(ano, mes, 1);

                InformacionEstadisticaBo infoEstadisticasBoTmp = new InformacionEstadisticaBo();
                infoEstadisticasBoTmp.ActualizarValorInfoEstadistica(idInfoEstadistica, valorTmp, fecha, nombre, esAcumulada, ordenTmp, sufijo, valorAcumulado);

                gvwInformacionEstadistica.EditIndex = -1;
                CargarGrilla();
                this.VerDetalleVariable(idInfoEstadistica, DateTime.Now.Year);

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
            if (this.IdInfoEstadisticaSeleccionado != -1)
                this.VerDetalleVariable(this.IdInfoEstadisticaSeleccionado, int.Parse(ddlAno.SelectedValue));
        }

        protected void cbVariableAcumular_CheckedChanged(object sender, EventArgs e)
        {
            txtValor.Enabled = !cbVariableAcumular.Checked;
            RequiredFieldValidator3.Enabled = !cbVariableAcumular.Checked;
        }
        
        protected void gvwDetalleVariable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                infoEstadisticaBoTmp = new InformacionEstadisticaBo();
                infoEstadisticaBoTmp.EliminarHistorial((int)gvwDetalleVariable.DataKeys[e.RowIndex].Value);

                VerDetalleVariable(this.IdInfoEstadisticaSeleccionado, int.Parse(ddlAno.SelectedValue));

                this.divExito.Visible = true;
                this.lbltextoExito.Text = "La informacion estadistica, fue eliminada con exito.";
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

        #endregion        
    }
}