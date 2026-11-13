
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.DTOs;
using S.A_Esquadrias.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace S.A_Esquadrias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Usuario> _hasher;

        public LoginController(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _hasher = new PasswordHasher<Usuario>();
        }

        [HttpPost]
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

            var perfil = usuario.Perfil.Trim().ToUpperInvariant();

            if (perfil != "ADM" &&
                perfil != "VENDEDOR" &&
                perfil != "MONTADOR")
            {
                return BadRequest("Perfil de usuário inválido.");
            }

            var perfilToken = perfil switch
            {
                "ADM" => "ADM",
                "VENDEDOR" => "Vendedor",
                "MONTADOR" => "Montador",
                _ => perfil
            };

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, perfilToken)
            };

            var chave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credenciais = new SigningCredentials(
                chave,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    Convert.ToDouble(_configuration["Jwt:ExpireDays"] ?? "7")
                ),
                signingCredentials: credenciais
            );

            var tokenGerado = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new
            {
                mensagem = "Login realizado com sucesso!",
                id = usuario.Id,
                email = usuario.Email,
                perfil = perfilToken,
                token = tokenGerado
            });
        }
    }
}
