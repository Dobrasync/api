using Dobrasync.Api.Shared.Localization;
using Microsoft.Extensions.Localization;

namespace Dobrasync.Api.BusinessLogic.Services.Core.Localization;

public class LocalizationService(IStringLocalizer<SharedResources> localizer) : ILocalizationService
{
    public string GetLocKey(string localizationKey)
    {
        return localizer[localizationKey] ?? $"LOCKEY#{localizationKey}";
    }

    public string GetLocKey(string localizationKey, Dictionary<string, string> interpolation)
    {
        var sourceStr = GetLocKey(localizationKey);

        foreach (var key in interpolation.Keys)
            sourceStr = sourceStr.Replace(key, interpolation.GetValueOrDefault(key));

        return sourceStr;
    }
}