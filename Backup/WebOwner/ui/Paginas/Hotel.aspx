<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="Hotel.aspx.cs" Inherits="WebOwner.Hotel" %>
<%@ Register src="../WebUserControls/WebUserHotel.ascx" tagname="WebUserHotel" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">
    
    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloPrincipalHotel %>"></asp:Label>
        </h2>
    </div>      
    
    <uc1:WebUserHotel ID="WebUserHotel1" runat="server" />
    
</asp:Content>
