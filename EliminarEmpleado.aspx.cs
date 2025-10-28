using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace proyectoPracticaProfecional
{
    public partial class ModificarEmpleado : System.Web.UI.Page
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
                    // usa la estructura de tu tabla PERSONAL
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
                CargarDatosEmpleadoParaEliminar(idPersonal);
            }
        }

        private void CargarDatosEmpleadoParaEliminar(string idPersonal)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    // usa la estructura de PERSONAL
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
                            // Almacenar ID del empleado en ViewState
                            ViewState["IDEmpleadoAEliminar"] = reader["ID_PERSONAL"].ToString();

                            // Mostrar información del empleado seleccionado
                            string nombreCompleto = string.Format("{0}, {1}", reader["apellido"], reader["nombre"]);
                            lblEmpleadoAEliminar.Text = nombreCompleto;

                            // Mostrar detalles - AJUSTADO a las columnas reales
                            lblDocumento.Text = reader["DNI"].ToString();
                            lblCargo.Text = reader["TIPO"] != null ? reader["TIPO"].ToString() : "No especificado";
                            
                            // Si no tienes estos campos en tu tabla, los ocultamos o mostramos valores por defecto
                            lblLegajo.Text = reader["ID_PERSONAL"].ToString(); // Usar ID como legajo
                            lblEstado.Text = "Activo"; // Asumir activo si no hay campo estado

                            pnlConfirmacion.Visible = true;
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

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            EliminarEmpleadoDefinitivamente();
        }

        private void EliminarEmpleadoDefinitivamente()
        {
            string idEmpleado = ViewState["IDEmpleadoAEliminar"] != null ? 
                ViewState["IDEmpleadoAEliminar"].ToString() : "";

            if (string.IsNullOrEmpty(idEmpleado))
            {
                MostrarMensajeError("No se ha seleccionado un empleado válido para eliminar.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    if (TieneRegistrosRelacionados(idEmpleado))
                    {
                        // Si tu tabla PERSONAL tiene campo estado, hacer soft delete
                        // Si no, comentar esta parte y solo hacer hard delete
                        string softDeleteQuery = @"
                        UPDATE PERSONAL 
                        SET estado = 'eliminado'
                        WHERE ID_PERSONAL = @idEmpleado";

                        using (SqlCommand command = new SqlCommand(softDeleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MostrarMensajeExito("Empleado marcado como eliminado (soft delete) debido a registros relacionados.");
                                LimpiarInterfaz();
                            }
                            else
                            {
                                MostrarMensajeError("No se pudo marcar el empleado como eliminado.");
                            }
                        }
                    }
                    else
                    {
                        // Hard delete - ELIMINACIÓN PERMANENTE
                        string hardDeleteQuery = "DELETE FROM PERSONAL WHERE ID_PERSONAL = @idEmpleado";
                        using (SqlCommand command = new SqlCommand(hardDeleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MostrarMensajeExito("Empleado eliminado permanentemente del sistema.");
                                LimpiarInterfaz();
                            }
                            else
                            {
                                MostrarMensajeError("No se pudo eliminar el empleado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar empleado: " + ex.Message);
            }
        }

        private bool TieneRegistrosRelacionados(string idEmpleado)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Cadena))
                {
                    // VERIFICAR TABLAS RELACIONADAS - ajusta según tu base de datos
                    string[] tablasRelacionadas = {
                        "Pagos", "Asistencias", "Horarios", "Permisos", "Vacaciones"
                    };

                    foreach (string tabla in tablasRelacionadas)
                    {
                        // Verificar si la tabla existe y tiene registros
                        string checkTableQuery = @"
                        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tablaName)
                        BEGIN
                            SELECT COUNT(1) FROM " + tabla + " WHERE ID_PERSONAL = @idEmpleado END ELSE BEGIN SELECT 0 END";

                        using (SqlCommand command = new SqlCommand(checkTableQuery, connection))
                        {
                            command.Parameters.AddWithValue("@tablaName", tabla);
                            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                            if (connection.State != ConnectionState.Open)
                                connection.Open();

                            int count = (int)command.ExecuteScalar();
                            if (count > 0)
                                return true;
                        }
                    }
                    return false;
                }
            }
            catch
            {
                // Por seguridad, si hay error asumimos que hay relaciones
                return true;
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
            pnlConfirmacion.Visible = false;
            ViewState["IDEmpleadoAEliminar"] = null;
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