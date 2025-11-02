using BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace BoTech.SharpStudio.CSharpEngine.Models;

public class Solution
{
    /// <summary>
    /// Info about the .sln File
    /// </summary>
    public FileSystemItem  FileSystemItem { get; set; }
    /// <summary>
    /// The parsed .sln File.
    /// </summary>
    public SolutionFile SolutionFile { get; private set; }
    /// <summary>
    /// The absolute Path to the solution file (.sln File)
    /// </summary>
    public string AbsolutePath { get; private set; }
    /// <summary>
    /// The Path to the solution folder without the solution file name and extension.
    /// Can throw exceptions which will be logged to the console, then null will be returned
    /// </summary>
    public string? SolutionFolderPath
    {
        get
        {
            try
            {
                return Path.GetDirectoryName(AbsolutePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
    /// <summary>
    /// All projects in the Solution
    /// </summary>
    public List<Project> Projects { get; set; } = new List<Project>();
	/// <summary>
	/// Created by Microsoft.Build.Evaluation to load Projects
	/// </summary>
	public ProjectCollection ProjectCollection { get; private set; }

    public Solution(SolutionFile solutionFile, string absolutePath)
    {
        SolutionFile = solutionFile;
        AbsolutePath = absolutePath;
        ProjectCollection = new ProjectCollection();
        FileSystemItem = new FileSystemItem(new FileInfo(absolutePath));
	}
}