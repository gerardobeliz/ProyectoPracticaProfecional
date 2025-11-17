<%@ Page Title="Cerrar Sesión" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="salir.aspx.cs" Inherits="proyectoPracticaProfecional.salir" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!-- Modal de Confirmación -->
<div class="modal-backdrop fade show" id="modalBackdrop"></div>
<div class="modal fade show d-block" id="confirmLogoutModal" tabindex="-1" aria-labelledby="confirmLogoutModalLabel" aria-modal="true" role="dialog">
    <div class="modal-dialog modal-dialog-centered modal-sm">
        <div class="modal-content shadow-lg rounded-3">
            <!-- Header con icono animado -->
            <div class="modal-header border-0 pb-0">
                <div class="logout-icon-container mx-auto">
                    <div class="logout-icon animate-pulse">
                        <i class="fas fa-door-open"></i>
                    </div>
                    <div class="logout-pulse"></div>
                </div>
            </div>
            
            <!-- Contenido principal -->
            <div class="modal-body text-center px-4 py-3">
                <h5 class="modal-title text-dark mb-2 fw-bold">¿Cerrar sesión?</h5>
                <p class="text-muted mb-4 small">Se cerrará tu sesión actual y deberás ingresar nuevamente para continuar.</p>
                
                <!-- Información de usuario (opcional) -->
                <div class="user-info bg-light rounded-2 p-3 mb-3">
                    <div class="user-avatar bg-primary rounded-circle d-inline-flex align-items-center justify-content-center text-white mb-2" 
                         style="width: 40px; height: 40px; font-size: 1rem;">
                        <i class="fas fa-user"></i>
                    </div>
                    <div class="user-details">
                        <asp:Label ID="lblUsuario" runat="server" Text="Usuario" CssClass="fw-semibold d-block"></asp:Label>
                        <small class="text-muted">Última conexión: <asp:Label ID="lblUltimaConexion" runat="server" Text="Hoy"></asp:Label></small>
                    </div>
                </div>
            </div>

            <!-- Botones de acción MODIFICADOS -->
            <div class="modal-footer border-0 pt-0">
                <div class="w-100 d-flex gap-3">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CssClass="btn btn-outline-secondary flex-fill rounded-2 fw-semibold" 
                        OnClientClick="cancelLogout(); return false;" />
                    <asp:Button ID="btnConfirmarSalir" runat="server" Text="Cerrar" 
                        CssClass="btn btn-danger flex-fill rounded-2 fw-semibold shadow-sm" 
                        OnClick="btnConfirmarSalir_Click" />
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Pantalla de progreso -->
<div id="logoutProgress" class="logout-progress-container">
    <div class="logout-progress-content">
        <!-- Animación de carga -->
        <div class="progress-animation mb-4">
            <div class="progress-circle">
                <div class="progress-spinner"></div>
            </div>
            <div class="progress-check">
                <i class="fas fa-check"></i>
            </div>
        </div>
        
        <!-- Contenido informativo -->
        <div class="text-center">
            <h4 class="text-dark mb-3 fw-bold">Cerrando sesión</h4>
            <p class="text-muted mb-4">Estamos finalizando tu sesión de forma segura...</p>
            
            <!-- Progreso paso a paso -->
            <div class="logout-steps">
                <div class="step completed">
                    <span class="step-icon">1</span>
                    <span class="step-text">Verificando sesión</span>
                </div>
                <div class="step active">
                    <span class="step-icon">2</span>
                    <span class="step-text">Cerrando conexión</span>
                </div>
                <div class="step">
                    <span class="step-icon">3</span>
                    <span class="step-text">Redirigiendo</span>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    // Estado de la aplicación
    var logoutState = {
        isLoggingOut: false,
        userConfirmed: false
    };

    // Función para cancelar salida
    function cancelLogout() {
        if (!logoutState.isLoggingOut) {
            // Animación suave al cancelar
            var modal = document.getElementById('confirmLogoutModal');
            modal.style.transform = 'scale(0.9)';
            modal.style.opacity = '0';

            setTimeout(function () {
                window.history.back();
            }, 300);
        }
    }

    // Función para confirmar salida
    function confirmLogout() {
        if (!logoutState.userConfirmed) {
            logoutState.userConfirmed = true;
            showLogoutProgress();
        }
    }

    // Mostrar pantalla de progreso
    function showLogoutProgress() {
        var modal = document.getElementById('confirmLogoutModal');
        var progress = document.getElementById('logoutProgress');

        // Animación de transición
        modal.style.transform = 'scale(0.8)';
        modal.style.opacity = '0';

        setTimeout(function () {
            modal.style.display = 'none';
            progress.style.display = 'flex';

            // Iniciar animación de progreso
            startProgressAnimation();
        }, 400);
    }

    // Animación del progreso
    function startProgressAnimation() {
        var steps = document.querySelectorAll('.logout-steps .step');
        var currentStep = 1;

        // Paso 1: Ya completado
        setTimeout(function () {
            steps[1].classList.add('completed');
            steps[1].classList.remove('active');
            steps[2].classList.add('active');
            currentStep = 2;
        }, 1000);

        // Paso 2: Completado y redirección
        setTimeout(function () {
            steps[2].classList.add('completed');
            steps[2].classList.remove('active');

            // Mostrar check de confirmación
            document.querySelector('.progress-check').style.opacity = '1';
            document.querySelector('.progress-spinner').style.display = 'none';

            // Redirigir después de mostrar el check
            setTimeout(function () {
                window.location.href = 'Login.aspx?logout=success&t=' + new Date().getTime();
            }, 800);
        }, 2000);
    }

    // Prevenir acciones no deseadas
    document.addEventListener('DOMContentLoaded', function () {
        // Prevenir cierre con ESC
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                e.preventDefault();
                e.stopPropagation();
                cancelLogout();
            }
        });

        // Prevenir click fuera del modal
        document.getElementById('modalBackdrop').addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            cancelLogout();
        });

        // Efecto de entrada del modal
        setTimeout(function () {
            var modal = document.getElementById('confirmLogoutModal');
            modal.style.transform = 'scale(1)';
            modal.style.opacity = '1';
        }, 100);
    });

    // Prevenir doble clic en botones
    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('btn') && logoutState.isLoggingOut) {
            e.preventDefault();
            e.stopPropagation();
        }
    });
</script>

<style>
    /* Variables de colores */
    :root {
        --primary-color: #4361ee;
        --danger-color: #ef476f;
        --success-color: #06d6a0;
        --warning-color: #ffd166;
        --dark-color: #2b2d42;
        --light-color: #f8f9fa;
    }

    /* Animaciones */
    @keyframes pulse {
        0% { transform: scale(1); }
        50% { transform: scale(1.05); }
        100% { transform: scale(1); }
    }

    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }

    @keyframes checkmark {
        0% { transform: scale(0); opacity: 0; }
        50% { transform: scale(1.2); }
        100% { transform: scale(1); opacity: 1; }
    }

    /* Modal principal */
    .modal-backdrop {
        background: linear-gradient(135deg, rgba(43, 45, 66, 0.9) 0%, rgba(67, 97, 238, 0.7) 100%);
        z-index: 1040;
    }

    #confirmLogoutModal {
        z-index: 1050;
        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        transform: scale(0.7);
        opacity: 0;
    }

    .modal-content {
        border: none;
        background: white;
        transition: transform 0.3s ease;
    }

    /* Icono de logout animado */
    .logout-icon-container {
        position: relative;
        width: 80px;
        height: 80px;
        margin: 20px auto;
    }

    .logout-icon {
        width: 80px;
        height: 80px;
        background: linear-gradient(135deg, var(--danger-color) 0%, #d90429 100%);
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-size: 2rem;
        position: relative;
        z-index: 2;
        box-shadow: 0 8px 25px rgba(239, 71, 111, 0.3);
        animation: pulse 2s infinite;
    }

    .logout-pulse {
        position: absolute;
        top: -10px;
        left: -10px;
        right: -10px;
        bottom: -10px;
        background: rgba(239, 71, 111, 0.2);
        border-radius: 50%;
        animation: pulse 2s infinite 0.5s;
    }

    /* Información de usuario */
    .user-info {
        background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
        border: 1px solid rgba(0,0,0,0.05);
    }

    .user-avatar {
        background: linear-gradient(135deg, var(--primary-color) 0%, #3a56d4 100%);
    }

    /* BOTONES MODIFICADOS - UNO AL LADO DEL OTRO */
    .modal-footer .d-flex {
        display: flex !important;
    }

    .modal-footer .gap-3 {
        gap: 1rem !important;
    }

    .modal-footer .flex-fill {
        flex: 1 1 auto !important;
    }

    .btn {
        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        border: none;
        padding: 12px 16px;
        font-size: 0.95rem;
        min-height: 48px;
    }

    .btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 6px 20px rgba(0,0,0,0.15);
    }

    .btn:active {
        transform: translateY(0);
    }

    .btn-outline-secondary {
        border: 2px solid #6c757d;
        color: #6c757d;
        background: transparent;
        font-weight: 600;
    }

    .btn-outline-secondary:hover {
        background: #6c757d;
        color: white;
        border-color: #6c757d;
    }

    .btn-danger {
        background: linear-gradient(135deg, var(--danger-color) 0%, #d90429 100%);
        color: white;
        font-weight: 600;
    }

    /* Pantalla de progreso */
    .logout-progress-container {
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        align-items: center;
        justify-content: center;
        z-index: 1060;
    }

    .logout-progress-content {
        background: white;
        padding: 3rem;
        border-radius: 20px;
        box-shadow: 0 20px 60px rgba(0,0,0,0.2);
        text-align: center;
        max-width: 400px;
        width: 90%;
    }

    /* Animación de progreso */
    .progress-animation {
        position: relative;
        width: 80px;
        height: 80px;
        margin: 0 auto;
    }

    .progress-circle {
        width: 80px;
        height: 80px;
        border: 3px solid #e9ecef;
        border-radius: 50%;
        position: relative;
    }

    .progress-spinner {
        width: 60px;
        height: 60px;
        border: 3px solid transparent;
        border-top: 3px solid var(--primary-color);
        border-radius: 50%;
        position: absolute;
        top: 7px;
        left: 7px;
        animation: spin 1s linear infinite;
    }

    .progress-check {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        color: var(--success-color);
        font-size: 2rem;
        opacity: 0;
        transition: opacity 0.3s ease;
    }

    /* Pasos del logout */
    .logout-steps {
        display: flex;
        justify-content: space-between;
        margin: 2rem 0;
        position: relative;
    }

    .logout-steps::before {
        content: '';
        position: absolute;
        top: 20px;
        left: 10%;
        right: 10%;
        height: 2px;
        background: #e9ecef;
        z-index: 1;
    }

    .step {
        display: flex;
        flex-direction: column;
        align-items: center;
        position: relative;
        z-index: 2;
        flex: 1;
    }

    .step-icon {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        background: #e9ecef;
        color: #6c757d;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
        margin-bottom: 0.5rem;
        transition: all 0.3s ease;
    }

    .step-text {
        font-size: 0.8rem;
        color: #6c757d;
        transition: all 0.3s ease;
    }

    .step.active .step-icon {
        background: var(--primary-color);
        color: white;
        box-shadow: 0 4px 15px rgba(67, 97, 238, 0.3);
    }

    .step.active .step-text {
        color: var(--dark-color);
        font-weight: 600;
    }

    .step.completed .step-icon {
        background: var(--success-color);
        color: white;
    }

    .step.completed .step-text {
        color: var(--success-color);
    }

    /* Responsive para botones */
    @media (max-width: 576px) {
        .modal-footer .d-flex {
            flex-direction: column;
        }
        
        .modal-footer .gap-3 {
            gap: 0.75rem !important;
        }
    }

    /* Efectos de accesibilidad */
    @media (prefers-reduced-motion: reduce) {
        .logout-icon, .logout-pulse, .btn, .progress-spinner {
            animation: none;
        }
        
        .btn:hover {
            transform: none;
        }
    }
</style>

</asp:Content>