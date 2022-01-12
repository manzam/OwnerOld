<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserHotel.ascx.cs" Inherits="WebOwner.UI.WebControls.WebUserHotel" %>
    
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>    
    <%@ Register src="WebUserBuscadorSuite.ascx" tagname="WebUserBuscadorSuite" tagprefix="uc1" %>
    
    <script type="text/javascript">

        $j(document).ready(function() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

            CargasIniciales(null, null);

            function CargasIniciales(sender, args) {

                $j("#ctl00_Contenidoprincipal_WebUserHotel1_txtArea").numeric({ decimal: ",", negative: false });

                $j("#ctl00_Contenidoprincipal_WebUserHotel1_modalSuit").dialog({
                    width: 850,
                    autoOpen: false,
                    resizable: false,
                    show: "slow",
                    modal: true,
                    height: "auto",
                    buttons: {
                        "Aceptar": function() {
                            $j('#ctl00_Contenidoprincipal_WebUserHotel1_btnAceptarSuit').click();
                            if (Page_ClientValidate("AceptarSuit"))
                                $j(this).dialog("close");
                        },
                        "Cancelar": function() {
                            $j('#ctl00_Contenidoprincipal_WebUserHotel1_RequiredFieldValidator4').hide()
                            $j(this).dialog("close");
                        }
                    }
                }).parent().appendTo($j("form:first")).css('z-index', '1005');

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

                $j("#modalBuscadorSuite").dialog({
                    width: 1000,
                    autoOpen: false,
                    resizable: false,
                    show: "slow",
                    modal: true,
                    height: "auto",
                    buttons: {
                        "Aceptar": function() {
                            $j('#ctl00_Contenidoprincipal_WebUserHotel1_WebUserBuscadorSuite_btnAceptar').click();
                            $j(this).dialog("close");
                        },
                        "Cancelar": function() {
                            $j('#ctl00_Contenidoprincipal_WebUserHotel1_WebUserBuscadorSuite_btnCancelar').click();
                            $j(this).dialog("close");
                        }
                    }
                }).parent().appendTo($j("form:first")).css('z-index', '1005');
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
            <asp:Button ID="btnBuscar" runat="server" 
                Text="<%$ Resources:Resource, btnBuscar %>" Visible="false" 
                OnClientClick="$j('#modalBuscadorSuite').dialog('open')" 
                onclick="btnBuscar_Click" />
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
        
        <div id="NuevoHotel" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="4" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosHotel %>"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:15%;" class="textoTabla">
                            <asp:Label ID="Label4" runat="server" Text="Nombre"></asp:Label> *
                        </td>
                        <td style="width:35%;" >
                            <asp:TextBox ID="txtNombre" MaxLength="80" runat="server" Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNombre" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>
                        <td style="width:15%; text-align:left;" class="textoTabla">
                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblNit %>"></asp:Label> *
                        </td>
                        <td style="width:35%;" >
                            <asp:TextBox ID="txtNit" MaxLength="15" runat="server" Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNit" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" > </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblDireccion %>"></asp:Label> *
                        </td>
                        <td >
                            <asp:TextBox ID="txtDireccion" MaxLength="50" runat="server"  Width="90%" ValidationGroup="NuevoActualizar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtDireccion" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" > 
                            </asp:RequiredFieldValidator>
                            
                        </td>
                        <td text-align:left;" class="textoTabla">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblCorreo %>"></asp:Label>                            
                        </td>
                        <td >
                            <asp:TextBox ID="txtCorreo" MaxLength="50" runat="server"  Width="90%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtCorreo" ForeColor="#FF6C6C" CssClass="error"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" > 
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblDepto %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepto"  Width="90%" runat="server" 
                                OnSelectedIndexChanged="ddlDepto_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;" class="textoTabla">
                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblCiudad %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCiudad"  Width="90%" runat="server">
                                <asp:ListItem Selected="True" Text="Seleccione..." Value=" "></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Resource, lblCodigo %>"></asp:Label> *
                        </td>
                        <td >
                            <asp:TextBox ID="txtCodigo" runat="server" Width="90%" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtCodigo" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" > 
                            </asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align:left;" class="textoTabla">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblUnidadNegocio %>"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="txtUnidadNegocio" runat="server" Width="90%" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="textoTabla">
                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Resource, lblLogo %>"></asp:Label>
                        </td>
                        <td colspan="2" valign="top" >
                            <asp:AsyncFileUpload 
                                ID="AsyncFileUpload1"                                 
                                runat="server" 
                                Enabled="false" 
                                PersistFile="true"
                                OnClientUploadError="uploadError"
                                OnClientUploadComplete="uploadComplete"
                                onuploadedcomplete="AsyncFileUpload1_UploadedComplete" />                               
                            
                           <asp:Label ID="lblMesg" runat="server" Text=""></asp:Label>
                        </td>
                        <td valign="top" >
                            <asp:Image ID="imgLogo" runat="server" Width="80px" Height="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAgregarSuit" 
                                        runat="server" 
                                        Text="<%$ Resources:Resource, btnAgregarSuit %>" 
                                        OnClientClick='$j("#ctl00_Contenidoprincipal_WebUserHotel1_txtNumSuit").val("");
                                                       $j("#ctl00_Contenidoprincipal_WebUserHotel1_txtDescripcionSuit").val("");
                                                       $j("#ctl00_Contenidoprincipal_WebUserHotel1_txtArea").val("");
                                                       $j("#ctl00_Contenidoprincipal_WebUserHotel1_txtEscritura").val("");
                                                       $j("#ctl00_Contenidoprincipal_WebUserHotel1_txtDescripcionSuit").val("");
                                                       $j("#ctl00_Contenidoprincipal_WebUserHotel1_txtRegistroNotaria").val("");
                                                       $j("#ctl00_Contenidoprincipal_WebUserHotel1_modalSuit").dialog( "open" );' 
                                        ValidationGroup="AgregarSuit" />
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
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, TituloGrillaSuit %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwSuits" 
                                      runat="server" 
                                      DataKeyNames="IdSuit" 
                                      Width="100%" 
                                      AutoGenerateColumns="False" 
                                      CellPadding="2"
                                      EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                                      BorderColor="#7599A9" 
                                      BorderStyle="Solid" 
                                      BorderWidth="1px" 
                                      onselectedindexchanged="gvwSuits_SelectedIndexChanged" 
                                      onrowcancelingedit="gvwSuits_RowCancelingEdit" 
                                      onrowediting="gvwSuits_RowEditing" 
                                      onrowupdating="gvwSuits_RowUpdating"
                                      AllowPaging="True" 
                                      onpageindexchanging="gvwSuits_PageIndexChanging">
                            <Columns>
                                <asp:CommandField ButtonType="Image" ShowEditButton="true" HeaderText="<%$ Resources:Resource, lblAccion %>" 
                                                  UpdateImageUrl="~/img/131.png"
                                                  CancelImageUrl="~/img/72.png" 
                                                  DeleteImageUrl="~/img/126.png"
                                                  EditImageUrl="~/img/13.png" >
                                    <ControlStyle Height="30px" Width="30px" />
                                    <HeaderStyle Width="15%" />
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
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdSuit") %>'
                                            onclick="imgBtnEliminar_Click" />
                                    </ItemTemplate>
                                    <ControlStyle Height="30px" Width="30px" />
                                    <ItemStyle HorizontalAlign="Center" BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px"/>
                                    <HeaderStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNumSuit %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("NumSuit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNumSuit" runat="server" Width="70%" Text='<%# Bind("NumSuit") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblDescripcion %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" runat="server" Width="95%" Text='<%# Bind("Descripcion") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="45%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblEscritura %>">                                    
                                    <ItemTemplate>
                                        <asp:Label ID="Label19" runat="server" Text='<%# Bind("NumEscritura") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEscritura" Width="80%" runat="server" Text='<%# Bind("NumEscritura") %>'></asp:TextBox>
                                    </EditItemTemplate>                                    
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblRegistroNotaria %>">                                    
                                    <ItemTemplate>
                                        <asp:Label ID="Label23" runat="server" Text='<%# Bind("RegistroNotaria") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRegistroNotaria" Width="80%" runat="server" Text='<%# Bind("RegistroNotaria") %>'></asp:TextBox>
                                    </EditItemTemplate>                                    
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblActivo %>">                                    
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>' Enabled="false" />                                    
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chActivo" runat="server" Checked='<%# Bind("Activo") %>' />
                                    </EditItemTemplate>                                    
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                            <SelectedRowStyle BackColor="#FFFAAE" ForeColor="#585858" />
                        </asp:GridView>         
<%--                        <asp:ObjectDataSource 
                            ID="odsSuit" 
                            runat="server" SelectMethod="ObtenerSuitsPorHotel" TypeName="BO.SuitBo">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="gvwHoteles" Name="idHotel" 
                                    PropertyName="DataKeys" Type="Int32" />
                                <asp:Parameter Name="inicio" Type="Int32" />
                                <asp:Parameter Name="fin" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>--%>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="modalSuit" runat="server">
            <table width="100%" id="tablaSuits">
                <tr>
                    <td colspan="3" align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, DatosSuit %>"></asp:Label>
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>                            
                    <td style="width:20%; text-align:center;" class="textoTabla">
                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, lblNumSuit %>"></asp:Label>                        
                    </td>
                    <td style="width:20%; text-align:center;" class="textoTabla">                        
                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, lblActivo %>"></asp:Label>
                    </td>           
                    <td style="width:60%" class="textoTabla">                        
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, lblDescripcion %>"></asp:Label>
                    </td>
                </tr>
                <tr>                    
                    <td style="text-align:center;" valign="top">
                        <input id="txtNumSuit" maxlength="10" type="text" style="width:50%;" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNumSuit" Display="Dynamic" CssClass="error" ValidationGroup="AceptarSuit" > 
                        </asp:RequiredFieldValidator>
                    </td>
                    <td align="center" valign="top">
                        <asp:CheckBox ID="chActivoSuit" Checked="true" runat="server" ValidationGroup="AceptarSuit" />
                    </td>
                    <td rowspan="4">
                        <textarea id="txtDescripcionSuit" style="width:95%;height:100px;" runat="server" rows="2"></textarea>
                    </td>                    
                </tr>             
                <tr>
                    <td style="text-align:center;" valign="top" class="textoTabla">
                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, lblEscritura %>"></asp:Label>
                    </td>
                    <td align="center" valign="top" class="textoTabla">
                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, lblRegistroNotaria %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center;" valign="top" rowspan="2">
                        <input id="txtEscritura" maxlength="10" type="text" style="width:70%;" runat="server" />
                    </td>                    
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <input id="txtRegistroNotaria" maxlength="10" type="text" style="width:70%;" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="GrillaHotel" runat="server">
            
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, TituloGrillaHotel %>"></asp:Label>                          
                </h2>
            </div>
            
            <asp:GridView 
                ID="gvwHoteles" 
                runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                BorderColor="#7599A9" 
                CellPadding="2"
                DataKeyNames="IdHotel" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                OnSelectedIndexChanged="gvwHoteles_OnSelectedIndexChanged" 
                Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" 
                                Text='<%# Bind("Nombre") %>'></asp:LinkButton>                                    
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Nit" HeaderText="<%$ Resources:Resource, lblNit %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Direccion" HeaderText="<%$ Resources:Resource, lblDireccion %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
            <%--<asp:ObjectDataSource 
                ID="odsHotel" 
                runat="server" 
                SelectMethod="VerTodos" 
                SelectCountMethod="VerTodosCount"
                StartRowIndexParameterName="inicio"
                MaximumRowsParameterName="fin"
                EnablePaging="true"
                TypeName="BO.HotelBo">
                <SelectParameters>
                    <asp:Parameter Name="inicio" Type="Int32" />
                    <asp:Parameter Name="fin" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>
        </div>
        
        <div style="display:none;">
            <asp:Button ID="btnAceptarSuit" runat="server" Text="btnAceptarSuit" onclick="btnAceptarSuit_Click" ValidationGroup="AceptarSuit" />
            <asp:TextBox ID="txt" runat="server"></asp:TextBox>
        </div>
        
    </ContentTemplate>
</asp:UpdatePanel>


<div id="modalBuscadorSuite">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:WebUserBuscadorSuite ID="WebUserBuscadorSuite" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>            
</div>
