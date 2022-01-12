<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserRelacionCuentaHotel.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserRelacionCuentaHotel" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <script type="text/javascript">

        $j(document).ready(function() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

            CargasIniciales(null, null);

            function CargasIniciales(sender, args) {

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
                            $j('#ctl00_idCtrl').val('');
                            $j(this).dialog('close');
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
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="NuevoActualizar" onclick="btnActualizar_Click" />
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
        
        <br />
        <table width="100%" cellpadding="3" cellspacing="0">
             <tr>
                <td class="textoTabla" style="width:15%">
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                </td>
                <td style="width:85%">
                    <asp:DropDownList ID="ddlHotel" runat="server" Width="90%" AutoPostBack="true" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div runat="server" class="cuadradoInfo" id="divInfo" visible="false">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/52.png" 
                            Width="20px" Height="20px" ImageAlign="AbsMiddle" />
                        <asp:Label ID="lbltextoInfo" runat="server" CssClass="textoInfo" ></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <br />
                
        <asp:Panel ID="pnlNuevoEditar" runat="server" Visible="false">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td class="textoTabla" style="width:15%">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblCuentaContable %>"></asp:Label>
                    </td>
                    <td style="width:85%">
                        <asp:DropDownList ID="ddlCuentaContable" runat="server" Width="90%" >
                        </asp:DropDownList>
                        <asp:Label ID="lblCuentaContable" runat="server" Visible="false" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblCentroCosto %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCentroCosto" runat="server" Width="90%" >
                        </asp:DropDownList>                        
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label5" runat="server" Text="Tercero Variable"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbEsTerceroVariable" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblCodigoTercero %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCodigoTercero" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">Tiene Base</td>
                    <td>
                        <asp:CheckBox ID="cbEsConBase" AutoPostBack="true" runat="server" 
                            oncheckedchanged="cbEsConBase_CheckedChanged" /></td>
                </tr>
                <tr>
                    <td class="textoTabla">Conceptos</td>
                    <td>
                        <asp:DropDownList ID="ddlConceptos" runat="server" Width="90%" Enabled="false">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv_Conceptos" runat="server" ErrorMessage="*" Enabled="false" 
                            ControlToValidate="ddlConceptos" Display="Dynamic" CssClass="error" InitialValue="-1" ValidationGroup="NuevoActualizar" >
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlGrilla" runat="server">        
            <asp:GridView 
                ID="gvwHotelesCuenta" 
                runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                BorderColor="#7599A9" 
                CellPadding="2"
                DataKeyNames="IdCentroCosto_Hotel,IdCentroCosto,IdCuentaContable,IdConcepto,EsConBase,EsTerceroVariable" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                Width="100%" 
                onselectedindexchanged="gvwHotelesCuenta_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblCuentaContable %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblCuentaContable" runat="server" Text='<%# Bind("NombreCuentaContable") %>' CommandName="Select" >
                            </asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                        <HeaderStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblCentroCosto %>">
                        <ItemTemplate>
                            <asp:Label ID="lblCentroCosto" runat="server" Text='<%# Bind("NombreCentroCosto") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                        <HeaderStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblHotel %>">
                        <ItemTemplate>
                            <asp:Label ID="lblHotel" runat="server" Text='<%# Bind("NombreHotel") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                        <HeaderStyle Width="30%" />
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblCodigoTercero %>">
                        <ItemTemplate>
                            <asp:Label ID="lblCodigoTercero" runat="server" Text='<%# Bind("Codigo") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                        <HeaderStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" CommandArgument='<%# Bind("IdCentroCosto_Hotel") %>' ImageUrl="~/img/126.png" runat="server" 
                                onclick="imgBtnEliminar_Click" />
                            <asp:ConfirmButtonExtender ID="imgBtnEliminar_ConfirmButtonExtender" 
                                runat="server" ConfirmText="Esta seguro de eliminar esta Cuanta Contable - Hotel?" Enabled="True" TargetControlID="imgBtnEliminar">
                            </asp:ConfirmButtonExtender>
                        </ItemTemplate>   
                        <ControlStyle Height="30px" Width="30px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>                
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
        </asp:Panel>
    
        </ContentTemplate>
</asp:UpdatePanel>