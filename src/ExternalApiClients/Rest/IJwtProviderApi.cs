using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalApiClients.Rest;

public interface IJwtProviderApi
{
    [Post("/users")]
    public Task<ApiResponse<TokenResponse>> RegisterUser([Body] RegisterUserRequest request, CancellationToken cancellationToken);
}

public class RegisterUserRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Patronymic { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class TokenResponse
{
    public string AccessToken { get; set; }

    public Guid RefreshToken { get; set; }
}

public class JwtError
{
    public string ErrorCode { get; set; }

    public string ErrorMessage { get; set; }
}
