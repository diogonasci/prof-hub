namespace Prof.Hub.Application.Results
{
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(string errorMessage) => ErrorMessage = errorMessage;

        public ValidationError(string identifier, string errorMessage, string errorCode)
        {
            Identifier = identifier;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public string Identifier { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }
}
