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
                pnlBotonesAccion.Visible = false;
                pnlResumen.Visible = false;
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
                    string query = "SELECT DISTINCT curso FROM CURSOS ORDER BY curso";
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

                if (Alumnos.Count == 0)
                {
                    MostrarMensaje("No se encontraron alumnos para el curso seleccionado", "warning");
                    pnlEmpty.Visible = true;
                    gvAsistencia.Visible = false;
                    pnlBotonesAccion.Visible = false;
                    pnlResumen.Visible = false;
                    return;
                }

                // Generar la tabla de asistencia
                GenerarGridAsistencia();

                // Mostrar controles
                pnlEmpty.Visible = false;
                gvAsistencia.Visible = true;
                pnlBotonesAccion.Visible = true;
                pnlResumen.Visible = true;

                MostrarMensaje("Asistencia cargada para " + Alumnos.Count.ToString() + " alumnos del curso " + ddlCurso.SelectedItem.Text, "success");
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
        }

        private void GenerarGridAsistencia()
        {
            if (FechaSeleccionada == null || Alumnos == null || Alumnos.Count == 0)
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
                row["EstadoAsistencia"] = estadoExistente;
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
                    // Configurar el valor seleccionado desde el DataItem
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string estadoActual = rowView["EstadoAsistencia"].ToString();

                    if (!string.IsNullOrEmpty(estadoActual))
                    {
                        ddlEstadoAsistencia.SelectedValue = estadoActual;

                        // Aplicar estilo inmediatamente via JavaScript
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "estilo_" + e.Row.RowIndex.ToString(),
                            "setTimeout(function() { aplicarEstiloDropdown(document.getElementById('" + ddlEstadoAsistencia.ClientID + "')); }, 100);",
                            true);
                    }

                    // Agregar evento JavaScript
                    ddlEstadoAsistencia.Attributes["onchange"] = "aplicarEstiloDropdown(this); actualizarResumen();";
                }
            }
        }

        protected void gvAsistencia_DataBound(object sender, EventArgs e)
        {
            // Asegurar que los dropdowns muestren el estilo correcto después del databind
            ScriptManager.RegisterStartupScript(this, GetType(), "inicializarDropdowns",
                "setTimeout(function() { inicializarDropdowns(); actualizarResumen(); }, 200);", true);
        }

        private string ObtenerEstadoAsistencia(int legajo, DateTime fecha)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();
                    string query = "SELECT PRESENTE FROM ASISTENCIA WHERE LEGAJO = @Legajo AND FECHA = @Fecha";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Legajo", legajo);
                        cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                        object resultado = cmd.ExecuteScalar();
                        if (resultado != null && resultado != DBNull.Value)
                        {
                            int estadoBD = Convert.ToInt32(resultado);

                            // Mapear INT de BD a STRING para el dropdown
                            switch (estadoBD)
                            {
                                case 1: return "P"; // Presente
                                case 0: return "A"; // Ausente
                                case 2: return "J"; // Justificado
                                default: return ""; // Valor desconocido
                            }
                        }
                        else
                        {
                            return ""; // No existe registro
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener asistencia - Legajo: " + legajo.ToString() + ", Fecha: " + fecha.ToString("yyyy-MM-dd") + ", Error: " + ex.Message);
                return "";
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
                List<string> errores = new List<string>();

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

                            // Solo guardar si se seleccionó un estado
                            if (alumno != null && !string.IsNullOrEmpty(estado))
                            {
                                if (GuardarAsistenciaBD(alumno.Legajo, alumno.Id_Curso, FechaSeleccionada.Value, estado))
                                    registrosGuardados++;
                                else
                                {
                                    registrosConError++;
                                    errores.Add("Alumno: " + alumno.NombreCompleto);
                                }
                            }
                        }
                    }
                }

                // Mostrar mensaje según resultados
                if (registrosGuardados > 0)
                {
                    string mensaje = "✅ Asistencia guardada correctamente. " + registrosGuardados.ToString() + " registros actualizados.";
                    if (registrosConError > 0)
                        mensaje += " ❌ " + registrosConError.ToString() + " registros con errores.";

                    MostrarMensaje(mensaje, "success");

                    // Recargar los datos para mostrar los valores guardados
                    GenerarGridAsistencia();
                }
                else
                {
                    MostrarMensaje("No se guardó ninguna asistencia. Verifique que haya seleccionado estados.", "warning");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar asistencia: " + ex.Message, "error");
            }
        }

        private bool GuardarAsistenciaBD(int legajo, int id_Curso, DateTime fecha, string estado)
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
                            cmd.Parameters.AddWithValue("@IdCurso", id_Curso);
                            cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                            // Convertir estado STRING a INT para la base de datos
                            int estadoBD = 0; // Por defecto ausente
                            switch (estado)
                            {
                                case "P": estadoBD = 1; break; // Presente
                                case "A": estadoBD = 0; break; // Ausente
                                case "J": estadoBD = 2; break; // Justificado
                                default: estadoBD = 0; break; // Por defecto ausente
                            }

                            cmd.Parameters.AddWithValue("@Presente", estadoBD);

                            int result = cmd.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine("Guardado - Legajo: " + legajo.ToString() + ", Estado: " + estado + "->" + estadoBD.ToString() + ", Filas: " + result.ToString());
                            return result > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error BD al guardar asistencia - Legajo: " + legajo.ToString() + ", Error: " + ex.Message);
                return false;
            }
        }

        private void ActualizarContadoresResumen()
        {
            if (Alumnos == null || Alumnos.Count == 0 || FechaSeleccionada == null) return;

            int presentes = 0;
            int ausentes = 0;
            int justificados = 0;

            // Contar desde los datos actuales del GridView (no de la BD)
            foreach (GridViewRow row in gvAsistencia.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlEstadoAsistencia = (DropDownList)row.FindControl("ddlEstadoAsistencia");
                    if (ddlEstadoAsistencia != null)
                    {
                        switch (ddlEstadoAsistencia.SelectedValue)
                        {
                            case "P": presentes++; break;
                            case "A": ausentes++; break;
                            case "J": justificados++; break;
                        }
                    }
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
                    pnlMensaje.CssClass = "mensaje-exito";
                    break;
                case "error":
                    pnlMensaje.CssClass = "mensaje-error";
                    break;
                case "warning":
                    pnlMensaje.CssClass = "mensaje-error"; // Usar mismo estilo para warning
                    break;
                case "info":
                    pnlMensaje.CssClass = "mensaje-info";
                    break;
                default:
                    pnlMensaje.CssClass = "mensaje-exito";
                    break;
            }

            // Ocultar mensaje después de 5 segundos
            ScriptManager.RegisterStartupScript(this, GetType(), "hideMessage",
                "setTimeout(function() { var panel = document.getElementById('" + pnlMensaje.ClientID + "'); if(panel) panel.style.display = 'none'; }, 5000);", true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Limpiar sesiones
            Session.Remove("Alumnos");
            Session.Remove("FechaSeleccionada");

            // Recargar página
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Alumnos == null || Alumnos.Count == 0)
                {
                    MostrarMensaje("No hay datos para exportar", "warning");
                    return;
                }

                // Aquí iría la lógica de exportación a Excel
                // Por ahora mostramos un mensaje
                MostrarMensaje("La funcionalidad de exportación a Excel está en desarrollo", "info");

                // TODO: Implementar exportación a Excel
                // ExportarAsistenciaExcel();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al exportar a Excel: " + ex.Message, "error");
            }
        }

        // Método para debug (opcional)
        private void DebugAsistencia()
        {
            System.Diagnostics.Debug.WriteLine("=== DEBUG ASISTENCIA ===");
            System.Diagnostics.Debug.WriteLine("Alumnos count: " + (Alumnos != null ? Alumnos.Count.ToString() : "0"));
            System.Diagnostics.Debug.WriteLine("Fecha: " + FechaSeleccionada.ToString());
            System.Diagnostics.Debug.WriteLine("Curso: " + ddlCurso.SelectedValue);

            if (Alumnos != null && FechaSeleccionada != null)
            {
                foreach (var alumno in Alumnos)
                {
                    string estado = ObtenerEstadoAsistencia(alumno.Legajo, FechaSeleccionada.Value);
                    System.Diagnostics.Debug.WriteLine("Alumno: " + alumno.NombreCompleto + ", Legajo: " + alumno.Legajo.ToString() + ", Estado BD: " + estado);
                }
            }
            System.Diagnostics.Debug.WriteLine("=== FIN DEBUG ===");
        }

        // Clase Alumno
        public class Alumno
        {
            public int Legajo { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int Id_Curso { get; set; }
            public string NombreCompleto { get; set; }
        }

        // Método para limpiar controles (opcional)
        private void LimpiarControles()
        {
            pnlEmpty.Visible = true;
            gvAsistencia.Visible = false;
            pnlMensaje.Visible = false;
            pnlFechaSeleccionada.Visible = false;
            pnlBotonesAccion.Visible = false;
            pnlResumen.Visible = false;
            Alumnos = new List<Alumno>();
            FechaSeleccionada = null;
        }
    }
}