using Microsoft.AspNetCore.Mvc;
using APIRESTUnityWeb.Models;
using APIRESTUnityWeb.Data;

namespace APIRESTUnityWeb.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor que recibe el contexto de base de datos
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // --------------------------------------
        // POST /auth/register
        // Registra un nuevo usuario
        // --------------------------------------
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // Verificar si ya existe un usuario con el mismo email
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El correo ya está registrado." });
            }

            // Asignar la fecha actual y guardar el nuevo usuario
            user.RegistrationDate = DateTime.UtcNow;
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuario registrado correctamente." });
        }

        // --------------------------------------
        // POST /auth/login
        // Inicia sesión con email y contraseña
        // --------------------------------------
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var foundUser = _context.Users
                .FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (foundUser == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            return Ok(new
            {
                message = "Inicio de sesión exitoso.",
                user = new
                {
                    id = foundUser.UserId,
                    firstName = foundUser.FirstName,
                    lastName = foundUser.LastName,
                    email = foundUser.Email
                }
            });
        }
    }
}
