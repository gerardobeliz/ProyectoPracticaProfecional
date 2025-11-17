using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace proyectoPracticaProfecional
{
    public partial class AlumnoNotas : System.Web.UI.Page
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

                CargarDatosAlumno();
                CargarNotasAlumno();
            }
        }

        private void CargarDatosAlumno()
        {
            if (Session["legajo_alumno"] == null) return;

            int legajo = Convert.ToInt32(Session["legajo_alumno"]);

            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    string query = @"
                        SELECT A.LEGAJO, A.DNI, A.NOMBRE, A.APELLIDO, A.CARRERA, A.EMAIL, A.TELEFONO
                        FROM ALUMNOS A
                        WHERE A.LEGAJO = @legajo";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@legajo", legajo);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblNombreAlumno.Text = reader["NOMBRE"] + " " + reader["APELLIDO"];
                            lblLegajo.Text = reader["LEGAJO"].ToString();
                            lblDNI.Text = reader["DNI"].ToString();
                            lblCarrera.Text = reader["CARRERA"].ToString();
                            lblEmail.Text = reader["EMAIL"].ToString();
                            lblTelefono.Text = reader["TELEFONO"].ToString();
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar datos del alumno: " + ex.Message);
            }
        }

        private void CargarNotasAlumno()
        {
            if (Session["legajo_alumno"] == null) return;

            int legajo = Convert.ToInt32(Session["legajo_alumno"]);

            try
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    string query = @"
                        SELECT 
                            CURSO,
                            ISNULL(PARCIAL1, 0) AS PARCIAL1,
                            ISNULL(REC_PARCIAL1, 0) AS REC_PARCIAL1,
                            ISNULL(PARCIAL2, 0) AS PARCIAL2,
                            ISNULL(REC_PARCIAL2, 0) AS REC_PARCIAL2,
                            ISNULL(FINAL, 0) AS FINAL
                        FROM NOTAS 
                        WHERE LEGAJO = @legajo";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@legajo", legajo);
                        con.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            gvNotas.DataSource = dt;
                            gvNotas.DataBind();
                            pnlNotas.Visible = true;
                            lblSinNotas.Visible = false;
                        }
                        else
                        {
                            pnlNotas.Visible = false;
                            lblSinNotas.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar notas: " + ex.Message);
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Limpiar sesión
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx");
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