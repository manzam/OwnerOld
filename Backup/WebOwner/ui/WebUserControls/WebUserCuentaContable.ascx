<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserCuentaContable.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserCuentaContable" %>

<script type="text/javascript">

    $j(document).ready(function() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(dragLista);
        dragLista(null, null);

        function dragLista(sender, args) {

            $j("#modalOk").dialog({
                autoOpen: false,
                resizable: false,
                show: "slow",
                modal: true,
                height: "auto",
                title: "Eliminar",
                buttons: {
                    "Aceptar": function() {
                        $j(this).dialog('close');
                        $j('#' + $j('#ctl00_idCtrl').val()).click();
                    },
                    "Cancelar": function() {
                        $j(this).dialog('close');
                        $j('#ctl00_idCtrl').val('');
                    }
                }
            });
        }

    });

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
        
        <div id="NuevoCuentaContable" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="4" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosCuentaContable %>"></asp:Label>                          
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:15%; vertical-align:top;" class="textoTabla">
                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblCodigo %>"></asp:Label>*                            
                        </td>
                        <td style="width:35%; vertical-align:top;">
                            <asp:TextBox ID="txtCodigo" MaxLength="10" runat="server" Width="50%" ValidationGroup="NuevoActualizar">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtCodigo" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" > 
                            </asp:RequiredFieldValidator>
                        </td>
                        <td style="width:15%; vertical-align:top;" class="textoTabla">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblDescripcion %>"></asp:Label>*
                        </td>
                        <td style="width:35%;">
                            <asp:TextBox ID="txtNombre" TextMode="MultiLine" MaxLength="80" Height="80px" runat="server" Width="98%" ValidationGroup="NuevoActualizar">
                            </asp:TextBox>
                        </td>
                    </tr>                    
                    <tr>                        
                        <td class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblTipoCuenta %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoCuenta" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblCentroCostoVariable %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chbCentroCosto" runat="server" AutoPostBack="true" 
                                oncheckedchanged="chbCentroCosto_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblDocCruce %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chbDocCruce" runat="server" />
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblEncabezadoDocCruce %>"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtEncabezadoCruce" runat="server" Width="50%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblNaturalezaCuenta %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNaturaleza" runat="server" Width="90%">
                                <asp:ListItem Text="Debito" Value="Debito"></asp:ListItem>
                                <asp:ListItem Text="Credito" Value="Credito"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="textoTabla">
                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, lblCentroCosto %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCentroCosto" Width="90%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, lblUnidadNegocio %>"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ToolTip="Si este campo se deja vacio, se cojera la Unidad de Negocio del Hotel." ID="txtUnidadNegocio" runat="server" Width="50%"></asp:TextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
            
            <br />
            
        </div>
        
        <div id="GrillaCuentaContable" runat="server">
            
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, TituloGrillaCuentaContable %>"></asp:Label>                          
                </h2>
            </div>
            
            <asp:GridView 
                ID="gvwCuentaContable" 
                runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                BorderColor="#7599A9" 
                CellPadding="2"
                DataKeyNames="IdCuentaContable" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                OnSelectedIndexChanged="gvwCuentaContable_OnSelectedIndexChanged" 
                Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblCodigo %>">
                        <ItemTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="width:90%; text-align:left;">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Text='<%# Bind("Codigo") %>'></asp:LinkButton>
                                    </td>
                                    <td style="width:10%; text-align:right;">
                                        <asp:ImageButton ID="imgBtnEliminar" ToolTip="<%$ Resources:Resource, lblMensajeEliminarPregunta %>" ImageUrl="~/img/126.png" Width="20px" Height="20px" runat="server" OnClientClick="ventanaOk(this);" />
                                        <asp:ImageButton 
                                            ID="ImageButton1" 
                                            runat="server"
                                            CssClass="ctrlOculto" 
                                            CommandArgument='<%# Bind("IdCuentaContable") %>'
                                            OnClick="imgBtnEliminar_Click" />
                                    </td>
                                </tr>
                            </table>                            
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="Descripcion"  HeaderText="Nombre" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="TipoCuenta" HeaderText="<%$ Resources:Resource, lblTipoCuenta %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
        </div>        
        
    </ContentTemplate>
</asp:UpdatePanel>


