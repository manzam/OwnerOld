<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ReporteConsolidadoPorSuite.aspx.cs" Inherits="WebOwner.ui.Paginas.ReporteConsolidadoPorSuite" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>

    <script type="text/jscript" language="javascript">

        var $j = jQuery.noConflict();

        $j(document).ready(function() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

            CargasIniciales(null, null);

            function CargasIniciales(sender, args) {

                var d = new Date();
                var y = d.getFullYear();

                $j("#ctl00_Contenidoprincipal_txtFecha,#ctl00_Contenidoprincipal_txtFechaPropietario").spinner({
                    min: 2000,
                    max: y
                });
            }
        });
            
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
        <ProgressTemplate>
            <div class="procesar redondeo">Procesando..</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <table width="100%">
                <tr>
                    <td style="width:20%;" class="textoTabla">Reporte</td>
                    <td style="width:80%;">
                        <asp:DropDownList ID="ddlReporte" AutoPostBack="true" runat="server" 
                            Width="450px" onselectedindexchanged="ddlReporte_SelectedIndexChanged">
                            <asp:ListItem Text="Seleccione" Value="S"></asp:ListItem>
                            <asp:ListItem Text="Propietarios - Suite" Value="uno"></asp:ListItem>
                            <asp:ListItem Text="Hotel - Suite" Value="dos"></asp:ListItem>
                            <asp:ListItem Text="Variables" Value="tres"></asp:ListItem>
                            <asp:ListItem Text="Usuarios" Value="cuatro"></asp:ListItem>
                            <asp:ListItem Text="Perfil - Permisos" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Historial Liquidacion" Value="H"></asp:ListItem>
                            <asp:ListItem Text="Consolidado por Suite" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Consolidado por Propietario" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Extracto Consolidado" Value="EC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <br />
            
            <div class="tituloPrincipal">
                <h2>
                    <asp:Label ID="Label1" runat="server" Text="Consolidado Suite"></asp:Label>
                </h2>
            </div> 
            
            <br />
            
            <table width="100%">
                <tr>
                    <td class="textoTabla" style="text-align:center;">
                        Filtros                    
                    </td>
                </tr>
            </table>
            
            <table width="100%">
                <tr>
                    <td class="textoTabla" style="width:10%">Hotel</td>
                    <td style="width:90%">
                        <asp:DropDownList ID="ddlHotelConsolidadoPorsuite" Width="90%" runat="server" 
                            AutoPostBack="true" 
                            onselectedindexchanged="ddlHotelConsolidadoPorsuite_SelectedIndexChanged">
                        </asp:DropDownList> 
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                    Suite
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSuiteConsolidadoPorsuite" Width="15%" runat="server" ValidationGroup="ConsolidadoPorSuite">
                            <asp:ListItem Text="Seleccione..." Value="-1" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                            ControlToValidate="ddlSuiteConsolidadoPorsuite" InitialValue="-1" Display="Dynamic" CssClass="error" ValidationGroup="ConsolidadoPorSuite" > 
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="50%">
                            <tr>
                                <td style="width:20%" class="textoTabla">Mes Inicio</td>
                                <td style="width:20%" class="textoTabla">Mes Final</td>
                                <td style="width:10%" class="textoTabla">Año</td>
                            </tr>
                            <tr>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlMesInicio" Width="110px" runat="server">
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
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlMesFin" Width="110px" runat="server">
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
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecha" Enabled="false" runat="server" Width="40px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="btnConsolidadoPorSuite" runat="server" Text="Aceptar" 
                            onclick="btnConsolidadoPorSuite_Click" ValidationGroup="ConsolidadoPorSuite" />
                    </td>
                </tr>
           </table> 
           
           <br />
            
            <dx:ReportToolbar ID="ReportToolbar1" runat='server' Width="100%" 
                ShowDefaultButtons='False' ReportViewer="<%# ReportViewerReporte %>">
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
            
            <asp:Panel ID="pnlReporte" runat="server" ScrollBars="Both" >
            
                <dx:ReportViewer ID="ReportViewerReporte" Width="100%" runat="server" 
                    LoadingPanelText="Cargando">
                </dx:ReportViewer>
            
            </asp:Panel>
            
            <dx:ReportToolbar ID="ReportToolbar2" runat='server' Width="100%" 
                ShowDefaultButtons='False' ReportViewer="<%# ReportViewerReporte %>">
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
            
        </ContentTemplate>
    </asp:UpdatePanel>
            
</asp:Content>
