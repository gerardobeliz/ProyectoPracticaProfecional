
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
namespace proyectoPracticaProfecional
{
    public partial class AsistenciaProfe : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;
        private List<DateTime> DiasDelMes = new List<DateTime>();
        private List<Alumnos> Alumnoslis = new List<Alumnos>();
        private string GetSafeText(string text)
        {

            // Usar un espacio invisible (non-breaking space) después del texto

            return text + "&nbsp;";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                pnlEmpty.Visible = true;
                tblAsistencia.Visible = false;
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            if (ddlCurso.SelectedValue != "" && !string.IsNullOrEmpty(txtFecha.Text))
            {
                DateTime fechaSeleccionada = DateTime.Parse(txtFecha.Text);
                DiasDelMes = ObtenerDiasLaborablesMes(fechaSeleccionada.Year, fechaSeleccionada.Month);
                CargarDatosAsistencia();
                GenerarGridAsistencia();
                pnlEmpty.Visible = false;
                tblAsistencia.Visible = true;
            }
        }

        private void GenerarGridAsistencia()
        {
            // Limpiar tabla existente
            tblAsistencia.Rows.Clear();

            // Crear header
            TableHeaderRow headerRow = new TableHeaderRow();
            headerRow.CssClass = "grid-header";

            // Celda para "Alumno"
            TableHeaderCell headerAlumno = new TableHeaderCell();
            headerAlumno.Text = "Alumno";
            headerAlumno.CssClass = "header-alumno";
            headerRow.Cells.Add(headerAlumno);

            // Celdas para cada día
            foreach (DateTime dia in DiasDelMes)
            {
                TableHeaderCell headerDia = new TableHeaderCell();
                headerDia.Text = string.Format("{0}<br/><small>{1}</small>",
                    dia.ToString("ddd"),
                    dia.ToString("dd/MM"));
                headerDia.CssClass = "header-dia";
                headerRow.Cells.Add(headerDia);
            }

            tblAsistencia.Rows.Add(headerRow);

            // Crear filas para cada alumno
            foreach (Alumnos alumno in Alumnoslis)
            {
                TableRow row = new TableRow();
                row.CssClass = "grid-row";

                // Celda con nombre del alumno
                TableCell cellAlumno = new TableCell();
                cellAlumno.Text = alumno.Nombre;
                cellAlumno.CssClass = "student-name";
                row.Cells.Add(cellAlumno);

                // Celdas con dropdowns para cada día
                for (int i = 0; i < DiasDelMes.Count; i++)
                {
                    TableCell cellDia = new TableCell();
                    cellDia.CssClass = "dia-cell";

                    DropDownList ddl = new DropDownList();
                    ddl.CssClass = "dropdown-asistencia";
                    ddl.ID = string.Format("ddl_{0}_{1}", alumno.Id, i);

                    // SOLUCIÓN DEFINITIVA: Usar LiteralControl para evitar auto-format
                    ddl.Items.Add(new ListItem(GetSafeText("P"), "P"));
                    ddl.Items.Add(new ListItem(GetSafeText("A"), "A"));
                    ddl.Items.Add(new ListItem(GetSafeText("J"), "J"));

                    // Valor por defecto
                    ddl.SelectedValue = "P";

                    // Evento para cambiar estilo
                    ddl.Attributes["onchange"] = "aplicarEstiloDropdown(this)";

                    cellDia.Controls.Add(ddl);
                    row.Cells.Add(cellDia);
                }

                tblAsistencia.Rows.Add(row);
            }

            totalAlumnos.InnerText = Alumnoslis.Count.ToString();
        }

        private List<DateTime> ObtenerDiasLaborablesMes(int año, int mes)
        {
            List<DateTime> diasLaborables = new List<DateTime>();
            DateTime primerDia = new DateTime(año, mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            for (DateTime dia = primerDia; dia <= ultimoDia; dia = dia.AddDays(1))
            {
                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasLaborables.Add(dia);
                }
            }
            return diasLaborables; 
        }

        private void CargarDatosAsistencia()
        {
            
            {
                Alumnoslis = new List<Alumnos>();


                
                using (SqlConnection cadena = new SqlConnection(Cadena))
                {
                    cadena.Open();
                    string query = "SELECT legajo, nombre FROM ALUMNOS";

                    using (SqlCommand cmd = new SqlCommand(query, cadena))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Alumnos alumno = new Alumnos
                                {
                                    Id = Convert.ToInt32(reader["LEGAJO"]),
                                    Nombre = reader["NOMBRE"].ToString()
                                };
                                Alumnoslis.Add(alumno);
                            }
                        }
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = 0;

                // Recorrer la tabla para guardar (empezar desde 1 porque 0 es el header)
                for (int i = 1; i < tblAsistencia.Rows.Count; i++)
                {
                    TableRow row = tblAsistencia.Rows[i];

                    if (rowIndex < Alumnoslis.Count)
                    {
                        string alumnoNombre = Alumnoslis[rowIndex].Nombre;

                        // Recorrer las celdas de días (empezando desde índice 1)
                        for (int j = 1; j < row.Cells.Count; j++)
                        {
                            string controlID = string.Format("ddl_{0}_{1}", Alumnoslis[rowIndex].Id, j - 1);
                            DropDownList ddl = row.Cells[j].FindControl(controlID) as DropDownList;

                            if (ddl != null)
                            {
                                string estado = ddl.SelectedValue;
                                string fecha = DiasDelMes[j - 1].ToString("yyyy-MM-dd");

                                // Guardar en BD
                                if (!string.IsNullOrEmpty(estado))
                                {
                                    string mensaje = string.Format("Alumno: {0}, Fecha: {1}, Estado: {2}",
                                        alumnoNombre, fecha, estado);
                                    System.Diagnostics.Debug.WriteLine(mensaje);
                                }
                            }
                        }
                        rowIndex++;
                    }
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "success",
                    "alert('Asistencia guardada correctamente');", true);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error al guardar: {0}", ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    string.Format("alert('{0}');", errorMessage), true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            tblAsistencia.Rows.Clear();
            pnlEmpty.Visible = true;
            tblAsistencia.Visible = false;
            totalAlumnos.InnerText = "0";
            presentCount.InnerText = "0";
            absentCount.InnerText = "0";
            justifiedCount.InnerText = "0";
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            // Lógica para exportar a Excel
            ScriptManager.RegisterStartupScript(this, GetType(), "excel",
                "alert('Funcionalidad de exportación a Excel - Implementar según necesidades');", true);
        }
    }

    public class Alumnos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}