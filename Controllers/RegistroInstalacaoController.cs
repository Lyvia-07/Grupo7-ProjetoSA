
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Controllers
{
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
        public async Task<ActionResult<IEnumerable<RegistroInstalacao>>> GetRegistros()
        {
            var registros = await _context.RegistrosInstalacao
                .Include(r => r.Instalacao)
                .Include(r => r.Fotos)
                .ToListAsync();

            return Ok(registros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroInstalacao>> GetRegistro(int id)
        {
            var registro = await _context.RegistrosInstalacao
                .Include(r => r.Instalacao)
                .Include(r => r.Fotos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro == null)
            {
                return NotFound("Registro de instalação não encontrado.");
            }

            return Ok(registro);
        }

        [HttpPost]
        public async Task<ActionResult<RegistroInstalacao>> PostRegistro(RegistroInstalacao registro)
        {
            _context.RegistrosInstalacao.Add(registro);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRegistro),
                new { id = registro.Id },
                registro
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistro(int id, RegistroInstalacao registro)
        {
            if (id != registro.Id)
            {
                return BadRequest("ID inválido.");
            }

            var registroExistente = await _context.RegistrosInstalacao
                .FindAsync(id);

            if (registroExistente == null)
            {
                return NotFound("Registro de instalação não encontrado.");
            }

            registroExistente.IdInstalacao = registro.IdInstalacao;
            registroExistente.TipoProblema = registro.TipoProblema;
            registroExistente.ObservacoesGerais = registro.ObservacoesGerais;
            registroExistente.DescricaoProblema = registro.DescricaoProblema;
            registroExistente.DataRegistro = registro.DataRegistro;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            var registro = await _context.RegistrosInstalacao
                .FindAsync(id);

            if (registro == null)
            {
                return NotFound("Registro de instalação não encontrado.");
            }

            _context.RegistrosInstalacao.Remove(registro);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
