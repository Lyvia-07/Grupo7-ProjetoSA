
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.DTOs;
using S.A_Esquadrias.Models;
using System.Security.Claims;

namespace S.A_Esquadrias.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FotoInstalacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FotoInstalacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "ADM,Montador")]
        public async Task<ActionResult<IEnumerable<FotoInstalacaoDTO>>> GetFotos()
        {
            var fotos = _context.FotoInstalacao
                .Include(f => f.RegistroInstalacao)
                .ThenInclude(r => r.Instalacao)
                .AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                fotos = fotos.Where(
                    f => f.RegistroInstalacao.Instalacao.IdMontador == idMontador
                );
            }

            var resultado = await fotos
                .Select(f => new FotoInstalacaoDTO
                {
                    Id = f.Id,
                    IdRegistro = f.IdRegistro,
                    CaminhoArquivo = f.CaminhoArquivo,
                    DataEnvio = f.DataEnvio
                })
                .ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADM,Montador")]
        public async Task<ActionResult<FotoInstalacaoDTO>> GetFoto(int id)
        {
            var foto = _context.FotoInstalacao
                .Include(f => f.RegistroInstalacao)
                .ThenInclude(r => r.Instalacao)
                .AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                foto = foto.Where(
                    f => f.RegistroInstalacao.Instalacao.IdMontador == idMontador
                );
            }

            var resultado = await foto
                .Where(f => f.Id == id)
                .Select(f => new FotoInstalacaoDTO
                {
                    Id = f.Id,
                    IdRegistro = f.IdRegistro,
                    CaminhoArquivo = f.CaminhoArquivo,
                    DataEnvio = f.DataEnvio
                })
                .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound("Foto não encontrada.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "ADM,Montador")]
        public async Task<ActionResult<FotoInstalacaoDTO>> CreateFoto(
            CriarFotoInstalacaoDTO fotoDto)
        {
            var registro = await _context.RegistroInstalacao
                .Include(r => r.Instalacao)
                .FirstOrDefaultAsync(r => r.Id == fotoDto.IdRegistro);

            if (registro == null)
            {
                return BadRequest("Esse registro de instalação não foi encontrado.");
            }

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                if (registro.Instalacao.IdMontador != idMontador)
                {
                    return Forbid();
                }
            }

            var foto = new FotoInstalacao
            {
                IdRegistro = fotoDto.IdRegistro,
                CaminhoArquivo = fotoDto.CaminhoArquivo,
                DataEnvio = DateTime.Now
            };

            _context.FotoInstalacao.Add(foto);

            await _context.SaveChangesAsync();

            var resposta = new FotoInstalacaoDTO
            {
                Id = foto.Id,
                IdRegistro = foto.IdRegistro,
                CaminhoArquivo = foto.CaminhoArquivo,
                DataEnvio = foto.DataEnvio
            };

            return CreatedAtAction(
                nameof(GetFoto),
                new { id = foto.Id },
                resposta
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteFoto(int id)
        {
            var foto = await _context.FotoInstalacao.FindAsync(id);

            if (foto == null)
            {
                return NotFound("Foto não encontrada.");
            }

            _context.FotoInstalacao.Remove(foto);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
