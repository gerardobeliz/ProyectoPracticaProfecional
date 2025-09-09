<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="ModificarAlumno.aspx.cs" Inherits="proyectoPracticaProfecional.ModificarAlumno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="container-fluid min-vh-100 d-flex align-items-center justify-content-center py-5">
        <div class="row w-100 justify-content-center">
            <div class="col-xl-12 col-lg-10 col-md-12">
                <!-- Header Minimalista -->
                <div class="text-center mb-5">
                    <div class="header-icon mb-3">
                        <i class="fas fa-user-edit"></i>
                    </div>
                    <h2 class="fw-light text-dark mb-2">Modificar Datos del Alumno</h2>
                    <p class="text-muted">Actualice la información del alumno seleccionado</p>
                </div>

                <!-- Card de Búsqueda -->
                <div class="card border-0 shadow-sm rounded-3 mb-4">
                    <div class="card-body p-4">
                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                            <i class="fas fa-search me-2"></i>
                            Buscar Alumno
                        </h6>
                        
                        <div class="row align-items-end">
                            <div class="col-md-8 mb-3">
                                <label for="txtBuscarAlumno" class="form-label small fw-medium">Buscar por nombre, apellido o documento</label>
                                <asp:TextBox ID="txtBuscarAlumno" runat="server" CssClass="form-control" 
                                    placeholder="Ingrese nombre, apellido o documento"></asp:TextBox>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                    CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
                            </div>
                        </div>

                        <!-- Grid de resultados -->
                        <asp:Panel ID="pnlResultados" runat="server" Visible="false" CssClass="mt-3">
                            <div class="table-responsive">
                                <asp:GridView ID="gvAlumnos" runat="server" CssClass="table table-hover table-sm"
                                    AutoGenerateColumns="false" DataKeyNames="ID" OnSelectedIndexChanged="gvAlumnos_SelectedIndexChanged"
                                    GridLines="None" BorderStyle="None">
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seleccionar" 
                                            ControlStyle-CssClass="btn btn-outline-primary btn-sm" />
                                        <asp:BoundField DataField="Documento" HeaderText="Documento" HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                        <asp:BoundField DataField="Curso" HeaderText="Curso" HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                    </Columns>
                                    <HeaderStyle CssClass="table-primary" />
                                    <RowStyle CssClass="align-middle" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </div>
                </div>

                <!-- Form Card - Solo visible cuando se selecciona un alumno -->
                <asp:Panel ID="pnlFormulario" runat="server" Visible="false">
                    <div class="card border-0 shadow-sm rounded-3">
                        <div class="card-body p-4 p-md-5">
                            <div class="d-flex justify-content-between align-items-center mb-4">
                                <h6 class="section-title mb-0 text-uppercase small fw-bold text-blue">
                                    <i class="fas fa-user-circle me-2"></i>
                                    Editando: <asp:Label ID="lblAlumnoSeleccionado" runat="server" CssClass="text-dark"></asp:Label>
                                </h6>
                                <asp:Label ID="lblIDAlumno" runat="server" Visible="false"></asp:Label>
                            </div>

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
                                                <asp:ListItem Value="" Text="Seleccione género" />
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
                                                required="true" placeholder="Número de documento"></asp:TextBox>
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
                                        <div class="col-md-6 mb-3">
                                            <label for="txtTelefono" class="form-label small fw-medium">Teléfono</label>
                                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" 
                                                TextMode="Phone" placeholder="+54 11 1234-5678"></asp:TextBox>
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
                                            <label for="ddlCurso" class="form-label required-field small fw-medium">Curso</label>
                                            <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-select" required="true">
                                                <asp:ListItem Value="" Text="Seleccione curso" />
                                                <asp:ListItem Value="1" Text="Primero Básico" />
                                                <asp:ListItem Value="2" Text="Segundo Básico" />
                                                <asp:ListItem Value="3" Text="Tercero Básico" />
                                                <asp:ListItem Value="4" Text="Cuarto Básico" />
                                                <asp:ListItem Value="5" Text="Quinto Básico" />
                                                <asp:ListItem Value="6" Text="Sexto Básico" />
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
                                        <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label small fw-medium" for="chkActivo">
                                            Alumno activo
                                        </label>
                                    </div>
                                </div>

                                <!-- Botones -->
                                <div class="d-grid gap-3 d-md-flex justify-content-center mt-5 pt-3">
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                                        CssClass="btn btn-outline-secondary px-4 py-2" 
                                        OnClick="btnCancelar_Click" CausesValidation="false" />
                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                                        CssClass="btn btn-danger px-4 py-2" OnClick="btnEliminar_Click" 
                                        CausesValidation="false" OnClientClick="return confirm('¿Está seguro de eliminar este alumno?');" />
                                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar Datos" 
                                        CssClass="btn btn-primary px-4 py-2" OnClick="btnActualizar_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Mensajes -->
                <asp:Panel ID="pnlSuccessMessage" runat="server" CssClass="alert alert-success mt-4 py-3" Visible="false" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="fas fa-check-circle me-3"></i>
                        <asp:Label ID="lblSuccessMessage" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="alert alert-danger mt-4 py-3" Visible="false" role="alert">
                    <div class="d-flex align-items-center">
                        <i class="fas fa-exclamation-circle me-3"></i>
                        <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <style>
        /* Estilos consistentes con la página de alta */
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
        }
        
        .form-control, .form-select {
            border: 1px solid #d1d5db;
            border-radius: 8px;
            padding: 0.75rem 1rem;
            font-size: 1rem;
            transition: all 0.2s ease;
            background: #ffffff;
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
        
        .btn-danger {
            background: linear-gradient(135deg, #dc3545 0%, #c53030 100%);
            border: none;
            border-radius: 8px;
            font-weight: 500;
            font-size: 1rem;
            padding: 0.75rem 2rem;
            transition: all 0.2s ease;
        }
        
        .btn-danger:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(220, 53, 69, 0.3);
        }
        
        .table-hover tbody tr:hover {
            background-color: #f8fafc;
        }
        
        .table-primary {
            background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
            color: white;
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
    </style>

    <!-- Font Awesome Icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
</asp:Content>
