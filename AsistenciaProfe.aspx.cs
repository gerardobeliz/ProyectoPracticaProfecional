
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;

namespace proyectoPracticaProfecional
{
    public partial class AsistenciaProfe : System.Web.UI.Page
    {
        // Usar la cadena de conexión del web.config
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id_personal"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                lblFecha.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
                CargarAlumnosProfesor();
            }
        }

        void CargarAlumnosProfesor()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString))
            {
                con.Open();

                SqlCommand cmdCurso = new SqlCommand(
                    "SELECT id_curso, curso FROM cursos WHERE id_personal = @id", con);
                cmdCurso.Parameters.AddWithValue("@id", Session["id_personal"].ToString());

                SqlDataReader dr = cmdCurso.ExecuteReader();
                if (!dr.Read())
                {
                    lblCurso.Text = "No tenés ningún curso asignado.";
                    dr.Close();
                    return;
                }

                int idCurso = Convert.ToInt32(dr["id_curso"]);
                string nombreCurso = dr["curso"].ToString();

                // ✅ Guardamos en sesión
                Session["id_curso"] = idCurso;

                lblCurso.Text = "Curso: " + nombreCurso;
                dr.Close();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT legajo, nombre, apellido FROM alumnos WHERE carrera = @carrera", con);
                da.SelectCommand.Parameters.AddWithValue("@carrera", nombreCurso);

                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        void MostrarMensaje(string mensaje)
        {
            Response.Write("<script>alert('" + mensaje + "');</script>");
        }
   protected void btnGuardar_Click(object sender, EventArgs e)
{
    // Validaciones mínimas
    if (Session["id_curso"] == null)
    {
        MostrarMensaje("Error: no se encontró el curso del profesor en sesión.");
        return;
    }

    if (GridView1.Rows.Count == 0)
    {
        MostrarMensaje("No hay alumnos para guardar.");
        return;
    }

    int idCurso = Convert.ToInt32(Session["id_curso"]);
    DateTime fechaHoy = DateTime.Today;

    int inserts = 0;
    int updates = 0;
    int errores = 0;

    string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

    try
    {
        using (SqlConnection con = new SqlConnection(cadena))
        {
            con.Open();
            using (SqlTransaction tx = con.BeginTransaction())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        // Obtener legajo desde DataKeys
                        int legajo = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

                        CheckBox chk = (CheckBox)row.FindControl("chkPresente");
                        int presente = (chk != null && chk.Checked) ? 1 : 0;

                        // Intentar UPDATE primero
                        using (SqlCommand cmdUpdate = new SqlCommand(
                            @"UPDATE ASISTENCIA
                              SET ID_CURSO = @IdCurso, PRESENTE = @Presente
                              WHERE LEGAJO = @Legajo AND FECHA = @Fecha", con, tx))
                        {
                            cmdUpdate.Parameters.AddWithValue("@IdCurso", idCurso);
                            cmdUpdate.Parameters.AddWithValue("@Presente", presente);
                            cmdUpdate.Parameters.AddWithValue("@Legajo", legajo);
                            cmdUpdate.Parameters.AddWithValue("@Fecha", fechaHoy);

                            int afectados = cmdUpdate.ExecuteNonQuery();

                            if (afectados > 0)
                            {
                                updates++;
                                continue; // siguiente alumno
                            }
                        }

                        // Si no actualizó nada, insertar
                        using (SqlCommand cmdInsert = new SqlCommand(
                            @"INSERT INTO ASISTENCIA (LEGAJO, ID_CURSO, FECHA, PRESENTE)
                              VALUES (@Legajo, @IdCurso, @Fecha, @Presente)", con, tx))
                        {
                            cmdInsert.Parameters.AddWithValue("@Legajo", legajo);
                            cmdInsert.Parameters.AddWithValue("@IdCurso", idCurso);
                            cmdInsert.Parameters.AddWithValue("@Fecha", fechaHoy);
                            cmdInsert.Parameters.AddWithValue("@Presente", presente);

                            int r = cmdInsert.ExecuteNonQuery();
                            if (r > 0) inserts++;
                            else errores++;
                        }
                    }

                    tx.Commit();
                }
                catch (Exception exTx)
                {
                    tx.Rollback();
                    throw; // será capturado abajo
                }
            }
        }

        // Mensaje final
        string mensaje = "Operación finalizada. ";
        if (updates > 0) mensaje += updates + " registros actualizados. ";
        if (inserts > 0) mensaje += inserts + " registros insertados. ";
        if (errores > 0) mensaje += errores + " registros con error.";
        MostrarMensaje(mensaje.Trim());
    }
    catch (Exception ex)
    {
        MostrarMensaje("Error al guardar asistencia: " + ex.Message);
    }
}
    }
}
