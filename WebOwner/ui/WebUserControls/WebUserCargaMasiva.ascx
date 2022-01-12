<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserCargaMasiva.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserCargaMasiva" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type = "text/javascript">
    function uploadComplete(sender) {
        $j("#ctl00_Contenidoprincipal_WebUserCargaMasiva1_ddlTabla").prop('disabled', true);
    }

    function uploadError(sender) {
    }
</script>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upMensajes" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>
    
<asp:UpdatePanel ID="upMensajes" runat="server">
    <ContentTemplate>
        <div runat="server" class="cuadradoExito" id="divExito" >
            <asp:Image ID="imgExitoMsg" runat="server" ImageUrl="~/img/33.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoExito" runat="server" CssClass="textoExito" ></asp:Label>
        </div>
        
        <div runat="server" class="cuadradoError" id="divError" visible="false">
            <asp:Image ID="imgErrorMsg" runat="server" ImageUrl="~/img/115.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoError" runat="server" CssClass="textoError" ></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td style="width:5%; text-align:left;" class="textoTabla">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblArchivo %>"></asp:Label>
                    </td>
                    <td colspan="2" style="width:10%; text-align:left;">                        
                        <asp:DropDownList ID="ddlTabla" runat="server" Width="150px" 
                            AutoPostBack="true" onselectedindexchanged="ddlTabla_SelectedIndexChanged" >
                            <asp:ListItem Text="<%$ Resources:Resource, lblHotelSuite %>" Value="HS"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Resource, TituloprincipalPropietarioCliente %>" Value="P"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Resource, TituloPrincipalVariablesSuit %>" Value="VS"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Resource, lblVariablesHotel %>" Value="VH"></asp:ListItem>
                            <asp:ListItem Text="Informacion Certificado" Value="IC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblEstructura %>"></asp:Label>
                    </td>
                    <td style="width:85%; text-align:left;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblInfoEstructura" runat="server" Font-Italic="True"  
                                    Font-Bold="True" Font-Size="X-Small"></asp:Label>
                            </ContentTemplate>                            
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTabla" 
                                    EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width:5%; text-align:left;" class="textoTabla">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblArchivo %>"></asp:Label>
                    </td>
                    <td colspan="2" style="width:95%; text-align:left;">
                        <asp:AsyncFileUpload ID="AsyncFileUpload" 
                            runat="server"
                            Width="90%" PersistFile="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="width:70%; text-align:left;">                        
                        
                        <asp:UpdatePanel ID="upCargue" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                                    
                                <asp:Button ID="btnCargar" runat="server" 
                                    Text="<%$ Resources:Resource, btnCargarArchivo %>" 
                                    onclick="btnCargar_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelar" runat="server" 
                                    Text="<%$ Resources:Resource, btnCancelar %>" 
                                    onclick="btnCancelar_Click"  />
                                
                                <br /><br />
                                
                                <asp:Panel ID="pnlErrores" runat="server">
                                </asp:Panel>
                                <%--<asp:Label ID="lblEstado" CssClass="" runat="server" Visible="false"></asp:Label>--%>
                                <asp:Label ID="lblEstadoExito" CssClass="cuadradoExito" runat="server" Visible="false"></asp:Label>
                                <asp:Button ID="btnAceptar" runat="server" Visible="false" 
                                    Text="<%$ Resources:Resource, btnAceptar %>" onclick="btnAceptar_Click" />
                                
                                <br />
                                
                                <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="upCargue" runat="server">
                                    <ProgressTemplate>
                                        <div class="loader" style="width:70%; height:30%;">
                                            <div style="width:800px; height:50%; position:absolute; left:50%; top:50%;">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/ajax-loader.gif" />
                                                <p>
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblCargando %>"></asp:Label>
                                                </p>                                
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            
