using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Text;

namespace GlideBuy.Core.Infrastructure
{
    /**
     * By inheriting from PhysicalFileProvider, the class integrates cleanly
     * with ASP.NET Core’s file abstraction ecosystem, meaning it can be used
     * both for runtime IO and for static file serving without duplicating logic.
     * By implementing its own interface, Nop keeps the rest of the application
     * insulated from concrete filesystem semantics, which is crucial if you ever
     * want to swap or partially replace this implementation, for example with a
     * cloud-backed or virtualized storage layer.
     * 
     * ThumbService never worries about path separators, root directories, UNC paths,
     * or platform quirks. It simply asks for an absolute path, checks for existence,
     * and writes bytes. All of the messy, error-prone details live in NopFileProvider,
     * exactly where they belong.
     * 
     * The core utility methods exist to smooth over platform differences and edge cases
     * that the raw System.IO APIs do not handle gracefully. The Combine method is a good example.
     * 
     * When you step back and look at these methods collectively, a pattern emerges. None of them introduces complicated logic on its own. Their purpose is to establish a single, stable gateway to the filesystem. Every directory creation, file read, write, move, or deletion flows through this provider. That centralization makes the rest of the NopCommerce codebase easier to maintain because filesystem behavior is defined in one place instead of being scattered across dozens of services.
     */
    public class GlideBuyFileProvider : PhysicalFileProvider, IGlideBuyFileProvider
    {
        /**
         * The constructor establishes two roots that matter throughout the class:
         * the content root and the web root. The content root is where the application
         * itself lives, while the web root is where publicly servable files live.
         * 
         * ContentRootPath is server-private by default.
         * WebRootPath is the directory that ASP.NET Core uses to serve static files.
         * This is typically the wwwroot folder.
         * 
         * You do not want configuration files, plugins, or binaries to be downloadable
         * just because they exist on disk. By keeping them under ContentRootPath and
         * outside WebRootPath, ASP.NET Core guarantees that they are never served accidentally.
         * 
         * Product images, thumbnails, and uploaded media live under WebRootPath
         * because they must be publicly accessible. Plugins, themes, and internal
         * resources live under ContentRootPath because they must not be exposed directly.
         * 
         * The constructor defensively handles the case where the provided path might
         * point to a file rather than a directory (Path.GetDirectoryName(path) returns
         * the directory containing 'path' if 'path' is a file),
         * normalizing both roots to directory paths. This normalization is essential
         * because later path combinations assume directory semantics and would break
         * subtly if a file path slipped through.
         * 
         */
        public GlideBuyFileProvider(IWebHostEnvironment webHostEnvironment)
            : base(File.Exists(webHostEnvironment.ContentRootPath)
                  ? Path.GetDirectoryName(webHostEnvironment.ContentRootPath)!
                  : webHostEnvironment.ContentRootPath)
        {
            WebRootPath = File.Exists(webHostEnvironment.WebRootPath)
                ? Path.GetDirectoryName(webHostEnvironment.WebRootPath)!
                : webHostEnvironment.WebRootPath;
        }

        public string WebRootPath { get; }

        /**
         * This helper performs the actual deletion and then waits briefly while repeatedly checking whether the directory still exists. The reason for this loop is tied to Windows filesystem semantics. The Windows API can mark a directory for deletion while it is still technically present on disk due to open handles, particularly if the directory is visible in Windows Explorer. The short retry loop gives the operating system time to finish removing the directory before the application proceeds. Without this safeguard, applications that frequently manipulate directories can experience intermittent failures that are very difficult to reproduce.
         */
        protected virtual void DeleteDirectoryRecursive(string path)
        {
            Directory.Delete(path, true);
            const int maxIterationToWait = 10;
            var curIteration = 0;

            while (Directory.Exists(path))
            {
                curIteration += 1;

                if (curIteration > maxIterationToWait)
                    return;

                Thread.Sleep(100);
            }
        }

        /**
         * Is the string a valid Universal Naming Convention (UNC) path?
         * 
         * UNC stands for Universal (or Uniform or Unified) Naming Convention
         * and is a syntax for accessing folders and files on a network of computers.
         * The syntax is as follows:
         * \\<computer name>\<shared directory>\
         * The computer name is always preceded by a double backward slash (\\).
         * UNC paths cannot contain a drive letter (such as D).
         */
        protected static bool IsUncPath(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
        }

        /**
         * The Windows convention is to use a backward slash (\) as the separator in a path. UNIX systems use a forward slash (/). 
         * 
         * The method normalizes path separators, preserves UNC paths on Windows,
         * and ensures that Unix paths are rooted correctly by adding a leading
         * slash when necessary. This one method quietly prevents an entire class
         * of bugs that would otherwise appear only after deployment to Linux-based hosting.
         * 
         * If Nop were targeting only a single operating system and only using clean inputs, it could simply call Path.Combine. But in a real application there are several complications: paths may contain mixed separators (/ and \), they may already be partially combined, they may contain UNC network paths, and the application may run on both Windows and Unix-based systems. The Combine method therefore acts as a normalization layer before delegating the final combination to Path.Combine.
         */
        public virtual string Combine(params string[] paths)
        {
            /**
             * What this line does conceptually is flatten the input paths into clean segments before combining them. If a path element is a normal path, it is split by both backslashes and forward slashes. This means that inputs like "images/thumbs", "images\\thumbs", or "images/thumbs/" all become the same set of segments. This normalization prevents subtle errors where Path.Combine would otherwise treat a full string as a single segment and produce unexpected results.
             * 
             * However, the method makes an exception for UNC paths. A UNC path is a Windows network path such as \\server\share\folder. The helper IsUncPath checks whether a string represents such a path. If it does, the code avoids splitting it into segments. This is important because splitting a UNC path would destroy its meaning. The entire string must be treated as a single root path when combining.
             * 
             * Once the segments are flattened and normalized, the method passes them to Path.Combine. At that point the .NET runtime produces a valid platform-specific path.
             */
            var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? [p] : p.Split('\\', '/')).ToArray());

            /**
             * This line ensures that the resulting path is absolute on Unix-like systems.
             * 
             * When you split segments aggressively and recombine them, it is possible to accidentally lose the leading slash that indicates an absolute path. By explicitly restoring it, the method guarantees that paths such as /var/www/images/thumbs remain absolute rather than becoming var/www/images/thumbs.
             * 
             * Multiple slashes are allowed and are equivalent to a single slash. From the Single Unix specification (version 4), base definitions §3.271 pathname: “Multiple successive slashes are considered to be the same as one slash.”
             */
            if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
            {
                path = "/" + path;
            }

            return path;
        }

        #region Directory/file utilities

        /**
         * When you examine the parts of NopFileProvider that deal with directory creation, file creation, deletion, and movement, it is useful to think of them not as complex algorithms but as controlled wrappers around System.IO. The .NET runtime already provides all of these capabilities, but NopCommerce introduces a provider layer so that the rest of the application never interacts with System.IO directly. This allows the framework to enforce consistent behavior, handle common edge cases once, and isolate platform differences in a single location.
         */

        /**
         * The method CreateDirectory checks whether the directory already exists before calling Directory.CreateDirectory. The underlying .NET method already tolerates existing directories, but the explicit check makes the intent clearer and prevents unnecessary system calls. More importantly, it ensures that the provider always uses its own DirectoryExists method rather than letting other parts of the application call Directory.Exists directly. That consistency is the real purpose of the wrapper.
         */
        public virtual void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }

        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public virtual void DeleteDirectory(string path)
        {
            ArgumentNullException.ThrowIfNull(path);

            // See this answer https://stackoverflow.com/a/1703799/6093615
            // This solution only addresses the peculiarities of interacting with Windows Explorer. 

            foreach (var directory in Directory.GetDirectories(path))
                DeleteDirectory(directory);

            try
            {
                DeleteDirectoryRecursive(path);
            }
            catch (IOException)
            {
                DeleteDirectoryRecursive(path);
            }
            catch (UnauthorizedAccessException)
            {
                DeleteDirectoryRecursive(path);
            }
        }

        public virtual void DirectoryMove(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        public virtual IEnumerable<string> EnumerateFiles(string dirPath, string searchPattern, bool topDirOnly = true)
        {
            return Directory.EnumerateFiles(dirPath, searchPattern, topDirOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /**
         * When CreateFile is called, the provider first checks whether the file
         * already exists. If it does, the method simply returns. If it does not,
         * the provider determines the directory that should contain the file and
         * ensures that directory exists by calling CreateDirectory. Only then does
         * it create the file using File.Create.
         * 
         * This is important because the .NET file creation APIs do not automatically
         * create parent directories. Without this step, callers would need to manually
         * ensure directory existence before creating files, which would scatter filesystem
         * knowledge throughout the codebase.
         */
        public virtual void CreateFile(string path)
        {
            if (File.Exists(path))
                return;

            var fileInfo = new FileInfo(path);
            CreateDirectory(fileInfo.DirectoryName);

            using (File.Create(path)) { }
        }

        public virtual bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public virtual void DeleteFile(string path)
        {
            if (!FileExists(path))
                return;

            File.Delete(path);
        }

        public virtual void FileMove(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        public virtual void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public virtual long FileLength(string path)
        {
            if (!FileExists(path))
                return -1;

            return new FileInfo(path).Length;
        }

        #endregion

        #region Getters
        #endregion

        #region Read/Write operations

        public virtual async Task<byte[]> ReadAllBytesAsync(string filePath)
        {
            return FileExists(filePath) ? await File.ReadAllBytesAsync(filePath) : Array.Empty<byte>();
        }

        public virtual async Task<string> ReadAllTextAsync(string path, Encoding encoding)
        {
            /**
             * The Nop implementation deliberately opens the file with a custom FileStream configuration so it can control the file sharing mode, something the convenience API does not allow.
             * 
             * The important part here is FileShare.ReadWrite. This flag allows the file to be read even if another process or thread currently has the file open for writing. In a web application like NopCommerce this situation happens frequently. For example, configuration files, plugin resources, or generated files might be updated while the application is still running. If the file were opened with the default sharing rules, the read operation could throw an exception when another process holds a write lock.
             * 
             * The convenience method File.ReadAllTextAsync() does not give you control over the sharing mode. Internally it opens the file using a FileStream with default parameters, which typically result in FileShare.Read. That means the file can be read by multiple readers but not while it is open for writing. In practice this can cause intermittent IOException errors in long-running web applications where files might be modified concurrently.
             */

            await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(fileStream, encoding);

            return await streamReader.ReadToEndAsync();
        }

        public virtual string ReadAllText(string path, Encoding encoding)
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(fileStream, encoding);

            return streamReader.ReadToEnd();
        }

        public virtual async Task WriteAllBytesAsync(string filePath, byte[] bytes)
        {
            await File.WriteAllBytesAsync(filePath, bytes);
        }

        public virtual async Task WriteAllTextAsync(string filePath, string contents, Encoding encoding)
        {
            await File.WriteAllTextAsync(filePath, contents, encoding);
        }

        public virtual void WriteAllText(string filePath, string contents, Encoding encoding)
        {
            File.WriteAllText(filePath, contents, encoding);
        }

        #endregion
    }
}
