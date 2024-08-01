namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public class InvalidCronExpressionException : DefinedException
    {
        public InvalidCronExpressionException(string cronExpression)
            : base($"{cronExpression} is not valid Quartz cron expression")
        {
        }
    }
}