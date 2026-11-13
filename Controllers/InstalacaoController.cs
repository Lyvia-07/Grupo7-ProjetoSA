using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.DTOs;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Controllers
{
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
        public async Task<ActionResult<IEnumerable<InstalacaoDTO>>> GetInstalacoes()
        {
            return await _context.Instalacao
                .Select(i => new InstalacaoDTO
                {
                    Id = i.Id,
                    IdObra = i.IdObra,
                    IdMontador = i.IdMontador,
                    DataInicio = i.DataInicio,
                    DataTermino = i.DataTermino
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstalacaoDTO>> GetInstalacao(int id)
        {
            var instalacao = await _context.Instalacao.FindAsync(id);

            if (instalacao == null)
            {
                return NotFound();
            }

            return new InstalacaoDTO
            {
                Id = instalacao.Id,
                IdObra = instalacao.IdObra,
                IdMontador = instalacao.IdMontador,
                DataInicio = instalacao.DataInicio,
                DataTermino = instalacao.DataTermino
            };
        }

        [HttpPost]
        public async Task<ActionResult<InstalacaoDTO>> CreateInstalacao(
            InstalacaoDTO instalacaoDto)
        {
            var obraExiste = await _context.Obra
                .AnyAsync(o => o.Id == instalacaoDto.IdObra);

            if (!obraExiste)
            {
                return BadRequest("Obra não encontrada.");
            }

            var montadorExiste = await _context.Usuario
                .AnyAsync(u => u.Id == instalacaoDto.IdMontador);

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

            instalacaoDto.Id = instalacao.Id;

            return CreatedAtAction(
                nameof(GetInstalacao),
                new { id = instalacao.Id },
                instalacaoDto
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstalacao(
            int id, InstalacaoDTO instalacaoDto)
        {
            var instalacao = await _context.Instalacao.FindAsync(id);

            if (instalacao == null)
            {
                return NotFound();
            }

            var obraExiste = await _context.Obra
                .AnyAsync(o => o.Id == instalacaoDto.IdObra);

            if (!obraExiste)
            {
                return BadRequest("Obra não encontrada.");
            }

            var montadorExiste = await _context.Usuario
                .AnyAsync(u => u.Id == instalacaoDto.IdMontador);

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
        public async Task<IActionResult> DeleteInstalacao(int id)
        {
            var instalacao = await _context.Instalacao.FindAsync(id);

            if (instalacao == null)
            {
                return NotFound();
            }

            _context.Instalacao.Remove(instalacao);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}