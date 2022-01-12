<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="InformacionEstadistica.aspx.cs" Inherits="WebOwner.ui.Paginas.InformacionEstadistica" %>
<%@ Register src="../WebUserControls/WebUserInformacionEstadistica.ascx" tagname="WebUserInformacionEstadistica" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblInfomacionEstadistica %>"></asp:Label>            
        </h2>
    </div>
    
    <uc1:WebUserInformacionEstadistica ID="WebUserInformacionEstadistica1" runat="server" />
    
</asp:Content>
