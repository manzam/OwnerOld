<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUsers.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserUsers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        
        <div id="GrillaUsuario" runat="server">
        
            <table width="100%">
                <tr>
                    <td style="width:10%" class="textoTabla">
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                    </td>
                    <td style="width:90%; text-align:left;">
                        <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" Width="350px" 
                            onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
        
             <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Resource, TituloprincipalUsuario %>"></asp:Label>                          
                </h2>
            </div>
        
            <asp:GridView ID="gvwUsuario" runat="server" 
                    AutoGenerateColumns="False" 
                    BorderStyle="Solid" BorderWidth="1px" BorderColor="#7599A9"
                    CellPadding="2"
                    DataKeyNames="IdUsuario,IdPerfil" 
                    EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                    onselectedindexchanging="gvwUsuario_SelectedIndexChanging" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" CssClass="mayuscula"
                                    Text='<%# Bind("Nombre") %>'></asp:LinkButton>                                    
                            </ItemTemplate>
                            <HeaderStyle Width="30%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Resource, lblApellido %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Select" CssClass="mayuscula"  
                                    Text='<%# Bind("Apellido") %>'></asp:LinkButton>                                    
                            </ItemTemplate>
                            <HeaderStyle Width="30%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="NombrePerfil" HeaderText="<%$ Resources:Resource, lblPerfil %>" >
                            <HeaderStyle Width="20%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Login" HeaderText="<%$ Resources:Resource, lblLogin %>" >
                            <HeaderStyle Width="20%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                </asp:GridView>
        </div>
        
        <div id="NuevoUsuario" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="4" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosPropietarioUsuario %>"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width: 20%;" class="textoTabla">
                            Login *
                        </td>
                        <td style="width: 30%;">
                            <asp:TextBox ID="txtLogin" Width="80%" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars" 
                            ValidChars="0123456789qwertyuiopasdfghjklñzxcvbnmQWERTYUIOPASDFGHJKLÑZXCVBNM" FilterType="Custom"
                            TargetControlID="txtLogin">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtLogin" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 15%;">
                            <asp:Button ID="btnReset" runat="server" Text="Restablecer Constraseña" 
                                Visible="false" onclick="btnReset_Click" />
                        </td>
                        <td style="width: 35%;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, lblPerfil %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPerfil" Width="200px" runat="server" AutoPostBack="true" 
                                onselectedindexchanged="ddlPerfil_SelectedIndexChanged">
                            </asp:DropDownList>                            
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, lblActivo %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chActivo" runat="server" Checked="true" AutoPostBack="true" 
                                oncheckedchanged="chActivo_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblNombreSolo %>"></asp:Label> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombre" MaxLength="100" runat="server" Width="80%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNombre" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" ></asp:RequiredFieldValidator>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblApellidoSolo %>"></asp:Label> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtApellido" MaxLength="100" runat="server" Width="80%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtApellido" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblNumIdentificacion %>"></asp:Label> *
                        </td>
                        <td>                         
                            <asp:TextBox ID="txtNumIdentificacion" runat="server" MaxLength="15" 
                                Width="80%" ValidationGroup="NuevoActualizar" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumIdentificacion" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNumIdentificacion"
                                FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789-">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblCorreo %>"></asp:Label> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtCorreo" CssClass="minuscula" MaxLength="100" runat="server" Width="80%" ValidationGroup="NuevoActualizar"></asp:TextBox>                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtCorreo" CssClass="error" ValidationGroup="NuevoActualizar"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                           </asp:RegularExpressionValidator>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtCorreo" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>                           
                        </td>
                    </tr>                                       
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Resource, lblTelefono1 %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTel1" MaxLength="30" Width="80%" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender 
                                ID="FilteredTextBoxExtender2" 
                                FilterType="Custom" 
                                TargetControlID="txtTel1" 
                                FilterMode="ValidChars"
                                ValidChars="-0123456789()"
                                runat="server">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        
                        <td class="textoTabla">
                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Resource, lblTelefono2 %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTel2" MaxLength="30" Width="80%" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender 
                                ID="FilteredTextBoxExtender3" 
                                FilterType="Custom" 
                                TargetControlID="txtTel2" 
                                FilterMode="ValidChars"
                                ValidChars="-0123456789()"
                                runat="server">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>                                      
                </tbody>
            </table>
            
            <br />
            
            <table width="100%">
                <tr>
                    <td align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, TituloGrillaHotel %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwHotel" 
                                  runat="server" 
                                  DataKeyNames="IdHotel" 
                                  Width="100%" 
                                  AutoGenerateColumns="False" 
                                  CellPadding="2"
                                  EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" BorderColor="#7599A9" 
                                  BorderStyle="Solid" BorderWidth="1px" 
                            onselectedindexchanged="gvwHotel_SelectedIndexChanged">
                            <PagerSettings Position="TopAndBottom"  />
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="<%$ Resources:Resource, lblNombre %>" >
                                    <HeaderStyle Width="90%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblSeleccion %>">
                                    <ItemTemplate>
                                        <asp:ImageButton 
                                            ID="imgBtnSeleccion" 
                                            runat="server"
                                            Height="35px" 
                                            Width="35px"                                    
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdHotel") %>'
                                            ImageUrl="~/img/117.png" onclick="imgBtnSeleccion_Click" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle BorderColor="#7599A9" HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" />
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle ForeColor="White" BackColor="#4D606E" />
                            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                        </asp:GridView>                        
                        
                    </td>
                </tr>
            </table>
                        
        </div>   
    
    </ContentTemplate>
</asp:UpdatePanel>
