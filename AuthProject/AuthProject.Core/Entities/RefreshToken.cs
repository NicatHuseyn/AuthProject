namespace AuthProject.Core.Entities;

public class RefreshToken:BaseEntity
{
    public string AppUserId { get; set; }
    public string UserRefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
