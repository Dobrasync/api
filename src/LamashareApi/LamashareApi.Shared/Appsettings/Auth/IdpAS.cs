using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class IdpAS
{
    [Required] 
    public WebAS Web { get; set; }
    
    [Required]
    public DeviceAS Device { get; set; }
    
    [Required]
    public string Authority { get; set; } = default!;
    
    public string ClientId { get; set; } = default!;

    public string AppId { get; set; } = default!;

    public string KeyId { get; set; } = default!;

    public string Key { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;
}