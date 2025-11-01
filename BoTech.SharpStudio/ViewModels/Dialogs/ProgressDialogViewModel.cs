using System;
using System.Text;
using System.Threading;
using Avalonia.Media;
using BoTech.SharpStudio.Services;
using BoTech.SharpStudio.Views.Dialogs;
using Material.Icons;
using ReactiveUI;
using ShadUI;

namespace BoTech.SharpStudio.ViewModels.Dialogs;

public class ProgressDialogViewModel : DialogPageBase
{
    private string _progressText = "View Init. Please wait...";
    /// <summary>
    /// Text above the ProgressBar
    /// </summary>
    public string ProgressText
    {
        get => _progressText;
        set
        {
            string visualImprovedString = InsertNewLineEvery26Chars(value);
            this.RaiseAndSetIfChanged(ref _progressText, visualImprovedString);
        } 
    }
    public static string InsertNewLineEvery26Chars(string input)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.Length; i += 26)
        {
            int length = Math.Min(26, input.Length - i);
            result.Append(input.Substring(i, length));
            if (i + 26 < input.Length) result.Append("\n");
        }
        return result.ToString();
    }
    

    private int _minimum = 0;
    /// <summary>
    /// Progressbar minimum value
    /// </summary>
    public int Minimum
    {
        get => _minimum; 
        set => this.RaiseAndSetIfChanged(ref _minimum, value);
    }
    private int _maximum = 100;
    /// <summary>
    /// Progressbar Maximum value => default is 100%
    /// </summary>
    public int Maximum
    {
        get => _maximum; 
        set => this.RaiseAndSetIfChanged(ref _maximum, value);
    }
    private int _value = 0;
    /// <summary>
    /// Progressbar Value value
    /// </summary>
    public int Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);
            if(_value == Maximum) UpdateDialogButtons(true);
        }
    }

    private bool _isIndeterminate = false;
    /// <summary>
    /// Progressbar Value value
    /// </summary>
    public bool IsIndeterminate
    {
        get => _isIndeterminate; 
        set => this.RaiseAndSetIfChanged(ref _isIndeterminate, value);
    }
    /// <summary>
    /// The action that should be executed on Show. <br/>
    /// return value is not processed
    /// </summary>
    public Action<object?, ProgressDialogViewModel>  Action { get; set; }

    public ProgressDialogViewModel()
    {
        PagedDialog.OnDialogInitialized += OnDialogInitialized;
    }
    public static void CreateProgressbarPage(Action<object?, ProgressDialogViewModel> action)
    {
        DialogManager? manager = (DialogManager?)ServiceGenerator.ServiceProvider.GetService(typeof(DialogManager));
        ProgressDialogViewModel progressVm = new ProgressDialogViewModel();
        progressVm.Action = action;
        ProgressDialogView progressDialogView = new ProgressDialogView()
        {
            DataContext = progressVm
        };
        PagedDialog dialogView = PagedDialog.CreateOneDialogPage(progressDialogView, PagedDialog.OnePageButtonTypes.Ok, () =>
        { 
            manager.Close(progressVm, new CloseDialogOptions()
            {
                Success = true
            });
           // InteractiveContainer.CloseDialog();
        });

        //InteractiveContainer.ShowDialog(dialogView);
    }
    public override void OnPageShow()
    {
        Thread actionThread;
        if (Dialog != null)
        {
            actionThread = new Thread(() =>
            {
                Action.Invoke(Dialog.DialogDataModel, this);
            });
        }
        else
        {
            actionThread = new Thread(() =>
            {
                Action.Invoke("", this);
            });
        }
        actionThread.Start();
    }

    public void Next(int count, string progressText)
    {
        Value += count;
        ProgressText = progressText;
    }

    public void UpdateProgressIntermediate(bool isIndeterminate, string progressText, IBrush iconColor,
        MaterialIconKind iconKind)
    {
        if (DialogSettings != null)
        {
            DialogSettings.Icon = iconKind;
            DialogSettings.IconColor = iconColor;
        }
        ProgressText = progressText;
        IsIndeterminate = isIndeterminate;
    }
    public void UpdateProgress(int value, string progressText, IBrush? iconColor = null,
    MaterialIconKind? iconKind = null)
    {
        IsIndeterminate = false;
        if (DialogSettings != null)
        {
            if (iconColor == null && iconKind == null)
            {
                if (value < Maximum)
                {
                    DialogSettings.Icon = MaterialIconKind.Loading;
                    DialogSettings.IconColor = Brushes.Blue;
                }
                else
                {
                    DialogSettings.Icon = MaterialIconKind.Check;
                    DialogSettings.IconColor = Brushes.Green;
                }
            }
            else if(iconKind != null && iconColor != null)
            {
                DialogSettings.IconColor = iconColor;
                DialogSettings.Icon = (MaterialIconKind)iconKind;
            }
          
        }
        ProgressText = progressText;
        Value = value;
    }
    /// <summary>
    /// sets all buttons to disabled until the given action has been executed.
    /// </summary>
    /// <param name="dialog"></param>
    private void OnDialogInitialized(PagedDialog dialog)
    {
        UpdateDialogButtons(false);
    }
    /// <summary>
    /// When the progress value is equals to Maximum, all buttons will be enabled.
    /// </summary>
    private void UpdateDialogButtons(bool areButtonsEnabled)
    {
        if(this.DialogSettings != null)
            foreach (DialogButton button in this.DialogSettings.DialogButtons)
                button.IsEnabled = areButtonsEnabled;
    }
}