
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.DTOs;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Controllers
{
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
        public async Task<ActionResult<IEnumerable<FotoInstalacaoDTO>>> GetFotos()
        {
            return await _context.FotoInstalacao
                .Select(f => new FotoInstalacaoDTO
                {
                    Id = f.Id,
                    IdRegistro = f.IdRegistro,
                    CaminhoArquivo = f.CaminhoArquivo,
                    DataEnvio = f.DataEnvio
                })
                .ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<FotoInstalacaoDTO>> GetFoto(int id)
        {
            var foto = await _context.FotoInstalacao.FindAsync(id);

            if (foto == null)
            {
                return NotFound();
            }

            return new FotoInstalacaoDTO
            {
                Id = foto.Id,
                IdRegistro = foto.IdRegistro,
                CaminhoArquivo = foto.CaminhoArquivo,
                DataEnvio = foto.DataEnvio
            };
        }

        [HttpPost]
        public async Task<ActionResult<FotoInstalacaoDTO>> CreateFoto(
            FotoInstalacaoDTO fotoDto)
        {
            var registroExiste = await _context.RegistroInstalacao
                .AnyAsync(r => r.Id == fotoDto.IdRegistro);

            if (!registroExiste)
            {
                return BadRequest("Esse Registro de instalação não foi encontrado.");
            }

            var foto = new FotoInstalacao
            {
                IdRegistro = fotoDto.IdRegistro,
                CaminhoArquivo = fotoDto.CaminhoArquivo,
                DataEnvio = DateTime.Now
            };

            _context.FotoInstalacao.Add(foto);

            await _context.SaveChangesAsync();

            fotoDto.Id = foto.Id;
            fotoDto.DataEnvio = foto.DataEnvio;

            return CreatedAtAction(
                nameof(GetFoto),
                new { id = foto.Id },
                fotoDto
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoto(int id)
        {
            var foto = await _context.FotoInstalacao.FindAsync(id);

            if (foto == null)
            {
                return NotFound();
            }

            _context.FotoInstalacao.Remove(foto);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
