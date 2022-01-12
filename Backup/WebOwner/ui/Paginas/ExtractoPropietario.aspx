<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="WebUserExtractoPropietario.ascx.cs" Inherits="WebOwner.ui.Paginas.ExtractoPropietario" %>

<%@ Register src="../WebUserControls/WebUserExtractoPropietario.ascx" tagname="WebUserExtractoPropietario" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblEstracto %>"></asp:Label>            
        </h2>        
    </div>
    
    <uc1:WebUserExtractoPropietario ID="WebUserExtractoPropietario1" runat="server" />
    
</asp:Content>
