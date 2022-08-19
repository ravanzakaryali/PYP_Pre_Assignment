namespace Business.Exceptions.FileExceptions
{
    public class FileTypeException : FileException
    {
        public FileTypeException() : base("The file type is incorrect") { }
        public FileTypeException(string message) : base(message) { }
        public FileTypeException(string message, Exception innerException) : base(message, innerException) { }

    }
}
