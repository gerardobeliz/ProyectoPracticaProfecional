using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace proyectoPracticaProfecional
{
    public partial class EditarPersonal : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlResultados.Visible = false;
                pnlEdicion.Visible = false;
                pnlMensaje.Visible = false;
            }

            // Manejar el postback desde JavaScript
            if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].StartsWith("SeleccionarEmpleado:"))
            {
                string idPersonal = Request["__EVENTARGUMENT"].Replace("SeleccionarEmpleado:", "");
                CargarEmpleado(idPersonal);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscarEmpleado.Text.Trim();

            if (string.IsNullOrEmpty(criterio))
            {
                MostrarMensaje("Por favor, ingrese un criterio de búsqueda.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    string query = @"SELECT ID_PERSONAL, DNI, nombre, apellido, TIPO 
                                   FROM PERSONAL 
                                   WHERE nombre LIKE @criterio OR apellido LIKE @criterio OR DNI LIKE @criterio";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@criterio", "%" + criterio + "%");
                        con.Open();

                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());

                        if (dt.Rows.Count > 0)
                        {
                            gvEmpleados.DataSource = dt;
                            gvEmpleados.DataBind();
                            pnlResultados.Visible = true;
                            pnlMensaje.Visible = false;
                        }
                        else
                        {
                            pnlResultados.Visible = false;
                            MostrarMensaje("No se encontraron resultados.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error: " + ex.Message);
            }
        }

        protected void gvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Método tradicional por si acaso
            if (gvEmpleados.SelectedRow != null)
            {
                string idPersonal = gvEmpleados.SelectedDataKey.Value.ToString();
                CargarEmpleado(idPersonal);
            }
        }

        private void CargarEmpleado(string idPersonal)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    string query = @"SELECT ID_PERSONAL, DNI, nombre, apellido, tipo, email,
                                   fecha_nac, fecha_ingreso, direccion, cp, tel, pass
                                   FROM PERSONAL WHERE ID_PERSONAL = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", idPersonal);
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Cargar datos básicos
                                txtLegajo.Text = reader["ID_PERSONAL"].ToString();
                                txtDocumento.Text = reader["DNI"].ToString();
                                txtNombre.Text = reader["nombre"].ToString();
                                txtApellido.Text = reader["apellido"].ToString();
                                txtEmail.Text = reader["email"] != DBNull.Value ? reader["email"].ToString() : "";

                                // Cargar fechas
                                if (reader["fecha_nac"] != DBNull.Value)
                                {
                                    DateTime fechaNac = Convert.ToDateTime(reader["fecha_nac"]);
                                    txtFechaNacimiento.Text = fechaNac.ToString("yyyy-MM-dd");
                                }

                                if (reader["fecha_ingreso"] != DBNull.Value)
                                {
                                    DateTime fechaIngreso = Convert.ToDateTime(reader["fecha_ingreso"]);
                                    txtFechaIngreso.Text = fechaIngreso.ToString("yyyy-MM-dd");
                                }

                                // Cargar otros campos
                                txtDireccion.Text = reader["direccion"] != DBNull.Value ? reader["direccion"].ToString() : "";
                                txtCP.Text = reader["cp"] != DBNull.Value ? reader["cp"].ToString() : "";
                                txtTelefono.Text = reader["tel"] != DBNull.Value ? reader["tel"].ToString() : "";

                                // Cargar dropdown
                                if (reader["tipo"] != DBNull.Value)
                                {
                                    ddlCargo.SelectedValue = reader["tipo"].ToString();
                                }

                                lblEmpleadoAEditar.Text = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();
                                pnlEdicion.Visible = true;
                                MostrarMensaje("Empleado cargado: " + reader["nombre"].ToString() + " " + reader["apellido"].ToString());
                            }
                            else
                            {
                                MostrarMensaje("No se encontró el empleado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar: " + ex.Message);
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    string query = @"UPDATE PERSONAL SET 
                                   nombre = @nombre, 
                                   apellido = @apellido, 
                                   tipo = @tipo,
                                   DNI = @dni,
                                   email = @email,
                                   fecha_nac = @fecha_nac,
                                   fecha_ingreso = @fecha_ingreso,
                                   direccion = @direccion,
                                   cp = @cp,
                                   tel = @tel
                                   WHERE ID_PERSONAL = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", txtLegajo.Text);
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                        cmd.Parameters.AddWithValue("@tipo", ddlCargo.SelectedValue);
                        cmd.Parameters.AddWithValue("@dni", txtDocumento.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@fecha_nac", string.IsNullOrEmpty(txtFechaNacimiento.Text) ? (object)DBNull.Value : DateTime.Parse(txtFechaNacimiento.Text));
                        cmd.Parameters.AddWithValue("@fecha_ingreso", string.IsNullOrEmpty(txtFechaIngreso.Text) ? (object)DBNull.Value : DateTime.Parse(txtFechaIngreso.Text));
                        cmd.Parameters.AddWithValue("@direccion", string.IsNullOrEmpty(txtDireccion.Text) ? (object)DBNull.Value : txtDireccion.Text);
                        cmd.Parameters.AddWithValue("@cp", string.IsNullOrEmpty(txtCP.Text) ? (object)DBNull.Value : txtCP.Text);
                        cmd.Parameters.AddWithValue("@tel", string.IsNullOrEmpty(txtTelefono.Text) ? (object)DBNull.Value : txtTelefono.Text);

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MostrarMensaje("Empleado actualizado correctamente.");
                            pnlEdicion.Visible = false;
                        }
                        else
                        {
                            MostrarMensaje("No se pudo actualizar el empleado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlEdicion.Visible = false;
            pnlMensaje.Visible = false;
        }

        private void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
            pnlMensaje.Visible = true;
        }
    }
}