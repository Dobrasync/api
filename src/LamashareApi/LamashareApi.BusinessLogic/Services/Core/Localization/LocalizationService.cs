using LamashareApi.Shared.Localization;
using Microsoft.Extensions.Localization;

namespace Lamashare.BusinessLogic.Services.Core.Localization;

public class LocalizationService(IStringLocalizer<SharedResources> localizer) : ILocalizationService
{
    public string GetLocKey(string localizationKey)
    {
        return localizer[localizationKey] ?? $"LOCKEY#{localizationKey}";
    }
}