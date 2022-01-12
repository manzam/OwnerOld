<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ReporteCertificado.aspx.cs" Inherits="WebOwner.ui.Paginas.ReporteCertificado" %>

<%@ Register src="../WebUserControls/WebUserCertificado.ascx" tagname="WebUserCertificado" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Certificado"></asp:Label>
        </h2>        
    </div>    

    <uc1:WebUserCertificado ID="WebUserCertificado1" runat="server" />
</asp:Content>
