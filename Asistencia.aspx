<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Asistencia.aspx.cs" Inherits="proyectoPracticaProfecional.Asistencia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <style>
                .asistencia-container {
                    max-width: 95%;
                    margin: 2rem auto;
                    padding: 0 1rem;
                    font-family: 'Segoe UI', system-ui, sans-serif;
                }

                .page-title {
                    text-align: center;
                    color: #2c3e50;
                    margin-bottom: 2rem;
                    font-weight: 300;
                    font-size: 1.8rem;
                }

                .filtros-container {
                    display: flex;
                    gap: 1rem;
                    align-items: end;
                    margin-bottom: 2rem;
                    flex-wrap: wrap;
                }

                .filter-group {
                    display: flex;
                    flex-direction: column;
                    gap: 0.5rem;
                }

                .filter-group label {
                    font-weight: 600;
                    color: #495057;
                    font-size: 0.9rem;
                }

                .form-control {
                    padding: 0.5rem;
                    border: 1px solid #e0e0e0;
                    border-radius: 6px;
                    font-size: 0.9rem;
                }

                .btn-cargar {
                    padding: 0.5rem 1rem;
                    background: #3498db;
                    color: white;
                    border: none;
                    border-radius: 6px;
                    cursor: pointer;
                    transition: background 0.2s;
                }

                .btn-cargar:hover {
                    background: #2980b9;
                }

                .grid-container {
                    border: 1px solid #e0e0e0;
                    border-radius: 12px;
                    overflow-x: auto;
                    background: #fff;
                    box-shadow: 0 2px 8px rgba(0,0,0,0.08);
                    margin-bottom: 2rem;
                }

                .grid-table {
                    width: 100%;
                    border-collapse: collapse;
                }

                .grid-header {
                    background: #f8f9fa;
                    border-bottom: 2px solid #e0e0e0;
                }

                .grid-header th {
                    padding: 1rem;
                    font-weight: 600;
                    color: #495057;
                    text-align: center;
                    border-right: 1px solid #e0e0e0;
                }

                .header-alumno {
                    text-align: left !important;
                    min-width: 250px;
                }

                .header-dia {
                    font-size: 0.9rem;
                    min-width: 150px;
                    background: #e3f2fd;
                }

                .grid-row td {
                    padding: 0.8rem;
                    border-bottom: 1px solid #f0f0f0;
                    border-right: 1px solid #f0f0f0;
                }

                .grid-row:hover {
                    background: #f8f9fa;
                }

                .student-name {
                    font-weight: 500;
                    color: #2c3e50;
                    text-align: left;
                }

                .dia-cell {
                    text-align: center;
                    vertical-align: middle;
                }

                .dropdown-asistencia {
                    width: 100px;
                    padding: 0.5rem;
                    border: 1px solid #e0e0e0;
                    border-radius: 6px;
                    text-align: center;
                    font-weight: 600;
                    cursor: pointer;
                    font-size: 0.9rem;
                }

                .dropdown-asistencia:focus {
                    outline: 2px solid #3498db;
                    outline-offset: 2px;
                }

                /* Estilos para los diferentes estados */
                .estado-P { background-color: #d4edda; color: #155724; border-color: #c3e6cb; }
                .estado-A { background-color: #f8d7da; color: #721c24; border-color: #f5c6cb; }
                .estado-J { background-color: #fff3cd; color: #856404; border-color: #ffeaa7; }
                .estado-default { background-color: #f8f9fa; color: #6c757d; }

                .action-buttons {
                    display: flex;
                    gap: 1rem;
                    justify-content: center;
                    margin-bottom: 2rem;
                }

                .btn-guardar, .btn-cancelar, .btn-excel {
                    padding: 0.7rem 1.5rem;
                    border: none;
                    border-radius: 6px;
                    cursor: pointer;
                    font-weight: 500;
                    transition: all 0.2s;
                }

                .btn-guardar {
                    background: #27ae60;
                    color: white;
                }

                .btn-guardar:hover {
                    background: #219653;
                }

                .btn-cancelar {
                    background: #e74c3c;
                    color: white;
                }

                .btn-cancelar:hover {
                    background: #c0392b;
                }

                .btn-excel {
                    background: #3498db;
                    color: white;
                }

                .btn-excel:hover {
                    background: #2980b9;
                }

                .resumen-container {
                    display: flex;
                    gap: 2rem;
                    justify-content: center;
                    flex-wrap: wrap;
                }

                .resumen-item {
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    gap: 0.5rem;
                }

                .resumen-label {
                    font-size: 0.9rem;
                    font-weight: 600;
                }

                .resumen-value {
                    font-size: 1.5rem;
                    font-weight: 700;
                }

                .empty-state {
                    text-align: center;
                    padding: 3rem;
                    color: #6c757d;
                    font-style: italic;
                }

                .mensaje-exito {
                    background-color: #d4edda;
                    color: #155724;
                    padding: 10px;
                    border-radius: 4px;
                    margin: 10px 0;
                    text-align: center;
                }

                .mensaje-error {
                    background-color: #f8d7da;
                    color: #721c24;
                    padding: 10px;
                    border-radius: 4px;
                    margin: 10px 0;
                    text-align: center;
                }

                .fecha-seleccionada {
                    text-align: center;
                    margin-bottom: 1rem;
                    font-weight: 600;
                    color: #3498db;
                    font-size: 1.1rem;
                }

                @media (max-width: 768px) {
                    .filtros-container {
                        flex-direction: column;
                        align-items: stretch;
                    }
                    
                    .grid-container {
                        font-size: 0.8rem;
                    }
                    
                    .header-dia {
                        min-width: 120px;
                    }
                    
                    .dropdown-asistencia {
                        width: 80px;
                    }
                }
            </style>

            <div class="asistencia-container">
                <h3 class="page-title">📊 Control de Asistencia Diaria</h3>
                
                <!-- Mensajes de estado -->
                <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="mensaje-exito">
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </asp:Panel>
                
                <!-- Filtros -->
                <div class="filtros-container">
                    <div class="filter-group">
                        <label>Curso:</label>
                        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-control" AutoPostBack="false">
                            <asp:ListItem Value="" Text="Seleccionar curso" />
                            <asp:ListItem Value="programacion" Text="Programación" />
                            <asp:ListItem Value="Biologia" Text="Biología" />
                            <asp:ListItem Value="Historia" Text="Historia" />
                            <asp:ListItem Value="Psicopedagogia" Text="Psicopedagogía" />
                        </asp:DropDownList>
                    </div>
                    
                    <div class="filter-group">
                        <label>Fecha:</label>
                        <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    
                    <asp:Button ID="btnCargar" runat="server" Text="📥 Cargar Asistencia" 
                        CssClass="btn-cargar" OnClick="btnCargar_Click" />
                </div>

                <!-- Fecha seleccionada -->
                <asp:Panel ID="pnlFechaSeleccionada" runat="server" CssClass="fecha-seleccionada" Visible="false">
                    📅 Asistencia para: <asp:Label ID="lblFechaSeleccionada" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <!-- Grid de asistencia -->
                <div class="grid-container">
                    <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty-state" Visible="true">
                        👆 Selecciona un curso y fecha para cargar la asistencia
                    </asp:Panel>
                    
                    <asp:GridView ID="gvAsistencia" runat="server" CssClass="grid-table" AutoGenerateColumns="false" 
                        Visible="false" ShowHeader="true" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Alumno" HeaderStyle-CssClass="header-alumno">
                                <ItemTemplate>
                                    <div class="student-name">
                                        <%# Eval("NombreCompleto") %>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Asistencia" HeaderStyle-CssClass="header-dia">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlEstadoAsistencia" runat="server" CssClass="dropdown-asistencia"
                                        onchange="aplicarEstiloDropdown(this)">
                                        <asp:ListItem Value="" Text="Seleccionar" CssClass="estado-default" />
                                        <asp:ListItem Value="P" Text="Presente" CssClass="estado-P" />
                                        <asp:ListItem Value="A" Text="Ausente" CssClass="estado-A" />
                                        <asp:ListItem Value="J" Text="Justificado" CssClass="estado-J" />
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfAlumnoId" runat="server" Value='<%# Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Botones de acción -->
                <div class="action-buttons">
                    <asp:Button ID="btnGuardar" runat="server" Text="💾 Guardar Asistencia" 
                        CssClass="btn-guardar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="❌ Cancelar" 
                        CssClass="btn-cancelar" OnClick="btnCancelar_Click" />
                    <asp:Button ID="btnExcel" runat="server" Text="📊 Exportar Excel" 
                        CssClass="btn-excel" OnClick="btnExcel_Click" />
                </div>

                <!-- Resumen -->
                <div class="resumen-container">
                    <div class="resumen-item">
                        <span class="resumen-label">Total Alumnos:</span>
                        <span class="resumen-value" id="totalAlumnos" runat="server">0</span>
                    </div>
                    <div class="resumen-item">
                        <span class="resumen-label" style="color: #155724;">Presentes:</span>
                        <span class="resumen-value" id="presentCount" runat="server">0</span>
                    </div>
                    <div class="resumen-item">
                        <span class="resumen-label" style="color: #721c24;">Ausentes:</span>
                        <span class="resumen-value" id="absentCount" runat="server">0</span>
                    </div>
                    <div class="resumen-item">
                        <span class="resumen-label" style="color: #856404;">Justificados:</span>
                        <span class="resumen-value" id="justifiedCount" runat="server">0</span>
                    </div>
                </div>
            </div>

            <script type="text/javascript">
                function aplicarEstiloDropdown(dropdown) {
                    // Remover todas las clases de estado
                    dropdown.className = 'dropdown-asistencia';

                    // Agregar clase según el valor seleccionado
                    if (dropdown.value === 'P') {
                        dropdown.classList.add('estado-P');
                    } else if (dropdown.value === 'A') {
                        dropdown.classList.add('estado-A');
                    } else if (dropdown.value === 'J') {
                        dropdown.classList.add('estado-J');
                    } else {
                        dropdown.classList.add('estado-default');
                    }

                    actualizarResumen();
                }

                function actualizarResumen() {
                    var presentes = 0;
                    var ausentes = 0;
                    var justificados = 0;
                    var sinSeleccionar = 0;

                    var dropdowns = document.querySelectorAll('.dropdown-asistencia');
                    dropdowns.forEach(function (dropdown) {
                        if (dropdown.value === 'P') presentes++;
                        else if (dropdown.value === 'A') ausentes++;
                        else if (dropdown.value === 'J') justificados++;
                        else sinSeleccionar++;
                    });

                    // Actualizar contadores
                    if (document.getElementById('<%= presentCount.ClientID %>')) {
                        document.getElementById('<%= presentCount.ClientID %>').innerText = presentes;
                    }
                    if (document.getElementById('<%= absentCount.ClientID %>')) {
                        document.getElementById('<%= absentCount.ClientID %>').innerText = ausentes;
                    }
                    if (document.getElementById('<%= justifiedCount.ClientID %>')) {
                        document.getElementById('<%= justifiedCount.ClientID %>').innerText = justificados;
                    }
                    if (document.getElementById('<%= totalAlumnos.ClientID %>')) {
                        document.getElementById('<%= totalAlumnos.ClientID %>').innerText = dropdowns.length;
                    }
                }

                function inicializarDropdowns() {
                    var dropdowns = document.querySelectorAll('.dropdown-asistencia');
                    dropdowns.forEach(function (dropdown) {
                        // Aplicar estilo inicial basado en el valor actual
                        aplicarEstiloDropdown(dropdown);

                        // Configurar evento change
                        dropdown.addEventListener('change', function () {
                            aplicarEstiloDropdown(this);
                            actualizarResumen();
                        });
                    });

                    actualizarResumen();
                }

                // Inicializar cuando el DOM esté listo
                document.addEventListener('DOMContentLoaded', function () {
                    inicializarDropdowns();
                });

                // Para postbacks de ASP.NET
                if (typeof (Sys) !== 'undefined') {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        inicializarDropdowns();
                    });
                }

                // Función para formatear fecha
                function formatearFecha(fechaStr) {
                    if (!fechaStr) return '';
                    var fecha = new Date(fechaStr);
                    return fecha.toLocaleDateString('es-ES', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    });
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>