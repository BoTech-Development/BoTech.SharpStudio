using Avalonia.Threading;
using BoTech.SharpStudio.CSharpEngine.Controller;
using BoTech.SharpStudio.CSharpEngine.Models;
using BoTech.SharpStudio.ViewModels;
using BoTech.SharpStudio.ViewModels.Editor;
using ShadUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.Controller
{
    internal class EditorController
    {
        public SolutionController? SolutionController { get; private set; } 
        public static EditorController Instance { get; } = new EditorController();
        private EditorContainerViewModel _editorContainerViewModel;
        private DialogManager _dialogManager;
        private ToastManager _toastManager;
        private EditorController() { }
        public void LoadSolutionAndInitializeEditorViews(string solutionPath, DialogManager dialogManager, ToastManager toastManager)
        {
            this._dialogManager = dialogManager;
            this._toastManager = toastManager;

            SolutionController = new SolutionController();
            Solution solution = SolutionController.LoadSolutionFromFile(solutionPath);
            SolutionController.AnalyzeSolution(solution);
            _editorContainerViewModel = new EditorContainerViewModel(dialogManager, toastManager);
            _editorContainerViewModel.OnSolutionLoaded(solution);
            MainViewModel.CurrentInstance.CurrentContent = new EditorContainer()
            {
                DataContext = _editorContainerViewModel
            };
            NotifyUserThatSolutionIsLoaded(toastManager, solution);
        }
        public void ReloadSolutionProjectsAndRefreshEditorViews(Solution solution)
        { 
            SolutionController.ReloadSolutionProjects(solution);
            SolutionController.AnalyzeSolution(solution);
            _editorContainerViewModel.OnSolutionLoaded(solution);
            NotifyUserThatSolutionIsLoaded(_toastManager, solution);
        }

        private void NotifyUserThatSolutionIsLoaded(ToastManager toastManager, Solution solution)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                toastManager.CreateToast($"Solution {solution.SolutionFolderPath} loaded with {solution.Projects.Count} projects.")
                    .WithContent("You can now start working on your solution.")
                    .DismissOnClick()
                    .ShowSuccess();

            });
        }
    }
}
