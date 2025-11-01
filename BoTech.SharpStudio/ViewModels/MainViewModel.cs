using Avalonia.Controls;
using BoTech.SharpStudio.Views;
using ReactiveUI;
using ShadUI;

namespace BoTech.SharpStudio.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DialogManager _dialogManager;
    private readonly ToastManager _toastManager;
    private Control _currentContent = new TextBlock()
    {
        Text = "BoTech.SharpStudio is loading..."
    };

    public Control CurrentContent
    {
        get => _currentContent; 
        set => this.RaiseAndSetIfChanged(ref _currentContent, value);
    }
    public DialogManager DialogManager => _dialogManager;
    public ToastManager ToastManager => _toastManager;
    public static MainViewModel CurrentInstance { get; private set; }
    public MainViewModel(DialogManager dialogManager, ToastManager toastManager)
    {
        _dialogManager = dialogManager;
        _toastManager = toastManager;
        _currentContent = new WelcomeView()
        {
            DataContext = new WelcomeViewModel(_dialogManager, _toastManager)
        };
        CurrentInstance = this;
    }
}