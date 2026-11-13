using System.Text.Json.Serialization;

namespace S.A_Esquadrias.Models
{
    public class RegistroInstalacao
    {
        public int Id { get; set; }
        public int IdInstalacao { get; set; }
        public string TipoProblema { get; set; }
        public string? ObservacoesGerais { get; set; }
        public string? DescricaoProblema { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        [JsonIgnore]
        public Instalacao Instalacao { get; set; }

        public ICollection<FotoInstalacao> Fotos { get; set; }
    }
}
