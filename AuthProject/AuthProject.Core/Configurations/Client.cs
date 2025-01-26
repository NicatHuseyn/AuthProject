namespace AuthProject.Core.Configurations;

public class Client
{
    public string Id { get; set; }
    public string Secret { get; set; }

    // hansi API-leri elde ede biler
    public List<string> Audiences { get; set; }
}
