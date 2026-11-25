
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.Models;
using S.A_Esquadrias.DTOs;

namespace S.A_Esquadrias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher;
        private readonly IConfiguration _configuration;

        public UsuarioController(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _hasher = new PasswordHasher<Usuario>();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null)
            {
                return Unauthorized("E-mail ou senha incorretos.");
            }

            var resultado = _hasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                loginDTO.Senha
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                return Unauthorized("E-mail ou senha incorretos.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var chave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credenciais = new SigningCredentials(
                chave,
                SecurityAlgorithms.HmacSha256
            );

            var validade = DateTime.UtcNow.AddDays(
                Convert.ToDouble(_configuration["Jwt:ExpireDays"])
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: validade,
                signingCredentials: credenciais
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),

                usuario = new
                {
                    usuario.Id,
                    usuario.Email,
                    usuario.Perfil
                }
            });
        }

        [HttpGet]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuario
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.Perfil
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuario
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.Perfil
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario);
        }

        
        [HttpPost]
        public async Task<IActionResult> PostUsuario(UsuarioDTO usuarioDTO)
        {
            var existeUsuario = await _context.Usuario.AnyAsync();

            if (existeUsuario && !User.IsInRole("ADM"))
            {
                return Unauthorized("Somente um administrador pode cadastrar usuários.");
            }

            var perfil = existeUsuario ? usuarioDTO.Perfil : "ADM";

            var usuario = new Usuario
            {
                Email = usuarioDTO.Email,
                Perfil = perfil
            };

            usuario.Senha = _hasher.HashPassword(usuario, usuarioDTO.Senha);

            _context.Usuario.Add(usuario);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuario.Id },
                new
                {
                    usuario.Id,
                    usuario.Email,
                    usuario.Perfil
                }
            );
        }

    
        [HttpPut("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            var usuarioExistente = await _context.Usuario.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            usuarioExistente.Email = usuarioDTO.Email;
            usuarioExistente.Perfil = usuarioDTO.Perfil;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Senha))
            {
                usuarioExistente.Senha = _hasher.HashPassword(
                    usuarioExistente,
                    usuarioDTO.Senha
                );
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _context.Usuario.Remove(usuario);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
