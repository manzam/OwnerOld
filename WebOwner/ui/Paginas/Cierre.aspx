<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Cierre.aspx.cs" Inherits="WebOwner.ui.Paginas.Cierre" %>
<%@ MasterType VirtualPath="~/templates/TemplatePrincipal.Master" %>  
<%@ Register src="../WebUserControls/WebUserCierre.ascx" tagname="WebUserCierre" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">


    <div class="tituloPrincipal">        
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblCierreLiquidacion %>"></asp:Label>            
        </h2>
    </div>
    
    <uc1:WebUserCierre ID="WebUserCierre1" runat="server" />
    

</asp:Content>
