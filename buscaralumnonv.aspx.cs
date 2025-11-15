using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace proyectoPracticaProfecional
{
    public partial class BuscarAlumnoNV : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar sesión del profesor
                if (Session["id_personal"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }

                LimpiarInterfaz();
                OcultarMensajes();
                CargarCursoProfesor();
                InicializarFiltrosAsistencia();
            }
        }

        private void CargarCursoProfesor()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    string query = @"
                    SELECT id_curso, curso 
                    FROM cursos 
                    WHERE id_personal = @idPersonal";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPersonal", Session["id_personal"].ToString());

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Session["id_curso"] = reader["id_curso"].ToString();
                            Session["nombre_curso"] = reader["curso"].ToString();
                            lblCursoActual.Text = "Curso actual: " + reader["curso"].ToString();
                        }
                        else
                        {
                            MostrarMensajeError("No tenés ningún curso asignado.");
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar el curso: " + ex.Message);
            }
        }

        private void InicializarFiltrosAsistencia()
        {
            // Inicializar ddlMes con nombres de meses
            ddlMes.Items.Clear();
            ddlMes.Items.Add(new ListItem("Enero", "1"));
            ddlMes.Items.Add(new ListItem("Febrero", "2"));
            ddlMes.Items.Add(new ListItem("Marzo", "3"));
            ddlMes.Items.Add(new ListItem("Abril", "4"));
            ddlMes.Items.Add(new ListItem("Mayo", "5"));
            ddlMes.Items.Add(new ListItem("Junio", "6"));
            ddlMes.Items.Add(new ListItem("Julio", "7"));
            ddlMes.Items.Add(new ListItem("Agosto", "8"));
            ddlMes.Items.Add(new ListItem("Septiembre", "9"));
            ddlMes.Items.Add(new ListItem("Octubre", "10"));
            ddlMes.Items.Add(new ListItem("Noviembre", "11"));
            ddlMes.Items.Add(new ListItem("Diciembre", "12"));

            ddlAño.Items.Clear();
            int añoActual = DateTime.Now.Year;
            for (int a = añoActual - 3; a <= añoActual + 1; a++)
                ddlAño.Items.Add(new ListItem(a.ToString(), a.ToString()));

            ddlMes.SelectedValue = DateTime.Now.Month.ToString();
            ddlAño.SelectedValue = DateTime.Now.Year.ToString();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = txtBuscarAlumno.Text.Trim();

            if (string.IsNullOrEmpty(criterioBusqueda))
            {
                MostrarMensajeError("Por favor, ingrese un criterio de búsqueda.");
                return;
            }

            if (Session["id_curso"] == null)
            {
                MostrarMensajeError("No tiene un curso asignado para buscar alumnos.");
                return;
            }

            BuscarAlumnos(criterioBusqueda);
        }

        private void BuscarAlumnos(string criterio)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    string query = @"
                    SELECT A.LEGAJO, A.DNI, A.NOMBRE, A.APELLIDO, A.EMAIL, A.TELEFONO, A.CARRERA
                    FROM ALUMNOS A
                    WHERE A.CARRERA = @nombreCurso 
                    AND (A.nombre LIKE @criterio OR A.apellido LIKE @criterio OR CONVERT(VARCHAR, A.DNI) LIKE @criterio OR A.LEGAJO LIKE @criterio)
                    ORDER BY A.apellido, A.nombre";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreCurso", Session["nombre_curso"].ToString());
                        command.Parameters.AddWithValue("@criterio", "%" + criterio + "%");

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            gvAlumnos.DataSource = dt;
                            gvAlumnos.DataBind();
                            pnlResultados.Visible = true;
                            OcultarMensajes();

                            lblResultados.Text = "Se encontraron " + dt.Rows.Count.ToString() + " alumno(s)";
                            lblResultados.Visible = true;

                            // Ocultar paneles de detalles hasta que seleccionen un alumno
                            pnlNotas.Visible = false;
                            pnlAsistencias.Visible = false;
                        }
                        else
                        {
                            pnlResultados.Visible = false;
                            string mensaje = "No se encontraron alumnos con '" + criterio + "' en el curso " + Session["nombre_curso"].ToString() + ".";
                            MostrarMensajeError(mensaje);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar alumnos: " + ex.Message);
            }
        }

        protected void gvAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvAlumnos.SelectedRow != null)
            {
                string legajo = gvAlumnos.DataKeys[gvAlumnos.SelectedIndex].Value.ToString();

                // Guardar legajo en ViewState para usar en notas y asistencias
                ViewState["LegajoSeleccionado"] = legajo;

                // Mostrar información del alumno seleccionado
                string nombre = gvAlumnos.SelectedRow.Cells[3].Text; // Columna Nombre
                string apellido = gvAlumnos.SelectedRow.Cells[2].Text; // Columna Apellido
                lblAlumnoSeleccionado.Text = "Alumno seleccionado: " + apellido + ", " + nombre;

                // Cargar notas del alumno
                CargarNotasAlumno(legajo);

                // Ocultar asistencias inicialmente
                pnlAsistencias.Visible = false;

                MostrarMensajeExito("Alumno seleccionado: " + apellido + ", " + nombre);
            }
        }

        private void CargarNotasAlumno(string legajo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    string query = @"
                    SELECT 
                        ISNULL(PARCIAL1, 0) AS PARCIAL1,
                        ISNULL(REC_PARCIAL1, 0) AS REC_PARCIAL1,
                        ISNULL(PARCIAL2, 0) AS PARCIAL2,
                        ISNULL(REC_PARCIAL2, 0) AS REC_PARCIAL2,
                        ISNULL(FINAL, 0) AS FINAL
                    FROM NOTAS 
                    WHERE LEGAJO = @legajo AND CURSO = @curso";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@legajo", legajo);
                        command.Parameters.AddWithValue("@curso", Session["nombre_curso"].ToString());

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            gvNotas.DataSource = dt;
                        }
                        else
                        {
                            // Crear fila vacía si no hay notas
                            DataTable dtVacio = new DataTable();
                            dtVacio.Columns.Add("PARCIAL1", typeof(decimal));
                            dtVacio.Columns.Add("REC_PARCIAL1", typeof(decimal));
                            dtVacio.Columns.Add("PARCIAL2", typeof(decimal));
                            dtVacio.Columns.Add("REC_PARCIAL2", typeof(decimal));
                            dtVacio.Columns.Add("FINAL", typeof(decimal));

                            DataRow row = dtVacio.NewRow();
                            row["PARCIAL1"] = 0;
                            row["REC_PARCIAL1"] = 0;
                            row["PARCIAL2"] = 0;
                            row["REC_PARCIAL2"] = 0;
                            row["FINAL"] = 0;
                            dtVacio.Rows.Add(row);

                            gvNotas.DataSource = dtVacio;
                        }

                        gvNotas.DataBind();
                        pnlNotas.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar notas: " + ex.Message);
            }
        }

        protected void btnVerAsistencias_Click(object sender, EventArgs e)
        {
            if (ViewState["LegajoSeleccionado"] == null)
            {
                MostrarMensajeError("Primero seleccione un alumno.");
                return;
            }

            int legajo = Convert.ToInt32(ViewState["LegajoSeleccionado"]);

            int mes, año;
            if (!int.TryParse(ddlMes.SelectedValue, out mes))
            {
                MostrarMensajeError("Seleccione un mes válido.");
                return;
            }
            if (!int.TryParse(ddlAño.SelectedValue, out año))
            {
                MostrarMensajeError("Seleccione un año válido.");
                return;
            }

            // DEBUG: Verificar asistencias del alumno en el rango específico
            VerificarAsistenciasEnBaseDeDatos(legajo, mes, año);

            // ✅ CORREGIDO: Ya no necesitamos pasar idCurso
            CargarAsistenciasAlumno(legajo, mes, año);
        }
        private void VerificarAsistenciasEnBaseDeDatos(int legajo, int mes, int año)
        {
            try
            {
                DateTime primerDia = new DateTime(año, mes, 1);
                DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    con.Open();

                    // Consulta para ver las asistencias del alumno en el rango de fechas específico
                    SqlCommand cmd = new SqlCommand(@"
                SELECT FECHA, PRESENTE, LEGAJO, ID_CURSO
                FROM ASISTENCIA 
                WHERE LEGAJO = @legajo 
                AND FECHA >= @fechaInicio 
                AND FECHA <= @fechaFin
                ORDER BY FECHA", con);

                    cmd.Parameters.AddWithValue("@legajo", legajo);
                    cmd.Parameters.AddWithValue("@fechaInicio", primerDia);
                    cmd.Parameters.AddWithValue("@fechaFin", ultimoDia);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        System.Diagnostics.Debug.WriteLine("=== ASISTENCIAS DEL ALUMNO " + legajo + " PARA " + mes + "/" + año + " ===");
                        int count = 0;
                        while (dr.Read())
                        {
                            DateTime fecha = Convert.ToDateTime(dr["FECHA"]);
                            int estado = Convert.ToInt32(dr["PRESENTE"]);
                            int cursoId = Convert.ToInt32(dr["ID_CURSO"]);

                            System.Diagnostics.Debug.WriteLine("Fecha: " + fecha.ToString("dd/MM/yyyy") +
                                                             " | Estado: " + estado +
                                                             " | Curso: " + cursoId);
                            count++;
                        }
                        System.Diagnostics.Debug.WriteLine("Total asistencias encontradas en el rango: " + count);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR en VerificarAsistenciasEnBaseDeDatos: " + ex.Message);
            }
        }
        private void CargarAsistenciasAlumno(int legajo, int mes, int año)
        {
            try
            {
                DateTime primerDia = new DateTime(año, mes, 1);
                DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

                DataTable dtMes = new DataTable();
                dtMes.Columns.Add("Día", typeof(string));

                // Crear columnas para cada día hábil del mes
                List<DateTime> diasHabiles = new List<DateTime>();
                for (DateTime d = primerDia; d <= ultimoDia; d = d.AddDays(1))
                {
                    if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                    {
                        dtMes.Columns.Add(d.ToString("dd/MM"), typeof(string));
                        diasHabiles.Add(d);
                    }
                }

                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    con.Open();

                    // ✅ CORREGIDO: Buscar asistencias solo por LEGAJO y fechas, sin filtrar por ID_CURSO
                    SqlCommand cmdAsis = new SqlCommand(@"
                SELECT FECHA, PRESENTE
                FROM ASISTENCIA
                WHERE LEGAJO = @legajo 
                  AND FECHA >= @fechaInicio 
                  AND FECHA <= @fechaFin
                ORDER BY FECHA", con);

                    cmdAsis.Parameters.AddWithValue("@legajo", legajo);
                    cmdAsis.Parameters.AddWithValue("@fechaInicio", primerDia);
                    cmdAsis.Parameters.AddWithValue("@fechaFin", ultimoDia);

                    // DEBUG: Mostrar parámetros para verificar
                    System.Diagnostics.Debug.WriteLine("Buscando asistencias para:");
                    System.Diagnostics.Debug.WriteLine("Legajo: " + legajo);
                    System.Diagnostics.Debug.WriteLine("Fecha inicio: " + primerDia.ToString("dd/MM/yyyy"));
                    System.Diagnostics.Debug.WriteLine("Fecha fin: " + ultimoDia.ToString("dd/MM/yyyy"));

                    Dictionary<DateTime, int> asistencias = new Dictionary<DateTime, int>();
                    using (SqlDataReader dr = cmdAsis.ExecuteReader())
                    {
                        int count = 0;
                        while (dr.Read())
                        {
                            DateTime fecha = Convert.ToDateTime(dr["FECHA"]);
                            int estado = Convert.ToInt32(dr["PRESENTE"]);
                            asistencias[fecha] = estado;
                            count++;

                            // DEBUG: Mostrar cada asistencia encontrada
                            System.Diagnostics.Debug.WriteLine("Asistencia encontrada: " + fecha.ToString("dd/MM/yyyy") + " - Estado: " + estado);
                        }

                        // DEBUG: Mostrar total encontrado
                        System.Diagnostics.Debug.WriteLine("Total asistencias encontradas: " + count);
                    }

                    // Crear fila para el alumno
                    DataRow filaAlumno = dtMes.NewRow();
                    filaAlumno["Día"] = "Asistencia";

                    // Llenar las asistencias para cada día hábil
                    for (int i = 0; i < diasHabiles.Count; i++)
                    {
                        DateTime dia = diasHabiles[i];
                        string nombreColumna = dia.ToString("dd/MM");
                        string valor = "A"; // Ausente por defecto

                        if (asistencias.ContainsKey(dia))
                        {
                            int estado = asistencias[dia];
                            if (estado == 1)
                                valor = "P"; // Presente
                            else if (estado == 2)
                                valor = "J"; // Justificado
                        }

                        filaAlumno[nombreColumna] = valor;
                    }

                    dtMes.Rows.Add(filaAlumno);

                    // Configurar la grilla
                    gvAsistencias.DataSource = dtMes;
                    gvAsistencias.DataBind();
                    pnlAsistencias.Visible = true;

                    // Mostrar mensaje informativo
                    MostrarMensajeExito("Asistencias cargadas para " + DateTime.Now.ToString("MMMM yyyy"));
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar asistencias: " + ex.Message);

                // DEBUG: Mostrar error completo
                System.Diagnostics.Debug.WriteLine("ERROR en CargarAsistenciasAlumno: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }

        protected void gvAsistencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // La primera celda (Día) no necesita estilo especial
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    string valor = e.Row.Cells[i].Text.Trim();
                    switch (valor)
                    {
                        case "P":
                            e.Row.Cells[i].CssClass = "presente";
                            e.Row.Cells[i].Text = "P";
                            break;
                        case "A":
                            e.Row.Cells[i].CssClass = "ausente";
                            e.Row.Cells[i].Text = "A";
                            break;
                        case "J":
                            e.Row.Cells[i].CssClass = "justificado";
                            e.Row.Cells[i].Text = "J";
                            break;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                // Estilizar el header
                e.Row.CssClass = "table-primary";
            }
        }

        protected void btnGuardarNotas_Click(object sender, EventArgs e)
        {
            if (ViewState["LegajoSeleccionado"] == null || gvNotas.Rows.Count == 0)
            {
                MostrarMensajeError("No hay alumno seleccionado o notas para guardar.");
                return;
            }

            int legajo = Convert.ToInt32(ViewState["LegajoSeleccionado"]);
            GridViewRow row = gvNotas.Rows[0];

            TextBox txtParcial1 = row.FindControl("txtParcial1") as TextBox;
            TextBox txtRecParcial1 = row.FindControl("txtRecParcial1") as TextBox;
            TextBox txtParcial2 = row.FindControl("txtParcial2") as TextBox;
            TextBox txtRecParcial2 = row.FindControl("txtRecParcial2") as TextBox;
            TextBox txtFinal = row.FindControl("txtFinal") as TextBox;

            decimal p1 = ParseDecimalOrZero(txtParcial1 != null ? txtParcial1.Text : "");
            decimal rp1 = ParseDecimalOrZero(txtRecParcial1 != null ? txtRecParcial1.Text : "");
            decimal p2 = ParseDecimalOrZero(txtParcial2 != null ? txtParcial2.Text : "");
            decimal rp2 = ParseDecimalOrZero(txtRecParcial2 != null ? txtRecParcial2.Text : "");
            decimal final = ParseDecimalOrZero(txtFinal != null ? txtFinal.Text : "");

            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    con.Open();
                    SqlCommand cmdUpdate = new SqlCommand(@"
                        UPDATE NOTAS SET PARCIAL1=@p1, REC_PARCIAL1=@rp1, PARCIAL2=@p2, REC_PARCIAL2=@rp2, FINAL=@final
                        WHERE LEGAJO=@legajo AND CURSO=@curso", con);

                    cmdUpdate.Parameters.AddWithValue("@p1", p1);
                    cmdUpdate.Parameters.AddWithValue("@rp1", rp1);
                    cmdUpdate.Parameters.AddWithValue("@p2", p2);
                    cmdUpdate.Parameters.AddWithValue("@rp2", rp2);
                    cmdUpdate.Parameters.AddWithValue("@final", final);
                    cmdUpdate.Parameters.AddWithValue("@legajo", legajo);
                    cmdUpdate.Parameters.AddWithValue("@curso", Session["nombre_curso"].ToString());

                    int afectados = cmdUpdate.ExecuteNonQuery();
                    if (afectados == 0)
                    {
                        SqlCommand cmdInsert = new SqlCommand(@"
                            INSERT INTO NOTAS (LEGAJO, CURSO, PARCIAL1, REC_PARCIAL1, PARCIAL2, REC_PARCIAL2, FINAL)
                            VALUES (@legajo, @curso, @p1, @rp1, @p2, @rp2, @final)", con);

                        cmdInsert.Parameters.AddWithValue("@legajo", legajo);
                        cmdInsert.Parameters.AddWithValue("@curso", Session["nombre_curso"].ToString());
                        cmdInsert.Parameters.AddWithValue("@p1", p1);
                        cmdInsert.Parameters.AddWithValue("@rp1", rp1);
                        cmdInsert.Parameters.AddWithValue("@p2", p2);
                        cmdInsert.Parameters.AddWithValue("@rp2", rp2);
                        cmdInsert.Parameters.AddWithValue("@final", final);

                        cmdInsert.ExecuteNonQuery();
                    }
                }

                MostrarMensajeExito("Notas guardadas correctamente.");
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al guardar notas: " + ex.Message);
            }
        }

        private decimal ParseDecimalOrZero(string s)
        {
            decimal v;
            if (decimal.TryParse(s != null ? s.Trim() : "", out v))
                return v;
            return 0;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarInterfaz();
        }

        private void LimpiarInterfaz()
        {
            txtBuscarAlumno.Text = string.Empty;
            pnlResultados.Visible = false;
            pnlNotas.Visible = false;
            pnlAsistencias.Visible = false;
            lblResultados.Visible = false;
            ViewState["LegajoSeleccionado"] = null;
            OcultarMensajes();
        }

        private void MostrarMensajeExito(string mensaje)
        {
            lblSuccessMessage.Text = mensaje;
            pnlSuccessMessage.Visible = true;
            pnlErrorMessage.Visible = false;
        }

        private void MostrarMensajeError(string mensaje)
        {
            lblErrorMessage.Text = mensaje;
            pnlErrorMessage.Visible = true;
            pnlSuccessMessage.Visible = false;
        }

        private void OcultarMensajes()
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
        }
    }
}