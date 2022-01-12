<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="PropietarioCliente.aspx.cs" Inherits="WebOwner.ui.Paginas.PropietarioCliente" %>
<%@ Register src="../WebUserControls/WebUserPropietarioCliente.ascx" tagname="WebUserPropietarioCliente" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">
    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloprincipalPropietarioCliente %>"></asp:Label>
        </h2>
    </div>  

    <uc1:WebUserPropietarioCliente ID="WebUserPropietarioCliente1" runat="server" />
</asp:Content>
