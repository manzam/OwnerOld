<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserConfigExtracto.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserConfigExtracto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>  

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
        
        <script type="text/javascript">
            function uploadComplete(sender) {
                $get("<%=lblMesg.ClientID%>").innerHTML = "Archivo cargado exitosamente.";
            }

            function uploadError(sender) {
                $get("<%=lblMesg.ClientID%>").innerHTML = "El archivo no se pudo cargar con éxito.";
            } 
        </script>


   <table>
        <tr>
            <td style="width:15%;" class="textoTabla">
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label> *
            </td>
            <td style="width:85%;">
                <asp:DropDownList ID="ddlHotel" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlHotel_SelectedIndexChanged" Width="450px">
                </asp:DropDownList>                
            </td>
        </tr>
        <tr>
            <td class="textoTabla">Tipo Extracto</td>
            <td>
                <asp:DropDownList ID="ddlTipoExtracto" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlTipoExtracto_SelectedIndexChanged" Width="450px">
                    <asp:ListItem Text="----------" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Extracto 1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Extracto 2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Extracto 3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Extracto 4" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Extracto 5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Extracto 6" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Extracto 7" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Extracto 8" Value="8"></asp:ListItem>
                    <asp:ListItem Text="Extracto 9" Value="9"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" onclick="btnGuardar_Click" />
            </td>
        </tr>
    </table>
        
    <br />
    
        <asp:Panel ID="pnlFormatoUno" Visible="false" runat="server">
        
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
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Suite Escritura No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Coeficiente Suite:</td>
                    <td>
                        <asp:DropDownList ID="ddlCoeficiente" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">Renta Acumulada año suite:</td>
                    <td>00.00</td>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion año acumulado:</td>
                    <td>00.00 %</td>
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESO TOTAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL COSTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL GASTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td class="textoTabla">UTILIDAD OPERACIONAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlUtilidadOperacional" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESOS DISPONIBLES</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">
                                    INGRESO DEL PROPIETARIO <br />
                                    (<asp:DropDownList ID="ddlPorcentajeIngresoPropietario" Width="90%" runat="server"></asp:DropDownList>%) <br />
                                    A DISTRIBUIR
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Eficiencia Operativa</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlEficienciaOperativaH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlEficienciaOperativaS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la Fuente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Descuento Fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Total Descuentos</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">PARTICIPACION A DISTRIBUIR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoDos" Visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                    INFORME DE DISTRIBUCION DE LA RENTABILIDAD DEL HOTEL ESTELAR XXXXX DE ACUERDO AL CONTRATO DE CUENTAS EN PARTICIPACION ENTRE LOS PROPIETARIOS Y HOTELES S.A.
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;">Bogotá D.C</td>
                    <td style="width:75%;">dia - Mes - Año</td>
                </tr>
                <tr>
                    <td colspan="2">Señores</td>
                </tr>
                <tr>
                    <td colspan="2">xxxxxx xxxxxx xxxxxx</td>
                </tr>
                <tr>
                    <td colspan="2">Dirección xxxx xxxxx xxxxx</td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="2">Estimado Propietario</td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Suite</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width:100%;">
                            <tr>
                                <td class="textoTabla" style="width:25%;">
                                    Coeficiente Suite</td>
                                <td style="width:25%;">
                                    <asp:DropDownList ID="ddlCoeficienteSuite" Width="90%" runat="server"></asp:DropDownList> 
                                </td>
                                <td class="textoTabla" style="width:25%;">
                                    Coeficiente Participación</td>
                                <td style="width:25%;">
                                    <asp:DropDownList ID="ddlCoeficienteParticipacion" Width="90%" runat="server"></asp:DropDownList> 
                                </td>
                            </tr>
                        </table>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="2">
                        A continuación le informamos los resultados financieros de la Operación del Hotel Estelar XXXXXX XXXX                        
                        <br />
                        Se vendieron 
                        <asp:DropDownList ID="ddlNochesVendidas" Width="90%" runat="server"></asp:DropDownList> 
                        noches, el porcentaje de ocupación acumulado quedó en el 
                        <asp:DropDownList ID="ddlOcupacion" Width="90%" runat="server"></asp:DropDownList>%
                        vs el presupuesto acumulado del <asp:DropDownList ID="ddlAcumulado" Width="90%" runat="server"></asp:DropDownList>%.
                        <br />
                        La renta de copropietarios acumulada se ejecuto en un 
                        <asp:DropDownList ID="ddlRenta" Width="90%" runat="server"></asp:DropDownList>%. 
                        <br />
                        A continuación los datos referentes a los valores de renta del mes.
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">COPROPIETARIOS</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Utilidad - Perdida propietarios</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlUtilidadOperativaCop" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlUtilidadOperativaSuite" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:center"><strong>Menos</strong></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Fara Contractual</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlFaraContractualCop" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlFaraContractualSuite" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Fara Adicional</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlFaraAdicionalCop" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlFaraAdicionalSuite" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Retención en la fuente %</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionFuenteCop" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionFuenteSuite" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="border-top-color:Black;">
                                <td class="textoTabla">Valor a Pagar (Consignación)</td>
                                <td style="text-align:right;">
                                    00.00
                                </td>
                                <td style="text-align:right;">
                                    00.00
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Para este mes se aplico una FARA adicional del &nbsp;
                        <asp:DropDownList ID="ddlFaraAdicional" Width="90%" runat="server">
                        </asp:DropDownList>%</td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoTres" Visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Suite Escritura No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Coeficiente Suite:</td>
                    <td>
                        <asp:DropDownList ID="ddlCoeficiente3" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario3" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto3" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">Renta Acumulada año suite:</td>
                    <td>00.00</td>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion año acumulado:</td>
                    <td>00.00 %</td>
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Ventas Totales</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlVentaTotalH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlVentaTotalS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">30% Ventas Alojamiento</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlAlojamientoH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlAlojamientoS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">10% Ventas Servicios Complementarios</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlServiciosCompleH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlServiciosCompleS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Contraprestación Propietarios</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalIngresosH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalIngresosS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Remanente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRemanenteH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRemanenteS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Costos y gastos operación</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlCostosGastosH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlCostosGastosS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Excedente operacional</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlExedenteH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlExedenteS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Nota Crédito Operador *</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlNotaCreditoH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlNotaCreditoS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Administración copropiedad</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlAdminH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlAdminS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH3" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS3" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Retención en la fuenta</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlReteH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlReteS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Cuota Extraordinaria</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlCuotaH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlCuotaS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>                            
                            <tr>
                                <td class="textoTabla">Subtotal descuentos</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlSubTotalDesH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlSubTotalDesS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Total, distribución propietarios</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalH" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalS" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">* Facturas comisión y otros de periodos asumidos por el operador.</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoCuatro" Visible="false" runat="server">
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image4" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Encargo Fiduciario No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>                    
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario4" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto4" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>                    
                    <td class="textoTabla">% Ocupacion año</td>
                    <td>00.00 %</td>
                </tr>
                <tr>                    
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">DERECHO FIDUCIARIO</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESO TOTAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL COSTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL GASTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESOS DISPONIBLES</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td class="textoTabla">UTILIDAD OPERACIONAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlUtilidadOperacional4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                </td>
                            </tr>                            
                            <tr>
                                <td class="textoTabla">
                                    INGRESO DEL PROPIETARIO <br />
                                    (<asp:DropDownList ID="ddlPorcentajeIngresoPropietario4" Width="90%" runat="server"></asp:DropDownList>%) <br />
                                    A DISTRIBUIR
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Descuento Fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la fuente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRete4H" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRete4S" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Total otros descuentos</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlOtrosDes4H" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlOtrosDes4S" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>                            
                            <tr>
                                <td class="textoTabla">PARTICIPACION A DISTRIBUIR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropH4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS4" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoCinco" Visible="false" runat="server">
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image5" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Encargo Fiduciario No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>                    
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario5" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto5" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>                    
                    <td class="textoTabla">% Ocupacion año</td>
                    <td>00.00 %</td>
                </tr>
                <tr>                    
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">DERECHO FIDUCIARIO</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESO TOTAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL COSTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL GASTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESOS DISPONIBLES</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td class="textoTabla">UTILIDAD OPERACIONAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlUtilidadOperacional5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                </td>
                            </tr>                            
                            <tr>
                                <td class="textoTabla">
                                    INGRESO DEL PROPIETARIO <br />
                                    (<asp:DropDownList ID="ddlPorcentajeIngresoPropietario5" Width="90%" runat="server"></asp:DropDownList>%) <br />
                                    A DISTRIBUIR
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Descuento Fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la fuente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRete5H" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRete5S" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">Otros descuentos</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlOtrosConceptosH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlOtrosConceptosS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropH5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS5" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>                            
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoSeis" Visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Suite Escritura No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Coeficiente Suite:</td>
                    <td>
                        <asp:DropDownList ID="ddlCoeficiente6" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario6" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto6" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">Renta Acumulada año suite:</td>
                    <td>00.00</td>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion año acumulado:</td>
                    <td>00.00 %</td>
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESO TOTAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL COSTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL GASTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td class="textoTabla">UTILIDAD OPERACIONAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlUtilidadOperacional6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESOS DISPONIBLES</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">
                                    INGRESO DEL PROPIETARIO A DISTRIBUIR
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Cuota de administración PH</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlCuotaH6" Width="90%" runat="server" >
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlCuotaS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la Fuente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Fondo fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Otros descuentos segun contrato</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">PARTICIPACION A DISTRIBUIR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropH6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS6" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoSiete" Visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image7" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Suite Escritura No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Coeficiente Suite:</td>
                    <td>
                        <asp:DropDownList ID="ddlCoeficiente7" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario7" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto7" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">Renta Acumulada año suite:</td>
                    <td>00.00</td>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion año acumulado:</td>
                    <td>00.00 %</td>
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESO TOTAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL COSTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">APOYO CORPORATIVO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlApoyoH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlApoyoS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL GASTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">UTILIDAD/PÉRDIDA OPERACIÓN</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrsosDisponiblesH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrsosDisponiblesS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">
                                    INGRESO DEL PROPIETARIO A DISTRIBUIR
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">PARTICIPACION GESTOR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlGestorH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlGestorS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la Fuente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Descuento Fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Cuota extraordinaria</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Seguro inmueble y mobiliario</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlSeguroH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlSeguroS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">PARTICIPACION A DISTRIBUIR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropH7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS7" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoOcho" Visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image8" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Suite Escritura No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Coeficiente Suite:</td>
                    <td>
                        <asp:DropDownList ID="ddlCoeficiente8" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario8" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto8" TextMode="MultiLine" Width="100%" 
                            Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">Renta Acumulada año suite:</td>
                    <td>00.00</td>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion año acumulado:</td>
                    <td>00.00 %</td>
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESO TOTAL</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresoTotalS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL COSTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalCostosS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL GASTOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlTotalGastosS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">INGRESOS DISPONIBLES</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrsosDisponiblesH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrsosDisponiblesS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">
                                    INGRESO DEL PROPIETARIO A DISTRIBUIR
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngrePropiS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la Fuente</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Fondo Fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDescFaraS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Otros descuentos según contrato</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlDesSegAnoS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">PARTICIPACION A DISTRIBUIR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlPartiDistS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropH8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS8" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <asp:Panel ID="pnlFormatoNueve" Visible="false" runat="server">
        
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Image ID="Image9" runat="server" ImageUrl="~/img/63.png" Width="100px" Height="100px" />
                        <br />
                        Nombre del Hotel
                        <br />
                        Nit del Hotel
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;" class="textoTabla">Fecha de Expedicion:</td>
                    <td style="width:25%;">dia - Mes - Año</td>
                    <td style="width:25%;" class="textoTabla">Mes de Liquidacion:</td>
                    <td style="width:25%;">Mes - Año</td>
                </tr>
                <tr>
                    <td class="textoTabla">Propietario:</td>
                    <td>xxxxx xxxxxxx xxxxxxx xxxxxxx</td>
                    <td class="textoTabla">Nit:</td>
                    <td>xx.xxx.xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Direccion:</td>
                    <td>xxxxx xxxxx</td>
                    <td class="textoTabla">Suite Escritura No.:</td>
                    <td>xxx</td>
                </tr>
                <tr>
                    <td class="textoTabla">Coeficiente Suite:</td>
                    <td>
                        <asp:DropDownList ID="ddlCoeficiente9" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="textoTabla">Participacion Propietario:</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipacionPropietario9" Width="90%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" style="text-align:center" colspan="4">EXTRACO DE CUENTA</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescExtracto9" TextMode="MultiLine" Width="100%" Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="textoTabla" colspan="4">Informacion Estadistica</td>
                </tr>
                <tr>
                    <td class="textoTabla">Renta Acumulada año suite:</td>
                    <td>00.00</td>
                    <td class="textoTabla">% Ocupacion mes:</td>
                    <td>00.00 %</td>
                </tr>
                <tr>
                    <td class="textoTabla">% Ocupacion año acumulado:</td>
                    <td>00.00 %</td>
                    <td class="textoTabla">Tarifa Promedio:</td>
                    <td>00.00</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td class="textoTabla" style="width:35%;">CONCEPTO</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">HOTEL</td>
                                <td class="textoTabla" style="text-align:center;width:32.5%;">SUITE</td>
                            </tr>
                            <tr>
                                <td>INGRESOS</td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Alojamiento</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlAlojamientoH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlAlojamientoS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Servicios Complementarios</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlServiciosH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlServiciosS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">TOTAL INGRESOS</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlIngresosDispoS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td class="textoTabla">30% Ventas de Alojamiento</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlVentasAlojamientoH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlVentasAlojamientoS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">10% Ventas de Servicios Complementarios</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlVentasServicioH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlVentasServicioS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">
                                    PARTICIPACIÓN PROPIETARIOS
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlParticipacionH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlParticipacionS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">MENOS</td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Descuento fara</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlFaraH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlFaraS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Honorarios Administración</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlHonorariosH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlHonorariosS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">&nbsp;&nbsp;&nbsp;&nbsp;Retención en la fuente</td>
                                <td style="text-align:center;">
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRetencionS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">PARTICIPACION A DISTRIBUIR</td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlParticipacionDistriH9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlParticipacionDistriS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="textoTabla">RENTA PROPIETARIO</td>
                                <td style="text-align:center;">
                                </td>
                                <td style="text-align:center;">
                                    <asp:DropDownList ID="ddlRentaPropS9" Width="90%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
        
        <table style="width:100%">                    
            <tr>
                <td>
                    <table style="width:100%;">
                        <tr>
                            <td valign="top" class="textoTabla">
                                <asp:Label ID="Label20" runat="server" Text="Firma imagen"></asp:Label>
                            </td>
                            <td valign="top" >
                                <asp:AsyncFileUpload 
                                    ID="AsyncFileUpload1"                                 
                                    runat="server"
                                    PersistFile="true"
                                    OnClientUploadError="uploadError"
                                    OnClientUploadComplete="uploadComplete"
                                    onuploadedcomplete="AsyncFileUpload1_UploadedComplete" />                               
                                
                               <asp:Label ID="lblMesg" runat="server" Text=""></asp:Label>
                            </td>
                            <td valign="top" >
                                <asp:Image ID="imgLogo" runat="server" Width="150px" Height="80px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtPieExtracto" runat="server" width="50%" height="80px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
    
    </ContentTemplate>
</asp:UpdatePanel>