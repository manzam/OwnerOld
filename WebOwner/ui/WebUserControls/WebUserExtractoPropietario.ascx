<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserExtractoPropietario.ascx.cs"
    Inherits="WebOwner.ui.WebUserControls.WebUserExtractoPropietario" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
    
    <script language="javascript" type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j("#ctl00_Contenidoprincipal_WebUserExtractoPropietario1_txtFecha,#ctl00_Contenidoprincipal_WebUserExtractoPropietario1_txtFechaDesde").spinner({
                min: 2000,
                max: y
            });
        }
    });
    
    </script>
    
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
    <table width="100%">
        <tr>
            <td style="width:10%;" class="textoTabla">Fecha Desde</td>
            <td style="width:80%;">
                <asp:DropDownList ID="ddlMes" Width="110px" runat="server">
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
                <asp:TextBox ID="txtFecha" Enabled="false" runat="server" Width="40px"></asp:TextBox>
            </td>
        </tr>
        <td class="textoTabla">Fecha Hasta</td>
            <td>
                <asp:DropDownList ID="ddlMesHasta" Width="110px" runat="server">
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
                <asp:TextBox ID="txtFechaDesde" Enabled="false" runat="server" Width="40px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="textoTabla">Acumulado</td>
            <td>
                <asp:CheckBox ID="cbEsAcumulado" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    
    <asp:GridView 
        ID="gvwSuitsHotel" 
        runat="server" 
        Width="100%"
        AutoGenerateColumns="False" 
        CellPadding="2" 
        EmptyDataText="<%$ Resources:Resource, lblHotelSuite %>"
        BorderColor="#7599A9" 
        BorderStyle="Solid" 
        BorderWidth="1px">
        <PagerSettings Position="TopAndBottom" />
        <Columns>
            <asp:BoundField DataField="NombreHotel" HeaderText="<%$ Resources:Resource, lblHotel %>" >
                <HeaderStyle Width="70%" />
                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>
            <asp:BoundField DataField="NumSuit" HeaderText="<%$ Resources:Resource, lblNumSuit %>" >
                <HeaderStyle Width="15%" />
                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="<%$ Resources:Resource, lblSeleccion %>">
                <ItemTemplate>                                        
                    <asp:Button 
                        ID="btnVerExtracto" 
                        runat="server" 
                        Text="<%$ Resources:Resource, btnVerExtracto %>"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdHotel") + "," + DataBinder.Eval(Container.DataItem,"IdSuit") %>' 
                        onclick="btnVerExtracto_Click"
                        ValidationGroup="GenerarExtracto" />
                </ItemTemplate>
                <HeaderStyle Width="15%" />
                <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <SelectedRowStyle ForeColor="White" BackColor="#4D606E" />
        <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
    </asp:GridView>
    
    
    <br />
            
    <asp:Panel ID="pnlReportes" runat="server">
                
        <dx:ReportToolbar ID="ReportToolbar_Reporte"
                          runat='server' 
                          Width='900px'
                          ShowDefaultButtons='False' 
                          ReportViewer="<%# ReportViewer_Reporte %>">
            <Items>
                <dx:ReportToolbarButton ItemKind='Search' />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind='PrintReport' />
                <dx:ReportToolbarButton ItemKind='PrintPage' />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' />
                <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' />
                <dx:ReportToolbarLabel ItemKind='PageLabel' />
                <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind='OfLabel' />
                <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
                <dx:ReportToolbarButton ItemKind='NextPage' />
                <dx:ReportToolbarButton ItemKind='LastPage' />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind='SaveToDisk' />
                <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>                            
                    <Elements>
                        <dx:ListElement Value='pdf' />
                    </Elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft='3px' MarginRight='3px' />
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
        
        <dx:ReportViewer ID="ReportViewer_Reporte" Width="900px" runat="server">
        </dx:ReportViewer>
        
    </asp:Panel>
