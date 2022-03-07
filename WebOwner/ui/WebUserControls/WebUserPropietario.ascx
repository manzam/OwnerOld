<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserPropietario.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserPropietario" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>    
<%@ Register src="WebUserBuscadorPropietario.ascx" tagname="WebUserBuscadorPropietario" tagprefix="uc1" %>
<script src="../../js/variablePropietario.js?v=00010"></script>
    
   
        <div class="botonera">
            <asp:Button ID="btnNuevo" runat="server" Text="<%$ Resources:Resource, btnNuevo %>" ValidationGroup="Nuevo" />
            <input type="button" value="Guardar" onclick="GuardarPropietario();" id="btnguardar" class="ui-button ui-widget ui-state-default ui-corner-all" />
            <asp:Button ID="btnVerTodos" runat="server" Text="Ver Todos" OnClientClick="verTodos();" />
            <asp:Button ID="btnBuscar" runat="server" Text="<%$ Resources:Resource, btnBuscar %>" OnClientClick="$j('#modalBuscadorPropietario').dialog('open')" />
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" Visible="false" />            
        </div>

        <input type="hidden" id="idPropietario" value="-1" />
        <input type="hidden" id="idSuitePropietario" value="-1" />
        
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
        
        <div id="dataPropietario">
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
                            <asp:CheckBox ID="chActivo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Resource, lblTipoPersona %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoPersona" runat="server" Width="150px">
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
                            <select id="selectDepto" onchange="selectDepto_onchange()">                            
                            </select>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, lblCiudad %>"></asp:Label>
                        </td>
                        <td>
                            <select id="selectCity">                            
                            </select>
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
            
            <div id="listaSuite">
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
                </table>
                <table style="width:100%">
                    <thead>
                        <tr style="color: White; background-color: #7599A9; border-color: #7599A9;">
                            <td>Ver detalle</td>
                            <td>Eliminar</td>
                            <td>Hotel</td>
                            <td>No. Suite</td>
                            <td>Escritura</td>
                            <td>Activo</td>
                        </tr>
                    </thead>
                    <tbody id="tblListaSuite"></tbody>                    
                </table>
            </div>

            <br />

            <div id="editSuite">
                <table width="100%" id="tablaSuit">
                    <tr>
                        <td colspan="4" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label4" runat="server" Text="Datos Suite"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                    <tr id="nuevaSuite">
                        <td style="width:15%" class="textoTabla">
                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                        </td>
                        <td style="vertical-align:top;width:35%;">
                            <select id="selectHotel" onchange="selectHotel_onchange()">                            
                            </select>
                        </td>
                        <td style="width:20%;" class="textoTabla">
                            <asp:Label ID="Label8" runat="server" Text="Num. Suite"></asp:Label> *
                        </td>
                        <td style="vertical-align:top;width:30%;">
                            <select id="selectSuite" onchange="selectSuite_onchange()">                            
                            </select>
                        </td>                    
                    </tr>
                    <tr id="updateSuite">
                        <td style="width:15%" class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                        </td>
                        <td style="vertical-align:top;width:35%;">
                            <span id="nombreHotel"></span>
                        </td>
                        <td style="width:20%;" class="textoTabla">
                            <asp:Label ID="Label2" runat="server" Text="Num. Suite"></asp:Label>
                        </td>
                        <td style="vertical-align:top;width:30%;">
                            <span id="nombreSuite"></span>
                        </td>                    
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, lblDescripcion %>"></asp:Label>                        
                        </td>                    
                        <td>
                            <textarea id="txtDescripcionSuit" style="width:95%;" rows="3" disabled></textarea>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Resource, lblEstadias %>"></asp:Label> *
                        </td>
                        <td>
                            <input type="text" id="txtEstadias" />
                        </td>
                    </tr> 
                    <tr>                    
                        <td class="textoTabla">
                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Resource, lblTitular %>"></asp:Label> *
                        </td>
                        <td>
                            <input type="text" id="txtTitularCuenta" style="width: 90%;" />
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Resource, lblTipoCuenta %>"></asp:Label>
                        </td>
                        <td>
                            <select id="selectTipoCuenta">
                                <option value="CH">Cuenta de Ahorro</option>
                                <option value="CC">Cuenta Corriente</option>
                            </select>                        
                        </td>                    
                    </tr>
                    <tr>                    
                        <td class="textoTabla">
                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Resource, lblBanco %>"></asp:Label>
                        </td>                    
                        <td>
                            <select id="selectBank">                            
                            </select>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Resource, lblCuenta %>"></asp:Label> *
                        </td>
                        <td>
                            <input id="txtNumCuenta" type="text" />
                        </td>
                    </tr>
                </table>

                <br />
                <table id="tblVariables" style="width:100%">                    
                </table>
                <br />
                <div>
                    <div style="float:left; width:80%;">Los valores de porcentajes, debe ir expresdas en decimales.</div>
                    <div style="float:left; text-align:right;">
                        <input type="button" value="Actualizar valores" id="btnUpdateVariables" onclick="UpdateVariables()"  class="ui-button ui-widget ui-state-default ui-corner-all" />
                    </div>
                </div>
            </div>
            
        </div>
 
        <div id="GrillaPropietario">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <div class="tituloGrilla" style="text-align:center;">
                        <h2>
                            Lista Propietarios                       
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
                        OnRowDataBound="gvwPropietario_RowDataBound"
                        Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombreSolo %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkSelect" runat="server" Text='<%# Bind("NombreCompleto") %>'>
                                    </asp:LinkButton>                                    
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
                </ContentTemplate>
                <Triggers>

                </Triggers>
            </asp:UpdatePanel>   
        
            
            
            
        
            
            
        </div>

<div>
    <br />
    <div id="divValidPesos" style="display:none">
        <div>
            <h2>Error Coeficientes propietarios vr Suites</h2>
        </div>
        <div>
            <table style="width:100%; border-spacing:0px;">
                <thead>
                    <tr>
                        <td class="textoTabla" style="text-align: center;">No Suite</td>
                        <td class="textoTabla" style="text-align: center;">Propietario</td>
                        <td class="textoTabla" style="text-align: center;">No Identificación</td>
                        <td class="textoTabla" style="text-align: center;">Valor Propietario</td>
                        <td class="textoTabla" style="text-align: center;">Valor Suite</td>
                    </tr>
                </thead>
                <tbody id="tBodyPesos"></tbody>
            </table>
        </div>
    </div>
    <div id="divValidCoef" style="display:none">
        <div>
            <h2>Error Coeficientes propietarios</h2>
        </div>
        <div>
            <table style="width:100%; border-spacing:0px;">
                <thead>
                    <tr>
                        <td class="textoTabla" style="text-align: center;">No Suite</td>
                        <td class="textoTabla" style="text-align: center;">Propietario</td>
                        <td class="textoTabla" style="text-align: center;">No Identificación</td>
                        <td class="textoTabla" style="text-align: center;">Valor Propietario</td>
                    </tr>
                </thead>
                <tbody id="tBodyCoef"></tbody>
            </table>
        </div>
    </div>
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