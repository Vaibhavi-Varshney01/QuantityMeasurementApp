using SharedModels.DTO;

namespace AuthService.Interface;

public interface IAuthService
{
    AuthResponseDTO Register(RegisterDTO dto);
    AuthResponseDTO Login(LoginDTO dto);
}
