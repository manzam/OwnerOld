<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserNoticia.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserNoticia" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<script language="javascript" type="text/javascript">

    $j(document).ready(function() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargarCalendario);

        CargarCalendario(null, null);

        function CargarCalendario(sender, args) {
            $j("#<%= txtFecha.ClientID %>").datepicker({
                showOn: "button",
                buttonImage: "../../img/calendar.gif",
                dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy"
            });
        }
    });

    function uploadComplete(sender) {
        $get("<%=lblMesg.ClientID%>").innerHTML = "Archivo cargado exitosamente.";
    }

    function uploadError(sender) {
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
            <asp:Button ID="btnNuevo" runat="server" 
                Text="<%$ Resources:Resource, btnNuevo %>" ValidationGroup="Nuevo" onclick="btnNuevo_Click" />                                       
            <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="NuevoActualizar" onclick="btnGuardar_Click" />
            <asp:Button ID="btnActualizar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="Actualizar" onclick="btnActualizar_Click" />
            <asp:Button ID="btnVerTodos" runat="server" 
                Text="<%$ Resources:Resource, btnVerTodos %>" ValidationGroup="VerTodos" onclick="btnVerTodos_Click" />
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
        
        <div id="NuevoNoticia" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="3" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosNoticia %>"></asp:Label>                          
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:15%;" class="textoTabla">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblTitulo %>"></asp:Label>
                        </td>
                        <td style="width:85%;">
                            <asp:TextBox ID="txtTitulo" MaxLength="80" runat="server" Width="90%" ValidationGroup="NuevoActualizar">
                            </asp:TextBox>                           
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblFechaCaducidad %>"></asp:Label> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtFecha" runat="server" Width="100px" Enabled="false" ValidationGroup="NuevoActualizar">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtFecha" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblActivo %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbEstado" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblNoticia %>"></asp:Label>
                            <br />
                            [ jpeg, gif, png ]
                        </td>
                        <td>
                            <asp:AsyncFileUpload 
                                ID="AsyncFileUpload1" 
                                runat="server" 
                                PersistFile="True"
                                OnClientUploadError="uploadError"
                                OnClientUploadComplete="uploadComplete"
                                onuploadedcomplete="AsyncFileUpload1_UploadedComplete"  />
                            <asp:Label ID="lblMesg" runat="server" Text=""></asp:Label>                                
                        </td>
                    </tr>
                </tbody>
            </table>    
        </div>
        
        <div id="GrillaHotel" runat="server">
            
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, TituloGrillaNoticia %>"></asp:Label>                          
                </h2>
            </div>
            
            <asp:GridView ID="gvwNoticias" runat="server" 
                    AutoGenerateColumns="False" 
                    BorderStyle="Solid" BorderWidth="1px" BorderColor="#7599A9" 
                    CellPadding="2"
                    DataKeyNames="IdNoticia" 
                    EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                    OnSelectedIndexChanged="gvwNoticias_OnSelectedIndexChanged" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Resource, lblTitulo %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" 
                                    Text='<%# Bind("Titulo") %>'></asp:LinkButton>                                    
                            </ItemTemplate>
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                            <HeaderStyle  Width="70%"/>
                        </asp:TemplateField>  
                        <asp:BoundField DataField="FechaCaducidad" DataFormatString="{0:dd-MM-yyyy}" HeaderText="<%$ Resources:Resource, lblFechaCaducidad %>" >
                            <ItemStyle BorderColor="#7599A9" HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" />
                            <HeaderStyle  Width="10%"/>
                        </asp:BoundField>
                        <asp:ImageField headertext="IMAGEN" DataImageUrlField="Imagen">
                            <ControlStyle Width="70px" Height="70px" />
                            <ItemStyle HorizontalAlign="Center" verticalalign="Middle" />
                            <HeaderStyle  Width="20%"/>
                        </asp:ImageField>                        
                    </Columns>
                    <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                </asp:GridView>
        </div>
        
    </ContentTemplate>
</asp:UpdatePanel>
