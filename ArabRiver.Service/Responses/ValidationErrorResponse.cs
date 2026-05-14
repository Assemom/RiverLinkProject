namespace ArabRiver.Service.Responses
{
    public class ValidationErrorResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> Errors
        {
            get; set;
        }

        public ValidationErrorResponse(
            IEnumerable<string> errors)
        {
            Success = false;

            Message = "Validation failed";

            Errors = errors;
        }
    }
}
