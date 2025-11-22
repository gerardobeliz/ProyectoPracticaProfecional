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
                    OcultarMensajes();
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
            OcultarMensajes();
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
            try
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

                                // Cargar carrera en TextBox
                                txtCarrera.Text = reader["carrera"].ToString();

                                if (reader["fecha_ingreso"] != DBNull.Value)
                                    txtFechaInscripcion.Text = Convert.ToDateTime(reader["fecha_ingreso"]).ToString("yyyy-MM-dd");

                                lblIDAlumno.Text = legajo;
                                lblAlumnoSeleccionado.Text = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();

                                pnlFormulario.Visible = true;
                                pnlResultados.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar datos del alumno: " + ex.Message);
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
             if (Page.IsValid)
            {
                try
                {
                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        string query = @"UPDATE Alumnos SET 
                                       nombre = @nombre, 
                                       apellido = @apellido, 
                                       dni = @dni, 
                                       fecha_nac = @fecha_nac, 
                                       genero = @genero, 
                                       direccion = @direccion, 
                                       email = @email, 
                                       telefono = @telefono, 
                                       carrera = @carrera, 
                                       fecha_ingreso = @fecha_ingreso 
                                       WHERE legajo = @legajo";

                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@legajo", lblIDAlumno.Text);
                            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                            cmd.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim());
                            cmd.Parameters.AddWithValue("@dni", txtDocumento.Text.Trim());
                            cmd.Parameters.AddWithValue("@fecha_nac", Convert.ToDateTime(txtFechaNacimiento.Text));
                            cmd.Parameters.AddWithValue("@genero", ddlGenero.SelectedValue);
                            cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
                            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                            cmd.Parameters.AddWithValue("@carrera", txtCarrera.Text);
                            cmd.Parameters.AddWithValue("@fecha_ingreso", Convert.ToDateTime(txtFechaInscripcion.Text));

                            conexion.Open();
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                               
                                LimpiarFormulario();
                            }
                            else
                            {
                                MostrarMensajeError("No se pudo actualizar el alumno.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensajeError("Error al actualizar alumno: " + ex.Message);
                }
            }
        

                bool actualizado = true; // Cambiar por tu lógica real

                if (actualizado)
                {
                    // Configurar el hidden field para mostrar el mensaje
                    hdnMostrarMensaje.Value = "true";

                    // Opcional: Limpiar el formulario o hacer otras acciones
                    // LimpiarFormulario();

                    // También puedes mostrar el panel de éxito tradicional
                    pnlSuccessMessage.Visible = true;
                    lblSuccessMessage.Text = "Los datos del alumno se han actualizado correctamente.";
                    pnlErrorMessage.Visible = false;
                }
                else
                {
                    pnlErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Error al actualizar los datos del alumno.";
                    pnlSuccessMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pnlErrorMessage.Visible = true;
                lblErrorMessage.Text = "Error: " + ex.Message;
                pnlSuccessMessage.Visible = false;
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string legajo = lblIDAlumno.Text;

                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();
                    using (SqlTransaction transaction = conexion.BeginTransaction())
                    {
                        try
                        {
                            // 1. PRIMERO: Eliminar los registros relacionados en la tabla CURSOS
                            string deleteCursos = "DELETE FROM CURSOS WHERE LEGAJO = @legajo";
                            using (SqlCommand cmdCursos = new SqlCommand(deleteCursos, conexion, transaction))
                            {
                                cmdCursos.Parameters.AddWithValue("@legajo", legajo);
                                int cursosEliminados = cmdCursos.ExecuteNonQuery();
                            }

                            // 2. LUEGO: Eliminar el alumno de la tabla Alumnos
                            string deleteAlumno = "DELETE FROM Alumnos WHERE legajo = @legajo";
                            using (SqlCommand cmdAlumno = new SqlCommand(deleteAlumno, conexion, transaction))
                            {
                                cmdAlumno.Parameters.AddWithValue("@legajo", legajo);
                                int result = cmdAlumno.ExecuteNonQuery();

                                if (result > 0)
                                {
                                    transaction.Commit();
                                    MostrarMensajeExito("Alumno y todos sus cursos asociados eliminados correctamente.");
                                    LimpiarFormulario();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MostrarMensajeError("No se pudo eliminar el alumno.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error en la eliminación en cascada: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar alumno: " + ex.Message);
            }
        }

        // MÉTODO QUE FALTABA - btnCancelar_Click
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDocumento.Text = "";
            txtFechaNacimiento.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtDireccion.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtCarrera.Text = "";
            txtFechaInscripcion.Text = "";
            lblIDAlumno.Text = "";
            lblAlumnoSeleccionado.Text = "";
            pnlFormulario.Visible = false;
            pnlResultados.Visible = false;
            txtBuscarAlumno.Text = "";

            // Ocultar mensajes
            OcultarMensajes();
        }

        private void MostrarMensajeExito(string mensaje)
        {
            pnlSuccessMessage.Visible = true;
            pnlErrorMessage.Visible = false;
            lblSuccessMessage.Text = mensaje;
        }

        private void MostrarMensajeError(string mensaje)
        {
            pnlErrorMessage.Visible = true;
            pnlSuccessMessage.Visible = false;
            lblErrorMessage.Text = mensaje;
        }

        private void OcultarMensajes()
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
        }
    }
}