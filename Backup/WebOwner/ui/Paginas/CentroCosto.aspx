<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="CentroCosto.aspx.cs" Inherits="WebOwner.ui.Paginas.CentroCosto" %>
<%@ Register src="../WebUserControls/WebUserCentroCosto.ascx" tagname="WebUserCentroCosto" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblCentroCosto %>"></asp:Label>            
        </h2>
    </div>
    
    <uc1:WebUserCentroCosto ID="WebUserCentroCosto1" runat="server" />
    
</asp:Content>
