namespace S.A_Esquadrias.DTOs
{
    public class FotoInstalacaoDTO
    {
        public int Id { get; set; }
        public int IdRegistro { get; set; }
        public string CaminhoArquivo { get; set; }
        public DateTime DataEnvio { get; set; }
    }
}
