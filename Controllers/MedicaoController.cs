

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicao>>> GetMedicoes()
        {
            var medicoes = await _context.Medicoes
                .Include(m => m.Obra)
                .Include(m => m.Vendedor)
                .ToListAsync();

            return Ok(medicoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicao>> GetMedicao(int id)
        {
            var medicao = await _context.Medicoes
                .Include(m => m.Obra)
                .Include(m => m.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicao == null)
            {
                return NotFound("Medição não encontrada.");
            }

            return Ok(medicao);
        }

        [HttpPost]
        public async Task<ActionResult<Medicao>> PostMedicao(Medicao medicao)
        {
            _context.Medicoes.Add(medicao);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMedicao),
                new { id = medicao.Id },
                medicao
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicao(int id, Medicao medicao)
        {
            if (id != medicao.Id)
            {
                return BadRequest("ID inválido.");
            }

            var medicaoExistente = await _context.Medicoes
                .FindAsync(id);

            if (medicaoExistente == null)
            {
                return NotFound("Medição não encontrada.");
            }

            medicaoExistente.IdObra = medicao.IdObra;
            medicaoExistente.IdVendedor = medicao.IdVendedor;
            medicaoExistente.DataMedicao = medicao.DataMedicao;
            medicaoExistente.Tag = medicao.Tag;
            medicaoExistente.TipoEsquadria = medicao.TipoEsquadria;
            medicaoExistente.Localizacao = medicao.Localizacao;
            medicaoExistente.LarguraNominal = medicao.LarguraNominal;
            medicaoExistente.AlturaNominal = medicao.AlturaNominal;
            medicaoExistente.ObsTecnicas = medicao.ObsTecnicas;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicao(int id)
        {
            var medicao = await _context.Medicoes
                .FindAsync(id);

            if (medicao == null)
            {
                return NotFound("Medição não encontrada.");
            }

            _context.Medicoes.Remove(medicao);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
