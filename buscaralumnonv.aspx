<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="BuscarAlumnoNV.aspx.cs" Inherits="proyectoPracticaProfecional.BuscarAlumnoNV" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

<div class="container-fluid min-vh-100 py-5">
    <div class="row w-100 justify-content-center">
        <div class="col-xl-10 col-lg-12 col-md-12">
            <!-- Header -->
            <div class="text-center mb-5">
                <div class="header-icon mb-3">
                    <i class="fas fa-search"></i>
                </div>
                <h2 class="fw-light text-dark mb-2">Buscar Alumnos</h2>
                <p class="text-muted">Busque alumnos en su curso asignado</p>
                
                <!-- Información del curso -->
                <asp:Label ID="lblCursoActual" runat="server" CssClass="badge bg-primary fs-6 mt-2" Text=""></asp:Label>
            </div>
            
            <!-- Panel de Búsqueda -->
            <div class="row justify-content-center mb-4">
                <div class="col-lg-12">
                    <div class="card border-0 shadow-sm rounded-3">
                        <div class="card-body p-4">
                            <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue text-center">
                                <i class="fas fa-search me-2"></i>Buscar Alumnos
                            </h6>
                            <div class="row align-items-end justify-content-center">
                                <div class="col-md-8 mb-3">
                                    <label for="txtBuscarAlumno" class="form-label small fw-medium text-center w-100">
                                        Buscar por nombre, apellido, DNI o legajo</label>
                                    <asp:TextBox ID="txtBuscarAlumno" runat="server" CssClass="form-control text-center" 
                                        placeholder="Ingrese nombre, apellido, DNI o legajo"></asp:TextBox>
                                </div>
                                <div class="col-md-4 mb-3">
                                    <div class="d-flex justify-content-center gap-2">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                            CssClass="btn btn-primary px-4"
                                            OnClick="btnBuscar_Click" />
                                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" 
                                            CssClass="btn btn-outline-secondary px-4"
                                            OnClick="btnLimpiar_Click" />
                                    </div>
                                </div>
                            </div>

                            <!-- Contador de resultados -->
                            <asp:Label ID="lblResultados" runat="server" CssClass="small text-muted text-center d-block mt-2" 
                                Visible="false" Text=""></asp:Label>

                            <!-- Grid de resultados -->
                            <asp:Panel ID="pnlResultados" runat="server" Visible="false" CssClass="mt-4">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvAlumnos" runat="server" CssClass="table table-hover table-sm"
                                        AutoGenerateColumns="False" DataKeyNames="LEGAJO" 
                                        OnSelectedIndexChanged="gvAlumnos_SelectedIndexChanged"
                                        GridLines="None" BorderStyle="None">
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Seleccionar" 
                                                ControlStyle-CssClass="btn btn-outline-primary btn-sm">
                                                <ControlStyle CssClass="btn btn-outline-primary btn-sm" />
                                            </asp:CommandField>
                                            <asp:BoundField DataField="LEGAJO" HeaderText="Legajo" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="DNI" HeaderText="DNI" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="APELLIDO" HeaderText="Apellido" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="EMAIL" HeaderText="Email" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="TELEFONO" HeaderText="Teléfono" 
                                                HeaderStyle-CssClass="small fw-bold" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="CARRERA" HeaderText="Carrera" 
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
            </div>

            <!-- Alumno seleccionado -->
            <asp:Label ID="lblAlumnoSeleccionado" runat="server" CssClass="h5 text-primary mb-3 d-block" Text=""></asp:Label>

            <!-- Panel de Notas -->
            <asp:Panel ID="pnlNotas" runat="server" Visible="false" CssClass="mb-4">
                <div class="card border-0 shadow-sm rounded-3">
                    <div class="card-body p-4">
                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                            <i class="fas fa-edit me-2"></i>Notas del Alumno
                        </h6>
                        <div class="table-responsive">
                            <asp:GridView ID="gvNotas" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Parcial 1">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtParcial1" runat="server" Text='<%# Bind("PARCIAL1") %>' 
                                                CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Rec. Parcial 1">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRecParcial1" runat="server" Text='<%# Bind("REC_PARCIAL1") %>' 
                                                CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Parcial 2">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtParcial2" runat="server" Text='<%# Bind("PARCIAL2") %>' 
                                                CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Rec. Parcial 2">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRecParcial2" runat="server" Text='<%# Bind("REC_PARCIAL2") %>' 
                                                CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Final">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFinal" runat="server" Text='<%# Bind("FINAL") %>' 
                                                CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="table-primary" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="align-middle" />
                            </asp:GridView>
                        </div>
                        <div class="text-center mt-3">
                            <asp:Button ID="btnGuardarNotas" runat="server" Text="Guardar Notas" 
                                CssClass="btn btn-success px-4" OnClick="btnGuardarNotas_Click" />
                            <asp:Button ID="btnVerAsistencias" runat="server" Text="Ver Asistencias" 
                                CssClass="btn btn-info px-4 ms-2" OnClick="btnVerAsistencias_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Panel de Asistencias (oculto inicialmente) -->
            <asp:Panel ID="pnlAsistencias" runat="server" Visible="false" CssClass="mb-4">
                <div class="card border-0 shadow-sm rounded-3">
                    <div class="card-body p-4">
                        <h6 class="section-title mb-3 text-uppercase small fw-bold text-blue">
                            <i class="fas fa-calendar-alt me-2"></i>Asistencias del Alumno
                        </h6>
                        <div class="row mb-3">
                            <div class="col-md-3">
                                <label class="form-label small">Mes:</label>
                                <asp:DropDownList ID="ddlMes" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label small">Año:</label>
                                <asp:DropDownList ID="ddlAño" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="table-responsive">
                            <asp:GridView ID="gvAsistencias" runat="server" 
                                AutoGenerateColumns="True"
                                OnRowDataBound="gvAsistencias_RowDataBound"
                                CssClass="table table-bordered table-sm tabla-asistencias" 
                                GridLines="Horizontal" ShowHeader="True">
                                <HeaderStyle CssClass="table-primary" HorizontalAlign="Center" />
                                <RowStyle CssClass="align-middle text-center" HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                        
                        <div class="mt-3">
                            <small class="text-muted">
                                <strong>Leyenda:</strong> 
                                <span class="badge bg-success">P = Presente</span>
                                <span class="badge bg-danger">A = Ausente</span>
                                <span class="badge bg-warning">J = Justificado</span>
                            </small>
                        </div>
                    </div>
                </div>
            </asp:Panel>

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
    body {
        background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
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
    
    .form-control {
        border: 1px solid #d1d5db;
        border-radius: 8px;
        padding: 0.75rem 1rem;
        font-size: 0.9rem;
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
        padding: 0.75rem 1.5rem;
    }
    
    .btn-success {
        background: linear-gradient(135deg, #10b981 0%, #059669 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
        padding: 0.75rem 1.5rem;
    }
    
    .btn-info {
        background: linear-gradient(135deg, #0ea5e9 0%, #0284c7 100%);
        border: none;
        border-radius: 8px;
        font-weight: 500;
        padding: 0.75rem 1.5rem;
    }
    
    .btn-outline-secondary {
        border: 2px solid #d1d5db;
        border-radius: 8px;
        font-weight: 500;
        padding: 0.75rem 1.5rem;
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
    }
    
    /* Estilos para la tabla de asistencias */
    .tabla-asistencias {
        border-collapse: collapse;
        width: 100%;
        margin-top: 15px;
        font-size: 0.8rem;
    }

    .tabla-asistencias th, 
    .tabla-asistencias td {
        border: 1px solid #dee2e6;
        padding: 6px 4px;
        text-align: center;
        min-width: 40px;
    }

    .tabla-asistencias th {
        background: linear-gradient(135deg, #2c5aa0 0%, #3b82f6 100%);
        color: white;
        font-weight: bold;
        font-size: 0.75rem;
    }

    .tabla-asistencias td:first-child {
        background-color: #f8f9fa;
        font-weight: bold;
        text-align: left;
        padding: 6px 12px;
    }

    /* Estilos para los estados de asistencia */
    .presente { 
        background-color: #d4edda !important; 
        color: #155724 !important; 
        font-weight: bold;
    }

    .ausente { 
     background-color: #f8d7da !important; 
        color: #721c24 !important; 
        font-weight: bold;
    }

    .justificado { 
        background-color: #fff3cd !important; 
        color: #856404 !important; 
        font-weight: bold;
    }

    /* Badges para la leyenda */
    .badge {
        font-size: 0.7rem;
        margin: 0 3px;
        padding: 3px 6px;
    }

    .bg-success { background-color: #28a745 !important; }
    .bg-danger { background-color: #dc3545 !important; }
    .bg-warning { background-color: #ffc107 !important; color: #212529 !important; }
</style>

<!-- Font Awesome Icons -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
</asp:Content>