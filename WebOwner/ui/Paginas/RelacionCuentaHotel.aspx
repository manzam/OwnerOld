<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="RelacionCuentaHotel.aspx.cs" Inherits="WebOwner.ui.Paginas.RelacionCuentaHotel" %>

<%@ Register src="../WebUserControls/WebUserRelacionCuentaHotel.ascx" tagname="WebUserRelacionCuentaHotel" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloPrincipalRelacionContableHotel %>"></asp:Label>
        </h2>                
    </div> 
    
    <uc1:WebUserRelacionCuentaHotel ID="WebUserRelacionCuentaHotel1" runat="server" />
    
</asp:Content>
