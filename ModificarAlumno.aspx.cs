using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace proyectoPracticaProfecional
{
    public partial class ModificarAlumno : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlFormulario.Visible = false;
                pnlResultados.Visible = false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscarAlumno.Text.Trim();

            if (!string.IsNullOrEmpty(criterio))
            {
                DataTable dt = BuscarAlumnos(criterio);

                if (dt.Rows.Count > 0)
                {
                    gvAlumnos.DataSource = dt;
                    gvAlumnos.DataBind();
                    pnlResultados.Visible = true;
                }
                else
                {
                    pnlResultados.Visible = false;
                    MostrarMensajeError("No se encontraron alumnos con ese criterio de búsqueda.");
                }
            }
            else
            {
                MostrarMensajeError("Por favor ingrese un criterio de búsqueda.");
            }
        }

        protected void gvAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string legajo = gvAlumnos.SelectedDataKey.Value.ToString();
            CargarDatosAlumno(legajo);
        }

        private DataTable BuscarAlumnos(string criterio)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("legajo");
            dt.Columns.Add("documento");
            dt.Columns.Add("apellido");
            dt.Columns.Add("nombre");

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string script = "SELECT legajo, dni as documento, apellido, nombre FROM Alumnos WHERE nombre LIKE '%' + @criterio + '%' OR apellido LIKE '%' + @criterio + '%' OR dni LIKE '%' + @criterio + '%' ORDER BY apellido, nombre";

                conexion.Open();

                using (SqlCommand command = new SqlCommand(script, conexion))
                {
                    command.Parameters.AddWithValue("@criterio", criterio);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dt.Rows.Add(
                                reader["legajo"].ToString(),
                                reader["documento"].ToString(),
                                reader["apellido"].ToString(),
                                reader["nombre"].ToString()
                            );
                        }
                    }
                }
            }

            return dt;
        }

        private void CargarDatosAlumno(string legajo)
        {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string query = "SELECT * FROM Alumnos WHERE legajo = @legajo";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@legajo", legajo);
                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Llenar los campos del formulario
                            txtNombre.Text = reader["nombre"].ToString();
                            txtApellido.Text = reader["apellido"].ToString();
                            txtDocumento.Text = reader["dni"].ToString();

                            if (reader["fecha_nac"] != DBNull.Value)
                                txtFechaNacimiento.Text = Convert.ToDateTime(reader["fecha_nac"]).ToString("yyyy-MM-dd");

                            ddlGenero.SelectedValue = reader["genero"].ToString();
                            txtDireccion.Text = reader["direccion"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            txtTelefono.Text = reader["telefono"].ToString();
                            ddlCarrera.SelectedValue = reader["carrera"].ToString();

                            if (reader["fecha_ingreso"] != DBNull.Value)
                                txtFechaInscripcion.Text = Convert.ToDateTime(reader["fecha_ingreso"]).ToString("yyyy-MM-dd");

                            lblIDAlumno.Text = legajo;
                            lblAlumnoSeleccionado.Text = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();

                            pnlFormulario.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    ActualizarAlumno();
                    MostrarMensajeExito("Alumno actualizado correctamente.");
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    MostrarMensajeError("Error al actualizar el alumno: " + ex.Message);
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                EliminarAlumno();
                MostrarMensajeExito("Alumno eliminado correctamente.");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar el alumno: " + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void ActualizarAlumno()
        {
            string legajo = lblIDAlumno.Text;

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string query = "UPDATE Alumnos SET nombre = @nombre, apellido = @apellido, dni = @dni, fecha_nac = @fechaNacimiento, genero = @genero, direccion = @direccion, email = @email, telefono = @telefono, carrera = @carrera, fecha_ingreso = @fechaInscripcion WHERE legajo = @legajo";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@legajo", legajo);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                    cmd.Parameters.AddWithValue("@dni", txtDocumento.Text);
                    cmd.Parameters.AddWithValue("@fechaNacimiento", Convert.ToDateTime(txtFechaNacimiento.Text));
                    cmd.Parameters.AddWithValue("@genero", ddlGenero.SelectedValue);
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text ?? "");
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text ?? "");
                    cmd.Parameters.AddWithValue("@carrera", ddlCarrera.SelectedValue);
                    cmd.Parameters.AddWithValue("@fechaInscripcion", Convert.ToDateTime(txtFechaInscripcion.Text));

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void EliminarAlumno()
        {
            string legajo = lblIDAlumno.Text;

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string query = "DELETE FROM Alumnos WHERE legajo = @legajo";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@legajo", legajo);
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LimpiarFormulario()
        {
            // Limpiar campos
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDocumento.Text = "";
            txtFechaNacimiento.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtDireccion.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlCarrera.SelectedIndex = 0;
            txtFechaInscripcion.Text = "";

            // Ocultar paneles
            pnlFormulario.Visible = false;
            pnlResultados.Visible = false;
            txtBuscarAlumno.Text = "";

            LimpiarMensajes();
        }

        private void MostrarMensajeExito(string mensaje)
        {
            pnlSuccessMessage.Visible = true;
            pnlErrorMessage.Visible = false;
            lblSuccessMessage.Text = mensaje;
        }

        private void MostrarMensajeError(string mensaje)
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = true;
            lblErrorMessage.Text = mensaje;
        }

        private void LimpiarMensajes()
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
            lblSuccessMessage.Text = "";
            lblErrorMessage.Text = "";
        }

        protected void ddlCarrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Método vacío - posiblemente para eventos futuros
        }
    }
}