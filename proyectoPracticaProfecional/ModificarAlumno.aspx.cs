using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace proyectoPracticaProfecional
{
    public partial class ModificarAlumno : System.Web.UI.Page
    {
         private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlFormulario.Visible = false;
                pnlResultados.Visible = false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscarAlumno.Text.Trim();

            if (!string.IsNullOrEmpty(criterio))
            {
                // Simulación de búsqueda en base de datos
                DataTable dt = BuscarAlumnos(criterio);

                if (dt.Rows.Count > 0)
                {
                    gvAlumnos.DataSource = dt;
                    gvAlumnos.DataBind();
                    pnlResultados.Visible = true;
                }
                else
                {
                    pnlResultados.Visible = false;
                    MostrarMensajeError("No se encontraron alumnos con ese criterio de búsqueda.");
                }
            }
            else
            {
                MostrarMensajeError("Por favor ingrese un criterio de búsqueda.");
            }
        }

        protected void gvAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idAlumno = gvAlumnos.SelectedDataKey.Value.ToString();
            CargarDatosAlumno(idAlumno);
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Aquí iría el código para actualizar en la base de datos
                    ActualizarAlumno();
                    MostrarMensajeExito("Alumno actualizado correctamente.");
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    MostrarMensajeError("Error al actualizar el alumno: " + ex.Message);
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Aquí iría el código para eliminar de la base de datos
                EliminarAlumno();
                MostrarMensajeExito("Alumno eliminado correctamente.");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar el alumno: " + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private DataTable BuscarAlumnos(string criterio)
        {

             using (SqlConnection conexion = new SqlConnection(Cadena))

        //    // Simulación de datos - Reemplazar con conexión a BD real
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ID");
        //    dt.Columns.Add("Documento");
        //    dt.Columns.Add("Apellido");
        //    dt.Columns.Add("Nombre");
        //    dt.Columns.Add("Curso");
        //    dt.Columns.Add("Email");

        //    // Datos de ejemplo
        //    dt.Rows.Add("1", "12345678", "Pérez", "Juan", "Primero Básico", "juan@email.com");
        //    dt.Rows.Add("2", "87654321", "Gómez", "María", "Segundo Básico", "maria@email.com");

        //    return dt;
        //}

        private void CargarDatosAlumno(string idAlumno)
        {
            // Simulación de carga de datos - Reemplazar con conexión a BD real
            lblIDAlumno.Text = idAlumno;
            txtNombre.Text = "Juan";
            txtApellido.Text = "Pérez";
            txtFechaNacimiento.Text = "2010-05-15";
            ddlGenero.SelectedValue = "M";
            txtDocumento.Text = "12345678";
            txtDireccion.Text = "Calle Falsa 123";
            txtEmail.Text = "juan@email.com";
            txtTelefono.Text = "+54 11 1234-5678";
            ddlCurso.SelectedValue = "1";
            txtFechaInscripcion.Text = DateTime.Today.ToString("yyyy-MM-dd");
            chkActivo.Checked = true;

            lblAlumnoSeleccionado.Text = "Pérez, Juan (DOC: 12345678)";
            pnlFormulario.Visible = true;
        }

        private void ActualizarAlumno()
        {
            // Aquí iría el código para actualizar en la base de datos
            // usando los valores de los controles del formulario
        }

        private void EliminarAlumno()
        {
            // Aquí iría el código para eliminar de la base de datos
        }

        private void LimpiarFormulario()
        {
            pnlFormulario.Visible = false;
            pnlResultados.Visible = false;
            txtBuscarAlumno.Text = "";
            LimpiarMensajes();
        }

        private void MostrarMensajeExito(string mensaje)
        {
            pnlSuccessMessage.Visible = true;
            pnlErrorMessage.Visible = false;
            lblSuccessMessage.Text = mensaje;
        }

        private void MostrarMensajeError(string mensaje)
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = true;
            lblErrorMessage.Text = mensaje;
        }

        private void LimpiarMensajes()
        {
            pnlSuccessMessage.Visible = false;
            pnlErrorMessage.Visible = false;
        }
    }
}