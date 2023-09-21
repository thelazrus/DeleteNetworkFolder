using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: DeleteFolders.exe <UNC_Path1> <UNC_Path2> ...");
            return;
        }

        foreach (var uncPath in args)
        {
            DeleteNetworkFolder(uncPath);
        }
    }

    static void DeleteNetworkFolder(string folderPath)
    {
        try
        {
            if (Directory.Exists(folderPath))
            {
                // Recursively remove read-only attribute from the folder and its contents
                RemoveReadOnlyAttributeRecursively(folderPath);

                // Delete the folder and its contents recursively
                Directory.Delete(folderPath, true);
                Console.WriteLine($"Deleted folder: {folderPath}");
            }
            else
            {
                Console.WriteLine($"Folder not found: {folderPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting folder {folderPath}: {ex.Message}");
        }
    }

    static void RemoveReadOnlyAttributeRecursively(string folderPath)
    {
        try
        {
            // Remove read-only attribute from the folder itself
            if ((File.GetAttributes(folderPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(folderPath, File.GetAttributes(folderPath) & ~FileAttributes.ReadOnly);
            }

            // Remove read-only attribute from all files and subfolders recursively
            foreach (var file in Directory.GetFiles(folderPath))
            {
                if ((File.GetAttributes(file) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);
                }
            }

            foreach (var subfolder in Directory.GetDirectories(folderPath))
            {
                RemoveReadOnlyAttributeRecursively(subfolder);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing read-only attribute in {folderPath}: {ex.Message}");
        }
    }
}
