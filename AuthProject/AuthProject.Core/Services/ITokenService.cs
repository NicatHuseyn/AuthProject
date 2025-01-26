using AuthProject.Core.Configurations;
using AuthProject.Core.DTOs;
using AuthProject.Core.Entities;

namespace AuthProject.Core.Services;

public interface ITokenService
{
    TokenDto CreateToken(AppUser appUser);
    ClientTokenDto CreateTokenByClient(Client client);
}
