namespace JTea.OfferScrappers.WindowsService.ErrorHandling
{
    public class RequestError
    {
        public string ExceptionType { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}