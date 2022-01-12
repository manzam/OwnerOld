<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserValorVariable.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserValorVariable" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<script type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var str = $j("#ctl00_Contenidoprincipal_WebUserValorVariable1_lbltextoError").text();
            $j("#ctl00_Contenidoprincipal_WebUserValorVariable1_lbltextoError").html(str);
        }

        $j("#modalReporteVariables").dialog({
            width: 1150,
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: 800,
            buttons: {
                "Salir": function() {
                    $j(this).dialog("close");
                }
            }
        }).parent().appendTo($j("form:first")).css('z-index', '1005');
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
            <asp:Button ID="btnGuardar" runat="server" Text="<%$ Resources:Resource, btnGuardar %>" onclick="btnGuardar_Click" />
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
        
        <br />
        
        
        
        <table width="70%">
            <tr>
                <td style="width:10%;" class="textoTabla">
                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label> *
                </td>
                <td style="width:90%;">
                    <asp:DropDownList ID="ddlMes" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlMes_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Enero" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                        <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                        <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                        <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                        <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                        <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                        <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                        <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                        <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                        <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                    </asp:DropDownList>
                    -
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="true" onselectedindexchanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label> *
                </td>                
                <td>
                    <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" Width="70%" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <br />
        
        <asp:GridView 
            ID="gvwVariable" 
            runat="server" 
            AutoGenerateColumns="False" 
            BorderStyle="Solid" 
            BorderWidth="1px" 
            BorderColor="#7599A9" 
            CellPadding="2"
            DataKeyNames="IdVariable" 
            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>"
            Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                    <ItemTemplate>
                        <asp:Label ID="lblNombreVariable" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="70%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblValor %>" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtValor" runat="server" Width="100px" Text='<%# Bind("Valor") %>'></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtValor_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Custom,Numbers" 
                            FilterMode="ValidChars" ValidChars="0123456789,." TargetControlID="txtValor">
                        </asp:FilteredTextBoxExtender>
                    </ItemTemplate>
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" Width="30%" HorizontalAlign="Center" />
                </asp:TemplateField>               
            </Columns>
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>        
    
    </ContentTemplate>
</asp:UpdatePanel>
