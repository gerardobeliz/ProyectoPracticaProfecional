using System;
using System.Web.UI;

namespace Instituto46
{
    public partial class Default : Page
    {
      

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Aquí procesarías el registro del usuario
                // Por ejemplo, guardar en una base de datos

                // Mostrar mensaje de éxito
                ScriptManager.RegisterStartupScript(this, GetType(), "success",
                    "alert('Formulario enviado correctamente');", true);
            }
        }

        protected void ValidarLongitudUsuario(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Length >= 4;
        }

        protected void ValidarLongitudPassword(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Length >= 6;
        }
    }
}