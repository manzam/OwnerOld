using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using Servicios;
using System.Drawing;
using System.Text;
using DM;
using System.Globalization;
using System.IO;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserLiquidacion : System.Web.UI.UserControl
    {
        CierreBo cierreBo;
        LiquidacionBo liquidacionBo;

        protected void Page_Load(object sender, EventArgs e)
        {
            //divError.Visible = false;
            //divExito.Visible = false;

            //uc_WebUserBuscadorPropietarioSuite.AlAceptar += new EventHandler(uc_WebUserBuscadorPropietarioSuite_AlAceptar);

            //if (!IsPostBack)
            //{
            //    this.txtFecha.Text = DateTime.Now.Year.ToString();
            //    this.txtFechaDesde.Text = DateTime.Now.Year.ToString();

            //    ParametroBo parametroBoTmp = new ParametroBo();
            //    this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
            //    //gvwPropietarios.PageSize = this.PageSize;

            //    List<int> listaIdSuite = new List<int>();
            //    Session["listaIdSuite"] = listaIdSuite;

            //    List<int> listaIdSuiteBuscar = new List<int>();
            //    Session["listaIdSuiteBuscar"] = listaIdSuiteBuscar;

            CargarCombos();
            //    ddlHotel_SelectedIndexChanged(null, null);

            //    gvwPropietariosBuscar.DataSource = null;
            //    gvwPropietariosBuscar.DataBind();

            //    gvwDetalleConceptoHotel.DataSource = null;
            //    gvwDetalleConceptoHotel.DataBind();

            //}            

            String hiddenFieldValue = hidLastTab.Value;
            StringBuilder js = new StringBuilder();
            js.Append("<script type='text/javascript'>");
            js.Append("var previouslySelectedTab = ");
            js.Append(hiddenFieldValue);
            js.Append(";</script>");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "acttab", js.ToString());
        }

        //    #region Propiedades
        //    public int PageSize
        //    {
        //        get { return (int)ViewState["PageSize"]; }
        //        set { ViewState["PageSize"] = value; }
        //    }
        //    public bool esLiquidacionTotal
        //    {
        //        get { return (bool)ViewState["esLiquidacionTotal"]; }
        //        set { ViewState["esLiquidacionTotal"] = value; }
        //    }
        //    public int IdHotel
        //    {
        //        get { return (int)ViewState["IdHotel"]; }
        //        set { ViewState["IdHotel"] = value; }
        //    }


        //    #endregion

        //    #region Metodos

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = null;

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
            {
                listaHotel = hotelBoTmp.VerTodos();
                ddlHotel.DataSource = listaHotel;
                ddlHotelEliminarLiquidacion.DataSource = listaHotel;
            }
            else
            {
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);
                ddlHotel.DataSource = listaHotel;
                ddlHotelEliminarLiquidacion.DataSource = listaHotel;
            }

            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();

            ddlHotelEliminarLiquidacion.DataTextField = "Nombre";
            ddlHotelEliminarLiquidacion.DataValueField = "IdHotel";
            ddlHotelEliminarLiquidacion.DataBind();
        }

        //    private void CargarPropietarios()
        //    {
        //        //PropietarioBo propietarioBoTmp = new PropietarioBo();
        //        //gvwPropietarios.DataSource = propietarioBoTmp.ObtenerPropietariosConSuiteActivas(int.Parse(ddlHotel.SelectedValue));
        //        //gvwPropietarios.DataBind();
        //    }

        //    protected void imgBtnSeleccion_Click(object sender, ImageClickEventArgs e)
        //    {
        //        ImageButton imgBtnTmp = (ImageButton)sender;
        //        List<int> listaIdSuite = (List<int>)Session["listaIdSuite"];

        //        if (imgBtnTmp.ImageUrl == "~/img/117.png")
        //        {
        //            imgBtnTmp.ImageUrl = "~/img/95.png";
        //            listaIdSuite.Add(int.Parse(imgBtnTmp.CommandArgument));
        //        }
        //        else
        //        {
        //            imgBtnTmp.ImageUrl = "~/img/117.png";
        //            listaIdSuite.Remove(int.Parse(imgBtnTmp.CommandArgument));
        //        }

        //        Session["listaIdSuite"] = listaIdSuite;
        //    }

        //    /// <summary>
        //    /// Valida si las variables estan ya diligenciadas
        //    /// </summary>
        //    /// <param name="tipo"></param>
        //    /// <param name="idHotel"></param>
        //    /// <param name="fecha"></param>
        //    /// <returns></returns>
        //    private bool EsCorrectoVariables(string tipo, int idHotel, DateTime fecha)
        //    {
        //        VariableBo variableBotmp = new VariableBo();
        //        List<ObjetoGenerico> listaVariables = variableBotmp.ValidarVariable(tipo, idHotel, fecha);

        //        if (listaVariables.Count > 0)
        //        {
        //            gvwVariablesPendientes.DataSource = listaVariables;
        //            gvwVariablesPendientes.DataBind();

        //            pnlVariablespendientes.Visible = true;

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_14;

        //            return false;
        //        }
        //        else
        //        {
        //            pnlVariablespendientes.Visible = false;
        //            return true;
        //        }
        //    }

        //    private void MostrarPreLiquidarPropietario(int numColumnas, List<ObjetoGenerico> listaResultadosConceptosFinal, List<string> listaConcepto)
        //    {
        //        listaResultadosConceptosFinal.OrderBy(L => L.Orden);
        //        try
        //        {
        //            if (listaResultadosConceptosFinal.Count > 0)
        //            {
        //                //btnAceptarLiquidacionpropietario_uno.Visible = true;
        //                //btnAceptarLiquidacionpropietario_dos.Visible = true;

        //                Table miTabla = new Table();
        //                //miTabla.Width = new Unit("100%");

        //                TableRow miFila;
        //                TableCell miCelda;

        //                miFila = new TableRow();
        //                miFila.BorderColor = Color.FromArgb(117, 153, 169);
        //                miFila.BorderStyle = BorderStyle.Solid;
        //                miFila.BorderWidth = new Unit(1);                    
        //                miFila.BackColor = Color.FromName("#7599A9");

        //                miCelda = new TableCell();
        //                miCelda.ForeColor = Color.White;
        //                miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                miCelda.Text = "Propietario";
        //                miCelda.Font.Size = new FontUnit(10);
        //                miFila.Cells.Add(miCelda);

        //                miCelda = new TableCell();
        //                miCelda.ForeColor = Color.White;
        //                miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                miCelda.Text = "Nit";
        //                miCelda.Font.Size = new FontUnit(10);
        //                miFila.Cells.Add(miCelda);

        //                miCelda = new TableCell();
        //                miCelda.ForeColor = Color.White;
        //                miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                miCelda.Text = "N° Suit";
        //                miCelda.Font.Size = new FontUnit(10);
        //                miFila.Cells.Add(miCelda);

        //                miCelda = new TableCell();
        //                miCelda.ForeColor = Color.White;
        //                miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                miCelda.Text = "N° Escritura";
        //                miCelda.Font.Size = new FontUnit(10);
        //                miFila.Cells.Add(miCelda);

        //                for (int n = 0; n < numColumnas; n++)
        //                {
        //                    miCelda = new TableCell();
        //                    miCelda.ForeColor = Color.White;
        //                    miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                    miCelda.Text = listaConcepto[n];
        //                    miCelda.Font.Size = new FontUnit(10);
        //                    miFila.Cells.Add(miCelda);
        //                }
        //                miTabla.Rows.Add(miFila);

        //                // Resultados
        //                int numColumnasTmp = numColumnas;

        //                for (int c = 0; c < listaResultadosConceptosFinal.Count; c = c + numColumnas)
        //                {
        //                    miFila = new TableRow();
        //                    miFila.CssClass = "Celda";

        //                    miCelda = new TableCell();
        //                    miCelda.Text = listaResultadosConceptosFinal[c].NombreCompleto;
        //                    miCelda.HorizontalAlign = HorizontalAlign.Left;
        //                    miFila.Cells.Add(miCelda);

        //                    miCelda = new TableCell();
        //                    miCelda.Text = listaResultadosConceptosFinal[c].NumIdentificacion;
        //                    miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                    miFila.Cells.Add(miCelda);

        //                    miCelda = new TableCell();
        //                    miCelda.Text = listaResultadosConceptosFinal[c].NumSuit;
        //                    miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                    miFila.Cells.Add(miCelda);

        //                    miCelda = new TableCell();
        //                    miCelda.Text = listaResultadosConceptosFinal[c].NumEscritura;
        //                    miCelda.HorizontalAlign = HorizontalAlign.Center;
        //                    miFila.Cells.Add(miCelda);

        //                    for (int i = c; i < numColumnasTmp; i++)
        //                    {
        //                        if (i < listaResultadosConceptosFinal.Count)
        //                        {

        //                            miCelda = new TableCell();
        //                            miCelda.Text = listaResultadosConceptosFinal[i].Valor.ToString("N" + listaResultadosConceptosFinal[i].NumDecimales);
        //                            miCelda.HorizontalAlign = HorizontalAlign.Right;
        //                            miCelda.Style.Add(HtmlTextWriterStyle.PaddingRight, "10px");
        //                            miCelda.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
        //                            miFila.Cells.Add(miCelda);
        //                        }
        //                    }
        //                    numColumnasTmp = numColumnasTmp + numColumnas;
        //                    miTabla.Rows.Add(miFila);
        //                }


        //                //Calculo de las sumatorias de las columnas.
        //                miFila = new TableRow();
        //                miFila.BorderColor = Color.FromArgb(117, 153, 169);
        //                miFila.BorderStyle = BorderStyle.Solid;
        //                miFila.BorderWidth = new Unit(1);
        //                miFila.BackColor = Color.FromName("#7599A9");

        //                miCelda = new TableCell();
        //                miCelda.ForeColor = Color.White;
        //                miCelda.BorderStyle = BorderStyle.Solid;
        //                miCelda.HorizontalAlign = HorizontalAlign.Right;
        //                miCelda.Style.Add(HtmlTextWriterStyle.Padding, "3px");
        //                miCelda.Text = "Totales";
        //                miCelda.ColumnSpan = 4;
        //                miFila.Cells.Add(miCelda);

        //                double sumaColumna;

        //                for (int c = 0; c < miTabla.Rows[0].Cells.Count - 4; c++)
        //                {
        //                    sumaColumna = 0;
        //                    for (int v = 1; v < miTabla.Rows.Count; v++)
        //                    {
        //                        sumaColumna += double.Parse(miTabla.Rows[v].Cells[c + 4].Text);
        //                    }

        //                    miCelda = new TableCell();
        //                    miCelda.Text = sumaColumna.ToString("N4");
        //                    miCelda.ForeColor = Color.White;
        //                    miCelda.HorizontalAlign = HorizontalAlign.Right;
        //                    miCelda.Style.Add(HtmlTextWriterStyle.PaddingRight, "10px");
        //                    miCelda.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
        //                    miFila.Cells.Add(miCelda);
        //                }

        //                miTabla.Rows.Add(miFila);

        //                //pnlTablaLiquidacion.Controls.Add(miTabla);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }
        //    }

        //    private bool EsValidoLiquidarPropietario(DateTime periodo, int idHotel)
        //    {
        //        LiquidacionBo liquidacionBoTmp = new LiquidacionBo();
        //        return liquidacionBoTmp.EsValidoLiquidarPropietario(periodo, idHotel);
        //    }        

        //    #endregion

        //    #region Eventos

        //    protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        //    {
        //        Session["listaIdSuite"] = new List<int>();

        //        //btnAceptarLiquidacionpropietario_uno.Visible = false;
        //        //btnAceptarLiquidacionpropietario_dos.Visible = false;
        //        btnLiquidarTodosHotel.Visible = false;
        //        gvwPreLiquidacionHotel.Visible = false;
        //        btnGuardarTodoHotel.Visible = false;

        //        ConceptoBo conceptoBoTmp = new ConceptoBo();
        //        gvwConceptos.DataSource = conceptoBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue), 1);
        //        gvwConceptos.DataBind();

        //        if (gvwConceptos.Rows.Count > 0)
        //            btnLiquidarTodosHotel.Visible = true;

        //        CargarPropietarios();

        //        btnDetalleConceptoHotel_Click(null, null);
        //    }

        //    protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        //    {
        //        ConceptoBo conceptoBoTmp = new ConceptoBo();
        //        DateTime fecha;

        //        if (txtFecha.Text != string.Empty)
        //            fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
        //        else
        //            fecha = new DateTime(1753, 1, 1, 12, 00, 00, 00);

        //        int idHotel = int.Parse(ddlHotel.SelectedValue);

        //        gvwDetalleConceptoHotel.DataSource = conceptoBoTmp.ListaValorLiquidacionConceptos(fecha, idHotel, true);
        //        gvwDetalleConceptoHotel.DataBind();
        //    }

        //    protected void gvwPropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //    {
        //        //gvwPropietarios.PageIndex = e.NewPageIndex;
        //        //CargarPropietarios();
        //    }

        //    protected void gvwPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        //    {
        //        //if (e.Row.RowType == DataControlRowType.DataRow)
        //        //{
        //        //    int idSuite = int.Parse(gvwPropietarios.DataKeys[e.Row.RowIndex]["IdSuit"].ToString());
        //        //    ImageButton imgBtnTmp = (ImageButton)e.Row.FindControl("imgBtnSeleccion");

        //        //    List<int> listaIdSuite = (List<int>)Session["listaIdSuite"];

        //        //    if (listaIdSuite.Count > 0)
        //        //    {
        //        //        if (listaIdSuite.Where(I => I == idSuite).Count() > 0)
        //        //        {
        //        //            imgBtnTmp.ImageUrl = "~/img/95.png";
        //        //            listaIdSuite.Add(int.Parse(imgBtnTmp.CommandArgument));
        //        //        }
        //        //        else
        //        //        {
        //        //            imgBtnTmp.ImageUrl = "~/img/117.png";
        //        //            listaIdSuite.Remove(int.Parse(imgBtnTmp.CommandArgument));
        //        //        }
        //        //    }
        //        //}
        //    }

        //    protected void gvwPropietariosBuscar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //    {
        //        gvwPropietariosBuscar.PageIndex = e.NewPageIndex;
        //        btnBuscar_Click(null, null);
        //    }

        //    protected void gvwPropietariosBuscar_RowDataBound(object sender, GridViewRowEventArgs e)
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            int idSuite = int.Parse(gvwPropietariosBuscar.DataKeys[e.Row.RowIndex]["IdSuit"].ToString());
        //            ImageButton imgBtnTmp = (ImageButton)e.Row.FindControl("imgBtnSeleccion");

        //            List<int> listaIdSuite = (List<int>)Session["listaIdSuite"];

        //            if (listaIdSuite.Count > 0)
        //            {
        //                if (listaIdSuite.Where(I => I == idSuite).Count() > 0)
        //                {
        //                    imgBtnTmp.ImageUrl = "~/img/95.png";
        //                    listaIdSuite.Add(int.Parse(imgBtnTmp.CommandArgument));
        //                }
        //                else
        //                {
        //                    imgBtnTmp.ImageUrl = "~/img/117.png";
        //                    listaIdSuite.Remove(int.Parse(imgBtnTmp.CommandArgument));
        //                }
        //            }
        //        }
        //    }

        //    void uc_WebUserBuscadorPropietarioSuite_AlAceptar(object sender, EventArgs e)
        //    {
        //        //DateTime fechaPeriodo = new DateTime(int.Parse(this.txtFechaReporte.Text), int.Parse(ddlMesReporte.SelectedValue), 1, 00, 00, 00);
        //        //int idHotel = int.Parse(ddlHotelReporte.SelectedValue);
        //        //int idPropietario = uc_WebUserBuscadorPropietarioSuite.IdPropietarioBuscado;
        //        //int idSuite = uc_WebUserBuscadorPropietarioSuite.IdSuiteBuscado;

        //        //string nombreTitulo = "LIQUIDACION - " + ddlHotelReporte.SelectedItem.Text + " N° SUITE " + uc_WebUserBuscadorPropietarioSuite.NumSuite;

        //        //Reportes.XtraReport_LiquidacionPropietario reporteTmp = new WebOwner.Reportes.XtraReport_LiquidacionPropietario(fechaPeriodo, nombreTitulo, idHotel, idPropietario, idSuite);
        //        //ReportViewer_Liquidacion.Report = reporteTmp;

        //        //this.EsReporteLiquidacionHotel = false;
        //        //this.EsReporteLiquidacionPropietario = true;
        //    }       

        //    #endregion        

        //    #region Boton

        //    protected void btnEliminarLiqui_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            cierreBo = new CierreBo();

        //            int idHotel = int.Parse(ddlHotelEliminarLiquidacion.SelectedValue);
        //            string nombreHotel = ddlHotelEliminarLiquidacion.SelectedItem.Text;
        //            DateTime fecha = new DateTime(int.Parse(this.txtFechaDesde.Text), int.Parse(ddlMesDesde.SelectedValue), 1, 00, 00, 00);

        //            if (!cierreBo.ValidarCierre(fecha, idHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_22;
        //                return;
        //            }

        //            liquidacionBo = new LiquidacionBo();
        //            liquidacionBo.EliminarLiquidacion(fecha, idHotel, nombreHotel, ((ObjetoGenerico)Session["usuarioLogin"]).Id);

        //            this.divExito.Visible = true;
        //            this.divError.Visible = false;
        //            this.lbltextoExito.Text = "La liquidación ha sido eliminada con éxito.";
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }
        //    }

        //    protected void btnPreLiquidacionHotel_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            gvwPreLiquidacionHotel.Visible = false;

        //            int idHotel = int.Parse(ddlHotel.SelectedValue);
        //            DateTime fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

        //            #region Validar si la liquidacion esta cerrada
        //            CierreBo cierreBoTmp = new CierreBo();
        //            if (!cierreBoTmp.ValidarCierre(fecha, idHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_19;
        //                return;
        //            }
        //            #endregion

        //            if (this.EsCorrectoVariables("H", idHotel, fecha))
        //            {
        //                Button btnTmp = (Button)sender;
        //                int idConcepto = int.Parse(btnTmp.CommandArgument);

        //                LiquidacionBo liquidacionBoTmp = new LiquidacionBo(fecha, idHotel, 1, -1);
        //                liquidacionBoTmp.IdConcepto = idConcepto;
        //                List<ObjetoGenerico> listaRespuesta = liquidacionBoTmp.LiquidarConceptoHotel();

        //                Session.Remove("listaAuditoria");
        //                Session.Remove("listaRespuesta");

        //                Session["listaAuditoria"] = liquidacionBoTmp.ListaAuditoria;
        //                Session["listaRespuesta"] = listaRespuesta;

        //                if (listaRespuesta.Count > 0)
        //                {
        //                    gvwPreLiquidacionHotel.Visible = true;
        //                    btnGuardarTodoHotel.Visible = true;
        //                    gvwPreLiquidacionHotel.DataSource = listaRespuesta;
        //                    gvwPreLiquidacionHotel.DataBind();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }            
        //    }

        //    /// <summary>
        //    /// Si el usuario valida que todo esta correcto, da en aceptar para guardar la liquidación
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    protected void btnAceptarLiquidacionHotel_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            DateTime fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
        //            Button btnTmp = (Button)sender;
        //            int idConcepto = int.Parse(btnTmp.CommandArgument);
        //            int idHotel = int.Parse(ddlHotel.SelectedValue);

        //            #region Validar si la liquidacion esta cerrada
        //            CierreBo cierreBoTmp = new CierreBo();
        //            if (!cierreBoTmp.ValidarCierre(fecha, idHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_19;
        //                return;
        //            }
        //            #endregion                              

        //            List<ObjetoGenerico> listaAuditoria = (List<ObjetoGenerico>)Session["listaAuditoria"];
        //            List<ObjetoGenerico> listaRespuesta = (List<ObjetoGenerico>)Session["listaRespuesta"];

        //            LiquidacionBo liquidacionBoTmp = new LiquidacionBo();
        //            liquidacionBoTmp.AceptarLiquidacionHotel(listaRespuesta, listaAuditoria, fecha,
        //                                                    ((ObjetoGenerico)Session["usuarioLogin"]).Id, idConcepto, fecha, ddlHotel.SelectedItem.Text);

        //            btnDetalleConceptoHotel_Click(null, null);

        //            this.divExito.Visible = true;
        //            this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
        //            this.divError.Visible = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }            
        //    }        

        //    /// <summary>
        //    /// Liquida todos los concpetos de propietario
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    protected void btnLiquidarTodos_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            this.esLiquidacionTotal = true;
        //            this.IdHotel = int.Parse(ddlHotel.SelectedValue);
        //            DateTime fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

        //            #region Para liquidar, primero deben haber liquidado los concepto de hote
        //            if (!this.EsValidoLiquidarPropietario(fecha, this.IdHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_20;
        //                return;
        //            }
        //            #endregion

        //            #region Validar si la liquidacion esta cerrada
        //            CierreBo cierreBoTmp = new CierreBo();
        //            if (!cierreBoTmp.ValidarCierre(fecha, this.IdHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_19;
        //                return;
        //            }
        //            #endregion

        //            LiquidacionBo liquidacionBoTmp = new LiquidacionBo(fecha, this.IdHotel, 2, -1);
        //            liquidacionBoTmp.LiquidarConceptoPropietario(string.Empty);

        //            if (liquidacionBoTmp.ListaErrores.Length > 0)
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                liquidacionBoTmp.ListaErrores.AppendLine("Errores de liquidación.");
        //                this.lbltextoError.Text = liquidacionBoTmp.ListaErrores.ToString();
        //                return;
        //            }

        //            int numColumnas = liquidacionBoTmp.NumReglasTotal;

        //            List<ObjetoGenerico> ListaResultadosConceptosFinal = liquidacionBoTmp.ListaResultadosConceptosFinal;
        //            Session["listaResultadosConceptosFinalPropietario"] = ListaResultadosConceptosFinal;

        //            this.MostrarPreLiquidarPropietario(numColumnas, ListaResultadosConceptosFinal, liquidacionBoTmp.ListaNombresConcepto);
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }

        //    }

        //    /// <summary>
        //    /// liquida solo los propietarios seleccionados
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    protected void btnLiquidarSeleccionados_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            this.esLiquidacionTotal = false;
        //            this.IdHotel = int.Parse(ddlHotel.SelectedValue);
        //            DateTime fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

        //            if (!this.EsValidoLiquidarPropietario(fecha, this.IdHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_20;
        //                return;
        //            }

        //            #region Validar si la liquidacion esta cerrada
        //            CierreBo cierreBoTmp = new CierreBo();
        //            if (!cierreBoTmp.ValidarCierre(fecha, this.IdHotel))
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                this.lbltextoError.Text = Resources.Resource.lblMensajeError_19;
        //                return;
        //            }
        //            #endregion

        //            List<int> listaIdSuite = (List<int>)Session["listaIdSuite"];
        //            string resultId = string.Join(",", listaIdSuite.Select(x => x.ToString()).ToArray());

        //            LiquidacionBo liquidacionBoTmp = new LiquidacionBo(fecha, this.IdHotel, 2, -1);
        //            liquidacionBoTmp.LiquidarConceptoPropietario(resultId);

        //            if (liquidacionBoTmp.ListaErrores.Length > 0)
        //            {
        //                this.divExito.Visible = false;
        //                this.divError.Visible = true;
        //                liquidacionBoTmp.ListaErrores.AppendLine("Errores de liquidación.");
        //                this.lbltextoError.Text = liquidacionBoTmp.ListaErrores.ToString();
        //                return;
        //            }

        //            int numColumnas = liquidacionBoTmp.NumReglasTotal;
        //            List<ObjetoGenerico> ListaResultadosConceptosFinal = liquidacionBoTmp.ListaResultadosConceptosFinal;
        //            Session["listaResultadosConceptosFinalPropietario"] = ListaResultadosConceptosFinal;

        //            this.MostrarPreLiquidarPropietario(numColumnas, ListaResultadosConceptosFinal, liquidacionBoTmp.ListaNombresConcepto);
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }
        //    }

        //    /// <summary>
        //    /// muestra la liquidacion ya echa de hotel, para uso informativo
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    protected void btnDetalleConceptoHotel_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            //btnAceptarLiquidacionpropietario_uno.Visible = false;
        //            //btnAceptarLiquidacionpropietario_dos.Visible = false;

        //            ConceptoBo conceptoBoTmp = new ConceptoBo();
        //            DateTime fecha;

        //            if (txtFecha.Text != string.Empty)
        //                fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
        //            else
        //                fecha = new DateTime(1753, 1, 1, 12, 00, 00, 00);

        //            int idHotel = int.Parse(ddlHotel.SelectedValue);

        //            gvwDetalleConceptoHotel.DataSource = conceptoBoTmp.ListaValorLiquidacionConceptos(fecha, idHotel, true);
        //            gvwDetalleConceptoHotel.DataBind();
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }            
        //    }        

        //    protected void gvwPreLiquidacionHotel_SelectedIndexChanged(object sender, EventArgs e)
        //    {

        //    }

        //    /// <summary>
        //    /// Guarda la liquidacion de propietarios
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    protected void btnAceptarLiquidacionpropietario_uno_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            DateTime fecha = new DateTime(int.Parse(this.txtFecha.Text), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);

        //            //btnAceptarLiquidacionpropietario_uno.Visible = false;
        //            //btnAceptarLiquidacionpropietario_dos.Visible = false;

        //            List<ObjetoGenerico> listaRespuesta = (List<ObjetoGenerico>)Session["listaResultadosConceptosFinalPropietario"];

        //            LiquidacionBo liquidacionBoTmp = new LiquidacionBo();
        //            liquidacionBoTmp.AceptarLiquidacionPropietario(listaRespuesta, ((ObjetoGenerico)Session["usuarioLogin"]).Id, fecha, this.esLiquidacionTotal, this.IdHotel);

        //            this.divExito.Visible = true;
        //            this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
        //            this.divError.Visible = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.Log(ex);

        //            this.divExito.Visible = false;
        //            this.divError.Visible = true;
        //            this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //        }
        //    }

        //    /// <summary>
        //    /// Busca un propietario segun el filtro de busqueda
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    protected void btnBuscar_Click(object sender, EventArgs e)
        //    {
        //        PropietarioBo propietarioBoTmp = new PropietarioBo();
        //        gvwPropietariosBuscar.DataSource = propietarioBoTmp.ObtenerPropietariosByFiltro(int.Parse(ddlHotel.SelectedValue), ddlFiltro.SelectedValue, txtBusqueda.Text);
        //        gvwPropietariosBuscar.DataBind();
        //    }       

        //    protected void btnAceptar_Click(object sender, EventArgs e)
        //    {
        //        CargarPropietarios();
        //    }        

        //    protected void btnCancelarBuscador_Click(object sender, EventArgs e)
        //    {
        //        List<int> listaIdSuiteBuscar = new List<int>();
        //        Session["listaIdSuiteBuscar"] = listaIdSuiteBuscar;
        //    }

        //    protected void btnAceptarBuscador_Click(object sender, EventArgs e)
        //    {
        //        //List<int> listaIdSuite = (List<int>)Session["listaIdSuite"];
        //        //List<int> listaIdSuiteBuscar = (List<int>)Session["listaIdSuiteBuscar"];

        //        //listaIdSuite.AddRange(listaIdSuiteBuscar);
        //        CargarPropietarios();
        //    }

        //    #endregion
    }
}