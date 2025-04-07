
namespace TaskManagementSystem.Domain.Exception
{
    public class CustomException  
        //: Exception
    {
        public int StatusCode { get; }

        public CustomException(string message, int statusCode = 400)
           // : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }

    public class ValidationException : CustomException
    {
        public ValidationException(string message) : base(message, 400) { }
    }
}
