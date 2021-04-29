namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Setings
{
    public class SqlSettings
    {
        public string ConnectionString { get; set; }
        public bool InMemory { get; set; }
        public int? CommandTimeout { get; set; }
    }
}
