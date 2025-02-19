using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Validation;

/// <summary>
///     Basically your generic bad request response.
/// </summary>
public class BadInputUSException()
    : UserspaceException(400, LocKeys.ExceptionBadInput.KeyTemplate, "The provided input is invalid.");