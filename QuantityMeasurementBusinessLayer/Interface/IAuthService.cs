using QuantityMeasurementBusinessLayer.DTO;

namespace QuantityMeasurementBusinessLayer.Interface;

public interface IAuthService
{
    AuthResponseDTO Register(RegisterDTO dto);
    AuthResponseDTO Login(LoginDTO dto);
}
 