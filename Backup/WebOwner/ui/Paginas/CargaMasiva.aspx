<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="CargaMasiva.aspx.cs" Inherits="WebOwner.ui.Paginas.CargaMasiva" %>
<%@ Register src="../WebUserControls/WebUserCargaMasiva.ascx" tagname="WebUserCargaMasiva" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloPrincipalCargaMasiva %>"></asp:Label>
        </h2>
    </div>    
    
    <uc1:WebUserCargaMasiva ID="WebUserCargaMasiva1" runat="server" />
    
</asp:Content>