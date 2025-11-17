using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace Instituto46
{
    public partial class Default : System.Web.UI.Page
    {
        string usuario;
        string password;
        string tipoUsuario;

        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización de la página
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Validar credenciales
            usuario = txtUsuario.Text.Trim();
            password = txtPassword.Text;

            // Primero intentar autenticar como personal
            if (AutenticarPersonal(usuario, password))
            {
                // Guardar sesiones comunes
                Session["Usuario"] = txtUsuario.Text;
                Session["tipo"] = tipoUsuario;
                Session["id_personal"] = idPersonal;

                // Redirección según tipo
                Response.Redirect("home.aspx");
            }
            // Si no es personal, intentar autenticar como alumno
            else if (AutenticarAlumno(usuario, password))
            {
                // Guardar sesiones para alumno
                Session["Usuario"] = txtUsuario.Text;
                Session["tipo"] = "alumno";
                Session["legajo_alumno"] = legajoAlumno;
                Session["nombre_alumno"] = nombreAlumno;

                // Redireccionar a página específica para alumnos
                Response.Redirect("home.aspx");
            }
            else
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "alert('El Usuario y/o Contraseña son incorrectos. Por Favor Intentelo Nuevamente.');", true);
            }
        }

        private string idPersonal = string.Empty;

        private bool AutenticarPersonal(string usuario, string password)
        {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string script = "SELECT ID_PERSONAL, NOMBRE, PASS, TIPO FROM PERSONAL WHERE NOMBRE = @usuario AND PASS = @password";

                conexion.Open();
                SqlCommand command = new SqlCommand(script, conexion);
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idPersonal = reader.GetInt32(0).ToString();
                        tipoUsuario = reader.GetString(3);
                    }
                    reader.Close();
                    return true;
                }
                reader.Close();
                return false;
            }
        }

        private string legajoAlumno = string.Empty;
        private string nombreAlumno = string.Empty;

        private bool AutenticarAlumno(string dni, string apellido)
        {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string script = @"
                    SELECT LEGAJO, NOMBRE, APELLIDO, DNI 
                    FROM ALUMNOS 
                    WHERE DNI = @dni AND APELLIDO = @apellido";

                conexion.Open();
                SqlCommand command = new SqlCommand(script, conexion);
                command.Parameters.AddWithValue("@dni", dni);
                command.Parameters.AddWithValue("@apellido", apellido);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        legajoAlumno = reader.GetInt32(0).ToString();
                        nombreAlumno = reader.GetString(1) + " " + reader.GetString(2);
                    }
                    reader.Close();
                    return true;
                }
                reader.Close();
                return false;
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            // Este método puede quedar vacío o eliminarse
        }
    }
}