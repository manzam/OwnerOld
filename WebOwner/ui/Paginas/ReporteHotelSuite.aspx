<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ReporteHotelSuite.aspx.cs" Inherits="WebOwner.ui.Paginas.ReporteHotelSuite" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>
    
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
                            <asp:ListItem Text="Hotel - Suite" Value="dos" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Variables" Value="tres"></asp:ListItem>
                            <asp:ListItem Text="Usuarios" Value="cuatro"></asp:ListItem>
                            <asp:ListItem Text="Perfil - Permisos" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Historial Liquidacion" Value="H"></asp:ListItem>                            
                            <asp:ListItem Text="Consolidado por Suite" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Consolidado por Propietario" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Extracto Consolidado" Value="EC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <br />
            
            <div class="tituloPrincipal">
                <h2>
                    <asp:Label ID="Label1" runat="server" Text="Hotel - Suite"></asp:Label>
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
                    <td class="textoTabla" style="width:10%" valign="top">Hotel</td>
                    <td style="width:90%">
                        <asp:DropDownList ID="ddlHotelSuite" runat="server" Width="90%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="width:10%" valign="top">N° Suite</td>
                    <td style="width:90%">
                        <asp:TextBox ID="txtNumSuite" runat="server" Width="10%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="btnAceptarDos" runat="server" Text="Aceptar" 
                            onclick="btnAceptarDos_Click" />
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
