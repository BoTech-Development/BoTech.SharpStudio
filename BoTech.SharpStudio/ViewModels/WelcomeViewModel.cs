using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BoTech.SharpStudio.CSharpEngine.Controller;
using BoTech.SharpStudio.CSharpEngine.Models;
using BoTech.SharpStudio.Services;
using BoTech.SharpStudio.ViewModels.Dialogs.Git;
using BoTech.SharpStudio.ViewModels.Editor;
using ReactiveUI;
using ShadUI;

namespace BoTech.SharpStudio.ViewModels;

public class WelcomeViewModel : ViewModelBase
{
    private readonly DialogManager _dialogManager;
    private readonly ToastManager _toastManager;
    public ObservableCollection<ProjectViewModel> RecentProjects { get; set;  } = new ObservableCollection<ProjectViewModel>();
    private string _versionInfo = "Version: v1.0.1.Alpha";

    public string VersionInfo
    {
        get => _versionInfo; 
        set => this.RaiseAndSetIfChanged(ref _versionInfo, value);
    } 
    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    public ReactiveCommand<Unit, Unit> GetSolutionFromVersionCommand { get; }
    public WelcomeViewModel(DialogManager dialogManager, ToastManager toastManager)
    {
        _dialogManager = dialogManager;
        _toastManager = toastManager;
        /*
        RecentProjects = new ObservableCollection<ProjectViewModel>()
        {
            new ProjectViewModel()
            {
                ProjectName = "BoTech.SharpStudio",
                ProjectColor = Brushes.Green,
                FirstGradientColor = Colors.LimeGreen,
                SecondGradientColor = Colors.MediumSeaGreen,
                ThirdGradientColor = Colors.LimeGreen,
                FourthGradientColor =  Colors.LightSeaGreen,
                SubInfo = "13.09.25"
            }
        };*/
        OpenCommand = ReactiveCommand.Create(SelectAndOpenSolution);
        GetSolutionFromVersionCommand = ReactiveCommand.Create(GetSolutionFromVersionControl);
    }

    private void SelectAndOpenSolution()
    {
        Thread openFilePickerThread = new Thread(() =>
        {
            List<IStorageFile> files =StorageProviderService.GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Please select an .sln File.",
                AllowMultiple = false,
            }).Result.ToList();
            Console.WriteLine($"You selected {files[0].Name} => {files[0].Path}");
            SolutionController controller = new SolutionController();
            Solution solution = controller.LoadSolutionFromFile(files[0].Path.AbsolutePath);
            controller.AnalyzeSolution(solution);
			Dispatcher.UIThread.InvokeAsync(() =>
            {
                _toastManager.CreateToast($"Solution {solution.SolutionFolderPath} loaded with {solution.Projects.Count} projects.")
                    .WithContent("You can now start working on your solution.")
                    .DismissOnClick()
                    .ShowSuccess();
				EditorContainerViewModel vm = new EditorContainerViewModel();
                vm.OnSolutionLoaded(solution);
				MainViewModel.CurrentInstance.CurrentContent = new EditorContainer()
				{
					DataContext = vm
				};
			});
		});
        
        openFilePickerThread.Start();
    }

    private void GetSolutionFromVersionControl()
    {
        LoginToGitHubDialogViewModel vm = new LoginToGitHubDialogViewModel(_dialogManager);
        _dialogManager.CreateDialog(vm)
            .Dismissible()
            .WithSuccessCallback(() =>
                _toastManager.CreateToast($"Sign in successful {vm.UserName} + {vm.Password}")
                    .WithContent(new Button()
                    {
                        Content = "Login",
                    })
                    .DismissOnClick()
                    .ShowSuccess())
            .WithCancelCallback(() =>
                _toastManager.CreateToast("Sign in cancelled")
                    .WithContent("Please sign in to continue.")
                    .DismissOnClick()
                    .ShowWarning())
            .Show();
    }
}