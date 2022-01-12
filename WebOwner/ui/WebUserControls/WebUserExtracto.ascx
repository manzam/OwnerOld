<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserExtracto.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserEstracto" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register src="WebUserBuscadorPropietarioSuite.ascx" tagname="WebUserBuscadorPropietarioSuite" tagprefix="uc1" %>

<script language="javascript" type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j("[id$=txtAno]").spinner({
                min: 2000,
                max: y
            });

            $j("[id$=txtAnoHasta]").spinner({
                min: 2000,
                max: y
            });

            $j("#modalBuscadorPropietario").dialog({
                width: 1200,
                autoOpen: false,
                resizable: false,
                show: "slow",
                modal: true,
                height: 700,
                open: function(event, ui) {
                    var idCmb = $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_ddlHotelReporte').val();
                    $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_uc_WebUserBuscadorPropietario_ddlHotelBuscador').val(idCmb);
                },
                buttons: {
                    "Aceptar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserEstracto1_uc_WebUserBuscadorPropietarioSuite_btnAceptar').click();
                        $j(this).dialog("close");
                    },
                    "Cancelar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserEstracto1_uc_WebUserBuscadorPropietarioSuite_btnCancelar').click();
                        $j(this).dialog("close");
                    }
                }
            }).parent().appendTo($j("form:first")).css('z-index', '1005');

        }
    });

    function GetExtractoActual(ctrl) {
        var idP = $j(ctrl).attr("idProp");
        var idS = $j(ctrl).attr("idsuite");
        var idH = $j(ctrl).attr("idHotel");
        var m = $j("[id$=ddlMes]").val();
        var y = $j("[id$=txtAno]").val();

        var data = { idProp: idP, idSuite: idS, mm: m, yyyy: y, IdHotel: idH };
        var dataExtracto = JSON.stringify(data);

        window.open("../../handlers/HandlerExtracto.ashx?data=" + dataExtracto, '_blank');
        //window.location = "../../handlers/HandlerExtracto.ashx?data=" + dataExtracto;
    }
    
</script>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        
            <div class="botonera">
                <asp:Button ID="btnEnviarExtracto" 
                    runat="server" 
                    Text="<%$ Resources:Resource, btnEnviarExtractos %>" 
                    OnClick="btnEnviarExtracto_Click" ValidationGroup="GenerarExtracto" />
                <asp:Button ID="btnEnviarSeleccionados" 
                    runat="server" 
                    Text="Enviar Seleccionados" 
                    OnClick="btnEnviarSeleccionados_Click" ValidationGroup="GenerarExtracto" />
                <asp:Button ID="btnBuscar" runat="server" 
                    Text="<%$ Resources:Resource, btnBuscar %>" 
                    OnClientClick="$j('#modalBuscadorPropietario').dialog('open')" 
                    onclick="btnBuscar_Click" ValidationGroup="Buscar" />
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
        
            <div style="width:940px;">
            
                  <asp:UpdateProgress ID="UpdateProgress2"  AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                    <ProgressTemplate>
                        <div class="loader" style="width:940px; height:900px;">
                            <div style="width:940px; height:100%; position:absolute; left:50%; top:50%;">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/ajax-loader.gif" />
                                <p>
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblCargando %>"></asp:Label>                                
                                </p>                                
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
                <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                        <td style="width:10%;" class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                        </td>
                        <td style="width:90%;">
                            <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" Width="450px"  
                                onselectedindexchanged="ddlHotel_SelectedIndexChanged" ValidationGroup="GenerarExtracto">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                                ControlToValidate="ddlHotel" Display="Dynamic" CssClass="error" InitialValue="-1" ValidationGroup="GenerarExtracto">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;" class="textoTabla">
                            <asp:Label ID="Label2" runat="server" Text="Fecha Desde"></asp:Label>                            
                        </td>
                        <td style="width:90%;">
                            <asp:DropDownList ID="ddlMes" Width="110px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text="Enero" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                            </asp:DropDownList>
                            -
                            <asp:TextBox ID="txtAno" Enabled="false" runat="server" ValidationGroup="GenerarExtracto" Width="40px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtAno" Display="Dynamic" CssClass="error" ValidationGroup="GenerarExtracto" >
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label4" runat="server" Text="Fecha Hasta"></asp:Label>                            
                        </td>
                        <td style="width:90%;">
                            <asp:DropDownList ID="ddlMesHasta" Width="110px" runat="server">
                                <asp:ListItem Value="1" Text="Enero" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                            </asp:DropDownList>
                            -
                            <asp:TextBox ID="txtAnoHasta" Enabled="false" runat="server" ValidationGroup="GenerarExtracto" Width="40px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">Acumulado</td>
                        <td>
                            <asp:CheckBox ID="cbEsAcumulado" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <%--<asp:Button ID="bntGuardarTodos" runat="server" Text="Guardar Extractos" 
                                onclick="bntGuardarTodos_Click" ValidationGroup="GenerarExtracto"/>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <div class="tituloPrincipal">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, TituloGrillaPropietarios %>"></asp:Label>
                                </h2>        
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView 
                                ID="gvwPropietarios" 
                                runat="server"
                                DataKeyNames="IdPropietario,IdSuit" 
                                Width="100%" 
                                AutoGenerateColumns="False" 
                                CellPadding="2"
                                AllowPaging="True"
                                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                                BorderColor="#7599A9" 
                                BorderStyle="Solid" 
                                BorderWidth="1px" 
                                onrowdatabound="gvwPropietarios_RowDataBound"
                                onpageindexchanging="gvwPropietarios_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblSeleccion %>">
                                        <ItemTemplate>
                                            <asp:ImageButton 
                                                ID="imgBtnSeleccion" 
                                                runat="server"
                                                Height="35px" 
                                                Width="35px"                                    
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdSuit") + "," + DataBinder.Eval(Container.DataItem,"IdPropietario") %>'
                                                ImageUrl="~/img/117.png" onclick="imgBtnSeleccion_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Nombre" HeaderText="<%$ Resources:Resource, lblConcepto %>" >
                                        <HeaderStyle Width="55%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NumIdentificacion" HeaderText="<%$ Resources:Resource, lblNumIdentificacion %>" >
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NumSuit" HeaderText="<%$ Resources:Resource, lblNumSuit %>" >
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NumEscritura" HeaderText="Num. Escritura" >
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Extracto Histórico">
                                        <ItemTemplate>                                        
                                            <input id="Button1"
                                                type="button"
                                                runat="server"
                                                value="Extracto Histórico"
                                                class="ui-button ui-widget ui-state-default ui-corner-all"
                                                onclick="GetExtractoActual(this);"
                                                idHotel='<%# DataBinder.Eval(Container.DataItem,"IdHotel") %>'
                                                idProp='<%# DataBinder.Eval(Container.DataItem,"IdPropietario") %>'
                                                idSuite='<%# DataBinder.Eval(Container.DataItem,"IdSuit") %>'/>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblSeleccion %>">
                                        <ItemTemplate>                                        
                                            <asp:Button 
                                                ID="btnVerExtracto" 
                                                runat="server"
                                                Text="<%$ Resources:Resource, btnVerExtracto %>"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdPropietario") + "," + DataBinder.Eval(Container.DataItem,"IdSuit") %>' 
                                                onclick="btnVerExtracto_Click"
                                                ValidationGroup="GenerarExtracto" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enviar Extracto">
                                        <ItemTemplate>                                        
                                            <asp:Button 
                                                ID="btnEnviarExtractoIndividual" 
                                                runat="server"
                                                Text="Enviar"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdPropietario") + "," + DataBinder.Eval(Container.DataItem,"IdSuit") %>' 
                                                onclick="btnEnviarExtractoIndividual_Click"
                                                ValidationGroup="GenerarExtracto" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            
            <br />
            
            <asp:Panel ID="pnlReportes" runat="server">
                        
                <dx:ReportToolbar ID="ReportToolbar_Reporte"
                                  runat='server' 
                                  Width='900px'
                                  ShowDefaultButtons='False' 
                                  ReportViewer="<%# ReportViewer_Reporte %>">
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
                            </Elements>
                        </dx:ReportToolbarComboBox>
                    </Items>
                    <Styles>
                        <LabelStyle>
                            <Margins MarginLeft='3px' MarginRight='3px' />
                        </LabelStyle>
                    </Styles>
                </dx:ReportToolbar>
                
                <dx:ReportViewer ID="ReportViewer_Reporte" Width="900px" runat="server">
                </dx:ReportViewer>
                
            </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>


<div id="modalBuscadorPropietario">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:WebUserBuscadorPropietarioSuite ID="uc_WebUserBuscadorPropietarioSuite" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>            
</div>