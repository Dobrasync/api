namespace Lamashare.BusinessLogic.Dtos.Auth;

public class TokenCreateDto
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}