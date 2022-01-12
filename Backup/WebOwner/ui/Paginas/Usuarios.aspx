<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="WebOwner.ui.Paginas.Usuarios" %>

<%@ Register src="../WebUserControls/WebUsers.ascx" tagname="WebUsers" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloprincipalUsuario %>"></asp:Label>            
        </h2>
    </div>  
    
    <uc1:WebUsers ID="WebUsers1" runat="server" />
    
</asp:Content>
