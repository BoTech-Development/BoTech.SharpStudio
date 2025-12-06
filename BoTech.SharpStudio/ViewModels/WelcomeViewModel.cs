using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BoTech.SharpStudio.Controller;
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
  
    public ObservableCollection<ProjectViewModel> RecentProjects { get; set;  } = new ObservableCollection<ProjectViewModel>();
    private string _versionInfo = "Version: v1.0.1.Alpha";

    public string VersionInfo
    {
        get => _versionInfo; 
        set => this.RaiseAndSetIfChanged(ref _versionInfo, value);
    } 
    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    public ReactiveCommand<Unit, Unit> GetSolutionFromVersionCommand { get; }
    public WelcomeViewModel(DialogManager dialogManager, ToastManager toastManager) : base(dialogManager, toastManager)
    {
        OpenCommand = ReactiveCommand.Create(SelectAndOpenSolution);
        GetSolutionFromVersionCommand = ReactiveCommand.Create(GetSolutionFromVersionControl);
    }

    private void SelectAndOpenSolution()
    {
        Thread openFilePickerThread = new Thread(() =>
        {
            List<IStorageFile> files = StorageProviderService.GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Please select an .sln File.",
                AllowMultiple = false,
            }).Result.ToList();
            Console.WriteLine($"You selected {files[0].Name} => {files[0].Path}");
            Dispatcher.UIThread.Invoke(() =>
            {
                EditorController.Instance.LoadSolutionAndInitializeEditorViews(files[0].Path.AbsolutePath, DialogManager, ToastManager);
            });
        });
        
        openFilePickerThread.Start();
    }

    private void GetSolutionFromVersionControl()
    {
        LoginToGitHubDialogViewModel vm = new LoginToGitHubDialogViewModel(DialogManager, ToastManager);
        DialogManager.CreateDialog(vm)
            .Dismissible()
            .WithSuccessCallback(() =>
                ToastManager.CreateToast($"Sign in successful {vm.UserName} + {vm.Password}")
                    .WithContent(new Button()
                    {
                        Content = "Login",
                    })
                    .DismissOnClick()
                    .ShowSuccess())
            .WithCancelCallback(() =>
                ToastManager.CreateToast("Sign in cancelled")
                    .WithContent("Please sign in to continue.")
                    .DismissOnClick()
                    .ShowWarning())
            .Show();
    }
}