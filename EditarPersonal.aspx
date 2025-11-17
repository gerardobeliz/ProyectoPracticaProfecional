<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="EditarPersonal.aspx.cs" Inherits="proyectoPracticaProfecional.EditarPersonal" %>
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
                    Editar PERSONAL</h2>
                <p class="text-muted">
                    Busque y edite la información del personal</p>
            </div>
            
            <!-- Contenedor Principal -->
            <div class="row justify-content-center">
                <!-- Columna de Búsqueda -->
                <div class="col-lg-10 mb-4">
                    <div class="card border-0 shadow-sm rounded-3">
                        <div class="card-body p-4">
                            <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue text-center">
                                <i class="fas fa-search me-2"></i>Buscar Empleado a Editar
                            </h6>
                            <div class="row align-items-end justify-content-center">
                                <div class="col-md-8 mb-3">
                                    <label class="form-label small fw-medium text-center w-100">
                                        Buscar por nombre, apellido o DNI</label>
                                    <asp:TextBox ID="txtBuscarEmpleado" runat="server" CssClass="form-control text-center" 
                                        placeholder="Ingrese nombre, apellido o DNI"></asp:TextBox>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                            CssClass="btn btn-primary px-4" OnClick="btnBuscar_Click" />
                                    </div>
                                </div>
                            </div>
                            
                            <!-- Grid de resultados -->
                            <asp:Panel ID="pnlResultados" runat="server" Visible="false" CssClass="mt-3">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvEmpleados" runat="server" CssClass="table table-hover table-sm"
                                        AutoGenerateColumns="False" DataKeyNames="ID_PERSONAL" 
                                        OnSelectedIndexChanged="gvEmpleados_SelectedIndexChanged"
                                        GridLines="None" BorderStyle="None">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnSeleccionar" runat="server" 
                                                        CommandName="Select" 
                                                        Text="Seleccionar" 
                                                        CssClass="btn btn-outline-primary btn-sm"
                                                        OnClientClick='<%# "seleccionarEmpleado(\"" + Eval("ID_PERSONAL") + "\"); return false;" %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID_PERSONAL" HeaderText="ID" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="DNI" HeaderText="DNI" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="apellido" HeaderText="Apellido" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="TIPO" HeaderText="Tipo" 
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

                <!-- Panel de Edición -->
                <div class="col-lg-10 mb-4">
                    <asp:Panel ID="pnlEdicion" runat="server" Visible="false">
                        <div class="card border-0 shadow-sm rounded-3 border-primary">
                            <div class="card-body p-4">
                                <div class="text-center mb-4">
                                    <i class="fas fa-user-edit text-primary fa-3x mb-3"></i>
                                    <h5 class="text-primary fw-bold">Editar Información del Empleado</h5>
                                    <asp:Label ID="lblEmpleadoAEditar" runat="server" CssClass="text-muted"></asp:Label>
                                </div>

                                <!-- Formulario de edición -->
                                <div class="mb-4 p-3 bg-light rounded">
                                    <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                        <i class="fas fa-user-edit me-2"></i>Datos del Empleado
                                    </h6>
                                    
                                    <div class="row">
                                        <!-- Información básica -->
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label small fw-medium">ID / Legajo</label>
                                            <asp:TextBox ID="txtLegajo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label small fw-medium">DNI *</label>
                                            <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDocumento" runat="server" 
                                                ControlToValidate="txtDocumento" ErrorMessage="El DNI es obligatorio" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revDocumento" runat="server"
                                                ControlToValidate="txtDocumento" ErrorMessage="El DNI debe contener solo números"
                                                ValidationExpression="^\d+$" CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-4 mb-3">
                                            <label class="form-label small fw-medium">Tipo/Cargo *</label>
                                            <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-select">
                                                <asp:ListItem Text="Seleccione un cargo" Value=""></asp:ListItem>
                                                <asp:ListItem Text="directivo" Value="directivo"></asp:ListItem>
                                                <asp:ListItem Text="preceptor" Value="preceptor"></asp:ListItem>
                                                <asp:ListItem Text="profesor" Value="profesor"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCargo" runat="server" 
                                                ControlToValidate="ddlCargo" ErrorMessage="El cargo es obligatorio" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- Fechas -->
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Fecha de Nacimiento *</label>
                                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" 
                                                TextMode="Date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" 
                                                ControlToValidate="txtFechaNacimiento" ErrorMessage="La fecha de nacimiento es obligatoria" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Fecha de Ingreso *</label>
                                            <asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="form-control" 
                                                TextMode="Date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFechaIngreso" runat="server" 
                                                ControlToValidate="txtFechaIngreso" ErrorMessage="La fecha de ingreso es obligatoria" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- Nombre y Apellido -->
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Nombre *</label>
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Apellido *</label>
                                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server" 
                                                ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>

                                        <!-- Información de contacto -->
                                        <div class="col-12 mt-3">
                                            <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                                <i class="fas fa-address-card me-2"></i>Información de Contacto
                                            </h6>
                                        </div>
                                        
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Dirección</label>
                                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" 
                                                placeholder="Calle, número, piso, departamento"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 mb-3">
                                            <label class="form-label small fw-medium">Código Postal</label>
                                            <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 mb-3">
                                            <label class="form-label small fw-medium">Teléfono</label>
                                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <!-- Email y Contraseña -->
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Email *</label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                                ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                                ControlToValidate="txtEmail" ErrorMessage="Formato de email inválido" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-6 mb-3">
                                            <label class="form-label small fw-medium">Contraseña</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" 
                                                    TextMode="Password" placeholder="Dejar en blanco para no modificar"></asp:TextBox>
                                                <button type="button" class="btn btn-outline-secondary" onclick="togglePassword()">
                                                    <i class="fas fa-eye"></i>
                                                </button>
                                            </div>
                                            <small class="text-muted">Mínimo 6 caracteres. Dejar en blanco para mantener la contraseña actual.</small>
                                            <asp:CustomValidator ID="cvPassword" runat="server" 
                                                ControlToValidate="txtPassword"
                                                ClientValidationFunction="validatePassword"
                                                ErrorMessage="La contraseña debe tener al menos 6 caracteres"
                                                CssClass="text-danger small" Display="Dynamic"></asp:CustomValidator>
                                        </div>
                                    </div>
                                </div>

                                <!-- Botones de acción -->
                                <div class="d-grid gap-2 d-md-flex justify-content-center mt-4 pt-3 border-top">
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary px-4 py-2"
                                        OnClick="btnCancelar_Click" CausesValidation="false" />
                                    <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar Cambios" 
                                        CssClass="btn btn-success px-4 py-2" OnClick="btnGuardarCambios_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <!-- Mensajes -->
            <asp:Panel ID="pnlMensaje" runat="server" CssClass="alert alert-info mt-4 py-3" Visible="false" role="alert">
                <div class="d-flex align-items-center">
                    <i class="fas fa-info-circle me-3"></i>
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function seleccionarEmpleado(idPersonal) {
        __doPostBack('', 'SeleccionarEmpleado:' + idPersonal);
    }

    function togglePassword() {
        var txtPassword = document.getElementById('<%= txtPassword.ClientID %>');
        var icon = event.target.querySelector('i') || event.target;

        if (txtPassword.type === 'password') {
            txtPassword.type = 'text';
            icon.className = 'fas fa-eye-slash';
        } else {
            txtPassword.type = 'password';
            icon.className = 'fas fa-eye';
        }
    }

    function validatePassword(source, args) {
        var password = document.getElementById('<%= txtPassword.ClientID %>').value;

        // Si está vacío, es válido (no se quiere cambiar la contraseña)
        if (password === '') {
            args.IsValid = true;
            return;
        }

        // Si tiene contenido, debe tener al menos 6 caracteres
        args.IsValid = password.length >= 6;
    }

    // Manejar el postback desde JavaScript
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function (sender, args) {
        var postbackElement = args.get_postBackElement();
        if (postbackElement && postbackElement.id && postbackElement.id.indexOf('seleccionarEmpleado') !== -1) {
            // Mostrar loading si es necesario
        }
    });
</script>

<style>
    /* Estilos consistentes */
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
        background: linear-gradient(135deg, #198754 0%, #0d6a4a 100%);
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
    
    .card.border-primary {
        border: 2px solid #2c5aa0;
    }
    
    .form-control, .form-select {
        border: 1px solid #d1d5db;
        border-radius: 8px;
        padding: 0.75rem 1rem;
        font-size: 0.9rem;
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
    
    .btn-primary {
        background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
        font-size: 0.9rem;
        padding: 0.75rem 1.5rem;
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
        font-size: 0.9rem;
        padding: 0.75rem 1.5rem;
        color: #6b7280;
        transition: all 0.2s ease;
    }
    
    .btn-outline-secondary:hover {
        border-color: #3b82f6;
        background-color: #f8fafc;
        color: #3b82f6;
    }
    
    .btn-success {
        background: linear-gradient(135deg, #198754 0%, #0d6a4a 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
        font-size: 0.9rem;
        padding: 0.75rem 1.5rem;
        transition: all 0.2s ease;
    }
    
    .btn-success:hover {
        transform: translateY(-2px);
        box-shadow: 0 6px 20px rgba(25, 135, 84, 0.3);
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
        font-size: 0.9rem;
    }
    
    .alert-info {
        border-left-color: #3b82f6;
        background-color: #f0f7ff;
        color: #1e40af;
    }
</style>
<!-- Font Awesome Icons -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
</asp:Content>