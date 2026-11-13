
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
    public class ObraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ObraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "ADM,Vendedor,Montador")]
        public async Task<ActionResult<IEnumerable<Obra>>> GetObras()
        {
            var obras = _context.Obra
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.Medicoes)
                .Include(o => o.Instalacoes)
                .AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                obras = obras.Where(o =>
                    o.Instalacoes.Any(i => i.IdMontador == idMontador)
                );
            }

            return Ok(await obras.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADM,Vendedor,Montador")]
        public async Task<ActionResult<Obra>> GetObra(int id)
        {
            var obra = _context.Obra
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.Medicoes)
                .Include(o => o.Instalacoes)
                .AsQueryable();

            if (User.IsInRole("Montador"))
            {
                var idMontador = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                obra = obra.Where(o =>
                    o.Instalacoes.Any(i => i.IdMontador == idMontador)
                );
            }

            var resultado = await obra.FirstOrDefaultAsync(o => o.Id == id);

            if (resultado == null)
            {
                return NotFound("Obra não encontrada.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<ActionResult<Obra>> PostObra(ObraDTO obraDTO)
        {
            var clienteExiste = await _context.Cliente
                .AnyAsync(c => c.Id == obraDTO.IdCliente);

            if (!clienteExiste)
            {
                return BadRequest("Cliente não encontrado.");
            }

            int idVendedor = obraDTO.IdVendedor;

            if (User.IsInRole("Vendedor"))
            {
                idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );
            }

            var vendedorExiste = await _context.Usuario
                .AnyAsync(u =>
                    u.Id == idVendedor &&
                    u.Perfil.Trim().ToUpper() == "VENDEDOR"
                );

            if (!vendedorExiste)
            {
                return BadRequest("Vendedor não encontrado ou perfil inválido.");
            }

            var obra = new Obra
            {
                IdCliente = obraDTO.IdCliente,
                IdVendedor = idVendedor,
                NomeObra = obraDTO.NomeObra,
                StatusObra = obraDTO.StatusObra,
                Endereco = obraDTO.Endereco,
                ObsGerais = obraDTO.ObsGerais
            };

            _context.Obra.Add(obra);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetObra),
                new { id = obra.Id },
                obra
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<IActionResult> PutObra(int id, ObraDTO obraDTO)
        {
            var obraExistente = await _context.Obra.FindAsync(id);

            if (obraExistente == null)
            {
                return NotFound("Obra não encontrada.");
            }

            if (User.IsInRole("Vendedor"))
            {
                var idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                if (obraExistente.IdVendedor != idVendedor)
                {
                    return Forbid();
                }
            }

            var clienteExiste = await _context.Cliente
                .AnyAsync(c => c.Id == obraDTO.IdCliente);

            if (!clienteExiste)
            {
                return BadRequest("Cliente não encontrado.");
            }

            obraExistente.IdCliente = obraDTO.IdCliente;
            obraExistente.NomeObra = obraDTO.NomeObra;
            obraExistente.StatusObra = obraDTO.StatusObra;
            obraExistente.Endereco = obraDTO.Endereco;
            obraExistente.ObsGerais = obraDTO.ObsGerais;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteObra(int id)
        {
            var obra = await _context.Obra.FindAsync(id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada.");
            }

            _context.Obra.Remove(obra);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
