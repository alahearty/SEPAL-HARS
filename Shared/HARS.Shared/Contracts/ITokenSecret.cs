namespace HARS.Shared.Contracts
{
    public interface ITokenSecret
    {
        string CypherCrescentSecretKey { get; set; }
        string Audience { get; set; }
        string Issuer { get; set; }
    }
}
