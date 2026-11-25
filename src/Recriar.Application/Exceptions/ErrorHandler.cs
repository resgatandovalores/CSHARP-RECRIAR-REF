namespace Recriar.Application.Exceptions
{
    public class ErrorHandler
    {

        public string GetMessage(Exception exception)
        {

            if (exception is ArgumentException)
            {
                return exception.Message;
            }

            return new ErrorMessages().getUnexpected();
        }
    }
}
