<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserHistorialLiquidacionCliente.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserHistorialLiquidacionCliente" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
    
<script language="javascript" type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        var newIdx = -1;
        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j("[id$=txtAnoDesde]").spinner({
                min: 2000,
                max: y
            });

            $j("[id$=txtAnoHasta]").spinner({
                min: 2000,
                max: y
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
    
        <table width="70%">
            <tr>
                <td style="width:20%;" class="textoTabla">
                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                </td>
                <td style="width:80%;">
                    <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlHotel_SelectedIndexChanged" Width="450px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblNumSuit %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSuite" runat="server" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label>
                    <span class="error" id="error" runat="server" visible="false">*</span>
                </td>
                <td>
                    De :
                    &nbsp;
                    <asp:DropDownList ID="ddlMesDesde" Width="110px" runat="server">
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
                    <asp:TextBox ID="txtAnoDesde" Enabled="false" runat="server" Width="40px"></asp:TextBox>
                    &nbsp;&nbsp;
                    Hasta :
                    &nbsp;
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
                    <asp:TextBox ID="txtAnoHasta" Enabled="false" runat="server" Width="40px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <asp:Button ID="btnAceptar" runat="server" 
                        Text="<%$ Resources:Resource, btnAceptar %>" onclick="btnAceptar_Click" />
                </td>
            </tr>
        </table>
        
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
        
        <asp:Panel ID="Panel1" runat="server" Width="900px" ScrollBars="Horizontal" 
            BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px">
        
                <dx:ReportToolbar 
                    ID="ReportToolbar1"
                    runat='server' 
                    Width='900px'
                    ReportViewer="<%# ReportViewer_HistorialLiquidacion %>"
                    ShowDefaultButtons='False'>
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
                              <dx:ListElement Value='xls' />
                              <dx:ListElement Value='xlsx' />
                              <dx:ListElement Value='rtf' />
                              <dx:ListElement Value='mht' />
                              <dx:ListElement Value='html' />
                              <dx:ListElement Value='txt' />
                              <dx:ListElement Value='csv' />
                              <dx:ListElement Value='png' />
                          </Elements>
                      </dx:ReportToolbarComboBox>
                  </Items>
                  <Styles>
                      <LabelStyle>
                          <Margins MarginLeft='3px' MarginRight='3px' />
                      </LabelStyle>
                  </Styles>
              </dx:ReportToolbar>
                  
              <dx:ReportViewer ID="ReportViewer_HistorialLiquidacion" runat="server">
              </dx:ReportViewer>
        
        </asp:Panel>
      
    </ContentTemplate>
</asp:UpdatePanel>