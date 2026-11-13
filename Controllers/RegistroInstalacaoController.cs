
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.Models;
using S.A_Esquadrias.DTOs;
using System.Security.Claims;

namespace S.A_Esquadrias.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroInstalacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistroInstalacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "ADM,Montador")]
        public async Task<ActionResult<IEnumerable<RegistroInstalacao>>> GetRegistros()
        {
            var registros = _context.RegistroInstalacao
                .Include(r => r.Instalacao)
                .Include(r => r.Fotos)
                .AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                registros = registros.Where(
                    r => r.Instalacao.IdMontador == idMontador
                );
            }

            var resultado = await registros.ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADM,Montador")]
        public async Task<ActionResult<RegistroInstalacao>> GetRegistro(int id)
        {
            var registro = _context.RegistroInstalacao
                .Include(r => r.Instalacao)
                .Include(r => r.Fotos)
                .AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                registro = registro.Where(
                    r => r.Instalacao.IdMontador == idMontador
                );
            }

            var resultado = await registro
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resultado == null)
            {
                return NotFound("Registro de instalação não encontrado.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "ADM,Montador")]
        public async Task<ActionResult<RegistroInstalacao>> PostRegistro(
            RegistroInstalacaoDTO registroDTO)
        {
            var instalacao = await _context.Instalacao
                .FirstOrDefaultAsync(i => i.Id == registroDTO.IdInstalacao);

            if (instalacao == null)
            {
                return BadRequest("Instalação não encontrada.");
            }

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                if (instalacao.IdMontador != idMontador)
                {
                    return Forbid();
                }
            }

            var registro = new RegistroInstalacao
            {
                IdInstalacao = registroDTO.IdInstalacao,
                TipoProblema = registroDTO.TipoProblema,
                ObservacoesGerais = registroDTO.ObsGerais,
                DescricaoProblema = registroDTO.DescricaoProblema,
                DataRegistro = DateTime.Now
            };

            _context.RegistroInstalacao.Add(registro);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRegistro),
                new { id = registro.Id },
                registro
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            var registro = await _context.RegistroInstalacao
                .FindAsync(id);

            if (registro == null)
            {
                return NotFound("Registro de instalação não encontrado.");
            }

            _context.RegistroInstalacao.Remove(registro);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
