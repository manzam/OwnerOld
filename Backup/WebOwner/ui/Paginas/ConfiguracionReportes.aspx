<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ConfiguracionReportes.aspx.cs" Inherits="WebOwner.ui.Paginas.ConfiguracionReportes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>
    
<script type="text/javascript">

    var $j = jQuery.noConflict();

    $j(document).ready(function() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            $j("#ctl00_Contenidoprincipal_ddlGrupo").change(function() {

                var res = $j("#ctl00_Contenidoprincipal_ddlGrupo option:selected").text().split('-');

                $j("#ctl00_Contenidoprincipal_txtGrupo").val(res[1].trim());
                $j("#ctl00_Contenidoprincipal_txtOrden").val(res[0].trim());

            });
        }
    });    

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contenidoprincipal" runat="server">

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

    <div class="tituloPrincipal">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Configuracion Reportes"></asp:Label>
        </h2>
    </div> 
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
    
        <div class="botonera">
                <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" ValidationGroup="NuevoActualizar" onclick="btnGuardar_Click" />
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
                <td class="textoTabla" style="width:15%;">Reporte</td>
                <td style="width:85%;">
                    <asp:DropDownList ID="ddlReportes" runat="server" Width="90%" AutoPostBack="true" 
                        onselectedindexchanged="ddlReportes_SelectedIndexChanged">                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">Hotel</td>
                <td>
                    <asp:DropDownList ID="ddlHotel" runat="server" Width="90%" AutoPostBack="true" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        
        <br />
        
        <table width="100%">
            <tr>
                <td><h2>Campos</h2></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID="gvwVariables" 
                        runat="server" 
                        AutoGenerateColumns="False" 
                        BorderStyle="Solid" 
                        BorderWidth="1px" 
                        BorderColor="#7599A9" 
                        CellPadding="2" 
                        DataSourceID="odsVariables"
                        DataKeyNames="IdVariable,Tipo" 
                        EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"  
                        Width="100%"
                        AllowPaging="True" ondatabound="gvwVariables_DataBound" 
                        onselectedindexchanged="gvwVariables_SelectedIndexChanged" >
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" HeaderText="Selección" 
                                SelectText="Seleccionar." >
                                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" >
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                        <SelectedRowStyle BackColor="#FFFAAE" ForeColor="#585858" />
                    </asp:GridView>
                    <asp:ObjectDataSource 
                        ID="odsVariables" 
                        runat="server" 
                        StartRowIndexParameterName="inicio" 
                        MaximumRowsParameterName="fin"
                        EnablePaging="True" 
                        SelectMethod="ListaVariables"
                        SelectCountMethod="ListaVariablesCount"
                        TypeName="BO.ConfiguracionReporteBo">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlHotel" Name="idHotel" PropertyName="SelectedValue" Type="Int32" />
                            <asp:Parameter Name="inicio" Type="Int32" />
                            <asp:Parameter Name="fin" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>

        <asp:Panel ID="pnlConsolidadoSuite" runat="server" Visible="false">  
            <table width="100%">     
                <tr>
                    <td><h2>Detalle</h2></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView 
                            ID="gvwDetalleReporte" 
                            runat="server" 
                            AutoGenerateColumns="False" 
                            BorderStyle="Solid" 
                            BorderWidth="1px" 
                            BorderColor="#7599A9" 
                            CellPadding="2"
                            DataKeyNames="IdReporteDetalle" 
                            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
                            Width="100%">
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                    <HeaderStyle Width="95%" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Orden">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOrden" Text='<%# Bind("Orden") %>' Width="60px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender 
                                            ID="txtOrden_FilteredTextBoxExtender" 
                                            runat="server" 
                                            FilterType="Numbers"
                                            Enabled="True" 
                                            TargetControlID="txtOrden">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" 
                                            CommandArgument='<%# Bind("IdReporteDetalle") %>' ImageUrl="~/img/126.png" 
                                            Height="25px" Width="25px" runat="server" onclick="imgBtnEliminar_Click" />                                    
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>
        
        
        <asp:Panel ID="pnlConsolidadoPropietario" runat="server" Visible="false">
        
            <table width="100%">
                <tr>
                    <td class="textoTabla" style="width:15%">Grupo</td>
                    <td style="width:85%;">
                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="80%" >
                            <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                        </asp:DropDownList>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td class="textoTabla">Nombre Grupo</td>
                    <td>
                        <asp:TextBox ID="txtGrupo" runat="server" Width="50%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
                            ControlToValidate="txtGrupo" Display="Dynamic" CssClass="error" ValidationGroup="NuevoEditarGrupo" > 
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla">Orden</td>
                    <td>
                        <asp:TextBox ID="txtOrden" runat="server" Width="10%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtOrden">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                            ControlToValidate="txtOrden" Display="Dynamic" CssClass="error" ValidationGroup="NuevoEditarGrupo" > 
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnNuevoGrupo" runat="server" Text="Nuevo Grupo" 
                            onclick="btnNuevoGrupo_Click" ValidationGroup="NuevoEditarGrupo" />&nbsp;&nbsp;
                        <asp:Button ID="btnEditarGrupo" runat="server" Text="Editar Grupo" 
                            onclick="btnEditarGrupo_Click" ValidationGroup="NuevoEditarGrupo" />&nbsp;&nbsp;
                        <asp:Button ID="btnEliminarGrupo" runat="server" Text="Eliminar Grupo" 
                            onclick="btnEliminarGrupo_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><h2>Configuracion</h2></td>
                </tr>
                <tr>
                    <td colspan="2">
                    
                        <asp:GridView 
                            ID="gvwConfiguracion" 
                            runat="server" 
                            AutoGenerateColumns="False" 
                            BorderStyle="Solid" 
                            BorderWidth="1px" 
                            BorderColor="#7599A9" 
                            CellPadding="2"
                            DataKeyNames="IdReporteGrupoDetalle,IdVariable,Tipo" 
                            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"  
                            Width="100%" >
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Concepto" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                    <HeaderStyle Width="43%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Grupo" >
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                                    <HeaderStyle Width="42%" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnEliminar" ImageUrl="~/img/126.png" Width="25px" CommandArgument='<%# BIND("IdReporteGrupoDetalle") %>' 
                                            Height="25px" runat="server" onclick="imgBtnEliminar_Click1" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                            <SelectedRowStyle BackColor="#FFFAAE" ForeColor="#585858" />
                        </asp:GridView>
                    
                    </td>                    
                </tr>
            </table>
        
        </asp:Panel>
        
   </ContentTemplate>
</asp:UpdatePanel>   

</asp:Content>
