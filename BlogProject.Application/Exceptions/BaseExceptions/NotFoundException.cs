namespace BlogProject.Application.Exceptions.BaseExceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }
}
