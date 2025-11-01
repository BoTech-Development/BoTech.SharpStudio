using BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace BoTech.SharpStudio.CSharpEngine.Models;
/// <summary>
/// The Project Model is a wrapper class for the Class <see cref="ProjectInSolution"/><br/>
/// This class is used to extract the most important information from the <see cref="ProjectInSolution"/> class.<br/>
/// This class can also be serialized and thus saved.
/// </summary>
public class Project : IInitializable
{
    
    /// <summary>
    /// The parsed Info of this Project.
    /// </summary>
    public  Microsoft.Build.Evaluation.Project ProjectInfo { get; private set; }
    /// <summary>
    /// The Name of the Project, based on the <see cref="ProjectInSolution"/> param in the <see cref="Project"/> Constructor.
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The id of the Project, based on the <see cref="ProjectInSolution"/> param in the <see cref="Project"/> Constructor.
    /// </summary>
    public string Guid { get; private set; }
    /// <summary>
    /// The Path to the Project File, based on the <see cref="ProjectInSolution"/> param in the <see cref="Project"/> Constructor.
    /// </summary>
    public string AbsolutePath { get; private set; }

    /// <summary>
    /// A List of all Projects that this project needs to compile.
    /// </summary>
    public List<Project> DependsOn { get; private set; } = new List<Project>();
    public FilesSystemItem ProjectFiles { get; private set;  }

	private Solution _solution;

    public Project(Solution solution, ProjectInSolution projectInSolution, Microsoft.Build.Evaluation.Project loadedProject)
    {
        _solution = solution;
        Name = projectInSolution.ProjectName;
        Guid = projectInSolution.ProjectGuid;
        AbsolutePath = projectInSolution.AbsolutePath;
        ProjectInfo = loadedProject;
        ProjectFiles = new FilesSystemItem(new DirectoryInfo(loadedProject.DirectoryPath));
	}

    /// <inheritdoc />
    public void Initialize()
    {
        InitializeDependencyProperty();
    }
    /// <summary>
    /// Inits the <see cref="DependsOn"/> Property
    /// </summary>
    /// <exception cref="TypeInitializationException"></exception>
    private void InitializeDependencyProperty()
    {
       // foreach (var property in ProjectInfo.Properties) Console.WriteLine(property.Name);
        ICollection<ProjectItem> projectReferences = ProjectInfo.GetItems("ProjectReference");
        foreach (ProjectItem reference in projectReferences)
        {
            Console.WriteLine($"Project {Name} has a reference to: {reference.EvaluatedInclude}"); // Path to the referenced .csproj
            // Find the Project by its relative name:
            string? solutionFolderPath = _solution.SolutionFolderPath;
            if (solutionFolderPath != null)
            {
                // Remove the first .. at the begining of the Path : e.g. ../MyProject/MyProject.csproj
                string absoluteProjectPath = solutionFolderPath + reference.EvaluatedInclude.Substring(2, reference.EvaluatedInclude.Length - 2);
                Project? dependProject = _solution.Projects.Find(project => project.ProjectInfo.FullPath.Equals(absoluteProjectPath));
                if(dependProject != null) 
                    DependsOn.Add(dependProject);
            }
        }
    }
}