using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

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

                            int numTel;
                            numTel = int.Parse(txtTelefono.Text);
                            string script = String.Format("INSERT INTO PERSONAL VALUES ({0}, '{1}', GETDATE(), '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}')",
                                ddlTipoPersonal.SelectedValue,txtDocumento.Text,txtFechaIngreso.Text,                   txtApellido.Text, txtDireccion.Text, 1754, numTel, ddlGenero.Text,  txtEmail.Text);

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
                                  
                    // Mostrar modal de confirmación
                    //string script = string.Format("showConfirmation('{0}', '{1}', '{2}', '{3}');", 
                    //    txtNombre.Text + " " + txtApellido.Text, 
                    //    ddlTipoPersonal.SelectedItem.Text, 
                    //    txtEmail.Text, 
                    //    txtFechaIngreso.Text);
                    
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showConfirmation", script, true);
                    
                    // Limpiar formulario después de guardar
                    LimpiarFormulario();
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
    }
}