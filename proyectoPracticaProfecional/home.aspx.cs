using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;


namespace proyectoPracticaProfecional
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Esto tiene que estar en todos los load de cada pagina para verificar que la session es valida
            // Verificar primero si la sesión existe y no es nula
            if (Session["Usuario"] == null || string.IsNullOrEmpty(Session["Usuario"].ToString()))
            {
                Response.Redirect("default.aspx");
                return; // Importante para detener la ejecución
            }

            string idSession = Session["Usuario"].ToString();
            string val = Request.QueryString["val"] ?? string.Empty;
            string t = Request.QueryString["tipo"] ?? string.Empty;
        }
    }
 }
