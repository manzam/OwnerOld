<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserConfigCertificado.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserCertificado" %>



<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>
    
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
    
        <table style="width:100%">
            <tr>
                <td style="width:15%;" class="textoTabla">Hotel</td>
                <td>
                    <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" Width="100%" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        
        <br />
        
        <table width="100%" cellpadding="3" cellspacing="0">
            <tr>
                <td colspan="4" style="text-align:center;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                    <br />
                    Nombre del Hotel
                    <br />
                    Nit del Hotel
                </td>
            </tr>
            <tr>
                <td>CERTIFICA</td>
            </tr>
            <tr>
                <td>
                    Que durante el año gravable xxxx (Hoteles Estelar) identifcado(a) con la cedula ó nit xxx.xxx.xxx recibo de nuestra compañia
                    la suma de #.###.### (valor en letras) por concepto de:
                    <asp:TextBox ID="txtDescripcion" runat="server" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr><td></td></tr>
            <tr>
                <td>
                
                    <asp:GridView 
                        ID="gvwConceptos" 
                        runat="server"
                        DataKeyNames="IdConcepto" 
                        Width="100%" 
                        AutoGenerateColumns="False" 
                        CellPadding="2"
                        EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                        BorderColor="#7599A9" 
                        BorderStyle="Solid" 
                        BorderWidth="1px" 
                        onrowdatabound="gvwConceptos_RowDataBound">
                        <Columns>                            
                            <asp:BoundField DataField="Nombre" HeaderText="<%$ Resources:Resource, lblConcepto %>" >
                                <HeaderStyle Width="55%" />
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblSeleccion %>">
                                <ItemTemplate>
                                    <asp:ImageButton 
                                        ID="imgBtnSeleccion" 
                                        runat="server"
                                        Height="35px" 
                                        Width="35px"                                    
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdConcepto") %>'
                                        ImageUrl="~/img/117.png" onclick="imgBtnSeleccion_Click" />
                                </ItemTemplate>
                                <HeaderStyle Width="15%" />
                                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>                        
                        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                    </asp:GridView>
                
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>Expide en la ciudad de xxxxx a los xx del mes de xxxxxxx del año xxxx</td>
            </tr>
        </table>

    </ContentTemplate>
</asp:UpdatePanel>