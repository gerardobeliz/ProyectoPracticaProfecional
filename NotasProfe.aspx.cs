using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;

namespace proyectoPracticaProfecional
{
    public partial class NotasProfe : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id_personal"] == null)
                {
                    Response.Redirect("Default.aspx");
                    return;
                }

                CargarAlumnosProfesor();
            }
        }

        void CargarAlumnosProfesor()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString))
            {
                con.Open();

                SqlCommand cmdCurso = new SqlCommand(
                    "SELECT id_curso, curso FROM CURSOS WHERE id_personal = @idPersonal", con);
                cmdCurso.Parameters.AddWithValue("@idPersonal", Session["id_personal"].ToString());

                SqlDataReader drCurso = cmdCurso.ExecuteReader();
                if (!drCurso.Read())
                {
                    lblCurso.Text = "No tenés ningún curso asignado.";
                    drCurso.Close();
                    return;
                }

                string nombreCurso = drCurso["curso"].ToString();
                lblCurso.Text = "Curso: " + nombreCurso;
                drCurso.Close();

                SqlDataAdapter da = new SqlDataAdapter(@"
    SELECT A.legajo, A.nombre, A.apellido,
       ISNULL(N.PARCIAL1, 0) AS PARCIAL1,
       ISNULL(N.REC_PARCIAL1, 0) AS REC_PARCIAL1,
       ISNULL(N.PARCIAL2, 0) AS PARCIAL2,
       ISNULL(N.REC_PARCIAL2, 0) AS REC_PARCIAL2,
       ISNULL(N.FINAL, 0) AS FINAL
FROM ALUMNOS A
LEFT JOIN NOTAS N ON A.legajo = N.legajo AND N.curso = @curso
WHERE A.CARRERA = @curso
ORDER BY A.apellido, A.nombre
", con);

da.SelectCommand.Parameters.AddWithValue("@curso", nombreCurso);

DataTable dt = new DataTable();
da.Fill(dt);

GridView1.DataSource = dt;
GridView1.DataBind();
            }
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Cadena))
            {
                con.Open();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    int legajo = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

                    TextBox txtParcial1 = (TextBox)row.FindControl("txtParcial1");
                    TextBox txtRecParcial1 = (TextBox)row.FindControl("txtRecParcial1");
                    TextBox txtParcial2 = (TextBox)row.FindControl("txtParcial2");
                    TextBox txtRecParcial2 = (TextBox)row.FindControl("txtRecParcial2");
                    TextBox txtFinal = (TextBox)row.FindControl("txtFinal");

                    decimal parcial1 = string.IsNullOrEmpty(txtParcial1.Text) ? 0 : Convert.ToDecimal(txtParcial1.Text);
                    decimal recParcial1 = string.IsNullOrEmpty(txtRecParcial1.Text) ? 0 : Convert.ToDecimal(txtRecParcial1.Text);
                    decimal parcial2 = string.IsNullOrEmpty(txtParcial2.Text) ? 0 : Convert.ToDecimal(txtParcial2.Text);
                    decimal recParcial2 = string.IsNullOrEmpty(txtRecParcial2.Text) ? 0 : Convert.ToDecimal(txtRecParcial2.Text);
                    decimal final = string.IsNullOrEmpty(txtFinal.Text) ? 0 : Convert.ToDecimal(txtFinal.Text);

                    // Verificar si ya existe la nota para este alumno y curso
                    SqlCommand cmdCheck = new SqlCommand(
                        "SELECT COUNT(1) FROM NOTAS WHERE legajo = @legajo AND curso = @curso", con);
                    cmdCheck.Parameters.AddWithValue("@legajo", legajo);
                    cmdCheck.Parameters.AddWithValue("@curso", lblCurso.Text.Replace("Curso: ", ""));
                    int existe = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    SqlCommand cmd;
                    if (existe > 0)
                    {
                        // Actualizar
                        cmd = new SqlCommand(@"UPDATE NOTAS 
                                               SET PARCIAL1=@p1, REC_PARCIAL1=@rp1, PARCIAL2=@p2, REC_PARCIAL2=@rp2, FINAL=@final
                                               WHERE legajo=@legajo AND curso=@curso", con);
                    }
                    else
                    {
                        // Insertar
                        cmd = new SqlCommand(@"INSERT INTO NOTAS
                                               (legajo, curso, PARCIAL1, REC_PARCIAL1, PARCIAL2, REC_PARCIAL2, FINAL)
                                               VALUES (@legajo, @curso, @p1, @rp1, @p2, @rp2, @final)", con);
                    }

                    cmd.Parameters.AddWithValue("@legajo", legajo);
                    cmd.Parameters.AddWithValue("@curso", lblCurso.Text.Replace("Curso: ", ""));
                    cmd.Parameters.AddWithValue("@p1", parcial1);
                    cmd.Parameters.AddWithValue("@rp1", recParcial1);
                    cmd.Parameters.AddWithValue("@p2", parcial2);
                    cmd.Parameters.AddWithValue("@rp2", recParcial2);
                    cmd.Parameters.AddWithValue("@final", final);

                    cmd.ExecuteNonQuery();
                }
            }

            Response.Write("<script>alert('Notas guardadas correctamente');</script>");
        }
    }
}
