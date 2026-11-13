
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.Models;
using S.A_Esquadrias.DTOs;
using System.Security.Claims;

namespace S.A_Esquadrias.Controllers
{
    [Authorize(Roles = "ADM,Vendedor")]
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
            var medicoes = _context.Medicao
                .Include(m => m.Obra)
                .Include(m => m.Vendedor)
                .AsQueryable();

            if (User.IsInRole("Vendedor"))
            {
                var idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                medicoes = medicoes.Where(m => m.IdVendedor == idVendedor);
            }

            return Ok(await medicoes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicao>> GetMedicao(int id)
        {
            var medicao = _context.Medicao
                .Include(m => m.Obra)
                .Include(m => m.Vendedor)
                .AsQueryable();

            if (User.IsInRole("Vendedor"))
            {
                var idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                medicao = medicao.Where(m => m.IdVendedor == idVendedor);
            }

            var resultado = await medicao.FirstOrDefaultAsync(m => m.Id == id);

            if (resultado == null)
            {
                return NotFound("Medição não encontrada.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<Medicao>> PostMedicao(MedicaoDTO medicaoDTO)
        {
            var idVendedor = medicaoDTO.IdVendedor;

            if (User.IsInRole("Vendedor"))
            {
                idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );
            }

            var medicao = new Medicao
            {
                IdObra = medicaoDTO.IdObra,
                IdVendedor = idVendedor,
                DataMedicao = medicaoDTO.DataMedicao,
                Tag = medicaoDTO.Tag,
                TipoEsquadria = medicaoDTO.TipoEsquadria,
                Localizacao = medicaoDTO.Localizacao,
                LarguraNominal = medicaoDTO.LarguraNominal,
                AlturaNominal = medicaoDTO.AlturaNominal,
                ObsTecnicas = medicaoDTO.ObsTecnicas
            };

            _context.Medicao.Add(medicao);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMedicao),
                new { id = medicao.Id },
                medicao
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicao(int id, MedicaoDTO medicaoDTO)
        {
            var medicaoExistente = await _context.Medicao.FindAsync(id);

            if (medicaoExistente == null)
            {
                return NotFound("Medição não encontrada.");
            }

            if (User.IsInRole("Vendedor"))
            {
                var idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                if (medicaoExistente.IdVendedor != idVendedor)
                {
                    return Forbid();
                }
            }

            medicaoExistente.IdObra = medicaoDTO.IdObra;
            medicaoExistente.DataMedicao = medicaoDTO.DataMedicao;
            medicaoExistente.Tag = medicaoDTO.Tag;
            medicaoExistente.TipoEsquadria = medicaoDTO.TipoEsquadria;
            medicaoExistente.Localizacao = medicaoDTO.Localizacao;
            medicaoExistente.LarguraNominal = medicaoDTO.LarguraNominal;
            medicaoExistente.AlturaNominal = medicaoDTO.AlturaNominal;
            medicaoExistente.ObsTecnicas = medicaoDTO.ObsTecnicas;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteMedicao(int id)
        {
            var medicao = await _context.Medicao.FindAsync(id);

            if (medicao == null)
            {
                return NotFound("Medição não encontrada.");
            }

            _context.Medicao.Remove(medicao);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
