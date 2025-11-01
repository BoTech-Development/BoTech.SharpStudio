using BoTech.SharpStudio.CSharpEngine.Models;
using Microsoft.Build.Construction;
using Microsoft.Build.Locator;

namespace BoTech.SharpStudio.CSharpEngine.Controller;

public class SolutionController
{
    /// <summary>
    /// Loads all .sln's and .csproj / .vbproj / .fsproj and creates the Models.
    /// </summary>
    /// <param name="absoluteFilePath">The path to the main .sln file.</param>
    /// <returns>A Model structure which represents the structure of the project.</returns>
    public Solution LoadSolutionFromFile(string absoluteFilePath)
    {
        MSBuildLocator.RegisterDefaults();

        Solution solution = TryToLoadSolutionFromFile(absoluteFilePath);
        LoadProjects(solution);
        InitializeAllProjects(solution);
        return solution;
    }
    /// <summary>
    /// Tries to create the <see cref="Solution"/> Model from the SolutionFile
    /// </summary>
    /// <param name="absoluteFilePath">The absolute path to the .sln File.</param>
    /// <returns>The instance of the Solution Model</returns>
    /// <exception cref="FileNotFoundException">Occurs when the .sln file does not exists.</exception>
    private Solution TryToLoadSolutionFromFile(string absoluteFilePath)
    {
        if (!File.Exists(absoluteFilePath)) throw new FileNotFoundException(absoluteFilePath);
        SolutionFile solution = SolutionFile.Parse(absoluteFilePath);
        return new Solution(solution, absoluteFilePath);
    }
    /// <summary>
    /// Calls the init method of all Projects, to init all properties of the Project instances.
    /// </summary>
    /// <param name="solution">The solution Model which includes all Projects</param>
    private void InitializeAllProjects(Solution solution)
    {
        foreach (var project in solution.Projects)
        {
            project.Initialize();
        }
    }
    /// <summary>
    /// Loads all .csproj / .vbproj / .fsproj files, parses them and injects them into the Solution.
    /// </summary>
    /// <param name="solution"></param>
    private void LoadProjects(Solution solution)
    {
        foreach (ProjectInSolution project in solution.SolutionFile.ProjectsInOrder)
        { 
            Microsoft.Build.Evaluation.Project? loadedProject = TryToLoadProject(solution, project.AbsolutePath);
            if(loadedProject != null) solution.Projects.Add(new Models.Project(solution, project,  loadedProject));
        }
    }
    /// <summary>
    /// Loads a project but with executions which will be logged to the Console.
    /// </summary>
    /// <param name="solution">The Parent solution</param>
    /// <param name="projectFilePath">The project to load</param>
    /// <returns>The Project Model or null when an exception occurs.</returns>
    private Microsoft.Build.Evaluation.Project? TryToLoadProject(Solution solution, string projectFilePath)
    {
        try
        {
            return solution.ProjectCollection.LoadProject(projectFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}