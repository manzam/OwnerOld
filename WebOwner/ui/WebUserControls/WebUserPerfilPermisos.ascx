<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserPerfilPermisos.ascx.cs" Inherits="WebOwner.ui.WebUserControls.WebUserPerfilPermisos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        
        <div id="NuevoModulo" runat="server" visible="false">
            <table width="100%" cellpadding="3" cellspacing="0">
                <thead>
                    <tr>
                        <td colspan="2" align="center">
                            <div class="tituloGrilla">
                                <h2>
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, DatosPerfilpermiso %>"></asp:Label>
                                </h2>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:25%;" class="textoTabla">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lblNombrePerfil %>"></asp:Label> *
                        </td>
                        <td style="width:75%;">
                            <asp:TextBox ID="txtNombre" MaxLength="80" runat="server" Width="80%" ValidationGroup="NuevoActualizar" Enabled="false">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                               ControlToValidate="txtNombre" Display="Dynamic" CssClass="error" ValidationGroup="NuevoActualizar" >
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:25%; vertical-align:top;" class="textoTabla">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lblDescripcion %>"></asp:Label>
                        </td>
                        <td style="width:75%;">
                            <asp:TextBox ID="txtDescripcion" MaxLength="80" runat="server" Width="80%" Height="70px" 
                                         ValidationGroup="NuevoActualizar" Enabled="false" TextMode="MultiLine">
                            </asp:TextBox>                           
                        </td>
                    </tr>
                </tbody>
            </table>
            
            <br />
            
            <table width="100%">
                <tr>
                    <td align="center">
                        <div class="tituloGrilla">
                            <h2>
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, TituloGrillaModulos %>"></asp:Label>                            
                            </h2>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwModulo" runat="server" DataKeyNames="IdModulo" 
                                      Width="100%" 
                                      AutoGenerateColumns="False" 
                                      CellPadding="2"
                                      EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" BorderColor="#7599A9" 
                                      BorderStyle="Solid" BorderWidth="1px" >
                              <Columns>
                                <asp:BoundField HeaderText="<%$ Resources:Resource, lblNombre %>" DataField="Nombre" >
                                    <HeaderStyle Width="80%" />
                                    <ItemStyle BorderColor="#7599A9" HorizontalAlign="Left" BorderStyle="Solid" BorderWidth="1px" /> 
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, lblTienePermiso %>">
                                    <HeaderTemplate>
                                        <asp:Label Text="<%$ Resources:Resource, lblTienePermiso %>" runat="server"></asp:Label>
                                        <input type="checkbox" onclick="checkAll(this,'ctl00_Contenidoprincipal_WebUserPerfilPermisos1_gvwModulo')" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:CheckBox ID="chEsPermitido" runat="server" />--%>
                                        <input id="chEsPermitido" type="checkbox" runat="server" onclick="valCheckAll('ctl00_Contenidoprincipal_WebUserPerfilPermisos1_gvwModulo')" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" /> 
                                </asp:TemplateField>                                
                              </Columns>
                              <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="GrillaPerfil" runat="server">
        
            <div class="tituloGrilla" style="text-align:center;">
                <h2>
                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, TituloGrillaPerfil %>"></asp:Label>                          
                </h2>
            </div>
        
            <asp:GridView ID="gvwPerfil" runat="server" 
                AutoGenerateColumns="False" 
                BorderStyle="Solid" BorderWidth="1px" 
                CellPadding="2"
                DataKeyNames="IdPerfil" 
                EmptyDataText="<%$ Resources:Resource, TituloGrillaSinDatos %>" 
                BorderColor="#7599A9"
                OnSelectedIndexChanged="gvwPerfil_OnSelectedIndexChanged" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Resource, lblNombre %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" 
                                Text='<%# Bind("Nombre") %>'></asp:LinkButton>                                    
                        </ItemTemplate>
                        <HeaderStyle Width="40%" />
                        <ItemStyle BorderColor="#7599A9" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:TemplateField>                        
                    <asp:BoundField HeaderText="<%$ Resources:Resource, lblDescripcion %>" DataField="Descripcion">
                        <HeaderStyle Width="60%" />
                        <ItemStyle BorderColor="#7599A9" HorizontalAlign="Left" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" CommandArgument='<%# Bind("IdPerfil") %>' 
                                ImageUrl="~/img/118.png" Width="30px" height="30px" runat="server" 
                                onclick="imgBtnEliminar_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#7599a9" ForeColor="White" BorderColor="#7599a9" />
            </asp:GridView>
        </div>
    

    </ContentTemplate>
</asp:UpdatePanel>