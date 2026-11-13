namespace S.A_Esquadrias.Models
{
    public class FotoInstalacao
    {
        public int Id { get; set; }
        public int IdRegistro { get; set; }
        public string CaminhoArquivo { get; set; }
        public DateTime DataEnvio { get; set; } 

        public RegistroInstalacao RegistroInstalacao { get; set; }
    }
}
