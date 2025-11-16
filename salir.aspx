<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="salir.aspx.cs" Inherits="proyectoPracticaProfecional.salir" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!-- Modal de Confirmación -->
<div class="modal fade show d-block" id="confirmLogoutModal" tabindex="-1" aria-labelledby="confirmLogoutModalLabel" aria-modal="true" role="dialog" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header border-0">
                <div class="logout-icon">
                    <i class="fas fa-sign-out-alt"></i>
                </div>
                <h5 class="modal-title ms-3" id="confirmLogoutModalLabel">Confirmar Salida</h5>
            </div>
            <div class="modal-body text-center py-4">
                <i class="fas fa-question-circle text-warning fa-3x mb-3"></i>
                <h5 class="mb-3">¿Estás seguro de que quieres salir?</h5>
                <p class="text-muted">Se cerrará tu sesión actual y serás redirigido al login.</p>
            </div>
            <div class="modal-footer border-0 justify-content-center">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                    CssClass="btn btn-secondary px-4" OnClientClick="cancelLogout(); return false;" />
                <asp:Button ID="btnConfirmarSalir" runat="server" Text="Sí, Salir" 
                    CssClass="btn btn-danger px-4" OnClick="btnConfirmarSalir_Click" />
            </div>
        </div>
    </div>
</div>

<!-- Pantalla de Cerrando Sesión (oculta inicialmente) -->
<div id="logoutProgress" class="container-fluid min-vh-100 d-flex align-items-center justify-content-center" style="display: none;">
    <div class="row w-100 justify-content-center">
        <div class="col-md-6 col-lg-4">
            <div class="card border-0 shadow-sm rounded-3">
                <div class="card-body p-5 text-center">
                    <!-- Icono de loading -->
                    <div class="logout-icon mb-4">
                        <i class="fas fa-sign-out-alt"></i>
                    </div>
                    
                    <h4 class="text-dark mb-3">Cerrando Sesión</h4>
                    
                    <!-- Spinner -->
                    <div class="spinner-border text-primary mb-3" role="status">
                        <span class="visually-hidden">Cerrando sesión...</span>
                    </div>
                    
                    <p class="text-muted">Redirigiendo al login...</p>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    // Función para cancelar y volver atrás
    function cancelLogout() {
        window.history.back(); // Vuelve a la página anterior
    }

    // Función para mostrar la pantalla de progreso
    function showLogoutProgress() {
        document.getElementById('confirmLogoutModal').style.display = 'none';
        document.getElementById('logoutProgress').style.display = 'flex';

        // Redirigir automáticamente después de 2 segundos
        setTimeout(function () {
            window.location.href = 'Login.aspx?logout=success';
        }, 2000);
    }

    // Mostrar el modal automáticamente al cargar la página
    document.addEventListener('DOMContentLoaded', function () {
        // El modal ya está visible con las clases de Bootstrap
    });

    // Prevenir que el usuario cierre el modal con ESC o click fuera
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            e.preventDefault();
            cancelLogout();
        }
    });

    // Click fuera del modal para cancelar
    document.getElementById('confirmLogoutModal').addEventListener('click', function (e) {
        if (e.target === this) {
            cancelLogout();
        }
    });
</script>

<style>
    .logout-icon {
        width: 50px;
        height: 50px;
        background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%);
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-size: 1.5rem;
    }
    
    .spinner-border {
        width: 3rem;
        height: 3rem;
    }

    /* Asegurar que el modal ocupe toda la pantalla */
    #confirmLogoutModal {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        z-index: 9999;
    }

    .modal-content {
        margin: 100px auto;
        max-width: 400px;
    }

    /* Estilos para los botones */
    .btn-secondary {
        background: linear-gradient(135deg, #6c757d 0%, #5a6268 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
    }

    .btn-danger {
        background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
    }

    .btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 15px rgba(0,0,0,0.2);
    }
</style>

</asp:Content>