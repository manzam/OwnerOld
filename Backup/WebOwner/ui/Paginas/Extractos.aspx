<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Extractos.aspx.cs" Inherits="WebOwner.ui.Paginas.Estractos" %>

<%@ Register src="../WebUserControls/WebUserExtracto.ascx" tagname="WebUserEstracto" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblEstracto %>"></asp:Label>
        </h2>        
    </div>      
    
    <uc1:WebUserEstracto ID="WebUserEstracto1" runat="server" />
    
</asp:Content>
