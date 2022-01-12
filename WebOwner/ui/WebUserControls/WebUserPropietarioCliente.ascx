<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserPropietarioCliente.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserPropietarioCliente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    <div class="botonera">
        <asp:Button ID="btnGuardar" runat="server" ValidationGroup="Actualizar" 
            Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" onclick="btnGuardar_Click" />
        <asp:Button ID="btnActualizar" runat="server" ValidationGroup="Actualizar" 
            Text="<%$ Resources:Resource, btnModificar %>" onclick="btnActualizar_Click" />
        <asp:Button ID="btnCancelar" runat="server" ValidationGroup="Actualizar" 
            Text="<%$ Resources:Resource, btnCancelar %>" Visible="false" onclick="btnCancelar_Click" />
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
        
        <table width="100%">
            <tr>
                <td colspan="4" align="center">
                    <div class="tituloGrilla">
                        <h2>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosPropietarioCliente %>"></asp:Label>
                        </h2>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Resource, lblTipoPersona %>"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblTipoPersona" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:20%;" class="textoTabla">
                    <asp:Label ID="lblNombre" runat="server" Text="<%$ Resources:Resource, lblNombre %>"></asp:Label>                             
                </td>
                <td style="width:30%;">
                    <asp:Label ID="txtNombre" runat="server"></asp:Label>
                </td>
                <td style="width:20%;" class="textoTabla">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblNombreDos %>"></asp:Label>
                </td>
                <td style="width:30%;">
                    <asp:Label ID="txtNombreSegundo" runat="server"></asp:Label>                            
                </td>
            </tr>
            <tr>
                <td class="textoTabla" class="textoTabla">
                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Resource, lblApellido %>"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtApellidoPrimero" runat="server"></asp:Label>
                </td>
                <td class="textoTabla" class="textoTabla">
                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Resource, lblApellidoDos %>"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtApellidoSegundo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="lblIde" runat="server" Text="<%$ Resources:Resource, lblNumIdentificacion %>"></asp:Label>                            
                </td>
                <td>                         
                    <asp:Label ID="txtNumIdentificacion" runat="server"></asp:Label>
                </td>
                <td class="textoTabla">
                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, lblTipoDocumento %>"></asp:Label>
                    *
                </td>
                <td>                         
                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="90%" ValidationGroup="Actualizar" Enabled="false">
                        <asp:ListItem Text="Seleccione.." Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Nit" Value="NIT"></asp:ListItem>
                        <asp:ListItem Text="Cedula" Value="CC"></asp:ListItem>
                        <asp:ListItem Text="Cedula Extranjeria" Value="CE"></asp:ListItem>
                        <asp:ListItem Text="Targeta de Identidad" Value="TI"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfv_TipoDocuemnto" runat="server" ErrorMessage="*" InitialValue="0" 
                        ControlToValidate="ddlTipoDocumento" Display="Dynamic" CssClass="error" ValidationGroup="Actualizar" >
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblDepto %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDepto" Width="200px" runat="server" 
                        OnSelectedIndexChanged="ddlDepto_SelectedIndexChanged"
                        AutoPostBack="true" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td class="textoTabla">
                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblCiudad %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCiudad" Width="200px" runat="server" Enabled="false">
                        <asp:ListItem Selected="True" Text="Seleccione..." Value=" "></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label5" runat="server" Text="Correo 1 *"></asp:Label>                
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCorreo" MaxLength="50" runat="server" Width="70%" CssClass="minuscula" ValidationGroup="Actualizar" Enabled="false"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*"
                        ControlToValidate="txtCorreo" CssClass="error" ValidationGroup="Actualizar"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                   </asp:RegularExpressionValidator>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtCorreo" Display="Dynamic" CssClass="error" ValidationGroup="Actualizar" >
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label1" runat="server" Text="Correo 2 *"></asp:Label>                
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCorreo2" MaxLength="50" runat="server" Width="70%" CssClass="minuscula" ValidationGroup="Actualizar" Enabled="false"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="*"
                        ControlToValidate="txtCorreo2" CssClass="error" ValidationGroup="Actualizar"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                   </asp:RegularExpressionValidator>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtCorreo2" Display="Dynamic" CssClass="error" ValidationGroup="Actualizar" >
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label4" runat="server" Text="Correo *"></asp:Label>                
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCorreo3" MaxLength="50" runat="server" Width="70%" CssClass="minuscula" ValidationGroup="Actualizar" Enabled="false"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="*"
                        ControlToValidate="txtCorreo3" CssClass="error" ValidationGroup="Actualizar"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                   </asp:RegularExpressionValidator>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtCorreo3" Display="Dynamic" CssClass="error" ValidationGroup="Actualizar" >
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label24" runat="server" Text="Dirección *"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtDireccion" MaxLength="80" Width="70%" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"  
                        ControlToValidate="txtDireccion" Display="Dynamic" CssClass="error" ValidationGroup="Actualizar" >
                    </asp:RequiredFieldValidator>
                </td>                        
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label25" runat="server" Text="Telefono 1"></asp:Label>
                    *
                </td>
                <td>
                    <asp:TextBox ID="txtTel1" MaxLength="30" Width="200px" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtTel1" Display="Dynamic" CssClass="error" ValidationGroup="Actualizar" >
                    </asp:RequiredFieldValidator>
                </td>
                <td class="textoTabla">
                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Resource, lblTelefono2 %>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTel2" MaxLength="30" Width="200px" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">Nombre Contacto</td>
                <td>
                    <asp:TextBox ID="txtNombreContacto" MaxLength="30" Width="200px" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td class="textoTabla">Telefono Contacto</td>
                <td>
                    <asp:TextBox ID="txtTelefonoContacto" MaxLength="30" Width="200px" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">Correo Contacto</td>
                <td colspan="3">
                    <asp:TextBox ID="txtCorreoContacto" MaxLength="30" Width="70%" CssClass="minuscula" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator
                        ID="RegularExpressionValidator2" runat="server" ErrorMessage="*" Text="*" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  ValidationGroup="Actualizar" 
                        ControlToValidate="txtCorreoContacto"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">Retenedor</td>
                <td>
                    <asp:CheckBox ID="cbEsRetenedor" runat="server" Enabled="false" /></td>
            </tr>
            <tr>
                <td colspan="10"></td>
            </tr>
            <tr>
                <td colspan="10">
                    <asp:GridView 
                        ID="gvwSuites" 
                        runat="server" 
                        AutoGenerateColumns="False" 
                        BorderStyle="Solid" 
                        BorderWidth="1px" 
                        BorderColor="#7599A9" 
                        CellPadding="2"
                        EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="NombreHotel" HeaderText="Hotel" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                <HeaderStyle Width="70%" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="NumSuit" HeaderText="Suite" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                <HeaderStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumEscritura" HeaderText="Suite Escritura" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                <HeaderStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RegistroNotaria" HeaderText="Registro Notaria" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                <HeaderStyle Width="10%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    
    </ContentTemplate>
</asp:UpdatePanel>
