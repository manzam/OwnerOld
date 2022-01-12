<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Certificado.aspx.cs" Inherits="WebOwner.ui.Paginas.Certificado" %>


<%@ Register src="../WebUserControls/WebUserConfigCertificado.ascx" tagname="WebUserConfigCertificado" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

<div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Configuracion Certificado"></asp:Label>            
        </h2>
    </div> 
    
    <uc1:WebUserConfigCertificado ID="WebUserConfigCertificado1" runat="server" />

</asp:Content>
