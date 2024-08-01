var input = new Input();

string[] inputs = input.InputString.Split('\n');

var root = new Directory
{
    Name = "/"
};

// Default to root
var currentDirectory = root;
var directories = new List<Directory>
{
    root
};

// Main
foreach(var inp in inputs)
{
    if(string.IsNullOrWhiteSpace(inp))
    {
        continue;
    }

    // Commands
    if(inp[0] == '$')
    {
        // According to the rules, the only commands used for this exercise are "cd" and "ls", 
        // so I can safely check for those specific length of characters.
        var command = inp.Substring(2,2);
        
        switch(command)
        {
            // Change directory
            case("cd"):
                var directoryName = inp.Substring(5).Trim();

                if(directoryName == "/")
                {
                    currentDirectory = root;
                }
                // Go back a level
                else if(directoryName == "..")
                {
                    currentDirectory = currentDirectory?.ParentDirectory;
                }
                // Change directory
                else
                {
                    // The directory to change to should already exist in parent, so use that one
                    currentDirectory = currentDirectory?.ChildDirectories.Single(x => x.Name == directoryName);
                }
                break;

            // List directory
            case("ls"):
                break;
        }
    }
    else
    {
        if(inp.Substring(0, 3) == "dir")
        {
            // Child Directories
            var newChild = new Directory
            {
                Name = inp.Substring(4).Trim(),
                ParentDirectory = currentDirectory
            };

            directories.Add(newChild);
            currentDirectory?.ChildDirectories.Add(newChild);
        }
        else
        {
            //File
            string[] file = inp.Split(" ");
            var newFile = new File
            {
                FileSize = int.Parse(file[0]),
                FileName = file[1].Trim(),
            };
            currentDirectory?.Files.Add(newFile);
        }
    }
}

int GetTotalSpaceUsedForDirectory(string directoryName)
    => directories.Where(x => x.Name == directoryName).Sum(x => GetFileSizes(x));

int GetSizeOfDirectoriesUnderOneHundred(List<Directory> yourDirectories)
{
    var directoriesUnder = new List<Directory>();
    foreach(var d in yourDirectories)
    {
        d.TotalSize = GetFileSizes(d);
        
        if(d.TotalSize <= 100000)
        {
            directoriesUnder.Add(d);
        }
    }

    var size = 0;
    foreach(var dir in directoriesUnder)
    {
        size += dir.TotalSize;
    }

    return size;
}

int GetFileSizes(Directory directory)
{
    var size = 0;
    if(directory.Files != null)
    {
        size += directory.Files.Sum(d => d.FileSize);
    }

    if(directory.ChildDirectories != null)
    {
        foreach(var child in directory.ChildDirectories)
        {
            size += GetFileSizes(child);
        };
    }

    return size;
}

// Part #1
System.Console.WriteLine("Part #1");
Console.WriteLine("Total Size Under 100000: " + GetSizeOfDirectoriesUnderOneHundred(directories));

// Part #2
System.Console.WriteLine("\n");
System.Console.WriteLine("Part #2");

var totalSpaceAvailable = 70000000;
var unusedSpaceRequired = 30000000;
var totalUsedSpace = GetTotalSpaceUsedForDirectory("/");
var totalUnusedSpace = totalSpaceAvailable - totalUsedSpace;
var spaceNeeded = unusedSpaceRequired - totalUnusedSpace;
// Console.WriteLine("Used space: " + totalUsedSpace);
// Console.WriteLine("Unused space: " + totalUnusedSpace);
// Console.WriteLine("Space still needed: " + spaceNeeded);

var allDirectoriesThatWouldGiveMeEnoughSpace = directories.Where(d => d.TotalSize > spaceNeeded).ToList();
var smallestOneToDelete = allDirectoriesThatWouldGiveMeEnoughSpace.Min(d => d.TotalSize);

System.Console.WriteLine("Smallest one: " + smallestOneToDelete);

// DONE!