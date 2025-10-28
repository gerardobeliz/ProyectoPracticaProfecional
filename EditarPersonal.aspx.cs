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
                    // Consulta básica - ajusta según tu estructura de tabla
                    string query = @"
                    SELECT ID_PERSONAL, DNI, nombre, apellido, TIPO
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

                            // Cargar dropdown de cargo
                            if (reader["TIPO"] != DBNull.Value)
                            {
                                // Buscar el item en el dropdown y seleccionarlo
                                var item = ddlCargo.Items.FindByValue(reader["TIPO"].ToString());
                                if (item != null)
                                    item.Selected = true;
                            }

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
                    string updateQuery = @"
                    UPDATE PERSONAL 
                    SET nombre = @nombre,
                        apellido = @apellido,
                        TIPO = @tipo
                    WHERE ID_PERSONAL = @idEmpleado";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        command.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim());
                        command.Parameters.AddWithValue("@tipo", ddlCargo.SelectedValue);

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

            // Limpiar controles de formulario
            txtLegajo.Text = string.Empty;
            txtDocumento.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            ddlCargo.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;

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
    }
}