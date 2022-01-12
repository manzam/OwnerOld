<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="PerfilAdmon.aspx.cs" Inherits="WebOwner.PerfilAdmon" %>
<%@ Register src="ui/WebUserControls/WebUserMisNoticias.ascx" tagname="WebUserMisNoticias" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">
    <uc1:WebUserMisNoticias ID="WebUserMisNoticias1" runat="server" />
</asp:Content>
