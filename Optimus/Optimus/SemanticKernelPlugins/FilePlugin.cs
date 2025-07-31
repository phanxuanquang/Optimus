using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins
{
    public class FilePlugin
    {
        // ---------- FILE OPERATIONS ----------

        [KernelFunction, Description("Reads the full text content from the specified file path.")]
        public async Task<string> ReadFileAsync(
            [Description("The full path to the file to read.")] string filePath)
        {
            return File.Exists(filePath)
                ? await File.ReadAllTextAsync(filePath, Encoding.UTF8)
                : $"File not found: `{filePath}`";
        }

        [KernelFunction, Description("Writes text to the specified file, with optional overwrite.")]
        public async Task<string> WriteFileAsync(
            [Description("The full file path to write to.")] string filePath,
            [Description("The content to write into the file.")] string content,
            [Description("Set to true to overwrite the file if it exists.")] bool overwrite = true)
        {
            if (File.Exists(filePath) && !overwrite)
                return $"File already exists: `{filePath}`";

            await File.WriteAllTextAsync(filePath, content, Encoding.UTF8);
            return $"Written to file: `{filePath}`";
        }

        [KernelFunction, Description("Appends text to the end of a file. Creates the file if it does not exist.")]
        public async Task<string> AppendToFileAsync(
            [Description("The full file path to append to.")] string filePath,
            [Description("The content to append.")] string content)
        {
            await File.AppendAllTextAsync(filePath, content, Encoding.UTF8);
            return $"Appended to file: `{filePath}`";
        }

        [KernelFunction, Description("Renames a file to the new name within the same directory.")]
        public string RenameFile(
            [Description("The full path to the original file.")] string sourcePath,
            [Description("The new name of the file (without path).")] string newName)
        {
            if (!File.Exists(sourcePath))
                return $"File not found: {sourcePath}";

            var directory = Path.GetDirectoryName(sourcePath)!;
            var destPath = Path.Combine(directory, newName);
            File.Move(sourcePath, destPath);
            return $"Renamed to: {destPath}";
        }

        [KernelFunction, Description("Deletes the specified file if it exists.")]
        public string DeleteFile(
            [Description("The full file path to delete.")] string filePath)
        {
            if (!File.Exists(filePath))
                return $"File not found: `{filePath}`";

            File.Delete(filePath);
            return $"Deleted file: `{filePath}`";
        }

        [KernelFunction, Description("Moves a file to a new destination path.")]
        public string MoveFile(
            [Description("The full source file path.")] string sourcePath,
            [Description("The destination file path.")] string destPath)
        {
            if (!File.Exists(sourcePath))
                return $"File not found: {sourcePath}";

            File.Move(sourcePath, destPath, true);
            return $"Moved to: {destPath}";
        }

        [KernelFunction, Description("Copies a file to a new destination.")]
        public string CopyFile(
            [Description("The full source file path.")] string sourcePath,
            [Description("The destination file path.")] string destPath,
            [Description("Set to true to overwrite the destination if it exists.")] bool overwrite = true)
        {
            if (!File.Exists(sourcePath))
                return $"Source file not found: {sourcePath}";

            File.Copy(sourcePath, destPath, overwrite);
            return $"Copied to: {destPath}";
        }

        [KernelFunction, Description("Checks whether a file exists at the given path.")]
        public string FileExists(
            [Description("The full path to the file to check.")] string filePath)
        {
            return File.Exists(filePath) ? "true" : "false";
        }

        [KernelFunction, Description("Returns metadata information about the specified file.")]
        public string GetFileInfo(
            [Description("The full path to the file.")] string filePath)
        {
            if (!File.Exists(filePath))
                return $"File not found: `{filePath}`";

            var info = new FileInfo(filePath);
            return $"Name: {info.Name}\nSize: {info.Length} bytes\nCreated: {info.CreationTime}\nLast Modified: {info.LastWriteTime}\nExtension: {info.Extension}";
        }

        [KernelFunction, Description("Searches for files within a folder matching a keyword and optional pattern.")]
        public string SearchFiles(
            [Description("The root folder to search in.")] string folderPath,
            [Description("Keyword to match in file names.")] string keyword,
            [Description("File pattern to search for, e.g., *.txt")] string searchPattern = "*.*")
        {
            if (!Directory.Exists(folderPath))
                return $"Folder not found: {folderPath}";

            var files = Directory.GetFiles(folderPath, searchPattern, SearchOption.AllDirectories)
                                 .Where(f => Path.GetFileName(f).Contains(keyword, StringComparison.OrdinalIgnoreCase));

            return string.Join("\n", files);
        }

        // ---------- FOLDER OPERATIONS ----------

        [KernelFunction, Description("Creates a new folder at the specified path.")]
        public string CreateFolder(
            [Description("The full folder path to create.")] string folderPath)
        {
            if (Directory.Exists(folderPath))
                return $"Folder already exists: {folderPath}";

            Directory.CreateDirectory(folderPath);
            return $"Created folder: {folderPath}";
        }

        [KernelFunction, Description("Deletes the folder at the specified path. Recursive by default.")]
        public string DeleteFolder(
            [Description("The full folder path to delete.")] string folderPath,
            [Description("Set to true to delete contents recursively.")] bool recursive = true)
        {
            if (!Directory.Exists(folderPath))
                return $"Folder not found: {folderPath}";

            Directory.Delete(folderPath, recursive);
            return $"Deleted folder: {folderPath}";
        }

        [KernelFunction, Description("Renames a folder to a new name in the same parent directory.")]
        public string RenameFolder(
            [Description("The full path to the folder.")] string folderPath,
            [Description("The new folder name (not full path).")] string newName)
        {
            if (!Directory.Exists(folderPath))
                return $"Folder not found: {folderPath}";

            var parent = Path.GetDirectoryName(folderPath)!;
            var destPath = Path.Combine(parent, newName);
            Directory.Move(folderPath, destPath);
            return $"Renamed folder to: {destPath}";
        }

        [KernelFunction, Description("Lists all files in a specified folder.")]
        public string ListFiles(
            [Description("The full folder path to list files from.")] string folderPath,
            [Description("Optional search pattern, e.g., *.csv")] string searchPattern = "*.*")
        {
            if (!Directory.Exists(folderPath))
                return $"Folder not found: {folderPath}";

            var files = Directory.GetFiles(folderPath, searchPattern);
            return string.Join("\n", files);
        }

        [KernelFunction, Description("Lists all subfolders in a specified folder.")]
        public string ListFolders(
            [Description("The full folder path to list subfolders from.")] string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return $"Folder not found: {folderPath}";

            var folders = Directory.GetDirectories(folderPath);
            return string.Join("\n", folders);
        }

        [KernelFunction, Description("Checks whether a folder exists at the given path.")]
        public string FolderExists(
            [Description("The full folder path to check.")] string folderPath)
        {
            return Directory.Exists(folderPath) ? "true" : "false";
        }

        [KernelFunction, Description("Returns the parent folder path of a given file or folder path.")]
        public string GetParentFolder(
            [Description("The full path to a file or folder.")] string path)
        {
            return Directory.Exists(path) || File.Exists(path)
                ? Path.GetDirectoryName(path) ?? "No parent"
                : $"Path not found: {path}";
        }
    }
}
