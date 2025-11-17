using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace proyectoPracticaProfecional
{
    public partial class Asistencia : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;
        private List<Alumno> Alumnos = new List<Alumno>();
        private DateTime? FechaSeleccionada = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCursos();
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                pnlEmpty.Visible = true;
                gvAsistencia.Visible = false;
                pnlMensaje.Visible = false;
                pnlFechaSeleccionada.Visible = false;
            }
            else
            {
                // Recuperar datos de sesión en postback
                if (Session["Alumnos"] != null)
                    Alumnos = (List<Alumno>)Session["Alumnos"];
                if (Session["FechaSeleccionada"] != null)
                    FechaSeleccionada = (DateTime)Session["FechaSeleccionada"];
            }
        }

        private void CargarCursos()
        {
            try
            {
                ddlCurso.Items.Clear();
                ddlCurso.Items.Add(new ListItem("Seleccionar curso", ""));

                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();
                    string query = "SELECT DISTINCT curso FROM CURSOS";
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombreCurso = reader["curso"].ToString();
                            ddlCurso.Items.Add(new ListItem(nombreCurso, nombreCurso));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar cursos: " + ex.Message, "error");
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlCurso.SelectedValue) || ddlCurso.SelectedValue == "")
                {
                    MostrarMensaje("Por favor seleccione un curso", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(txtFecha.Text))
                {
                    MostrarMensaje("Por favor seleccione una fecha", "warning");
                    return;
                }

                // Fecha seleccionada
                FechaSeleccionada = DateTime.Parse(txtFecha.Text);
                Session["FechaSeleccionada"] = FechaSeleccionada;

                // Mostrar fecha formateada
                CultureInfo cultura = new CultureInfo("es-ES");
                lblFechaSeleccionada.Text = FechaSeleccionada.Value.ToString("dddd, dd 'de' MMMM 'de' yyyy", cultura);
                pnlFechaSeleccionada.Visible = true;

                // Cargar alumnos del curso
                CargarAlumnosDelCurso();

                // Generar la tabla de asistencia
                GenerarGridAsistencia();

                // Mostrar panel de tabla
                pnlEmpty.Visible = false;
                gvAsistencia.Visible = true;

                MostrarMensaje("Asistencia cargada para " + Alumnos.Count.ToString() + " alumnos", "info");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar datos: " + ex.Message, "error");
            }
        }

        private void CargarAlumnosDelCurso()
        {
            string cursoSeleccionado = ddlCurso.SelectedValue;

            if (string.IsNullOrEmpty(cursoSeleccionado) || cursoSeleccionado == "Seleccionar curso")
            {
                MostrarMensaje("Seleccione un curso válido.", "error");
                return;
            }

            Alumnos = new List<Alumno>();

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                conexion.Open();
                string query = @"
                    SELECT A.LEGAJO, A.NOMBRE, A.APELLIDO, C.ID_CURSO
                    FROM ALUMNOS A
                    INNER JOIN CURSOS C ON A.LEGAJO = C.LEGAJO
                    WHERE C.CURSO = @Curso
                    ORDER BY A.APELLIDO, A.NOMBRE";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Curso", cursoSeleccionado);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alumno alumno = new Alumno
                            {
                                Legajo = Convert.ToInt32(reader["LEGAJO"]),
                                Nombre = reader["NOMBRE"].ToString(),
                                Apellido = reader["APELLIDO"].ToString(),
                                Id_Curso = Convert.ToInt32(reader["ID_CURSO"]),
                                NombreCompleto = reader["APELLIDO"].ToString() + ", " + reader["NOMBRE"].ToString()
                            };
                            Alumnos.Add(alumno);
                        }
                    }
                }
            }

            Session["Alumnos"] = Alumnos;

            if (Alumnos.Count == 0)
                MostrarMensaje("No se encontraron alumnos para el curso seleccionado", "warning");
        }

        private void GenerarGridAsistencia()
        {
            if (FechaSeleccionada == null || Alumnos == null)
                return;

            // Crear DataTable para el GridView
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("NombreCompleto", typeof(string));
            dt.Columns.Add("EstadoAsistencia", typeof(string));

            foreach (Alumno alumno in Alumnos)
            {
                // Obtener estado de asistencia existente para esta fecha
                string estadoExistente = ObtenerEstadoAsistencia(alumno.Legajo, FechaSeleccionada.Value);

                DataRow row = dt.NewRow();
                row["Id"] = alumno.Legajo;
                row["NombreCompleto"] = alumno.NombreCompleto;
                row["EstadoAsistencia"] = estadoExistente; // CORREGIDO: estadoExistente en lugar de estadoExistencia
                dt.Rows.Add(row);
            }

            gvAsistencia.DataSource = dt;
            gvAsistencia.DataBind();

            // Actualizar contadores
            totalAlumnos.InnerText = Alumnos.Count.ToString();
            ActualizarContadoresResumen();
        }

        protected void gvAsistencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEstadoAsistencia = (DropDownList)e.Row.FindControl("ddlEstadoAsistencia");
                HiddenField hfAlumnoId = (HiddenField)e.Row.FindControl("hfAlumnoId");

                if (ddlEstadoAsistencia != null)
                {
                    // Configurar el valor seleccionado
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string estadoActual = rowView["EstadoAsistencia"].ToString();

                    if (!string.IsNullOrEmpty(estadoActual))
                    {
                        ddlEstadoAsistencia.SelectedValue = estadoActual;
                    }

                    // Agregar evento JavaScript
                    ddlEstadoAsistencia.Attributes["onchange"] = "aplicarEstiloDropdown(this)";
                }
            }
        }

        private string ObtenerEstadoAsistencia(int legajo, DateTime fecha)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();
                    string query = "SELECT PRESENTE FROM ASISTENCIA WHERE LEGAJO = '@Legajo' AND FECHA = '@Fecha'";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Legajo", legajo);
                        cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                        object resultado = cmd.ExecuteScalar();
                        if (resultado != null)
                        {
                            int presente = Convert.ToInt32(resultado);
                            return presente == 1 ? "P" : "A";
                        }
                        else
                        {
                            return ""; // Valor por defecto vacío
                        }
                    }
                }
            }
            catch
            {
                return ""; // En caso de error, retornar vacío
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Recuperar datos desde sesión
                if (Session["Alumnos"] != null)
                    Alumnos = (List<Alumno>)Session["Alumnos"];
                if (Session["FechaSeleccionada"] != null)
                    FechaSeleccionada = (DateTime)Session["FechaSeleccionada"];

                // Validaciones
                if (Alumnos == null || Alumnos.Count == 0)
                {
                    MostrarMensaje("No hay alumnos para guardar.", "error");
                    return;
                }

                if (FechaSeleccionada == null)
                {
                    MostrarMensaje("No hay fecha seleccionada.", "error");
                    return;
                }

                int registrosGuardados = 0;
                int registrosConError = 0;

                // Recorrer el GridView para obtener los valores
                foreach (GridViewRow row in gvAsistencia.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlEstadoAsistencia = (DropDownList)row.FindControl("ddlEstadoAsistencia");
                        HiddenField hfAlumnoId = (HiddenField)row.FindControl("hfAlumnoId");

                        if (ddlEstadoAsistencia != null && hfAlumnoId != null)
                        {
                            int legajo = Convert.ToInt32(hfAlumnoId.Value);
                            string estado = ddlEstadoAsistencia.SelectedValue;
                            Alumno alumno = Alumnos.Find(a => a.Legajo == legajo);

                            if (alumno != null && !string.IsNullOrEmpty(estado))
                            {
                                if (GuardarAsistenciaBD(alumno.Legajo, alumno.Id_Curso, FechaSeleccionada.Value, estado))
                                    registrosGuardados++;
                                else
                                    registrosConError++;
                            }
                        }
                    }
                }

                // Mostrar mensaje según resultados
                if (registrosGuardados > 0)
                {
                    string mensaje = "Asistencia guardada correctamente. " + registrosGuardados.ToString() + " registros actualizados.";
                    if (registrosConError > 0)
                        mensaje += " " + registrosConError.ToString() + " registros con errores.";

                    MostrarMensaje(mensaje, "success");
                }
                else
                {
                    MostrarMensaje("No se guardó ninguna asistencia.", "warning");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar asistencia: " + ex.Message, "error");
            }
        }

        private bool GuardarAsistenciaBD(int legajo, int Id_Curso, DateTime fecha, string estado)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();

                    // VERIFICAR SI YA EXISTE el registro
                    string checkQuery = "SELECT COUNT(1) FROM ASISTENCIA WHERE LEGAJO = @Legajo AND FECHA = @Fecha";

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conexion))
                    {
                        checkCmd.Parameters.AddWithValue("@Legajo", legajo);
                        checkCmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                        bool existe = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;

                        string query;
                        if (existe)
                        {
                            // UPDATE si existe
                            query = @"UPDATE ASISTENCIA 
                                     SET PRESENTE = @Presente, ID_CURSO = @IdCurso
                                     WHERE LEGAJO = @Legajo AND FECHA = @Fecha";
                        }
                        else
                        {
                            // INSERT si no existe
                            query = @"INSERT INTO ASISTENCIA (LEGAJO, ID_CURSO, FECHA, PRESENTE)
                                     VALUES (@Legajo, @IdCurso, @Fecha, @Presente)";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@Legajo", legajo);
                            cmd.Parameters.AddWithValue("@IdCurso", Id_Curso);
                            cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                            // Convertir estado a valor booleano para la base de datos
                            int presente = (estado == "P") ? 1 : 0;
                            cmd.Parameters.AddWithValue("@Presente", presente);

                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error (en producción usar un logger)
                System.Diagnostics.Debug.WriteLine("Error BD: " + ex.Message);
                return false;
            }
        }

        private void ActualizarContadoresResumen()
        {
            if (FechaSeleccionada == null) return;

            int presentes = 0;
            int ausentes = 0;
            int justificados = 0;

            foreach (Alumno alumno in Alumnos)
            {
                string estado = ObtenerEstadoAsistencia(alumno.Legajo, FechaSeleccionada.Value);
                switch (estado)
                {
                    case "P": presentes++; break;
                    case "A": ausentes++; break;
                    case "J": justificados++; break;
                }
            }

            presentCount.InnerText = presentes.ToString();
            absentCount.InnerText = ausentes.ToString();
            justifiedCount.InnerText = justificados.ToString();
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            lblMensaje.Text = mensaje;

            switch (tipo.ToLower())
            {
                case "success":
                case "ok":
                case "info":
                    pnlMensaje.CssClass = "mensaje-exito";
                    break;
                case "error":
                    pnlMensaje.CssClass = "mensaje-error";
                    break;
                case "warning":
                    pnlMensaje.CssClass = "mensaje-error";
                    break;
                default:
                    pnlMensaje.CssClass = "mensaje-exito";
                    break;
            }

            // Ocultar mensaje después de 5 segundos
            ScriptManager.RegisterStartupScript(this, GetType(), "hideMessage",
                "setTimeout(function() { document.getElementById('" + pnlMensaje.ClientID + "').style.display = 'none'; }, 5000);", true);
        }

        protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lógica opcional para recargar automáticamente
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Limpiar y recargar
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (Alumnos == null || Alumnos.Count == 0)
            {
                MostrarMensaje("No hay datos para exportar", "warning");
                return;
            }

            MostrarMensaje("Funcionalidad de exportación a Excel - En desarrollo", "info");
        }

        public class Alumno
        {
            public int Legajo { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int Id_Curso { get; set; }
            public string NombreCompleto { get; set; }
        }
    }
}