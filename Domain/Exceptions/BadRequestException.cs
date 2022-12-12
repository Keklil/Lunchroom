namespace Domain.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string massage) 
            : base(massage) { }
    }
}
