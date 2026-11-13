using System.Text.Json.Serialization;

namespace S.A_Esquadrias.Models
{
    public class Obra
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdVendedor { get; set; }
        public string NomeObra { get; set; }
        public string StatusObra { get; set; }
        public string Endereco { get; set; }
        public string? ObsGerais { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }

        [JsonIgnore]
        public Usuario Vendedor { get; set; }

        public ICollection<Medicao> Medicoes { get; set; }
        public ICollection<Instalacao> Instalacoes { get; set; }
    }
}
