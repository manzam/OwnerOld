<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserReserva.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserReserva" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript">

        $j(document).ready(function() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

            CargasIniciales(null, null);

            function CargasIniciales(sender, args) {

                $j("#ctl00_Contenidoprincipal_WebUserReserva1_txtFechaLlegada").datepicker({
                    showOn: "button",
                    buttonImage: "../../img/calendar.gif",
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                    buttonImageOnly: true,
                    dateFormat: "dd/mm/yy"
                });

                $j("#ctl00_Contenidoprincipal_WebUserReserva1_txtFechaSalida").datepicker({
                    showOn: "button",
                    buttonImage: "../../img/calendar.gif",
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                    buttonImageOnly: true,
                    dateFormat: "dd/mm/yy"
                });
            
            }
        });


        function ValidarFechas() {
        
            $j('#errorFechaSalida').hide();
            $j('#errorFechaLlegada').hide();

            if ($j('#ctl00_Contenidoprincipal_WebUserReserva1_txtFechaLlegada').val() == "") {
                $j('#errorFechaLlegada').show();
                return false;
            }
            else if ($j('#ctl00_Contenidoprincipal_WebUserReserva1_txtFechaSalida').val() == "") {
                $j('#errorFechaSalida').show();
                return false;
            }
            else
                return true;
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
            Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="NuevoActualizar" onclick="btnGuardar_Click" OnClientClick="return ValidarFechas();" />        
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

    <div id="NuevoReserva" runat="server" visible="false">
    <table style="width: 100%;">
        <tr>
            <td colspan="8" align="center">
                <div class="tituloGrilla">
                    <h2>
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosReserva %>"></asp:Label>                          
                    </h2>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width:12%;">
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
            </td>
            <td style="width:12%;">
                <asp:DropDownList ID="ddlHotel" runat="server" Width="95%" AutoPostBack="true" 
                    onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width:12%;">
                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblCiudad %>"></asp:Label>
            </td>
            <td style="width:12%;">
                <asp:Label ID="lblCiudad" runat="server" Width="100%"></asp:Label>
            </td>
            <td style="width:12%;">
                &nbsp;
            </td>
            <td style="width:12%;">
                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, lblSuit %>"></asp:Label>
            </td>
            <td style="width:12%;">
                <asp:DropDownList ID="ddlSuit" runat="server" Width="100%" AutoPostBack="true" 
                    onselectedindexchanged="ddlSuit_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width:12%;">
                <asp:Label ID="lblDescripcionSuit" runat="server" Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblEntrada %>"></asp:Label>
                <span id="errorFechaLlegada" style="display:none;" class="error">*</span>
            </td>
            <td>
                <input id="txtFechaLlegada" runat="server" type="text" style="width:60%;" />
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblSalida %>"></asp:Label>
                <span id="errorFechaSalida" style="display:none;" class="error">*</span>
            </td>
            <td>
                <input id="txtFechaSalida" runat="server" type="text" style="width:60%;" />
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblAdulto %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNumAdultos" runat="server" Width="60%"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                FilterType="Numbers"
                TargetControlID="txtNumAdultos">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblNinos %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNumNinos" runat="server" Width="60%"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                FilterType="Numbers"
                TargetControlID="txtNumNinos">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, lblObservacion %>"></asp:Label>
            </td>
            <td colspan="7">
                <asp:TextBox ID="txtObervacion" Width="100%" Height="150px" MaxLength="300" TextMode="MultiLine" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>


    <div id="GrillaHotel" runat="server">
    <table style="width: 100%;">
        <tr>
            <td align="center">
                <div class="tituloGrilla">
                    <h2>
                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, TituloGrillaReservas %>"></asp:Label>
                    </h2>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvwReservas" 
                              runat="server" 
                              DataKeyNames="IdSuit" 
                              Width="100%" 
                              AutoGenerateColumns="False" 
                              CellPadding="2"
                              EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" BorderColor="#7599A9" 
                              BorderStyle="Solid" BorderWidth="1px">
                    <Columns>
                        <asp:BoundField DataField="NombreHotel" HeaderText="<%$ Resources:Resource, lblHotel %>" >
                            <HeaderStyle Width="20%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NombreCiudad" HeaderText="<%$ Resources:Resource, lblCiudad %>" >
                            <HeaderStyle Width="15%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NumSuit" HeaderText="<%$ Resources:Resource, lblNumSuit %>" >
                            <HeaderStyle Width="8%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Fecha" HeaderText="<%$ Resources:Resource, lblFechaReserva %>" >
                            <HeaderStyle Width="12%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaLlegada" HeaderText="<%$ Resources:Resource, lblEntrada %>" DataFormatString="{0:dd/MM/yyyy}" >
                            <HeaderStyle Width="10%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaSalida" HeaderText="<%$ Resources:Resource, lblSalida %>" DataFormatString="{0:dd/MM/yyyy}" >
                            <HeaderStyle Width="10%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:Resource, lblObservacion %>" >
                            <HeaderStyle Width="25%" />
                            <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                </asp:GridView>
            </td>
        </tr>        
    </table>
</div>

</ContentTemplate>
</asp:UpdatePanel>
