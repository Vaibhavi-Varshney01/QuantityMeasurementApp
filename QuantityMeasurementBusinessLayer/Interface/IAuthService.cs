using QuantityMeasurementModel.DTO;

namespace QuantityMeasurementBusinessLayer.Interface;

/// <summary>
/// Handles user registration and authentication responses.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Register a new user and return the authentication response.
    /// </summary>
    AuthResponseDTO Register(RegisterDTO dto);

    /// <summary>
    /// Authenticate an existing user and produce a token response.
    /// </summary>
    AuthResponseDTO Login(LoginDTO dto);
}
 
