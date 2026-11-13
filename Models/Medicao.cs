using System.Text.Json.Serialization;

namespace S.A_Esquadrias.Models
{
    public class Medicao
    {
        public int Id { get; set; }
        public int IdObra { get; set; }
        public int IdVendedor { get; set; }
        public DateTime DataMedicao { get; set; }

        public string Tag { get; set; }
        public string TipoEsquadria { get; set; }
        public string Localizacao { get; set; }
        public decimal LarguraNominal { get; set; }
        public decimal AlturaNominal { get; set; }
        public string? ObsTecnicas { get; set; }

        [JsonIgnore]
        public Obra Obra { get; set; }

        [JsonIgnore]
        public Usuario Vendedor { get; set; }
    }
}
