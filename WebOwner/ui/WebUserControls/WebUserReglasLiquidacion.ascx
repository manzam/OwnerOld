<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserReglasLiquidacion.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserReglasLiquidacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

  <style type="text/css">
    ul { list-style-type: none; margin: 0; padding: 0; margin-bottom: 10px; }
    .op {
        border: 1px solid #fad42e;
        background: #fbec88;
        color: #363636;
        padding: 2px;
        text-align: center;
        margin-right: 1px;
        margin-left: 1px;
    }
    .var {
        border: 1px solid #c5dbec;
        background: #dfeffc;
        font-weight: bold;
        color: #2e6e9e;
        padding: 2px;
    }
  </style>

<script type="text/javascript">

    $j(document).ready(function() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(dragLista);
        dragLista(null, null);

        function dragLista(sender, args) {

            $j('').change(function() {

                if ($j('[id$=cbSegundaCuenta]').is(":checked")) {

                    $j("[id$=ddlVariableCondicion]").prop("disabled", true);
                    $j("[id$=ddlCondicion]").prop("disabled", true);
                    $j("[id$=txtValorCondicion]").prop("disabled", true);
                    $j("[id$=ddlCuentaContable2]").prop("disabled", true);

                    $j("[id$=ddlVariableCondicion]").removeAttr("disabled");
                    $j("[id$=ddlCondicion]").removeAttr("disabled");
                    $j("[id$=txtValorCondicion]").removeAttr("disabled");
                    $j("[id$=ddlCuentaContable2]").removeAttr("disabled");
                }
                else {
                    $j("[id$=ddlVariableCondicion]").prop("disabled", false);
                    $j("[id$=ddlCondicion]").prop("disabled", false);
                    $j("[id$=txtValorCondicion]").prop("disabled", false);
                    $j("[id$=ddlCuentaContable2]").prop("disabled", false);

                    $j("[id$=ddlVariableCondicion]").attr("disabled", "disabled");
                    $j("[id$=ddlCondicion]").attr("disabled", "disabled");
                    $j("[id$=txtValorCondicion]").attr("disabled", "disabled");
                    $j("[id$=ddlCuentaContable2]").attr("disabled", "disabled");
                }
            });

            $j("#modalOk").dialog({
                autoOpen: false,
                resizable: false,
                show: "slow",
                modal: true,
                height: "auto",
                title: "Eliminar",
                buttons: {
                    "Aceptar": function() {
                        $j(this).dialog('close');
                        $j('#' + $j('#ctl00_idCtrl').val()).click();
                    },
                    "Cancelar": function() {
                        $j('#ctl00_idCtrl').val('');
                        $j(this).dialog('close');
                    }
                }
            });

            //$j("#ctl00_Contenidoprincipal_WebUserReglasLiquidacion1_sortable").sortable({
            //    revert: true
            //});

            //$j(".variable").draggable({
            //    connectToSortable: "#ctl00_Contenidoprincipal_WebUserReglasLiquidacion1_sortable",
            //    helper: "clone",
            //    revert: "invalid",
            //    stop: function(event, ui) {
            //        organizarRegla();
            //    }
            //});

            $j("ul, li").disableSelection();

            $j("#modalReporteReglas").dialog({
                width: 1100,
                autoOpen: false,
                resizable: false,
                show: "slow",
                modal: true,
                height: "auto",
                buttons: {
                    "Salir": function() {
                        $j(this).dialog("close");
                    }
                }
            }).parent().appendTo($j("form:first")).css('z-index', '1005');

            $j("[id$=misVariables]").accordion();

            $j(".cbActivo").change(function() {
                var id = this.attributes[1].nodeValue;
                $j.ajax({
                    method: "POST",
                    //async: false,
                    url: "../../handlers/HandlerLiquidacion.ashx",
                    data: { ActionType: 6, idConcepto: id }
                })
                .done(function(res) {
                });
            });
        }
    });

    function organizarRegla() {
        $j("[id$=txtRegla]").val("");
        $j("[id$=txtVariable]").val("");
        $j("[id$=txtVariableComas]").val("");

        var regla = "";
        var variable = "";
        var varComas = "";

        var con = $j("[id$=sortable] li").length;

        $j("[id$=sortable] li").each(function(k) {
            if (k > (con - 1))
                return;

            var id = $j(this).attr('class').split(' ')[4];
            var txt = $j(this).text();

            if (txt == "" || txt == " ")
                return;

            regla += txt;

            if (esOperador(txt)) {
                variable += "," + txt;
                $j(this).attr("id", "operador_" + k);
            }
            else {
                variable += "," + id;
                varComas += "," + txt;
                $j(this).attr("id", id);
            }

            $j(this).attr("ondblclick", "eliminar(this);");
        });

        variable = variable.substr(1);
        varComas = varComas.substr(1);

        $j("[id$=txtRegla]").val(regla);
        $j("[id$=txtVariable]").val(variable);
        $j("[id$=txtVariableComas]").val(varComas);

        return true;
    }

    function esOperador(txt) {
        if (txt == "+" || txt == "-" || txt == "*" || txt == "/" || txt == "(" || txt == ")" ||
            txt == " case " || txt == " when " || txt == ">" || txt == "<" || txt == " else " || txt == " end " || txt == " then " || txt == " and " || txt == "=")
            return true;
        else
            return false;
    }


    function validarRegla() {
        $j('#' + ctl00_Contenidoprincipal_WebUserReglasLiquidacion1_btnValidarExpresion).click();
    }

    function eliminar(ctrl) {
        $j("#" + ctrl.id).remove();
        organizarRegla();
    }

    function compilarReglas() {
        var reglaUser = ($j("[id$=txtReglaUsuario]").val()).trim();
        // var expresionRegular = /[)*(-/<>]/;
        var resRegla = reglaUser.split(" ");

        if (resRegla.length >= 1) {
            var htmlReglas = '';
            var varAllComas = '';
            var varVarComas = '';
            resRegla.each(function(item) {
                item = item.toUpperCase();
                if (esOperador(item)) {
                    htmlReglas += '<span class="op">' + item + '</span>';
                    varAllComas += item + ',';
                } else {
                    var nom = getVariableId(item);
                    if (nom !== '') {
                        varAllComas += nom + ',';
                        varVarComas += item + ',';
                        htmlReglas += '<span class="var">' + item + '</span>';
                    }
                }
            })
            htmlReglas = htmlReglas;
            $j("div[id$='reglaCrear']").html(htmlReglas);
            $j("input[id$='txtRegla']").val(reglaUser.toUpperCase());
            $j("input[id$='txtVariable']").val(varAllComas.slice(0, -1));
            $j("input[id$='txtVariableComas']").val(varVarComas.slice(0, -1));
        }
    }

    function getVariableId(name) {
        var idRes = '';
        $j("[id$=misVariables-panel-1] li").each(function(item) {
            if ($j(this).text() === name)
                idRes = ("varV_" + $j(this).attr("id").split("varV_")[1]);
        });
        $j("[id$=misVariables-panel-2] li").each(function(item) {
            if ($j(this).text() === name)
                idRes = ("varC_" + $j(this).attr("id").split("varC_")[1]);
        });
        $j("[id$=misVariables-panel-3] li").each(function(item) {
            if ($j(this).text() === name)
                idRes = ("varH_" + $j(this).attr("id").split("varH_")[1]);
        });
        $j("[id$=misVariables-panel-4] li").each(function(item) {
            if ($j(this).text() === name)
                idRes = ("varCO_" + $j(this).attr("id").split("varCO_")[1]);
        });
        return idRes;
    }

    function copiarAlPortapapeles(id_elemento) {

        // Crea un campo de texto "oculto"
        var aux = document.createElement("input");

        // Asigna el contenido del elemento especificado al valor del campo
        aux.setAttribute("value", $j("li[id$='" + id_elemento + "']").text());

        // Añade el campo a la página
        document.body.appendChild(aux);

        // Selecciona el contenido del campo
        aux.select();

        // Copia el texto seleccionado
        document.execCommand("copy");

        // Elimina el campo de la página
        document.body.removeChild(aux);

    }

</script> 
    
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
    
        <div class="botonera">
            <asp:Button ID="btnNuevo" runat="server" 
                Text="<%$ Resources:Resource, btnNuevo %>" ValidationGroup="Nuevo" OnClick="btnNuevo_Click" />                                       
            <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="GuardarActualizar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnActualizar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="GuardarActualizar" OnClick="btnActualizar_Click"  />
            <asp:Button ID="btnVerTodos" runat="server" 
                Text="<%$ Resources:Resource, btnVerTodos %>" ValidationGroup="VerTodos" OnClick="btnVerTodos_Click" />
           <asp:Button ID="btnInformeReglas" runat="server" 
                Text="<%$ Resources:Resource, btnExportarRegla %>" OnClientClick='$j("#modalReporteReglas").dialog("open");' />
        </div>       

        <br />

        <div runat="server" class="cuadradoExito" id="divExito" visible="false">
            <asp:Image ID="imgExitoMsg" runat="server" ImageUrl="~/img/33.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoExito" runat="server" CssClass="textoExito" ></asp:Label>
        </div>

        <div runat="server" class="cuadradoError" id="divError" visible="false">
            <asp:Image ID="imgErrorMsg" runat="server" ImageUrl="~/img/115.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoError" runat="server" CssClass="textoError" ></asp:Label>
        </div>
        
        <table width="100%">
            <tr>
                <td style="width:15%;" class="textoTabla">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                </td>
                <td style="width:80%;text-align:left;">
                    <asp:DropDownList ID="ddlHotel" AutoPostBack="true"
                        runat="server"
                        Width="300px" 
                        OnSelectedIndexChanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>                    
                </td>
            </tr>
        </table>
        
        <div id="NuevoRegla" visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="2" align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosConcepto %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;"  class="textoTabla">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblNombreConcepto %>"></asp:Label> *
                    </td>
                    <td style="width:80%;text-align:left;">
                        <asp:TextBox ID="txtNombreConcepto" 
                                     runat="server" 
                                     Width="80%" 
                                     CssClass="soloMayusculas"
                                     MaxLength="100" 
                                     ValidationGroup="GuardarActualizar">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender 
                            ID="FilteredTextBoxExtender2" 
                            runat="server"
                            FilterMode="ValidChars" 
                            ValidChars="qwertyuioplkjhgfdsazxcvbnm_QWERTYUIOPLKJHGFDSAZXCVBNM" 
                            FilterType="Custom" 
                            TargetControlID="txtNombreConcepto">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNombreConcepto" Display="Dynamic" CssClass="error" ValidationGroup="GuardarActualizar" >
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblMostrarEnExtracto %>"></asp:Label>
                    </td>
                    <td style="width:80%;text-align:left;">
                        <asp:CheckBox ID="cbMostrarExtracto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblTipoConcepto %>"></asp:Label>
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:DropDownList ID="ddlNivelConcepto" runat="server" Width="400px">
                            <asp:ListItem Value="2" Text="<%$ Resources:Resource, TituloprincipalPropietarioCliente %>"></asp:ListItem>
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, lblHotel %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblCuentaContable %>"></asp:Label>
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:DropDownList ID="ddlCuentaContable" runat="server" Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>                
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        Variable acumular
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:DropDownList ID="ddlVariableEstadistica" runat="server" Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, lblNumDecimales %>"></asp:Label> *
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:TextBox ID="txtNumDecimales" runat="server" Text="2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender 
                            ID="FilteredTextBoxExtender1"                            
                            FilterType="Custom"
                            FilterMode="ValidChars"
                            ValidChars="0123456789"
                            TargetControlID="txtNumDecimales"
                            runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumDecimales" Display="Dynamic" CssClass="error" ValidationGroup="GuardarActualizar" >
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        Código Tercero
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:TextBox ID="txtCodigoTercero" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender 
                            ID="FilteredTextBoxExtender3"                            
                            FilterType="Custom"
                            FilterMode="ValidChars"
                            ValidChars="0123456789"
                            TargetControlID="txtCodigoTercero"
                            runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        Orden
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:TextBox ID="txtOrden" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender 
                            ID="FilteredTextBoxExtender4"                            
                            FilterType="Custom"
                            FilterMode="ValidChars"
                            ValidChars="0123456789"
                            TargetControlID="txtOrden"
                            runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        Mostrar en reporte liquidación
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:CheckBox ID="cbMostrarEnLiquidacion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:right;" class="textoTabla">
                        Aplica Retención
                    </td>
                    <td style="width:80%; text-align:left;">
                        <asp:CheckBox ID="cbEsRetencionAplicar" runat="server" />
                    </td>
                </tr>
                
                <tr>
                    <td style="padding-left:5px;" colspan="2">
                        <table style="width:100%;">
                            <tr>
                                <td class="textoTabla" style="width:20%; text-align:right;">Con Segunda Cuenta</td>
                                <td style="width:80%;text-align:left;">
                                    <asp:CheckBox ID="cbSegundaCuenta" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:20%; text-align:right;" class="textoTabla"><asp:Label ID="Label70" runat="server" Text="<%$ Resources:Resource, lblCuentaContable %>"></asp:Label></td>
                                <td style="width:80%;text-align:left;">
                                    <asp:DropDownList ID="ddlCuentaContable2" runat="server" Width="400px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="center" class="textoTabla" colspan="3">Condicion</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlVariableCondicion" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlCondicion" runat="server">
                                                    <asp:ListItem Text="Mayor e Igual" Value="&gt;="></asp:ListItem>
                                                    <asp:ListItem Text="Menor e Igual" Value="&lt;="></asp:ListItem>
                                                    <asp:ListItem Text="Igual" Value="="></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtValorCondicion" runat="server" Text="0"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender 
                                                    ID="FilteredTextBoxExtender50"                            
                                                    FilterType="Custom"
                                                    FilterMode="ValidChars"
                                                    ValidChars=".0123456789,"
                                                    TargetControlID="txtValorCondicion"
                                                    runat="server">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>                            
                        </table>
                    </td>
                </tr>
                
             </table>
             
             <br />            
             
             <table>
                <tr>
                    <td style="width:20%;">&nbsp;</td>
                    <td style="width:80%;" class="textoTabla">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblReglaValidacion %>"></asp:Label> *
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtRegla" Display="Dynamic" CssClass="error" ValidationGroup="GuardarActualizar" >
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>                 
                <tr>
                    <td valign="top">
                        <ul id="misVariables" runat="server">                     
                        </ul>
                    </td>                   
                    
                    <td valign="top" style="text-align:left;">
                        <table>
                            <tr>
                                 <td>
                                    <textarea 
                                        runat="server"
                                        name="txtReglaUsuario" 
                                        id="txtReglaUsuario" 
                                        rows="8" 
                                        cols="100" 
                                        placeholder="Pegue aqui la regla"
                                        onkeyup="compilarReglas()">
                                        </textarea>
                                </td>
                             </tr>
                            <tr>
                                <td>
                                    <div id="reglaCrear" runat="server"></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div runat="server" class="cuadradoExito" id="divResultado" visible="false">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/104.png" 
                                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
                            <asp:Label ID="textoResultado" runat="server" CssClass="textoExito" ></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                         <div style="display:none;">
                            <asp:TextBox ID="txtRegla" runat="server" ValidationGroup="GuardarActualizar" Width="100%"></asp:TextBox>
                            <asp:TextBox ID="txtVariable" runat="server" Width="100%"></asp:TextBox>
                            <asp:TextBox ID="txtVariableComas" runat="server" Width="100%"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        
        </div>        
        
        <div id="GrillaRegla" runat="server">
        
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Resource, TituloGrillaReglas %>"></asp:Label>                          
                </h2>
            </div>        
            
                <asp:GridView ID="gvwReglas" runat="server" 
                        AutoGenerateColumns="False"
                        BackColor="White" 
                        BorderStyle="Solid" 
                        BorderWidth="1px"
                        BorderColor="#7599A9"
                        CellPadding="2"
                        DataKeyNames="IdConcepto" 
                        EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                        OnSelectedIndexChanged="gvwReglas_OnSelectedIndexChanged"
                        Width="100%">
                        <Columns>
                            <asp:TemplateField  HeaderText="Activo" >
                               <ItemTemplate>
                                 <asp:CheckBox 
                                    id="cbActivo" 
                                    runat="server" 
                                    CssClass="cbActivo" 
                                    CommandArgument='<%# Eval("IdConcepto") %>'
                                    Checked='<%#(Eval("Activo"))%>'  />
                                   </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width:90%; text-align:left;">
                                                <asp:LinkButton ID="LinkButton80" runat="server" CommandName="Select" Text='<%# Bind("Nombre") %>'></asp:LinkButton> 
                                            </td>
                                            <td style="width:10%; text-align:right;">
                                                <asp:ImageButton ID="imgBtnEliminar90" ToolTip="<%$ Resources:Resource, lblMensajeEliminarPregunta %>" ImageUrl="~/img/126.png" Width="20px" Height="20px" runat="server" OnClientClick="ventanaOk(this);" />
                                                <asp:ImageButton 
                                                    ID="ImageButton1" 
                                                    runat="server"
                                                    CssClass="ctrlOculto" 
                                                    CommandArgument='<%# Bind("IdConcepto") %>'
                                                    OnClick="imgBtnEliminar_Click" />
                                            </td>
                                        </tr>
                                    </table>                                                                       
                                </ItemTemplate>
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Orden" HeaderText="Orden" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="Regla" HeaderText="<%$ Resources:Resource, lblRegla %>" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                            </asp:BoundField> 
                        </Columns>
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
              </asp:GridView>
            
        </div>

    </ContentTemplate>
</asp:UpdatePanel>


<div id="modalReporteReglas">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
        <table width="100%">
            <tr>
                <td style="width:20%;">
                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                </td>
                <td style="width:80%;text-align:left;">
                    <asp:DropDownList ID="ddlHotelReporte" AutoPostBack="true"
                        runat="server"
                        Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
                
            <dx:ReportToolbar 
            ID="ReportToolbar1" 
            runat='server' 
            Width='800px'
            ReportViewer="<%# ReportViewer_Reglas %>"
            ShowDefaultButtons='False'>
              <Items>
                  <dx:ReportToolbarButton ItemKind='Search' />
                  <dx:ReportToolbarSeparator />
                  <dx:ReportToolbarButton ItemKind='PrintReport' />
                  <dx:ReportToolbarButton ItemKind='PrintPage' />
                  <dx:ReportToolbarSeparator />
                  <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' />
                  <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' />
                  <dx:ReportToolbarLabel ItemKind='PageLabel' />
                  <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
                  </dx:ReportToolbarComboBox>
                  <dx:ReportToolbarLabel ItemKind='OfLabel' />
                  <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
                  <dx:ReportToolbarButton ItemKind='NextPage' />
                  <dx:ReportToolbarButton ItemKind='LastPage' />
                  <dx:ReportToolbarSeparator />
                  <dx:ReportToolbarButton ItemKind='SaveToDisk' />
                  <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                      <Elements>
                          <dx:ListElement Value='pdf' />
                          <dx:ListElement Value='xls' />
                          <dx:ListElement Value='xlsx' />
                          <dx:ListElement Value='rtf' />
                          <dx:ListElement Value='mht' />
                          <dx:ListElement Value='html' />
                          <dx:ListElement Value='txt' />
                          <dx:ListElement Value='csv' />
                          <dx:ListElement Value='png' />
                      </Elements>
                  </dx:ReportToolbarComboBox>
              </Items>
              <Styles>
                  <LabelStyle>
                      <Margins MarginLeft='3px' MarginRight='3px' />
                  </LabelStyle>
              </Styles>
          </dx:ReportToolbar>
          
          <dx:ReportViewer ID="ReportViewer_Reglas" runat="server">
          </dx:ReportViewer>
          
        </ContentTemplate>
    </asp:UpdatePanel>

</div> 
