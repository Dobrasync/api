namespace Dobrasync.Api.BusinessLogic.Dtos.Auth;

public class AuthDto
{
    public string AuthToken { get; set; } = default!;
    public DateTime ExpiresUtc { get; set; }
}