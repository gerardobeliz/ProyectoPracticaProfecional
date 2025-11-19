<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="ModificarAlumno.aspx.cs" Inherits="proyectoPracticaProfecional.ModificarAlumno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

<div class="container-fluid min-vh-100 d-flex align-items-center justify-content-center py-5">
    <div class="row w-100 justify-content-center">
        <div class="col-xl-12 col-lg-10 col-md-12">
            <!-- Header Minimalista -->
            <div class="text-center mb-5">
                <div class="header-icon mb-3">
                    <i class="fas fa-user-edit"></i>
                </div>
                <h2 class="fw-light text-dark mb-2">
                    Modificar Datos del Alumno</h2>
                <p class="text-muted">
                    Actualice la información del alumno seleccionado</p>
            </div>
            
            <!-- Contenedor Principal - Búsqueda y Formulario en misma fila -->
            <div class="row">
                <!-- Columna de Búsqueda -->
                <div class="col-lg-10 mb-4">
                    <div class="card border-0 shadow-sm rounded-3">
                        <div class="card-body p-4">
                            <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue text-center">
                                <i class="fas fa-search me-2"></i>Buscar Alumno
                            </h6>
                            <div class="row align-items-end justify-content-center">
                                <div class="col-md-8 mb-3">
                                    <label for="txtBuscarAlumno" class="form-label small fw-medium text-center w-100">
                                        Buscar por nombre, apellido o documento</label>
                                    <asp:TextBox ID="txtBuscarAlumno" runat="server" CssClass="form-control text-center" 
                                        placeholder="Ingrese nombre, apellido o documento"></asp:TextBox>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                            CssClass="btn btn-primary px-4"
                                            OnClick="btnBuscar_Click" />
                                    </div>
                                </div>
                            </div>
                            <!-- Grid de resultados -->
                            <asp:Panel ID="pnlResultados" runat="server" Visible="false" CssClass="mt-3">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvAlumnos" runat="server" CssClass="table table-hover table-sm"
                                        AutoGenerateColumns="False" DataKeyNames="legajo" OnSelectedIndexChanged="gvAlumnos_SelectedIndexChanged"
                                        GridLines="None" BorderStyle="None">
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seleccionar" 
                                                ControlStyle-CssClass="btn btn-outline-primary btn-sm">
                                                <ControlStyle CssClass="btn btn-outline-primary btn-sm" />
                                            </asp:CommandField>
                                            <asp:BoundField DataField="legajo" HeaderText="Legajo" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="documento" HeaderText="Documento" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="apellido" HeaderText="Apellido" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                        </Columns>
                                        <HeaderStyle CssClass="table-primary" />
                                        <RowStyle CssClass="align-middle" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <!-- Columna del Formulario -->
                <div class="col-lg-6 mb-4">
                    <asp:Panel ID="pnlFormulario" runat="server" Visible="false">
                        <div class="card border-0 shadow-sm rounded-3">
                            <div class="card-body p-4">
                                <div class="d-flex justify-content-between align-items-center mb-4">
                                    <h6 class="section-title mb-0 text-uppercase small fw-bold text-blue">
                                        <i class="fas fa-user-circle me-2"></i>Editando Alumno
                                    </h6>
                                    <asp:Label ID="lblIDAlumno" runat="server" Visible="false"></asp:Label>
                                </div>
                                
                                <!-- Información del alumno seleccionado -->
                                <div class="alert alert-info mb-4 py-2">
                                    <div class="d-flex align-items-center">
                                        <i class="fas fa-info-circle me-2"></i>
                                        <div>
                                            <strong>Alumno seleccionado:</strong>
                                            <asp:Label ID="lblAlumnoSeleccionado" runat="server" CssClass="ms-1"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-container">
                                    <!-- Sección 1: Datos Personales -->
                                    <div class="mb-4">
                                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                            <i class="fas fa-user-circle me-2"></i>Información Personal
                                        </h6>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="txtNombre" class="form-label required-field small fw-medium">
                                                    Nombre</label>
                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" required="true"
                                                    placeholder="Nombre del alumno"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="txtApellido" class="form-label required-field small fw-medium">
                                                    Apellido</label>
                                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" required="true"
                                                    placeholder="Apellido del alumno"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="txtFechaNacimiento" class="form-label required-field small fw-medium">
                                                    Fecha de Nacimiento</label>
                                                <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date"
                                                    required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ControlToValidate="txtFechaNacimiento"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="ddlGenero" class="form-label required-field small fw-medium">
                                                    Género</label>
                                                <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select" required="true">
                                                    <asp:ListItem Value="" Text="Seleccione género" />
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
                                                    Documento (DNI)</label>
                                                <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" required="true"
                                                    placeholder="Número de documento (8 dígitos)" MaxLength="8"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDocumento" runat="server" ControlToValidate="txtDocumento"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revDocumento" runat="server" 
                                                    ControlToValidate="txtDocumento"
                                                    ErrorMessage="El DNI debe tener exactamente 8 dígitos" 
                                                    ValidationExpression="^\d{8}$"
                                                    Display="Dynamic" CssClass="text-danger small">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="txtDireccion" class="form-label small fw-medium">
                                                    Dirección</label>
                                                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                    Rows="2" placeholder="Dirección completa"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <!-- Sección 2: Contacto -->
                                    <div class="mb-4">
                                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                            <i class="fas fa-envelope me-2"></i>Contacto
                                        </h6>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="txtEmail" class="form-label required-field small fw-medium">
                                                    Email</label>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"
                                                    required="true" placeholder="ejemplo@correo.com"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                    ErrorMessage="Formato inválido" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                                    Display="Dynamic" CssClass="text-danger small"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="txtTelefono" class="form-label small fw-medium">
                                                    Teléfono (10 dígitos)</label>
                                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" TextMode="Phone"
                                                    placeholder="11 1234-5678" MaxLength="10"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revTelefono" runat="server" 
                                                    ControlToValidate="txtTelefono"
                                                    ErrorMessage="El teléfono debe tener 10 dígitos" 
                                                    ValidationExpression="^\d{10}$"
                                                    Display="Dynamic" CssClass="text-danger small">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <!-- Sección 3: Académica -->
                                    <div class="mb-4">
                                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                            <i class="fas fa-graduation-cap me-2"></i>Información Académica
                                        </h6>
                                        <div class="row">
                                            <!-- TextBox para Carrera (No editable) -->
                                            <div class="col-md-6 mb-3">
                                                <label for="txtCarrera" class="form-label required-field small fw-medium">
                                                    Carrera</label>
                                                <asp:TextBox ID="txtCarrera" runat="server" CssClass="form-control" 
                                                    ReadOnly="true" BackColor="#f8f9fa" required="true"
                                                    placeholder="Carrera del alumno"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvCarrera" runat="server" ControlToValidate="txtCarrera"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="txtFechaInscripcion" class="form-label required-field small fw-medium">
                                                    Fecha Inscripción</label>
                                                <asp:TextBox ID="txtFechaInscripcion" runat="server" CssClass="form-control" TextMode="Date"
                                                    required="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFechaInscripcion" runat="server" ControlToValidate="txtFechaInscripcion"
                                                    ErrorMessage="Requerido" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <!-- Botones -->
                                    <div class="d-grid gap-2 d-md-flex justify-content-center mt-4 pt-3 border-top">
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary px-3 py-2"
                                            OnClick="btnCancelar_Click" CausesValidation="false" />
                                        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar Datos" CssClass="btn btn-primary px-3 py-2"
                                            OnClick="btnActualizar_Click" />
                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger px-3 py-2"
                                            OnClick="btnEliminar_Click" CausesValidation="false" 
                                            OnClientClick="return confirmarEliminacion();" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <!-- Mensajes -->
            <asp:Panel ID="pnlSuccessMessage" runat="server" CssClass="alert alert-success mt-4 py-3"
                Visible="false" role="alert">
                <div class="d-flex align-items-center">
                    <i class="fas fa-check-circle me-3"></i>
                    <asp:Label ID="lblSuccessMessage" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="alert alert-danger mt-4 py-3"
                Visible="false" role="alert">
                <div class="d-flex align-items-center">
                    <i class="fas fa-exclamation-circle me-3"></i>
                    <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

<!-- Script para confirmación de eliminación -->
<script type="text/javascript">
    function confirmarEliminacion() {
        var alumnoSeleccionado = document.getElementById('<%= lblAlumnoSeleccionado.ClientID %>');
        var nombreAlumno = alumnoSeleccionado ? alumnoSeleccionado.innerText : 'este alumno';

        var mensaje = "¿Está seguro que desea eliminar a " + nombreAlumno + "?\n\n" +
                     "⚠️ ADVERTENCIA: Esta acción también eliminará TODOS los cursos asociados al alumno.\n\n" +
                     "Esta operación NO se puede deshacer.";

        return confirm(mensaje);
    }
</script>

    </ContentTemplate>
</asp:UpdatePanel>
<style>
    /* Estilos consistentes con la página de alta */
    body
    {
        background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
        font-family: 'Inter' , 'Segoe UI' , system-ui, -apple-system, sans-serif;
    }
    
    .text-blue
    {
        color: #2c5aa0 !important;
    }
    
    .header-icon
    {
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
    
    .card
    {
        border: 1px solid #e2e8f0;
        background: #ffffff;
    }
    
    .form-control, .form-select
    {
        border: 1px solid #d1d5db;
        border-radius: 8px;
        padding: 0.75rem 1rem;
        font-size: 0.9rem;
        transition: all 0.2s ease;
        background: #ffffff;
    }
    
    .form-control:focus, .form-select:focus
    {
        border-color: #3b82f6;
        box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.15);
        outline: none;
    }
    
    /* Estilo específico para campos de solo lectura */
    .form-control[readonly]
    {
        background-color: #f8f9fa !important;
        border-color: #e2e8f0;
        cursor: not-allowed;
        color: #4b5563;
    }
    
    .section-title
    {
        color: #2c5aa0;
        letter-spacing: 0.5px;
        font-size: 0.9rem;
    }
    
    .required-field::after
    {
        content: " *";
        color: #dc3545;
    }
    
    .btn-primary
    {
        background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
        font-size: 0.9rem;
        padding: 0.75rem 1.5rem;
        transition: all 0.2s ease;
    }
    
    .btn-primary:hover
    {
        transform: translateY(-2px);
        box-shadow: 0 6px 20px rgba(59, 130, 246, 0.3);
    }
    
    .btn-outline-secondary
    {
        border: 2px solid #d1d5db;
        border-radius: 8px;
        font-weight: 500;
        font-size: 0.9rem;
        padding: 0.75rem 1.5rem;
        color: #6b7280;
        transition: all 0.2s ease;
    }
    
    .btn-outline-secondary:hover
    {
        border-color: #3b82f6;
        background-color: #f8fafc;
        color: #3b82f6;
    }
    
    .btn-danger
    {
        background: linear-gradient(135deg, #dc3545 0%, #c53030 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
        font-size: 0.9rem;
        padding: 0.75rem 1.5rem;
        transition: all 0.2s ease;
    }
    
    .btn-danger:hover
    {
        transform: translateY(-2px);
        box-shadow: 0 6px 20px rgba(220, 53, 69, 0.3);
    }
    
    .table-hover tbody tr:hover
    {
        background-color: #f8fafc;
    }
    
    .table-primary
    {
        background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
        color: white;
    }
    
    .alert
    {
        border: none;
        border-radius: 8px;
        border-left: 4px solid;
        background: #f8f9fa;
        font-size: 0.9rem;
    }
    
    .alert-success
    {
        border-left-color: #10b981;
        color: #065f46;
    }
    
    .alert-danger
    {
        border-left-color: #ef4444;
        color: #7f1d1d;
    }
    
    .alert-info
    {
        border-left-color: #3b82f6;
        background-color: #f0f7ff;
        color: #1e40af;
    }
</style>
<!-- Font Awesome Icons -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
</asp:Content>