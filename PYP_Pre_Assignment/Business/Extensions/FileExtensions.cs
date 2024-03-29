﻿using Business.Common;
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
        /// <summary>
        /// Checks the file size
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="fileSize">Size of file being checked</param>
        /// <returns>Returns <see langword="true"/> if the supplied size match, <see langword="fase"/> otherwise</returns>
        public static bool CheckFileSize(this IFormFile file, int kb = 5120)
        {
            if ((file.Length / 1024) >= kb)
            {
                return true;
            }
            return false;
        }
        public static async Task<string> FileUploadAsync(this IFormFile file, string root,params string[] folders)
        {
            string fileNewName = Path.Combine(Helper.GenerateUniqueDateName().CharacterRegulatory() + file.FileName);
            string rootInFolder = folders.Aggregate((result, folder) => Path.Combine(result, folder));
            string path = Path.Combine(root, rootInFolder, fileNewName);
            using (FileStream fs = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return fileNewName;
        }
    }
}
