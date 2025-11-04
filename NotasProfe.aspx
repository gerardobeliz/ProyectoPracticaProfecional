<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="NotasProfe.aspx.cs"
    Inherits="proyectoPracticaProfecional.NotasProfe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 style="margin-bottom:5px;">Registrar Notas</h2>
    <h3><asp:Label ID="lblCurso" runat="server" Font-Bold="true"></asp:Label></h3>
    <h5><asp:Label ID="lblFecha" runat="server" Font-Bold="true" Text='<%# DateTime.Now.ToString("dddd, dd MMMM yyyy") %>'></asp:Label></h5>
    <br />

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="legajo" Width="80%" GridLines="Both" CellPadding="5" Style="margin-left:20px; margin-bottom:20px;">
        <Columns>
            <asp:BoundField DataField="legajo" HeaderText="Legajo" ReadOnly="True" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" ReadOnly="True" />
            <asp:BoundField DataField="apellido" HeaderText="Apellido" ReadOnly="True" />

            <asp:TemplateField HeaderText="Parcial 1">
                <ItemTemplate>
                    <asp:TextBox ID="txtParcial1" runat="server" Width="60px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Rec. Parcial 1">
                <ItemTemplate>
                    <asp:TextBox ID="txtRecParcial1" runat="server" Width="60px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Parcial 2">
                <ItemTemplate>
                    <asp:TextBox ID="txtParcial2" runat="server" Width="60px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Rec. Parcial 2">
                <ItemTemplate>
                    <asp:TextBox ID="txtRecParcial2" runat="server" Width="60px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Final">
                <ItemTemplate>
                    <asp:TextBox ID="txtFinal" runat="server" Width="60px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Notas" OnClick="btnGuardar_Click"
        Style="margin-left:20px; padding:6px 12px; background-color:#494ca2; color:white; border:none; border-radius:4px; cursor:pointer;" />

</asp:Content>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="NotasProfe.aspx.cs"
    Inherits="proyectoPracticaProfecional.NotasProfe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .contenedor {
            width: 90%;
            max-width: 1600px;
            margin: auto;
            background: #ffffff;
            padding: 20px 25px;
            border-radius: 10px;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.10);
        }

        h2, h3, h5 {
            margin: 0;
            padding: 5px 0;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .btn-home {
            background-color: #494ca2;
            color: #fff;
            padding: 8px 14px;
            border: none;
            border-radius: 6px;
            font-size: 14px;
            cursor: pointer;
            transition: 0.2s;
        }

        .btn-home:hover {
            background-color: #3d3f8f;
        }

        #GridView1 {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        #GridView1 th {
            background-color: #494ca2;
            color: #fff;
            padding: 10px;
            text-align: center;
        }

        #GridView1 td {
            background-color: #f7f7f7;
            padding: 8px;
            text-align: center;
            border-bottom: 1px solid #ddd;
        }

        .btn-guardar {
            margin-top: 18px;
            background-color: #28a745;
            color: white;
            padding: 10px 18px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 15px;
            transition: 0.2s;
        }

        .btn-guardar:hover {
            background-color: #1e8035;
        }
    </style>

    <div class="contenedor">

        <div class="header">
            <div>
                <h2>Registrar Notas</h2>
                <h3><asp:Label ID="lblCurso" runat="server" Font-Bold="true"></asp:Label></h3>
                <h5><asp:Label ID="lblFecha" runat="server" Font-Bold="true"></asp:Label></h5>
            </div>

            <asp:Button ID="btnVolver" runat="server" Text="🏠 Volver"
                CssClass="btn-home"
                OnClick="btnVolver_Click" />
        </div>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            DataKeyNames="legajo"
            CellPadding="6" GridLines="None">
            <Columns>
                <asp:BoundField DataField="legajo" HeaderText="Legajo" ReadOnly="True" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" ReadOnly="True" />
                <asp:BoundField DataField="apellido" HeaderText="Apellido" ReadOnly="True" />

               <asp:TemplateField HeaderText="Parcial 1">
    <ItemTemplate>
        <asp:TextBox ID="txtParcial1" runat="server" Width="60px"
            Text='<%# Eval("PARCIAL1") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Rec. Parcial 1">
    <ItemTemplate>
        <asp:TextBox ID="txtRecParcial1" runat="server" Width="60px"
            Text='<%# Eval("REC_PARCIAL1") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Parcial 2">
    <ItemTemplate>
        <asp:TextBox ID="txtParcial2" runat="server" Width="60px"
            Text='<%# Eval("PARCIAL2") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Rec. Parcial 2">
    <ItemTemplate>
        <asp:TextBox ID="txtRecParcial2" runat="server" Width="60px"
            Text='<%# Eval("REC_PARCIAL2") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Final">
    <ItemTemplate>
        <asp:TextBox ID="txtFinal" runat="server" Width="60px"
            Text='<%# Eval("FINAL") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Button ID="btnGuardar" runat="server" Text="✅ Guardar Notas"
            CssClass="btn-guardar"
            OnClick="btnGuardar_Click" />

    </div>

</asp:Content>