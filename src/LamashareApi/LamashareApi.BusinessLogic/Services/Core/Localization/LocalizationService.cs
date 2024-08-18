using LamashareApi.Shared.Localization;
using Microsoft.Extensions.Localization;

namespace Lamashare.BusinessLogic.Services.Core.Localization;

public class LocalizationService(IStringLocalizer<SharedResources> localizer) : ILocalizationService
{
    public string GetLocKey(string localizationKey)
    {
        return localizer[localizationKey] ?? $"LOCKEY#{localizationKey}";
    }
    
    public string GetLocKey(string localizationKey, Dictionary<string, string> interpolation)
    {
        string sourceStr = GetLocKey(localizationKey);

        foreach (var key in interpolation.Keys)
        {
            sourceStr = sourceStr.Replace(key, interpolation.GetValueOrDefault(key));
        }
        
        return sourceStr;
    }
}