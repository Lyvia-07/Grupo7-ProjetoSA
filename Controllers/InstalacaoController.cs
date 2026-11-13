
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
    public class InstalacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstalacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "ADM,Vendedor,Montador")]
        public async Task<ActionResult<IEnumerable<InstalacaoDTO>>> GetInstalacoes()
        {
            var instalacoes = _context.Instalacao.AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                instalacoes = instalacoes.Where(
                    i => i.IdMontador == idMontador
                );
            }

            var resultado = await instalacoes
                .Select(i => new InstalacaoDTO
                {
                    Id = i.Id,
                    IdObra = i.IdObra,
                    IdMontador = i.IdMontador,
                    DataInicio = i.DataInicio,
                    DataTermino = i.DataTermino
                })
                .ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADM,Vendedor,Montador")]
        public async Task<ActionResult<InstalacaoDTO>> GetInstalacao(int id)
        {
            var instalacao = _context.Instalacao.AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                instalacao = instalacao.Where(
                    i => i.IdMontador == idMontador
                );
            }

            var resultado = await instalacao
                .Where(i => i.Id == id)
                .Select(i => new InstalacaoDTO
                {
                    Id = i.Id,
                    IdObra = i.IdObra,
                    IdMontador = i.IdMontador,
                    DataInicio = i.DataInicio,
                    DataTermino = i.DataTermino
                })
                .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound("Instalação não encontrada.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "ADM")]
        public async Task<ActionResult<InstalacaoDTO>> CreateInstalacao(
            CriarInstalacaoDTO instalacaoDto)
        {
            var obraExiste = await _context.Obra
                .AnyAsync(o => o.Id == instalacaoDto.IdObra);

            if (!obraExiste)
            {
                return BadRequest("Obra não encontrada.");
            }

            var montadorExiste = await _context.Usuario
                .AnyAsync(u =>
                    u.Id == instalacaoDto.IdMontador &&
                    u.Perfil == "Montador"
                );

            if (!montadorExiste)
            {
                return BadRequest("Montador não encontrado.");
            }

            var instalacao = new Instalacao
            {
                IdObra = instalacaoDto.IdObra,
                IdMontador = instalacaoDto.IdMontador,
                DataInicio = instalacaoDto.DataInicio,
                DataTermino = instalacaoDto.DataTermino
            };

            _context.Instalacao.Add(instalacao);

            await _context.SaveChangesAsync();

            var resposta = new InstalacaoDTO
            {
                Id = instalacao.Id,
                IdObra = instalacao.IdObra,
                IdMontador = instalacao.IdMontador,
                DataInicio = instalacao.DataInicio,
                DataTermino = instalacao.DataTermino
            };

            return CreatedAtAction(
                nameof(GetInstalacao),
                new { id = instalacao.Id },
                resposta
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> UpdateInstalacao(
            int id, CriarInstalacaoDTO instalacaoDto)
        {
            var instalacao = await _context.Instalacao.FindAsync(id);

            if (instalacao == null)
            {
                return NotFound("Instalação não encontrada.");
            }

            var obraExiste = await _context.Obra
                .AnyAsync(o => o.Id == instalacaoDto.IdObra);

            if (!obraExiste)
            {
                return BadRequest("Obra não encontrada.");
            }

            var montadorExiste = await _context.Usuario
                .AnyAsync(u =>
                    u.Id == instalacaoDto.IdMontador &&
                    u.Perfil == "Montador"
                );

            if (!montadorExiste)
            {
                return BadRequest("Montador não encontrado.");
            }

            instalacao.IdObra = instalacaoDto.IdObra;
            instalacao.IdMontador = instalacaoDto.IdMontador;
            instalacao.DataInicio = instalacaoDto.DataInicio;
            instalacao.DataTermino = instalacaoDto.DataTermino;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteInstalacao(int id)
        {
            var instalacao = await _context.Instalacao.FindAsync(id);

            if (instalacao == null)
            {
                return NotFound("Instalação não encontrada.");
            }

            _context.Instalacao.Remove(instalacao);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
