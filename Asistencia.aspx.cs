using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data;

namespace proyectoPracticaProfecional
{
    public partial class Asistencia : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Cadenaint46"].ConnectionString;
        private List<DateTime> DiasDelMes = new List<DateTime>();
        private List<Alumno> Alumnos = new List<Alumno>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCursos();
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                pnlEmpty.Visible = true;
                tblAsistencia.Visible = false;
            }
        }

        private void CargarCursos()
        {
            try
            {
                ddlCurso.Items.Clear();
                ddlCurso.Items.Add(new ListItem("Seleccionar curso", ""));

                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();

                    string query = "SELECT DISTINCT curso FROM CURSOS ";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombreCurso = reader["curso"].ToString();
                                ddlCurso.Items.Add(new ListItem(nombreCurso, nombreCurso));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar cursos: " + ex.Message, "error");
                //CargarCursosEjemplo();
            }
        }

        //private void CargarCursosEjemplo()
        //{
        //    ddlCurso.Items.Clear();
        //    ddlCurso.Items.Add(new ListItem("Seleccionar curso", ""));
        //    ddlCurso.Items.Add(new ListItem("Programación", "Programación"));
        //    ddlCurso.Items.Add(new ListItem("Historia", "Historia"));
        //    ddlCurso.Items.Add(new ListItem("Biología", "Biología"));
        //    ddlCurso.Items.Add(new ListItem("Psicopedagogía", "Psicopedagogía"));
        //}

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCurso.SelectedValue == "")
                {
                    MostrarMensaje("Por favor seleccione un curso", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(txtFecha.Text))
                {
                    MostrarMensaje("Por favor seleccione una fecha", "warning");
                    return;
                }

                DateTime fechaSeleccionada = DateTime.Parse(txtFecha.Text);
                DiasDelMes = ObtenerDiasLaborablesMes(fechaSeleccionada.Year, fechaSeleccionada.Month);
                CargarAlumnosDelCurso();
                GenerarGridAsistencia();

                pnlEmpty.Visible = false;
                tblAsistencia.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar datos: " + ex.Message, "error");
            }
        }

        private void CargarAlumnosDelCurso()
        {
            Alumnos = new List<Alumno>();

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                conexion.Open();

                string query = "select  nombre, apellido from ALUMNOS inner join CURSOS on ALUMNOS.LEGAJO=CURSOS.LEGAJO WHERE CURSO='"+ddlCurso.Text+"'";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Curso", ddlCurso.SelectedValue);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alumno alumno = new Alumno
                            {
                               
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString()
                            };
                            Alumnos.Add(alumno);
                        }
                    }
                }
            }

            if (Alumnos.Count == 0)
            {
                MostrarMensaje("No se encontraron alumnos para el curso seleccionado", "warning");
            }
        }

        private void GenerarGridAsistencia()
        {
            tblAsistencia.Rows.Clear();

            TableHeaderRow headerRow = new TableHeaderRow();
            headerRow.CssClass = "grid-header";

            TableHeaderCell headerAlumno = new TableHeaderCell();
            headerAlumno.Text = "Alumno";
            headerAlumno.CssClass = "header-alumno";
            headerAlumno.Style.Add("min-width", "250px");
            headerRow.Cells.Add(headerAlumno);

            CultureInfo cultura = new CultureInfo("es-ES");

            foreach (DateTime dia in DiasDelMes)
            {
                TableHeaderCell headerDia = new TableHeaderCell();

                string nombreDia = cultura.DateTimeFormat.GetAbbreviatedDayName(dia.DayOfWeek);
                string fechaCorta = dia.ToString("d MMM", cultura);

                headerDia.Text = string.Format("{0}<br/><small>{1}</small>", nombreDia, fechaCorta);
                headerDia.CssClass = "header-dia";
                headerDia.ToolTip = dia.ToString("dddd d 'de' MMMM 'de' yyyy", cultura);
                headerRow.Cells.Add(headerDia);
            }

            tblAsistencia.Rows.Add(headerRow);

            foreach (Alumno alumno in Alumnos)
            {
                TableRow row = new TableRow();
                row.CssClass = "grid-row";

                TableCell cellAlumno = new TableCell();
                cellAlumno.Text = alumno.Apellido + ", " + alumno.Nombre;
                cellAlumno.CssClass = "student-name";
                row.Cells.Add(cellAlumno);

                for (int i = 0; i < DiasDelMes.Count; i++)
                {
                    TableCell cellDia = new TableCell();
                    cellDia.CssClass = "dia-cell";

                    // CREAR Y CONFIGURAR EL DROPDOWNLIST
                    DropDownList ddl = CrearDropDownListAsistencia(alumno.Legajo, i, DiasDelMes[i]);

                    cellDia.Controls.Add(ddl);
                    row.Cells.Add(cellDia);
                }

                tblAsistencia.Rows.Add(row);
            }

            totalAlumnos.InnerText = Alumnos.Count.ToString();
            ActualizarContadoresResumen();

            ScriptManager.RegisterStartupScript(this, GetType(), "initStyles",
                "setTimeout(function(){ forzarTextoDefinitivo(); actualizarResumen(); }, 100);", true);
        }

        private DropDownList CrearDropDownListAsistencia(int legajo, int indiceDia, DateTime fecha)
        {
            DropDownList ddl = new DropDownList();
            ddl.CssClass = "dropdown-asistencia";
            ddl.ID = "ddl_" + legajo.ToString() + "_" + indiceDia.ToString();

            // CONFIGURAR LAS OPCIONES DEL DROPDOWN
            ConfigurarOpcionesDropDown(ddl);

            // CARGAR VALOR EXISTENTE O VALOR POR DEFECTO
            CargarValorDropDown(ddl, legajo, fecha);

            // AGREGAR EVENTO ONCHANGE
            ddl.Attributes["onchange"] = "aplicarEstiloDropdown(this); actualizarResumen();";

            return ddl;
        }

        private void ConfigurarOpcionesDropDown(DropDownList ddl)
        {
            // LIMPIAR OPCIONES EXISTENTES
            ddl.Items.Clear();

            // AGREGAR OPCIONES CON VALOR Y TEXTO
            ddl.Items.Add(new ListItem("P", "P"));
            ddl.Items.Add(new ListItem("A", "A"));
            ddl.Items.Add(new ListItem("J", "J"));

            // CONFIGURAR PROPIEDADES ADICIONALES
            ddl.Width = Unit.Pixel(70);
            ddl.AutoPostBack = false;
        }

        private void CargarValorDropDown(DropDownList ddl, int legajo, DateTime fecha)
        {
            try
            {
                // OBTENER VALOR EXISTENTE DE LA BASE DE DATOS
                string estadoExistente = ObtenerEstadoAsistencia(legajo, fecha);

                if (!string.IsNullOrEmpty(estadoExistente))
                {
                    // SI EXISTE UN VALOR EN LA BD, SELECCIONARLO
                    if (ddl.Items.FindByValue(estadoExistente) != null)
                    {
                        ddl.SelectedValue = estadoExistente;
                    }
                    else
                    {
                        ddl.SelectedValue = "P"; // VALOR POR DEFECTO
                    }
                }
                else
                {
                    // SI NO EXISTE, USAR VALOR POR DEFECTO
                    ddl.SelectedValue = "P";
                }
            }
            catch (Exception ex)
            {
                // EN CASO DE ERROR, USAR VALOR POR DEFECTO
                ddl.SelectedValue = "P";
                System.Diagnostics.Debug.WriteLine("Error al cargar valor dropdown: " + ex.Message);
            }
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

        private string ObtenerEstadoAsistencia(int legajo, DateTime fecha)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();
                    string query = "SELECT estado FROM ASISTENCIA WHERE legajo_alumno = @Legajo AND fecha = @Fecha";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Legajo", legajo);
                        cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                        object resultado = cmd.ExecuteScalar();
                        return resultado != null ? resultado.ToString() : null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener asistencia: " + ex.Message);
                return null;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarDatos())
                    return;

                // CAPTURAR TODOS LOS VALORES DE LOS DROPDOWNLIST
                Dictionary<string, string> valoresCapturados = CapturarValoresDropDowns();

                int registrosGuardados = 0;

                foreach (Alumno alumno in Alumnos)
                {
                    for (int i = 0; i < DiasDelMes.Count; i++)
                    {
                        string clave = "ddl_" + alumno.Legajo.ToString() + "_" + i.ToString();

                        // BUSCAR EL VALOR EN EL DICCIONARIO
                        if (valoresCapturados.ContainsKey(clave))
                        {
                            string valorSeleccionado = valoresCapturados[clave];

                            if (!string.IsNullOrEmpty(valorSeleccionado))
                            {
                                if (GuardarAsistenciaBD(alumno.Legajo, DiasDelMes[i], valorSeleccionado))
                                    registrosGuardados++;
                            }
                        }
                    }
                }

                MostrarMensaje("Asistencia guardada correctamente. " + registrosGuardados.ToString() + " registros actualizados.", "success");

                // MOSTRAR VALORES CAPTURADOS (PARA DEBUG)
                MostrarValoresCapturados(valoresCapturados);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, "error");
            }
        }

        private Dictionary<string, string> CapturarValoresDropDowns()
        {
            Dictionary<string, string> valores = new Dictionary<string, string>();

            // RECORRER TODAS LAS FILAS DE LA TABLA
            foreach (TableRow row in tblAsistencia.Rows)
            {
                if (row is TableHeaderRow)
                    continue;

                // RECORRER TODAS LAS CELDAS (OMITIENDO LA PRIMERA QUE ES EL NOMBRE)
                for (int i = 1; i < row.Cells.Count; i++)
                {
                    TableCell cell = row.Cells[i];

                    if (cell.Controls.Count > 0 && cell.Controls[0] is DropDownList)
                    {
                        DropDownList ddl = (DropDownList)cell.Controls[0];
                        valores[ddl.ID] = ddl.SelectedValue;
                    }
                }
            }

            return valores;
        }

        private void MostrarValoresCapturados(Dictionary<string, string> valores)
        {
            // ESTE MÉTODO ES PARA DEBUG - PUEDES ELIMINARLO EN PRODUCCIÓN
            System.Diagnostics.Debug.WriteLine("=== VALORES CAPTURADOS DE DROPDOWNS ===");
            foreach (var valor in valores)
            {
                System.Diagnostics.Debug.WriteLine("ID: " + valor.Key + " - Valor: " + valor.Value);
            }
            System.Diagnostics.Debug.WriteLine("Total de dropdowns capturados: " + valores.Count);
        }

        private bool GuardarAsistenciaBD(int legajo, DateTime fecha, string estado)
        {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                conexion.Open();

                string query = "MERGE ASISTENCIA AS target " +
                               "USING (VALUES (@Legajo, @Fecha, @Estado)) AS source (legajo_alumno, fecha, estado) " +
                               "ON target.legajo_alumno = source.legajo_alumno AND target.fecha = source.fecha " +
                               "WHEN MATCHED THEN " +
                               "    UPDATE SET estado = source.estado, fecha_modificacion = GETDATE() " +
                               "WHEN NOT MATCHED THEN " +
                               "    INSERT (legajo_alumno, fecha, estado, fecha_creacion) " +
                               "    VALUES (source.legajo_alumno, source.fecha, source.estado, GETDATE());";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Legajo", legajo);
                    cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private bool ValidarDatos()
        {
            if (Alumnos == null || Alumnos.Count == 0)
            {
                MostrarMensaje("No hay alumnos para guardar", "warning");
                return false;
            }

            if (DiasDelMes == null || DiasDelMes.Count == 0)
            {
                MostrarMensaje("No hay días seleccionados", "warning");
                return false;
            }

            return true;
        }

        private void ActualizarContadoresResumen()
        {
            int presentes = 0;
            int ausentes = 0;
            int justificados = 0;

            foreach (TableRow row in tblAsistencia.Rows)
            {
                if (row is TableHeaderRow)
                    continue;

                for (int i = 1; i < row.Cells.Count; i++)
                {
                    TableCell cell = row.Cells[i];

                    if (cell.Controls.Count > 0 && cell.Controls[0] is DropDownList)
                    {
                        DropDownList ddl = (DropDownList)cell.Controls[0];

                        switch (ddl.SelectedValue)
                        {
                            case "P":
                                presentes++;
                                break;
                            case "A":
                                ausentes++;
                                break;
                            case "J":
                                justificados++;
                                break;
                        }
                    }
                }
            }

            presentCount.InnerText = presentes.ToString();
            absentCount.InnerText = ausentes.ToString();
            justifiedCount.InnerText = justificados.ToString();
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            string script = "alert('" + mensaje.Replace("'", "\\'") + "');";
            ScriptManager.RegisterStartupScript(this, GetType(), tipo, script, true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (Alumnos.Count == 0)
            {
                MostrarMensaje("No hay datos para exportar", "warning");
                return;
            }

            MostrarMensaje("Funcionalidad de exportación a Excel - En desarrollo", "info");
        }
    }

    public class Alumno
    {
        public int Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}