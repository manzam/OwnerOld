<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserParametros.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserParametros" %>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
        <asp:GridView
            ID="GridView1" 
            runat="server" 
            DataKeyNames="IdParametro"
            AutoGenerateColumns="False"
            Width="100%">
            <Columns>
                <asp:CommandField 
                    ButtonType="Image" 
                    ShowEditButton="true"
                    HeaderText="<%$ Resources:Resource, lblAccion %>" 
                    UpdateImageUrl="~/img/131.png"
                    CancelImageUrl="~/img/72.png" 
                    DeleteImageUrl="~/img/126.png"
                    EditImageUrl="~/img/13.png" >
                    <ControlStyle Height="30px" Width="30px" />
                    <HeaderStyle Width="15%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CommandField>
                <asp:BoundField DataField="Llave" HeaderText="<%$ Resources:Resource, lblLlave %>" >
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblValor %>">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Valor") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Valor") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderStyle Width="15%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:Resource, lblDescripcion %>" >
                    <HeaderStyle Width="55%" />
                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
        </asp:GridView>
    
    </ContentTemplate>
</asp:UpdatePanel>
