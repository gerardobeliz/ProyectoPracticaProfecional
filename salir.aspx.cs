using System;
using System.Web;
using System.Web.Security;

namespace proyectoPracticaProfecional
{
    public partial class salir : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Esta página solo muestra la confirmación
            // La acción de logout se ejecuta en btnConfirmarSalir_Click

        }

        protected void btnConfirmarSalir_Click(object sender, EventArgs e)
        {
            PerformLogout();
        }

        private void PerformLogout()
        {
            try
            {
                // 1. Limpiar todas las variables de sesión
                Session.Clear();
                Session.Abandon();

                // 2. Cerrar la autenticación de Forms
                FormsAuthentication.SignOut();

                // 3. Limpiar cookies de sesión
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }

                // 4. Limpiar cookie de autenticación
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    Response.Cookies[FormsAuthentication.FormsCookieName].Value = string.Empty;
                    Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddMonths(-20);
                }

                // 5. Configurar headers para no cachear
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetNoStore();

                // 6. Redirigir directamente al login
                Response.Redirect("default.aspx?logout=success", true);

            }
            catch (Exception ex)
            {
                // En caso de error, redirigir igualmente al login
                Response.Redirect("default.aspx");
            }
        }
    }
}