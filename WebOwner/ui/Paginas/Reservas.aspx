<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Reservas.aspx.cs" Inherits="WebOwner.ui.Paginas.Reservas" %>
<%@ Register src="../WebUserControls/WebUserReserva.ascx" tagname="WebUserReserva" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloPrincipalReservaciones %>"></asp:Label>
        </h2>        
    </div>
    
    <uc1:WebUserReserva ID="WebUserReserva1" runat="server" />
    
</asp:Content>
