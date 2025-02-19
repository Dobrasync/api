namespace Dobrasync.Api.BusinessLogic.Dtos.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public Guid[] LibraryIds { get; set; } = default!;
}