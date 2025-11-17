using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace proyectoPracticaProfecional
{
    public partial class AlumnoInfo : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar sesión de alumno
                if (Session["legajo_alumno"] == null || Session["tipo"] == null || Session["tipo"].ToString() != "alumno")
                {
                    Response.Redirect("Default.aspx");
                    return;
                }

                CargarInformacionAlumno();
            }
        }

        private void CargarInformacionAlumno()
        {
            if (Session["legajo_alumno"] == null) return;

            int legajo = Convert.ToInt32(Session["legajo_alumno"]);

            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    string query = @"
                        SELECT LEGAJO, DNI, NOMBRE, APELLIDO, CARRERA, EMAIL, TELEFONO, 
                               DIRECCION, CP, GENERO, FECHA_NAC, FECHA_INGRESO
                        FROM ALUMNOS 
                        WHERE LEGAJO = @legajo";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@legajo", legajo);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblLegajo.Text = reader["LEGAJO"].ToString();
                            lblDNI.Text = reader["DNI"].ToString();
                            lblNombre.Text = reader["NOMBRE"] + " " + reader["APELLIDO"];
                            lblCarrera.Text = reader["CARRERA"].ToString();
                            lblEmail.Text = reader["EMAIL"].ToString();
                            lblTelefono.Text = reader["TELEFONO"].ToString();
                            lblDireccion.Text = reader["DIRECCION"].ToString();
                            lblCP.Text = reader["CP"].ToString();

                            // ✅ CORREGIDO: Mostrar género completo
                            string genero = reader["GENERO"].ToString();
                            lblGenero.Text = ObtenerGeneroCompleto(genero);

                            if (reader["FECHA_NAC"] != DBNull.Value)
                                lblFechaNac.Text = Convert.ToDateTime(reader["FECHA_NAC"]).ToString("dd/MM/yyyy");

                            if (reader["FECHA_INGRESO"] != DBNull.Value)
                                lblFechaIngreso.Text = Convert.ToDateTime(reader["FECHA_INGRESO"]).ToString("dd/MM/yyyy");
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar información: " + ex.Message);
            }
        }

        // ✅ NUEVO: Método para convertir la letra del género a texto completo
        private string ObtenerGeneroCompleto(string genero)
        {
            switch (genero.ToUpper())
            {
                case "M":
                    return "Masculino";
                case "F":
                    return "Femenino";
                case "O":
                    return "Otros";
                default:
                    return genero; // Si no coincide, devuelve el valor original
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            // Método alternativo sin ScriptManager
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "showAlert",
                "alert('" + mensaje.Replace("'", "\\'") + "');",
                true
            );
        }
    }
}