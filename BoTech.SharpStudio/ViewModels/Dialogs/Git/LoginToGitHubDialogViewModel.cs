using System.Reactive;
using ReactiveUI;
using ShadUI;

namespace BoTech.SharpStudio.ViewModels.Dialogs.Git;

public class LoginToGitHubDialogViewModel : ViewModelBase
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public LoginToGitHubDialogViewModel(DialogManager dialogManager)
    {
        LoginCommand = ReactiveCommand.Create(() => dialogManager.Close(this, new CloseDialogOptions { Success = true }));
        CancelCommand = ReactiveCommand.Create(() => dialogManager.Close(this, new CloseDialogOptions { Success = false }));
    }
}