<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserPropietario.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserPropietario" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>    
<%@ Register src="WebUserBuscadorPropietario.ascx" tagname="WebUserBuscadorPropietario" tagprefix="uc1" %>
<script src="../../js/variablePropietario.js?v=00007"></script>
    
<%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>   --%> 
    
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>   --%> 

<asp:HiddenField ID="hiddenIdSuitPropietarioSeleccionado" runat="server" />
<asp:HiddenField ID="hiddenIdSuitSeleccionado" runat="server" />
<asp:HiddenField ID="hiddenIdUsuario" runat="server" />
<asp:HiddenField ID="HiddenIdPropietario" runat="server" />
    
        <div class="botonera">
            
            <asp:Button ID="btnNuevo" runat="server" 
                Text="<%$ Resources:Resource, btnNuevo %>" ValidationGroup="Nuevo" onclick="btnNuevo_Click" />                               

            <input type="button" value="Guardar" onclick="GuardarPropietario();" id="btnguardar" class="ui-button ui-widget ui-state-default ui-corner-all" />


            <%--<asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="NuevoActualizar" onclick="btnGuardar_Click" />
            <asp:Button ID="btnActualizar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="Actualizar" onclick="btnActualizar_Click" />--%>
            <asp:Button ID="btnVerTodos" runat="server" 
                Text="<%$ Resources:Resource, btnVerTodos %>" ValidationGroup="VerTodos" onclick="btnVerTodos_Click" />
            <asp:Button ID="btnBuscar" runat="server" 
                Text="<%$ Resources:Resource, btnBuscar %>" 
                OnClientClick="$j('#modalBuscadorPropietario').dialog('open')" 
                onclick="btnBuscar_Click" />
            <asp:Button ID="btnEliminar" runat="server" 
                Text="Eliminar" Visible="false" onclick="btnEliminar_Click" />

            
        </div>       
        
        <br />
        
        <div class="cuadradoExito" id="divExito" style="display:none">
            <asp:Image ID="imgExitoMsg" runat="server" ImageUrl="~/img/33.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoExito" runat="server" CssClass="textoExito" ></asp:Label>
        </div>
        
        <div class="cuadradoError" id="divError" style="display:none">
            <asp:Image ID="imgErrorMsg" runat="server" ImageUrl="~/img/115.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoError" runat="server" CssClass="textoError" ></asp:Label>
        </div>
        
        <div id="NuevoHotel" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="4" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosPropietario %>"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width: 15%;" class="textoTabla">
                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, lblPerfil %>"></asp:Label>
                        </td>
                        <td style="width: 35%;">
                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, TituloprincipalPropietarioCliente %>"></asp:Label>
                        </td>
                        <td style="width: 15%;" class="textoTabla">
                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, lblActivo %>"></asp:Label>
                        </td>
                        <td style="width: 35%;">
                            <asp:CheckBox ID="chActivo" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Resource, lblTipoPersona %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoPersona" runat="server" Width="150px" 
                                AutoPostBack="true" 
                                onselectedindexchanged="ddlTipoPersona_SelectedIndexChanged">
                                <asp:ListItem Text="NATURAL" Value="NATURAL"></asp:ListItem>
                                <asp:ListItem Text="JURIDICO" Value="JURIDICO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Sujeto a Retención</td>
                        <td>
                            <asp:CheckBox ID="cbEsRetenedor" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="lblNombre" runat="server" Text="<%$ Resources:Resource, lblNombre %>"></asp:Label> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombre" MaxLength="100" runat="server" Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNombre" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" ></asp:RequiredFieldValidator>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Resource, lblNombreDos %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombreSegundo" MaxLength="100" runat="server" Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Resource, lblApellido %>"></asp:Label> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtApellidoPrimero" MaxLength="100" runat="server" Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Apellido" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtApellidoPrimero" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" ></asp:RequiredFieldValidator>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Resource, lblApellidoDos %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtApellidoSegundo" MaxLength="100" runat="server" Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="lblIde" runat="server" Text="<%$ Resources:Resource, lblNumIdentificacion %>"></asp:Label> *
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNumIdentificacion"
                                FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789-">
                            </asp:FilteredTextBoxExtender>                            
                        </td>
                        <td>                         
                            <asp:TextBox ID="txtNumIdentificacion" runat="server" MaxLength="15" 
                                Width="200px" ValidationGroup="NuevoActualizar" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumIdentificacion" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblTipoDocumento %>"></asp:Label> *
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtNumIdentificacion"
                                FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789-">
                            </asp:FilteredTextBoxExtender>                            
                        </td>
                        <td>                         
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="90%" ValidationGroup="NuevoActualizar">
                                <asp:ListItem Text="Seleccione.." Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Nit" Value="NIT"></asp:ListItem>
                                <asp:ListItem Text="Cedula" Value="CC"></asp:ListItem>
                                <asp:ListItem Text="Cedula Extranjeria" Value="CE"></asp:ListItem>
                                <asp:ListItem Text="Tarjeta de Identidad" Value="TI"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfv_TipoDocuemnto" runat="server" ErrorMessage="*" InitialValue="0" 
                                ControlToValidate="ddlTipoDocumento" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, lblDepto %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepto" Width="90%" runat="server" 
                                OnSelectedIndexChanged="ddlDepto_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, lblCiudad %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCiudad" Width="90%" runat="server">
                                <asp:ListItem Selected="True" Text="Seleccione..." Value=" "></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                                        
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Resource, lblDireccion %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDireccion" MaxLength="80" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblCorreo %>"></asp:Label> 1 *
                        </td>
                        <td>
                            <asp:TextBox ID="txtCorreo" MaxLength="100" runat="server" Width="90%" CssClass="minuscula" ValidationGroup="NuevoActualizar"></asp:TextBox>                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtCorreo" CssClass="error" ValidationGroup="NuevoActualizar"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                           </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label98" runat="server" Text="<%$ Resources:Resource, lblCorreo %>"></asp:Label> 2 *
                        </td>
                        <td>
                            <asp:TextBox ID="txtCorreo2" MaxLength="100" runat="server" Width="90%" CssClass="minuscula" ValidationGroup="NuevoActualizar"></asp:TextBox>                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="*"
                                ControlToValidate="txtCorreo2" CssClass="error" ValidationGroup="NuevoActualizar"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                           </asp:RegularExpressionValidator>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label103" runat="server" Text="<%$ Resources:Resource, lblCorreo %>"></asp:Label> 3
                        </td>
                        <td>
                            <asp:TextBox ID="txtCorreo3" MaxLength="100" runat="server" Width="90%" CssClass="minuscula" ValidationGroup="NuevoActualizar"></asp:TextBox>                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="*"
                                ControlToValidate="txtCorreo3" CssClass="error" ValidationGroup="NuevoActualizar"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                           </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Resource, lblTelefono1 %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTel1" MaxLength="30" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Resource, lblTelefono2 %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTel2" MaxLength="30" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="textoTabla"><asp:Label ID="Label36" runat="server" Text="<%$ Resources:Resource, lblNombreContacto %>"></asp:Label></td>
                                    <td class="textoTabla"><asp:Label ID="Label37" runat="server" Text="<%$ Resources:Resource, lblTelefonoContacto %>"></asp:Label></td>
                                    <td class="textoTabla">
                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Resource, lblCorreoContacto %>"></asp:Label> *
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:TextBox ID="txtNombreContacto" MaxLength="100" Width="80%" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtTelContacto" MaxLength="50" Width="80%" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoContacto" MaxLength="100" Width="80%" runat="server" CssClass="minuscula"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtCorreoContacto" CssClass="error" ValidationGroup="NuevoActualizar"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                                       </asp:RegularExpressionValidator>   
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input 
                                type="button" 
                                id="btnAgregarSuit" 
                                value="Agregar Suite" 
                                class="ui-button ui-widget ui-state-default ui-corner-all"
                                onclick='AgregarSuite();'  />                             
                        </td>
                    </tr>                                       
                </tbody>
            </table>
            
            <br />
            
            <asp:Panel ID="pnlSuit" runat="server">
            
                <table width="100%">
                    <tr>
                        <td align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, TituloGrillaSuit %>"></asp:Label>                                                                      
                                </h2>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvwSuits" 
                                          runat="server" 
                                          DataKeyNames="IdSuitPropietario,IdSuit" 
                                          Width="100%" 
                                          AutoGenerateColumns="False" 
                                          CellPadding="2"
                                          EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" BorderColor="#7599A9" 
                                          BorderStyle="Solid" BorderWidth="1px"
                                          onselectedindexchanging="gvwSuits_SelectedIndexChanging">
                                <PagerSettings Position="TopAndBottom"  />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" HeaderText="Ver Detalle">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"
                                                CommandName="Select" 
                                                ImageUrl="~/img/23.png" 
                                                Text="<%$ Resources:Resource, btnSeleccionar %>" 
                                                ToolTip="<%$ Resources:Resource, btnSeleccionar %>" />
                                        </ItemTemplate>
                                        <ControlStyle Height="30px" Width="30px" />
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" 
                                            HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblEliminar %>">
                                        <ItemTemplate>
                                            <asp:ImageButton 
                                                ID="imgBtnEliminar" 
                                                runat="server"
                                                ImageUrl="~/img/126.png"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdSuitPropietario") %>'
                                                onclick="imgBtnEliminar_Click" />
                                        </ItemTemplate>
                                        <ControlStyle Height="30px" Width="30px" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblHotel %>">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("NombreHotel") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="35%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNumSuit %>">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("NumSuit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblEscritura %>">
                                        <ItemTemplate>
                                            <asp:Label ID="Label100" runat="server" Text='<%# Bind("NumEscritura") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblEstadia %>">
                                        <ItemTemplate>
                                            <asp:Label ID="Label51" runat="server" Text='<%# Bind("NumEstadias") %>'></asp:Label>                                        
                                        </ItemTemplate>                                    
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEstado" CommandArgument='<%# Bind("IdSuitPropietario") %>' 
                                                runat="server" Text='<%# Bind("Estado") %>' onclick="btnEstado_Click" />
                                        </ItemTemplate>                                    
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle ForeColor="White" BackColor="#4D606E" />
                                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                            </asp:GridView>
                            
                            <div id="divNuevasSuite" style="display:none">
                                <table style="width:100%" id="tblSuitNuevos" cellspacing="0" border="1" style="border-color:#7599A9;border-width:1px;border-style:Solid;width:100%;border-collapse:collapse;">
                                    
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
            
        </div>       
        
        <div id="suitDetalle" runat="server" visible="false">
            <table width="100%" id="Table1">
                <tr>
                    <td colspan="3" align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, DatosSuit %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td style="width:33%;" class="textoTabla"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label></td>
                                <td style="width:33%;" class="textoTabla"><asp:Label ID="Label30" runat="server" Text="<%$ Resources:Resource, lblSuit %>"></asp:Label></td>
                                <td style="width:33%;" class="textoTabla"><asp:Label ID="Label39" runat="server" Text="<%$ Resources:Resource, lblEscritura %>"></asp:Label></td>
                            </tr>
                        </table>                        
                    </td>
                    <td style="width:33%;" class="textoTabla">
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Resource, lblBanco %>"></asp:Label>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td style="width:33%;">
                                    <asp:Label ID="lblHotelDetalle" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td style="width:33%;">
                                    <asp:Label ID="lblSuitDetalle" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td style="width:33%;">
                                    <asp:Label ID="lblEscrituraDetalle" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>                        
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBancoDetalleUpdate" runat="server" Width="90%" ValidationGroup="ActualizarSuit">
                        </asp:DropDownList>
                    </td>                    
                </tr> 
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Resource, lblTitular %>"></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtTitularDetalleUpdate" Display="Dynamic" CssClass="error" ValidationGroup="AceptarSuit" >
                        </asp:RequiredFieldValidator>--%>
                    </td>                                
                    <td class="textoTabla">
                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Resource, lblTipoCuenta %>"></asp:Label>
                    </td>
                    <td style="width:10%;" class="textoTabla">
                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Resource, lblCuenta %>"></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtCuentaDetalleUpdate" Display="Dynamic" CssClass="error" ValidationGroup="ActualizarSuit" >
                        </asp:RequiredFieldValidator>--%>
                    </td>
                </tr>             
                <tr>
                    <td>
                        <asp:TextBox ID="txtTitularDetalleUpdate" Width="90%" MaxLength="80" runat="server" ValidationGroup="ActualizarSuit"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoCuentaDetalleUpdate" Width="80%" runat="server">
                            <asp:ListItem Value="-1" Text="Sin Tipo Cuenta"></asp:ListItem>
                            <asp:ListItem Value="CH" Text="<%$ Resources:Resource, lblCuentaAhorro %>"></asp:ListItem>
                            <asp:ListItem Value="CC" Text="<%$ Resources:Resource, lblCuentaCorriente %>"></asp:ListItem>
                            <asp:ListItem Value="HH" Text="CHEQUE"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCuentaDetalleUpdate" runat="server" MaxLength="50" ValidationGroup="ActualizarSuit"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Custom,Numbers" ValidChars="-"  
                            runat="server" Enabled="True" TargetControlID="txtCuentaDetalleUpdate">
                        </asp:FilteredTextBoxExtender>                        
                        
                    </td>
                </tr> 
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Resource, lblEstadia %>"></asp:Label> *
                    </td>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNumEstadiasUpdate" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumEstadiasUpdate" Display="Dynamic" CssClass="error" ValidationGroup="ActualizarSuit" >
                        </asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Numbers"  
                            runat="server" Enabled="True" TargetControlID="txtNumEstadiasUpdate">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td colspan="2">&nbsp;</td>
                    
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>  
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlVariablesValor" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <input type="button" onclick="SaveData();" value="Guardar" class="ui-button ui-widget ui-state-default ui-corner-all" />
                    </td>
                </tr>
            </table>  
            
            <div style="display:none;">
                <asp:TextBox ID="txtValorVariableUpdate" runat="server"></asp:TextBox>
            </div>
            
        </div>        
                  
        <div id="GrillaPropietario" runat="server">
        
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Resource, TituloGrillaPropietarios %>"></asp:Label>                          
                </h2>
            </div>
            
            <table width="100%">
                <tr>
                    <td style="width:10%;" class="textoTabla">Hotel</td>
                    <td style="width:90%;">
                        <asp:DropDownList ID="ddlHotelFiltro" Width="50%" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="ddlHotelFiltro_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <br />
        
            <asp:GridView 
                ID="gvwPropietario" 
                runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" 
                AllowPaging="true"
                BorderWidth="1px"               
                BorderColor="#7599A9"
                CellPadding="2"
                DataKeyNames="IdPropietario" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                OnPageIndexChanging="gvwPropietario_PageIndexChanging"
                OnSelectedIndexChanged="gvwPropietario_OnSelectedIndexChanged" 
                Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombreSolo %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" 
                                Text='<%# Bind("NombreCompleto") %>'></asp:LinkButton>                                    
                        </ItemTemplate>
                        <HeaderStyle Width="70%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TipoPersona" HeaderText="Tipo Persona" >
                        <HeaderStyle Width="10%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NumSuit" HeaderText="Num. Suite" >
                        <HeaderStyle Width="10%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NumEscritura" HeaderText="Num. Escritura" >
                        <HeaderStyle Width="10%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
            
        </div>
        
        <div style="display:none;">
            <asp:Button ID="btnAceptarSuit" runat="server" Text="btnAceptarSuit" onclick="btnAceptarSuit_Click" ValidationGroup="AceptarSuit" />
            <input type="text" id="idCtrl" value="" />
        </div>
        
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>

<div id="modalSuit" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>  

            <table width="100%" id="tablaSuit">
                <tr>
                    <td colspan="4" align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Resource, DatosSuit %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width:15%" class="textoTabla">
                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                    </td>
                    <td style="vertical-align:top;width:35%;">
                        <asp:DropDownList ID="ddlHotel" runat="server" Width="90%" AutoPostBack="true" 
                            onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width:20%;" class="textoTabla">
                        <asp:Label ID="Label19" runat="server" Text="Num. Suite"></asp:Label> *
                    </td>
                    <td style="vertical-align:top;width:30%;">
                        <asp:DropDownList ID="ddlSuit" runat="server" Width="50%" AutoPostBack="true" 
                            onselectedindexchanged="ddlSuit_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" 
                                ControlToValidate="ddlSuit" Display="Dynamic" CssClass="error" ValidationGroup="AceptarSuit" >
                        </asp:RequiredFieldValidator>
                    </td>                    
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Resource, lblDescripcion %>"></asp:Label>                        
                    </td>                    
                    <td>
                        <asp:TextBox ID="txtDescripcionSuit" TextMode="MultiLine" runat="server" Width="95%" Height="60" Enabled="false"></asp:TextBox>
                    </td>
                    <td class="textoTabla">
                        <asp:Label ID="Label50" runat="server" Text="<%$ Resources:Resource, lblEstadias %>"></asp:Label> *
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumEstadias" runat="server" Width="50%" ValidationGroup="AceptarSuit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumEstadias" Display="Dynamic" CssClass="error" ValidationGroup="AceptarSuit" >
                        </asp:RequiredFieldValidator> 
                        <asp:FilteredTextBoxExtender ID="txtEstadias_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtNumEstadias">
                        </asp:FilteredTextBoxExtender>    
                    </td>
                </tr> 
                <tr>                    
                    <td class="textoTabla">
                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Resource, lblTitular %>"></asp:Label> *
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitular" Width="80%" MaxLength="80" runat="server" ValidationGroup="AceptarSuit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtTitular" Display="Dynamic" CssClass="error" ValidationGroup="AceptarSuit" >
                        </asp:RequiredFieldValidator>
                    </td>
                    <td class="textoTabla">
                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Resource, lblTipoCuenta %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoCuenta" Width="80%" runat="server">
                            <asp:ListItem Selected="True" Value="CH" Text="<%$ Resources:Resource, lblCuentaAhorro %>"></asp:ListItem>
                            <asp:ListItem Value="CC" Text="<%$ Resources:Resource, lblCuentaCorriente %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>
                <tr>                    
                    <td class="textoTabla">
                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Resource, lblBanco %>"></asp:Label>
                    </td>                    
                    <td>
                        <asp:DropDownList ID="ddlBanco" runat="server" Width="90%" ValidationGroup="AceptarSuit">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">
                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Resource, lblCuenta %>"></asp:Label> *
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumCuenta" runat="server" MaxLength="50" Width="50%" ValidationGroup="AceptarSuit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumCuenta" Display="Dynamic" CssClass="error" ValidationGroup="AceptarSuit" >
                        </asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="txtNumCuenta_FilteredTextBoxExtender" FilterType="Custom,Numbers" ValidChars="-"  
                            runat="server" Enabled="True" TargetControlID="txtNumCuenta">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
            
            <div style="display:none;">
                <asp:TextBox ID="txtValoresVariables" runat="server"></asp:TextBox>
            </div>
            
            <br />          
            
            <asp:Panel ID="pnlMisVariables" runat="server">
            </asp:Panel>
                        
            <br />
            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblAyudaPorcentaje %>"></asp:Label>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div id="modalBuscadorPropietario">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:WebUserBuscadorPropietario ID="uc_WebUserBuscadorPropietario" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>            
</div>