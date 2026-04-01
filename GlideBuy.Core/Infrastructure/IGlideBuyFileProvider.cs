using Microsoft.Extensions.FileProviders;
using System.Text;

namespace GlideBuy.Core.Infrastructure
{
    public interface IGlideBuyFileProvider
    {
        public string WebRootPath { get; }

        string Combine(params string[] paths);

        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void DeleteDirectory(string path);
        void DirectoryMove(string sourceDirName, string destDirName);
        void CreateFile(string path);
        bool FileExists(string path);
        void DeleteFile(string path);
        void FileMove(string sourceFileName, string destFileName);

        void FileCopy(string sourceFileName, string destFileName, bool overwrite = false);

        long FileLength(string path);

        string GetAbsolutePath(params string[] paths);

        DateTime GetCreationTime(string path);

        string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true);

        string? GetDirectoryName(string path);

        string GetDirectoryNameOnly(string path);

        string GetFileExtension(string filePath);

        string GetFileName(string path);

        string GetFileNameWithoutExtension(string filePath);

        IEnumerable<string> EnumerateFiles(string dirPath, string searchPattern, bool topDirOnly = true);

        string[] GetFiles(string dirPath, string searchPattern = "", bool topDirOnly = true);

        FileStream GetOrCreateFile(string path);

        bool IsDirectory(string path);

        string GetVirtualPath(string? path);

        string MapPath(string path);

        IFileInfo GetFileInfo(string subpath);

        bool IsPathRooted(string path);

        Task<byte[]> ReadAllBytesAsync(string filePath);

        Task<string> ReadAllTextAsync(string path, Encoding encoding);

        string ReadAllText(string path, Encoding encoding);

        Task WriteAllBytesAsync(string filePath, byte[] bytes);

        Task WriteAllTextAsync(string filePath, string contents, Encoding encoding);

        void WriteAllText(string filePath, string contents, Encoding encoding);
    }
}
