using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoPracticaProfecional
{
    public partial class altaUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Código de inicialización si es necesario
        }

        // Valida que el usuario tenga al menos 4 caracteres
        protected void ValidarLongitudUsuario(object source, ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Length >= 4;
        }

        // Valida que la contraseña tenga al menos 6 caracteres
        protected void ValidarLongitudPassword(object source, ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Length >= 6;
        }

        // Evento click del botón Registrar
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Obtener los valores del formulario
                    string nombre = txtNombre.Text;
                    string email = txtEmail.Text;
                    string usuario = txtUsuario.Text;
                    string password = txtPassword.Text;
                    string tipoUsuario = ddlTipoUsuario.SelectedValue;

                    // Aquí iría el código para registrar el usuario en la base de datos
                    bool registroExitoso = RegistrarUsuario(nombre, email, usuario, password, tipoUsuario);

                    if (registroExitoso)
                    {
                        // Registro exitoso - redirigir o mostrar mensaje
                        Response.Redirect("RegistroExitoso.aspx");
                    }
                    else
                    {
                        MostrarError("Ocurrió un error al registrar el usuario");
                    }
                }
                catch (Exception ex)
                {
                    // Registrar el error y mostrar mensaje al usuario
                    MostrarError("Error: " + ex.Message);
                }
            }
        }

        private bool RegistrarUsuario(string nombre, string email, string usuario, string password, string tipoUsuario)
        {
            // Implementar lógica de registro en base de datos
            // Retornar true si fue exitoso, false si falló
            
            // EJEMPLO (debes implementar tu propia lógica):
            /*
            using (var conexion = new SqlConnection("tu_cadena_conexion"))
            {
                var comando = new SqlCommand("INSERT INTO Usuarios (...) VALUES (...)", conexion);
                // Agregar parámetros...
                conexion.Open();
                int resultado = comando.ExecuteNonQuery();
                return resultado > 0;
            }
            */
            
            return true; // Cambiar por implementación real
        }

        private void MostrarError(string mensaje)
        {
            // Mostrar mensaje de error al usuario
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarError", 
                "alert('{mensaje}');", true);
        }
    }
}