<%@ Page Title="" EnableEventValidation="true" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ValorVariable.aspx.cs" Inherits="WebOwner.ui.Paginas.ValorVariable" %>
<%@ Register src="../WebUserControls/WebUserValorVariable.ascx" tagname="WebValorVariable" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblValorVariable %>"></asp:Label>
        </h2>
    </div>
    
    <uc1:WebValorVariable ID="WebValorVariable1" runat="server" />
    
</asp:Content>
