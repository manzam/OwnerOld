<%@ Page Title="" Language="C#" MasterPageFile="~/templates/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="ReporteHistorialLiquidacion.aspx.cs" Inherits="WebOwner.ui.Paginas.ReporteHistorialLiquidacion" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register src="~/ui/WebUserControls/WebUserBuscadorPropietarioSuite.ascx" tagname="WebUserBuscadorPropietarioSuite" tagprefix="uc1" %>  
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    var $j = jQuery.noConflict();
    $j(document).ready(function() {

        $j("#modalBuscadorPropietario").dialog({
            width: 1000,
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: "auto",
            open: function(event, ui) {
                var idCmb = $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_ddlHotelReporte').val();
                $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_uc_WebUserBuscadorPropietario_ddlHotelBuscador').val(idCmb);
            },
            close: function(event, ui) {
                var idCmb = $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_uc_WebUserBuscadorPropietario_ddlHotelBuscadorctl0').val();
                $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_ddlHotelReporte').val(idCmb);
            },
            buttons: {
                "Aceptar": function() {
                    $j('#ctl00_Contenidoprincipal_uc_WebUserBuscadorPropietarioSuite_btnAceptar').click();
                    $j(this).dialog("close");
                },
                "Cancelar": function() {
                    $j('#ctl00$Contenidoprincipal$uc_WebUserBuscadorPropietarioSuite$btnCancelar').click();
                    $j(this).dialog("close");
                }
            }
        }).parent().appendTo($j("form:first")).css('z-index', '1005');

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);
        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j("[id$=txtFechaDesde]").spinner({
                min: 2000,
                max: y
            });

            $j("[id$=txtFechaHasta]").spinner({
                min: 2000,
                max: y
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
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
                    <td style="width:20%;" class="textoTabla">Reporte</td>
                    <td style="width:80%;">
                        <asp:DropDownList ID="ddlReporte" AutoPostBack="true" runat="server" 
                            Width="450px" onselectedindexchanged="ddlReporte_SelectedIndexChanged">
                            <asp:ListItem Text="Seleccione" Value="S"></asp:ListItem>
                            <asp:ListItem Text="Propietarios - Suite" Value="uno"></asp:ListItem>
                            <asp:ListItem Text="Hotel - Suite" Value="dos"></asp:ListItem>
                            <asp:ListItem Text="Variables" Value="tres"></asp:ListItem>
                            <asp:ListItem Text="Usuarios" Value="cuatro"></asp:ListItem>
                            <asp:ListItem Text="Perfil - Permisos" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Historial Liquidacion" Value="H" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Consolidado por Suite" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Consolidado por Propietario" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Extracto Consolidado" Value="EC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <br />
            
            <table width="100%">
                <tr>
                    <td class="textoTabla" style="text-align:center;">
                        Filtros                    
                    </td>
                </tr>
            </table>
            
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <div class="tituloPrincipal">
                                <h2>
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, lblHistorialLiquidacion %>"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;" class="textoTabla">
                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                        </td>
                        <td style="width:85%;">
                            <asp:DropDownList ID="ddlHotelReporte" Width="450px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label11" runat="server" Text="Fecha desde"></asp:Label>                            
                        </td>
                        <td>                        
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
                            <asp:TextBox ID="txtFechaDesde" Enabled="false" runat="server" Width="40px" ValidationGroup="Reporte"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="Fecha hasta"></asp:Label>                            
                        </td>
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
                            <asp:TextBox ID="txtFechaHasta" Enabled="false" runat="server" Width="40px" ValidationGroup="Reporte"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:left">
                            <asp:Button ID="btnReporteLiquidacionHotel" runat="server" Text="<%$ Resources:Resource, lblLiquidacionHotel %>" 
                                OnClick="btnReporteLiquidacionHotel_Click" ValidationGroup="Reporte" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnReporteLiquidacionPropietario" runat="server" Text="<%$ Resources:Resource, lblLiquidacionPropietario %>" 
                                OnClick="btnReporteLiquidacionPropietario_Click" ValidationGroup="Reporte" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button1" runat="server" Text="Buscar Propietario" 
                                OnClientClick="$j('#modalBuscadorPropietario').dialog('open')" onclick="btnBuscarHistorial_Click" />                            
                        </td>
                    </tr>
                </table>
                
                <br />
            
              <asp:Panel ID="Panel1" runat="server" Width="900px" ScrollBars="Both">
                  <dx:ReportToolbar 
                    ID="ReportToolbar1" 
                    runat='server' 
                    Width='900px'
                    ReportViewer="<%# ReportViewer_Liquidacion %>"
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
                  
                  <dx:ReportViewer ID="ReportViewer_Liquidacion" runat="server">
                  </dx:ReportViewer>
              </asp:Panel>             
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="modalBuscadorPropietario">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:WebUserBuscadorPropietarioSuite ID="uc_WebUserBuscadorPropietarioSuite" runat="server" />
            </ContentTemplate>
            <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            </Triggers>--%>
        </asp:UpdatePanel>            
    </div>
        
</asp:Content>
