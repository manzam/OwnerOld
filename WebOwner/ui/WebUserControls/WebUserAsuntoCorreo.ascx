<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserAsuntoCorreo.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserAsuntoCorreo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>    
<%@ Register src="WebUserBuscadorSuite.ascx" tagname="WebUserBuscadorSuite" tagprefix="uc1" %>


<script src="../../js/fancyapps-fancyBox-18d1712/lib/jquery.mousewheel-3.0.6.pack.js"
    type="text/javascript"></script>
<link href="../../js/fancyapps-fancyBox-18d1712/source/jquery.fancybox.css" rel="stylesheet"
    type="text/css" />
<script src="../../js/fancyapps-fancyBox-18d1712/source/jquery.fancybox.pack.js"
    type="text/javascript"></script>
    
<script type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j("input[id$='txtFecha']").spinner({
                min: 2000,
                max: y
            });

            $j(".fancybox").fancybox();
        }

    });

    function uploadComplete(sender, args) {

        var fileName = args.get_fileName();
        // var ext = fileName.split(".")[1];
        var ext = fileName.substring(fileName.lastIndexOf(".") + 1);

        if (ext == "png" || ext == "jpeg" || ext == "pdf" || ext == "xlsx" || ext == "xls" || ext == "xltx" || ext == "xlt" || ext == "docx" || ext == "doc") {
            $get("<%=lblMesg.ClientID%>").innerHTML = "Archivo cargado exitosamente.";
        }
        else {
            $("#ctl00_Contenidoprincipal_WebUserAsuntoCorreo1_AsyncFileUpload1_ctl02").css("background-color", "red");            
            $get("<%=lblMesg.ClientID%>").innerHTML = "<span style='color:red;'>Este tipo de archivo no es valido.</span>";
        }
    }

    function uploadError(sender, args) {
        $get("<%=lblMesg.ClientID%>").innerHTML = "El archivo no se pudo cargar con éxito.";
    }
</script>



<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
        <div class="botonera">            
            <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" ValidationGroup="NuevoActualizar" onclick="btnGuardar_Click" />
        </div>       
        
        <br />
    
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
        
        
        <table width="100%" cellpadding="3" cellspacing="0">
            <tr>
                <td style="width:10%;" class="textoTabla">Hotel</td>
                <td style="width:90%;">
                    <asp:DropDownList ID="ddlHotel" Width="50%" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="textoTabla">Fecha</td>
                <td>
                    <asp:DropDownList ID="ddlMes" Width="110px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMes_OnSelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Enero" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                        <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                        <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                        <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                        <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                        <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                        <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                        <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                        <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                        <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                    </asp:DropDownList>
                    -
                    <asp:TextBox ID="txtFecha" Enabled="false" runat="server" Width="40px"></asp:TextBox> 
                </td>
                <td>                    
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td style="width:10%;" class="textoTabla" colspan="2">Texto infomativo</td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtAsunto" TextMode="MultiLine" Width="95%" Height="250px" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtAsunto" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" ></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td class="textoTabla" style="width:80%;">Lista Adjuntos</td>
                            <td class="textoTabla">Correos Informativos</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:AsyncFileUpload 
                                    ID="AsyncFileUpload1"                                 
                                    runat="server" 
                                    PersistFile="true"
                                    OnClientUploadError="uploadError"
                                    OnClientUploadComplete="uploadComplete"
                                    onuploadedcomplete="AsyncFileUpload1_UploadedComplete" />                               
                                            
                                <asp:Label ID="lblMesg" runat="server" Text=""></asp:Label>
                                <asp:Image ID="imgLogo" runat="server" Width="150px" Height="80px" Visible="false" />
                            </td>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:AsyncFileUpload 
                                                ID="AsyncFileUpload2"                                 
                                                runat="server" 
                                                PersistFile="true"
                                                OnClientUploadError="uploadError"
                                                OnClientUploadComplete="uploadComplete"
                                                onuploadedcomplete="AsyncFileUpload2_UploadedComplete" />                               
                                            
                                           <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>                                        
                                            <asp:TextBox ID="txtCuerpoInfo" TextMode="MultiLine" Width="95%" Height="250px" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnEnviarInfo" runat="server" Text="Enviar Información" ValidationGroup="EnviarInfo" onclick="btnEnviarInfo_Click" />
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                    </table>
                </td>                
                
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView 
                        ID="gvwAdjuntos" 
                        runat="server" 
                        DataKeyNames="IdAsuntoAdjunto" 
                        Width="50%" 
                        AutoGenerateColumns="False" 
                        ellPadding="2" 
                        PageSize="15"
                        OnRowDataBound="gvwAdjuntos_OnRowDataBound"
                        EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                        BorderColor="#7599A9" 
                        BorderStyle="Solid" 
                        BorderWidth="1px">
                        <Columns>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:MM-yyyy}">
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                <HeaderStyle Width="20%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Archivo">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" Text="Ver"  CommandArgument='<%# Eval("AdjuntoRuta") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>                                
                                </ItemTemplate>
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                <HeaderStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Archivo">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgButton" ImageUrl="~/img/126.png" runat="server" CommandArgument='<%# Eval("IdAsuntoAdjunto") %>' OnClick="RemoveClick" />
                                </ItemTemplate>
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                <HeaderStyle Width="30%" />
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle ForeColor="White" BackColor="#4D606E" />
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    
    </ContentTemplate>
</asp:UpdatePanel>    