<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplateLimpio.Master" AutoEventWireup="true" CodeBehind="OlvideClave.aspx.cs" Inherits="WebOwner.OlvideClave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
    
    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">
    
    <div>
        <div style="float:left; margin-right:10px;">
            <img src="img/65.png" alt="info" />
        </div>
        <div style="float:left;" id="lblInfo" runat="server">
            Su contraseña ha sido cambiada correctamente, su nueva contraseña sera su usuario. 
            <br />
            Inicia sesion de nuevo.
            <br />
            Gracias.
        </div>
    </div>
    
</asp:Content>
