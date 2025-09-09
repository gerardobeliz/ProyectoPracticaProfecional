<%@ Page Title="Alta de Alumnos" Language="C#" MasterPageFile="~/Principal.master" AutoEventWireup="true" CodeBehind="AltaAlumnos.aspx.cs" Inherits="proyectoPracticaProfecional.AltaAlumnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div class="container-fluid min-vh-100 d-flex align-items-center justify-content-center py-5">
        <div class="row w-100 justify-content-center">
            <div class="col-xl-12 col-lg-10 col-md-12"> <!-- Aumenté el ancho máximo -->
                <!-- Header Minimalista -->
                <div class="text-center mb-5">
                    <div class="header-icon mb-3">
                        <i class="fas fa-user-graduate"></i>
                    </div>
                    <h4 class="fw-light text-dark mb-2">Sistema de Alta de Alumnos</h4>
                    <p class="text-muted">Complete el formulario para registrar un nuevo alumno</p>
                </div>

                <!-- Form Card - Ahora más ancho -->
                <div class="card border-0 shadow-sm rounded-3">
                    <div class="card-body p-4 p-md-5">
                        <div class="form-container">
                            <!-- Sección 1: Datos Personales -->
                            <div class="mb-4">
                                <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                    <i class="fas fa-user-circle me-2"></i>
                                    Información Personal
                                </h6>
                                
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label for="txtNombre" class="form-label required-field small fw-medium">Nombre</label>
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" 
                                            required="true" placeholder="Nombre del alumno"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                            ControlToValidate="txtNombre" ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="txtApellido" class="form-label required-field small fw-medium">Apellido</label>
                                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" 
                                            required="true" placeholder="Apellido del alumno"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" 
                                            ControlToValidate="txtApellido" ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label for="txtFechaNacimiento" class="form-label required-field small fw-medium">Fecha de Nacimiento</label>
                                        <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" 
                                            TextMode="Date" required="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" 
                                            ControlToValidate="txtFechaNacimiento" ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="ddlGenero" class="form-label required-field small fw-medium">Género</label>
                                        <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select" required="true">
                                            <asp:ListItem Value="" Text="Seleccione género" Selected="True" />
                                            <asp:ListItem Value="M" Text="Masculino" />
                                            <asp:ListItem Value="F" Text="Femenino" />
                                            <asp:ListItem Value="O" Text="Otro" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvGenero" runat="server" 
                                            ControlToValidate="ddlGenero" InitialValue=""
                                            ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                     <div class="col-md-6 mb-3">
        <label for="txtDocumento" class="form-label required-field small fw-medium">Documento</label>
        <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" 
            required="true" placeholder="Solo números (8 dígitos)"
            MaxLength="8" onkeypress="return validarSoloNumeros(event)"></asp:TextBox>
        
        <!-- AGREGA este CustomValidator para DNI -->
        <asp:CustomValidator ID="cvDocumento" runat="server" 
            ControlToValidate="txtDocumento"
            OnServerValidate="cvDocumento_ServerValidate"
            ClientValidationFunction="validarDocumentoCliente"
            Display="Dynamic"
            CssClass="text-danger small"
            ErrorMessage="El DNI debe tener exactamente 8 dígitos numéricos">
        </asp:CustomValidator>
        
        <asp:RequiredFieldValidator ID="rfvDocumento" runat="server" 
            ControlToValidate="txtDocumento" ErrorMessage="Requerido" 
            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="txtDireccion" class="form-label small fw-medium">Dirección</label>
                                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" 
                                            TextMode="MultiLine" Rows="3" placeholder="Dirección completa"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <!-- Línea divisoria -->
                            <div class="border-top my-4"></div>

                            <!-- Sección 2: Contacto -->
                            <div class="mb-4">
                                <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                    <i class="fas fa-envelope me-2"></i>
                                    Contacto
                                </h6>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label for="txtEmail" class="form-label required-field small fw-medium">Email</label>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                            TextMode="Email" required="true" placeholder="ejemplo@correo.com"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                            ControlToValidate="txtEmail" ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                            ControlToValidate="txtEmail" ErrorMessage="Formato inválido"
                                            ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RegularExpressionValidator>
                                    </div>
                                     <!-- Modifica la sección del teléfono: -->
          <div class="col-md-6 mb-3">
        <label for="txtTelefono" class="form-label small fw-medium">Teléfono</label>
        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" 
            TextMode="Number" MaxLength="11" placeholder="Solo números (máx. 11)"
            onkeypress="return validarSoloNumeros(event)"></asp:TextBox>
        
        <!-- AGREGA este CustomValidator -->
        <asp:CustomValidator ID="cvTelefono" runat="server" 
            ControlToValidate="txtTelefono"
            OnServerValidate="cvTelefono_ServerValidate"
            ClientValidationFunction="validarTelefonoCliente"
            Display="Dynamic"
            CssClass="text-danger small"
            ErrorMessage="Solo se permiten números (máx. 11 dígitos)">
        </asp:CustomValidator>
    </div>
                                </div>
                            </div>

                            <!-- Línea divisoria -->
                            <div class="border-top my-4"></div>

                            <!-- Sección 3: Académica -->
                            <div class="mb-4">
                                <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                    <i class="fas fa-graduation-cap me-2"></i>
                                    Información Académica
                                </h6>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label for="ddlCurso" class="form-label required-field small fw-medium">Carrera</label>
                                        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-select" required="true">
                                            <asp:ListItem Value="" Text="Seleccione Carrera" Selected="True" />
                                            <asp:ListItem Value="programacion" Text="programacion" />
                                            <asp:ListItem Value="hoteleria" Text="hoteleria" />
                                            <asp:ListItem Value="educacion primaria" Text="educacion primaria" />
                                            <asp:ListItem Value="biologia" Text="biologia" />
                                            <asp:ListItem Value="historia" Text="historia" />
                                            <asp:ListItem Value="psicopedagogia" Text="psicopedagogia" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCurso" runat="server" 
                                            ControlToValidate="ddlCurso" InitialValue=""
                                            ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label for="txtFechaInscripcion" class="form-label required-field small fw-medium">Fecha Inscripción</label>
                                        <asp:TextBox ID="txtFechaInscripcion" runat="server" CssClass="form-control" 
                                            TextMode="Date" required="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFechaInscripcion" runat="server" 
                                            ControlToValidate="txtFechaInscripcion" ErrorMessage="Requerido" 
                                            Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-check form-switch mt-3">
                                    <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label small fw-medium" for="chkActivo">
                                        Alumno activo
                                    </label>
                                </div>
                            </div>

                            <!-- Botones más grandes y mejor espaciados -->
                            <div class="d-grid gap-3 d-md-flex justify-content-center mt-5 pt-3">
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Formulario" 
                                    CssClass="btn btn-outline-secondary px-4 py-2" 
                                    OnClick="btnLimpiar_Click" CausesValidation="false" />
                    <!-- Modifica el botón de guardar: -->
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Alumno" 
        CssClass="btn btn-primary px-4 py-2" OnClick="btnGuardar_Click" />
    </div>
                            
                            <!-- Mensajes -->
                            <asp:Panel ID="pnlSuccessMessage" runat="server" CssClass="alert alert-success mt-4 py-3" Visible="false" role="alert">
                                <div class="d-flex align-items-center">
                                    <i class="fas fa-check-circle me-3"></i>
                                    <span>Alumno registrado correctamente en el sistema</span>
                                </div>
                            </asp:Panel>
                            
                            <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="alert alert-danger mt-4 py-3" Visible="false" role="alert">
                                <div class="d-flex align-items-center">
                                    <i class="fas fa-exclamation-circle me-3"></i>
                                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        /* Estilo minimalista con azul profesional - Ajustado para más ancho */
        body {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            font-family: 'Inter', 'Segoe UI', system-ui, -apple-system, sans-serif;
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
            border: 1px solid #e2e8f0;
            background: #ffffff;
            max-width: 1200px;
            margin: 0 auto;
        }
        
        .form-control, .form-select {
            border: 1px solid #d1d5db;
            border-radius: 8px;
            padding: 0.75rem 1rem;
            font-size: 1rem;
            transition: all 0.2s ease;
            background: #ffffff;
            height: auto;
        }
        
        .form-control:focus, .form-select:focus {
            border-color: #3b82f6;
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.15);
            outline: none;
        }
        
        .section-title {
            color: #2c5aa0;
            letter-spacing: 0.5px;
            font-size: 0.9rem;
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
            transition: all 0.2s ease;
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
            transition: all 0.2s ease;
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
        
        .form-check-input:checked {
            background-color: #2c5aa0;
            border-color: #2c5aa0;
        }
        
        .form-check-input:focus {
            box-shadow: 0 0 0 3px rgba(44, 90, 160, 0.1);
        }
        
        .shadow-sm {
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06) !important;
        }
        
        .rounded-3 {
            border-radius: 16px !important;
        }
        
        .fw-medium {
            font-weight: 500 !important;
        }
        
        /* Mejora la legibilidad de las etiquetas */
        .form-label {
            margin-bottom: 0.5rem;
            color: #374151;
            font-size: 0.95rem;
        }
        
        /* Espaciado mejorado */
        .mb-3 {
            margin-bottom: 1.25rem !important;
        }
        
        .mb-4 {
            margin-bottom: 2rem !important;
        }
        
        /* Ajustes para pantallas grandes */
        @media (min-width: 1200px) {
            .col-xl-8 {
                flex: 0 0 auto;
                width: 90%;
            }
        }
        
        @media (min-width: 1400px) {
            .col-xl-8 {
                flex: 0 0 auto;
                width: 85%;
            }
        }
        
        /* Mejor contenedor principal */
        .container-fluid {
            padding-left: 2rem;
            padding-right: 2rem;
        }
    </style>
    <style>
    /* AGREGA esto en el estilo existente */
    .text-danger.small {
        font-size: 0.85rem;
        margin-top: 0.25rem;
        display: block;
    }

    input[type="number"] {
        -moz-appearance: textfield;
    }

    input[type="number"]::-webkit-outer-spin-button,
    input[type="number"]::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }
</style>
<style>
    /* Estilos para validadores */
    .text-danger.small {
        font-size: 0.85rem;
        margin-top: 0.25rem;
        display: block;
    }

    /* Para campos numéricos */
    input[type="number"] {
        -moz-appearance: textfield;
    }

    input[type="number"]::-webkit-outer-spin-button,
    input[type="number"]::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }

    /* Para el campo de documento también */
    input[type="text"] {
        -moz-appearance: textfield;
    }
</style>
    <!-- Font Awesome Icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">

   <script type="text/javascript">
       // Función compartida para validar solo números
       function validarSoloNumeros(event) {
           var charCode = (event.which) ? event.which : event.keyCode;
           if (charCode > 31 && (charCode < 48 || charCode > 57)) {
               event.preventDefault();
               return false;
           }
           return true;
       }

       // Validación CLIENTE para Documento/DNI
       function validarDocumentoCliente(source, arguments) {
           var documento = arguments.Value;
           var regex = /^\d+$/;

           if (!regex.test(documento)) {
               arguments.IsValid = false; // Contiene letras o caracteres inválidos
           } else if (documento.length !== 8) {
               arguments.IsValid = false; // No tiene exactamente 8 dígitos
           } else {
               arguments.IsValid = true; // Válido
           }
       }

       // Validación CLIENTE para Teléfono
       function validarTelefonoCliente(source, arguments) {
           var telefono = arguments.Value;
           var regex = /^\d+$/;

           if (telefono === "") {
               arguments.IsValid = true; // Opcional
           } else if (!regex.test(telefono)) {
               arguments.IsValid = false; // Contiene letras
           } else if (telefono.length > 11) {
               arguments.IsValid = false; // Más de 11 dígitos
           } else {
               arguments.IsValid = true; // Válido
           }
       }

       document.addEventListener('DOMContentLoaded', function () {
           // Configuración para Documento/DNI
           var txtDocumento = document.getElementById('<%= txtDocumento.ClientID %>');
           if (txtDocumento) {
               txtDocumento.addEventListener('paste', function (e) {
                   e.preventDefault();
                   var text = (e.clipboardData || window.clipboardData).getData('text');
                   var numeros = text.replace(/\D/g, '');
                   document.execCommand('insertText', false, numeros);
               });

               txtDocumento.addEventListener('input', function () {
                   if (this.value.length > 8) {
                       this.value = this.value.slice(0, 8); // Limita a 8 caracteres
                   }
               });
           }

           // Configuración para Teléfono
           var txtTelefono = document.getElementById('<%= txtTelefono.ClientID %>');
           if (txtTelefono) {
               txtTelefono.addEventListener('paste', function (e) {
                   e.preventDefault();
                   var text = (e.clipboardData || window.clipboardData).getData('text');
                   var numeros = text.replace(/\D/g, '');
                   document.execCommand('insertText', false, numeros);
               });

               txtTelefono.addEventListener('input', function () {
                   if (this.value.length > 11) {
                       this.value = this.value.slice(0, 11); // Limita a 11 caracteres
                   }
               });
           }
       });
    </script>
  <script type="text/javascript">
      function validarSoloNumeros(event) {
          var charCode = (event.which) ? event.which : event.keyCode;
          if (charCode > 31 && (charCode < 48 || charCode > 57)) {
              event.preventDefault();
              return false;
          }
          return true;
      }

      function validarTelefonoCliente(source, arguments) {
          var telefono = arguments.Value;
          var regex = /^\d+$/;

          if (telefono === "") {
              arguments.IsValid = true;
          } else if (!regex.test(telefono)) {
              arguments.IsValid = false;
          } else if (telefono.length > 11) {
              arguments.IsValid = false;
          } else {
              arguments.IsValid = true;
          }
      }

      document.addEventListener('DOMContentLoaded', function () {
          var txtTelefono = document.getElementById('<%= txtTelefono.ClientID %>');

          if (txtTelefono) {
              txtTelefono.addEventListener('paste', function (e) {
                  e.preventDefault();
                  var text = (e.clipboardData || window.clipboardData).getData('text');
                  var numeros = text.replace(/\D/g, '');
                  document.execCommand('insertText', false, numeros);
              });

              txtTelefono.addEventListener('input', function () {
                  if (this.value.length > 10) {
                      this.value = this.value.slice(0, 10);
                  }
              });
          }
      });
    </script>
</asp:Content>

