using BoTech.SharpStudio.CSharpEngine.Models;
using BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles;
using Material.Icons;
using Microsoft.Build.Construction;
using Microsoft.VisualBasic;
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
            ItemNode newNode = new ItemNode(item)
            {
                Name = item.Name,
                FileSystemItem = item
            };
            parentNode.Children.Add(newNode);
            foreach (var child in item.Children)
            {
				CreateFileSystemItemNodesForProject( child, newNode);
            }
		}
		public void OnCurrentObjectChanged(Solution oldObject, Solution newObject)
        {
            
        }

        public void OnReload()
        {
            
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
