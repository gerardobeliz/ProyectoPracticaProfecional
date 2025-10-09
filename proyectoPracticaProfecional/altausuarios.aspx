<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Instituto46.Default" %>

<!DOCTYPE html>
<html lang="es">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro de Usuarios</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 20px;
            background-color: #f4f4f4;
        }
        .container {
            max-width: 500px;
            margin: 0 auto;
            background: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        h1 {
            text-align: center;
            color: #333;
        }
        .form-group {
            margin-bottom: 15px;
        }
        label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }
        input[type="text"],
        input[type="email"],
        input[type="password"],
        select {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
        }
        button {
            background: #5cb85c;
            color: white;
            border: none;
            padding: 10px 15px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            width: 100%;
        }
        button:hover {
            background: #4cae4c;
        }
        .error {
            color: red;
            font-size: 14px;
            margin-top: 5px;
        }
    </style>
</head>
<body>
    <form id="registroForm" runat="server">
        <div class="container">
            <h1>Registro de Usuario</h1>
            
            <div class="form-group">
                <label for="nombre">Nombre completo:</label>
                <asp:TextBox ID="nombre" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                    ControlToValidate="nombre" ErrorMessage="El nombre es requerido" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <label for="email">Correo electrónico:</label>
                <asp:TextBox ID="email" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="email" ErrorMessage="El email es requerido" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="email" ErrorMessage="Ingrese un email válido"
                    ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$" CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>
            
            <div class="form-group">
                <label for="usuario">Nombre de usuario:</label>
                <asp:TextBox ID="usuario" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                    ControlToValidate="usuario" ErrorMessage="El usuario es requerido" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvUsuario" runat="server" 
                    ControlToValidate="usuario" ErrorMessage="El usuario debe tener al menos 4 caracteres" 
                    CssClass="error" Display="Dynamic" ClientValidationFunction="validarLongitudUsuario"
                    OnServerValidate="ValidarLongitudUsuario"></asp:CustomValidator>
            </div>
            
            <div class="form-group">
                <label for="password">Contraseña:</label>
                <asp:TextBox ID="password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="password" ErrorMessage="La contraseña es requerida" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvPassword" runat="server" 
                    ControlToValidate="password" ErrorMessage="La contraseña debe tener al menos 6 caracteres" 
                    CssClass="error" Display="Dynamic" ClientValidationFunction="validarLongitudPassword"
                    OnServerValidate="ValidarLongitudPassword"></asp:CustomValidator>
            </div>
            
            <div class="form-group">
                <label for="confirmarPassword">Confirmar contraseña:</label>
                <asp:TextBox ID="confirmarPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmarPassword" runat="server" 
                    ControlToValidate="confirmarPassword" ErrorMessage="Confirme su contraseña" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvConfirmarPassword" runat="server"
                    ControlToValidate="confirmarPassword" ControlToCompare="password"
                    ErrorMessage="Las contraseñas no coinciden" CssClass="error" Display="Dynamic"></asp:CompareValidator>
            </div>
            
            <div class="form-group">
                <label for="tipoUsuario">Tipo de usuario:</label>
                <asp:DropDownList ID="tipoUsuario" runat="server" CssClass="form-control">
                    <asp:ListItem Value="" Text="Seleccione una opción"></asp:ListItem>
                    <asp:ListItem Value="admin" Text="Administrador"></asp:ListItem>
                    <asp:ListItem Value="usuario" Text="Usuario normal"></asp:ListItem>
                    <asp:ListItem Value="invitado" Text="Invitado"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTipoUsuario" runat="server" 
                    ControlToValidate="tipoUsuario" ErrorMessage="Seleccione un tipo de usuario" 
                    CssClass="error" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" 
                    CssClass="btn btn-primary" OnClick="btnRegistrar_Click" />
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function validarLongitudUsuario(source, args) {
            args.IsValid = args.Value.length >= 4;
        }

        function validarLongitudPassword(source, args) {
            args.IsValid = args.Value.length >= 6;
        }
    </script>
</body>
</html>