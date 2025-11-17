<%@ Page Title="Mis Notas" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="AlumnoNotas.aspx.cs" Inherits="proyectoPracticaProfecional.AlumnoNotas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<style>
    .alumno-container {
        max-width: 1000px;
        margin: 20px auto;
        padding: 20px;
        font-family: Arial, sans-serif;
    }
    
    .header-alumno {
        background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
        color: white;
        padding: 20px;
        border-radius: 10px;
        margin-bottom: 20px;
    }
    
    .info-alumno {
        background: #f8fafc;
        padding: 20px;
        border-radius: 8px;
        margin-bottom: 20px;
        border-left: 4px solid #2563eb;
    }
    
    .notas-container {
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }
    
    .tabla-notas {
        width: 100%;
        border-collapse: collapse;
        margin-top: 15px;
    }
    
    .tabla-notas th {
        background: #2563eb;
        color: white;
        padding: 12px;
        text-align: center;
    }
    
    .tabla-notas td {
        padding: 10px;
        border-bottom: 1px solid #e2e8f0;
        text-align: center;
    }
    
    .btn-cerrar {
        background: #dc2626;
        color: white;
        padding: 10px 20px;
        border: none;
        border-radius: 6px;
        cursor: pointer;
        margin-top: 20px;
    }
    
    .btn-cerrar:hover {
        background: #b91c1c;
    }
</style>

<div class="alumno-container">
    <!-- Header -->
    <div class="header-alumno">
        <h1>Portal del Alumno</h1>
        <h2>Bienvenido, <asp:Label ID="lblNombreAlumno" runat="server" Text=""></asp:Label></h2>
    </div>

    <!-- Información del Alumno -->
    <div class="info-alumno">
        <h3>Mis Datos</h3>
        <div class="row">
            <div class="col-md-6">
                <strong>Legajo:</strong> <asp:Label ID="lblLegajo" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-6">
                <strong>DNI:</strong> <asp:Label ID="lblDNI" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-6">
                <strong>Carrera:</strong> <asp:Label ID="lblCarrera" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-6">
                <strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-6">
                <strong>Teléfono:</strong> <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>

    <!-- Notas -->
    <div class="notas-container">
        <h3>Mis Notas</h3>
        
        <asp:Panel ID="pnlNotas" runat="server" Visible="false">
            <div class="table-responsive">
                <asp:GridView ID="gvNotas" runat="server" AutoGenerateColumns="False" 
                    CssClass="tabla-notas" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="CURSO" HeaderText="Curso" />
                        <asp:BoundField DataField="PARCIAL1" HeaderText="Parcial 1" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="REC_PARCIAL1" HeaderText="Rec. Parcial 1" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="PARCIAL2" HeaderText="Parcial 2" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="REC_PARCIAL2" HeaderText="Rec. Parcial 2" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="FINAL" HeaderText="Final" DataFormatString="{0:0.00}" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        
        <asp:Label ID="lblSinNotas" runat="server" Text="No hay notas cargadas." 
            Visible="false" CssClass="text-muted"></asp:Label>
    </div>

    <!-- Botón Cerrar Sesión -->
    <div class="text-center">
        <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" 
            CssClass="btn-cerrar" OnClick="btnCerrarSesion_Click" />
    </div>
</div>

</asp:Content>