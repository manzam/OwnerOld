<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserLiquidacion.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserLiquidacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.2.Web, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register src="WebUserBuscadorPropietarioSuite.ascx" tagname="WebUserBuscadorPropietarioSuite" tagprefix="uc1" %>    

<style>
    .detailName {
        border-color:#7599A9; border-width:1px; border-style:Solid; text-align:left;
    }
    .detailCelda {
        border-color:#7599A9; border-width:1px; border-style:Solid; text-align:center;
    }
    .divOwnerDetail {
        overflow-y: scroll;
        height: 440px;
        padding-bottom: 15px;
        border-bottom: 1px solid black;
    }
    .headtableLiq {
        background-color:#7599A9; 
        border-color:#7599A9;
        color:white;
    }    

    .divOwnerLiq {
        background-color: white;
        overflow-x: scroll;
        overflow-y: scroll;
    }

    tbody#tblResultDetailLiq tr td {
        border-bottom: 1px solid #585858;
    }

</style>
<script src="../../js/liquidador.js?v=1003"></script>
<script src="../../js/jquery-number-master/jquery.number.min.js"></script>
<script language="javascript" type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        var newIdx = -1;
        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            //$j("#ctl00$Contenidoprincipal$WebUserLiquidacion1$btnReporteLiquidacionHotel").addClass("ui-button ui-widget ui-state-default ui-corner-all")

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
                        $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_uc_WebUserBuscadorPropietario_btnAceptar').click();
                        $j(this).dialog("close");
                    },
                    "Cancelar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_uc_WebUserBuscadorPropietario_btnCancelar').click();
                        $j(this).dialog("close");
                    }
                }
            }).parent().appendTo($j("form:first")).css('z-index', '100 5');

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
                        $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_btnEliminarLiqui').click();
                    },
                    "Cancelar": function() {
                        $j(this).dialog('close');
                    }
                }
            });

            $j("#modal_Buscador").dialog({
                width: 850,
                autoOpen: false,
                resizable: false,
                show: "slow",
                modal: true,
                height: "auto",
                buttons: {
                    "Aceptar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_btnAceptarBuscador').click();
                        $j(this).dialog("close");
                    },
                    "Cancelar": function() {
                        $j('#ctl00_Contenidoprincipal_WebUserLiquidacion1_btnCancelarBuscador').click();
                        $j(this).dialog("close");
                    }
                }
            }).parent().appendTo($j("form:first")).css('z-index', '1005');

            $j(function() {
                $j("#tabs").tabs({
                    activate: function() {
                        newIdx = $j('#tabs').tabs('option', 'active');
                    }, heightStyle: "auto",
                    active: previouslySelectedTab,
                    show: { effect: "fadeIn", duration: 1000 }
                });
            });

            $j('#tabs').tabs({ active: newIdx });

            $j("#div_DetalleConceptoHotel").draggable();

            var d = new Date();
            var y = d.getFullYear();

            $j("#txtYYYY").spinner({
                min: 2000,
                max: y,
                stop: function(event, ui) {
                    CargarPropietarios();
                }
            });

            $j("#txtFechaDeleteLiq").spinner({
                min: 2000,
                max: y
            });


        }
    });
    
</script>

<asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
        <div style="display:none;">
           <asp:HiddenField ID="hidLastTab" Value="0" runat="server" />
  <%--           <asp:Button ID="btnDetalleConceptoHotel" runat="server" 
            Text="DetalleConceptoHotel" onclick="btnDetalleConceptoHotel_Click" />--%>
        </div>
        
        <div class="cuadradoExito" id="divExito" style="display:none;">
            <asp:Image ID="imgExitoMsg" runat="server" ImageUrl="~/img/33.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <span id="lbltextoExito" class="textoExito"></span>
        </div>
        
        <div class="cuadradoError" id="divError" style="display:none;">
            <asp:Image ID="imgErrorMsg" runat="server" ImageUrl="~/img/115.png" 
                Width="20px" Height="20px" ImageAlign="AbsMiddle" />
            <span id="lbltextoError" class="textoError"></span>
        </div>
        
        <table>
            <tr>
                <td style="width:15%;" class="textoTabla">
                    <asp:Label ID="Label4" runat="server" Text="Hotel"></asp:Label> *
                </td>
                <td style="width:85%;">
                    <asp:DropDownList ID="ddlHotel" runat="server" Width="450px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label2" runat="server" Text="Fecha"></asp:Label> *
                </td>
                <td>
                    <select id="ddlMes" style="width:110px;">
                        <option value="1" selected="selected">Enero</option>
                        <option value="2">Febrero</option>
                        <option value="3">Marzo</option>
                        <option value="4">Abril</option>
                        <option value="5">Mayo</option>
                        <option value="6">Junio</option>
                        <option value="7">Julio</option>
                        <option value="8">Agosto</option>
                        <option value="9">Septiembre</option>
                        <option value="10">Octubre</option>
                        <option value="11">Noviembre</option>
                        <option value="12">Diciembre</option>
                    </select>
                    -
                    <input type="text" id="txtYYYY" disabled="disabled" style="width:40px" />
                </td>
            </tr>
        </table>
        
        <br />

        <div id="divValidParticipation" style="display:none">
            <div>
                <h2>Error participaciones propietarios</h2>
            </div>
            <div>
                <table id="tblValidadores" style="width:100%; border-spacing:0px;">
                    <thead>
                        <tr>
                            <td class="textoTabla" style="text-align: center;">No Suite</td>
                            <td class="textoTabla" style="text-align: center;">Propietario</td>
                            <td class="textoTabla" style="text-align: center;">No Identificación</td>
                            <td class="textoTabla" style="text-align: center;">Valor %</td>
                        </tr>
                    </thead>
                    <tbody id="tBodyValidadores"></tbody>
                </table>
            </div>
        </div>

        <br />
        
        <div id="tabs">
          <ul>
            <li>
                <a href="#tabs-1">
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblLiquidacionHotel %>"></asp:Label>
                </a>
            </li>
            <li>
                <a href="#tabs-2">
                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblLiquidacionPropietario %>"></asp:Label>
                </a>
            </li>
            <li>
                <a href="#tabs-3">
                    <asp:Label ID="Label9" runat="server" Text="Eliminar Liquidacion"></asp:Label>
                </a>
            </li>
          </ul>
          <div id="tabs-1">
              <div>
                 <div class="tituloPrincipal">
                     <h2>
                         <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, TituloGrillaConcepto %>"></asp:Label>
                     </h2>
                 </div>                    
              </div>
              <div>
                 <input type="button" value="Liquidar" onclick="LiquidarHotel();" class="ui-button ui-widget ui-state-default ui-corner-all" />
              </div>

              <br />

                <div>
                    <table style="width:100%; border-spacing:0px;">
                        <thead class="headtableLiq">
                            <tr>
                                <th style="width:10%">#</th>
                                <th style="width:90%">Concepto</th>
                            </tr>
                        </thead>
                        <tbody id="hotelConceptos">
                        </tbody>
                    </table>
                </div>

              <br />

              <div id="divLiqHotel" style="display:none;">

                <div>
                    <input type="button" value="Guardar Liquidación" onclick="GuardarLiquidarHotel();" class="ui-button ui-widget ui-state-default ui-corner-all" />                    
                </div>

                  <br />

                <table style="width:100%; border-spacing:0px;">
                  <thead class="headtableLiq">
                      <tr>
                          <th style="width:10%">#</th>
                          <th style="width:60%">Concepto</th>
                          <th style="width:30%">Valor</th>
                      </tr>
                  </thead>
                  <tbody id="tblResultDetailLiqHotel">
                  </tbody>
                </table>
              </div>
          </div>


          <div id="tabs-2">            

            <div>
                <input type="button" value="Liquidar todos" onclick="LiquidarTodos();" class="ui-button ui-widget ui-state-default ui-corner-all" />
                <input type="button" value="Liquidar Seleccionados" onclick="LiquidarSeleccionados();" class="ui-button ui-widget ui-state-default ui-corner-all" />
            </div>
            <br />
          
            <div class="divOwnerDetail">
                <table style="width:100%; border-spacing:0px;">
                  <thead class="headtableLiq">
                    <tr>
                      <th style="width:5%">#</th>
                      <th style="width:66%">Nombre</th>
                      <th style="width:12%">N° Suite</th>
                      <th style="width:12%">N° Escritura</th>
                      <th style="width:5%">Sel.</th>
                    </tr>
                  </thead>
                  <tbody id="ownerDetail">
                  </tbody>
                </table>
            </div>

            <br />

            <div id="tblResultLiq" class="divOwnerLiq">

                <div>
                    <input id="GuardarAll" type="button" value="Guardar Liquidación" onclick="GuardarLiquidarProp(false);" class="ui-button ui-widget ui-state-default ui-corner-all" style="display:none" />
                    <input id="GuardarSel" type="button" value="Guardar Liquidación" onclick="GuardarLiquidarProp(true);" class="ui-button ui-widget ui-state-default ui-corner-all" style="display:none" />
                </div>

                  <br />

                <table style="width:100%; border-spacing: 2px">
                  <thead class="headtableLiq" id="tblResultColumnsLiq">
                  </thead>
                  <tbody id="tblResultDetailLiq">
                  </tbody>
                </table>
            </div>

            <div style="clear:both;"></div>
            <br />
          </div>


          <div id="tabs-3">
          
          <table width="100%">
                    <tr>
                        <td colspan="2">
                            <div class="tituloPrincipal">
                                <h2>
                                    <asp:Label ID="Label12" runat="server" Text="Eliminar Liquidaciones"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;" class="textoTabla">
                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label>
                        </td>
                        <td style="width:85%;">
                            <asp:DropDownList ID="ddlHotelEliminarLiquidacion" Width="450px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="textoTabla">
                            <asp:Label ID="Label11" runat="server" Text="fecha"></asp:Label>                            
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
                            <input type="text" id="txtFechaDeleteLiq" disabled="disabled" style="width:40px" />
                        </td>
                    </tr>
                    <tr>
                        <td>          
                            <input type="button" value="Eliminar Liquidación" onclick="DeleteLiq();" class="ui-button ui-widget ui-state-default ui-corner-all" />
                        </td>
                    </tr>
                </table>                
          </div>
          
        </div>
        
    <br />        
        
    <div id="div_DetalleConceptoHotel" class="dragPanel" style="width:915px;">
        <div>
            <div class="tituloGrilla" style="padding-left:8px; padding-right:8px;">
                <h2>
                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, lblDetalleConceptoHotel %>"></asp:Label>
                </h2>
            </div>
            
            <asp:GridView 
                ID="gvwDetalleConceptoHotel" 
                runat="server"
                Width="100%" 
                AutoGenerateColumns="False" 
                CellPadding="2"
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                BorderColor="#7599A9"
                BorderStyle="Solid" 
                BorderWidth="1px">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="<%$ Resources:Resource, lblConcepto %>" >
                        <HeaderStyle Width="55%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Valor" HeaderText="<%$ Resources:Resource, lblValor %>" DataFormatString="{0:N}" >
                        <HeaderStyle Width="15%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="<%$ Resources:Resource, lblFechaElaboracion %>" DataFormatString="{0:dd-MM-yyyy}" >
                        <HeaderStyle Width="15%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Login" HeaderText="<%$ Resources:Resource, lblResponsable %>" >
                        <HeaderStyle Width="15%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                    </asp:BoundField>                    
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
        </div>
    </div>
    
    <div style="display:none">
<%--        <asp:Button ID="btnAceptarBuscador" runat="server" Text="Button" 
            onclick="btnAceptarBuscador_Click" />
        <asp:Button ID="btnCancelarBuscador" runat="server" Text="Button" 
            onclick="btnCancelarBuscador_Click" />--%>
    </div>
    
    </ContentTemplate>
</asp:UpdatePanel>

<div id="modal_Buscador">

    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <table>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, lblBuscarPor %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlFiltro" runat="server">
                    <asp:ListItem Text="<%$ Resources:Resource, lblNombre %>" Value="N" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Resource, lblNumSuit %>" Value="S" ></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Resource, lblEscritura %>" Value="E" ></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td><asp:TextBox ID="txtBusqueda" runat="server"></asp:TextBox></td>
            <td>
                <asp:Button ID="btnBuscar" runat="server" Text="<%$ Resources:Resource, btnBuscar %>" onclick="btnBuscar_Click" />
            </td>
        </tr>
    </table>
            <br />           
    
            <asp:GridView 
            ID="gvwPropietariosBuscar" 
            runat="server"
            DataKeyNames="IdPropietario,IdSuit" 
            Width="100%" 
            AllowPaging="true"
            AutoGenerateColumns="False" 
            CellPadding="2"
            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
            BorderColor="#7599A9" 
            BorderStyle="Solid" 
            BorderWidth="1px" 
            onpageindexchanging="gvwPropietariosBuscar_PageIndexChanging" 
            onrowdatabound="gvwPropietariosBuscar_RowDataBound">
            <Columns>
                <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" >
                    <HeaderStyle Width="70%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                </asp:BoundField>
                <asp:BoundField DataField="NumSuit" HeaderText="<%$ Resources:Resource, lblNumSuit %>" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblSeleccion %>">
                    <ItemTemplate>
                        <asp:ImageButton 
                            ID="imgBtnSeleccion" 
                            runat="server"
                            Height="35px" 
                            Width="35px"                                    
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdSuit") %>'
                            ImageUrl="~/img/117.png" onclick="imgBtnSeleccion_Click" />
                    </ItemTemplate>
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>
        
         </ContentTemplate>
    </asp:UpdatePanel>--%>
    
</div>

<div id="modalBuscadorPropietario">
<%--    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:WebUserBuscadorPropietarioSuite ID="uc_WebUserBuscadorPropietarioSuite" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel> --%>           
</div>