using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.Shared.Appsettings.Auth.Idp;

public class IdpAS
{
    /// <summary>
    ///     Global authority address.
    /// </summary>
    [Required]
    public string Authority { get; set; } = default!;

    /// <summary>
    ///     IDP credentials for the API itself. Used when making auth introspection requests.
    /// </summary>
    [Required]
    public ApiAS Api { get; set; } = default!;

    /// <summary>
    ///     Public credentials that client devices can use to generate
    ///     codes and tokens.
    /// </summary>
    [Required]
    public DeviceAS Device { get; set; } = default!;
}