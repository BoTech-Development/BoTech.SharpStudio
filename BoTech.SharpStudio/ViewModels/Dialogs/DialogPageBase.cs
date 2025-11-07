using ReactiveUI;

namespace BoTech.SharpStudio.ViewModels.Dialogs;

public class DialogPageBase : ReactiveObject
{
    public PagedDialog? Dialog { get; set; }
    public GenericDialogViewModel? DialogSettings { get; set; }

    public virtual void OnPageShow()
    {
        
    }

    public void DisableDialogButton(string buttonText)
    {
        if (DialogSettings != null)
        {
            DialogButton? button = DialogSettings.DialogButtons.Find(b => b.ButtonText == buttonText);
            if (button != null)
            {
                button.IsEnabled = false;
            }
        }
    }
    public void EnableDialogButton(string buttonText)
    {
        if (DialogSettings != null)
        {
            DialogButton? button = DialogSettings.DialogButtons.Find(b => b.ButtonText == buttonText);
            if (button != null)
            {
                button.IsEnabled = true;
            }
        }
    }
}