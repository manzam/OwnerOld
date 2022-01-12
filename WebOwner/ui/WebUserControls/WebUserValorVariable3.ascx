<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserValorVariable3.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebValorVariable3" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<script type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j(".fecha").spinner({
                min: 2000,
                max: y
            });

            $j(".fecha").val(y);

            var str = $j("#ctl00_Contenidoprincipal_WebUserValorVariable1_lbltextoError").text();
            $j("#ctl00_Contenidoprincipal_WebUserValorVariable1_lbltextoError").html(str);
        }

        $j("#modalReporteVariables").dialog({
            width: 1150,
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: 800,
            buttons: {
                "Salir": function() {
                    $j(this).dialog("close");
                }
            }
        }).parent().appendTo($j("form:first")).css('z-index', '1005');
    });        
    
</script>

<div style="display:none;">
    <asp:Button ID="btnDescarPlano" runat="server" Text="Descargar Plantilla" ValidationGroup="DescarPlano"  onclick="btnDescarPlano_Click" />
</div>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />        
        
        <div class="botonera">
            <%--<asp:Button ID="btnExportar" runat="server" Text="<%$ Resources:Resource, btnExportarVariables %>" OnClientClick='$j("#modalReporteVariables").dialog("open");' />--%>
            <asp:Button ID="btnClickDescarPlano" runat="server" Text="Descargar Plantilla" ValidationGroup="DescarPlano" OnClientClick="$j('#ctl00_Contenidoprincipal_WebValorVariable1_btnDescarPlano').click();" />
        </div>
        
    
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
        
        <br />
    
        <table width="70%">
            <tr>
                <td style="width:10%;" class="textoTabla">
                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label> *
                </td>
                <td style="width:90%;">
                    <asp:DropDownList ID="ddlMes" runat="server">
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
                    <asp:TextBox ID="txtFecha" Enabled="false" CssClass="fecha" Width="40px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label> *
                </td>                
                <td>
                    <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" Width="70%" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    Cargar
                </td>
                <td>
                    <div style="width:35%;float:left;">
                        <asp:AsyncFileUpload ID="AsyncFileUpload" Width="95%" runat="server" ThrobberID="ImgLoader" PersistFile="true" />
                    </div>
                    <div style="width:10%;float:left;">
                        <asp:Button ID="btnCargar" runat="server" Text="Cargar" ValidationGroup="DescarPlano" onclick="btnCargar_Click" />
                    </div>
                    <div style="clear:both"></div>
                    <asp:Image ID="ImgLoader" ImageUrl="~/img/ajax-loader.gif" runat="server" />
                </td>
            </tr>
        </table>

        <br />
    
        <asp:GridView 
            ID="gvwVariable" 
            runat="server" 
            AutoGenerateColumns="False" 
            BorderStyle="Solid" 
            BorderWidth="1px" 
            BorderColor="#7599A9" 
            CellPadding="2"
            DataKeyNames="IdVariable" 
            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
            Width="100%"
            onselectedindexchanged="gvwVariable_SelectedIndexChanged" 
            onrowcancelingedit="gvwVariable_RowCancelingEdit" 
            onrowediting="gvwVariable_RowEditing" 
            onrowupdating="gvwVariable_RowUpdating">
            <Columns>
                <asp:CommandField ButtonType="Image" 
                                  ShowSelectButton="true"
                                  ShowEditButton="true"                                   
                                  HeaderText="<%$ Resources:Resource, lblAccion %>" 
                                  SelectImageUrl="~/img/23.png"
                                  UpdateImageUrl="~/img/131.png"
                                  CancelImageUrl="~/img/72.png"
                                  EditImageUrl="~/img/13.png" >
                    <ControlStyle Height="30px" Width="30px" />
                    <HeaderStyle Width="15%" />
                    <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                </asp:CommandField>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="55%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblFecha %>" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtF" Enabled="false" runat="server" Width="75px"></asp:TextBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlMes" runat="server" Width="105px">
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
                        <asp:TextBox ID="txtFecha" Enabled="false" CssClass="fecha" runat="server" Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="15%" HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblValor %>" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtV" Enabled="false" runat="server" Width="75px"></asp:TextBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtValor" runat="server" Width="100px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtValor_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Custom,Numbers" 
                            FilterMode="ValidChars" ValidChars="0123456789,." TargetControlID="txtValor">
                        </asp:FilteredTextBoxExtender>
                    </EditItemTemplate>
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="15%" HorizontalAlign="Center" />
                </asp:TemplateField>               
            </Columns>
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>        
        
        <br />

            
        <h2>
            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblDetalle %>"></asp:Label>
        </h2>
        
        <table width="30%">
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblDetalle %>"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>               
            
                    <asp:GridView 
                        ID="gvwDetalleVariable" 
                        runat="server" 
                        AutoGenerateColumns="False" 
                        BorderStyle="Solid" 
                        BorderWidth="1px" 
                        BorderColor="#7599A9" 
                        CellPadding="2"
                        EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                        Width="100%">
                        <Columns>                
                            <asp:BoundField HeaderText="<%$ Resources:Resource, lblValor %>" DataField="Valor" DataFormatString="{0:N}" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="35%" HorizontalAlign="Right" />
                                <HeaderStyle Width="70%" />
                            </asp:BoundField >
                            <asp:BoundField HeaderText="<%$ Resources:Resource, lblFecha %>" DataField="Fecha" DataFormatString="{0: MM-yyyy}" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="35%" HorizontalAlign="Center" />
                                <HeaderStyle Width="30%" />
                            </asp:BoundField >                    
                        </Columns>
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                    </asp:GridView>
                </td>
            </tr>        
        </table>
        
    </ContentTemplate>
</asp:UpdatePanel>


<div id="modalReporteVariables">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <table width="70%">
                <tr>
                    <td style="width:10%;" class="textoTabla">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label>
                    </td>
                    <td style="width:90%;">
                        <asp:TextBox ID="txtFechaReporte" Enabled="false" Width="20%" CssClass="fecha" runat="server" ValidationGroup="reporte"></asp:TextBox>
                        <asp:RequiredFieldValidator 
                            ID="RequiredFieldValidator1" 
                            runat="server" 
                            ErrorMessage="*" 
                            ValidationGroup="reporte"
                            Text="*"
                            ControlToValidate="txtFechaReporte">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlHotelReporte" runat="server" Width="70%" ValidationGroup="reporte">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator 
                            ID="RequiredFieldValidator2" 
                            runat="server" 
                            ErrorMessage="*" 
                            ValidationGroup="reporte"
                            Text="*"
                            ControlToValidate="ddlHotelReporte">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnAceptarReporte" runat="server" Text="Aceptar" ValidationGroup="reporte" 
                            onclick="btnAceptarReporte_Click" />
                    </td>
                </tr>
            </table>
        
            <dx:ReportToolbar 
                ID="ReportToolbar1" 
                runat='server' 
                Width='900px'
                ReportViewer="<%# ReportViewer_ReporteVariables %>"
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
            
            <dx:ReportViewer ID="ReportViewer_ReporteVariables" runat="server">
            </dx:ReportViewer>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
