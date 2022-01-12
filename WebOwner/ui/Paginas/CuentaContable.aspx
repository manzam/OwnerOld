<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="CuentaContable.aspx.cs" Inherits="WebOwner.ui.Paginas.CuentaContable" %>
<%@ Register src="../WebUserControls/WebUserCuentaContable.ascx" tagname="WebUserCuentaContable" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblCuentaContable %>"></asp:Label>            
        </h2>
    </div> 
    
    <uc1:WebUserCuentaContable ID="WebUserCuentaContable1" runat="server" />
    
</asp:Content>
