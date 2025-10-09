<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Asistencia.aspx.cs" Inherits="proyectoPracticaProfecional.Asistencia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            min-width: 200px;
        }

        .header-dia {
            font-size: 0.8rem;
            min-width: 80px;
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
            width: 70px;
            padding: 0.3rem;
            border: 1px solid #e0e0e0;
            border-radius: 4px;
            text-align: center;
            font-weight: 600;
            cursor: pointer;
        }

        .dropdown-asistencia option {
            font-weight: 600;
            text-align: center;
        }

        /* Estilos para los diferentes estados */
        .estado-P { background-color: #d4edda; color: #155724; }
        .estado-A { background-color: #f8d7da; color: #721c24; }
        .estado-J { background-color: #fff3cd; color: #856404; }
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

        @media (max-width: 768px) {
            .filtros-container {
                flex-direction: column;
                align-items: stretch;
            }
            
            .grid-container {
                font-size: 0.8rem;
            }
            
            .header-dia {
                min-width: 60px;
            }
        }
    </style>

    <div class="asistencia-container">
        <h3 class="page-title">📊 Control de Asistencia Mensual</h3>
        
        <!-- Filtros -->
        <div class="filtros-container">
            <div class="filter-group">
                <label>Curso:</label>
                <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-control">
                    <asp:ListItem Value="" Text="Seleccionar curso" />
                 
                </asp:DropDownList>
            </div>
            
            <div class="filter-group">
                <label>Fecha:</label>
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>
            
            <asp:Button ID="btnCargar" runat="server" Text="📥 Cargar Asistencia" 
                CssClass="btn-cargar" OnClick="btnCargar_Click" />
        </div>

        <!-- Grid de asistencia -->
        <div class="grid-container">
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty-state" Visible="true">
                👆 Selecciona un curso y fecha para cargar la asistencia
            </asp:Panel>
            
            <asp:Table ID="tblAsistencia" runat="server" CssClass="grid-table" Visible="false">
            </asp:Table>
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

        function prevenirAutoFormat() {
            var dropdowns = document.querySelectorAll('.dropdown-asistencia');
            dropdowns.forEach(function (dropdown) {
                // Prevenir que Web Forms modifique el texto
                for (var i = 0; i < dropdown.options.length; i++) {
                    var option = dropdown.options[i];
                    if (option.value === 'P' && option.text !== 'P') {
                        option.text = 'P';
                    } else if (option.value === 'A' && option.text !== 'A') {
                        option.text = 'A';
                    } else if (option.value === 'J' && option.text !== 'J') {
                        option.text = 'J';
                    }
                }

                // Observar cambios en el dropdown
                dropdown.addEventListener('change', function () {
                    setTimeout(function () {
                        if (dropdown.value === 'P' && dropdown.options[dropdown.selectedIndex].text !== 'P') {
                            dropdown.options[dropdown.selectedIndex].text = 'P';
                        } else if (dropdown.value === 'A' && dropdown.options[dropdown.selectedIndex].text !== 'A') {
                            dropdown.options[dropdown.selectedIndex].text = 'A';
                        } else if (dropdown.value === 'J' && dropdown.options[dropdown.selectedIndex].text !== 'J') {
                            dropdown.options[dropdown.selectedIndex].text = 'J';
                        }
                        aplicarEstiloDropdown(dropdown);
                    }, 10);
                });

                aplicarEstiloDropdown(dropdown);
            });
        }

        function actualizarResumen() {
            var presentes = 0;
            var ausentes = 0;
            var justificados = 0;

            var dropdowns = document.querySelectorAll('.dropdown-asistencia');
            dropdowns.forEach(function (dropdown) {
                if (dropdown.value === 'P') presentes++;
                else if (dropdown.value === 'A') ausentes++;
                else if (dropdown.value === 'J') justificados++;
            });

            document.getElementById('presentCount').innerText = presentes;
            document.getElementById('absentCount').innerText = ausentes;
            document.getElementById('justifiedCount').innerText = justificados;
        }

        // Solución definitiva: modificar el HTML entities en el code-behind
        function forzarTextoDefinitivo() {
            var dropdowns = document.querySelectorAll('.dropdown-asistencia');
            dropdowns.forEach(function (dropdown) {
                for (var i = 0; i < dropdown.options.length; i++) {
                    var option = dropdown.options[i];
                    // Reemplazar cualquier texto por el correcto
                    if (option.value === 'P') {
                        option.text = 'P';
                        option.innerHTML = 'P'; // Forzar HTML interno también
                    } else if (option.value === 'A') {
                        option.text = 'A';
                        option.innerHTML = 'A';
                    } else if (option.value === 'J') {
                        option.text = 'J';
                        option.innerHTML = 'J';
                    }
                }
                aplicarEstiloDropdown(dropdown);
            });
        }

        // Inicializar
        document.addEventListener('DOMContentLoaded', function () {
            forzarTextoDefinitivo();
            actualizarResumen();

            // Monitorear cambios continuamente
            setInterval(forzarTextoDefinitivo, 100);
        });

        // Para postbacks de UpdatePanel
        if (typeof (Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                forzarTextoDefinitivo();
                actualizarResumen();
            });
        }
</script>
</asp:Content>