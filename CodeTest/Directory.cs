public class Directory
{
    public string Name { get; set; } = string.Empty;

    public int TotalSize { get; set; }

    public Directory? ParentDirectory { get; set; }

    public List<Directory> ChildDirectories { get; set; } = new List<Directory>();

    public List<File> Files { get; set; } = new List<File>();
}