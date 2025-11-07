using ReactiveUI;
using ShadUI;

namespace BoTech.SharpStudio.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public readonly DialogManager DialogManager;
    public readonly ToastManager ToastManager;
    public ViewModelBase(DialogManager dialogManager, ToastManager toastManager)
    {
        DialogManager = dialogManager;
        ToastManager = toastManager;
    }
}