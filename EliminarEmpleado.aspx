<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="EliminarEmpleado.aspx.cs" Inherits="proyectoPracticaProfecional.ModificarEmpleado" %>
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
                    <i class="fas fa-user-times"></i>
                </div>
                <h2 class="fw-light text-dark mb-2">
                    Eliminar Empleado</h2>
                <p class="text-muted">
                    Seleccione y elimine un empleado del sistema</p>
            </div>
            
            <!-- Contenedor Principal -->
            <div class="row justify-content-center">
                <!-- Columna de Búsqueda -->
                <div class="col-lg-10 mb-4">
                    <div class="card border-0 shadow-sm rounded-3">
                        <div class="card-body p-4">
                            <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue text-center">
                                <i class="fas fa-search me-2"></i>Buscar Empleado a Eliminar
                            </h6>
                            <div class="row align-items-end justify-content-center">
                                <div class="col-md-8 mb-3">
                                    <label for="txtBuscarEmpleado" class="form-label small fw-medium text-center w-100">
                                        Buscar por nombre, apellido o DNI</label>
                                    <asp:TextBox ID="txtBuscarEmpleado" runat="server" CssClass="form-control text-center" 
                                        placeholder="Ingrese nombre, apellido o DNI"></asp:TextBox>
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
                                    <asp:GridView ID="gvEmpleados" runat="server" CssClass="table table-hover table-sm"
                                        AutoGenerateColumns="False" DataKeyNames="ID_PERSONAL" OnSelectedIndexChanged="gvEmpleados_SelectedIndexChanged"
                                        GridLines="None" BorderStyle="None">
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seleccionar" 
                                                ControlStyle-CssClass="btn btn-outline-primary btn-sm">
                                                <ControlStyle CssClass="btn btn-outline-primary btn-sm" />
                                            </asp:CommandField>
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

                <!-- Panel de Confirmación -->
                <div class="col-lg-6 mb-4">
                    <asp:Panel ID="pnlConfirmacion" runat="server" Visible="false">
                        <div class="card border-0 shadow-sm rounded-3 border-danger">
                            <div class="card-body p-4">
                                <div class="text-center mb-4">
                                    <i class="fas fa-exclamation-triangle text-danger fa-3x mb-3"></i>
                                    <h5 class="text-danger fw-bold">Confirmar Eliminación</h5>
                                </div>
                                
                                <!-- Información del empleado a eliminar -->
                                <div class="alert alert-warning mb-4">
                                    <div class="d-flex align-items-center">
                                        <i class="fas fa-exclamation-circle me-2"></i>
                                        <div>
                                            <strong>¿Está seguro que desea eliminar al siguiente empleado?</strong>
                                            <div class="mt-2">
                                                <asp:Label ID="lblEmpleadoAEliminar" runat="server" CssClass="fw-bold"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Detalles del empleado -->
                                <div class="mb-4 p-3 bg-light rounded">
                                    <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                                        <i class="fas fa-info-circle me-2"></i>Detalles del Empleado
                                    </h6>
                                    <div class="row small">
                                        <div class="col-6">
                                            <strong>ID:</strong>
                                            <asp:Label ID="lblLegajo" runat="server" CssClass="ms-1"></asp:Label>
                                        </div>
                                        <div class="col-6">
                                            <strong>DNI:</strong>
                                            <asp:Label ID="lblDocumento" runat="server" CssClass="ms-1"></asp:Label>
                                        </div>
                                        <div class="col-6 mt-2">
                                            <strong>Tipo:</strong>
                                            <asp:Label ID="lblCargo" runat="server" CssClass="ms-1"></asp:Label>
                                        </div>
                                        <div class="col-6 mt-2">
                                            <strong>Estado:</strong>
                                            <asp:Label ID="lblEstado" runat="server" CssClass="ms-1"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <!-- Advertencia -->
                                <div class="alert alert-danger">
                                    <div class="d-flex align-items-start">
                                        <i class="fas fa-radiation-alt me-2 mt-1"></i>
                                        <div>
                                            <strong>Advertencia:</strong> Esta acción no se puede deshacer. 
                                            Todos los datos del empleado serán eliminados permanentemente del sistema.
                                        </div>
                                    </div>
                                </div>

                                <!-- Botones de acción -->
                                <div class="d-grid gap-2 d-md-flex justify-content-center mt-4 pt-3 border-top">
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary px-4 py-2"
                                        OnClick="btnCancelar_Click" CausesValidation="false" />
                                    <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Confirmar Eliminación" 
                                        CssClass="btn btn-danger px-4 py-2"
                                        OnClick="btnConfirmarEliminar_Click" 
                                        OnClientClick="return confirm('¿ESTÁ ABSOLUTAMENTE SEGURO? Esta acción es irreversible.');" />
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
    </ContentTemplate>
</asp:UpdatePanel>
<style>
    /* Estilos consistentes */
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
        background: linear-gradient(135deg, #dc3545 0%, #c53030 100%);
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
    
    .card.border-danger
    {
        border: 2px solid #dc3545;
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
    
    .section-title
    {
        color: #2c5aa0;
        letter-spacing: 0.5px;
        font-size: 0.9rem;
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
    
    .alert-warning
    {
        border-left-color: #f59e0b;
        background-color: #fffbeb;
        color: #92400e;
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