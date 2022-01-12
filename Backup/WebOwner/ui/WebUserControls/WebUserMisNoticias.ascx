<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserMisNoticias.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserMisNoticias" %>

<asp:Repeater ID="RpMisNoticias" runat="server">
    <ItemTemplate>
        <table width="100%">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Titulo") %>'></asp:Label>
                    </h2>
                </td>
            </tr>
            <tr>
                <td style="text-align:center;">
                    <asp:Image ID="Image1" ImageUrl='<%# Eval("Imagen") %>' runat="server" width="700px" height="500px" />
                </td>
            </tr>
        </table>
    </ItemTemplate>
    
    <SeparatorTemplate>
        <br />
        <div class="separadorNoticia"></div>
    </SeparatorTemplate>
</asp:Repeater>
