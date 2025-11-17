using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace proyectoPracticaProfecional
{
    public partial class AltaPersonal : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer fecha actual en fecha de ingreso
                txtFechaIngreso.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Cargar las carreras desde la base de datos
                CargarCarreras();
            }
        }

        // Método para cargar las carreras desde la tabla CURSOS
        private void CargarCarreras()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    // Query usando DISTINCT para obtener carreras únicas
                    string query = "SELECT DISTINCT Curso FROM CURSOS WHERE Curso IS NOT NULL AND Curso != '' ORDER BY Curso";

                    conexion.Open();
                    SqlCommand command = new SqlCommand(query, conexion);
                    SqlDataReader reader = command.ExecuteReader();

                    // Asignar los datos al DropDownList
                    ddlCarrera.DataSource = reader;
                    ddlCarrera.DataTextField = "Curso";
                    ddlCarrera.DataValueField = "Curso";
                    ddlCarrera.DataBind();

                    reader.Close();

                    // Agregar item por defecto después del bind
                    ddlCarrera.Items.Insert(0, new ListItem("Seleccione carrera", ""));
                }
            }
            catch (Exception ex)
            {
                // Manejar error silenciosamente o mostrar mensaje
                pnlErrorMessage.Visible = true;
                lblError.Text = "Error al cargar las carreras: " + ex.Message;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    // Aquí la lógica para guardar en la base de datos
                    try
                    {
                        using (SqlConnection conexion = new SqlConnection(Cadena))
                        {
                            conexion.Open();

                            int numTel;
                            numTel = int.Parse(txtTelefono.Text);

                            // PRIMER INSERT: Guardar en tabla PERSONAL
                            string script = String.Format("INSERT INTO PERSONAL (Tipo, dni, Fecha_nac, Fecha_ingreso, Nombre, Apellido, Direccion, CP, Tel, Email, Pass, Usuario) VALUES ('{0}','{1}','{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}','{9}','{10}','{11}'); SELECT SCOPE_IDENTITY();",
                                ddlTipoPersonal.SelectedValue, txtDocumento.Text, txtFechaNacimiento.Text, txtFechaIngreso.Text,
                                txtNombre.Text, txtApellido.Text, txtDireccion.Text, txtCodigoPostal.Text, numTel,
                                txtEmail.Text, txtPassword.Text, txtUsuario.Text);

                            SqlCommand command = new SqlCommand(script, conexion);

                            // Ejecutar y obtener el legajo (ID) generado
                            int legajo = Convert.ToInt32(command.ExecuteScalar());

                            // SEGUNDO INSERT: Guardar en tabla carrera_profe
                            string scriptCarreraProfe = "INSERT INTO carrera_profe (legajo, carrera_nom) VALUES (@legajo, @carrera_nom)";

                            SqlCommand commandCarreraProfe = new SqlCommand(scriptCarreraProfe, conexion);
                            commandCarreraProfe.Parameters.AddWithValue("@legajo", legajo);
                            commandCarreraProfe.Parameters.AddWithValue("@carrera_nom", ddlCarrera.SelectedValue);

                            commandCarreraProfe.ExecuteNonQuery();

                            conexion.Close();

                            pnlSuccessMessage.Visible = true;
                            pnlErrorMessage.Visible = false;

                            // Mostrar modal de confirmación (actualizado con carrera)
                            string script2 = string.Format("showConfirmation('{0}', '{1}', '{2}', '{3}', '{4}');",
                                txtNombre.Text + " " + txtApellido.Text,
                                ddlTipoPersonal.SelectedItem.Text,
                                ddlCarrera.SelectedItem.Text,
                                txtEmail.Text,
                                txtFechaIngreso.Text);

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showConfirmation", script2, true);

                            // Limpiar formulario después de guardar
                            LimpiarFormulario();
                        }
                    }
                    catch (Exception ex)
                    {
                        pnlSuccessMessage.Visible = false;
                        pnlErrorMessage.Visible = true;
                        lblError.Text = "Error al guardar en la base de datos: " + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                pnlSuccessMessage.Visible = false;
                pnlErrorMessage.Visible = true;
                lblError.Text = "Error al guardar el personal: " + ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            // Limpiar todos los campos excepto fecha de ingreso
            ddlTipoPersonal.SelectedIndex = 0;
            txtDocumento.Text = "";
            // Mantener fecha de ingreso actual
            txtFechaIngreso.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtFechaNacimiento.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtDireccion.Text = "";
            txtCodigoPostal.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtUsuario.Text = "";

            // Resetear carrera al item por defecto
            if (ddlCarrera.Items.Count > 0)
            {
                ddlCarrera.SelectedIndex = 0;
            }

            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
        }

        // Validadores personalizados
        protected void cvDocumento_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validar que el DNI tenga exactamente 8 dígitos numéricos
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                return;
            }

            args.IsValid = System.Text.RegularExpressions.Regex.IsMatch(args.Value, @"^\d{8}$");
        }

        protected void cvTelefono_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validar que el teléfono contenga solo números
            if (!string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = System.Text.RegularExpressions.Regex.IsMatch(args.Value, @"^\d+$");
            }
            else
            {
                args.IsValid = true; // No es requerido
            }
        }

        protected void cvPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validar que la contraseña tenga al menos 8 caracteres
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                return;
            }

            args.IsValid = args.Value.Length >= 8;
        }

        // Método para validar si el usuario ya existe
        protected void cvUsuario_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();

                // Validación de longitud mínima
                if (usuario.Length < 4)
                {
                    args.IsValid = false;
                    cvUsuario.ErrorMessage = "El usuario debe tener al menos 4 caracteres";
                    return;
                }

                // Aquí implementarías la lógica real para verificar si el usuario existe en la base de datos
                bool usuarioExiste = VerificarUsuarioExistente(usuario);

                if (usuarioExiste)
                {
                    args.IsValid = false;
                    cvUsuario.ErrorMessage = "El nombre de usuario ya está en uso";
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                cvUsuario.ErrorMessage = "Error al validar el usuario";
            }
        }

        // Método para verificar usuario existente
        private bool VerificarUsuarioExistente(string usuario)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    string query = "SELECT COUNT(*) FROM PERSONAL WHERE Usuario = @Usuario";

                    conexion.Open();
                    SqlCommand command = new SqlCommand(query, conexion);
                    command.Parameters.AddWithValue("@Usuario", usuario);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // En caso de error, asumimos que el usuario no existe para no bloquear el registro
                return false;
            }
        }

        // Método opcional: Verificar si el DNI ya existe
        protected void cvDocumentoExistente_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string documento = txtDocumento.Text.Trim();

                if (string.IsNullOrEmpty(documento))
                {
                    args.IsValid = true;
                    return;
                }

                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    string query = "SELECT COUNT(*) FROM PERSONAL WHERE Documento = @Documento";

                    conexion.Open();
                    SqlCommand command = new SqlCommand(query, conexion);
                    command.Parameters.AddWithValue("@Documento", documento);

                    int count = (int)command.ExecuteScalar();

                    args.IsValid = count == 0;

                    if (count > 0)
                    {
                        cvDocumento.ErrorMessage = "El DNI ya está registrado en el sistema";
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, asumimos que el DNI no existe para no bloquear el registro
                args.IsValid = true;
            }
        }
    }
}

//namespace proyectoPracticaProfecional
//{
//    public partial class AltaPersonal : System.Web.UI.Page
//    {
//        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                // Establecer fecha actual en fecha de ingreso
//                txtFechaIngreso.Text = DateTime.Now.ToString("yyyy-MM-dd");
//            }
//        }

//        protected void btnGuardar_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (Page.IsValid)
//                {
//                    // Aquí la lógica para guardar en la base de datos
//                    try
//                    {
//                        using (SqlConnection conexion = new SqlConnection(Cadena))
//                        {

//                            int numTel;
//                            numTel = int.Parse(txtTelefono.Text);
//                            string script = String.Format("INSERT INTO PERSONAL VALUES ('{0}','{1}','{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}','{9}','{10}','{11}')",
//                                ddlTipoPersonal.SelectedValue, txtDocumento.Text, txtFechaNacimiento.Text, txtFechaIngreso.Text, txtNombre.Text, txtApellido.Text,
//                                txtDireccion.Text, txtCodigoPostal.Text, numTel, txtEmail.Text, txtPassword.Text, txtUsuario.Text);

//                            conexion.Open();
//                            SqlCommand command = new SqlCommand(script, conexion);
//                            SqlDataReader reader = command.ExecuteReader();
//                            reader.Close();
//                            conexion.Close();

//                            pnlSuccessMessage.Visible = true;
//                            pnlErrorMessage.Visible = false;
//                        }

//                    }
//                    catch (Exception ex)
//                    {
//                        pnlSuccessMessage.Visible = false;
//                        pnlErrorMessage.Visible = true;
//                        lblError.Text = ex.Message;
//                    }
                                  
//                    // Mostrar modal de confirmación
//                    string script2 = string.Format("showConfirmation('{0}', '{1}', '{2}', '{3}');",
//                        txtNombre.Text + " " + txtApellido.Text,
//                        ddlTipoPersonal.SelectedItem.Text,
//                        txtEmail.Text,
//                        txtFechaIngreso.Text);

//                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showConfirmation", script2, true);
                    
//                    // Limpiar formulario después de guardar
//                    LimpiarFormulario();
//                }
//            }
//            catch (Exception ex)
//            {
//                pnlSuccessMessage.Visible = false;
//                pnlErrorMessage.Visible = true;
//                lblError.Text = "Error al guardar el personal: " + ex.Message;
//            }
//        }

//        protected void btnLimpiar_Click(object sender, EventArgs e)
//        {
//            LimpiarFormulario();
//        }

//        private void LimpiarFormulario()
//        {
//            // Limpiar todos los campos excepto fecha de ingreso
//            ddlTipoPersonal.SelectedIndex = 0;
//            txtDocumento.Text = "";
//            // Mantener fecha de ingreso actual
//            txtFechaIngreso.Text = DateTime.Now.ToString("yyyy-MM-dd");
//            txtNombre.Text = "";
//            txtApellido.Text = "";
//            txtFechaNacimiento.Text = "";
//            ddlGenero.SelectedIndex = 0;
//            txtDireccion.Text = "";
//            txtCodigoPostal.Text = "";
//            txtTelefono.Text = "";
//            txtEmail.Text = "";
//            txtPassword.Text = "";
//            txtConfirmPassword.Text = "";
//            txtUsuario.Text = "";
//            pnlSuccessMessage.Visible = false;
//            pnlErrorMessage.Visible = false;
//        }

//        // Validadores personalizados
//        protected void cvDocumento_ServerValidate(object source, ServerValidateEventArgs args)
//        {
//            // Validar que el DNI tenga exactamente 8 dígitos numéricos
//            if (string.IsNullOrEmpty(args.Value))
//            {
//                args.IsValid = false;
//                return;
//            }
            
//            args.IsValid = System.Text.RegularExpressions.Regex.IsMatch(args.Value, @"^\d{8}$");
//        }

//        protected void cvTelefono_ServerValidate(object source, ServerValidateEventArgs args)
//        {
//            // Validar que el teléfono contenga solo números
//            if (!string.IsNullOrEmpty(args.Value))
//            {
//                args.IsValid = System.Text.RegularExpressions.Regex.IsMatch(args.Value, @"^\d+$");
//            }
//            else
//            {
//                args.IsValid = true; // No es requerido
//            }
//        }

//        protected void cvPassword_ServerValidate(object source, ServerValidateEventArgs args)
//        {
//            // Validar que la contraseña tenga al menos 8 caracteres
//            if (string.IsNullOrEmpty(args.Value))
//            {
//                args.IsValid = false;
//                return;
//            }
            
//            args.IsValid = args.Value.Length >= 8;
//        }
//        // Método para validar si el usuario ya existe
//protected void cvUsuario_ServerValidate(object source, ServerValidateEventArgs args)
//{
//    try
//    {
//        string usuario = txtUsuario.Text.Trim();
        
//        // Validación de longitud mínima
//        if (usuario.Length < 4)
//        {
//            args.IsValid = false;
//            cvUsuario.ErrorMessage = "El usuario debe tener al menos 4 caracteres";
//            return;
//        }
        
//        // Aquí implementarías la lógica real para verificar si el usuario existe en la base de datos
//        bool usuarioExiste = VerificarUsuarioExistente(usuario);
        
//        if (usuarioExiste)
//        {
//            args.IsValid = false;
//            cvUsuario.ErrorMessage = "El nombre de usuario ya está en uso";
//        }
//        else
//        {
//            args.IsValid = true;
//        }
//    }
//    catch (Exception ex)
//    {
//        args.IsValid = false;
//        cvUsuario.ErrorMessage = "Error al validar el usuario";
//    }
//}

//// Método simulado para verificar usuario existente
//private bool VerificarUsuarioExistente(string usuario)
//{
//    // Aquí iría tu lógica real de base de datos
//    // Por ahora, simulamos que algunos usuarios ya existen
//    string[] usuariosExistentes = { "admin", "usuario1", "test" };
//    return usuariosExistentes.Contains(usuario.ToLower());
//}
//    }
//}