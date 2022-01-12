<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ConfiguracionExtracto.aspx.cs" Inherits="WebOwner.ui.Paginas.ConfiguracionExtracto" %>
<%@ Register src="../WebUserControls/WebUserConfigExtracto.ascx" tagname="WebUserConfigExtracto" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Configuración Extracto"></asp:Label>            
        </h2>        
    </div>
    
    <uc1:WebUserConfigExtracto ID="WebUserConfigExtracto" runat="server" />
    
</asp:Content>
