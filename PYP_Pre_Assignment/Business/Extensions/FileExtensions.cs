using Microsoft.AspNetCore.Http;

namespace Business.Extensions
{
    public static class FileExtensions
    {
        /// <summary>
        /// Checks the file type
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="fileTypes">Types of file being checked</param>
        /// <returns>Returns <see langword="true"/> if the supplied types match, <see langword="fase"/> otherwise</returns>
        public static bool CheckFileType(this IFormFile file, params string[] fileTypes)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            return fileTypes.Any(fileType => extension.Contains(fileType.ToLower()));
        }
    }
}
