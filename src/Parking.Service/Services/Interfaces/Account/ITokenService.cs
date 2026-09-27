using Parking.Service.DTOs.Account;

namespace Parking.Service.Services.Interfaces.Account;

public interface ITokenService
{
    UserTokenDTO GenerateToken(string email);
}
