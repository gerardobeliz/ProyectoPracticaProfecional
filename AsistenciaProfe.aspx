
<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="AsistenciaProfe.aspx.cs"
    Inherits="proyectoPracticaProfecional.AsistenciaProfe" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style>
/* ✅ Contenedor del registro de asistencia (arriba) — dos columnas */
.contenedor-asistencia {
    margin: 20px auto;
    max-width: 1200px;
    font-family: Arial, Helvetica, sans-serif;
    background: #fff;
    padding: 25px;
    border-radius: 8px;
    box-shadow: 0px 0px 8px rgba(0,0,0,0.15);
    margin-left: 60px;

    display: flex; /* ✅ lado a lado */
    gap: 25px;
}

/* ✅ La tabla ocupa la mayor parte del ancho */
.tabla-asistencia-container {
    flex: 1;
}

/* ✅ Contenedor de histórico + estadísticas en columnas */
.contenedor-historico {
    margin: 25px auto;
    font-family: Arial, Helvetica, sans-serif;
    background: #eef0ff;
    padding: 20px;
    border-radius: 8px;
    margin-left: 60px;

    display: flex;
    align-items: flex-start;
    gap: 25px;
}

/* 📌 Contenedor de la grilla histórica */
.bloque-historico {
    flex: 1;
    overflow-x: auto;
    padding-bottom: 10px;
}

/* ✅ Tablas */
.tabla-asistencia, .tabla-historico {
    border-collapse: collapse;
    margin-top: 15px;
    font-size: 15px;
}

.tabla-asistencia th, .tabla-historico th {
    background: #494ca2;
    color: white;
    padding: 10px;
    text-align: center;
}

.tabla-asistencia td, .tabla-historico td {
    padding: 8px;
    border-bottom: 1px solid #e5e5e5;
    text-align: center;
    font-weight: bold;
}

.btn-volver, .btn-historico, .btn-guardar {
    background: #6c6eb1;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    font-size: 14px;
}

.btn-guardar {
    margin-top: 20px;
}

.btn-volver:hover, .btn-historico:hover, .btn-guardar:hover {
    background: #56579b;
}

.header-top {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

/* 🎨 Colores asistencia en histórico */
.presente {
    background-color: #d4ffcf !important;
    color: #006600 !important;
}

.ausente {
    background-color: #ffcccc !important;
    color: #cc0000 !important;
}

.justificado {
    background-color: #fff4c2 !important;
    color: #997a00 !important;
}

.total-col {
    background-color: #e0e0ff !important;
    font-weight: bold;
}

/* ✅ Tarjeta Resumen DIA */
.bloque-resumen-dia {
    width: 230px;
    background: #eef0ff;
    padding: 15px;
    border-radius: 8px;
    text-align: left;
    font-size: 15px;
    box-shadow: 0px 0px 6px rgba(0,0,0,0.10);
    height: fit-content;
}

/* 📊 Estadísticas Históricas */
#statsContainer {
    width: 270px;
    background: #f4f4ff;
    padding: 15px;
    border-radius: 8px;
    display: none;
    white-space: nowrap;
}

</style>

<!-- REGISTRO DE ASISTENCIA -->
<div class="contenedor-asistencia">

    <div class="tabla-asistencia-container">
        <div class="header-top">
            <h2>Registrar Asistencia</h2>
            <asp:Button ID="btnVolver" runat="server"
                Text="Volver" CssClass="btn-volver"
                OnClick="btnVolver_Click" />
        </div>

        <h3><asp:Label ID="lblCurso" runat="server" Font-Bold="true"></asp:Label></h3>
        <h5><asp:Label ID="lblFecha" runat="server" Font-Bold="true"></asp:Label></h5>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            DataKeyNames="legajo" CssClass="tabla-asistencia">
            <Columns>
                <asp:BoundField DataField="legajo" HeaderText="Legajo" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                <asp:TemplateField HeaderText="Presente">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkPresente" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Button ID="btnGuardar" runat="server" CssClass="btn-guardar"
            Text="Guardar asistencia" OnClick="btnGuardar_Click" />
    </div>

    <!-- ✅ TARJETA RESUMEN DEL DÍA -->
    <div class="bloque-resumen-dia">
        <h4>📌 Resumen del Día</h4>
        <table style="width:100%; font-size:14px;">
            <tr><td><strong>Presentes:</strong></td><td><asp:Label ID="lblPresentesHoy" runat="server" Text="0"></asp:Label></td></tr>
            <tr><td><strong>Ausentes:</strong></td><td><asp:Label ID="lblAusentesHoy" runat="server" Text="0"></asp:Label></td></tr>
            <tr><td><strong>% Asistencia:</strong></td><td><asp:Label ID="lblPorcHoy" runat="server" Text="0%"></asp:Label></td></tr>
        </table>
    </div>

</div>

<!-- HISTÓRICO DE ASISTENCIA -->
<div class="contenedor-historico">

    <div class="bloque-historico">
        <h3>Histórico de asistencias</h3>

        <asp:DropDownList ID="ddlMes" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="ddlAño" runat="server"></asp:DropDownList>

        <asp:Button ID="btnVerAsistencias" runat="server"
            CssClass="btn-historico"
            Text="Ver asistencias"
            OnClick="btnVerAsistencias_Click" />

        <asp:GridView ID="gvHistorico" runat="server"
            CssClass="tabla-historico"
            AutoGenerateColumns="True"
            Visible="False"
            ReadOnly="True"
            OnRowDataBound="gvHistorico_RowDataBound">
        </asp:GridView>
    </div>

    <div ID="statsContainer" runat="server" style="display:none;">
        <h4>📊 Estadísticas del Curso</h4>
        <table style="width:100%; font-size:14px;">
            <tr><td>Asistencia total:</td><td><asp:Label ID="lblAsistenciaTotal" runat="server" /></td></tr>
            <tr><td>Inasistencia total:</td><td><asp:Label ID="lblInasistenciaTotal" runat="server" /></td></tr>
            <tr><td>% Asistencia:</td><td><asp:Label ID="lblPorcAsistencia" runat="server" /></td></tr>
            <tr><td>% Inasistencia:</td><td><asp:Label ID="lblPorcInasistencia" runat="server" /></td></tr>
            <tr><td>Asistencia media:</td><td><asp:Label ID="lblAsistenciaMedia" runat="server" /></td></tr>
        </table>
    </div>

</div>

</asp:Content>
