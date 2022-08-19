namespace Business.Exceptions.FileExceptions
{
    public class FileException : Exception
    {
        public FileException() : base("Something is wrong with the file") { }
        public FileException(string message) : base(message) { }
        public FileException(string message, Exception exception) : base(message, exception) { }
    }
}
