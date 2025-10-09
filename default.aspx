<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Instituto46.Default" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Instituto 46</title>
    <style>
        /* CSS Minimalista para la página principal */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Arial', sans-serif;
            background-color: #f8f9fa;
            color: #333;
            line-height: 1.6;
            height: 100vh;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            background-image: linear-gradient(to bottom, #f8f9fa, #e9ecef);
        }
        
        .container {
            max-width: 800px;
            min-height: 400px;
            padding: 2rem;
            text-align: center;
            background-image: url('image/Malvinas.jpg');
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            position: relative;
            overflow: hidden;
        }
        
        .container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(255, 255, 255, 0.7);
            z-index: 0;
        }
        
        .container > * {
            position: relative;
            z-index: 1;
        }
        
        h1 {
            font-size: 2.5rem;
            margin-bottom: 1.5rem;
            color: #2c3e50;
            position: relative;
        }
        
        h1::after {
            content: "";
            display: block;
            width: 100px;
            height: 4px;
            background: linear-gradient(90deg, #75AADB, #fff, #75AADB);
            margin: 0.5rem auto;
        }
        
        p {
            font-size: 1.1rem;
            margin-bottom: 2rem;
            color: #495057;
        }
        
        .btn {
            display: inline-block;
            background-color: #75AADB;
            color: white;
            padding: 0.8rem 1.5rem;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            font-weight: bold;
            transition: all 0.3s ease;
            cursor: pointer;
            margin-top: 1rem;
        }
        
        .btn:hover {
            background-color: #5d8fc2;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
        
        footer {
            margin-top: 3rem;
            font-size: 0.9rem;
            color: #6c757d;
        }
        
        .malvinas-text {
            font-weight: bold;
            color: #75AADB;
        }
        
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .animated {
            animation: fadeIn 1s ease-out forwards;
        }
        
        .delay-1 { animation-delay: 0.2s; }
        .delay-2 { animation-delay: 0.4s; }
        .delay-3 { animation-delay: 0.6s; }
        
        /* Estilos para el modal de login (simplificado) */
        .overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.6);
            backdrop-filter: blur(4px);
            z-index: 1000;
            justify-content: center;
            align-items: center;
        }
        
        .auth-container {
            background: white;
            border-radius: 16px;
            width: 350px;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
            position: relative;
            padding: 2rem;
            box-sizing: border-box;
            min-height: 400px;
            display: flex;
            flex-direction: column;
        }
        
        .user-icon {
            width: 80px;
            height: 80px;
            background-color: #eff6ff;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            border: 2px solid #2563eb;
            margin: 0 auto 1.5rem;
        }
        
        .user-icon svg {
            width: 36px;
            height: 36px;
            color: #2563eb;
        }
        
        .auth-forms {
            position: relative;
            flex-grow: 1;
        }
        
        .login-form {
            width: 100%;
            height: 100%;
            display: flex;
            flex-direction: column;
        }
        
        .auth-container h1 {
            font-size: 1.5rem;
            color: #1e293b;
            margin-bottom: 1.5rem;
            text-align: center;
            font-weight: 600;
        }
        
        .auth-container input {
            width: 100%;
            padding: 12px;
            margin-bottom: 1rem;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            font-size: 0.95rem;
            transition: border 0.3s;
        }
        
        .auth-container input:focus {
            outline: none;
            border-color: #2563eb;
            box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
        }
        
        .auth-container button[type="submit"] {
            width: 100%;
            padding: 12px;
            background-color: #2563eb;
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 1rem;
            font-weight: 500;
            cursor: pointer;
            margin-top: 1rem;
            transition: background 0.3s;
        }
        
        .auth-container button[type="submit"]:hover {
            background-color: #1d4ed8;
        }
        
        .footer {
            margin-top: auto;
            text-align: center;
            font-size: 0.85rem;
            color: #64748b;
            padding-top: 1.5rem;
        }
        
        .close-btn {
            position: absolute;
            top: 1rem;
            right: 1rem;
            background: none;
            border: none;
            font-size: 1.5rem;
            cursor: pointer;
            color: #94a3b8;
        }
        
        .recover-link {
            color: #2563eb;
            text-decoration: none;
            display: block;
            margin-top: 1rem;
            text-align: center;
            font-size: 0.85rem;
        }
        
        .recover-link:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1 class="animated">Bienvenido</h1>
            <p class="animated delay-1"> <span class="malvinas-text">Instituto de Formación Docente y Técnica</span> N° 46</p>
            
            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" 
                CssClass="btn animated delay-2" OnClientClick="openAuthModal(); return false;" 
                onclick="btnIngresar_Click" />
            
            <footer class="animated delay-3">
                "Esta página está diseñada por alumnos del 46"
            </footer>
        </div>

        <!-- Modal de Login Simplificado -->
        <div class="overlay" id="authOverlay" runat="server">
            <div class="auth-container">
                <button type="button" class="close-btn" onclick="closeAuthModal()">&times;</button>
                
                <div class="user-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
                        <path d="M7.5 6a4.5 4.5 0 119 0 4.5 4.5 0 01-9 0zM3.751 20.105a8.25 8.25 0 0116.498 0 .75.75 0 01-.437.695A18.683 18.683 0 0112 22.5c-2.786 0-5.433-.608-7.812-1.7a.75.75 0 01-.437-.695z" />
                    </svg>
                </div>
                
                <div class="auth-forms">
                    <div class="login-form" id="loginForm">
                        <h1>Inicia sesión</h1>
                        <div>
                            <!-- Cambio principal: TextMode="SingleLine" en lugar de "Email" -->
                            <asp:TextBox ID="txtUsuario" runat="server" TextMode="SingleLine" placeholder="Nombre de usuario" required="required" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Contraseña" required="required" CssClass="form-control"></asp:TextBox>
                            <asp:Button ID="btnLogin" runat="server" Text="Continuar" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                            <%--<asp:HyperLink ID="lnkRecover" runat="server" NavigateUrl="~/RecoverPassword.aspx" CssClass="recover-link">¿Olvidaste tu contraseña?</asp:HyperLink>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        // Funciones para manejar el modal de autenticación
        function openAuthModal() {
            document.getElementById("authOverlay").style.display = "flex";
            document.body.style.overflow = "hidden";

            // Enfocar el campo de usuario al abrir el modal
            setTimeout(function () {
                document.getElementById("txtUsuario").focus();
            }, 100);
        }

        function closeAuthModal() {
            document.getElementById("authOverlay").style.display = "none";
            document.body.style.overflow = "auto";
        }

        // Cerrar modal al hacer clic fuera del contenedor
        document.getElementById("authOverlay").addEventListener("click", function (e) {
            if (e.target === this) closeAuthModal();
        });

        // Función para manejar la tecla Enter en el campo de contraseña
        function handleEnterKey(event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                document.getElementById("btnLogin").click();
            }
        }

        // Agregar el event listener al campo de contraseña cuando el modal esté visible
        document.getElementById("authOverlay").addEventListener('shown', function () {
            var passwordField = document.getElementById("txtPassword");
            if (passwordField) {
                passwordField.addEventListener("keydown", handleEnterKey);
            }
        });

        // Inicializar: agregar event listener al campo de contraseña
        document.addEventListener("DOMContentLoaded", function () {
            var passwordField = document.getElementById("txtPassword");
            if (passwordField) {
                passwordField.addEventListener("keydown", handleEnterKey);
            }

            // También agregar para el campo de usuario por si acaso
            var userField = document.getElementById("txtUsuario");
            if (userField) {
                userField.addEventListener("keydown", function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        document.getElementById("txtPassword").focus();
                    }
                });
            }
        });
    </script>
</body>
</html>