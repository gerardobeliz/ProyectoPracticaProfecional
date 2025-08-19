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

            // Aquí iría la lógica de autenticación
            if (AutenticarUsuario(usuario, password)==true)
            {
                // Autenticación exitosa
                Session["UsuarioAutenticado"] = true;
                Session["NombreUsuario"] = usuario; // Cambiado de EmailUsuario a NombreUsuario
                Response.Redirect("home.aspx");
            }
            else
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "alert('El Usuario y/o Contraseña son incorrectos. Por Favor Intentelo Nuevamente.');", true);
            }
        }

        private bool AutenticarUsuario(string usuario, string password)
        {
            // Acá iría la conexión y la autenticación en la base de datos

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string script = String.Format("SELECT ID_USUARIO, NOMBRE, PASS FROM USUARIO WHERE NOMBRE = '{0}' AND PASS = '{1}'", txtUsuario.Text, txtPassword.Text);

                conexion.Open();

                SqlCommand command = new SqlCommand(script, conexion);

                SqlDataReader reader = command.ExecuteReader();

                string id = String.Empty;
               
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0).ToString();
                    }
                }

                reader.Close();
                conexion.Close();

                if (id != String.Empty)
                {

                    return (true); // Cambiado el usuario de ejemplo 
                }
                else
                    return (false); // Cambiado el usuario de ejemplo
            // Ejemplo básico de autenticación (deberás adaptarlo a tu BD real)
            }
        }
    }
}