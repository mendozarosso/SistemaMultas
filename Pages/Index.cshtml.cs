using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SistemaMultas.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Si el usuario está autenticado, redirigir a su dashboard correspondiente
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (User.IsInRole("Agente"))
                    Response.Redirect("/Agente/Index");
                else if (User.IsInRole("Oficina"))
                    Response.Redirect("/Oficina/Index");
                else if (User.IsInRole("Admin"))
                    Response.Redirect("/Admin/Index");
            }
        }
    }
}