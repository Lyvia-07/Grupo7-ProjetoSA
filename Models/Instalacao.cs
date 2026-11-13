using System.Text.Json.Serialization;

namespace S.A_Esquadrias.Models
{
    public class Instalacao
    {
        public int Id { get; set; }
        public int IdObra { get; set; }
        public int IdMontador { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }

        [JsonIgnore]
        public Obra Obra { get; set; }

        [JsonIgnore]
        public Usuario Montador { get; set; }

        public ICollection<RegistroInstalacao> Registros { get; set; }
    }
}
