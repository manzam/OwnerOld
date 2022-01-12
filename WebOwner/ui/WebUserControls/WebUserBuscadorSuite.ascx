<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserBuscadorSuite.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserBuscadorSuite" %>

<table width="100%" cellpadding="3" cellspacing="0">
            <tr>
                <td class="textoTabla" style="width:110px"><asp:Label ID="Label14" runat="server" Text="Num. Suite *"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtBusqueda" runat="server" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtBusqueda" Display="Dynamic" CssClass="error" ValidationGroup="Buscador" > 
                    </asp:RequiredFieldValidator>
                    &nbsp;
                    <asp:Button ID="btnBuscar" runat="server" Text="<%$ Resources:Resource, btnBuscar %>" onclick="btnBuscar_Click" ValidationGroup="Buscador" />
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
            ID="gvwSuitePorHotel" 
            runat="server"
            DataKeyNames="IdSuit" 
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
                <asp:TemplateField HeaderText="N° Suite">
                    <ItemTemplate>      
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Text='<%# Bind("NumSuit") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                </asp:TemplateField>
                <asp:BoundField DataField="NumEscritura" HeaderText="N° Escritura" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RegistroNotaria" HeaderText="Registro Notaria" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descipción" >
                    <HeaderStyle Width="55%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <SelectedRowStyle BackColor="#B0C6CE" Font-Bold="True" />
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>