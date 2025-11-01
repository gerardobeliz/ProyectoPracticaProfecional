using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace proyectoPracticaProfecional
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        // Clase auxiliar DENTRO de la clase Principal
        public class MenuItem
        {
            public string Nombre { get; set; }
            public string Url { get; set; }
            public string Icono { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipo"] != null)
            {
                Label lbl = (Label)FindControl("lbtipo");
                if (lbl != null)
                    lbl.Text = Session["tipo"].ToString();
            }//hasta aca la etiqueta para el tipo
            
            if (Session["Usuario"] != null)
            {
                Label lbl = (Label)FindControl("lblNombreUsuario");
                if (lbl != null)
                    lbl.Text = Session["Usuario"].ToString();
            }//hasta aca la etiqueta para el nombre de usuario
            if (!IsPostBack)
            {
                CargarMenuDinamico();  // Llamar al método cuando se carga la página
            }
        }

        // Método que crea el menú dinámico
        public void CargarMenuDinamico()
        {
            // 1. Crear la lista de items del menú
            // 2. Recorrer cada item y crear el HTML
            string tipo = Session["tipo"].ToString();

            if (tipo == "profesor")
            {
                var menuprofesor = new List<MenuItem> {  
               
                new MenuItem { 
                    Nombre = "calificar", 
                    Url = "calificar.aspx", 
                    Icono = "fa-briefcase" 
                },
                new MenuItem { 
                    Nombre = "Asistencia", 
                    Url = "AsistenciaProfe.aspx", 
                    Icono = "fa-sticky-note" 
                },
                new MenuItem { 
                    Nombre = "Calendario", 
                    Url = "Calendario.aspx", 
                    Icono = "fa-calendar" 
                },
              };
                CrearMenu(menuprofesor);
            }//cierre if profesor
            
            if (tipo == "preceptor")
            {
                var menuprece = new List<MenuItem> {  
              new MenuItem { 
                        Nombre = "Alta Alumnos", 
                        Url = "AltaAlumnos.aspx", 
                        Icono = "fa-user" 
                    },
                                  new MenuItem { 
                        Nombre = "Modificar/eliminar Alumno", 
                        Url = "ModificarAlumno.aspx", 
                        Icono = "fa-trash" 
                    },
                     new MenuItem { 
                        Nombre = "asistencia", 
                        Url = "Asistencia.aspx", 
                        Icono = "fa-file" 
                    },
                     new MenuItem { 
                        Nombre = "Calendario", 
                        Url = "Calendario.aspx", 
                        Icono = "fa-calendar" 
                    },
                      
              };
                CrearMenu(menuprece);
            }//cierre if preceptor
         
            if (tipo == "directivo")
            {
                var menudirectivo = new List<MenuItem> {  
              new MenuItem { 
                        Nombre = "Alta Personal", 
                        Url = "AltaPersonal.aspx", 
                        Icono = "fa-briefcase" 
                    },
                    new MenuItem { 
                        Nombre = "Buscar Editar personal", 
                        Url = "EditarPersonal.aspx", 
                        Icono = "fa-sticky-note" 
                    },
                    
                     new MenuItem { 
                        Nombre = "Eliminar Personal", 
                        Url = "EliminarEmpleado.aspx", 
                        Icono = "fa-trash"  
                    },
              };
                CrearMenu(menudirectivo);
            }//cierre if directivo

        }//cierra el metodo 

        private void CrearMenu(List<MenuItem> menu)
        {
            foreach (var item in menu)
            {
                // Crear elemento <li>
                HtmlGenericControl li = new HtmlGenericControl("li");

                // Crear elemento <a> con el enlace
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes["href"] = item.Url;

                // Crear elemento <i> para el icono
                HtmlGenericControl icon = new HtmlGenericControl("i");
                // CORRECCIÓN: Usar string.Format en lugar de interpolación $
                icon.Attributes["class"] = string.Format("fa {0} mr-3", item.Icono);
                a.Controls.Add(icon);

                // Agregar texto al enlace
                a.Controls.Add(new LiteralControl(item.Nombre));

                // Agregar enlace al elemento de lista
                li.Controls.Add(a);

                // Agregar item al menú en la página
                dynamicMenu.Controls.Add(li);
            }
        }
    }
}


