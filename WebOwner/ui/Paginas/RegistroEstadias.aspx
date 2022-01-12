<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="RegistroEstadias.aspx.cs" Inherits="WebOwner.ui.Paginas.RegistroEstadias" %>
<%@ Register src="../WebUserControls/WebUserRegistroEstadias.ascx" tagname="WebUserRegistroEstadias" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblRegistroEstadias %>"></asp:Label>
        </h2>
    </div>
    
    
    <uc1:WebUserRegistroEstadias ID="WebUserRegistroEstadias1" runat="server" />
</asp:Content>
