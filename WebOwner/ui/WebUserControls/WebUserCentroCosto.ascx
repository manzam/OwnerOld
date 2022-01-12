<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserCentroCosto.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserCentroCosto" %>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1">
    <ProgressTemplate>
        <div class="procesar redondeo">Procesando..</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
    
        <div class="botonera">
            <asp:Button ID="btnNuevo" runat="server" 
                Text="<%$ Resources:Resource, btnNuevo %>" ValidationGroup="Nuevo" onclick="btnNuevo_Click" />                                       
            <asp:Button ID="btnGuardar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="NuevoActualizar" onclick="btnGuardar_Click" />
            <asp:Button ID="btnActualizar" runat="server" 
                Text="<%$ Resources:Resource, btnGuardar %>" Visible="false" ValidationGroup="Actualizar" onclick="btnActualizar_Click" />
            <asp:Button ID="btnVerTodos" runat="server" 
                Text="<%$ Resources:Resource, btnVerTodos %>" ValidationGroup="VerTodos" onclick="btnVerTodos_Click" />
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
        
        <div id="NuevoCeCo" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="4" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosCeCo %>"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:70%;" class="textoTabla">
                            <asp:Label ID="Label4" runat="server" Text="Nombre"></asp:Label> *                            
                        </td>
                        <td style="width:30%; text-align:center;" class="textoTabla">
                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, lblCodigo %>"></asp:Label> *
                        </td>
                    </tr>
                    <tr>
                        <td style="width:70%;">
                            <asp:TextBox ID="txtNombre" MaxLength="80" runat="server" Width="90%" ValidationGroup="NuevoActualizar">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtNombre" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>                        
                        <td style="width:30%;">
                            <asp:TextBox ID="txtCodigo" MaxLength="10" runat="server" Width="90%" ValidationGroup="NuevoActualizar">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
                                ControlToValidate="txtCodigo" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" > 
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </tbody>
            </table>
            
            <br />
            
        </div>
        
        <div id="GrillaCeCo" runat="server">
            
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, TituloCeCo %>"></asp:Label>                          
                </h2>
            </div>
            
            <asp:GridView 
                ID="gvwCeCo" 
                runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" 
                BorderWidth="1px" 
                BorderColor="#7599A9" 
                CellPadding="2"
                DataKeyNames="IdCentroCosto" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                OnSelectedIndexChanged="gvwCeCo_OnSelectedIndexChanged" 
                Width="100%"
                AllowPaging="True"
                PageSize="10">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" 
                                Text='<%# Bind("Nombre") %>'></asp:LinkButton>                                    
                        </ItemTemplate>
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Codigo" HeaderText="<%$ Resources:Resource, lblCodigo %>" >
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
        </div>        
        
    </ContentTemplate>
</asp:UpdatePanel>

