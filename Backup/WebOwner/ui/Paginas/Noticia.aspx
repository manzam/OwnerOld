<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Noticia.aspx.cs" Inherits="WebOwner.ui.Paginas.Noticia" %>
<%@ Register src="../WebUserControls/WebUserNoticia.ascx" tagname="WebUserNoticia" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloPrincipalNoticia %>"></asp:Label>
        </h2>
    </div>  
    
    <uc1:WebUserNoticia ID="WebUserNoticia1" runat="server" />    
    
</asp:Content>
