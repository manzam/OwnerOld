<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserBuscadorPropietarioSuite.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserBuscadorPropietarioSuite" %>

        <table width="100%" cellpadding="3" cellspacing="0">
            <tr>
                <td colspan="2"><asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, lblBuscarPor %>"></asp:Label></td>
            </tr>
            <tr>
                <td style="width:25%; text-align:left; vertical-align:top;">
                    <asp:DropDownList ID="ddlFiltro" runat="server" Width="90%">
                        <asp:ListItem Text="<%$ Resources:Resource, lblNombre %>" Value="N" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Apellido" Value="A" ></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, lblNumIdentificacion %>" Value="I" ></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="vertical-align:top;">
                    <asp:TextBox ID="txtBusqueda" runat="server" Width="250px"></asp:TextBox>
                    &nbsp;
                    <asp:Button ID="btnBuscar" runat="server" Text="<%$ Resources:Resource, btnBuscar %>" onclick="btnBuscar_Click" />
                </td>
                    
            </tr>
        </table>

        <br />      
        
        <div style="display:none;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                onclick="btnAceptar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                onclick="btnCancelar_Click" />
        </div>
        
        <asp:GridView 
            ID="gvwPropietariosBuscar" 
            runat="server"
            DataKeyNames="IdPropietario,IdSuit,NumSuit,IdHotel" 
            Width="100%" 
            AllowPaging="True"
            AutoGenerateColumns="False" 
            CellPadding="2"
            EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
            BorderColor="#7599A9" 
            BorderStyle="Solid" 
            BorderWidth="1px"             
            onselectedindexchanged="gvwPropietariosBuscar_SelectedIndexChanged" 
            onpageindexchanging="gvwPropietariosBuscar_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>      
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Text='<%# Bind("NombreCompleto") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="50%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                </asp:TemplateField>
                <asp:BoundField DataField="NumIdentificacion" HeaderText="<%$ Resources:Resource, lblNumIdentificacion %>" >
                    <HeaderStyle Width="20%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NombreHotel" HeaderText="<%$ Resources:Resource, lblHotel %>" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NumSuit" HeaderText="<%$ Resources:Resource, lblNumSuit %>" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NumEscritura" HeaderText="<%$ Resources:Resource, lblEscritura %>" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <SelectedRowStyle BackColor="#B0C6CE" Font-Bold="True" />
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>