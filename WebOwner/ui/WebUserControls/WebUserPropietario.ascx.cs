using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM;
using BO;
using Servicios;
using AjaxControlToolkit;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserPropietario : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            uc_WebUserBuscadorPropietario.AlAceptar += new EventHandler(uc_WebUserBuscadorPropietario_AlAceptar);

            if (!IsPostBack)
            {
                this.IdPropietarioSeleccionado = -1;

                CargarCombos();

                ParametroBo parametroBoTmp = new ParametroBo();
                this.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));
                gvwPropietario.PageSize = this.PageSize;

                ddlHotelFiltro_SelectedIndexChanged(null, null);
                ddlTipoPersona_SelectedIndexChanged(null, null);

                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                Session["ListaVariablesSuit"] = listaVariables;
            }

            //CargarReporte();
        }

        #region Propiedades

        public int IdPropietarioSeleccionado
        {
            get { return (int)ViewState["IdPropietarioSeleccionado"]; }
            set { ViewState["IdPropietarioSeleccionado"] = value; }
        }

        public int IdSuitPropietarioSeleccionado
        {
            get { return (int)ViewState["IdSuitPropietarioSeleccionado"]; }
            set { ViewState["IdSuitPropietarioSeleccionado"] = value; }
        }

        public int PageSize
        {
            get { return (int)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }

        public ObjetoGenerico PropietarioTmp
        {
            get { return (ObjetoGenerico)ViewState["Propietario"]; }
            set { ViewState["Propietario"] = value; }
        }

        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }

        public bool EsAdmon
        {
            get
            {
                try
                {
                    return (bool)ViewState["EsAdmon"];
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set { ViewState["EsAdmon"] = value; }
        }

        #endregion

        #region Metodos

        private void CargarReporte()
        {
            //if (ddlHotel.Items.Count > 0)
            //{
            //    Reportes.XtraReport_Propietarios reporte = new WebOwner.Reportes.XtraReport_Propietarios(int.Parse(ddlHotel.SelectedValue), ddlHotel.SelectedItem.Text);
            //    ReportViewer_ReportePropietarios.Report = reporte;
            //}
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtNombreSegundo.Text = "";
            txtApellidoPrimero.Text = "";
            txtApellidoSegundo.Text = "";
            txtNumIdentificacion.Text = "";
            txtCorreo.Text = "";
            txtCorreo2.Text = "";
            txtCorreo3.Text = "";
            txtDireccion.Text = "";
            txtTel1.Text = "";
            txtTel2.Text = "";
            txtNombreContacto.Text = "";
            txtTelContacto.Text = "";
            txtCorreoContacto.Text = "";
            chActivo.Checked = true;
            cbEsRetenedor.Checked = false;

            gvwSuits.DataSource = null;
            gvwSuits.DataBind();
        }

        private void CargarGrilla()
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            int idHotel = int.Parse(ddlHotelFiltro.SelectedValue);

            gvwPropietario.DataSource = propietarioBoTmp.VerTodosPorHotelPropietarioGrilla(idHotel);
            gvwPropietario.DataBind();
        }

        private void CargarCombos()
        {
            DeptoBo deptoBoTmp = new DeptoBo();
            ddlDepto.DataSource = deptoBoTmp.ObtenerTodos();
            ddlDepto.DataTextField = "Nombre";
            ddlDepto.DataValueField = "IdDepartamento";
            ddlDepto.DataBind();

            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
            {
                listaHotel = hotelBoTmp.VerTodos();
                this.EsAdmon = true;
            }
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataSource = listaHotel;
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();

            ddlHotelFiltro.DataSource = listaHotel;
            ddlHotelFiltro.DataTextField = "Nombre";
            ddlHotelFiltro.DataValueField = "IdHotel";
            ddlHotelFiltro.DataBind();

            BancoBo bancoBoTmp = new BancoBo();
            ddlBanco.DataSource = bancoBoTmp.VerTodos();
            ddlBanco.DataTextField = "Nombre";
            ddlBanco.DataValueField = "IdBanco";
            ddlBanco.DataBind();

            ddlBancoDetalleUpdate.DataSource = bancoBoTmp.VerTodos();
            ddlBancoDetalleUpdate.DataTextField = "Nombre";
            ddlBancoDetalleUpdate.DataValueField = "IdBanco";
            ddlBancoDetalleUpdate.DataBind();

            ddlHotel_SelectedIndexChanged(false, null);
            ddlDepto_SelectedIndexChanged(null, null);
        }

        private string CargarGrillaSuits(int idSuitPropietario)
        {
            SuitBo suitBoTmp = new SuitBo();
            UsuarioBo usuarioBoTmp = new UsuarioBo();

            List<int> idHoteles = usuarioBoTmp.ObtenerHotelesPorUsuario(this.UsuarioLogin.Id, this.EsAdmon);

            List<ObjetoGenerico> listaSuit = suitBoTmp.ObtenerSuitsPorPropietario(this.IdPropietarioSeleccionado);
            gvwSuits.DataSource = listaSuit.Where(S => idHoteles.Contains(S.IdHotel)).ToList();
            gvwSuits.DataBind();

            if (listaSuit.Count > 0)
                return listaSuit.Where(S => S.IdSuitPropietario == idSuitPropietario).Select(S => S.IdBanco).FirstOrDefault().ToString();
            else
                return 0.ToString();
        }

        private void CargarGrillaSuitsTmp()
        {
            gvwSuits.DataSource = (List<ObjetoGenerico>)Session["ListaSuit"];
            gvwSuits.DataBind();
        }

        private void CargarDatosPropietario(int idPropietario)
        {
            PropietarioBo propietarioBoTmp = new PropietarioBo();
            this.PropietarioTmp = propietarioBoTmp.ObtenerPropietario(idPropietario);

            HiddenIdPropietario.Value = PropietarioTmp.IdPropietario.ToString();
            hiddenIdUsuario.Value = UsuarioLogin.Id.ToString();
            txtNombre.Text = PropietarioTmp.PrimeroNombre;
            txtCorreo.Text = PropietarioTmp.Correo;
            txtCorreo2.Text = PropietarioTmp.Correo2;
            txtCorreo3.Text = PropietarioTmp.Correo3;
            txtNumIdentificacion.Text = PropietarioTmp.NumIdentificacion;
            txtDireccion.Text = PropietarioTmp.Direccion;
            txtTel1.Text = PropietarioTmp.Telefono1;
            txtTel2.Text = PropietarioTmp.Telefono2;
            ddlDepto.SelectedValue = PropietarioTmp.IdDepto.ToString();
            ddlDepto_SelectedIndexChanged(null, null);
            ddlCiudad.SelectedValue = PropietarioTmp.IdCiudad.ToString();
            chActivo.Checked = PropietarioTmp.Activo;
            cbEsRetenedor.Checked = PropietarioTmp.EsRetenedor;
            txtNombreContacto.Text = PropietarioTmp.NombreContacto;
            txtTelContacto.Text = PropietarioTmp.TelContacto;
            txtCorreoContacto.Text = PropietarioTmp.CorreoContacto;
            ddlTipoDocumento.SelectedValue = this.PropietarioTmp.TipoDocumento;
            ddlTipoPersona.SelectedValue = PropietarioTmp.TipoPersona.ToUpper();
            ddlTipoPersona_SelectedIndexChanged(null, null);
        }

        private void CargarVariablesSuite(int idHotel)
        {
            pnlVariablesValor.Controls.Clear();
            pnlMisVariables.Controls.Clear();

            VariableBo variablesSuiteBoTmp = new VariableBo();
            List<Variable> listaVariables = variablesSuiteBoTmp.VerTodos(idHotel, true, "P");

            int idSuite = -1;
            if (ddlSuit.SelectedValue != "")
                idSuite = int.Parse(ddlSuit.SelectedValue);

            if (listaVariables.Count > 0)
            {
                Table tablaTmp = new Table();
                tablaTmp.Width = new Unit(100, UnitType.Percentage);

                foreach (Variable variableTmp in listaVariables)
                {
                    double valMax = 0;

                    // control
                    TableCell celdaControlTmp = new TableCell();
                    celdaControlTmp.Width = new Unit(33, UnitType.Percentage);

                    TextBox txtControl = new TextBox();
                    txtControl.ID = "varSuit_" + variableTmp.IdVariable;
                    txtControl.Width = new Unit(90, UnitType.Percentage);
                    txtControl.CssClass = "valorVariables";
                    txtControl.Text = (valMax.ToString()).Replace(',', '.');

                    txtControl.Attributes.Add("IdVariable", variableTmp.IdVariable.ToString());
                    txtControl.Attributes.Add("ValMax", variableTmp.MaxNumero.ToString());
                    txtControl.Attributes.Add("EsValidacion", variableTmp.EsConValidacion ? "true" : "false");
                    txtControl.Attributes.Add("NomVariable", variableTmp.Nombre);

                    // etiqueta
                    TableCell celdaEtiquetaTmp = new TableCell();
                    celdaEtiquetaTmp.CssClass = "textoTabla";
                    celdaEtiquetaTmp.Width = new Unit(33, UnitType.Percentage);

                    Label lblEtiqueta = new Label();
                    lblEtiqueta.Text = variableTmp.Nombre;
                    lblEtiqueta.CssClass = "etiquetasListaVariables";
                    lblEtiqueta.ID = "etiqueta_" + variableTmp.IdVariable;

                    FilteredTextBoxExtender ftb = new FilteredTextBoxExtender();
                    ftb.FilterType = FilterTypes.Custom;
                    ftb.ValidChars = "0123456789,.";
                    ftb.FilterMode = FilterModes.ValidChars;
                    ftb.TargetControlID = txtControl.ID;

                    RequiredFieldValidator rfv = new RequiredFieldValidator();
                    rfv.ErrorMessage = "*";
                    rfv.ControlToValidate = "varSuit_" + variableTmp.IdVariable;
                    rfv.Display = ValidatorDisplay.Dynamic;
                    rfv.CssClass = "error";
                    rfv.ValidationGroup = "AceptarSuit";

                    // celda
                    celdaEtiquetaTmp.Controls.Add(lblEtiqueta);
                    celdaControlTmp.Controls.Add(ftb);
                    celdaControlTmp.Controls.Add(txtControl);
                    celdaControlTmp.Controls.Add(rfv);

                    TableCell celdaPropietariosTmp = new TableCell();
                    celdaPropietariosTmp.Width = new Unit(33, UnitType.Percentage);

                    // fila
                    TableRow filaTmp = new TableRow();

                    // Propietarios amarrados
                    if (variableTmp.EsConValidacion)
                    {
                        ValorVariableBo vvsBo = new ValorVariableBo();
                        string propietarios = string.Empty;
                        double sumValue = 0;
                        List<ObjetoGenerico> itmes = vvsBo.GetInforVariablesValidacion(variableTmp.IdVariable, idSuite);

                        foreach (var item in itmes)
                        {
                            propietarios += item.Nombre + " " + item.Apellido + "[ Identificacion: " + item.NumIdentificacion + "  Valor: " + item.Valor + " ]  ";
                            sumValue += item.Valor;
                        }
                        txtControl.Attributes.Add("valor", sumValue.ToString().Replace(",", "."));
                        filaTmp.ToolTip = propietarios;
                    }
                    filaTmp.Width = new Unit(90, UnitType.Percentage);

                    filaTmp.Controls.Add(celdaEtiquetaTmp);
                    filaTmp.Controls.Add(celdaControlTmp);
                    filaTmp.Controls.Add(celdaPropietariosTmp);

                    // tabla
                    tablaTmp.Controls.Add(filaTmp);
                }

                // panel
                pnlMisVariables.Controls.Add(tablaTmp);
            }
        }

        public void CargarVariablesSuite(List<ObjetoGenerico> listaVariables, int idSuite, int idSuitePropietario)
        {
            pnlMisVariables.Controls.Clear();
            pnlVariablesValor.Controls.Clear();

            Table tablaTmp = new Table();
            tablaTmp.Width = new Unit(100, UnitType.Percentage);

            bool EsValorFaltante = false;

            foreach (ObjetoGenerico variableTmp in listaVariables)
            {
                double valMax = 0;

                // control
                TableCell celdaControlTmp = new TableCell();
                celdaControlTmp.Width = new Unit(50, UnitType.Percentage);

                TextBox txtControl = new TextBox();
                txtControl.ID = "varSuitUpdate_" + variableTmp.IdValorVariableSuit;
                txtControl.Attributes.Add("IdValorVariableSuit", variableTmp.IdValorVariableSuit.ToString());
                txtControl.Attributes.Add("IdVariable", "-1");
                txtControl.Attributes.Add("ValMax", variableTmp.ValMax.ToString());
                txtControl.Attributes.Add("EsValidacion", variableTmp.EsValidacion ? "true" : "false");
                txtControl.Attributes.Add("NomVariable", variableTmp.Nombre);
                txtControl.Text = (variableTmp.Valor.ToString()).Replace(',', '.');
                txtControl.CssClass = "valorVariablesUpdate";
                txtControl.Width = new Unit(90, UnitType.Percentage);

                FilteredTextBoxExtender ftb = new FilteredTextBoxExtender();
                ftb.FilterType = FilterTypes.Custom;
                ftb.ValidChars = "0123456789.";
                ftb.FilterMode = FilterModes.ValidChars;
                ftb.TargetControlID = txtControl.ID;

                // etiqueta
                TableCell celdaEtiquetaTmp = new TableCell();
                celdaEtiquetaTmp.CssClass = "textoTabla";
                Label lblEtiqueta = new Label();
                lblEtiqueta.Text = variableTmp.Nombre;
                lblEtiqueta.CssClass = "etiquetasListaVariables";

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ErrorMessage = "*";
                rfv.ControlToValidate = "varSuitUpdate_" + variableTmp.IdValorVariableSuit;
                rfv.Display = ValidatorDisplay.Dynamic;
                rfv.CssClass = "error";
                rfv.ValidationGroup = "ActualizarSuit";

                // celda
                celdaEtiquetaTmp.Controls.Add(lblEtiqueta);
                celdaEtiquetaTmp.Controls.Add(rfv);
                celdaControlTmp.Controls.Add(txtControl);
                celdaControlTmp.Controls.Add(ftb);

                // fila
                TableRow filaTmp = new TableRow();

                // Propietarios amarrados
                if (variableTmp.EsValidacion)
                {
                    ValorVariableBo vvsBo = new ValorVariableBo();
                    string propietarios = string.Empty;
                    double sumValue = 0;
                    List<ObjetoGenerico> itmes = vvsBo.GetInforVariablesValidacion(variableTmp.IdVariableSuite, idSuite);

                    foreach (var item in itmes.Where(S => S.IdSuitPropietario != idSuitePropietario).ToList())
                    {
                        propietarios += item.Nombre + " " + item.Apellido + "[ Identificacion: " + item.NumIdentificacion + "  Valor: " + item.Valor + " ]  ";
                        sumValue += item.Valor;
                    }
                    txtControl.Attributes.Add("valor", sumValue.ToString().Replace(",", "."));
                    filaTmp.ToolTip = propietarios;
                }

                filaTmp.Controls.Add(celdaEtiquetaTmp);
                filaTmp.Controls.Add(celdaControlTmp);
                //filaTmp.Controls.Add(celdaPropietariosTmp);

                // tabla
                tablaTmp.Controls.Add(filaTmp);
            }

            if (EsValorFaltante)
            {
                TableRow filaTmp = new TableRow();
                filaTmp.CssClass = "cuadradoAdvertencia";

                TableCell celdaEtiquetaTmp = new TableCell();
                celdaEtiquetaTmp.ColumnSpan = 100;
                celdaEtiquetaTmp.Text = GetGlobalResourceObject("Resource", "lblMensajeAdvertencia_1").ToString();

                filaTmp.Controls.Add(celdaEtiquetaTmp);
                tablaTmp.Controls.Add(filaTmp);
            }

            // panel
            pnlVariablesValor.Controls.Add(tablaTmp);
        }

        public void CargarVariablesSuite(List<Valor_Variable_Suit> listaVariables)
        {
            pnlMisVariables.Controls.Clear();
            pnlVariablesValor.Controls.Clear();

            List<ObjetoGenerico> listaVariablesTmp = (List<ObjetoGenerico>)Session["ListaVariablesSuit"];

            Table tablaTmp = new Table();
            tablaTmp.Width = new Unit(100, UnitType.Percentage);

            foreach (Valor_Variable_Suit variableTmp in listaVariables)
            {
                // control
                TableCell celdaControlTmp = new TableCell();
                TextBox txtControl = new TextBox();
                //txtControl.ID = "varSuitUpdate_" + variableTmp.IdValorVariableSuit;
                txtControl.CssClass = "valorVariablesUpdate";
                txtControl.Text = variableTmp.Valor.ToString();
                txtControl.ValidationGroup = "ActualizarSuit";
                txtControl.Attributes.Add("idVar", variableTmp.IdValorVariableSuit.ToString());
                txtControl.Width = new Unit(50, UnitType.Percentage);

                FilteredTextBoxExtender ftb = new FilteredTextBoxExtender();
                ftb.FilterType = FilterTypes.Custom;
                ftb.ValidChars = "0123456789.";
                ftb.FilterMode = FilterModes.ValidChars;
                ftb.TargetControlID = txtControl.ID;

                // etiqueta
                TableCell celdaEtiquetaTmp = new TableCell();
                Label lblEtiqueta = new Label();

                lblEtiqueta.Text = listaVariablesTmp.
                                   Where(V => V.IdVariable == (int)variableTmp.VariableReference.EntityKey.EntityKeyValues[0].Value).
                                   Select(V => V.NombreVariable).
                                   FirstOrDefault();
                lblEtiqueta.CssClass = "etiquetasListaVariables";

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ErrorMessage = "*";
                rfv.ControlToValidate = "varSuitUpdate_" + variableTmp.IdValorVariableSuit;
                rfv.Display = ValidatorDisplay.Dynamic;
                rfv.CssClass = "error";
                rfv.ValidationGroup = "ActualizarSuit";

                // celda
                celdaEtiquetaTmp.Controls.Add(lblEtiqueta);
                celdaEtiquetaTmp.Controls.Add(rfv);
                celdaControlTmp.Controls.Add(txtControl);
                celdaControlTmp.Controls.Add(ftb);

                // fila
                TableRow filaTmp = new TableRow();
                filaTmp.Width = new Unit(25, UnitType.Percentage);
                filaTmp.Controls.Add(celdaEtiquetaTmp);
                filaTmp.Controls.Add(celdaControlTmp);

                // tabla
                tablaTmp.Controls.Add(filaTmp);
            }

            // panel
            pnlVariablesValor.Controls.Add(tablaTmp);
        }

        private void ObtenerVariablesConValor(ref Suit_Propietario suitPropietarioTmp, int suite)
        {
            string valoresVariables = txtValoresVariables.Text;
            string[] arrValores = valoresVariables.Split('%');
            List<ObjetoGenerico> listaVariables = (List<ObjetoGenerico>)Session["ListaVariablesSuit"];

            for (int i = 0; i < (arrValores.Length - 1); i = i + 3)
            {
                Random random = new Random();
                int idVariableSuit = random.Next();

                Valor_Variable_Suit variableSuitTmp = new Valor_Variable_Suit();

                int idVariable = int.Parse(arrValores[i]);
                variableSuitTmp.IdValorVariableSuit = idVariableSuit;
                variableSuitTmp.Valor = double.Parse(arrValores[i + 1]);
                variableSuitTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", idVariable);
                variableSuitTmp.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", suitPropietarioTmp.IdSuitPropietario);

                ObjetoGenerico varTmp = new ObjetoGenerico();
                varTmp.IdVariable = idVariable;
                varTmp.NombreVariable = arrValores[i + 2];

                listaVariables.Add(varTmp);
                suitPropietarioTmp.Valor_Variable_Suit.Add(variableSuitTmp);
            }

            Session["ListaVariablesSuit"] = listaVariables;
        }

        private bool ValidarPorcentajeParticipacion(int idPropietario, int idSuite)
        {
            return true;
        }
        #endregion

        #region Eventos

        protected void ddlTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoPersona.SelectedValue == "NATURAL")
            {
                lblIde.Text = GetGlobalResourceObject("Resource", "lblNumIdentificacion").ToString();
                lblNombre.Text = GetGlobalResourceObject("Resource", "lblNombre").ToString();

                txtNombreSegundo.Enabled = true;
                txtApellidoPrimero.Enabled = true;
                txtApellidoSegundo.Enabled = true;
                ddlTipoDocumento.Enabled = true;

                rfv_TipoDocuemnto.Enabled = true;
                rfv_Apellido.Enabled = true;

                if (this.PropietarioTmp != null)
                {
                    txtNombreSegundo.Text = this.PropietarioTmp.SegundoNombre;
                    txtApellidoPrimero.Text = this.PropietarioTmp.PrimerApellido;
                    txtApellidoSegundo.Text = this.PropietarioTmp.SegundoApellido;
                    ddlTipoDocumento.SelectedValue = this.PropietarioTmp.TipoDocumento;
                }
            }
            else
            {
                lblIde.Text = GetGlobalResourceObject("Resource", "lblNit").ToString();
                lblNombre.Text = GetGlobalResourceObject("Resource", "lblNombreSolo").ToString();

                rfv_TipoDocuemnto.Enabled = false;
                rfv_Apellido.Enabled = false;

                txtNombreSegundo.Enabled = false;
                txtApellidoPrimero.Enabled = false;
                txtApellidoSegundo.Enabled = false;

                ddlTipoDocumento.Enabled = false;
                ddlTipoDocumento.SelectedIndex = 1;

                txtNombreSegundo.Text = string.Empty;
                txtApellidoPrimero.Text = string.Empty;
                txtApellidoSegundo.Text = string.Empty;
            }
        }

        protected void ddlDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CiudadBo ciudadBoTmp = new CiudadBo();
            ddlCiudad.DataSource = ciudadBoTmp.ObtenerPorDepto(int.Parse(ddlDepto.SelectedValue));
            ddlCiudad.DataTextField = "Nombre";
            ddlCiudad.DataValueField = "IdCiudad";
            ddlCiudad.DataBind();
        }

        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtDescripcionSuit.Text = "";
                int idHotel = int.Parse(ddlHotel.SelectedValue);

                SuitBo suitBoTmp = new SuitBo();
                List<ObjetoGenerico> listaSuit = suitBoTmp.ObtenerSuitsPorHotel2(idHotel);

                List<Suit> listaSuitCurrent = suitBoTmp.ObtenerSuitesPorPropietario(this.IdPropietarioSeleccionado, idHotel);

                foreach (Suit itemSuit in listaSuitCurrent)
                {
                    listaSuit.RemoveAll(S => S.IdSuit == itemSuit.IdSuit);
                }

                ddlSuit.DataSource = listaSuit;
                ddlSuit.DataValueField = "IdSuit";
                ddlSuit.DataTextField = "NumSuit";
                ddlSuit.DataBind();

                ListItem itemTmp = new ListItem("Seleccione...", "");
                ddlSuit.Items.Insert(0, itemTmp);

                ddlSuit_SelectedIndexChanged(null, null);

                CargarVariablesSuite(int.Parse(ddlHotel.SelectedValue));
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void ddlSuit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSuit.SelectedValue == "")
                return;

            SuitBo suitBoTmp = new SuitBo();
            Suit suitTmp = suitBoTmp.ObtenerSuit(int.Parse(ddlSuit.SelectedValue));
            txtDescripcionSuit.Text = suitTmp.Descripcion;

            CargarVariablesSuite(int.Parse(ddlHotel.SelectedValue));
        }

        protected void gvwPropietario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow filaSeleccionada = gvwPropietario.SelectedRow;
            this.IdPropietarioSeleccionado = int.Parse(gvwPropietario.DataKeys[filaSeleccionada.RowIndex]["IdPropietario"].ToString());

            this.CargarDatosPropietario(this.IdPropietarioSeleccionado);
            this.CargarGrillaSuits(this.IdPropietarioSeleccionado);

            this.btnNuevo.Visible = false;
            this.GrillaPropietario.Visible = false;
            //this.btnGuardar.Visible = false;

            //this.btnActualizar.Visible = true;
            this.NuevoHotel.Visible = true;
            this.btnEliminar.Visible = true;
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = (ImageButton)sender;
            SuitPropietarioBo suitPropiedadBoTmp = new SuitPropietarioBo();
            int idSuitPropietario = int.Parse(imgButton.CommandArgument);

            if (HiddenIdPropietario.Value == "-1")
            {
                Propietario propietarioTmp = (Propietario)Session["NuevoPropietario"];
                Suit_Propietario suitPropietarioTmp = propietarioTmp.
                                                      Suit_Propietario.
                                                      Where(SP => SP.IdSuitPropietario == idSuitPropietario).
                                                      FirstOrDefault();

                List<ObjetoGenerico> listaSuit = (List<ObjetoGenerico>)Session["ListaSuit"];
                listaSuit.RemoveAll(S => S.IdSuitPropietario == suitPropietarioTmp.IdSuitPropietario);

                propietarioTmp.Suit_Propietario.Remove(suitPropietarioTmp);

                Session["ListaSuit"] = listaSuit;
                Session["NuevoPropietario"] = propietarioTmp;

                this.CargarGrillaSuitsTmp();
            }
            else
            {
                if (suitPropiedadBoTmp.EsSuitEliminada(idSuitPropietario, this.IdPropietarioSeleccionado))
                {

                    suitPropiedadBoTmp.Eliminar(idSuitPropietario, UsuarioLogin.Id);
                    this.CargarGrillaSuits(0);

                    lbltextoExito.Text = "Registro Eliminado";
                }
                else
                {
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_23;
                }
            }

            gvwSuits.EditIndex = -1;
        }

        protected void gvwSuits_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            pnlVariablesValor.Controls.Clear();
            pnlMisVariables.Controls.Clear();

            this.IdSuitPropietarioSeleccionado = Convert.ToInt32(gvwSuits.DataKeys[e.NewSelectedIndex]["IdSuitPropietario"]);
            int idSuit = Convert.ToInt32(gvwSuits.DataKeys[e.NewSelectedIndex]["IdSuit"]);
            hiddenIdSuitPropietarioSeleccionado.Value = this.IdSuitPropietarioSeleccionado.ToString();
            hiddenIdUsuario.Value = this.UsuarioLogin.Id.ToString();

            if (HiddenIdPropietario.Value == "-1")
            {
                Propietario propietarioTmp = (Propietario)Session["NuevoPropietario"];
                List<ObjetoGenerico> listaSuit = (List<ObjetoGenerico>)Session["ListaSuit"];
                ObjetoGenerico suitTmp = listaSuit.Where(S => S.IdSuitPropietario == this.IdSuitPropietarioSeleccionado).FirstOrDefault();

                lblHotelDetalle.Text = suitTmp.NombreHotel;
                lblSuitDetalle.Text = suitTmp.NumEscritura;
                lblEscrituraDetalle.Text = suitTmp.NumSuit;
                ddlBancoDetalleUpdate.SelectedValue = suitTmp.IdBanco.ToString();
                ddlTipoCuentaDetalleUpdate.SelectedValue = suitTmp.TipoCuenta;
                txtCuentaDetalleUpdate.Text = suitTmp.NumCuenta;
                txtNumEstadiasUpdate.Text = suitTmp.NumEstadias.ToString();
                txtTitularDetalleUpdate.Text = suitTmp.Titular;

                Suit_Propietario suitPropietarioTmp = propietarioTmp.
                                                        Suit_Propietario.
                                                        Where(SP => SP.IdSuitPropietario == this.IdSuitPropietarioSeleccionado).
                                                        FirstOrDefault();

                this.CargarVariablesSuite(suitPropietarioTmp.Valor_Variable_Suit.ToList());
            }
            else
            {
                ObjetoGenerico suitTmp = null;
                SuitBo suitBoTmp = new SuitBo();
                suitTmp = suitBoTmp.ObtenerSuitPorPropietario(this.IdSuitPropietarioSeleccionado);

                ValorVariableBo variableSuitBoTmp = new ValorVariableBo();

                VariableBo variablesSuiteBoTmp = new VariableBo();
                List<Variable> listaVariables = variablesSuiteBoTmp.VerTodos(suitTmp.IdHotel, true, "P");

                List<ObjetoGenerico> listaVariablesLlenas = variableSuitBoTmp.
                                                           ObtenerValoresVariables(this.IdSuitPropietarioSeleccionado, true);

                //int idTmp = 0;
                foreach (Variable itemVariable in listaVariables)
                {
                    if (listaVariablesLlenas.Where(V => V.IdVariableSuite == itemVariable.IdVariable).Count() == 0)
                    {
                        ObjetoGenerico variableTmp = new ObjetoGenerico();
                        //idTmp = idTmp - 1;
                        variableTmp.IdValorVariableSuit = itemVariable.IdVariable * (-1);
                        variableTmp.IdSuitPropietario = this.IdSuitPropietarioSeleccionado;// listaVariablesLlenas[0].IdSuitPropietario;
                        variableTmp.IdVariableSuite = itemVariable.IdVariable;
                        variableTmp.Nombre = itemVariable.Nombre;
                        variableTmp.Valor = 1;
                        variableTmp.EsValidacion = itemVariable.EsConValidacion;
                        variableTmp.ValMax = itemVariable.MaxNumero;

                        listaVariablesLlenas.Add(variableTmp);
                    }
                }

                this.CargarVariablesSuite(listaVariablesLlenas, idSuit, this.IdSuitPropietarioSeleccionado);

                lblHotelDetalle.Text = suitTmp.NombreHotel;
                lblSuitDetalle.Text = suitTmp.NumSuit;
                lblEscrituraDetalle.Text = suitTmp.NumEscritura;
                txtTitularDetalleUpdate.Text = suitTmp.Titular;
                ddlBancoDetalleUpdate.SelectedValue = suitTmp.IdBanco.ToString();
                ddlTipoCuentaDetalleUpdate.SelectedValue = suitTmp.TipoCuenta;
                txtCuentaDetalleUpdate.Text = suitTmp.NumCuenta;
                txtNumEstadiasUpdate.Text = suitTmp.NumEstadias.ToString();
            }

            suitDetalle.Visible = true;
        }

        protected void ddlHotelFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GrillaPropietario.Visible = true;
                this.CargarGrilla();
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void gvwPropietario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwPropietario.PageIndex = e.NewPageIndex;
            this.CargarGrilla();
        }

        void uc_WebUserBuscadorPropietario_AlAceptar(object sender, EventArgs e)
        {
            if (uc_WebUserBuscadorPropietario.IdPropietarioBuscado == -1)
                return;

            this.IdPropietarioSeleccionado = uc_WebUserBuscadorPropietario.IdPropietarioBuscado;

            this.CargarDatosPropietario(this.IdPropietarioSeleccionado);
            this.CargarGrillaSuits(this.IdPropietarioSeleccionado);

            this.btnNuevo.Visible = false;
            this.GrillaPropietario.Visible = false;
            //this.btnGuardar.Visible = false;

            //this.btnActualizar.Visible = true;
            this.NuevoHotel.Visible = true;
        }

        #endregion

        #region Botones

        protected void btnAceptarSuit_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                gvwSuits.SelectedIndex = -1;

                Random random = new Random();
                int idSuitPropietario = random.Next();

                Propietario propietarioTmp = (Propietario)Session["NuevoPropietario"];

                Suit_Propietario suitPropietarioTmp = new Suit_Propietario();

                suitPropietarioTmp.IdSuitPropietario = idSuitPropietario;
                suitPropietarioTmp.PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Propietario", "IdPropietario", this.IdPropietarioSeleccionado);
                suitPropietarioTmp.SuitReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit", "IdSuit", int.Parse(ddlSuit.SelectedValue));
                suitPropietarioTmp.BancoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Banco", "IdBanco", int.Parse(ddlBanco.SelectedValue));
                suitPropietarioTmp.Titular = txtTitular.Text;
                suitPropietarioTmp.NumCuenta = txtNumCuenta.Text;
                suitPropietarioTmp.TipoCuenta = ddlTipoCuenta.SelectedValue;
                suitPropietarioTmp.NumEstadias = int.Parse(txtNumEstadias.Text);
                suitPropietarioTmp.EsActivo = true;

                if (HiddenIdPropietario.Value != "-1")
                {
                    this.ObtenerVariablesConValor(ref suitPropietarioTmp, int.Parse(ddlSuit.SelectedValue));
                    propietarioTmp.Suit_Propietario.Add(suitPropietarioTmp);

                    //Lista de suit en sesion para el nuevo propietario.
                    List<ObjetoGenerico> listaSuit = (List<ObjetoGenerico>)Session["ListaSuit"];
                    ObjetoGenerico suitTmp = new ObjetoGenerico();
                    suitTmp.IdSuitPropietario = idSuitPropietario;
                    suitTmp.NombreHotel = ddlHotel.SelectedItem.Text;
                    suitTmp.NumSuit = ddlSuit.SelectedItem.Text;
                    suitTmp.NumEstadias = int.Parse(txtNumEstadias.Text);
                    suitTmp.IdBanco = int.Parse(ddlBanco.SelectedValue);
                    suitTmp.Titular = txtTitular.Text;
                    suitTmp.NumCuenta = txtNumCuenta.Text;
                    suitTmp.TipoCuenta = ddlTipoCuenta.SelectedValue;
                    suitTmp.Estado = "Inactivar";
                    listaSuit.Add(suitTmp);
                    Session["ListaSuit"] = listaSuit;

                    this.CargarGrillaSuitsTmp();
                    Session["NuevoPropietario"] = propietarioTmp;
                }
                else
                {
                    SuitPropietarioBo suitPropietarioBoTmp = new SuitPropietarioBo();
                    this.ObtenerVariablesConValor(ref suitPropietarioTmp, int.Parse(ddlSuit.SelectedValue));
                    string nombrePropietario = String.Concat(txtNombre, txtNombreSegundo, txtNombreSegundo, txtNombreSegundo);
                    idSuitPropietario = suitPropietarioBoTmp.Guardar(suitPropietarioTmp, ddlBanco.SelectedItem.Text, nombrePropietario, this.UsuarioLogin.Id);

                    //ValorVariableBo ValorVariableSuitBoTmp = new ValorVariableBo();
                    //ValorVariableSuitBoTmp.Guardar(listaSuitVariables);

                    this.CargarGrillaSuits(this.IdPropietarioSeleccionado);
                }

                txtNumCuenta.Text = "";
                ddlSuit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.EsIdentificacionExistentePropietario(txtNumIdentificacion.Text, this.IdPropietarioSeleccionado))
                {
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_10;
                    return;
                }

                PropietarioBo propietarioBoTmp = new PropietarioBo();

                propietarioBoTmp.Actualizar(this.IdPropietarioSeleccionado, txtNombre.Text, txtNombreSegundo.Text, txtApellidoPrimero.Text, txtApellidoSegundo.Text, ddlTipoPersona.SelectedValue,
                                            txtNumIdentificacion.Text, txtCorreo.Text, txtCorreo2.Text, txtCorreo3.Text, chActivo.Checked, int.Parse(ddlCiudad.SelectedValue),
                                            Properties.Settings.Default.IdPropietario, txtDireccion.Text, txtTel1.Text,
                                            txtTel2.Text, txtNombreContacto.Text, txtTelContacto.Text, txtCorreoContacto.Text, ddlTipoDocumento.SelectedValue, cbEsRetenedor.Checked, this.UsuarioLogin.Id);

                btnVerTodos_Click(null, null);

                lbltextoExito.Text = Resources.Resource.lblMensajeActualizar;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            gvwSuits.SelectedIndex = -1;

            LimpiarFormulario();
            CargarGrilla();

            this.GrillaPropietario.Visible = true;

            this.btnNuevo.Visible = true;
            //this.btnActualizar.Visible = false;
            //this.btnGuardar.Visible = false;
            this.btnEliminar.Visible = false;
            this.NuevoHotel.Visible = false;
            this.suitDetalle.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                PropietarioBo propietarioBoTmp = new PropietarioBo();
                Propietario propietarioTmp = propietarioBoTmp.ObtenerPropietario(txtNumIdentificacion.Text); //(Propietario)Session["NuevoPropietario"];

                if (propietarioTmp != null)
                {
                    if ((((Propietario)Session["NuevoPropietario"]).Suit_Propietario).Count > 0)
                    {
                        SuitPropietarioBo suitePropietarioBoTmp = new SuitPropietarioBo();
                        List<Suit_Propietario> listaSuitPropietario = ((Propietario)Session["NuevoPropietario"]).Suit_Propietario.ToList();
                        List<ObjetoGenerico> lstSuitPropietario = new List<ObjetoGenerico>();

                        foreach (Suit_Propietario itemSuitePropietario in listaSuitPropietario)
                        {
                            ObjetoGenerico item = new ObjetoGenerico();

                            item.NumCuenta = itemSuitePropietario.NumCuenta;
                            item.NumEstadias = itemSuitePropietario.NumEstadias;
                            item.TipoCuenta = itemSuitePropietario.TipoCuenta;
                            item.Titular = itemSuitePropietario.Titular;
                            item.IdSuit = (int)itemSuitePropietario.SuitReference.EntityKey.EntityKeyValues[0].Value;
                            item.IdPropietario = propietarioTmp.IdPropietario;
                            item.IdBanco = (int)itemSuitePropietario.BancoReference.EntityKey.EntityKeyValues[0].Value;

                            lstSuitPropietario.Add(item);
                            item.ListaVariables = new List<ObjetoGenerico>();
                            foreach (Valor_Variable_Suit itemValorVariable in itemSuitePropietario.Valor_Variable_Suit)
                            {
                                item.ListaVariables.Add(new ObjetoGenerico()
                                {
                                    IdVariable = (int)itemValorVariable.VariableReference.EntityKey.EntityKeyValues[0].Value,
                                    Valor = itemValorVariable.Valor
                                });
                            }
                        }

                        suitePropietarioBoTmp.Guardar(lstSuitPropietario, propietarioTmp.IdPropietario);

                        this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;

                        Session.Remove("NuevoPropietario");
                        Session.Remove("ListaSuit");
                        Session.Remove("ListaVariablesSuit");
                        btnVerTodos_Click(null, null);
                    }
                    else
                    {
                        this.lbltextoError.Text = Resources.Resource.lblMensajeError_15;
                    }
                }
                else
                {
                    string clave = Utilities.EncodePassword(String.Concat(txtNumIdentificacion.Text, txtNumIdentificacion.Text));

                    propietarioTmp = (Propietario)Session["NuevoPropietario"];
                    propietarioTmp.NombrePrimero = txtNombre.Text;
                    propietarioTmp.NombreSegundo = txtNombreSegundo.Text;
                    propietarioTmp.ApellidoPrimero = txtApellidoPrimero.Text;
                    propietarioTmp.ApellidoSegundo = txtApellidoSegundo.Text;
                    propietarioTmp.TipoPersona = ddlTipoPersona.SelectedValue;
                    propietarioTmp.TipoDocumento = ddlTipoDocumento.SelectedValue;

                    propietarioTmp.NumIdentificacion = txtNumIdentificacion.Text;
                    propietarioTmp.Login = txtNumIdentificacion.Text;
                    propietarioTmp.Pass = clave;
                    propietarioTmp.Activo = chActivo.Checked;
                    propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", int.Parse(ddlCiudad.SelectedValue));
                    propietarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", Properties.Settings.Default.IdPropietario);
                    propietarioTmp.Correo = txtCorreo.Text;
                    propietarioTmp.Correo2 = txtCorreo2.Text;
                    propietarioTmp.Correo3 = txtCorreo3.Text;
                    propietarioTmp.Direccion = txtDireccion.Text;
                    propietarioTmp.Telefono_1 = txtTel1.Text;
                    propietarioTmp.Telefono_2 = txtTel2.Text;
                    propietarioTmp.NombreContacto = txtNombreContacto.Text;
                    propietarioTmp.TelefonoContacto = txtTelContacto.Text;
                    propietarioTmp.CorreoContacto = txtCorreoContacto.Text;
                    propietarioTmp.EsRetenedor = cbEsRetenedor.Checked;
                    propietarioTmp.Cambio = true;

                    if (propietarioTmp.Suit_Propietario.Count > 0)
                    {
                        propietarioBoTmp.Guardar(propietarioTmp, this.UsuarioLogin.Id);

                        this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;

                        Session.Remove("NuevoPropietario");
                        Session.Remove("ListaSuit");
                        Session.Remove("ListaVariablesSuit");
                        btnVerTodos_Click(null, null);
                    }
                    else
                    {
                        this.lbltextoError.Text = Resources.Resource.lblMensajeError_15;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            //ReportViewer_ReportePropietarios.Report = null;

            Propietario propetarioTmp = new Propietario();
            Session["NuevoPropietario"] = propetarioTmp;
            Session["ListaSuit"] = new List<ObjetoGenerico>();
            Session["ListaVariablesSuit"] = new List<ObjetoGenerico>();

            this.IdPropietarioSeleccionado = -1; //limpiamos el id por precaucion.
            hiddenIdSuitPropietarioSeleccionado.Value = "-1";
            hiddenIdUsuario.Value = this.UsuarioLogin.Id.ToString();
            HiddenIdPropietario.Value = "-1";

            LimpiarFormulario();

            this.btnNuevo.Visible = false;
            //this.btnActualizar.Visible = false;
            this.GrillaPropietario.Visible = false;

            //this.btnGuardar.Visible = true;
            this.NuevoHotel.Visible = true;
        }

        //protected void btnActualizarSuit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string valoresVariables = txtValorVariableUpdate.Text;
        //        string[] arrValores = valoresVariables.Split('%');
        //        string nombrePropietario = (txtNombre.Text + " " + txtNombreSegundo.Text + " " + txtApellidoPrimero.Text + " " + txtApellidoSegundo.Text);

        //        if (btnGuardar.Visible)
        //        {
        //            Propietario propietarioTmp = (Propietario)Session["NuevoPropietario"];

        //            Suit_Propietario suitPropietarioTmp = propietarioTmp.
        //                                                  Suit_Propietario.
        //                                                  Where(SP => SP.IdSuitPropietario == this.IdSuitPropietarioSeleccionado).
        //                                                  FirstOrDefault();

        //            suitPropietarioTmp.NumCuenta = txtCuentaDetalleUpdate.Text;
        //            suitPropietarioTmp.NumEstadias = int.Parse(txtNumEstadiasUpdate.Text);
        //            suitPropietarioTmp.Titular = txtTitularDetalleUpdate.Text;
        //            suitPropietarioTmp.TipoCuenta = ddlTipoCuentaDetalleUpdate.SelectedValue;
        //            suitPropietarioTmp.EsActivo = true;
        //            suitPropietarioTmp.BancoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Banco", "IdBanco", int.Parse(ddlBancoDetalleUpdate.SelectedValue));

        //            for (int i = 0; i < (arrValores.Length - 1); i = i + 2)
        //            {
        //                Valor_Variable_Suit valorTmp = suitPropietarioTmp.Valor_Variable_Suit.Where(VVS => VVS.IdValorVariableSuit == int.Parse(arrValores[i])).FirstOrDefault();

        //                if (arrValores[i + 1].Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
        //                    valorTmp.Valor = Convert.ToDouble(arrValores[i + 1], System.Globalization.CultureInfo.InvariantCulture);
        //                else // aca quiere decir que puso una coma y lo reemplaza por un punto
        //                {
        //                    arrValores[i + 1].Replace(',', '.');
        //                    valorTmp.Valor = Convert.ToDouble(arrValores[i + 1]);
        //                }
        //            }

        //            //Lista de suit en sesion para el nuevo propietario.
        //            List<ObjetoGenerico> listaSuit = (List<ObjetoGenerico>)Session["ListaSuit"];

        //            ObjetoGenerico suitTmp = listaSuit.Where(S => S.IdSuitPropietario == this.IdSuitPropietarioSeleccionado).FirstOrDefault();
        //            suitTmp.NumEstadias = int.Parse(txtNumEstadiasUpdate.Text);
        //            suitTmp.IdBanco = int.Parse(ddlBancoDetalleUpdate.SelectedValue);
        //            suitTmp.Titular = txtTitularDetalleUpdate.Text;
        //            suitTmp.NumCuenta = txtCuentaDetalleUpdate.Text;
        //            suitTmp.TipoCuenta = ddlTipoCuentaDetalleUpdate.SelectedValue;

        //            Session["ListaSuit"] = listaSuit;
        //            Session["NuevoPropietario"] = propietarioTmp;

        //            this.CargarGrillaSuitsTmp();
        //        }
        //        else
        //        {
        //            SuitPropietarioBo suitPropietarioBoTmp = new SuitPropietarioBo();
        //            suitPropietarioBoTmp.Actualizar(this.IdSuitPropietarioSeleccionado,
        //                                            int.Parse(ddlBancoDetalleUpdate.SelectedValue),
        //                                            txtCuentaDetalleUpdate.Text,
        //                                            int.Parse(txtNumEstadiasUpdate.Text),
        //                                            txtTitularDetalleUpdate.Text,
        //                                            ddlTipoCuentaDetalleUpdate.SelectedValue,
        //                                            ddlBancoDetalleUpdate.SelectedItem.Text,
        //                                            nombrePropietario,
        //                                            this.UsuarioLogin.Id);

        //            List<ObjetoGenerico> listaSuitVariables = new List<ObjetoGenerico>();
        //            ValorVariableBo suitPropietarioVariableSuitBoTmp = new ValorVariableBo();

        //            for (int i = 0; i < (arrValores.Length - 1); i = i + 2)
        //            {
        //                ObjetoGenerico variableSuitTmp = new ObjetoGenerico();

        //                int idValorVariableSuit = int.Parse(arrValores[i]);                        

        //                if (idValorVariableSuit > 0)
        //                {
        //                    variableSuitTmp.IdValorVariableSuit = idValorVariableSuit;
        //                    if (arrValores[i + 1].Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
        //                    {
        //                        variableSuitTmp.Valor = Convert.ToDouble(arrValores[i + 1], System.Globalization.CultureInfo.InvariantCulture);

        //                    }
        //                    else // aca quiere decir que puso una coma y lo reemplaza por un punto
        //                    {
        //                        arrValores[i + 1].Replace(',', '.');
        //                        variableSuitTmp.Valor = Convert.ToDouble(arrValores[i + 1]);
        //                    }
        //                    //variableSuitTmp.Valor = double.Parse(arrValores[i + 1]);
        //                    listaSuitVariables.Add(variableSuitTmp);
        //                }
        //                else
        //                {
        //                    Valor_Variable_Suit valorVariableSuitTmp = new Valor_Variable_Suit();
        //                    valorVariableSuitTmp.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", this.IdSuitPropietarioSeleccionado);
        //                    valorVariableSuitTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", (idValorVariableSuit * (-1)));

        //                    if (arrValores[i + 1].Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
        //                        valorVariableSuitTmp.Valor = Convert.ToDouble(arrValores[i + 1], System.Globalization.CultureInfo.InvariantCulture);
        //                    else // aca quiere decir que puso una coma y lo reemplaza por un punto
        //                    {
        //                        arrValores[i + 1].Replace(',', '.');
        //                        valorVariableSuitTmp.Valor = Convert.ToDouble(arrValores[i + 1]);
        //                    }

        //                    //valorVariableSuitTmp.Valor = double.Parse(arrValores[i + 1]);
        //                    suitPropietarioVariableSuitBoTmp.Guardar(valorVariableSuitTmp);
        //                }
        //            }

        //            suitPropietarioVariableSuitBoTmp.Actualizar(listaSuitVariables, nombrePropietario, this.UsuarioLogin.Id);
        //        }

        //        suitDetalle.Visible = false;

        //        lbltextoExito.Text = Resources.Resource.lblMensajeActualizar; ;
        //        divExito.Visible = true;
        //        divError.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utilities.Log(ex);

        //        this.divExito.Visible = false;
        //        this.divError.Visible = true;
        //        this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
        //    }
        //}

        protected void btnAgregarSuit_Click(object sender, EventArgs e)
        {
            this.suitDetalle.Visible = false;
            gvwSuits.SelectedIndex = -1;

            ddlHotel_SelectedIndexChanged(false, null);
            //CargarVariablesSuite(int.Parse(ddlHotel.SelectedValue));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.suitDetalle.Visible = false;

            uc_WebUserBuscadorPropietario.PageSize = this.PageSize;
            uc_WebUserBuscadorPropietario.CargarGrilla();
        }

        protected void btnEstado_Click(object sender, EventArgs e)
        {
            try
            {
                //if (HiddenIdPropietario.Value != "-1")
                //    return;

                SuitPropietarioBo suitePropietarioBo = new SuitPropietarioBo();
                Button btnActivo = (Button)sender;

                if (btnActivo.Text == "Inactivar")
                {
                    suitePropietarioBo.SetEstadoSuitPropietario(int.Parse(btnActivo.CommandArgument), false);
                    this.lbltextoExito.Text = "Suite inactivada con exito.";
                }
                else
                {
                    suitePropietarioBo.SetEstadoSuitPropietario(int.Parse(btnActivo.CommandArgument), true);
                    this.lbltextoExito.Text = "Suite activada con exito.";
                }

                this.CargarGrillaSuits(this.IdPropietarioSeleccionado);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                PropietarioBo propietarioBo = new PropietarioBo();

                if (propietarioBo.EliminarPropietario(this.IdPropietarioSeleccionado))
                {
                    this.lbltextoExito.Text = "Propietario eliminado con exito.";
                }
                else
                {
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_9;
                }

                btnVerTodos_Click(null, null);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }
        }

        #endregion
    }
}

