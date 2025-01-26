using Microsoft.AspNetCore.Identity;

namespace AuthProject.Core.Entities;

public class AppUser:IdentityUser
{
    public string FullName { get; set; }
}
