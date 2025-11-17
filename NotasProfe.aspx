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
        .contenedor-principal {
            display: flex;
            gap: 20px;
            width: 95%;
            max-width: 1800px;
            margin: auto;
            padding: 20px 0;
        }

        .contenedor-notas {
            flex: 3;
            background: #ffffff;
            padding: 20px 25px;
            border-radius: 10px;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.10);
        }

        .contenedor-estadisticas {
            flex: 1;
            background: #ffffff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.10);
            min-width: 300px;
        }

        h2, h3, h5 {
            margin: 0;
            padding: 5px 0;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
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
            width: 100%;
        }

        .btn-guardar:hover {
            background-color: #1e8035;
        }

        /* Estilos para las tarjetas de estadísticas */
        .tarjeta-estadistica {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            border-left: 4px solid #494ca2;
            transition: all 0.3s ease;
        }

        .tarjeta-estadistica:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

        .titulo-tarjeta {
            font-size: 14px;
            font-weight: bold;
            color: #495057;
            margin-bottom: 8px;
        }

        .porcentaje {
            font-size: 24px;
            font-weight: bold;
            color: #494ca2;
        }

        .cantidad {
            font-size: 12px;
            color: #6c757d;
            margin-top: 5px;
        }

        .estadisticas-header {
            text-align: center;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #e9ecef;
        }

        .badge-aprobado {
            background-color: #28a745;
            color: white;
            padding: 2px 8px;
            border-radius: 12px;
            font-size: 12px;
        }

        .badge-desaprobado {
            background-color: #dc3545;
            color: white;
            padding: 2px 8px;
            border-radius: 12px;
            font-size: 12px;
        }
    </style>

    <div class="contenedor-principal">
        
        <!-- Panel izquierdo - Notas -->
        <div class="contenedor-notas">
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

        <!-- Panel derecho - Estadísticas -->
        <div class="contenedor-estadisticas">
            <div class="estadisticas-header">
                <h3>📊 Estadísticas del Curso</h3>
                <small>Nota de aprobación: 4.00</small>
            </div>

            <!-- Parcial 1 -->
            <div class="tarjeta-estadistica">
                <div class="titulo-tarjeta">Parcial 1 Aprobados</div>
                <div class="porcentaje"><asp:Label ID="lblPorcParcial1" runat="server" Text="0%"></asp:Label></div>
                <div class="cantidad">
                    <asp:Label ID="lblCantParcial1" runat="server" Text="0/0"></asp:Label>
                    <span class="badge-aprobado">≥ 4.00</span>
                </div>
            </div>

            <!-- Recuperatorio 1 -->
            <div class="tarjeta-estadistica">
                <div class="titulo-tarjeta">Rec. Parcial 1 Aprobados</div>
                <div class="porcentaje"><asp:Label ID="lblPorcRecParcial1" runat="server" Text="0%"></asp:Label></div>
                <div class="cantidad">
                    <asp:Label ID="lblCantRecParcial1" runat="server" Text="0/0"></asp:Label>
                    <span class="badge-aprobado">≥ 4.00</span>
                </div>
            </div>

            <!-- Parcial 2 -->
            <div class="tarjeta-estadistica">
                <div class="titulo-tarjeta">Parcial 2 Aprobados</div>
                <div class="porcentaje"><asp:Label ID="lblPorcParcial2" runat="server" Text="0%"></asp:Label></div>
                <div class="cantidad">
                    <asp:Label ID="lblCantParcial2" runat="server" Text="0/0"></asp:Label>
                    <span class="badge-aprobado">≥ 4.00</span>
                </div>
            </div>

            <!-- Recuperatorio 2 -->
            <div class="tarjeta-estadistica">
                <div class="titulo-tarjeta">Rec. Parcial 2 Aprobados</div>
                <div class="porcentaje"><asp:Label ID="lblPorcRecParcial2" runat="server" Text="0%"></asp:Label></div>
                <div class="cantidad">
                    <asp:Label ID="lblCantRecParcial2" runat="server" Text="0/0"></asp:Label>
                    <span class="badge-aprobado">≥ 4.00</span>
                </div>
            </div>

            <!-- Final -->
            <div class="tarjeta-estadistica">
                <div class="titulo-tarjeta">Final Aprobados</div>
                <div class="porcentaje"><asp:Label ID="lblPorcFinal" runat="server" Text="0%"></asp:Label></div>
                <div class="cantidad">
                    <asp:Label ID="lblCantFinal" runat="server" Text="0/0"></asp:Label>
                    <span class="badge-aprobado">≥ 4.00</span>
                </div>
            </div>

            <!-- Resumen General -->
            <div class="tarjeta-estadistica" style="background: #e8f4fd; border-left-color: #17a2b8;">
                <div class="titulo-tarjeta">Total Alumnos</div>
                <div class="porcentaje" style="color: #17a2b8;">
                    <asp:Label ID="lblTotalAlumnos" runat="server" Text="0"></asp:Label>
                </div>
                <div class="cantidad">En el curso actual</div>
            </div>
        </div>
    </div>

</asp:Content>