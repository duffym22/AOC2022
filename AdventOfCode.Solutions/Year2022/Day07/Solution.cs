using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Solutions.Year2022.Day07;

class Solution : SolutionBase
{

    public Solution() : base(07, 2022, "") { }

    protected override string SolvePartOne()
    {
        Guid root = Guid.NewGuid();
        Dictionary<Guid, Directory> DirectoryGuidDict = new()
        {
            { root, new Directory() { Name = "/", ParentDirectory = root, Files = new() } }
        };

        PopulateDirectoryDictionary(Input.SplitByNewline().ToList(), DirectoryGuidDict);
        CalculateFileSize(DirectoryGuidDict);

        //Attempt 1: 1783610 (correct)
        return DirectoryGuidDict.Select(x => x.Value).Where(x => x.CalculatedSumFileSize <= 100000).Sum(x => x.CalculatedSumFileSize).ToString();
    }

    protected override string SolvePartTwo()
    {
        int
            totalDiskSpace = 70000000,
            requiredDiskSpace = 30000000;

        Guid root = Guid.NewGuid();
        Dictionary<Guid, Directory> DirectoryGuidDict = new()
        {
            { root, new Directory() { Name = "/", ParentDirectory = root, Files = new() } }
        };

        PopulateDirectoryDictionary(Input.SplitByNewline().ToList(), DirectoryGuidDict);
        CalculateFileSize(DirectoryGuidDict);

        //Determine remaining disk space when subtracting root from it
        int currentDiskSpaceRemaining = totalDiskSpace - DirectoryGuidDict.First().Value.CalculatedSumFileSize;
        int remainingDiskSpace = requiredDiskSpace - currentDiskSpaceRemaining;

        List<int> allCalculatedSizes = DirectoryGuidDict.Select(x => x.Value.CalculatedSumFileSize).ToList();
        allCalculatedSizes.Sort();
        int retval = 0;
        for (int i = 0; i < allCalculatedSizes.Count; i++)
        {
            if (allCalculatedSizes[i] >= remainingDiskSpace)
            {
                retval = allCalculatedSizes[i];
                break;
            }

        }

        return retval.ToString();
    }

    private void PopulateDirectoryDictionary(List<string> lines, Dictionary<Guid, Directory> DirectoryGuidDict)
    {
        //Set the current directory to the root guid
        Guid CurrentDirectory = DirectoryGuidDict.Keys.FirstOrDefault();
        for (int i = 0; i < lines.Count; i++)
        {
            string[] split = lines[i].Split(' ');

            //Command
            if (lines[i].StartsWith("$"))
            {
                //split[0] ==> '$'
                //The only commands we will see are "cd [x]", "cd ..", and "ls"
                if (split[1].Equals("cd"))
                {
                    if (split[2].Equals(".."))
                    {
                        //Going up
                        CurrentDirectory = DirectoryGuidDict[CurrentDirectory].ParentDirectory;
                    }
                    else
                    {
                        //Going down - except if this is the root level
                        if (DirectoryGuidDict.Count != 1)
                        {
                            List<Directory> curSubs = DirectoryGuidDict[CurrentDirectory].Subdirectories;
                            foreach (Directory item in curSubs)
                            {
                                if (item.Name.Equals(split[2]))
                                {
                                    CurrentDirectory = item.Guid;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //ls - do nothing
                }
            }
            else if (lines[i].StartsWith("dir"))
            {
                //Add a directory to the current list
                Directory d = new() { Guid = Guid.NewGuid(), Name = split[1], ParentDirectory = CurrentDirectory, Files = new() };
                DirectoryGuidDict.Add(d.Guid, d);
                DirectoryGuidDict[CurrentDirectory].Subdirectories.Add(d);
            }
            else
            {
                //Anything else is a file which follows the convention <size> <name.extension>
                File f = new File() { Guid = Guid.NewGuid(), Name = split[1], Size = int.Parse(split[0]), ParentDirectory = CurrentDirectory };
                //Add the file to the directory with the filename as the key and the value as the size
                DirectoryGuidDict[CurrentDirectory].Files.Add(f);
            }
        }
    }

    private int CountFileSize(Directory dir)
    {
        if (dir.Subdirectories.Count.Equals(0))
        {
            return dir.SumFileSize;
        }
        else
        {
            int counter = 0;
            foreach (Directory item in dir.Subdirectories)
            {
                //If the value was already calculated, then we don't need to do anything
                if (item.CalculatedSumFileSize == 0)
                    item.CalculatedSumFileSize += CountFileSize(item);
                counter += item.CalculatedSumFileSize;
            }
            return dir.SumFileSize + counter;
        }
    }

    private void CalculateFileSize(Dictionary<Guid, Directory> directoryGuidDict)
    {
        foreach (Guid key in directoryGuidDict.Keys)
        {
            directoryGuidDict[key].CalculatedSumFileSize = CountFileSize(directoryGuidDict[key]); ;
        }
    }

    public class Directory
    {
        public Guid Guid { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public Guid ParentDirectory { get; set; } = new();
        public List<Directory> Subdirectories { get; set; } = new();
        public List<File> Files { get; set; } = new();
        public int SumFileSize
        {
            get { return Files.Sum(x => x.Size); }
        }
        public int CalculatedSumFileSize { get; set; }
    }

    public class File
    {
        public Guid Guid { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public Guid ParentDirectory { get; set; } = new();
        public int Size { get; set; } = 0;
    }
}
