using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;

namespace proyectoPracticaProfecional
{
    public partial class EditarPersonal : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimpiarInterfaz();
                OcultarMensajes();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = txtBuscarEmpleado.Text.Trim();

            if (string.IsNullOrEmpty(criterioBusqueda))
            {
                MostrarMensajeError("Por favor, ingrese un criterio de búsqueda.");
                return;
            }

            BuscarEmpleados(criterioBusqueda);
        }

        private void BuscarEmpleados(string criterio)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    string query = @"
                    SELECT ID_PERSONAL, DNI, nombre, apellido, TIPO
                    FROM PERSONAL
                    WHERE (nombre LIKE @criterio OR apellido LIKE @criterio OR CONVERT(VARCHAR, DNI) LIKE @criterio)
                    ORDER BY apellido, nombre";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@criterio", "%" + criterio + "%");

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            gvEmpleados.DataSource = dt;
                            gvEmpleados.DataBind();
                            pnlResultados.Visible = true;
                            OcultarMensajes();
                        }
                        else
                        {
                            pnlResultados.Visible = false;
                            MostrarMensajeError("No se encontraron empleados con el criterio especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar empleados: " + ex.Message);
            }
        }

        protected void gvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvEmpleados.SelectedRow != null)
            {
                string idPersonal = gvEmpleados.DataKeys[gvEmpleados.SelectedIndex].Value.ToString();
                CargarDatosEmpleadoParaEdicion(idPersonal);
            }
        }

        private void CargarDatosEmpleadoParaEdicion(string idPersonal)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    // Consulta actualizada con todos los nuevos campos
                    string query = @"
                    SELECT ID_PERSONAL, DNI, nombre, apellido, TIPO, 
                           fecha_nac, fecha_ingreso, direccion, cp, 
                           tel, email,pass
                    FROM PERSONAL 
                    WHERE ID_PERSONAL = @idPersonal";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPersonal", idPersonal);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            ViewState["IDEmpleadoAEditar"] = reader["ID_PERSONAL"].ToString();

                            // Mostrar información del empleado seleccionado
                            string nombreCompleto = string.Format("{0}, {1}", reader["apellido"], reader["nombre"]);
                            lblEmpleadoAEditar.Text = nombreCompleto;

                            // Cargar datos en los controles del formulario
                            txtLegajo.Text = reader["ID_PERSONAL"].ToString();
                            txtDocumento.Text = reader["DNI"].ToString();
                            txtNombre.Text = reader["nombre"].ToString();
                            txtApellido.Text = reader["apellido"].ToString();

                            // Nuevos campos
                            if (reader["fecha_nac"] != DBNull.Value)
                            {
                                txtFechaNacimiento.Text = Convert.ToDateTime(reader["fecha_nac"]).ToString("yyyy-MM-dd");
                            }

                            if (reader["fecha_ingreso"] != DBNull.Value)
                            {
                                txtFechaIngreso.Text = Convert.ToDateTime(reader["fecha_ingreso"]).ToString("yyyy-MM-dd");
                            }

                            txtDireccion.Text = reader["direccion"] != DBNull.Value ? reader["direccion"].ToString() : "";
                            txtCP.Text = reader["cp"] != DBNull.Value ? reader["cp"].ToString() : "";
                            txtTelefono.Text = reader["tel"] != DBNull.Value ? reader["tel"].ToString() : "";
                            txtEmail.Text = reader["email"] != DBNull.Value ? reader["email"].ToString() : "";

                            // Password - dejar en blanco por seguridad
                            txtPassword.Text = "";

                            // Cargar dropdowns
                            if (reader["TIPO"] != DBNull.Value)
                            {
                                var itemTipo = ddlCargo.Items.FindByValue(reader["TIPO"].ToString());
                                if (itemTipo != null)
                                    itemTipo.Selected = true;
                            }

                            //if (reader["estado"] != DBNull.Value)
                            //{
                            //    var itemEstado = ddlEstado.Items.FindByValue(reader["estado"].ToString());
                            //    if (itemEstado != null)
                            //        itemEstado.Selected = true;
                            //}

                            pnlEdicion.Visible = true;
                            OcultarMensajes();
                        }
                        else
                        {
                            MostrarMensajeError("No se pudieron cargar los datos del empleado.");
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar datos del empleado: " + ex.Message);
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ActualizarEmpleado();
            }
        }

        private void ActualizarEmpleado()
        {
            string idEmpleado = ViewState["IDEmpleadoAEditar"] != null ?
                ViewState["IDEmpleadoAEditar"].ToString() : "";

            if (string.IsNullOrEmpty(idEmpleado))
            {
                MostrarMensajeError("No se ha seleccionado un empleado válido para editar.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    // Query actualizada con todos los nuevos campos
                    string updateQuery = @"
                    UPDATE PERSONAL 
                    SET nombre = @nombre,
                        apellido = @apellido,
                        TIPO = @tipo,
                        DNI = @dni,
                        fecha_nac = @fechaNacimiento,
                        fecha_ingreso = @fechaIngreso,
                        direccion = @direccion,
                        cp= @codigoPostal,
                        tel = @telefono,
                        email = @email,
//                        estado = @estado
                        {0} -- Placeholder para actualización de contraseña
                    WHERE ID_PERSONAL = @idEmpleado";

                    // Manejar la contraseña (solo actualizar si se proporciona una nueva)
                    string passwordUpdate = "";
                    SqlParameter passwordParam = null;

                    if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
                    {
                        passwordUpdate = ", password = @password";
                        string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1");
                        passwordParam = new SqlParameter("@password", hashedPassword);
                    }

                    updateQuery = string.Format(updateQuery, passwordUpdate);

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        command.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim());
                        command.Parameters.AddWithValue("@tipo", ddlCargo.SelectedValue);
                        command.Parameters.AddWithValue("@dni", txtDocumento.Text.Trim());
                        //command.Parameters.AddWithValue("@estado", ddlEstado.SelectedValue);

                        // Nuevos campos con manejo de valores nulos
                        command.Parameters.AddWithValue("@fechaNacimiento",
                            string.IsNullOrEmpty(txtFechaNacimiento.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtFechaNacimiento.Text));

                        command.Parameters.AddWithValue("@fechaIngreso",
                            string.IsNullOrEmpty(txtFechaIngreso.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtFechaIngreso.Text));

                        command.Parameters.AddWithValue("@direccion",
                            string.IsNullOrEmpty(txtDireccion.Text) ? (object)DBNull.Value : txtDireccion.Text.Trim());

                        command.Parameters.AddWithValue("@codigoPostal",
                            string.IsNullOrEmpty(txtCP.Text) ? (object)DBNull.Value : txtCP.Text.Trim());

                        command.Parameters.AddWithValue("@telefono",
                            string.IsNullOrEmpty(txtTelefono.Text) ? (object)DBNull.Value : txtTelefono.Text.Trim());

                        command.Parameters.AddWithValue("@email",
                            string.IsNullOrEmpty(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());

                        // Agregar parámetro de contraseña si es necesario
                        if (passwordParam != null)
                        {
                            command.Parameters.Add(passwordParam);
                        }

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MostrarMensajeExito("Los datos del empleado se actualizaron correctamente.");
                            LimpiarInterfaz();
                        }
                        else
                        {
                            MostrarMensajeError("No se pudieron actualizar los datos del empleado.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Violación de unique key
                {
                    MostrarMensajeError("Error: El DNI o email ya existe en el sistema.");
                }
                else
                {
                    MostrarMensajeError("Error de base de datos al actualizar empleado: " + sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al actualizar empleado: " + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarInterfaz();
        }

        private void LimpiarInterfaz()
        {
            txtBuscarEmpleado.Text = string.Empty;
            pnlResultados.Visible = false;
            pnlEdicion.Visible = false;
            ViewState["IDEmpleadoAEditar"] = null;

            // Limpiar todos los controles del formulario
            txtLegajo.Text = string.Empty;
            txtDocumento.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtFechaNacimiento.Text = string.Empty;
            txtFechaIngreso.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtCP.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;

            // Resetear dropdowns

            ddlCargo.SelectedIndex = 0;
            //ddlEstado.SelectedIndex = 0;

            OcultarMensajes();
        }

        private void MostrarMensajeExito(string mensaje)
        {
            lblSuccessMessage.Text = mensaje;
            pnlSuccessMessage.Visible = true;
            pnlErrorMessage.Visible = false;
        }

        private void MostrarMensajeError(string mensaje)
        {
            lblErrorMessage.Text = mensaje;
            pnlErrorMessage.Visible = true;
            pnlSuccessMessage.Visible = false;
        }

        private void OcultarMensajes()
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
        }

        // Método adicional para validaciones personalizadas si es necesario
        protected void ValidarFechas(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            // Ejemplo de validación personalizada para fechas
            if (!string.IsNullOrEmpty(txtFechaNacimiento.Text) && !string.IsNullOrEmpty(txtFechaIngreso.Text))
            {
                DateTime fechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text);
                DateTime fechaIngreso = Convert.ToDateTime(txtFechaIngreso.Text);

                if (fechaNacimiento >= fechaIngreso)
                {
                    args.IsValid = false;
                    // Puedes mostrar un mensaje de error adicional aquí
                }
            }
        }
    }
}