
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.Models;
using S.A_Esquadrias.DTOs;

namespace S.A_Esquadrias.Controllers
{
    [Authorize(Roles = "ADM")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
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
            var usuario = new Usuario
            {
                Email = usuarioDTO.Email,
                Perfil = usuarioDTO.Perfil
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
