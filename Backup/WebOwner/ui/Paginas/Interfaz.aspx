<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Interfaz.aspx.cs" Inherits="WebOwner.ui.Paginas.Interfaz" %>
<%@ Register src="../WebUserControls/WebUserInterfaz.ascx" tagname="WebUserInterfaz" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblInterfaz %>"></asp:Label>            
        </h2>
    </div> 
    
    <uc1:WebUserInterfaz ID="WebUserInterfaz1" runat="server" />
    
</asp:Content>
