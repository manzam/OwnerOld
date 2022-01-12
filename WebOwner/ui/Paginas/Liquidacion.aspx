<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Liquidacion.aspx.cs" Inherits="WebOwner.ui.Paginas.Liquidacion" %>
<%@ Register src="../WebUserControls/WebUserLiquidacion.ascx" tagname="WebUserLiquidacion" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%= ResolveClientUrl("~/js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveClientUrl("~/js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js") %>' type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblLiquidacion %>"></asp:Label>            
        </h2>
    </div>
    
    <uc1:WebUserLiquidacion ID="WebUserLiquidacion1" runat="server" />
    
</asp:Content>
