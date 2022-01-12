<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Propietario.aspx.cs" Inherits="WebOwner.ui.Paginas.Propietario" %>
<%@ Register src="../WebUserControls/WebUserPropietario.ascx" tagname="WebUserPropietario" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloprincipalPropietario %>"></asp:Label>
        </h2>
    </div> 
    
    
    <uc1:WebUserPropietario ID="WebUserPropietario1" runat="server" />
    
</asp:Content>
