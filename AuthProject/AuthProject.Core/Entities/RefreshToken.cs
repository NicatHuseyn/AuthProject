namespace AuthProject.Core.Entities;

public class RefreshToken
{
    public string AppUserId { get; set; }
    public string UserRefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
