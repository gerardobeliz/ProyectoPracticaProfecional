<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="AsistenciaProfe.aspx.cs" Inherits="proyectoPracticaProfecional.AsistenciaProfe" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .contenedor-asistencia {
            margin-left: 100px; /* separación de la izquierda */
            margin-top: 1px;
            max-width: 900px;
            font-family: Arial;
        }

        .tabla-asistencia {
            width: 100%;
            border-collapse: collapse;
            background: #ffffff;
            border: 1px solid #ddd;
            margin-top: 10px;
        }

        .tabla-asistencia th {
            background: #494ca2;
            color: white;
            padding: 10px;
            text-align: left;
        }

        .tabla-asistencia td {
            padding: 8px;
            border-bottom: 1px solid #e5e5e5;
        }

        .btn-guardar {
            background: #494ca2;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin-top: 15px;
        }

        .btn-guardar:hover {
            background: #393c87;
        }

        h2, h3, h5 {
            margin: 3px 0;
        }
    </style>

    <div class="contenedor-asistencia">

        <h2>Registrar Asistencia</h2>
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

        <asp:Button ID="btnGuardar" runat="server"
            CssClass="btn-guardar"
            Text="Guardar asistencia" OnClick="btnGuardar_Click" />

    </div>

</asp:Content>