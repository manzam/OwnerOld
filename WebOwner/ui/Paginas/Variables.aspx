<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Variables.aspx.cs" Inherits="WebOwner.ui.Paginas.Variables" %>
<%@ Register src="../WebUserControls/WebUserVariables.ascx" tagname="WebUserVariables" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloprincipalVariables %>"></asp:Label>
        </h2>
    </div> 
    
    <uc1:WebUserVariables ID="WebUserVariables1" runat="server" />
</asp:Content>
