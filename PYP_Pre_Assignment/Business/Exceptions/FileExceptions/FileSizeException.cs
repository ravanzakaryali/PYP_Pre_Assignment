namespace Business.Exceptions.FileExceptions
{
    public class FileSizeException : FileException
    {
        public FileSizeException() : base("The file size is incorrect") { }
        public FileSizeException(string message) : base(message) { }
        public FileSizeException(string message, Exception innerException) : base(message, innerException) { }

    }
}
