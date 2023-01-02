using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalApiClients.Rest;

public interface IJwtProviderApi
{
    [Post("/users")]
    public TokenResponse RegisterUser([Body] RegisterUserRequest request);
}

public class RegisterUserRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Patronymic { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}

public class TokenResponse
{
    public string AccessToken { get; set; }

    public Guid RefreshToken { get; set; }
}
