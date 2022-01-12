<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="WebOwner.ui.Paginas.Reporte" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


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
                            <asp:ListItem Text="Seleccione" Value="S" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Propietarios - Suite" Value="uno"></asp:ListItem>
                            <asp:ListItem Text="Hotel - Suite" Value="dos"></asp:ListItem>
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
            
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
