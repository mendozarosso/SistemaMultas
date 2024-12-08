 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using SistemaMultas.Models;
using SistemaMultas.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaMultas.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginViewModel LoginInput { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Cedula == LoginInput.Cedula && u.Activo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(LoginInput.Clave, usuario.Clave))
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Cedula),
                new Claim(ClaimTypes.Role, usuario.Tipo)
            };

            await HttpContext.SignInAsync(
                new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")));

            return RedirectToPage("/Index");
        }
    }
}