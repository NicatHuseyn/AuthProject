namespace AuthProject.Core.DTOs;

// This class for unauthorization apps
public class ClientTokenDto
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
}
