using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Obras)
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Obras)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCliente),
                new { id = cliente.Id },
                cliente
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("ID inválido.");
            }

            var clienteExistente = await _context.Clientes
                .FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefone = cliente.Telefone;
            clienteExistente.Cep = cliente.Cep;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes
                .FindAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
