<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="BuscarAlumnoProfe.aspx.cs"
    Inherits="proyectoPracticaProfecional.BuscarAlumnoProfe" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .btn {
            background-color: #6c6eb1;
            color: white;
            padding: 8px 16px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }
        .btn:hover { background-color: #56579b; }

        .tabla-notas, .tabla-asistencias {
            border-collapse: collapse;
            width: 100%;
            margin-top: 15px;
        }

        .tabla-notas th, .tabla-notas td,
        .tabla-asistencias th, .tabla-asistencias td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: center;
        }

        .tabla-notas th, .tabla-asistencias th {
            background-color: #494ca2;
            color: white;
        }

        .presente { background-color: #d4ffcf !important; color: #006600 !important; }
        .ausente { background-color: #ffcccc !important; color: #cc0000 !important; }
        .justificado { background-color: #fff4c2 !important; color: #997a00 !important; }

        .contenedor {
            margin: 20px auto;
            max-width: 1000px;
            font-family: Arial;
            margin-left: 80px;
        }

        .header-buttons {
            text-align: right;
            margin-bottom: 10px;
        }
    </style>

     <div class="contenedor">
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
        <h2>Buscar Alumno</h2>
        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn" OnClientClick="history.back(); return false;" /> 
    </div>

    <asp:TextBox ID="txtBuscar" runat="server" placeholder="Legajo, nombre o apellido"></asp:TextBox>
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn" OnClick="btnBuscar_Click" />

        <h3>Notas del Alumno</h3>
        <asp:GridView ID="gvAlumno" runat="server" AutoGenerateColumns="False" CssClass="tabla-notas" Visible="False">
            <Columns>
                <asp:BoundField DataField="LEGAJO" HeaderText="legajo" ReadOnly="True" />
                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" ReadOnly="True" />
                <asp:BoundField DataField="APELLIDO" HeaderText="Apellido" ReadOnly="True" />

                <asp:TemplateField HeaderText="Parcial 1">
                    <ItemTemplate>
                        <asp:TextBox ID="txtParcial1" runat="server" Text='<%# Bind("PARCIAL1") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Rec. Parcial 1">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRecParcial1" runat="server" Text='<%# Bind("REC_PARCIAL1") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Parcial 2">
                    <ItemTemplate>
                        <asp:TextBox ID="txtParcial2" runat="server" Text='<%# Bind("PARCIAL2") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Rec. Parcial 2">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRecParcial2" runat="server" Text='<%# Bind("REC_PARCIAL2") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Final">
                    <ItemTemplate>
                        <asp:TextBox ID="txtFinal" runat="server" Text='<%# Bind("FINAL") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Button ID="btnGuardarNotas" runat="server" Text="Guardar Notas" CssClass="btn" OnClick="btnGuardarNotas_Click" Visible="False" />

        <h3>Asistencias del Alumno</h3>
        <asp:DropDownList ID="ddlMes" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="ddlAño" runat="server"></asp:DropDownList>
        <asp:Button ID="btnVerAsistencias" runat="server" Text="Ver Asistencias" OnClick="btnVerAsistencias_Click" CssClass="btn" />

        <asp:GridView ID="gvAsistencias" runat="server" AutoGenerateColumns="True"
            OnRowDataBound="gvAsistencias_RowDataBound" Visible="False" CssClass="tabla-asistencias" />
    </div>

</asp:Content>