<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SesionPropietario.aspx.cs" Inherits="WebOwner.SesionPropietario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inicio Sesion</title>
    
    <script src="js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function ValidarUsuario() {
            $("#loginOwner_txtUsuario").val($("#loginOwner_UserName").val().trim());
            
            if ($("#loginOwner_UserName").val().trim() == "") {
                $("#lblRespuesta").text('Debes escribir el Usuario.'); 
                return false;                
            }
            else {
                $("#lblRespuesta").text('');                 
                return true;
            }
        }
    </script>
    
    <style type="text/css">
        .login
        {
            border: 1px solid #333333;
            display: block;
            position: absolute;
            top: 20%;
            left: 30%;
            width: 500px;
            padding:2px;
        }

        .loginLogo
        {
            position:absolute;
            left:0; 
            top:0;
        }
        .procesar
        {
            position: absolute;
            bottom: 0;
            right: 5px;
            background-color: #fffaae;
            color: #585858;
            background-image: url('../img/ajax-loader.gif');
            background-repeat: no-repeat;
            padding: 15px 10px 15px 50px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 16px;
            border: 1px solid #585858;
        }

        .redondeo
        {
            -webkit-border-radius: 7px;
            -moz-border-radius: 7px;
            border-radius: 7px;
            border: 1px solid #7599a9;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
        <div class="login">
        
            <table>
                <tr>
                    <td style="background-color:#00244C; vertical-align:middle; padding-bottom:5px;">
                        <div style="margin-right:15px;text-align:left;padding-left:10px;">
                            <asp:Image ID="Image1" ImageUrl="~/img/logo2.png" Width="80px" Height="90px" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:bottom;">
                            
                            <asp:login runat="server"  
                                ID="loginOwner"  
                                Width="100%"  
                                Height="150px"
                                BackColor="White"                                           
                                onauthenticate="loginOwner_Authenticate"                
                                FailureText="<%$ Resources:Resource, lblMensajeError_5 %>" 
                                DisplayRememberMe="False" TitleText="Iniciar sesión Propietario">
                                <LayoutTemplate>
                                    <table border="0" cellpadding="1" cellspacing="0" 
                                        style="border-collapse:collapse;">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" style="height:150px;width:100%;">
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            Iniciar sesión Propietario</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuario:</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                                ControlToValidate="UserName" 
                                                                ErrorMessage="El nombre de usuario es obligatorio." 
                                                                ToolTip="El nombre de usuario es obligatorio." ValidationGroup="loginOwner">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Contraseña:</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                                ControlToValidate="Password" ErrorMessage="La contraseña es obligatoria." 
                                                                ToolTip="La contraseña es obligatoria." ValidationGroup="loginOwner">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2" style="color:Red;">
                                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" 
                                                                Text="Inicio de sesión" ValidationGroup="loginOwner" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                            </asp:login>    
                        
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
                                <ProgressTemplate>
                                    <div class="procesar redondeo">Procesando..</div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    
                                    <div style="text-align:right;">
                                        <asp:LinkButton ID="LinkButton1" OnClientClick="return ValidarUsuario();"  
                                        runat="server" onclick="lbOlvideContrasena_Click">Olvide mi contraseña.</asp:LinkButton>
                                    </div>
                                    
                                    <br />
                                    
                                    <asp:Label ID="lblRespuesta" runat="server" Text="" ></asp:Label>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="http://www.stt-solutions.com/" target="_blank">
                            <asp:Image ImageUrl="~/img/bg_cabezote.jpg" Width="100%" runat="server" />
                        </a>
                    </td>
                </tr>
            </table>
                 
        </div>  
    
    </form>
</body>
</html>
