using Avalonia.Controls;
using BoTech.SharpStudio.CSharpEngine.Models;
using BoTech.SharpStudio.ViewModels.Editor.Tools;
using ShadUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.ViewModels.Editor
{
    public class EditorContainerViewModel : ViewModelBase
    {
        public Control EditorLeftPaneView { get; set; }
        public Control EditorDocumentView { get; set; }
        public Control EditorRightPaneView { get; set; }
        private List<IToolViewModel<object>> _tools = new List<IToolViewModel<object>>();
        public EditorContainerViewModel(DialogManager dialogManager, ToastManager toastManager) : base(dialogManager, toastManager)
        {

        }
        public void OnSolutionLoaded(Solution solution)
        {
            SolutionExplorerViewModel vm = new SolutionExplorerViewModel(DialogManager, ToastManager);
            SolutionExplorerView view = new SolutionExplorerView()
            {
                DataContext = vm
            };
            _tools.Add(vm as IToolViewModel<object>);
            vm.CurrentObject = solution;
            vm.InitializeTool();
            EditorLeftPaneView = view;
            EditorDocumentView = new TextBlock() { Text = "Open a document over the solution explorer." };
        }
        public void OnSolutionReloaded(Solution solution)
        {
            foreach (var tool in _tools)
            {
                tool.OnSolutionReloaded(solution);
            }
        }
    }
}
