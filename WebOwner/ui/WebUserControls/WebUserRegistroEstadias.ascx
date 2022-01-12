<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserRegistroEstadias.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserRegistroEstadias" %>

<%@ Register src="WebUserBuscadorPropietarioSuite.ascx" tagname="WebUserBuscadorPropietarioSuite" tagprefix="uc1" %>

<script language="javascript" type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            $j(".miFecha").datepicker({
                showOn: "button",
                buttonImage: "../../img/calendar.gif",
                dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: false,
                dateFormat: "dd/mm/yy"
            });

            $j("#modalBuscadorPropietario").dialog({
                width: 900,
                autoOpen: false,
                resizable: false,
                show: "slow",
                modal: true,
                height: "auto",
                buttons: {
                    "Aceptar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserRegistroEstadias1_uc_WebUserBuscadorPropietario_btnAceptar').click();
                        $j(this).dialog("close");
                    },
                    "Cancelar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserRegistroEstadias1_uc_WebUserBuscadorPropietario_btnCancelar').click();
                        $j(this).dialog("close");
                    }
                }
            }).parent().appendTo($j("form:first")).css('z-index', '1005');
            
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
            <asp:Button ID="btnVerTodos" runat="server" 
                Text="<%$ Resources:Resource, btnVerTodos %>" ValidationGroup="VerTodos" onclick="btnVerTodos_Click" />
            <asp:Button ID="btnBuscar" runat="server" 
                Text="<%$ Resources:Resource, btnBuscar %>" 
                OnClientClick="$j('#modalBuscadorPropietario').dialog('open')" 
                onclick="btnBuscar_Click" />
        </div>       
        
        <br />
        
         <div runat="server" class="cuadradoExito" id="div1" visible="false">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/33.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="Label10" runat="server" CssClass="textoExito" ></asp:Label>
        </div>
        
        <div runat="server" class="cuadradoError" id="div2" visible="false">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/115.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <asp:Label ID="Label11" runat="server" CssClass="textoError" ></asp:Label>
        </div>
        
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
        
        <asp:Panel ID="pnlGrillaPropietarios" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td style="width:15%" class="textoTabla">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                    </td>
                    <td style="width:85%">
                        <asp:DropDownList ID="ddlhotel" runat="server" Width="350px" 
                            onselectedindexchanged="ddlhotel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <br />
            
            <asp:GridView ID="gvwPropietario" runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" BorderWidth="1px" BorderColor="#7599A9"
                CellPadding="2"
                AllowPaging="true"
                DataKeyNames="IdPropietario,IdSuit,NumEstadias,NumEscritura,NumSuit" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                Width="100%" 
                onpageindexchanging="gvwPropietario_PageIndexChanging" 
                onselectedindexchanging="gvwPropietario_SelectedIndexChanging" >
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbNombre" runat="server" CommandName="Select" 
                                Text='<%# Bind("Nombre") %>'></asp:LinkButton>                                    
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="NumSuit" HeaderText="<%$ Resources:Resource, lblNumSuit %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NumEscritura" HeaderText="<%$ Resources:Resource, lblEscritura %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreCiudad" HeaderText="<%$ Resources:Resource, lblCiudad %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
            
            <br />
            
        </asp:Panel>
        
        <asp:Panel ID="pnlDetallePropietario" runat="server" Visible="false">
        
            <table width="100%" cellspacing="0" cellpadding="3">
                <tr>
                    <td style="width:20%;" class="textoTabla">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblNombre %>"></asp:Label>
                    </td>
                    <td style="width:60%;"><asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></td>
                    <td style="width:15%;" class="textoTabla">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblNumSuit %>"></asp:Label>
                    </td>
                    <td style="width:15%;"><asp:Label ID="lblNumSuit" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td class="textoTabla"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblEstadias %>"></asp:Label></td>
                    <td><asp:Label ID="lblNumEstadias" runat="server" Text="Label"></asp:Label></td>
                    <td class="textoTabla"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblEscritura %>"></asp:Label></td>
                    <td><asp:Label ID="lblEscritura" runat="server" Text="Label"></asp:Label></td>
                </tr>
            </table>
            
            <br />
            
            <table width="100%">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td style="width:20%;" class="textoTabla">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblFechaFiltro %>"></asp:Label>
                                </td>
                                <td style="width:80%;">
                                    <asp:DropDownList ID="ddlMes" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="ddlMes_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width:80%; vertical-align:top;">
                        <asp:GridView ID="gvwEstadias" runat="server" 
                            AutoGenerateColumns="False" 
                            BorderStyle="Solid" 
                            BorderWidth="1px"                            
                            ShowFooter="true"
                            BorderColor="#7599A9"
                            CellPadding="2"
                            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                            Width="100%" onrowdatabound="gvwEstadias_RowDataBound" 
                            onrowcreated="gvwEstadias_RowCreated" >
                            <Columns>
                                <asp:BoundField DataField="FechaLlegada" DataFormatString="{0:dd-MM-yyyy}" HeaderText="<%$ Resources:Resource, lblFechaInicio %>" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="15%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaSalida" DataFormatString="{0:dd-MM-yyyy}" HeaderText="<%$ Resources:Resource, lblFechaFin %>" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="15%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NumEstadias" HeaderText="<%$ Resources:Resource, lblNumDias %>" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="10%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:Resource, lblDescripcion %>" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="50%" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nombre" HeaderText="<%$ Resources:Resource, lblNombre %>" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="10%" HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                        </asp:GridView>
                    </td>
                    <td style="width:20%; vertical-align:top;">
                        <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                                <td style="width:35%" class="textoTabla">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblFechaInicio %>"></asp:Label>
                                    <span class="error" id="errorI" runat="server" visible="false">*</span>
                                </td>
                                <td style="width:65%">
                                    <asp:TextBox ID="txtFechaInicio" CssClass="miFecha" Enabled="false" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblFechaFin %>"></asp:Label>
                                    <span class="error" id="errorF" runat="server" visible="false">*</span>
                                </td>
                                <td style="width:80%">
                                    <asp:TextBox ID="txtFechaFin" CssClass="miFecha" Enabled="false" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top;" class="textoTabla">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblObservacion %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Columns="20" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align:center;">
                                    <asp:Button ID="btnGuardar" runat="server" 
                                        Text="<%$ Resources:Resource, btnGuardar %>" onclick="btnGuardar_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>       
        
    </ContentTemplate>
</asp:UpdatePanel>

<div id="modalBuscadorPropietario">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:WebUserBuscadorPropietario ID="uc_WebUserBuscadorPropietarioSuite" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>            
</div>
