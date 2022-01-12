<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="AsuntoCorreo.aspx.cs" Inherits="WebOwner.ui.Paginas.AsuntoCorreo" %>

<%@ Register src="../WebUserControls/WebUserAsuntoCorreo.ascx" tagname="WebUserAsuntoCorreo" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Asunto Correos"></asp:Label>
        </h2>
    </div> 
    
    <uc1:WebUserAsuntoCorreo ID="WebUserAsuntoCorreo1" runat="server" />

</asp:Content>
