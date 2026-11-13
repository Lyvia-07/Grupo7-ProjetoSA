
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.Models;
using S.A_Esquadrias.DTOs;

namespace S.A_Esquadrias.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Cliente
                .Include(c => c.Obras)
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Cliente
                .Include(c => c.Obras)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            return Ok(cliente);
        }

        [HttpPost]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<ActionResult<Cliente>> PostCliente(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente
            {
                Nome = clienteDTO.Nome,
                Email = clienteDTO.Email,
                Telefone = clienteDTO.Telefone,
                Cep = clienteDTO.Cep
            };

            _context.Cliente.Add(cliente);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCliente),
                new { id = cliente.Id },
                cliente
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<IActionResult> PutCliente(int id, ClienteDTO clienteDTO)
        {
            var clienteExistente = await _context.Cliente.FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            clienteExistente.Nome = clienteDTO.Nome;
            clienteExistente.Email = clienteDTO.Email;
            clienteExistente.Telefone = clienteDTO.Telefone;
            clienteExistente.Cep = clienteDTO.Cep;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            _context.Cliente.Remove(cliente);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
