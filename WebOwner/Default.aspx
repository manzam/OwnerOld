<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebOwner.ui.Paginas.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inicio - Estelar</title>
    
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="description" content="Simple Template #2 from simpletemplates.org" />
    <meta name="keywords" content="simple #2, template, simpletemplates.org" />
    
    <link rel="stylesheet" type="text/css" href="css/style.css" />    
    
</head>
<body>

    <form id="form" runat="server">
        
    <div>
        <div id="top">
            <div id="header">
                <div style="float:left;margin-right:15px;">
                    <asp:Image ID="Image1" ImageUrl="~/img/logo2.png" Width="80px" Height="90px" runat="server" />
                </div>
                <div style="float:left;">
                    <h1>Owners</h1>
                </div>
            </div>
            <div id="topmenu">
                <ul>
                    <li>
                      <asp:HyperLink ID="HyperLink3" NavigateUrl="~/SesionPropietario.aspx" runat="server" Text="<%$ Resources:Resource, lblSesion %>"></asp:HyperLink>
                    </li>
                </ul>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="contentwrap">
                    
            <div class="clear">
            </div>
            
        </div>
        <div id="footer">
            <div class="left">
                Copyright &copy; <% Response.Write(DateTime.Now.Year.ToString()); %> by <a target="_blank" href="http://www.stt-solutions.com/">Softtronics S.A.</a>
            </div>
            <div class="right">
                
                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/SesionAdmin.aspx" runat="server" Text="Admin"></asp:HyperLink>
                &nbsp;
                Design by <a target="_blank" href="http://www.simpletemplates.org">Simple Website Templates</a>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    
    </form>
</body>
</html>
