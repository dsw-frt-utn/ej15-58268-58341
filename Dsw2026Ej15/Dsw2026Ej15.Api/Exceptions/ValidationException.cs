namespace Dsw2026Ej15.Api.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        { }

        public ValidationException(string detalle) : base(detalle)
        { }
    }
}