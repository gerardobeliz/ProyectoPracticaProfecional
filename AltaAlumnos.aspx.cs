using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace proyectoPracticaProfecional
{
    public partial class AltaAlumnos : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipo"] != null && Convert.ToString(Session["tipo"]) != "preceptor")
            {
                Response.Redirect("default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtFechaInscripcion.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        // VALIDACIÓN SERVIDOR para Teléfono
        protected void cvTelefono_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string telefono = args.Value;

            if (string.IsNullOrEmpty(telefono))
            {
                args.IsValid = true;
                return;
            }

            if (!Regex.IsMatch(telefono, @"^\d+$"))
            {
                args.IsValid = false;
                return;
            }

            if (telefono.Length > 10)
            {
                args.IsValid = false;
                return;
            }

            args.IsValid = true;
        }

        // VALIDACIÓN SERVIDOR para Documento/DNI
        protected void cvDocumento_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string documento = args.Value;

            if (!Regex.IsMatch(documento, @"^\d+$"))
            {
                args.IsValid = false;
                return;
            }

            if (documento.Length != 8)
            {
                args.IsValid = false;
                return;
            }

            args.IsValid = true;
        }

        public int ObtenerUltimoLegajo()
        {
            int ultimoLegajo = 0;
            string query = "SELECT MAX(legajo) AS ultimo_legajo FROM alumnos";

            using (SqlConnection conn = new SqlConnection(Cadena))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        ultimoLegajo = Convert.ToInt32(result);
                    }
                }
            }
            return ultimoLegajo;
        }

        // Método para obtener el último legajo insertado en carrera_profe
        private int ObtenerUltimoLegajoCarreraProfe()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    string query = "SELECT TOP 1 legajo FROM carrera_profe ORDER BY legajo DESC";

                    conexion.Open();
                    SqlCommand command = new SqlCommand(query, conexion);
                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener último legajo de carrera_profe: " + ex.Message);
                return 1;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                try
                {
                    int legajoGenerado = 0;
                    string nombreAlumno = txtNombre.Text.Trim();
                    string apellidoAlumno = txtApellido.Text.Trim();
                    string emailAlumno = txtEmail.Text.Trim();
                    string carreraAlumno = ddlCurso.SelectedItem.Text;

                    // PRIMERO: Insertar en ALUMNOS
                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        conexion.Open();

                        int numTel = 0;
                        if (!string.IsNullOrEmpty(txtTelefono.Text))
                        {
                            int.TryParse(txtTelefono.Text, out numTel);
                        }

                        string insertAlumno = @"INSERT INTO ALUMNOS 
                    (dni, fecha_nac, fecha_ingreso, nombre, apellido, direccion, cp, telefono, genero, carrera, email) 
                    VALUES (@dni, @fecha_nac, GETDATE(), @nombre, @apellido, @direccion, 1754, @telefono, @genero, @carrera, @email);
                    SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(insertAlumno, conexion))
                        {
                            cmd.Parameters.AddWithValue("@dni", txtDocumento.Text);
                            cmd.Parameters.AddWithValue("@fecha_nac", txtFechaNacimiento.Text);
                            cmd.Parameters.AddWithValue("@nombre", nombreAlumno);
                            cmd.Parameters.AddWithValue("@apellido", apellidoAlumno);
                            cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text ?? "");
                            cmd.Parameters.AddWithValue("@telefono", numTel);
                            cmd.Parameters.AddWithValue("@genero", ddlGenero.SelectedValue);
                            cmd.Parameters.AddWithValue("@carrera", carreraAlumno);
                            cmd.Parameters.AddWithValue("@email", emailAlumno);

                            legajoGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }

                    // SEGUNDO: Insertar en CURSOS
                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        conexion.Open();

                        string insertCurso = "INSERT INTO CURSOS (legajo, curso, id_personal) VALUES (@legajo, @curso, @idPersonal)";

                        using (SqlCommand cmd = new SqlCommand(insertCurso, conexion))
                        {
                            cmd.Parameters.AddWithValue("@legajo", legajoGenerado);
                            cmd.Parameters.AddWithValue("@curso", carreraAlumno);
                            cmd.Parameters.AddWithValue("@idPersonal", ObtenerUltimoLegajoCarreraProfe());

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // MOSTRAR MODAL DE CONFIRMACIÓN - CORREGIDO SIN $
                    string nombreCompleto = nombreAlumno + " " + apellidoAlumno;

                    // Escapar comillas simples para JavaScript
                    nombreCompleto = nombreCompleto.Replace("'", "\\'");
                    emailAlumno = emailAlumno.Replace("'", "\\'");
                    carreraAlumno = carreraAlumno.Replace("'", "\\'");

                    // Usar string.Format en lugar de interpolación con $
                    string script = string.Format("mostrarModalConfirmacion('{0}', '{1}', '{2}', '{3}');",
                        nombreCompleto, emailAlumno, carreraAlumno, legajoGenerado);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", script, true);

                    pnlSuccessMessage.Visible = true;
                    pnlErrorMessage.Visible = false;

                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    pnlSuccessMessage.Visible = false;
                    pnlErrorMessage.Visible = true;
                    lblError.Text = "Error al guardar: " + ex.Message;

                    System.Diagnostics.Debug.WriteLine("Error completo: " + ex.ToString());
                }
            }
            else
            {
                pnlSuccessMessage.Visible = false;
                pnlErrorMessage.Visible = true;
                lblError.Text = "Por favor, complete todos los campos requeridos correctamente.";
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtFechaNacimiento.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtDireccion.Text = "";
            txtDocumento.Text = "";
            ddlCurso.SelectedIndex = 0;
            txtFechaInscripcion.Text = DateTime.Today.ToString("yyyy-MM-dd");

            foreach (IValidator validator in Page.Validators)
            {
                validator.IsValid = true;
            }
        }
    }
}