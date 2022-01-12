<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="PerfilPermisos.aspx.cs" Inherits="WebOwner.ui.Paginas.PerfilPermisos" %>
<%@ Register src="../WebUserControls/WebUserPerfilPermisos.ascx" tagname="WebUserPerfilPermisos" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloPrincipalPerfilPermiso %>"></asp:Label>
        </h2>
    </div>  

    <uc1:WebUserPerfilPermisos ID="WebUserPerfilPermisos1" runat="server" />
</asp:Content>
