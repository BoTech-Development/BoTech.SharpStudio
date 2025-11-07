using Avalonia.Controls;
using BoTech.SharpStudio.Views;
using ReactiveUI;
using ShadUI;

namespace BoTech.SharpStudio.ViewModels;

public class MainViewModel : ReactiveObject
{

     public DialogManager DialogManager { get; set; }
    public ToastManager ToastManager { get; set; }
    private Control _currentContent = new TextBlock()
    {
        Text = "BoTech.SharpStudio is loading..."
    };

    public Control CurrentContent
    {
        get => _currentContent; 
        set => this.RaiseAndSetIfChanged(ref _currentContent, value);
    }

    public static MainViewModel CurrentInstance { get; private set; }
    public MainViewModel(DialogManager dialogManager, ToastManager toastManager) 
    {
        DialogManager = dialogManager;
        ToastManager = toastManager;
        _currentContent = new WelcomeView()
        {
            DataContext = new WelcomeViewModel(DialogManager, ToastManager)
        };
        CurrentInstance = this;
    }
}