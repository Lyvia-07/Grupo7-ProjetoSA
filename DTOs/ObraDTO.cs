namespace S.A_Esquadrias.DTOs
{
    public class ObraDTO
    {
        public int IdCliente { get; set; }
        public int IdVendedor { get; set; }
        public string NomeObra { get; set; }
        public string StatusObra { get; set; }
        public string Endereco { get; set; }
        public string? ObsGerais { get; set; }
    }
}
