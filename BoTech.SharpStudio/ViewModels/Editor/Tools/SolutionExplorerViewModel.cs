using BoTech.SharpStudio.CSharpEngine.Models;
using Microsoft.Build.Construction;
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
            SolutionNodes.Add(new ItemNode()
            {
                Name = System.IO.Path.GetFileName(CurrentObject.AbsolutePath),
                Solution = CurrentObject,
                Children = new ObservableCollection<ItemNode>(CurrentObject.Projects.Select(p => new ItemNode()
                {
                    Name = p.Name,
                    Project = p
                }))
            });
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
        public Solution? Solution { get; set; }
        public Project? Project { get; set; }
		public ItemNode() { }
	}
}
