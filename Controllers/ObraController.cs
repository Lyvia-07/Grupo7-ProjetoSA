
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using S.A_Esquadrias.Data;
using S.A_Esquadrias.Models;
using S.A_Esquadrias.DTOs;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

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
                ObsGerais = obraDTO.ObsGerais,
                ValorObra = obraDTO.ValorObra
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
            obraExistente.ValorObra = obraDTO.ValorObra;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        [Authorize(Roles = "ADM,Vendedor")]
        public async Task<IActionResult> GerarPdf(int id)
        {
            var obra = await _context.Obra
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.Medicoes)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada.");
            }

            if (User.IsInRole("Vendedor"))
            {
                var idVendedor = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                if (obra.IdVendedor != idVendedor)
                {
                    return Forbid();
                }
            }

            var cultura = new CultureInfo("pt-BR");

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .Text("S.A. ESQUADRIAS")
                                .FontSize(22)
                                .Bold()
                                .FontColor(Colors.Blue.Darken3);

                            column.Item()
                                .Text("RELATÓRIO DA OBRA")
                                .FontSize(14)
                                .Bold();

                            column.Item()
                                .Text($"Data de emissão: {DateTime.Now:dd/MM/yyyy HH:mm}");

                            column.Item().PaddingVertical(8)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten1);
                        });

                    page.Content().Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Text("DADOS DA OBRA").Bold().FontSize(13);

                        column.Item().Text($"Obra: {obra.NomeObra}");
                        column.Item().Text($"Endereço: {obra.Endereco}");
                        column.Item().Text($"Status: {obra.StatusObra}");
                        column.Item().Text($"Vendedor responsável: {obra.Vendedor?.Email ?? "Não informado"}");
                        column.Item().Text($"Valor total: {obra.ValorObra.ToString("C2", cultura)}");

                        column.Item().PaddingTop(8)
                            .Text("DADOS DO CLIENTE")
                            .Bold()
                            .FontSize(13);

                        column.Item().Text($"Nome: {obra.Cliente?.Nome ?? "Não informado"}");
                        column.Item().Text($"E-mail: {obra.Cliente?.Email ?? "Não informado"}");
                        column.Item().Text($"Telefone: {obra.Cliente?.Telefone ?? "Não informado"}");

                        var cliente = obra.Cliente;

                        var enderecoCliente = cliente == null
                            ? "Não informado"
                            : $"{cliente.Logradouro}, {cliente.Numero} - {cliente.Bairro}, {cliente.Cidade}/{cliente.Estado} - CEP: {cliente.Cep}";

                        column.Item().Text($"Endereço do cliente: {enderecoCliente}");

                        if (!string.IsNullOrWhiteSpace(cliente?.Complemento))
                        {
                            column.Item().Text($"Complemento: {cliente.Complemento}");
                        }

                        if (!string.IsNullOrWhiteSpace(obra.ObsGerais))
                        {
                            column.Item().PaddingTop(8)
                                .Text("OBSERVAÇÕES DA OBRA")
                                .Bold()
                                .FontSize(13);

                            column.Item().Text(obra.ObsGerais);
                        }

                        column.Item().PaddingTop(8)
                            .Text("MEDIÇÕES DAS PEÇAS")
                            .Bold()
                            .FontSize(13);

                        if (obra.Medicoes != null && obra.Medicoes.Any())
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tag").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tipo").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Local").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Largura").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Altura").Bold();
                                });

                                foreach (var medicao in obra.Medicoes)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(medicao.Tag ?? "");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(medicao.TipoEsquadria ?? "");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(medicao.Localizacao ?? "");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{medicao.LarguraNominal} mm");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{medicao.AlturaNominal} mm");
                                }
                            });

                            foreach (var medicao in obra.Medicoes)
                            {
                                if (!string.IsNullOrWhiteSpace(medicao.ObsTecnicas))
                                {
                                    column.Item().Text($"{medicao.Tag} - Observações: {medicao.ObsTecnicas}");
                                }
                            }
                        }
                        else
                        {
                            column.Item().Text("Nenhuma medição cadastrada.");
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("S.A. Esquadrias - Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Obra-{obra.Id}.pdf"
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> DeleteObra(int id)
        {
            var obra = await _context.Obra
                .Include(o => o.Medicoes)
                .Include(o => o.Instalacoes)
                    .ThenInclude(i => i.Registros)
                        .ThenInclude(r => r.Fotos)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada.");
            }

            var fotos = obra.Instalacoes
                .SelectMany(i => i.Registros)
                .SelectMany(r => r.Fotos)
                .ToList();

            var registros = obra.Instalacoes
                .SelectMany(i => i.Registros)
                .ToList();

            _context.RemoveRange(fotos);
            _context.RemoveRange(registros);
            _context.RemoveRange(obra.Instalacoes);
            _context.RemoveRange(obra.Medicoes);
            _context.Obra.Remove(obra);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
