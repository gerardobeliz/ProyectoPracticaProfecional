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

        //protected void btnLogin_Click(object sender, EventArgs e)
        //{
        //    // Validar credenciales
        //    usuario = txtUsuario.Text.Trim();
        //    password = txtPassword.Text;

        //    // Aquí iría la lógica de autenticación
        //    if (AutenticarUsuario(usuario, password) == true)
        //    // Autenticación exitosa
        //    {

        //        if (tipoUsuario == "directivo")
        //        {

        //            Session["Usuario"] = txtUsuario.Text;
        //            Session["tipo"] = tipoUsuario;
        //            Response.Redirect("home.aspx");

        //        }
        //        if (tipoUsuario == "profesor")
        //        {
        //            Session["Usuario"] = txtUsuario.Text;
        //            Session["tipo"] = tipoUsuario;
        //            Response.Redirect("home.aspx");

        //        }
        //        if (tipoUsuario == "preceptor")
        //        {
        //            Session["Usuario"] = txtUsuario.Text;
        //            Session["tipo"] = tipoUsuario;
        //            Response.Redirect("home.aspx");
        //        }

        //    }
        //    else
        //    {
        //        // Mostrar mensaje de error
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //            "alert('El Usuario y/o Contraseña son incorrectos. Por Favor Intentelo Nuevamente.');", true);
        //    }
        //}

        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Validar credenciales
            usuario = txtUsuario.Text.Trim();
            password = txtPassword.Text;

            // Aquí iría la lógica de autenticación
            if (AutenticarUsuario(usuario, password) == true) // Autenticación exitosa
            {
                // Guardar sesiones comunes
                Session["Usuario"] = txtUsuario.Text;
                Session["tipo"] = tipoUsuario;

                // Guardar ID_PERSONAL en sesión
                Session["id_personal"] = idPersonal; // idPersonal se define en AutenticarUsuario

                // Redirección según tipo
                if (tipoUsuario == "directivo")
                {
                    Response.Redirect("home.aspx");
                }
                else if (tipoUsuario == "profesor")
                {
                    Response.Redirect("home.aspx");
                }
                else if (tipoUsuario == "preceptor")
                {
                    Response.Redirect("home.aspx");
                }
            }
            else
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "alert('El Usuario y/o Contraseña son incorrectos. Por Favor Intentelo Nuevamente.');", true);
            }
        }

        private string idPersonal = string.Empty; // <-- Variable para guardar ID_PERSONAL

        private bool AutenticarUsuario(string usuario, string password)
        {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string script = String.Format("SELECT ID_PERSONAL, NOMBRE, PASS, TIPO FROM PERSONAL WHERE NOMBRE = '{0}' AND PASS = '{1}'", txtUsuario.Text, txtPassword.Text);

                conexion.Open();
                SqlCommand command = new SqlCommand(script, conexion);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idPersonal = reader.GetInt32(0).ToString(); // <-- Guardar ID_PERSONAL
                        tipoUsuario = reader.GetString(3);
                    }
                }

                reader.Close();
                conexion.Close();

                return !string.IsNullOrEmpty(idPersonal);
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {

        }
    }
}