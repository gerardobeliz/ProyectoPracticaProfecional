<%@ Page Title="Alta de Alumnos" Language="C#" MasterPageFile="~/Principal.master"
    AutoEventWireup="true" CodeBehind="AltaAlumnos.aspx.cs" Inherits="proyectoPracticaProfecional.AltaAlumnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container py-4">
                <!-- Header Minimalista -->
                <div class="text-center mb-5">
                    <div class="header-icon mb-3">
                        <i class="fas fa-user-graduate"></i>
                    </div>
                    <h4 class="fw-light text-dark mb-2">
                        Sistema de Alta de Alumnos</h4>
                    <p class="text-muted">
                        Complete el formulario para registrar un nuevo alumno</p>
                </div>
                
                <!-- Form Card -->
                <div class="card border-0 shadow-sm rounded-3">
                    <div class="card-body p-4 p-md-5">
                        <div class="form-container">
                            <div class="row">
                                <!-- Columna 1: Datos Personales -->
                                <div class="col-lg-6 border-end pe-lg-4">
                                    <div class="mb-4">
                                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                            <i class="fas fa-user-circle me-2"></i>Información Personal
                                        </h6>
                                        <div class="row">
                                            <div class="col-12 mb-3">
                                                <label for="txtNombre" class="form-label required-field small fw-medium">
                                                    Nombre</label>
                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control form-control-sm" 
                                                    placeholder="Nombre del alumno" required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 mb-3">
                                                <label for="txtApellido" class="form-label required-field small fw-medium">
                                                    Apellido</label>
                                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control form-control-sm" 
                                                    placeholder="Apellido del alumno" required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="txtFechaNacimiento" class="form-label required-field small fw-medium">
                                                    Fecha de Nacimiento</label>
                                                <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control form-control-sm" 
                                                    TextMode="Date" required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ControlToValidate="txtFechaNacimiento"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="ddlGenero" class="form-label required-field small fw-medium">
                                                    Género</label>
                                                <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select form-select-sm" required="true">
                                                    <asp:ListItem Value="" Text="Seleccione género" Selected="True" />
                                                    <asp:ListItem Value="M" Text="Masculino" />
                                                    <asp:ListItem Value="F" Text="Femenino" />
                                                    <asp:ListItem Value="O" Text="Otro" />
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvGenero" runat="server" ControlToValidate="ddlGenero"
                                                    InitialValue="" ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="txtDocumento" class="form-label required-field small fw-medium">
                                                    Documento</label>
                                                <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control form-control-sm" 
                                                    placeholder="Solo números (8 dígitos)" MaxLength="8" required="true"></asp:TextBox>
                                                <asp:CustomValidator ID="cvDocumento" runat="server" ControlToValidate="txtDocumento"
                                                    OnServerValidate="cvDocumento_ServerValidate" 
                                                    Display="Dynamic" CssClass="text-danger small" ErrorMessage="El DNI debe tener exactamente 8 dígitos numéricos">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvDocumento" runat="server" ControlToValidate="txtDocumento"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="txtDireccion" class="form-label small fw-medium">
                                                    Dirección</label>
                                                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control form-control-sm" 
                                                    TextMode="MultiLine" Rows="3" placeholder="Dirección completa"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <!-- Columna 2: Contacto y Académica -->
                                <div class="col-lg-6 ps-lg-4">
                                    <!-- Sección Contacto -->
                                    <div class="mb-4">
                                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                            <i class="fas fa-envelope me-2"></i>Contacto
                                        </h6>
                                        <div class="row">
                                            <div class="col-12 mb-3">
                                                <label for="txtEmail" class="form-label required-field small fw-medium">
                                                    Email</label>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm" 
                                                    TextMode="Email" placeholder="ejemplo@correo.com" required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                    ErrorMessage="Formato inválido" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                                    Display="Dynamic" CssClass="text-danger small"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="col-12 mb-3">
                                                <label for="txtTelefono" class="form-label small fw-medium">
                                                    Teléfono</label>
                                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control form-control-sm" 
                                                    MaxLength="10" placeholder="Solo números (máx. 10)"></asp:TextBox>
                                                <asp:CustomValidator ID="cvTelefono" runat="server" ControlToValidate="txtTelefono"
                                                    OnServerValidate="cvTelefono_ServerValidate" 
                                                    Display="Dynamic" CssClass="text-danger small" ErrorMessage="Solo se permiten números (máx. 10 dígitos)">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <!-- Línea divisoria -->
                                    <div class="border-top my-4"></div>
                                    
                                    <!-- Sección Académica -->
                                    <div class="mb-4">
                                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                            <i class="fas fa-graduation-cap me-2"></i>Información Académica
                                        </h6>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="ddlCurso" class="form-label required-field small fw-medium">
                                                    Carrera</label>
                                                <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-select form-select-sm" required="true">
                                                    <asp:ListItem Value="" Text="Seleccione Carrera" Selected="True" />
                                                    <asp:ListItem Value="programacion" Text="Programación" />
                                                    <asp:ListItem Value="hoteleria" Text="Hoteleria" />
                                                    <asp:ListItem Value="educacion primaria" Text="Educación Primaria" />
                                                    <asp:ListItem Value="biologia" Text="Biología" />
                                                    <asp:ListItem Value="historia" Text="Historia" />
                                                    <asp:ListItem Value="psicopedagogia" Text="Psicopedagogía" />
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCurso" runat="server" ControlToValidate="ddlCurso"
                                                    InitialValue="" ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="txtFechaInscripcion" class="form-label required-field small fw-medium">
                                                    Fecha Inscripción</label>
                                                <asp:TextBox ID="txtFechaInscripcion" runat="server" CssClass="form-control form-control-sm" 
                                                    TextMode="Date" required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFechaInscripcion" runat="server" ControlToValidate="txtFechaInscripcion"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <!-- Botones -->
                            <div class="d-grid gap-3 d-md-flex justify-content-center mt-5 pt-3 border-top">
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Formulario" CssClass="btn btn-outline-secondary px-4 py-2"
                                    OnClick="btnLimpiar_Click" CausesValidation="false" />
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Alumno" 
                                    CssClass="btn btn-primary px-4 py-2" OnClick="btnGuardar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                
                <!-- Mensajes de confirmación -->
                <asp:Panel ID="pnlSuccessMessage" runat="server" CssClass="alert alert-success mt-4 py-3"
                    Visible="false" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="fas fa-check-circle me-3"></i>
                        <span>Alumno registrado correctamente en el sistema</span>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="alert alert-danger mt-4 py-3"
                    Visible="false" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="fas fa-exclamation-circle me-3"></i>
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
            </div>

            <!-- Modal de confirmación -->
            <div class="confirmation-modal" id="confirmationModal" style="display: none;">
                <div class="confirmation-content">
                    <div class="confirmation-icon">
                        <i class="fas fa-check-circle"></i>
                    </div>
                    <h3>¡Alumno registrado con éxito!</h3>
                    <p>Los datos del alumno han sido guardados correctamente en el sistema.</p>
                    <div class="student-details mt-4">
                        <p><strong>Nombre:</strong> <span id="confirmNombre" runat="server"></span></p>
                        <p><strong>Email:</strong> <span id="confirmEmail" runat="server"></span></p>
                        <p><strong>Carrera:</strong> <span id="confirmCarrera" runat="server"></span></p>
                    </div>
                    <button class="btn btn-primary mt-4" onclick="closeConfirmation()">Aceptar</button>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <style>
        /* Estilos para bordes visibles en todos los campos */
        .form-control, .form-select {
            border: 2px solid #d1d9e6 !important;
            border-radius: 8px;
            padding: 0.75rem 1rem;
            font-size: 1rem;
            transition: all 0.3s ease;
            background: #ffffff;
        }
        
        .form-control:focus, .form-select:focus {
            border-color: #3b82f6 !important;
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.15);
            outline: none;
        }
        
        .text-blue {
            color: #2c5aa0 !important;
        }
        
        .header-icon {
            width: 80px;
            height: 80px;
            margin: 0 auto;
            background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 2rem;
        }
        
        .card {
            border: 1px solid #d1d9e6;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
        }
        
        .section-title {
            color: #2c5aa0;
            letter-spacing: 0.5px;
            font-size: 0.9rem;
            font-weight: 600;
        }
        
        .required-field::after {
            content: " *";
            color: #dc3545;
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
            border: none;
            border-radius: 8px;
            font-weight: 500;
            font-size: 1rem;
            padding: 0.75rem 2rem;
            transition: all 0.3s ease;
        }
        
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(59, 130, 246, 0.3);
        }
        
        .btn-outline-secondary {
            border: 2px solid #d1d5db;
            border-radius: 8px;
            font-weight: 500;
            font-size: 1rem;
            padding: 0.75rem 2rem;
            color: #6b7280;
            transition: all 0.3s ease;
        }
        
        .btn-outline-secondary:hover {
            border-color: #3b82f6;
            background-color: #f8fafc;
            color: #3b82f6;
        }
        
        .border-top {
            border-color: #e5e7eb !important;
        }
        
        .alert {
            border: none;
            border-radius: 8px;
            border-left: 4px solid;
            background: #f8f9fa;
            font-size: 1rem;
        }
        
        .alert-success {
            border-left-color: #10b981;
            color: #065f46;
        }
        
        .alert-danger {
            border-left-color: #ef4444;
            color: #7f1d1d;
        }
        
        .fw-medium {
            font-weight: 500 !important;
        }
        
        .form-label {
            margin-bottom: 0.5rem;
            color: #374151;
            font-size: 0.95rem;
            font-weight: 500;
        }
        
        .text-danger.small {
            font-size: 0.85rem;
            margin-top: 0.25rem;
            display: block;
        }
        
        .confirmation-modal {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 1000;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        
        .confirmation-content {
            background: white;
            padding: 2rem;
            border-radius: 12px;
            text-align: center;
            max-width: 500px;
            width: 90%;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
        }
        
        .confirmation-icon {
            font-size: 4rem;
            color: #10b981;
            margin-bottom: 1rem;
        }
        
        /* Estilos para el diseño horizontal */
        .form-control-sm, .form-select-sm {
            padding: 0.7rem;
            font-size: 0.95rem;
        }
        
        .border-end {
            border-right: 1px solid #e5e7eb !important;
        }
        
        @media (max-width: 991.98px) {
            .border-end {
                border-right: none !important;
                border-bottom: 1px solid #e5e7eb !important;
                padding-right: 0 !important;
                margin-bottom: 1.5rem;
                padding-bottom: 1.5rem;
            }
            
            .ps-lg-4 {
                padding-left: 0 !important;
            }
        }
        
        .student-details {
            background-color: #f8f9fa;
            border-radius: 8px;
            padding: 1rem;
            text-align: left;
        }
        
        .student-details p {
            margin-bottom: 0.5rem;
        }
    </style>

    <script type="text/javascript">
        function showConfirmation(nombre, email, carrera) {
            document.getElementById('confirmNombre').textContent = nombre;
            document.getElementById('confirmEmail').textContent = email;
            document.getElementById('confirmCarrera').textContent = carrera;
            document.getElementById('confirmationModal').style.display = 'flex';
        }

        function closeConfirmation() {
            document.getElementById('confirmationModal').style.display = 'none';
        }

        // Validación de solo números
        function validarSoloNumeros(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                event.preventDefault();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>