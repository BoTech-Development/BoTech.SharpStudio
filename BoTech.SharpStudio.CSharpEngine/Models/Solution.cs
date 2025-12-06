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
    public void SaveAllProjectsToDisk()
    {
        foreach (Project project in Projects)
        {
            project.ProjectInfo.Save();
        }
    }
    public bool CreatesDependencyToProjectACircularDependency(Project fromProject, Project toProject)
    {
        bool result = FindProjectDependencyInProjectAndSubProjects(fromProject, toProject);
        if(!result) result = FindProjectDependencyInProjectAndSubProjects(toProject, fromProject);
        return result;
    }
    /// <summary>
    /// Searches in all dependencies of the given project for the project to find-
    /// </summary>
    /// <param name="project"></param>
    /// <param name="projectToFind"></param>
    /// <returns>true, when the <see cref="projectToFind"/> param was found in a dependency of the given project or in a sub project. Else false.</returns>
    private bool FindProjectDependencyInProjectAndSubProjects(Project project, Project projectToFind)
    {
        bool result = false;
        foreach (Project currentProejct in project.DependsOn)
        {
            if(currentProejct == projectToFind) result = true;
            if(FindProjectDependencyInProjectAndSubProjects(currentProejct, projectToFind)) result = true;
        }
        return result;
    }

    public string GetRelativeProjectPathToSolutionFolder(string absolutePath)
    {
        if(FileSystemItem.FileInfo == null && FileSystemItem.FileInfo.Directory == null) throw new InvalidOperationException("Solution folder path could not be determined.");
        string SolutionFolderPath = FileSystemItem.FileInfo.Directory?.FullName;
        return ".." + absolutePath.Replace(SolutionFolderPath, string.Empty);
    }
}