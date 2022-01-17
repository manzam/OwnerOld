using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BO;
using DM;
using Servicios;
using System.Collections.Specialized;
using System.CodeDom.Compiler;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserReglasLiquidacion : System.Web.UI.UserControl
    {
        InformacionEstadisticaBo infoEstadisticaBo;
        ConceptoBo conceptoBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            misVariables.Controls.Clear();

            this.divExito.Visible = false;
            this.divError.Visible = false;

            if (!IsPostBack)
            {
                this.CargarCombo();
                this.IdReglaSeleccionado = -1;
                this.UsuarioLogin = (ObjetoGenerico)Session["usuarioLogin"];
                ddlHotel_SelectedIndexChanged(null, null);


            }

            this.CargarOperadores();
            this.CargarVariables();
            this.CargarConceptos();
            this.CargarVariablesHotel();
            this.CargarConstantes();

            CargarReporte();
        }

        #region Propiedades
        public List<ObjetoGenerico> ListaVariable
        {
            get { return (List<ObjetoGenerico>)ViewState["ListaVariable"]; }
            set { ViewState["ListaVariable"] = value; }
        }

        public int IdReglaSeleccionado
        {
            get { return (int)ViewState["IdReglaSeleccionado"]; }
            set { ViewState["IdReglaSeleccionado"] = value; }
        }

        public string NombreSeleccionado
        {
            get { return (string)ViewState["NombreSeleccionado"]; }
            set { ViewState["NombreSeleccionado"] = value; }
        }

        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)ViewState["UsuarioLogin"]; }
            set { ViewState["UsuarioLogin"] = value; }
        }

        #endregion

        #region Metodos

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

            CuentaContableBo cuentaContableBo = new CuentaContableBo();
            List<ObjetoGenerico> listaCuentaContable = cuentaContableBo.VerTodos();
            ObjetoGenerico cuentaSeleccione = new ObjetoGenerico();
            cuentaSeleccione.IdCuentaContable = -1;
            cuentaSeleccione.Nombre = GetGlobalResourceObject("Resource", "lblSeleccione").ToString();
            listaCuentaContable.Insert(0, cuentaSeleccione);

            ddlCuentaContable.DataSource = listaCuentaContable;
            ddlCuentaContable.DataTextField = "Nombre";
            ddlCuentaContable.DataValueField = "IdCuentaContable";
            ddlCuentaContable.DataBind();

            ddlCuentaContable2.DataSource = listaCuentaContable;
            ddlCuentaContable2.DataTextField = "Nombre";
            ddlCuentaContable2.DataValueField = "IdCuentaContable";
            ddlCuentaContable2.DataBind();
        }

        private void CargarGrilla()
        {
            if (ddlHotel.Items.Count == 0)
                return;

            ConceptoBo conceptoBoTmp = new ConceptoBo();
            List<Concepto> listaConceptoTmp = conceptoBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue));
            List<ObjetoGenerico> listaConcepto = new List<ObjetoGenerico>();

            foreach (Concepto itemConcepto in listaConceptoTmp)
            {
                ObjetoGenerico concepto = new ObjetoGenerico();
                concepto.IdConcepto = itemConcepto.IdConcepto;
                concepto.Nombre = itemConcepto.Nombre;
                concepto.Orden = itemConcepto.Orden;
                concepto.Activo = itemConcepto.EsActiva.Value;
                concepto.Regla = HttpUtility.HtmlDecode(conceptoBoTmp.ObtenerRegla2(itemConcepto.IdConcepto));
                listaConcepto.Add(concepto);
            }

            gvwReglas.DataSource = listaConcepto;
            gvwReglas.DataBind();
        }

        private void CargarOperadores()
        {
            string[] operadores = new string[] { "+", "-", "*", "/", "(", ")", ">", "<", " case ", " when ", " else ", " end ", " then ", " and ", "=" }; //Ʃ &Sigma;
            HtmlGenericControl controlTmp;
            controlTmp = new HtmlGenericControl();
            controlTmp.TagName = "h3";
            controlTmp.InnerText = "Operadores";
            misVariables.Controls.Add(controlTmp);

            HtmlGenericControl controlTmpDiv = new HtmlGenericControl();
            controlTmpDiv.TagName = "div";
            for (int i = 0; i < operadores.Length; i++)
            {
                controlTmp = new HtmlGenericControl();
                controlTmp.ID = "miVar" + i;
                controlTmp.TagName = "li";
                controlTmp.Attributes.Add("class", "op");
                controlTmp.Attributes.Add("onclick", "copiarAlPortapapeles('" + controlTmp.ClientID + "')");
                controlTmp.InnerText = operadores[i];
                controlTmpDiv.Controls.Add(controlTmp);
            }
            misVariables.Controls.Add(controlTmpDiv);
        }

        private void CargarVariables()
        {
            try
            {
                VariableBo variableBoTmp = new VariableBo();
                this.ListaVariable = variableBoTmp.VerTodos("P", int.Parse(ddlHotel.SelectedValue));

                if (this.ListaVariable.Count > 0)
                {
                    HtmlGenericControl controlTmp = null;
                    controlTmp = new HtmlGenericControl();
                    controlTmp.ID = "divVarV";
                    controlTmp.TagName = "h3";
                    controlTmp.InnerText = Resources.Resource.lblVariablesPropietarios;
                    misVariables.Controls.Add(controlTmp);

                    HtmlGenericControl controlTmpDiv = new HtmlGenericControl();
                    controlTmpDiv.TagName = "div";
                    List<string> lstvarV = new List<string>();
                    foreach (ObjetoGenerico varTmp in this.ListaVariable)
                    {
                        controlTmp = new HtmlGenericControl();
                        controlTmp.TagName = "li";
                        controlTmp.InnerText = varTmp.Nombre;
                        controlTmp.ID = "varV_" + varTmp.IdVariable.ToString();
                        controlTmp.EnableViewState = true;
                        controlTmp.Attributes.Add("title", varTmp.Descripcion);
                        controlTmp.Attributes.Add("class", "var");
                        controlTmp.Attributes.Add("onclick", "copiarAlPortapapeles('" + controlTmp.ClientID + "')");
                        controlTmpDiv.Controls.Add(controlTmp);
                    }
                    misVariables.Controls.Add(controlTmpDiv);
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void CargarVariablesHotel()
        {
            try
            {
                VariableBo variableBoTmp = new VariableBo();
                List<ObjetoGenerico> listaVariablesSuit = variableBoTmp.VerTodos("H", int.Parse(ddlHotel.SelectedValue));

                if (listaVariablesSuit.Count > 0)
                {
                    HtmlGenericControl controlTmp = null;

                    controlTmp = new HtmlGenericControl();
                    controlTmp.TagName = "h3";
                    controlTmp.InnerText = Resources.Resource.lblVariablesHotel;
                    misVariables.Controls.Add(controlTmp);

                    HtmlGenericControl controlTmpDiv = new HtmlGenericControl();
                    controlTmpDiv.TagName = "div";
                    foreach (ObjetoGenerico varTmp in listaVariablesSuit)
                    {
                        controlTmp = new HtmlGenericControl();
                        controlTmp.TagName = "li";
                        controlTmp.InnerText = varTmp.Nombre;
                        controlTmp.ID = "varH_" + varTmp.IdVariable.ToString();
                        controlTmp.EnableViewState = true;
                        controlTmp.Attributes.Add("class", "var");
                        controlTmp.Attributes.Add("onclick", "copiarAlPortapapeles('" + controlTmp.ClientID + "')");
                        controlTmpDiv.Controls.Add(controlTmp);
                    }
                    misVariables.Controls.Add(controlTmpDiv);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CargarConceptos()
        {
            try
            {
                ConceptoBo conceptoBoTmp = new ConceptoBo();
                List<Concepto> listaConcepto = conceptoBoTmp.VerTodos(int.Parse(ddlHotel.SelectedValue));

                if (this.IdReglaSeleccionado != -1)
                {
                    listaConcepto.RemoveAll(C => C.IdConcepto == this.IdReglaSeleccionado);
                }

                if (listaConcepto.Count > 0)
                {
                    HtmlGenericControl controlTmp = null;

                    controlTmp = new HtmlGenericControl();
                    controlTmp.TagName = "h3";
                    controlTmp.InnerText = Resources.Resource.lblVariables + " " + Resources.Resource.lblConcepto;
                    misVariables.Controls.Add(controlTmp);

                    HtmlGenericControl controlTmpDiv = new HtmlGenericControl();
                    controlTmpDiv.TagName = "div";
                    foreach (Concepto varTmp in listaConcepto)
                    {
                        controlTmp = new HtmlGenericControl();
                        controlTmp.TagName = "li";
                        controlTmp.InnerText = varTmp.Nombre;
                        controlTmp.ID = "varC_" + varTmp.IdConcepto;
                        controlTmp.EnableViewState = true;
                        controlTmp.Attributes.Add("class", "var");
                        controlTmp.Attributes.Add("onclick", "copiarAlPortapapeles('" + controlTmp.ClientID + "')");
                        controlTmpDiv.Controls.Add(controlTmp);
                    }
                    misVariables.Controls.Add(controlTmpDiv);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CargarConstantes()
        {
            try
            {
                VariableBo variableBoTmp = new VariableBo();
                List<ObjetoGenerico> listaVariablesSuit = variableBoTmp.VerTodos("C", int.Parse(ddlHotel.SelectedValue));

                if (listaVariablesSuit.Count > 0)
                {
                    HtmlGenericControl controlTmp = null;

                    controlTmp = new HtmlGenericControl();
                    controlTmp.TagName = "h3";
                    controlTmp.InnerText = "Constantes";
                    misVariables.Controls.Add(controlTmp);

                    HtmlGenericControl controlTmpDiv = new HtmlGenericControl();
                    controlTmpDiv.TagName = "div";
                    foreach (ObjetoGenerico varTmp in listaVariablesSuit)
                    {
                        controlTmp = new HtmlGenericControl();
                        controlTmp.TagName = "li";
                        controlTmp.InnerText = varTmp.Nombre;
                        controlTmp.ID = "varCO_" + varTmp.IdVariable.ToString();
                        controlTmp.EnableViewState = true;
                        controlTmp.Attributes.Add("class", "var");
                        controlTmp.Attributes.Add("onclick", "copiarAlPortapapeles('" + controlTmp.ClientID + "')");
                        controlTmpDiv.Controls.Add(controlTmp);
                    }
                    misVariables.Controls.Add(controlTmpDiv);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ReglaMala()
        {
            lbltextoError.Text = Resources.Resource.lblMensajeError_3;

            divError.Visible = true;
            divExito.Visible = false;
            divResultado.Visible = false;
        }

        private void LimpiarFormulario()
        {
            txtNombreConcepto.Text = string.Empty;
            txtRegla.Text = string.Empty;
            txtVariable.Text = string.Empty;
            txtCodigoTercero.Text = string.Empty;
            cbMostrarExtracto.Checked = false;
            cbSegundaCuenta.Checked = false;
            txtNumDecimales.Text = "2";
            txtOrden.Text = string.Empty;
            txtValorCondicion.Text = "0";
            ddlCuentaContable.SelectedIndex = -1;
            ddlVariableCondicion.Enabled = false;
            ddlCondicion.Enabled = false;
            txtValorCondicion.Enabled = false;
            ddlCuentaContable2.Enabled = false;
            cbEsRetencionAplicar.Checked = false;
            txtReglaUsuario.InnerText = string.Empty;
        }

        private void CargarRegla()
        {
            ConceptoBo conceptoBoTmp = new ConceptoBo();
            Concepto conceptoTmp = conceptoBoTmp.Obtener(this.IdReglaSeleccionado);
            txtNombreConcepto.Text = conceptoTmp.Nombre;
            txtNumDecimales.Text = conceptoTmp.NumDecimales.ToString();
            txtCodigoTercero.Text = conceptoTmp.CodigoTercero;
            txtOrden.Text = conceptoTmp.Orden.ToString();
            cbMostrarExtracto.Checked = conceptoTmp.EsMuestraExtracto;
            ddlHotel.SelectedValue = conceptoTmp.Hotel.IdHotel.ToString();
            ddlNivelConcepto.SelectedValue = conceptoTmp.NivelConcepto.ToString();
            cbMostrarEnLiquidacion.Checked = conceptoTmp.EsMuestraReporteLiquidacion;
            cbEsRetencionAplicar.Checked = conceptoTmp.EsRetencionAplicar;

            if (conceptoTmp.Cuenta_Contable != null)
                ddlCuentaContable.SelectedValue = conceptoTmp.Cuenta_Contable.IdCuentaContable.ToString();
            else
                ddlCuentaContable.SelectedValue = "-1";

            if (conceptoTmp.Informacion_Estadistica != null)
                ddlVariableEstadistica.SelectedValue = conceptoTmp.Informacion_Estadistica.IdInformacionEstadistica.ToString();
            else
                ddlVariableEstadistica.SelectedValue = "-1";

            cbSegundaCuenta.Checked = conceptoTmp.EsConSegundaCuentaContable;

            if (conceptoTmp.EsConSegundaCuentaContable)
            {
                if (conceptoTmp.Cuenta_Contable1 != null)
                    ddlCuentaContable2.SelectedValue = conceptoTmp.Cuenta_Contable1.IdCuentaContable.ToString();
                else
                    ddlCuentaContable2.SelectedValue = "-1";

                ddlVariableCondicion.SelectedValue = conceptoTmp.IdVariableCondicion.ToString();
                ddlCondicion.SelectedValue = conceptoTmp.Condicion;
                txtValorCondicion.Text = conceptoTmp.ValorCondicion.ToString();
            }
            else
            {
                ddlCuentaContable2.Enabled = false;
                ddlVariableCondicion.Enabled = false;
                ddlCondicion.Enabled = false;
                txtValorCondicion.Enabled = false;
            }

            CargarRegla(this.IdReglaSeleccionado);

            this.CargarOperadores();
            this.CargarVariables();
            this.CargarConceptos();
            this.CargarVariablesHotel();
            this.CargarConstantes();
        }

        private void CargarRegla(int idConcepto)
        {
            VariablesConceptoBo variableConceptoBoTmp = new VariablesConceptoBo();
            List<ObjetoGenerico> listaVariables = variableConceptoBoTmp.Obtener(idConcepto);

            string regla = "";
            string reglaUsuario = "";
            string variableComas = "";
            string variable = "";
            short k = 1;

            foreach (ObjetoGenerico item in listaVariables)
            {
                HtmlGenericControl controlTmp = new HtmlGenericControl();
                controlTmp.TagName = "span";
                controlTmp.EnableViewState = true;

                if (item.Operador != string.Empty)
                {
                    controlTmp.Attributes.Add("class", "op");
                    controlTmp.InnerText = HttpUtility.HtmlDecode(item.Operador);
                    controlTmp.ID = "operador_" + k;
                    if (item.Operador == "&Sigma;")
                    {
                        regla += "Ʃ";
                        variable += "Ʃ,";
                    }
                    else
                    {
                        regla += item.Operador;
                        reglaUsuario += item.Operador + " ";
                        variable += item.Operador + ",";
                    }
                }
                else
                {
                    switch (item.NomClass)
                    {
                        case "varV":
                            controlTmp.Attributes.Add("class", "var");
                            controlTmp.InnerText = item.NombreVariable;
                            controlTmp.ID = k.ToString();
                            regla += item.NombreVariable;
                            reglaUsuario += item.NombreVariable + " ";
                            variableComas += item.NombreVariable + ",";
                            variable += (item.NomClass + "_" + item.IdVariable) + ",";
                            break;

                        case "varCO":
                            controlTmp.Attributes.Add("class", "var");
                            controlTmp.InnerText = item.NombreVariable;
                            controlTmp.ID = k.ToString();
                            regla += item.NombreVariable;
                            reglaUsuario += item.NombreVariable + " ";
                            variableComas += item.NombreVariable + ",";
                            variable += (item.NomClass + "_" + item.IdVariable) + ",";
                            break;

                        case "varH":
                            controlTmp.Attributes.Add("class", "var");
                            controlTmp.InnerText = item.NombreVariable;
                            controlTmp.ID = k.ToString();
                            regla += item.NombreVariable;
                            reglaUsuario += item.NombreVariable + " ";
                            variableComas += item.NombreVariable + ",";
                            variable += (item.NomClass + "_" + item.IdVariable) + ",";
                            break;

                        case "varC":
                            controlTmp.Attributes.Add("class", "var");
                            controlTmp.InnerText = item.NombreConcepto;
                            controlTmp.ID = k.ToString();
                            regla += item.NombreConcepto;
                            reglaUsuario += item.NombreConcepto + " ";
                            variableComas += item.NombreConcepto + ",";
                            variable += (item.NomClass + "_" + item.IdConcepto) + ",";
                            break;

                        default:
                            break;
                    }
                }
                reglaCrear.Controls.Add(controlTmp);
                k++;
            }

            misVariables.Controls.Clear();
            txtRegla.Text = regla;
            txtReglaUsuario.InnerText = reglaUsuario;

            if (regla.Trim() != string.Empty)
            {
                txtVariableComas.Text = variableComas.Remove(variableComas.Length - 1);
                txtVariable.Text = variable.Remove(variable.Length - 1);
            }
        }

        private void CargarReporte()
        {
            if (ddlHotelReporte.Items.Count > 0)
            {
                Reportes.XtraReport_Reglas reporteTmp = new WebOwner.Reportes.XtraReport_Reglas(int.Parse(ddlHotelReporte.SelectedValue), ddlHotelReporte.SelectedItem.Text);
                ReportViewer_Reglas.Report = reporteTmp;
            }
        }

        #endregion

        #region Boton

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IdReglaSeleccionado = -1; //limpiamos el id por precaucion.
            LimpiarFormulario();

            this.btnNuevo.Visible = false;
            this.btnActualizar.Visible = false;
            this.GrillaRegla.Visible = false;
            this.divResultado.Visible = false;
            this.divError.Visible = false;
            this.divExito.Visible = false;

            this.btnGuardar.Visible = true;
            this.NuevoRegla.Visible = true;

            misVariables.Controls.Clear();

            this.CargarOperadores();
            this.CargarVariables();
            this.CargarConceptos();
            this.CargarVariablesHotel();
            this.CargarConstantes();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                conceptoBoTmp = new ConceptoBo();
                if (conceptoBoTmp.EsVariableRepetida(txtNombreConcepto.Text, int.Parse(ddlHotel.SelectedValue), -1))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Ya existe otra variable, con el mismo nombre.";
                    return;
                }

                ValidarExpresion mEval = new ValidarExpresion();
                List<string> variablesTmp = txtVariableComas.Text.Split(',').Distinct().ToList();

                if (!(txtRegla.Text.Contains("then") || txtRegla.Text.Contains("when") || txtRegla.Text.Contains("else") || txtRegla.Text.Contains("end") || txtRegla.Text.Contains("case")))
                {
                    if (!mEval.ValidaExpression(txtRegla.Text, variablesTmp))
                    {
                        ReglaMala();
                        return;
                    }
                }

                int orden = 0;
                if (txtOrden.Text != string.Empty)
                    orden = int.Parse(txtOrden.Text);

                conceptoBoTmp.Guardar(txtNombreConcepto.Text.ToUpper(), int.Parse(ddlHotel.SelectedValue),
                                      int.Parse(ddlNivelConcepto.SelectedValue), txtVariable.Text, int.Parse(ddlCuentaContable.SelectedValue),
                                      txtNombreConcepto.Text.ToUpper(), cbMostrarExtracto.Checked, int.Parse(txtNumDecimales.Text),
                                      txtCodigoTercero.Text, orden, int.Parse(ddlVariableEstadistica.SelectedValue), this.UsuarioLogin.Id,
                                      txtRegla.Text, ddlHotel.SelectedItem.Text, ddlCuentaContable.SelectedItem.Text, ddlVariableEstadistica.SelectedItem.Text,
                                      cbSegundaCuenta.Checked, int.Parse(ddlCuentaContable2.SelectedValue), int.Parse(ddlVariableCondicion.SelectedValue), ddlCondicion.SelectedValue,
                                      ((txtValorCondicion.Text.Trim() == string.Empty) ? 0 : double.Parse(txtValorCondicion.Text)),
                                      ((ddlVariableCondicion.SelectedItem.Text.Split('-')[1].Trim() == "Variable Hotel") ? "H" : "P"), cbMostrarEnLiquidacion.Checked, cbEsRetencionAplicar.Checked);

                btnVerTodos_Click(null, null);

                lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                divExito.Visible = true;
                divError.Visible = false;

            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                conceptoBoTmp = new ConceptoBo();
                if (conceptoBoTmp.EsVariableRepetida(txtNombreConcepto.Text, int.Parse(ddlHotel.SelectedValue), this.IdReglaSeleccionado))
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = "Ya existe otra variable, con el mismo nombre.";
                    return;
                }

                ValidarExpresion mEval = new ValidarExpresion();
                List<string> variablesTmp = txtVariableComas.Text.Split(',').Distinct().ToList();

                //if (txtRegla.Text.Contains("Ʃ"))
                //{
                //    if (variablesTmp.Count != 1)
                //    {
                //        ReglaMala();
                //        return;
                //    }
                //}
                //else if (!mEval.ValidaExpression(txtRegla.Text, variablesTmp))
                //{
                //    ReglaMala();
                //    return;
                //}
                //else
                //{ }

                if (!(txtRegla.Text.Contains("then") || txtRegla.Text.Contains("when") || txtRegla.Text.Contains("else") || txtRegla.Text.Contains("end") || txtRegla.Text.Contains("case")))
                {
                    if (!mEval.ValidaExpression(txtRegla.Text, variablesTmp))
                    {
                        ReglaMala();
                        return;
                    }
                }

                int orden = 0;
                string reglaOld = conceptoBoTmp.ObtenerRegla(this.IdReglaSeleccionado);

                // Eliminanos todos sus variables.
                VariablesConceptoBo variablesConceptoBoTmp = new VariablesConceptoBo();
                variablesConceptoBoTmp.Eliminar(this.IdReglaSeleccionado);

                if (txtOrden.Text != string.Empty)
                    orden = int.Parse(txtOrden.Text);

                conceptoBoTmp.Actualizar(this.IdReglaSeleccionado, txtNombreConcepto.Text, int.Parse(ddlHotel.SelectedValue), int.Parse(ddlNivelConcepto.SelectedValue),
                                         txtVariable.Text, int.Parse(ddlCuentaContable.SelectedValue), txtNombreConcepto.Text,
                                         cbMostrarExtracto.Checked, int.Parse(txtNumDecimales.Text), txtCodigoTercero.Text, orden,
                                         int.Parse(ddlVariableEstadistica.SelectedValue), this.UsuarioLogin.Id,
                                         txtRegla.Text, reglaOld, ddlHotel.SelectedItem.Text, ddlCuentaContable.SelectedItem.Text, ddlVariableEstadistica.SelectedItem.Text,
                                         cbSegundaCuenta.Checked, int.Parse(ddlCuentaContable2.SelectedValue), int.Parse(ddlVariableCondicion.SelectedValue), ddlCondicion.SelectedValue,
                                         ((txtValorCondicion.Text.Trim() == string.Empty) ? 0 : double.Parse(txtValorCondicion.Text)),
                                         ((ddlVariableCondicion.SelectedItem.Text.Split('-')[1].Trim() == "Variable Hotel") ? "H" : "P"), cbMostrarEnLiquidacion.Checked, cbEsRetencionAplicar.Checked);

                btnVerTodos_Click(null, null);

                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
                divExito.Visible = true;
                divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            CargarGrilla();

            this.btnNuevo.Visible = true;
            this.GrillaRegla.Visible = true;

            this.btnActualizar.Visible = false;
            this.btnGuardar.Visible = false;
            this.NuevoRegla.Visible = false;
            this.divError.Visible = false;
            this.divExito.Visible = false;
            this.divResultado.Visible = false;
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int idConcepto = int.Parse(((ImageButton)sender).CommandArgument);
                conceptoBoTmp = new ConceptoBo();

                if (conceptoBoTmp.EsBorrableRegla(idConcepto))
                {
                    conceptoBoTmp.Eliminar(idConcepto, this.UsuarioLogin.Id);

                    btnVerTodos_Click(null, null);

                    lbltextoExito.Text = Resources.Resource.lblMensajeEliminar;
                    divExito.Visible = true;
                    divError.Visible = false;
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_9;
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
        #endregion

        #region Eventos

        protected void gvwReglas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwReglas.SelectedRow;
            this.IdReglaSeleccionado = int.Parse(gvwReglas.DataKeys[filaSeleccionada.RowIndex]["IdConcepto"].ToString());

            this.CargarRegla();

            this.btnNuevo.Visible = false;
            this.GrillaRegla.Visible = false;
            this.btnGuardar.Visible = false;

            this.btnActualizar.Visible = true;
            this.NuevoRegla.Visible = true;
        }

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            infoEstadisticaBo = new InformacionEstadisticaBo();
            ddlVariableEstadistica.DataSource = infoEstadisticaBo.VerTodosAcumuladas(int.Parse(ddlHotel.SelectedValue));
            ddlVariableEstadistica.DataTextField = "Nombre";
            ddlVariableEstadistica.DataValueField = "IdInformacionEstadistica";
            ddlVariableEstadistica.DataBind();

            ddlVariableEstadistica.Items.Insert(0, new ListItem() { Text = "Seleccione...", Value = "-1", Selected = true });

            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();
            List<ObjetoGenerico> listaVariableCondicion = configExtractoBoTmp.ObtenerListaVariablesSinVariablesConcepto(int.Parse(ddlHotel.SelectedValue));
            ddlVariableCondicion.DataSource = listaVariableCondicion;
            ddlVariableCondicion.DataTextField = "Nombre";
            ddlVariableCondicion.DataValueField = "IdVariable";
            ddlVariableCondicion.DataBind();

            this.CargarGrilla();
        }

        #endregion
    }
}