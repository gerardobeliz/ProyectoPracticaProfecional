
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
namespace proyectoPracticaProfecional
{
    public partial class AsistenciaProfe : System.Web.UI.Page
    {
        // Usar la cadena de conexión del web.config
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarMesesAnios();
            if (!IsPostBack)
            {
                if (Session["id_personal"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                lblFecha.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
                CargarAlumnosProfesor();
                ActualizarResumenDia(); 
            }
        }

        void CargarAlumnosProfesor()
        {
            using (SqlConnection con = new SqlConnection(Cadena))
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
                Session["id_curso"] = idCurso;

                lblCurso.Text = "Curso: " + nombreCurso;
                dr.Close();

                // ✅ 1) Cargar alumnos
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT legajo, nombre, apellido FROM alumnos WHERE carrera = @carrera", con);
                da.SelectCommand.Parameters.AddWithValue("@carrera", nombreCurso);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "legajo" };
                GridView1.DataBind();
                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "UpdateStatsHoy",
                    "setTimeout(actualizarStatsHoy, 200);",
                    true
                );
                //----------------------------------------------
                // ✅ 2) Consultar asistencias YA cargadas hoy
                //----------------------------------------------
                DateTime hoy = DateTime.Today;

                SqlCommand cmdAsis = new SqlCommand(
                    @"SELECT legajo, presente 
              FROM ASISTENCIA 
              WHERE ID_CURSO = @curso AND FECHA = @fecha", con);
                cmdAsis.Parameters.AddWithValue("@curso", idCurso);
                cmdAsis.Parameters.AddWithValue("@fecha", hoy);

                SqlDataReader drAsis = cmdAsis.ExecuteReader();

                Dictionary<int, int> asistenciasHoy = new Dictionary<int, int>();

                while (drAsis.Read())
                {
                    asistenciasHoy.Add(
                        Convert.ToInt32(drAsis["legajo"]),
                        Convert.ToInt32(drAsis["presente"])
                    );
                }
                drAsis.Close();

                //----------------------------------------------
                // ✅ 3) Marcar los checkbox según asistencia
                //----------------------------------------------
                foreach (GridViewRow row in GridView1.Rows)
                {
                    int legajo = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                    CheckBox chk = row.FindControl("chkPresente") as CheckBox;

                    if (asistenciasHoy.ContainsKey(legajo))
                    {
                        chk.Checked = asistenciasHoy[legajo] == 1; // P = 1
                    }
                    else
                    {
                        chk.Checked = false; // Default ausente
                    }
                }
            }
        }
        void ActualizarResumenDia()
        {
            int presentes = 0;
            int ausentes = 0;

            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chk = row.FindControl("chkPresente") as CheckBox;
                if (chk != null && chk.Checked) presentes++;
                else ausentes++;
            }

            lblPresentesHoy.Text = presentes.ToString();
            lblAusentesHoy.Text = ausentes.ToString();

            int total = presentes + ausentes;
            double porc = (total > 0) ? Math.Round((double)presentes * 100 / total, 2) : 0;
            lblPorcHoy.Text = porc + "%";
        }
        void CargarMesesAnios()
        {
            if (!IsPostBack)
            {
                for (int m = 1; m <= 12; m++)
                    ddlMes.Items.Add(new ListItem(m.ToString(), m.ToString()));

                int añoActual = DateTime.Now.Year;
                for (int a = añoActual - 3; a <= añoActual + 1; a++)
                    ddlAño.Items.Add(new ListItem(a.ToString(), a.ToString()));

                ddlMes.SelectedValue = DateTime.Now.Month.ToString();
                ddlAño.SelectedValue = DateTime.Now.Year.ToString();
            }
        }
        void MostrarMensaje(string mensaje)
        {
            Response.Write("<script>alert('" + mensaje + "');</script>");
        }



        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx");
        }

      
        protected void btnVerAsistencias_Click(object sender, EventArgs e)
        {
            if (Session["id_curso"] == null) return;

            int idCurso = Convert.ToInt32(Session["id_curso"]);
            int mes = Convert.ToInt32(ddlMes.SelectedValue);
            int año = Convert.ToInt32(ddlAño.SelectedValue);

            DateTime primerDia = new DateTime(año, mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            // ✅ Obtener alumnos del curso
            DataTable alumnos = new DataTable();
            using (SqlConnection con = new SqlConnection(Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT LEGAJO, NOMBRE + ' ' + APELLIDO AS Alumno FROM ALUMNOS WHERE CARRERA = @carrera", con);

                da.SelectCommand.Parameters.AddWithValue("@carrera", lblCurso.Text.Replace("Curso: ", "").Trim());
                da.Fill(alumnos);
            }

            // ✅ Crear DataTable para la grilla del mes
            DataTable dtMes = new DataTable();
            dtMes.Columns.Add("Alumno");

            // ✅ Crear columnas por día hábil
            List<DateTime> diasHabiles = new List<DateTime>();
            for (DateTime d = primerDia; d <= ultimoDia; d = d.AddDays(1))
            {
                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                {
                    string colFecha = d.ToString("dd/MM");
                    dtMes.Columns.Add(colFecha);
                    diasHabiles.Add(d);
                }
            }

            using (SqlConnection con = new SqlConnection(Cadena))
            {
                con.Open();
                SqlCommand cmdAsis = new SqlCommand(@"
            SELECT LEGAJO, FECHA, PRESENTE 
            FROM ASISTENCIA 
            WHERE ID_CURSO = @curso
            AND MONTH(FECHA) = @mes
            AND YEAR(FECHA) = @año", con);

                cmdAsis.Parameters.AddWithValue("@curso", idCurso);
                cmdAsis.Parameters.AddWithValue("@mes", mes);
                cmdAsis.Parameters.AddWithValue("@año", año);

                SqlDataReader dr = cmdAsis.ExecuteReader();

                Dictionary<int, Dictionary<DateTime, int>> asistencias = new Dictionary<int, Dictionary<DateTime, int>>();

                while (dr.Read())
                {
                    int legajo = Convert.ToInt32(dr["legajo"]);
                    DateTime fecha = Convert.ToDateTime(dr["fecha"]);
                    int est = Convert.ToInt32(dr["presente"]);

                    if (!asistencias.ContainsKey(legajo))
                        asistencias[legajo] = new Dictionary<DateTime, int>();

                    asistencias[legajo][fecha] = est;
                }
                dr.Close();

                // ✅ Llenar DataTable
                foreach (DataRow al in alumnos.Rows)
                {
                    int legajo = Convert.ToInt32(al["legajo"]);
                    DataRow row = dtMes.NewRow();
                    row["Alumno"] = al["Alumno"].ToString();

                    foreach (var d in diasHabiles)
                    {
                        string col = d.ToString("dd/MM");
                        string valor = "A"; // default ausente

                        if (asistencias.ContainsKey(legajo) && asistencias[legajo].ContainsKey(d))
                        {
                            int estado = asistencias[legajo][d];
                            if (estado == 1) valor = "P";
                            else if (estado == 2) valor = "J";
                        }

                        row[col] = valor;
                    }

                    dtMes.Rows.Add(row);
                }
            }

            gvHistorico.DataSource = dtMes;
            gvHistorico.DataBind();
            gvHistorico.Visible = true;
            // === Cálculo de estadísticas del curso ===
            int totalPresentesCurso = 0;
            int totalAusentesCurso = 0;
            int totalJustificadosCurso = 0;

            // Recorremos la tabla ya generada
            foreach (DataRow row in dtMes.Rows)
            {
                int presentes = 0;
                int ausentes = 0;
                int justificados = 0;

                foreach (var d in diasHabiles)
                {
                    string col = d.ToString("dd/MM");
                    string valor = row[col].ToString().Trim();

                    if (valor == "P") presentes++;
                    else if (valor == "A") ausentes++;
                    else if (valor == "J") justificados++;
                }

                totalPresentesCurso += presentes;
                totalAusentesCurso += ausentes;
                totalJustificadosCurso += justificados;
            }

            int totalRegistros = totalPresentesCurso + totalAusentesCurso + totalJustificadosCurso;
            double porcentajeAsistencia = totalRegistros > 0 ? Math.Round((double)totalPresentesCurso * 100 / totalRegistros, 2) : 0;
            double porcentajeInasistencia = totalRegistros > 0 ? Math.Round((double)totalAusentesCurso * 100 / totalRegistros, 2) : 0;

            // Asistencia media = total presentes / cantidad de días hábiles
            double asistenciaMedia = diasHabiles.Count > 0 ? Math.Round((double)totalPresentesCurso / diasHabiles.Count, 2) : 0;

            // Mostrar stats
            lblAsistenciaTotal.Text = totalPresentesCurso.ToString();
            lblInasistenciaTotal.Text = totalAusentesCurso.ToString();
            lblPorcAsistencia.Text = porcentajeAsistencia + "%";
            lblPorcInasistencia.Text = porcentajeInasistencia + "%";
            lblAsistenciaMedia.Text = asistenciaMedia + " días";

            // Mostrar panel
            statsContainer.Style["display"] = "block";
        }
        protected void gvHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int totalPresentes = 0;
                int totalAusentes = 0;
                int totalJustificados = 0;

                // Recorremos columnas dinámicas
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    string valor = e.Row.Cells[i].Text.Trim();

                    switch (valor)
                    {
                        case "P":
                            e.Row.Cells[i].CssClass = "presente";
                            totalPresentes++;
                            break;

                        case "A":
                            e.Row.Cells[i].CssClass = "ausente";
                            totalAusentes++;
                            break;

                        case "J":
                            e.Row.Cells[i].CssClass = "justificado";
                            totalJustificados++;
                            break;
                    }
                }

                int dias = totalPresentes + totalAusentes + totalJustificados;
                double porcentaje = dias > 0 ? Math.Round((double)totalPresentes / dias * 100, 2) : 0;

                // Agregar totals
                e.Row.Cells.Add(new TableCell() { Text = totalPresentes.ToString(), CssClass = "total-col" });
                e.Row.Cells.Add(new TableCell() { Text = totalAusentes.ToString(), CssClass = "total-col" });
                e.Row.Cells.Add(new TableCell() { Text = totalJustificados.ToString(), CssClass = "total-col" });
                e.Row.Cells.Add(new TableCell() { Text = porcentaje + "%", CssClass = "total-col" });
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Agregamos headers de totales
                e.Row.Cells.Add(new TableCell() { Text = "P", CssClass = "total-col" });
                e.Row.Cells.Add(new TableCell() { Text = "A", CssClass = "total-col" });
                e.Row.Cells.Add(new TableCell() { Text = "J", CssClass = "total-col" });
                e.Row.Cells.Add(new TableCell() { Text = "% Presencia", CssClass = "total-col" });
            }
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
                            ActualizarResumenDia();
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
                Page.ClientScript.RegisterStartupScript(
    this.GetType(),
    "UpdateStatsHoyAfterSave",
    "setTimeout(actualizarStatsHoy, 500);",
    true
);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar asistencia: " + ex.Message);
            }
        }
    }
}
