<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="altaUsuario.aspx.cs" Inherits="proyectoPracticaProfecional.altaUsuario" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
    <div class="p-4 p-md-5 pt-5">
    <h1>Registro de Usuario</h1>
        <!-- Nombre Completo -->
        <div class="form-group">
            <label for="txtNombre">Nombre completo:</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                ControlToValidate="txtNombre" ErrorMessage="El nombre es requerido" 
                CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        
        <!-- Correo Electrónico -->
        <div class="form-group">
            <label for="txtEmail">Correo electrónico:</label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="El email es requerido" 
                CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail" ErrorMessage="Ingrese un email válido"
                ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$" CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
        </div>
        
        <!-- Nombre de Usuario -->
        <div class="form-group">
            <label for="txtUsuario">Nombre de usuario:</label>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                ControlToValidate="txtUsuario" ErrorMessage="El usuario es requerido" 
                CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvUsuario" runat="server" 
                ControlToValidate="txtUsuario" ErrorMessage="El usuario debe tener al menos 4 caracteres"
                OnServerValidate="ValidarLongitudUsuario" CssClass="text-danger" Display="Dynamic"></asp:CustomValidator>
        </div>
        
        <!-- Contraseña -->
        <div class="form-group">
            <label for="txtPassword">Contraseña:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="La contraseña es requerida" 
                CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvPassword" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="La contraseña debe tener al menos 6 caracteres"
                OnServerValidate="ValidarLongitudPassword" CssClass="text-danger" Display="Dynamic"></asp:CustomValidator>
        </div>
        
        <!-- Confirmar Contraseña -->
        <div class="form-group">
            <label for="txtConfirmarPassword">Confirmar contraseña:</label>
            <asp:TextBox ID="txtConfirmarPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvConfirmarPassword" runat="server" 
                ControlToValidate="txtConfirmarPassword" ErrorMessage="Confirme su contraseña" 
                CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvConfirmarPassword" runat="server"
                ControlToValidate="txtConfirmarPassword" ControlToCompare="txtPassword"
                ErrorMessage="Las contraseñas no coinciden" CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
        </div>
        
        <!-- Tipo de Usuario -->
        <div class="form-group">
            <label for="ddlTipoUsuario">Tipo de usuario:</label>
            <asp:DropDownList ID="ddlTipoUsuario" runat="server" CssClass="form-control">
                <asp:ListItem Value="" Text="Seleccione una opción"></asp:ListItem>
                <asp:ListItem Value="admin" Text="Administrador"></asp:ListItem>
                <asp:ListItem Value="usuario" Text="Usuario normal"></asp:ListItem>
                <asp:ListItem Value="invitado" Text="Invitado"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvTipoUsuario" runat="server" 
                ControlToValidate="ddlTipoUsuario" ErrorMessage="Seleccione un tipo de usuario" 
                CssClass="text-danger" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
        </div>
        
        <!-- Botón de Registro -->
        <!-- Sección del botón alineado a la derecha -->
<div class="form-group text-right"> <!-- text-right alinea el contenido a la derecha -->
    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" 
        CssClass="btn btn-primary" OnClick="btnRegistrar_Click" />
</div>
</div>
</asp:Content>