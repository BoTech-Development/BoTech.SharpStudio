using Avalonia.Controls;
using BoTech.SharpStudio.CSharpEngine.Models;
using BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles;
using Material.Icons;
using Microsoft.Build.Construction;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.ViewModels.Editor.Tools
{
    public class SolutionExplorerViewModel : ViewModelBase, IToolViewModel<Solution>
    {
        public ObservableCollection<ItemNode> SolutionNodes { get; set; } = new ObservableCollection<ItemNode>();

        public Solution CurrentObject { get; set; }
        private bool _excludeBinaries = true;
        public bool ExcludeBinaries
        {
            get => _excludeBinaries;
            set
            {
                this.RaiseAndSetIfChanged(ref _excludeBinaries, value);
                OnReload();
            }
        }
		/// <summary>
		/// Solution or File System View
		/// </summary>
		public int SelectedViewType { get => _isSolutionView ? 0 : 1;   
			set 
            { 
                if (value == 0)
                {
                    _isSolutionView = true;
				}
                else
                    _isSolutionView = false;
                OnReload();
			}
        }
        private bool _isSolutionView = true;

		public void InitializeTool()
        {
            SolutionNodes.Clear();
            SolutionNodes.Add(new ItemNode(CurrentObject.FileSystemItem)
            {
                Name = System.IO.Path.GetFileName(CurrentObject.AbsolutePath),
                Solution = CurrentObject,
                Children = new ObservableCollection<ItemNode>(CreateProjectItemNodes(CurrentObject))
            });
        }
        private List<ItemNode> CreateProjectItemNodes(Solution solution)
        {
            ItemNode temp;
            List<ItemNode> projectNodes = new List<ItemNode>();
            foreach (var project in solution.Projects)
            {
                temp = new ItemNode(project.FileSystemItem)
                {
                    Name = project.Name,
                    Project = project
                };
                // We need this foreach loop because the root folder should not be inserted "twice" into the Solution Explorer. 
                foreach (var child in project.FileSystemItem.Children)
                    CreateFileSystemItemNodesForProject(child, temp);
                projectNodes.Add(temp);
            }
            return projectNodes;
        }
        private void CreateFileSystemItemNodesForProject(FileSystemItem item, ItemNode parentNode)
        {
            if(ShouldExcludeItem(item))
                return;
			ItemNode newNode = new ItemNode(item)
            {
                Name = item.Name,
                FileSystemItem = item
            };
            parentNode.Children.Add(newNode);
            foreach (var child in item.Children)
            {
                CreateFileSystemItemNodesForProject(child, newNode);
            }
        }
        private bool ShouldExcludeItem(FileSystemItem item)
        {
            return (_excludeBinaries && (IsBinaryFile(item) || IsBinaryDirectory(item))) ||
                   (_isSolutionView && IsSolutionFile(item));
        }
        private bool IsSolutionFile(FileSystemItem item)
        {
            string extension = System.IO.Path.GetExtension(item.Name).ToLower();
            return extension == ".sln" || extension == ".csproj" || extension == ".vbproj" || extension == ".fsproj";
		}
		private bool IsBinaryDirectory(FileSystemItem item)
        {
            string lowerName = item.Name.ToLower();
            return lowerName == "bin" || lowerName == "obj" || lowerName == "debug" || lowerName == "release";
        }
		private bool IsBinaryFile(FileSystemItem item)
        {
            if (item.FileWithActions != null)
            {
                string extension = System.IO.Path.GetExtension(item.Name).ToLower();
                return extension switch
                {
                    ".dll" => true,
                    ".exe" => true,
                    ".pdb" => true,
                    ".lib" => true,
                    ".obj" => true,
                    _ => false,
                };
            }
            return false;
		}
        
		public void OnCurrentObjectChanged(Solution oldObject, Solution newObject)
        {
            
        }

        public void OnReload()
        {
            InitializeTool();
        }
    }
    public class ItemNode
    {
        public string Name { get; set; }
        public ObservableCollection<ItemNode> Children { get; set; } = new ObservableCollection<ItemNode>();
        public FileSystemItem FileSystemItem { get; set; }
		public Solution? Solution { get; set; }
        public Project? Project { get; set; }
        public MaterialIconKind Icon { get; private set; }
		public ItemNode(FileSystemItem fileSystemItem) 
        {
            if(fileSystemItem.FileWithActions != null)
                Icon = fileSystemItem.FileWithActions.Icon;
			else
			    Icon = MaterialIconKind.FolderOutline;
		    FileSystemItem = fileSystemItem;
		}
	}
}
