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
                    width: 120px;
                    padding: 0.5rem;
                    border: 1px solid #e0e0e0;
                    border-radius: 6px;
                    text-align: center;
                    font-weight: 600;
                    cursor: pointer;
                    font-size: 0.9rem;
                    transition: all 0.2s;
                }

                .dropdown-asistencia:focus {
                    outline: 2px solid #3498db;
                    outline-offset: 2px;
                }

                /* Estilos para los diferentes estados */
                .estado-P { 
                    background-color: #d4edda; 
                    color: #155724; 
                    border-color: #c3e6cb; 
                }
                .estado-A { 
                    background-color: #f8d7da; 
                    color: #721c24; 
                    border-color: #f5c6cb; 
                }
                .estado-J { 
                    background-color: #fff3cd; 
                    color: #856404; 
                    border-color: #ffeaa7; 
                }
                .estado-default { 
                    background-color: #f8f9fa; 
                    color: #6c757d; 
                }

                .action-buttons {
                    display: flex;
                    gap: 1rem;
                    justify-content: center;
                    margin-bottom: 2rem;
                    flex-wrap: wrap;
                }

                .btn-guardar, .btn-cancelar, .btn-excel {
                    padding: 0.7rem 1.5rem;
                    border: none;
                    border-radius: 6px;
                    cursor: pointer;
                    font-weight: 500;
                    transition: all 0.2s;
                    min-width: 160px;
                }

                .btn-guardar {
                    background: #27ae60;
                    color: white;
                }

                .btn-guardar:hover {
                    background: #219653;
                    transform: translateY(-2px);
                }

                .btn-cancelar {
                    background: #e74c3c;
                    color: white;
                }

                .btn-cancelar:hover {
                    background: #c0392b;
                    transform: translateY(-2px);
                }

                .btn-excel {
                    background: #3498db;
                    color: white;
                }

                .btn-excel:hover {
                    background: #2980b9;
                    transform: translateY(-2px);
                }

                .resumen-container {
                    display: flex;
                    gap: 2rem;
                    justify-content: center;
                    flex-wrap: wrap;
                    background: #f8f9fa;
                    padding: 1.5rem;
                    border-radius: 12px;
                    margin-top: 1rem;
                }

                .resumen-item {
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    gap: 0.5rem;
                    padding: 1rem;
                    background: white;
                    border-radius: 8px;
                    min-width: 120px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
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
                    background: #f8f9fa;
                    border-radius: 12px;
                    margin: 1rem 0;
                }

                .mensaje-exito {
                    background-color: #d4edda;
                    color: #155724;
                    padding: 12px 20px;
                    border-radius: 8px;
                    margin: 15px 0;
                    text-align: center;
                    border: 1px solid #c3e6cb;
                    font-weight: 500;
                }

                .mensaje-error {
                    background-color: #f8d7da;
                    color: #721c24;
                    padding: 12px 20px;
                    border-radius: 8px;
                    margin: 15px 0;
                    text-align: center;
                    border: 1px solid #f5c6cb;
                    font-weight: 500;
                }

                .mensaje-info {
                    background-color: #cce7ff;
                    color: #004085;
                    padding: 12px 20px;
                    border-radius: 8px;
                    margin: 15px 0;
                    text-align: center;
                    border: 1px solid #b3d7ff;
                    font-weight: 500;
                }

                .fecha-seleccionada {
                    text-align: center;
                    margin-bottom: 1rem;
                    font-weight: 600;
                    color: #3498db;
                    font-size: 1.1rem;
                    background: #e3f2fd;
                    padding: 0.8rem;
                    border-radius: 8px;
                }

                .loading-overlay {
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    background: rgba(255,255,255,0.8);
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    z-index: 1000;
                    display: none;
                }

                .spinner {
                    border: 4px solid #f3f3f3;
                    border-top: 4px solid #3498db;
                    border-radius: 50%;
                    width: 50px;
                    height: 50px;
                    animation: spin 1s linear infinite;
                }

                @keyframes spin {
                    0% { transform: rotate(0deg); }
                    100% { transform: rotate(360deg); }
                }

                .status-indicator {
                    display: inline-block;
                    width: 12px;
                    height: 12px;
                    border-radius: 50%;
                    margin-right: 8px;
                }

                .status-presente { background-color: #27ae60; }
                .status-ausente { background-color: #e74c3c; }
                .status-justificado { background-color: #f39c12; }

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
                        width: 100px;
                    }
                    
                    .action-buttons {
                        flex-direction: column;
                        align-items: center;
                    }
                    
                    .btn-guardar, .btn-cancelar, .btn-excel {
                        width: 100%;
                        max-width: 300px;
                    }
                    
                    .resumen-container {
                        gap: 1rem;
                    }
                    
                    .resumen-item {
                        min-width: 100px;
                        padding: 0.8rem;
                    }
                }

                @media (max-width: 480px) {
                    .asistencia-container {
                        margin: 1rem auto;
                        padding: 0 0.5rem;
                    }
                    
                    .page-title {
                        font-size: 1.4rem;
                        margin-bottom: 1rem;
                    }
                    
                    .resumen-container {
                        flex-direction: column;
                        align-items: center;
                    }
                    
                    .resumen-item {
                        width: 100%;
                        max-width: 200px;
                    }
                }
            </style>

            <!-- Loading Overlay -->
            <div id="loadingOverlay" class="loading-overlay">
                <div class="spinner"></div>
            </div>

            <div class="asistencia-container">
                <h3 class="page-title">📊 Control de Asistencia Diaria</h3>
                
                <!-- Mensajes de estado -->
                <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="mensaje-exito">
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </asp:Panel>
                
                <!-- Filtros -->
                <div class="filtros-container">
                    <div class="filter-group">
                        <label>🏫 Curso:</label>
                        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-control" AutoPostBack="false">
                            <asp:ListItem Value="" Text="Seleccionar curso" />
                        </asp:DropDownList>
                    </div>
                    
                    <div class="filter-group">
                        <label>📅 Fecha:</label>
                        <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    
                    <asp:Button ID="btnCargar" runat="server" Text="📥 Cargar Asistencia" 
                        CssClass="btn-cargar" OnClick="btnCargar_Click" 
                        OnClientClick="mostrarLoading()" />
                </div>

                <!-- Fecha seleccionada -->
                <asp:Panel ID="pnlFechaSeleccionada" runat="server" CssClass="fecha-seleccionada" Visible="false">
                    📅 Asistencia para: <asp:Label ID="lblFechaSeleccionada" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <!-- Grid de asistencia -->
                <div class="grid-container">
                    <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty-state" Visible="true">
                        <div style="font-size: 3rem; margin-bottom: 1rem;">👆</div>
                        <h3 style="color: #6c757d; margin-bottom: 0.5rem;">No hay datos cargados</h3>
                        <p>Selecciona un curso y fecha para cargar la asistencia</p>
                    </asp:Panel>
                    
                    <asp:GridView ID="gvAsistencia" runat="server" CssClass="grid-table" AutoGenerateColumns="false" 
                        Visible="false" ShowHeader="true" GridLines="None"
                        OnRowDataBound="gvAsistencia_RowDataBound" OnDataBound="gvAsistencia_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="👤 Alumno" HeaderStyle-CssClass="header-alumno" ItemStyle-CssClass="student-name">
                                <ItemTemplate>
                                    <div class="student-name">
                                        <%# Eval("NombreCompleto") %>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="📊 Estado de Asistencia" HeaderStyle-CssClass="header-dia" ItemStyle-CssClass="dia-cell">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlEstadoAsistencia" runat="server" CssClass="dropdown-asistencia estado-default"
                                        onchange="aplicarEstiloDropdown(this)">
                                        <asp:ListItem Value="" Text="-- Seleccionar --" CssClass="estado-default" />
                                        <asp:ListItem Value="P" Text="✅ Presente" CssClass="estado-P" />
                                        <asp:ListItem Value="A" Text="❌ Ausente" CssClass="estado-A" />
                                        <asp:ListItem Value="J" Text="📝 Justificado" CssClass="estado-J" />
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfAlumnoId" runat="server" Value='<%# Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Botones de acción -->
                <asp:Panel ID="pnlBotonesAccion" runat="server" Visible="false">
                    <div class="action-buttons">
                        <asp:Button ID="btnGuardar" runat="server" Text="💾 Guardar Asistencia" 
                            CssClass="btn-guardar" OnClick="btnGuardar_Click" 
                            OnClientClick="return confirmGuardar()" />
                        <asp:Button ID="btnCancelar" runat="server" Text="❌ Cancelar" 
                            CssClass="btn-cancelar" OnClick="btnCancelar_Click" />
                        <asp:Button ID="btnExcel" runat="server" Text="📊 Exportar Excel" 
                            CssClass="btn-excel" OnClick="btnExcel_Click" />
                    </div>
                </asp:Panel>

                <!-- Resumen -->
                <asp:Panel ID="pnlResumen" runat="server" Visible="false">
                    <div class="resumen-container">
                        <div class="resumen-item">
                            <span class="resumen-label">Total Alumnos:</span>
                            <span class="resumen-value" id="totalAlumnos" runat="server">0</span>
                        </div>
                        <div class="resumen-item">
                            <span class="resumen-label">
                                <span class="status-indicator status-presente"></span>
                                Presentes:
                            </span>
                            <span class="resumen-value" id="presentCount" runat="server" style="color: #27ae60;">0</span>
                        </div>
                        <div class="resumen-item">
                            <span class="resumen-label">
                                <span class="status-indicator status-ausente"></span>
                                Ausentes:
                            </span>
                            <span class="resumen-value" id="absentCount" runat="server" style="color: #e74c3c;">0</span>
                        </div>
                        <div class="resumen-item">
                            <span class="resumen-label">
                                <span class="status-indicator status-justificado"></span>
                                Justificados:
                            </span>
                            <span class="resumen-value" id="justifiedCount" runat="server" style="color: #f39c12;">0</span>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <script type="text/javascript">
                function mostrarLoading() {
                    document.getElementById('loadingOverlay').style.display = 'flex';
                }

                function ocultarLoading() {
                    document.getElementById('loadingOverlay').style.display = 'none';
                }

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
                    var totalAlumnos = document.getElementById('<%= totalAlumnos.ClientID %>');
                    var presentCount = document.getElementById('<%= presentCount.ClientID %>');
                    var absentCount = document.getElementById('<%= absentCount.ClientID %>');
                    var justifiedCount = document.getElementById('<%= justifiedCount.ClientID %>');

                    if (totalAlumnos) totalAlumnos.innerText = dropdowns.length;
                    if (presentCount) presentCount.innerText = presentes;
                    if (absentCount) absentCount.innerText = ausentes;
                    if (justifiedCount) justifiedCount.innerText = justificados;
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
                    ocultarLoading();
                }

                function confirmGuardar() {
                    var dropdowns = document.querySelectorAll('.dropdown-asistencia');
                    var seleccionados = 0;

                    dropdowns.forEach(function (dropdown) {
                        if (dropdown.value !== '') seleccionados++;
                    });

                    if (seleccionados === 0) {
                        return confirm('⚠️ No has seleccionado ningún estado de asistencia. ¿Deseas guardar de todas formas?');
                    }

                    mostrarLoading();
                    return confirm('¿Estás seguro de que deseas guardar la asistencia?');
                }

                // Inicializar cuando el DOM esté listo
                document.addEventListener('DOMContentLoaded', function () {
                    // Ocultar loading si está visible
                    ocultarLoading();

                    // Establecer fecha actual si está vacía
                    var txtFecha = document.getElementById('<%= txtFecha.ClientID %>');
                    if (txtFecha && !txtFecha.value) {
                        var today = new Date();
                        var dd = String(today.getDate()).padStart(2, '0');
                        var mm = String(today.getMonth() + 1).padStart(2, '0');
                        var yyyy = today.getFullYear();
                        txtFecha.value = yyyy + '-' + mm + '-' + dd;
                    }
                });

                // Para postbacks de ASP.NET
                if (typeof (Sys) !== 'undefined') {
                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
                        mostrarLoading();
                    });

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

                // Inicializar después de carga completa
                window.addEventListener('load', function () {
                    setTimeout(function () {
                        inicializarDropdowns();
                    }, 100);
                });
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>