namespace LamashareApi.Shared.Exceptions.ApiExceptions;

public class UserspaceException : Exception
{
    #region ctor

    public int HttpStatusCode { get; set; }
    public string UserMessage { get; set; }
    public string TechnicalDescription { get; set; }

    public UserspaceException(int httpHttpStatusCode, string userMsg, string technicalDescription)
    {
        HttpStatusCode = httpHttpStatusCode;
        UserMessage = userMsg;
        TechnicalDescription = technicalDescription;
    }

    #endregion
}