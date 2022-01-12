<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserCierre.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserCierre" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script language="javascript" type="text/javascript">

    $j(document).ready(function() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);
        CargasIniciales(null, null);

        function CargasIniciales(sender, args) {
            var d = new Date();
            var y = d.getFullYear();

            $j("#ctl00_Contenidoprincipal_WebUserCierre1_txtAno").spinner({
                min: 2000,
                max:y
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
                <td style="width:10%;" class="textoTabla">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblFecha %>"></asp:Label> *
                </td>
                <td style="width:90%;">
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
                    <asp:TextBox ID="txtAno" ValidationGroup="Cerrar" Enabled="false" Width="40px" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidator2" 
                        ControlToValidate="txtAno"
                        runat="server" 
                        ErrorMessage="*">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="textoTabla">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblHotel %>"></asp:Label> *
                </td>
                <td>
                    <asp:DropDownList ID="ddlHotel" ValidationGroup="Cerrar" runat="server" Width="450px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidator1" 
                        InitialValue="0"
                        ControlToValidate="ddlHotel"
                        runat="server" 
                        ErrorMessage="*">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" class="textoTabla">
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, lblObservacion %>"></asp:Label>
                </td>
                <td>                
                    <asp:TextBox ID="txtObservacion" TextMode="MultiLine" Width="70%" Height="80px" runat="server"></asp:TextBox>
                    <br />                
                    <asp:Button ID="btnCerrar" runat="server" 
                        Text="<%$ Resources:Resource, lblCierreLiquidacion %>" 
                        onclick="btnCerrar_Click" />  
                    &nbsp;
                    <asp:Button ID="btnAbrir" runat="server" Visible="false" 
                        Text="<%$ Resources:Resource, lblAbrirLiquidacion %>" 
                        onclick="btnAbrir_Click" />
                    &nbsp;
                    <asp:Button ID="btnCancelar" runat="server" Visible="false"
                        Text="<%$ Resources:Resource, btnCancelar %>" 
                        onclick="btnCancelar_Click" />                    
                </td>
            </tr>
        </table>
        
        <br />
        
        <asp:Panel ID="pnlHistorial" runat="server">
        
            <asp:GridView 
                ID="gvwCierres" 
                Width="100%"
                AllowPaging="True"
                DataSourceID="odsCierre"
                DataKeyNames="IdCierre,IdHotel,Activo"
                runat="server" 
                SelectedRowStyle-BackColor="#fffaae"
                SelectedRowStyle-ForeColor="#585858"
                CellPadding="2"
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                BorderColor="#7599A9" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                AutoGenerateColumns="False" 
                onselectedindexchanged="gvwHistorial_SelectedIndexChanged">
                <Columns>                                        
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblPeriodo %>" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbFecha" runat="server" CausesValidation="False" 
                                CommandName="Select" Text='<%# Bind("Periodo") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="20%" HorizontalAlign="Center" />  
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="<%$ Resources:Resource, lblHotel %>" DataField="NombreHotel" >
                     <HeaderStyle Width="25%" />
                    </asp:BoundField>                    
                    <asp:BoundField HeaderText="<%$ Resources:Resource, lblObservacion %>" DataField="Descripcion" >
                     <HeaderStyle Width="45%" />
                    </asp:BoundField>                    
                    <asp:ImageField HeaderText="<%$ Resources:Resource, lblEstado %>" DataImageUrlField="RutaLogo">
                        <ItemStyle Width="20px" Height="20px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderStyle Width="5%" />
                    </asp:ImageField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
            
            <asp:ObjectDataSource 
                ID="odsCierre" 
                runat="server"
                EnablePaging="true"
                StartRowIndexParameterName="inicio" 
                MaximumRowsParameterName="fin"
                SelectMethod="ListarTodos"
                SelectCountMethod="CountListarTodos" 
                TypeName="BO.CierreBo">
                <SelectParameters>
                    <asp:Parameter Name="inicio" Type="Int32" />
                    <asp:Parameter Name="fin" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            
            <br />
            
            <h2>Historial Cierre</h2>
            
            <asp:GridView 
                ID="gvwHistorial" 
                Width="100%"
                runat="server" 
                CellPadding="2"
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                BorderColor="#7599A9" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                AutoGenerateColumns="False" 
                onselectedindexchanged="gvwHistorial_SelectedIndexChanged">
                <Columns>                     
                    <asp:BoundField HeaderText="<%$ Resources:Resource, lblObservacion %>" DataField="Descripcion" >
                        <HeaderStyle Width="65%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:Resource, lblFecha %>" DataField="Fecha" >
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:Resource, lblUsuario %>" DataField="Nombre" >
                        <HeaderStyle Width="15%" />                        
                    </asp:BoundField>
                    
                    <asp:ImageField HeaderText="<%$ Resources:Resource, lblEstado %>" DataImageUrlField="RutaLogo">
                        <ItemStyle Width="20px" Height="20px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderStyle Width="5%" />
                    </asp:ImageField>
                    
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                <SelectedRowStyle BackColor="#fffaae" ForeColor="#585858" BorderColor="#fffaae" />
            </asp:GridView>
            
        </asp:Panel>
    
    </ContentTemplate>
</asp:UpdatePanel>