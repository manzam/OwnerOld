<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserInformacionEstadistica.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserInformacionEstadistica" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<script type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j(".fecha").spinner({
                min: 2000,
                max: y
            });

            $j(".fecha").val(y);

            var str = $j("#ctl00_Contenidoprincipal_WebUserInformacionEstadistica1_lbltextoError").text();
            $j("#ctl00_Contenidoprincipal_WebUserInformacionEstadistica1_lbltextoError").html(str);
        }
    });
        
    </script>  

<div style="display:none;">
    <asp:Button ID="btnDescarPlano" runat="server" Text="Descargar Plantilla" ValidationGroup="DescarPlano"  onclick="btnDescarPlano_Click" />
</div>

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
            <asp:Button ID="btnCancelar" runat="server" 
                Text="<%$ Resources:Resource, btnCancelar %>" Visible="false" 
                ValidationGroup="Cancelar" onclick="btnCancelar_Click" />
            <asp:Button ID="btnClickDescarPlano" runat="server" Text="Descargar Plantilla" ValidationGroup="DescarPlano" OnClientClick="$j('#ctl00_Contenidoprincipal_WebUserInformacionEstadistica1_btnDescarPlano').click();" />
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
        
        <br />
    
        <table width="100%">
            <tr>
                <td colspan="5">
                    <asp:Label ID="Label8" runat="server" Text="Use punto (.) para poner el valor en decimales."></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="textoTabla" style="width:10%;">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                </td>
                <td style="width:90%;">
                    <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" Width="450px" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    Cargar
                </td>
                <td>
                    <div style="width:35%;float:left;">
                        <asp:AsyncFileUpload ID="AsyncFileUpload" Width="95%" runat="server" ThrobberID="ImgLoader" PersistFile="true" />
                    </div>
                    <div style="width:10%;float:left;">
                        <asp:Button ID="btnCargar" runat="server" Text="Cargar" 
                            ValidationGroup="DescarPlano" onclick="btnCargar_Click" />
                    </div>
                    <div style="clear:both"></div>
                    <asp:Image ID="ImgLoader" ImageUrl="~/img/ajax-loader.gif" runat="server" />
                </td>
            </tr>
        </table>

        <br />
        
        <asp:Panel ID="pnlNuevo" runat="server" Visible="false">
            <table width="100%">
                <tr>
                    <td style="width:10%;" class="textoTabla">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblNombreSolo %>"></asp:Label> *
                    </td>
                    <td style="width:90%;">
                        <asp:TextBox ID="txtNombre" runat="server" Width="90%"></asp:TextBox>
                        <asp:RequiredFieldValidator 
                            ID="RequiredFieldValidator1" 
                            runat="server" Text="*" ValidationGroup="NuevoActualizar" 
                            ControlToValidate="txtNombre"
                            ErrorMessage="*"> 
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label> *
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMes" Width="110px" runat="server">
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
                        <asp:TextBox ID="txtFecha" CssClass="fecha" Enabled="false" runat="server" Width="40px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                            ControlToValidate="txtFecha" Display="Dynamic" CssClass="error" >
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">Variable acumulada</td>
                    <td>
                        <asp:CheckBox ID="cbVariableAcumular" AutoPostBack="true" runat="server" 
                            oncheckedchanged="cbVariableAcumular_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblValor %>"></asp:Label> *
                    </td>
                    <td>
                        <asp:TextBox ID="txtValor" runat="server" Width="20%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtValor_FilteredTextBoxExtender" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" 
                            runat="server" Enabled="True" TargetControlID="txtValor">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator 
                            ID="RequiredFieldValidator3" 
                            runat="server" Text="*" ValidationGroup="NuevoActualizar" 
                            ControlToValidate="txtValor"
                            ErrorMessage="*"> </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">Sufijo</td>
                    <td>
                        <asp:DropDownList ID="ddlSufijo" runat="server">
                            <asp:ListItem Text="Ninguno" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="%" Value="%"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">Valor acumulado</td>
                    <td>
                        <asp:DropDownList ID="ddlValorAcumulado" runat="server">
                            <asp:ListItem Text="Valor en cero (0)" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Ultimo valor del mes" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">Orden Extracto</td>
                    <td>
                        <asp:TextBox ID="txtOrden" runat="server" Width="20%" Text="0"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" 
                            runat="server" Enabled="True" TargetControlID="txtOrden">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator 
                            ID="RequiredFieldValidator5" 
                            runat="server" Text="*" ValidationGroup="NuevoActualizar" 
                            ControlToValidate="txtOrden"
                            ErrorMessage="*"> </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    
        <asp:Panel ID="pnlGrilla" runat="server">
        
            <asp:GridView 
                ID="gvwInformacionEstadistica" 
                runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                BorderColor="#7599A9" 
                CellPadding="2"
                DataKeyNames="IdInformacionEstadistica,EsAcumulada" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                Width="100%"
                onselectedindexchanged="gvwInformacionEstadistica_SelectedIndexChanged" 
                onrowcancelingedit="gvwInformacionEstadistica_RowCancelingEdit" 
                onrowediting="gvwInformacionEstadistica_RowEditing" 
                onrowupdating="gvwInformacionEstadistica_RowUpdating">
                <Columns>
                    <asp:CommandField ButtonType="Image" 
                                      ShowSelectButton="true"
                                      ShowEditButton="true"
                                      HeaderText="<%$ Resources:Resource, lblAccion %>" 
                                      SelectImageUrl="~/img/23.png"
                                      UpdateImageUrl="~/img/131.png"
                                      DeleteImageUrl="~/img/126.png"
                                      CancelImageUrl="~/img/72.png"
                                      EditImageUrl="~/img/13.png" >
                        <ControlStyle Height="30px" Width="30px" />
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNombreVariable" runat="server" Text='<%# Bind("Nombre") %>' Width="95%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        <HeaderStyle Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acumulada">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" Checked='<%# Bind("EsAcumulada") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cbEsAcumulada" runat="server" Checked='<%# Bind("EsAcumulada") %>' />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Orden Extracto">
                        <ItemTemplate>
                            <asp:Label ID="Label30" runat="server" Text='<%# Bind("Orden") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtOrden" runat="server" Text='<%# Bind("Orden") %>' Width="95%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtOrden_FilteredTextBoxExtender" FilterMode="ValidChars" FilterType="Custom" ValidChars="0123456789" 
                                runat="server" Enabled="True" TargetControlID="txtOrden">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator 
                                ID="RequiredFieldValidator40" 
                                runat="server"
                                Text="*" ControlToValidate="txtOrden" 
                                ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Sufijo">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlSufijo" runat="server" Enabled="false" SelectedValue='<%# Bind("Sufijo") %>'>
                                <asp:ListItem Text="Ninguno" Value="-1" Selected="True" ></asp:ListItem>
                                <asp:ListItem Text="%" Value="%"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlSufijo" runat="server" SelectedValue='<%# Bind("Sufijo") %>'>
                                <asp:ListItem Text="Ninguno" Value="-1" Selected="True" ></asp:ListItem>
                                <asp:ListItem Text="%" Value="%"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblFecha %>" >
                        <ItemTemplate>
                            <asp:TextBox ID="txtF" Enabled="false" runat="server" Width="75px"></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlMes" Width="110px" runat="server">
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
                            <asp:TextBox ID="txtFecha" CssClass="fecha" Enabled="false" runat="server" Width="40px"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="15%" HorizontalAlign="Center" />
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblValor %>" >
                        <ItemTemplate>
                            <asp:TextBox ID="txtV" Enabled="false" runat="server" Width="75px"></asp:TextBox>                            
                        </ItemTemplate>
                        <EditItemTemplate>                            
                            <asp:TextBox ID="txtValor" runat="server" Width="75px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtValor_FilteredTextBoxExtender" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789," 
                                runat="server" Enabled="True" TargetControlID="txtValor">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator 
                                ID="RequiredFieldValidator4" 
                                runat="server"
                                Text="*" ControlToValidate="txtValor" 
                                ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="10%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Valor acumulado" >
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlValorAcumulado" runat="server" Enabled="false" SelectedValue='<%# Bind("ValorAcumulado") %>'>
                                <asp:ListItem Text="Valor en cero (0)" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Ultimo valor del mes" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>                            
                            <asp:DropDownList ID="ddlValorAcumulado" runat="server" SelectedValue='<%# Bind("ValorAcumulado") %>'>
                                <asp:ListItem Text="Valor en cero (0)" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Ultimo valor del mes" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="15%" HorizontalAlign="Center" />
                    </asp:TemplateField>                   
                    
                    
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" CommandArgument='<%# Bind("IdInformacionEstadistica") %>' ImageUrl="~/img/126.png" runat="server" 
                                onclick="imgBtnEliminar_Click" />
                            <asp:ConfirmButtonExtender ID="imgBtnEliminar_ConfirmButtonExtender" 
                                runat="server" ConfirmText="Se eliminara la información estadistica y todo su historial, esta seguro?" Enabled="True" TargetControlID="imgBtnEliminar">
                            </asp:ConfirmButtonExtender>
                        </ItemTemplate>   
                        <ControlStyle Height="30px" Width="30px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
            
            <br />
            
            <h2>
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblHistorial %>"></asp:Label>
            </h2>
            
            <table width="60%">
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblAno %>"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="ddlAno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>                
                        <asp:GridView 
                            ID="gvwDetalleVariable" 
                            runat="server" 
                            AutoGenerateColumns="False" 
                            DataKeyNames="IdHistorialInformacionEstadistica"
                            BorderStyle="Solid" 
                            BorderWidth="1px" 
                            BorderColor="#7599A9" 
                            CellPadding="2"
                            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                            Width="100%" 
                            onrowdeleting="gvwDetalleVariable_RowDeleting">
                            <Columns>    
                                <asp:CommandField ButtonType="Image" 
                                          ShowDeleteButton="true"
                                          HeaderText="<%$ Resources:Resource, lblAccion %>"
                                          DeleteImageUrl="~/img/126.png" >
                                    <ControlStyle Height="30px" Width="30px" />
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                </asp:CommandField>            
                                <asp:BoundField HeaderText="<%$ Resources:Resource, lblValor %>" DataField="Valor" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="35%" HorizontalAlign="Right" />
                                </asp:BoundField >
                                <asp:BoundField HeaderText="<%$ Resources:Resource, lblFecha %>" DataField="Fecha" DataFormatString="{0: MM-yyyy}" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="35%" HorizontalAlign="Center" />
                                </asp:BoundField >                    
                            </Columns>
                            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                        </asp:GridView>
                    </td>
                </tr>                    
            
            </table>
        
        </asp:Panel>
        
    </ContentTemplate>
</asp:UpdatePanel>
