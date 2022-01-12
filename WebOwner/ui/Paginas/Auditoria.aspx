<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Auditoria.aspx.cs" Inherits="WebOwner.ui.Paginas.Auditoria" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <script type="text/javascript">

        $j(document).ready(function() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

            CargasIniciales(null, null);

            function CargasIniciales(sender, args) {

                $j("#ctl00_Contenidoprincipal_txtFechaDesde").datepicker({
                    defaultDate: "+1w",
                    changeMonth: true,
                    changeYear: true,
                    buttonImageOnly: true,
                    numberOfMonths: 3,
                    showOn: "button",
                    buttonImage: "../../img/calendar.gif",
                    dateFormat: "yy-mm-dd",
                    onClose: function(selectedDate) {
                        $j("#ctl00_Contenidoprincipal_txtFechaHasta").datepicker("option", "minDate", selectedDate);
                    }
                });
                $j("#ctl00_Contenidoprincipal_txtFechaHasta").datepicker({
                    defaultDate: "+1w",
                    changeMonth: true,
                    changeYear: true,
                    buttonImageOnly: true,
                    numberOfMonths: 3,
                    showOn: "button",
                    buttonImage: "../../img/calendar.gif",
                    dateFormat: "yy-mm-dd",
                    onClose: function(selectedDate) {
                        $j("#ctl00_Contenidoprincipal_txtFechaDesde").datepicker("option", "maxDate", selectedDate);
                    }
                });

            }
        });
        
    </script> 


    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Auditoria"></asp:Label>
        </h2>
    </div>


    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
        <ProgressTemplate>
            <div class="procesar redondeo">Procesando..</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <table width="100%">
                <tr>
                    <td style="width:10%" class="textoTabla">
                        <asp:Label ID="Label13" runat="server" Text="Modulo"></asp:Label>
                    </td>
                    <td style="width:90%; text-align:left;">
                        <asp:DropDownList ID="ddlModulo" runat="server" AutoPostBack="true" Width="350px" 
                            onselectedindexchanged="ddlModulo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:10%" class="textoTabla">
                        <asp:Label ID="Label2" runat="server" Text="Campos"></asp:Label>
                    </td>
                    <td style="width:90%; text-align:left;">
                        <asp:DropDownList ID="ddlCampos" runat="server" Width="350px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:10%" class="textoTabla">
                        <asp:Label ID="Label3" runat="server" Text="Acción"></asp:Label>
                    </td>
                    <td style="width:90%; text-align:left;">
                        <asp:DropDownList ID="ddlAccion" runat="server" Width="350px">
                            <asp:ListItem Text="Todos" Value="T" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Insertar" Value="Insertar"></asp:ListItem>
                            <asp:ListItem Text="Actualizar" Value="Actualizar"></asp:ListItem>
                            <asp:ListItem Text="Eliminar" Value="Eliminar"></asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>                    
                    <td colspan="2">
                        <table width="80%">                            
                            <tr>
                                <td style="width:20%" class="textoTabla">Desde</td>
                                <td style="width:20%" align="center">
                                    <asp:TextBox ID="txtFechaDesde" CssClass="fechaDesde" Width="70px" Enabled="false" runat="server"></asp:TextBox>
                                </td>
                                <td style="width:20%" class="textoTabla">Hasta</td>
                                <td style="width:20%" align="center">
                                    <asp:TextBox ID="txtFechaHasta" CssClass="fechaHasta" Width="70px" Enabled="false" runat="server"></asp:TextBox>
                                </td>
                                <td style="width:20%" align="center">
                                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" 
                                        onclick="btnFiltrar_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>            
            
            <br />
            
            <asp:Panel ID="pnlReporte" runat="server" ScrollBars="Vertical">
            
                <dx:ReportToolbar ID="ReportToolbar_Auditoria" runat='server' Width="100%" 
                    ShowDefaultButtons='False' ReportViewer="<%# ReportViewer_Auditoria %>">
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
                
                <dx:ReportViewer ID="ReportViewer_Auditoria" runat="server">
                </dx:ReportViewer>
            
            </asp:Panel>
        
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
