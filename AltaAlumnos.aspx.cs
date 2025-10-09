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
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // PRIMERO valida la página
            Page.Validate();

            if (Page.IsValid)  // <-- CAMBIA de "Page.IsValid" a validar explícitamente
            {
                try
                {
                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        
                        int numTel;
                        numTel = int.Parse(txtTelefono.Text);
                        string script = String.Format("INSERT INTO ALUMNOS VALUES ({0}, '{1}', GETDATE(), '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}')",
                            txtDocumento.Text,txtFechaNacimiento.Text,txtNombre.Text,txtApellido.Text,txtDireccion.Text,1754,numTel,ddlGenero.Text ,ddlCurso.Text,txtEmail.Text);

                        conexion.Open();
                        SqlCommand command = new SqlCommand(script, conexion);
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();
                        conexion.Close();

                        pnlSuccessMessage.Visible = true;
                        pnlErrorMessage.Visible = false;
                    }

                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    pnlSuccessMessage.Visible = false;
                    pnlErrorMessage.Visible = true;
                    lblError.Text = ex.Message;
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


