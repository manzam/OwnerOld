<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="HistorialLiquidacionCliente.aspx.cs" Inherits="WebOwner.ui.Paginas.HistrialLiquidacionCliente" %>
<%@ Register src="../WebUserControls/WebUserHistorialLiquidacionCliente.ascx" tagname="WebUserHistorialLiquidacionCliente" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHistorialLiquidacion %>"></asp:Label>            
        </h2>
    </div>

    <uc1:WebUserHistorialLiquidacionCliente ID="WebUserHistorialLiquidacionCliente1" runat="server" />
                
</asp:Content>
