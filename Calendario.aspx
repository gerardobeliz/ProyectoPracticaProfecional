<%@ Page Title="Calendario de Días Feriados" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="proyectoPracticaProfecional.Calendario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Agregar ScriptManager si no está en el Master Page -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container mt-4">
                <div class="row justify-content-center">
                    <div class="col-md-10 col-lg-8">
                        <div class="text-center mb-4">
                            <h2>Calendario de Días No Laborables</h2>
                        </div>
                        
                        <!-- Selector de Año -->
                        <div class="form-group text-center">
                            <label for="ddlAnio">Seleccionar Año:</label>
                            <asp:DropDownList ID="ddlAnio" runat="server" 
                                CssClass="form-control mx-auto" 
                                style="width: 150px;" 
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlAnio_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        
                        <!-- Calendario Centrado -->
                        <div class="calendar-container mt-4 d-flex justify-content-center">
                            <div class="calendar-wrapper">
                                <asp:Calendar ID="calFeriados" runat="server" 
                                    CssClass="calendar table table-bordered"
                                    OnDayRender="calFeriados_DayRender"
                                    ShowGridLines="true"
                                    Width="100%"
                                    DayNameFormat="Full"
                                    FirstDayOfWeek="Monday"
                                    SelectedDate="<%# DateTime.Today %>">
                                    <TitleStyle CssClass="calendar-title bg-primary text-white" />
                                    <DayHeaderStyle CssClass="calendar-day-header bg-light" />
                                    <TodayDayStyle CssClass="calendar-today bg-info text-white" />
                                    <SelectedDayStyle CssClass="calendar-selected bg-success text-white" />
                                    <WeekendDayStyle CssClass="calendar-weekend" />
                                    <OtherMonthDayStyle CssClass="calendar-other-month text-muted" />
                                </asp:Calendar>
                            </div>
                        </div>
                        
                        <!-- Leyenda Centrada -->
                        <div class="legend mt-4 text-center">
                            <h5>Leyenda:</h5>
                            <div class="d-flex justify-content-center flex-wrap">
                                <div class="legend-item mx-3 mb-2">
                                    <span class="legend-color bg-danger d-inline-block mr-2" style="width: 20px; height: 20px;"></span>
                                    <span>Día Feriado</span>
                                </div>
                                <div class="legend-item mx-3 mb-2">
                                    <span class="legend-color bg-warning d-inline-block mr-2" style="width: 20px; height: 20px;"></span>
                                    <span>Fin de Semana</span>
                                </div>
                                <div class="legend-item mx-3 mb-2">
                                    <span class="legend-color bg-success d-inline-block mr-2" style="width: 20px; height: 20px;"></span>
                                    <span>Día Laborable</span>
                                </div>
                                <div class="legend-item mx-3 mb-2">
                                    <span class="legend-color bg-info d-inline-block mr-2" style="width: 20px; height: 20px;"></span>
                                    <span>Hoy</span>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Resumen Centrado -->
                        <div class="summary mt-4 text-center">
                            <asp:Label ID="lblResumen" runat="server" CssClass="alert alert-info d-inline-block"></asp:Label>
                        </div>

                        <!-- Fecha Actual -->
                        <div class="current-date mt-3 text-center">
                            <asp:Label ID="lblFechaActual" runat="server" CssClass="badge badge-secondary p-2"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlAnio" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <style>
        .calendar-wrapper {
            max-width: 800px;
            margin: 0 auto;
        }
        .calendar {
            font-family: Arial, sans-serif;
            margin: 0 auto;
        }
        .calendar-title {
            font-weight: bold;
            text-align: center;
            padding: 15px;
            font-size: 1.2em;
        }
        .calendar-day-header {
            font-weight: bold;
            text-align: center;
            padding: 12px;
            background-color: #f8f9fa !important;
        }
        .calendar-weekend {
            background-color: #fff3cd;
            color: #856404;
        }
        .calendar-today {
            background-color: #17a2b8 !important;
            color: white !important;
            font-weight: bold;
            border: 2px solid #138496 !important;
        }
        .calendar-selected {
            background-color: #28a745 !important;
            color: white !important;
        }
        .calendar-other-month {
            background-color: #f8f9fa;
            color: #6c757d !important;
        }
        .calendar td {
            height: 85px;
            vertical-align: top;
            padding: 8px;
            position: relative;
            text-align: center;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        .calendar td:hover {
            transform: scale(1.05);
            z-index: 1;
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
        }
        .feriado {
            background-color: #f8d7da !important;
            color: #721c24 !important;
            font-weight: bold;
        }
        .no-laborable {
            background-color: #ffeaa7 !important;
            color: #2d3436 !important;
        }
        .legend-color {
            border: 1px solid #ddd;
            border-radius: 3px;
        }
        .current-date .badge {
            font-size: 1.1em;
            padding: 10px 20px;
        }
    </style>
</asp:Content>