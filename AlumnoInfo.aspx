<%@ Page Title="Mi Información" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="AlumnoInfo.aspx.cs" Inherits="proyectoPracticaProfecional.AlumnoInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<style>
    .info-container {
        max-width: 800px;
        margin: 20px auto;
        padding: 20px;
        font-family: Arial, sans-serif;
    }
    
    .info-card {
        background: white;
        padding: 30px;
        border-radius: 10px;
        box-shadow: 0 2px 15px rgba(0,0,0,0.1);
    }
    
    .info-item {
        margin-bottom: 15px;
        padding-bottom: 15px;
        border-bottom: 1px solid #e9ecef;
    }
    
    .info-label {
        font-weight: bold;
        color: #2563eb;
        display: inline-block;
        width: 150px;
    }
</style>

<div class="info-container">
    <div class="info-card">
        <h2>Mi Información Personal</h2>
        
        <div class="info-item">
            <span class="info-label">Legajo:</span>
            <asp:Label ID="lblLegajo" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">DNI:</span>
            <asp:Label ID="lblDNI" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Nombre:</span>
            <asp:Label ID="lblNombre" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Carrera:</span>
            <asp:Label ID="lblCarrera" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Email:</span>
            <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Teléfono:</span>
            <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Dirección:</span>
            <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Código Postal:</span>
            <asp:Label ID="lblCP" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Género:</span>
            <asp:Label ID="lblGenero" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Fecha Nacimiento:</span>
            <asp:Label ID="lblFechaNac" runat="server" Text=""></asp:Label>
        </div>
        
        <div class="info-item">
            <span class="info-label">Fecha Ingreso:</span>
            <asp:Label ID="lblFechaIngreso" runat="server" Text=""></asp:Label>
        </div>
    </div>
</div>

</asp:Content>
