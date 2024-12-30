namespace Dobrasync.Api.BusinessLogic.Services.Core.Localization;

public interface ILocalizationService
{
    public string GetLocKey(string localizationKey);
    public string GetLocKey(string localizationKey, Dictionary<string, string> interpolation);
}