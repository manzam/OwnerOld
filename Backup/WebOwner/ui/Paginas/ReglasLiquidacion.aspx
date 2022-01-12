<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ReglasLiquidacion.aspx.cs" Inherits="WebOwner.ui.Paginas.ReglasLiquidacion" %>
<%@ Register src="../WebUserControls/WebUserReglasLiquidacion.ascx" tagname="WebUserReglasLiquidacion" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">
    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloprincipalReglaLiquidacion %>"></asp:Label>        
        </h2>
        
        <uc1:WebUserReglasLiquidacion ID="WebUserReglasLiquidacion1" runat="server" />
    </div>  
</asp:Content>
