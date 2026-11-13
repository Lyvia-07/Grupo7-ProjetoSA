namespace S.A_Esquadrias.DTOs
{
    public class InstalacaoDTO
    {
        public int Id { get; set; }
        public int IdObra { get; set; }
        public int IdMontador { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}
