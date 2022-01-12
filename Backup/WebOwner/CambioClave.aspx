<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplateLimpio.Master" AutoEventWireup="true" CodeBehind="CambioClave.aspx.cs" Inherits="WebOwner.ui.Paginas.CambioClave" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script src="js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>

<script type="text/javascript">

    function ValidarPass() {
        var esValido = true;
        $j('#capital,#number,#length,#compare,#titulo').hide();
        var pass = $j('#ctl00_Contenidoprincipal_txtClaveNueva').val();
        var conPass = $j('#ctl00_Contenidoprincipal_txtClaveConfirmar').val();

        if (pass.length < 8) {
            $j('#length,#titulo').show();
            esValido = esValido && false;
        } else {
            $j('#length,#titulo').hide();
            esValido = esValido && true;
        }

        //validate capital letter
        if (pass.match(/[A-Z]/)) {
            $j('#capital,#titulo').hide();
            esValido = esValido && true;
        } else {
            $j('#capital,#titulo').show();
            esValido = esValido && false;
        }

        //validate number
        if (pass.match(/\d/)) {
            $j('#number,#titulo').hide();
            esValido = esValido && true;
        } else {
            $j('#number,#titulo').show();
            esValido = esValido && false;
        }

        //Comparar Claves
        if (pass != conPass) {
            $j('#compare,#titulo').show();
            esValido = esValido && false;
        } else {
            $j('#compare,#titulo').hide();
            esValido = esValido && true;
        }

        return esValido;
    }

//    var $j = jQuery.noConflict();
//$j(document).ready(function() {

//    $j('input[type=password]').keyup(function() {
//        // set password variable
//        var pswd = $j(this).val();
//        //validate the length
//        if ( pswd.length < 8 ) {
//            $j('#length').removeClass('valid').addClass('invalid');
//        } else {
//            $j('#length').removeClass('invalid').addClass('valid');
//        }

//        //validate letter
//        if ( pswd.match(/[A-z]/) ) {
//            $j('#letter').removeClass('invalid').addClass('valid');
//        } else {
//            $j('#letter').removeClass('valid').addClass('invalid');
//        }

//        //validate capital letter
//        if ( pswd.match(/[A-Z]/) ) {
//            $j('#capital').removeClass('invalid').addClass('valid');
//        } else {
//            $j('#capital').removeClass('valid').addClass('invalid');
//        }

//        //validate number
//        if ( pswd.match(/\d/) ) {
//            $j('#number').removeClass('invalid').addClass('valid');
//        } else {
//            $j('#number').removeClass('valid').addClass('invalid');
//        }

//    }).focus(function() {
//        $j('#pswd_info').show();
//    }).blur(function() {
//        $j('#pswd_info').hide();
//    });

//});
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table style="width: 50%;">
            <tr>
                <td colspan="2">
                    <div runat="server" class="cuadradoExito" id="divExito" visible="false">
                        <asp:Image ID="imgExitoMsg" runat="server" ImageUrl="~/img/33.png" 
                            Width="20px" Height="20px" ImageAlign="AbsMiddle" />
                        <asp:Label ID="lbltextoExito" runat="server" CssClass="textoExito" ></asp:Label>
                    </div>
                    
                    <div runat="server" class="cuadradoError" id="divError" visible="false">
                        <asp:Image ID="imgErrorMsg" runat="server" ImageUrl="~/img/115.png" 
                            Width="20px" Height="20px" ImageAlign="AbsMiddle" />
                        <asp:Label ID="lbltextoError" runat="server" CssClass="textoError" ></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td style="width:25%; text-align: right;">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblClaveActual %>"></asp:Label>
                    <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidator3" 
                        ControlToValidate="txtClaveActual"
                        runat="server" 
                        Display="Dynamic" ValidationGroup="Aceptar"
                        ErrorMessage="*">
                    </asp:RequiredFieldValidator>
                </td>
                <td style="width:25%">
                    <asp:TextBox ID="txtClaveActual" ValidationGroup="Aceptar" runat="server" Width="70%" TextMode="Password"></asp:TextBox>                
                </td>
            </tr>
            <tr>
                <td style="width:25%; text-align: right;">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblClaveNueva %>"></asp:Label>
                    <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidator1" 
                        ControlToValidate="txtClaveNueva"
                        runat="server" 
                        Display="Dynamic" ValidationGroup="Aceptar"
                        ErrorMessage="*">
                    </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtClaveNueva" ValidationGroup="Aceptar" runat="server" Width="70%" TextMode="Password"></asp:TextBox>                
                </td>
            </tr>
            <tr>
                <td style="width:25%; text-align: right;">
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblClaveConfirmar %>"></asp:Label>
                    <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidator2" 
                        ControlToValidate="txtClaveConfirmar"
                        runat="server" ValidationGroup="Aceptar" 
                        Display="Dynamic"
                        ErrorMessage="*">
                    </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtClaveConfirmar" ValidationGroup="Aceptar" runat="server" Width="70%" TextMode="Password"></asp:TextBox>                
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <div id="pswd_info">
                       <h4 id="titulo" style="display:none">La clave nueva debe cumplir con los siguientes requerimientos:</h4>
                       <ul>
                          <li id="capital" style="display:none">Al menos debería tener <strong>una letra en mayúsculas</strong></li>
                          <li id="number" style="display:none">Al menos debería tener <strong>un número</strong></li>
                          <li id="length" style="display:none">Debería tener <strong>8 carácteres</strong> como mínimo</li>
                          <li id="compare" style="display:none">Clave nueva y Confirmar Clave <strong>deben ser iguales</strong></li>
                       </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center">
                    <asp:Button ID="btnAceptar" runat="server" 
                        Text="<%$ Resources:Resource, btnAceptar %>" OnClientClick="return ValidarPass();" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    &nbsp;&nbsp;
                    <asp:Button ID="Button1" runat="server" 
                        Text="Cancelar" onclick="btnCancelar_Click" ValidationGroup="Cancelar" />                    
                </td>
            </tr>
        </table>
    
    </ContentTemplate>
</asp:UpdatePanel>
    
</asp:Content>
