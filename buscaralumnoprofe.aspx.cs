using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace proyectoPracticaProfecional
{
    public partial class BuscarAlumnoProfe : System.Web.UI.Page
    {
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

                // Inicializar ddlMes y ddlAño
                ddlMes.Items.Clear();
                for (int m = 1; m <= 12; m++)
                    ddlMes.Items.Add(new ListItem(m.ToString(), m.ToString()));

                ddlAño.Items.Clear();
                int añoActual = DateTime.Now.Year;
                for (int a = añoActual - 3; a <= añoActual + 1; a++)
                    ddlAño.Items.Add(new ListItem(a.ToString(), a.ToString()));

                ddlMes.SelectedValue = DateTime.Now.Month.ToString();
                ddlAño.SelectedValue = DateTime.Now.Year.ToString();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                MostrarMensaje("Ingrese legajo o nombre/apellido.");
                gvAlumno.Visible = gvAsistencias.Visible = btnGuardarNotas.Visible = false;
                return;
            }

            using (SqlConnection con = new SqlConnection(Cadena))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 1 a.LEGAJO, a.NOMBRE, a.APELLIDO,
                           ISNULL(n.PARCIAL1, 0) AS PARCIAL1,
                           ISNULL(n.REC_PARCIAL1, 0) AS REC_PARCIAL1,
                           ISNULL(n.PARCIAL2, 0) AS PARCIAL2,
                           ISNULL(n.REC_PARCIAL2, 0) AS REC_PARCIAL2,
                           ISNULL(n.FINAL, 0) AS FINAL,
                           c.CURSO, c.ID_CURSO
                    FROM ALUMNOS a
                    INNER JOIN CURSOS c ON a.CARRERA = c.CURSO
                    LEFT JOIN NOTAS n ON a.LEGAJO = n.LEGAJO AND n.CURSO = c.CURSO
                    WHERE c.ID_PERSONAL = @id_personal
                      AND (CAST(a.LEGAJO AS NVARCHAR(50)) LIKE @texto
                           OR a.NOMBRE LIKE @texto
                           OR a.APELLIDO LIKE @texto)
                    ORDER BY a.APELLIDO, a.NOMBRE", con);

                cmd.Parameters.AddWithValue("@id_personal", Session["id_personal"]);
                cmd.Parameters.AddWithValue("@texto", "%" + texto + "%");

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd)) da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MostrarMensaje("No se encontró el alumno en su curso.");
                    gvAlumno.Visible = gvAsistencias.Visible = btnGuardarNotas.Visible = false;
                    return;
                }

                gvAlumno.DataSource = dt;
                gvAlumno.DataBind();
                gvAlumno.Visible = true;
                btnGuardarNotas.Visible = true;

                // Limpiar grilla de asistencias
                gvAsistencias.DataSource = null;
                gvAsistencias.DataBind();
                gvAsistencias.Visible = false;

                // Guardar legajo e ID_CURSO en sesión para ver asistencias
                Session["legajo_alumno"] = dt.Rows[0]["LEGAJO"];
                Session["id_curso"] = dt.Rows[0]["ID_CURSO"];
            }
        }

        protected void btnVerAsistencias_Click(object sender, EventArgs e)
        {
            if (Session["legajo_alumno"] == null || Session["id_curso"] == null)
            {
                MostrarMensaje("Primero busque un alumno.");
                return;
            }

            int legajo = Convert.ToInt32(Session["legajo_alumno"]);
            int idCurso = Convert.ToInt32(Session["id_curso"]);

            int mes, año;
            if (!int.TryParse(ddlMes.SelectedValue, out mes))
            {
                MostrarMensaje("Seleccione un mes válido.");
                return;
            }
            if (!int.TryParse(ddlAño.SelectedValue, out año))
            {
                MostrarMensaje("Seleccione un año válido.");
                return;
            }

            DateTime primerDia = new DateTime(año, mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            DataTable dtMes = new DataTable();
            dtMes.Columns.Add("Alumno");

            List<DateTime> diasHabiles = new List<DateTime>();
            for (DateTime d = primerDia; d <= ultimoDia; d = d.AddDays(1))
            {
                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                {
                    dtMes.Columns.Add(d.ToString("dd/MM"));
                    diasHabiles.Add(d);
                }
            }

            using (SqlConnection con = new SqlConnection(Cadena))
            {
                con.Open();
                SqlCommand cmdAsis = new SqlCommand(@"
                    SELECT FECHA, PRESENTE
                    FROM ASISTENCIA
                    WHERE LEGAJO=@legajo AND ID_CURSO=@curso
                      AND MONTH(FECHA)=@mes AND YEAR(FECHA)=@año", con);

                cmdAsis.Parameters.AddWithValue("@legajo", legajo);
                cmdAsis.Parameters.AddWithValue("@curso", idCurso);
                cmdAsis.Parameters.AddWithValue("@mes", mes);
                cmdAsis.Parameters.AddWithValue("@año", año);

                Dictionary<DateTime, int> asistencias = new Dictionary<DateTime, int>();
                using (SqlDataReader dr = cmdAsis.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime fecha = Convert.ToDateTime(dr["FECHA"]);
                        int est = Convert.ToInt32(dr["PRESENTE"]);
                        asistencias[fecha] = est;
                    }
                }

                DataRow drAlumno = dtMes.NewRow();
                GridViewRow rowAlumno = gvAlumno.Rows[0];
                drAlumno["Alumno"] = rowAlumno.Cells[1].Text + " " + rowAlumno.Cells[2].Text;

                foreach (var d in diasHabiles)
                {
                    string col = d.ToString("dd/MM");
                    string valor = "A"; // default ausente
                    if (asistencias.ContainsKey(d))
                    {
                        int estado = asistencias[d];
                        if (estado == 1) valor = "P";
                        else if (estado == 2) valor = "J";
                    }
                    drAlumno[col] = valor;
                }

                dtMes.Rows.Add(drAlumno);
            }

            gvAsistencias.DataSource = dtMes;
            gvAsistencias.DataBind();
            gvAsistencias.Visible = true;
        }

        protected void gvAsistencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    string valor = e.Row.Cells[i].Text.Trim();
                    switch (valor)
                    {
                        case "P": e.Row.Cells[i].CssClass = "presente"; break;
                        case "A": e.Row.Cells[i].CssClass = "ausente"; break;
                        case "J": e.Row.Cells[i].CssClass = "justificado"; break;
                    }
                }
            }
        }

        protected void btnGuardarNotas_Click(object sender, EventArgs e)
        {
            if (gvAlumno.Rows.Count == 0) return;
            GridViewRow row = gvAlumno.Rows[0];

            int legajo = Convert.ToInt32(row.Cells[0].Text);
            string cursoNombre = ObtenerCursoProfesor();

            TextBox txtParcial1 = row.FindControl("txtParcial1") as TextBox;
            TextBox txtRecParcial1 = row.FindControl("txtRecParcial1") as TextBox;
            TextBox txtParcial2 = row.FindControl("txtParcial2") as TextBox;
            TextBox txtRecParcial2 = row.FindControl("txtRecParcial2") as TextBox;
            TextBox txtFinal = row.FindControl("txtFinal") as TextBox;

            decimal p1 = ParseDecimalOrZero(txtParcial1 != null ? txtParcial1.Text : "");
            decimal rp1 = ParseDecimalOrZero(txtRecParcial1 != null ? txtRecParcial1.Text : "");
            decimal p2 = ParseDecimalOrZero(txtParcial2 != null ? txtParcial2.Text : "");
            decimal rp2 = ParseDecimalOrZero(txtRecParcial2 != null ? txtRecParcial2.Text : "");
            decimal final = ParseDecimalOrZero(txtFinal != null ? txtFinal.Text : "");

            using (SqlConnection con = new SqlConnection(Cadena))
            {
                con.Open();
                SqlCommand cmdUpdate = new SqlCommand(@"
                    UPDATE NOTAS SET PARCIAL1=@p1, REC_PARCIAL1=@rp1, PARCIAL2=@p2, REC_PARCIAL2=@rp2, FINAL=@final
                    WHERE LEGAJO=@legajo AND CURSO=@curso", con);

                cmdUpdate.Parameters.AddWithValue("@p1", p1);
                cmdUpdate.Parameters.AddWithValue("@rp1", rp1);
                cmdUpdate.Parameters.AddWithValue("@p2", p2);
                cmdUpdate.Parameters.AddWithValue("@rp2", rp2);
                cmdUpdate.Parameters.AddWithValue("@final", final);
                cmdUpdate.Parameters.AddWithValue("@legajo", legajo);
                cmdUpdate.Parameters.AddWithValue("@curso", cursoNombre);

                int afectados = cmdUpdate.ExecuteNonQuery();
                if (afectados == 0)
                {
                    SqlCommand cmdInsert = new SqlCommand(@"
                        INSERT INTO NOTAS (LEGAJO, CURSO, PARCIAL1, REC_PARCIAL1, PARCIAL2, REC_PARCIAL2, FINAL)
                        VALUES (@legajo, @curso, @p1, @rp1, @p2, @rp2, @final)", con);

                    cmdInsert.Parameters.AddWithValue("@legajo", legajo);
                    cmdInsert.Parameters.AddWithValue("@curso", cursoNombre);
                    cmdInsert.Parameters.AddWithValue("@p1", p1);
                    cmdInsert.Parameters.AddWithValue("@rp1", rp1);
                    cmdInsert.Parameters.AddWithValue("@p2", p2);
                    cmdInsert.Parameters.AddWithValue("@rp2", rp2);
                    cmdInsert.Parameters.AddWithValue("@final", final);

                    cmdInsert.ExecuteNonQuery();
                }
            }

            MostrarMensaje("Notas guardadas correctamente.");
        }

        private decimal ParseDecimalOrZero(string s)
        {
            decimal v;
            if (decimal.TryParse(s != null ? s.Trim() : "", out v))
                return v;
            return 0;
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx");
        }
        private string ObtenerCursoProfesor()
        {
            if (Session["id_curso"] != null)
            {
                using (SqlConnection con = new SqlConnection(Cadena))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT CURSO FROM CURSOS WHERE ID_CURSO=@id", con);
                    cmd.Parameters.AddWithValue("@id", Session["id_curso"]);
                    object o = cmd.ExecuteScalar();
                    if (o != null) return o.ToString();
                }
            }
            return "";
        }

        void MostrarMensaje(string mensaje)
        {
            Response.Write("<script>alert('" + mensaje.Replace("'", "\\'") + "');</script>");
        }
    }
}
        