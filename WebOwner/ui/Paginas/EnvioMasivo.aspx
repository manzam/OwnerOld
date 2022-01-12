<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="EnvioMasivo.aspx.cs" Inherits="WebOwner.ui.Paginas.EnvioMasivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Envio masivo"></asp:Label>
        </h2>
    </div> 
    
    
    <asp:TextBox ID="txtEditor" runat="server" Width="300" Height="200" />
    <asp:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" TargetControlID="txtEditor">
    </asp:HtmlEditorExtender>
    
    
</asp:Content>
