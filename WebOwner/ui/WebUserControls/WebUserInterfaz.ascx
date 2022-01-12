<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserInterfaz.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserInterfaz" %>

<script type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {

            var d = new Date();
            var y = d.getFullYear();

            $j("#ctl00_Contenidoprincipal_WebUserInterfaz1_txtAno").spinner({
                min: 2000,
                max: y
            });
        }
    });
        
    </script>  
  
<%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>    

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <div class="loader">
                <div style="width:100%; height:100%; position:absolute; left:50%; top:50%;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/ajax-loader.gif" />
                    <p>
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, lblCargando %>"></asp:Label>                                
                    </p>                                
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    
        <div class="botonera">
        <asp:Button ID="btnAceptar" runat="server" 
            Text="<%$ Resources:Resource, btnAceptar %>" ValidationGroup="Nuevo" 
            onclick="btnAceptar_Click"/>
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
        
        <table width="100%" cellpadding="3" cellspacing="0">
            <tr>
                <td style="width:10%;" class="textoTabla">
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label> *
                </td>
                <td>
                    <asp:DropDownList ID="ddlHotel" runat="server" Width="450px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label> *
                    </td>
                <td>
                    
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
                    <asp:DropDownList ID="ddlAno" Width="60px" runat="server">
                    </asp:DropDownList>
                    
                    &nbsp;&nbsp;&nbsp;
                    
                    <asp:LinkButton 
                        ID="lbDescargarArchivo" 
                        runat="server" 
                        Visible="false" 
                        Text="<%$ Resources:Resource, lblDescargarArchivo %>" 
                        PostBackUrl="~/ui/Paginas/ReporteInterfaz.aspx">
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
        
        <br />
        <%--
        <asp:GridView 
            ID="gvwPropietarios" 
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
            onpageindexchanging="gvwPropietarios_PageIndexChanging" 
            onrowdatabound="gvwPropietarios_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="<%$ Resources:Resource, lblNombre %>" >
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
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdSuit") + "," + DataBinder.Eval(Container.DataItem, "IdPropietario") %>'
                            ImageUrl="~/img/117.png" onclick="imgBtnSeleccion_Click" />
                    </ItemTemplate>
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>
--%>
    
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>