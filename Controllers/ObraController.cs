
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ObraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obra>>> GetObras()
        {
            var obras = await _context.Obras
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.Medicoes)
                .Include(o => o.Instalacoes)
                .ToListAsync();

            return Ok(obras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Obra>> GetObra(int id)
        {
            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.Medicoes)
                .Include(o => o.Instalacoes)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada.");
            }

            return Ok(obra);
        }

        [HttpPost]
        public async Task<ActionResult<Obra>> PostObra(Obra obra)
        {
            _context.Obras.Add(obra);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetObra),
                new { id = obra.Id },
                obra
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutObra(int id, Obra obra)
        {
            if (id != obra.Id)
            {
                return BadRequest("ID inválido.");
            }

            var obraExistente = await _context.Obras
                .FindAsync(id);

            if (obraExistente == null)
            {
                return NotFound("Obra não encontrada.");
            }

            obraExistente.IdCliente = obra.IdCliente;
            obraExistente.IdVendedor = obra.IdVendedor;
            obraExistente.NomeObra = obra.NomeObra;
            obraExistente.StatusObra = obra.StatusObra;
            obraExistente.Endereco = obra.Endereco;
            obraExistente.ObsGerais = obra.ObsGerais;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObra(int id)
        {
            var obra = await _context.Obras
                .FindAsync(id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada.");
            }

            _context.Obras.Remove(obra);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
