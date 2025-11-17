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
                return; // Importante para detener la ejecución
            }//cierra el if de session

            if (!IsPostBack)
            {
                txtFechaInscripcion.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        // AGREGA este método para la validación del servidor
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

            if (telefono.Length > 11)
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

            // Validar que solo contenga números
            if (!Regex.IsMatch(documento, @"^\d+$"))
            {
                args.IsValid = false;
                return;
            }

            // Validar longitud exacta de 8 caracteres
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
        //---------------------
       
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
            return 0; // o -1 si no hay registros
        }
    }
    catch (Exception ex)
    {
        // Manejar el error
        return 0;
    }
}

// O si quieres obtener más información del último registro:
private void ObtenerUltimoRegistroCarreraProfe()
{
    try
    {
        using (SqlConnection conexion = new SqlConnection(Cadena))
        {
            string query = "SELECT TOP 1 legajo, carrera_nom FROM carrera_profe ORDER BY legajo DESC";

            conexion.Open();
            SqlCommand command = new SqlCommand(query, conexion);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int legajo = reader.GetInt32(0);
                string carrera = reader.GetString(1);

                // Usar los valores como necesites - CORREGIDO
                Console.WriteLine("Último legajo: " + legajo + ", Carrera: " + carrera);

                // O también puedes usar string.Format:
                // Console.WriteLine(string.Format("Último legajo: {0}, Carrera: {1}", legajo, carrera));
            }
            reader.Close();
        }
    }
    catch (Exception ex)
    {
        // Manejar el error
        Console.WriteLine("Error al obtener último registro: " + ex.Message);
    }
}

        //---------------------el boton guardar funciona ok---------------------------------------------
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                try
                {
                    int legajoGenerado = 0;

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
                    (dni, fecha_nac, fecha_ingreso, nombre, apellido, direccion,cp, telefono, genero, carrera, email) 
                    VALUES (@dni, @fecha_nac, GETDATE(), @nombre, @apellido, @direccion,1754, @telefono, @genero, @carrera, @email);
                    SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(insertAlumno, conexion))
                        {
                            cmd.Parameters.AddWithValue("@dni", txtDocumento.Text);
                            cmd.Parameters.AddWithValue("@fecha_nac", txtFechaNacimiento.Text);
                            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                            cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                            cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                            cmd.Parameters.AddWithValue("@telefono", numTel);
                            cmd.Parameters.AddWithValue("@genero", ddlGenero.Text);
                            cmd.Parameters.AddWithValue("@carrera", ddlCurso.Text);
                            cmd.Parameters.AddWithValue("@email", txtEmail.Text);

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
                            cmd.Parameters.AddWithValue("@curso", ddlCurso.Text);
                            cmd.Parameters.AddWithValue("@idPersonal", ObtenerUltimoLegajoCarreraProfe());

                            cmd.ExecuteNonQuery();
                        }
                    }

                    pnlSuccessMessage.Visible = true;
                    pnlErrorMessage.Visible = false;
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    pnlSuccessMessage.Visible = false;
                    pnlErrorMessage.Visible = true;
                    lblError.Text = "Error al guardar: " + ex.Message;
                }
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
            txtTelefono.Text = "";  // <-- Limpia también el teléfono
            txtFechaNacimiento.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtDireccion.Text = "";
            txtDocumento.Text = "";
            ddlCurso.SelectedIndex = 0;
            txtFechaInscripcion.Text = DateTime.Today.ToString("yyyy-MM-dd");
            //chkActivo.Checked = true;
        }
    }
}


