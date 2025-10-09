using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoPracticaProfecional
{
    public partial class Calendario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarAnios();
                CargarCalendario();
                MostrarFechaActual();
            }
        }

        private void CargarAnios()
        {
            ddlAnio.Items.Clear();
            int anioActual = DateTime.Now.Year;

            // Agregar años desde 2020 hasta 2030
            for (int anio = 2020; anio <= 2030; anio++)
            {
                ddlAnio.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
            }

            // Seleccionar año actual
            ddlAnio.SelectedValue = anioActual.ToString();
        }

        private void CargarCalendario()
        {
            if (!string.IsNullOrEmpty(ddlAnio.SelectedValue))
            {
                int anioSeleccionado = int.Parse(ddlAnio.SelectedValue);
                DateTime fechaActual = DateTime.Today;

                // Si el año seleccionado es el año actual, mostrar el mes actual
                // Si no, mostrar enero del año seleccionado
                if (anioSeleccionado == fechaActual.Year)
                {
                    calFeriados.VisibleDate = new DateTime(anioSeleccionado, fechaActual.Month, 1);
                }
                else
                {
                    calFeriados.VisibleDate = new DateTime(anioSeleccionado, 1, 1);
                }

                // Seleccionar fecha actual solo si estamos en el año actual
                if (anioSeleccionado == fechaActual.Year)
                {
                    calFeriados.SelectedDate = fechaActual;
                }
                else
                {
                    calFeriados.SelectedDate = DateTime.MinValue; // No seleccionar ninguna fecha
                }

                ActualizarResumen();
            }
        }

        private void MostrarFechaActual()
        {
            DateTime fechaActual = DateTime.Today;
            lblFechaActual.Text = "Fecha actual: " + fechaActual.ToString("dddd, dd 'de' MMMM 'de' yyyy");

            // También mostrar en qué mes y año está posicionado el calendario
            string mesPosicion = calFeriados.VisibleDate.ToString("MMMM 'de' yyyy");
            lblFechaActual.Text += " | Calendario mostrando: " + mesPosicion;
        }

        protected void ddlAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCalendario();
            MostrarFechaActual(); // Actualizar la información mostrada
        }

        protected void calFeriados_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime fecha = e.Day.Date;
            bool esFeriado = EsFeriado(fecha);
            bool esFinDeSemana = fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
            bool esNoLaborable = esFeriado || esFinDeSemana;
            bool esHoy = fecha.Date == DateTime.Today;
            bool esMesActual = calFeriados.VisibleDate.Year == DateTime.Today.Year &&
                              calFeriados.VisibleDate.Month == DateTime.Today.Month;

            // Agregar tooltip informativo
            e.Cell.ToolTip = ObtenerTooltipFecha(fecha, esFeriado, esFinDeSemana);

            // Estilo para hoy (siempre se aplica)
            if (esHoy)
            {
                e.Cell.CssClass += " calendar-today";

                // Si estamos en el mes actual, agregar un indicador especial
                if (esMesActual)
                {
                    e.Cell.Controls.Add(new LiteralControl("<br/><small>HOY</small>"));
                }
            }

            // Estilos para feriados y fines de semana
            if (esFeriado)
            {
                e.Cell.CssClass += " feriado";
                e.Cell.Controls.Add(new LiteralControl("<br/><small>FERIADO</small>"));
            }
            else if (esFinDeSemana)
            {
                e.Cell.CssClass += " no-laborable";
                e.Cell.Controls.Add(new LiteralControl("<br/><small>FIN SEMANA</small>"));
            }
        }

        // Los demás métodos (EsFeriado, CalcularPascua, ObtenerTooltipFecha, etc.)
        // se mantienen igual que en el código anterior...
        private bool EsFeriado(DateTime fecha)
        {
            // Lista de feriados fijos (puedes expandir esta lista)
            var feriadosFijos = new List<DateTime>
            {
                new DateTime(fecha.Year, 1, 1),   // Año Nuevo
                new DateTime(fecha.Year, 5, 1),    // Día del Trabajo
                new DateTime(fecha.Year, 7, 9),    // Día de la Independencia
                new DateTime(fecha.Year, 12, 25)   // Navidad
            };

            // Feriados trasladables (ejemplo para Argentina)
            var pascua = CalcularPascua(fecha.Year);
            var feriadosTrasladables = new List<DateTime>
            {
                pascua.AddDays(-2),  // Viernes Santo
                pascua.AddDays(-48), // Carnaval
                pascua.AddDays(-47), // Carnaval
                pascua.AddDays(26)   // Malvinas
            };

            // Verificar si la fecha es feriado
            return feriadosFijos.Contains(fecha) || feriadosTrasladables.Contains(fecha);
        }

        private DateTime CalcularPascua(int anio)
        {
            // Algoritmo para calcular Domingo de Pascua
            int a = anio % 19;
            int b = anio / 100;
            int c = anio % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int mes = (h + l - 7 * m + 114) / 31;
            int dia = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(anio, mes, dia);
        }

        private string ObtenerTooltipFecha(DateTime fecha, bool esFeriado, bool esFinDeSemana)
        {
            string nombreFeriado = esFeriado ? ObtenerNombreFeriado(fecha) : "";

            if (esFeriado)
            {
                return nombreFeriado + " - NO LABORABLE";
            }
            else if (esFinDeSemana)
            {
                return fecha.DayOfWeek.ToString() + " - NO LABORABLE";
            }
            else
            {
                return fecha.DayOfWeek.ToString() + " - DÍA LABORABLE";
            }
        }

        private string ObtenerNombreFeriado(DateTime fecha)
        {
            // Mapear fechas a nombres de feriados
            var feriados = new Dictionary<string, string>
            {
                { "01-01", "Año Nuevo" },
                { "05-01", "Día del Trabajo" },
                { "07-09", "Día de la Independencia" },
                { "12-25", "Navidad" }
            };

            string clave = fecha.ToString("MM-dd");
            return feriados.ContainsKey(clave) ? feriados[clave] : "Feriado";
        }

        private void ActualizarResumen()
        {
            if (!string.IsNullOrEmpty(ddlAnio.SelectedValue))
            {
                int anio = int.Parse(ddlAnio.SelectedValue);
                int totalFeriados = ContarFeriados(anio);
                int totalFinesSemana = ContarFinesSemana(anio);
                int totalNoLaborables = totalFeriados + totalFinesSemana;
                int totalLaborables = 365 - totalNoLaborables;

                // Ajustar para año bisiesto
                if (DateTime.IsLeapYear(anio))
                {
                    totalLaborables = 366 - totalNoLaborables;
                }

                lblResumen.Text = string.Format(
                    "Resumen {0}: {1} días laborables, {2} fines de semana, {3} días feriados. Total no laborables: {4}",
                    anio, totalLaborables, totalFinesSemana, totalFeriados, totalNoLaborables
                );
            }
        }

        private int ContarFeriados(int anio)
        {
            int count = 0;
            DateTime fecha = new DateTime(anio, 1, 1);
            DateTime finAnio = new DateTime(anio, 12, 31);

            while (fecha <= finAnio)
            {
                if (EsFeriado(fecha))
                    count++;
                fecha = fecha.AddDays(1);
            }
            return count;
        }

        private int ContarFinesSemana(int anio)
        {
            int count = 0;
            DateTime fecha = new DateTime(anio, 1, 1);
            DateTime finAnio = new DateTime(anio, 12, 31);

            while (fecha <= finAnio)
            {
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                    count++;
                fecha = fecha.AddDays(1);
            }
            return count;
        }
    }
}