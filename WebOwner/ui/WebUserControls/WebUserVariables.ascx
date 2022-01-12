<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserVariables.ascx.cs"
    Inherits="WebOwner.ui.WebUserControls.WebUserVariables" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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

                $j("#ctl00_Contenidoprincipal_WebUserVariables1_txtNumMaximo,.valMax").spinner({
                    min: 0,
                    max: 100
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
                Text="<%$ Resources:Resource, btnNuevo %>" ValidationGroup="Nuevo" OnClick="btnNuevo_Click" />
            <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnVerTodos" runat="server" 
                Text="<%$ Resources:Resource, btnVerTodos %>" ValidationGroup="VerTodos" OnClick="btnVerTodos_Click" />
        </div>
        
        <br />
        
        <div runat="server" class="cuadradoExito" id="divExito" visible="false">
            <asp:Image ID="imgExitoMsg" runat="server" ImageUrl="~/img/33.png" Width="20px" Height="20px"
                ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoExito" runat="server" CssClass="textoExito"></asp:Label>
        </div>
        <div runat="server" class="cuadradoError" id="divError" visible="false">
            <asp:Image ID="imgErrorMsg" runat="server" ImageUrl="~/img/115.png" Width="20px"
                Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="lbltextoError" runat="server" CssClass="textoError"></asp:Label>
        </div>
        
        <table width="100%">
            <tr>
                <td style="width: 10%" class="textoTabla">
                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHotel" runat="server" Width="70%" 
                        AutoPostBack="true" onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        
        <br />
        
        <div id="GrillaVariables" runat="server">
            <table width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="gvwVariables" runat="server" 
                            DataKeyNames="IdVariable,IdHotel,Tipo,Nombre" 
                            Width="100%"
                            AutoGenerateColumns="False" 
                            CellPadding="2" 
                            EmptyDataText="No se encontraron Variables."
                            BorderColor="#7599A9" 
                            BorderStyle="Solid" 
                            BorderWidth="1px" 
                            OnRowCancelingEdit="gvwVariables_RowCancelingEdit"
                            OnRowEditing="gvwVariables_RowEditing" 
                            OnRowUpdating="gvwVariables_RowUpdating" 
                            onrowdatabound="gvwVariables_RowDataBound">
                        <Columns>
                            <asp:CommandField ButtonType="Image" ValidationGroup="Edicion" ShowEditButton="true"
                                HeaderText="<%$ Resources:Resource, lblAccion %>" UpdateImageUrl="~/img/131.png" CancelImageUrl="~/img/72.png"
                                DeleteImageUrl="~/img/126.png" EditImageUrl="~/img/13.png">
                                <ControlStyle Height="30px" Width="30px" />
                                <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblEliminar %>">
                                <ItemTemplate>                                        
                                    <asp:ImageButton 
                                        ID="ImageButton2" 
                                        runat="server" 
                                        ImageUrl="~/img/126.png"
                                        ToolTip="<%$ Resources:Resource, lblMensajeEliminarPregunta %>"
                                        CssClass="botonEliminar" 
                                        OnClientClick="ventanaOk(this);" />
                                    <asp:ImageButton 
                                        ID="imgBtnEliminar" 
                                        runat="server"
                                        CssClass="ctrlOculto" 
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdVariable") %>'
                                        OnClick="imgBtnEliminar_Click" />
                                </ItemTemplate>
                                <ControlStyle Height="30px" Width="30px" />
                                <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombreGrilla" ValidationGroup="Edicion" runat="server" Width="90%"
                                        Text='<%# Bind("Nombre") %>'></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                        FilterMode="ValidChars" ValidChars="qwertyuioplkjhgfdsazxcvbnm_QWERTYUIOPLKJHGFDSAZXCVBNM" FilterType="Custom" TargetControlID="txtNombreGrilla">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtNombreGrilla" Display="Dynamic" CssClass="error" ValidationGroup="Edicion">
                                    </asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Validación">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbConValidacion" runat="server" Enabled="false" Checked='<%# Bind("EsValidacion") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cbConValidacion" runat="server" Checked='<%# Bind("EsValidacion") %>' />
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Val. Maximo">
                                <ItemTemplate>
                                    <asp:Label ID="Label20" runat="server" Text='<%# Bind("ValMax") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtValMaximo" CssClass="valMax" Width="40px" runat="server" Text='<%# Bind("ValMax") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblDescripcion %>">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescripcionGrilla" TextMode="MultiLine" Height="80px" Width="90%"
                                        runat="server" Text='<%# Bind("Descripcion") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblTipoVariable %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblTipoVariable" runat="server" ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlTipoVariable" runat="server" SelectedValue='<%# Bind("Tipo") %>'>
                                        <asp:ListItem Text="<%$ Resources:Resource, lblVariablesPropietarios %>" Value="P" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Resource, lblVariablesHotel %>" Value="H"></asp:ListItem>
                                        <asp:ListItem Text="Constante" Value="C"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Valor Constante">
                                <ItemTemplate>
                                    <asp:Label ID="Label40" runat="server" Text='<%# Bind("Valor") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtValorConstante" Width="40px" runat="server" Text='<%# Bind("Valor") %>'></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtValor_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Custom,Numbers" 
                                        FilterMode="ValidChars" ValidChars="0123456789,.-" TargetControlID="txtValorConstante">
                                    </asp:FilteredTextBoxExtender>
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblActivo %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" Checked='<%# Bind("Activo") %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chActivo" Checked='<%# Bind("Activo") %>' runat="server" AutoPostBack="true" 
                                        oncheckedchanged="chActivo_CheckedChanged" />
                                </EditItemTemplate>
                                <ItemStyle BorderColor="#7599A9" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center"  />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                    </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="NuevoVariable" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="2" align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, DatosVariables %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" class="textoTabla">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblNombre %>"></asp:Label> *
                    </td>
                    <td  style="width: 85%">
                        <asp:TextBox ID="txtNombre" MaxLength="100" runat="server" Width="80%" ValidationGroup="Guardar"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                            FilterMode="ValidChars" ValidChars="qwertyuioplkjhgfdsazxcvbnm_QWERTYUIOPLKJHGFDSAZXCVBNM" FilterType="Custom" TargetControlID="txtNombre">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                            ControlToValidate="txtNombre" Display="Dynamic" CssClass="error" ValidationGroup="Guardar">
                        </asp:RequiredFieldValidator>
                    </td>                                        
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblDescripcion %>"></asp:Label>
                    </td> 
                    <td valign="top">
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" MaxLength="300" runat="server"
                            Width="90%">
                        </asp:TextBox>
                    </td>                                       
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblTipoVariable %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoVariable" runat="server" Width="90%" 
                            onselectedindexchanged="ddlTipoVariable_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="<%$ Resources:Resource, lblVariablesPropietarios %>" Value="P" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Resource, lblVariablesHotel %>" Value="H"></asp:ListItem>
                            <asp:ListItem Text="Constante" Value="C"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblActivo %>"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chActivo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label3" runat="server" Text="Con Validación"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbEsConValidacion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">
                        <asp:Label ID="Label10" runat="server" Text="Numero Maximo"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumMaximo" Enabled="false" runat="server" Width="40px" Text="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td class="textoTabla">
                        <asp:Label ID="Label11" runat="server" Text="Valor Constante"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtValorConstante" Enabled="false" runat="server" Width="80px" Text="0"></asp:TextBox>
                        <asp:RequiredFieldValidator 
                            ID="rfv_ValorConstante" 
                            Enabled="false"
                            runat="server" 
                            ErrorMessage="*" 
                            ValidationGroup="reporte"
                            Text="*"
                            ControlToValidate="txtValorConstante">
                        </asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="txtValor_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Custom,Numbers" 
                            FilterMode="ValidChars" ValidChars="0123456789,.-" TargetControlID="txtValorConstante">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
        </div>
        
        
        <div style="display:">
            <%--<asp:TextBox ID="idCtrl" runat="server"></asp:TextBox>--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
