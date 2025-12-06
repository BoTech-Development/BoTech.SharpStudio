using Avalonia.Controls;
using BoTech.SharpStudio.CSharpEngine.Models;
using DynamicData;
using ReactiveUI;
using ShadUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.ViewModels.Editor.Tools
{
    internal class ManageProjectDependenciesViewModel : ViewModelBase
    {
        public ObservableCollection<DependencyItemViewModel> Dependencies { get; set; } = new ObservableCollection<DependencyItemViewModel>();
        public ObservableCollection<ComboBoxItem> AvailableProjects { get; set; } = new ObservableCollection<ComboBoxItem>();
        private int _selectedProjectIndex = 0;
        public int SelectedProjectIndex 
        { 
            get => _selectedProjectIndex;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _selectedProjectIndex, value);
                InitializeViewForSelectedProject();
			}
        }
        public List<List<DependencyItemViewModel>> Settings { get; private set; } = new List<List<DependencyItemViewModel>>();
        public List<List<DependencyItemViewModel>> SettingsOld { get; private set; } = new List<List<DependencyItemViewModel>>();

        public ReactiveCommand<Unit, Unit> CloseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }

        private Solution _loadedSolution;
        public ManageProjectDependenciesViewModel(DialogManager dialogManager, ToastManager toastManager, Solution loadedSolution, Project currentProject) : base(dialogManager, toastManager)
        {
            CloseCommand = ReactiveCommand.Create(() =>
            {
				dialogManager.Close(this);
            });
            SaveCommand = ReactiveCommand.Create(() =>
            {
				dialogManager.Close(this, new CloseDialogOptions { Success = true });
			});

            InitializeView(loadedSolution, currentProject);
            _loadedSolution = loadedSolution;
        }
		private void InitializeView(Solution loadedSolution, Project currentProject)
		{
            InitializeAvailableProjects(loadedSolution, currentProject);
			InitializeDependenciesForAllProjects(loadedSolution);

			InitializeDependenciesViewForProject(currentProject);
		}
        /// <summary>
        /// Creates ComboboxItem-List for the Combobox
        /// </summary>
        /// <param name="loadedSolution"></param>
        /// <param name="currentProject"></param>
		private void InitializeAvailableProjects(Solution loadedSolution, Project currentProject)
        {
            int index = 0;
			AvailableProjects.Clear();
            foreach (Project project in loadedSolution.Projects)
            {
                if(project == currentProject) SelectedProjectIndex = index;
				ComboBoxItem item = new ComboBoxItem()
                {
                    Content = project.Name
                };
                AvailableProjects.Add(item);
                index++;
            }
		}
        /// <summary>
        /// Inits the Settings list.
        /// </summary>
        /// <param name="loadedSolution"></param>
        private void InitializeDependenciesForAllProjects(Solution loadedSolution)
        {
            Settings.Clear();
            SettingsOld.Clear();
            foreach (Project project in loadedSolution.Projects)
            {
                List<DependencyItemViewModel> projectDependencies = new List<DependencyItemViewModel>();
                List<DependencyItemViewModel> projectDependenciesCopy = new List<DependencyItemViewModel>();
                foreach (Project otherProject in loadedSolution.Projects)
                {
                    bool isSelected = project.DependsOn.Contains(otherProject);
                    bool isCurrentProject = project == otherProject;
                    bool isDependencyCircular = false;

                    if (!isSelected && !isCurrentProject) isDependencyCircular = loadedSolution.CreatesDependencyToProjectACircularDependency(project, otherProject);//otherProject.DependsOn.Contains(project),
                    
                    projectDependencies.Add(new DependencyItemViewModel(DialogManager, ToastManager)
                    {
                        CurrentProjectName = project.Name,
                        ProjectName = otherProject.Name,
                        IsDependencyCircular = isDependencyCircular,
                        IsTheCurrentProject = isCurrentProject,
                        IsSelected = isSelected,
                    });
                    projectDependenciesCopy.Add(new DependencyItemViewModel(DialogManager, ToastManager)
                    {
                        CurrentProjectName = project.Name,
                        ProjectName = otherProject.Name,
                        IsDependencyCircular = isDependencyCircular,
                        IsTheCurrentProject = isCurrentProject,
                        IsSelected = isSelected,
                    });
                }
                Settings.Add(projectDependencies);
                SettingsOld.Add(projectDependenciesCopy);

            }
        }
        /// <summary>
        /// Updates the View to the selected Project.
        /// </summary>
        private void InitializeViewForSelectedProject()
        {
            if(_loadedSolution == null) return; // Through the reactive ui it could be the case that this method will be called before the inits of the ctor.
            Project currentProject = _loadedSolution.Projects[SelectedProjectIndex];
			InitializeDependenciesViewForProject(currentProject);
        }
        /// <summary>
        /// Finds the sub list of the Settings property and adds the list to the Dependencies Property
        /// </summary>
        /// <param name="currentProject"></param>
        private void InitializeDependenciesViewForProject(Project currentProject)
        {
            Dependencies.Clear();
            List<DependencyItemViewModel>? projectDependencyList = GetDependenciesForProject(currentProject);
            if(projectDependencyList != null)
            {
				Dependencies.AddRange(projectDependencyList);
			}
		}
        /// <summary>
        /// Finds the Sub list in the settings property which coresponds to the given Project
        /// </summary>
        /// <param name="project"></param>
        /// <returns>Returns founded list or null.</returns>
        private List<DependencyItemViewModel>? GetDependenciesForProject(Project project)
        {
            foreach (List<DependencyItemViewModel> projectDependencyList in Settings)
            {
                if(projectDependencyList.First().CurrentProjectName == project.Name) return projectDependencyList;
            }
            return null;
		}
	}
    internal class DependencyItemViewModel : ViewModelBase
    {
        /// <summary>
        /// The name of the project which could have a dependency to ProjectName.
        /// </summary>
        public string CurrentProjectName { get; set; }
        public bool IsSelected { get; set; } = false;
        public string ProjectName { get; set; }
		/// <summary>
		/// One project cannot depend on another project that already depends on it, therefore it will be disabled and labeled in the list.
		/// </summary>
		public bool IsDependencyCircular { get; set; } = false;
		/// <summary>
		/// Each project cannot depend on itself, therefore it will be disabled and labeled in the list.
		/// </summary>
		public bool IsTheCurrentProject { get; set; } = false;
        public bool IsCheckBoxEnabled => !(IsDependencyCircular || IsTheCurrentProject);
		public DependencyItemViewModel(DialogManager dialogManager, ToastManager toastManager) : base(dialogManager, toastManager)
        {
            
        }
    }
}
