using System;
using System.Collections.Generic;
using Avalonia.Controls;
using BoTech.SharpStudio.Views.Dialogs;

namespace BoTech.SharpStudio.ViewModels.Dialogs;
public delegate void OnDialogAccepted();
public delegate void OnDialogClosed();
public delegate void OnDialogInitialized(PagedDialog dialog);

/// <summary>
/// A control which can be used to show multiple page by using a next and previous button.
/// </summary>
public class PagedDialog : ContentControl
{
    /// <summary>
    /// Is called no matter which dialog was initialized
    /// </summary>
    public static OnDialogInitialized? OnDialogInitialized {get; set;}

    /// <summary>
    /// Event will be invoked, when the user click the ok button on the last page
    /// </summary>
    public OnDialogAccepted? OnDialogAccepted {get; set;}
    /// <summary>
    /// Event will be invoked, when the user clicks the cancel button on the first or last page
    /// </summary>
    public OnDialogAccepted? OnDialogCanceled {get; set;}
    /// <summary>
    /// A Model which could contain all inputs that the user makes on specific pages.
    /// </summary>
    public object? DialogDataModel {get; set;}
    /// <summary>
    /// All Views that can be displayed as pages.
    /// it is necessary to store all GenericDialogViews, because in this case the visual parent fill be the same. => No avalonia error
    /// </summary>
    private List<GenericDialogView> _pageViews = new List<GenericDialogView>();

    private int _currentPageIndex;
    
    public PagedDialog(List<UserControl> pageViews)
    {
        InitPages(pageViews);
        if(OnDialogInitialized != null)
            OnDialogInitialized.Invoke(this);
    }
    private PagedDialog()
    {
        
    }
    /// <summary>
    /// Creates an MessageBox with only one or more Buttons
    /// </summary>
    /// <param name="content"></param>
    /// <param name="buttonType">Which Buttons</param>
    /// <param name="actions"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static PagedDialog CreateOneDialogPage(UserControl content, OnePageButtonTypes buttonType, params Action[] actions)
    {
        PagedDialog dialog = new PagedDialog();
        GenericDialogView view = new GenericDialogView(); 
        switch (buttonType)
        {
            case OnePageButtonTypes.Ok:
                if (actions.Length == 1)
                {
                    view.DataContext = new GenericDialogViewModel(content)
                        .AddDialogButton("Ok", actions[0]);
                    break;
                }
                else
                    throw new ArgumentException("You must invoke this Method with one action for the ok button.");
            case OnePageButtonTypes.OkCancel:
                if (actions.Length == 2)
                {
                    view.DataContext = new GenericDialogViewModel(content)
                        .AddDialogButton("Ok", actions[0])
                        .AddDialogButton("Cancel", actions[1]);
                    break;
                }
                else
                    throw new ArgumentException("You must invoke this Method with two actions for the ok and cancel buttons.");
            case OnePageButtonTypes.YesNo:
                if (actions.Length == 2)
                {
                    view.DataContext = new GenericDialogViewModel(content)
                        .AddDialogButton("Yes", actions[0])
                        .AddDialogButton("No", actions[1]);
                    break;
                }
                else
                    throw new ArgumentException("You must invoke this Method two one actions for the Yes and No buttons.");
            case OnePageButtonTypes.YesNoCancel:
                if (actions.Length == 3)
                {
                    view.DataContext = new GenericDialogViewModel(content)
                        .AddDialogButton("Yes", actions[0])
                        .AddDialogButton("No", actions[1])
                        .AddDialogButton("Cancel", actions[2]);
                    break;
                }
                else
                    throw new ArgumentException("You must invoke this Method three actions for the Yes, No and Cancel buttons.");
        }
        InjectGenericDialogViewModelIfNeeded((view.DataContext as GenericDialogViewModel)!, content);
        dialog._pageViews.Add(view);
        dialog.ShowPage(0);
        return dialog;
    }

    private static void InjectGenericDialogViewModelIfNeeded(GenericDialogViewModel viewModel, UserControl content)
    {
        if (content.DataContext is DialogPageBase page)
        {
            page.DialogSettings = viewModel;
        }
    }
    
    private void InitPages(List<UserControl> pageViews)
    {
         int pageNumber = 0;
        foreach (var pageView in pageViews)
        {
            pageNumber++;
            GenericDialogViewModel genericDialogViewModel;
            // Is First Page?
            if (pageNumber == 1)
            {
                genericDialogViewModel = new GenericDialogViewModel(pageView)
                    .AddDialogButton("Cancel", CloseDialog)
                    .AddDialogButton("Next", ShowNextPage);

                // Is last Page?
            } else if (pageNumber == pageViews.Count)
            {

                genericDialogViewModel = new GenericDialogViewModel(pageView)
                    .AddDialogButton("Cancel", CloseDialog)
                    .AddDialogButton("Ok", CloseDialogWithAccepted);
            }
            else // Some page in the middle => previous and next
            {
                
                genericDialogViewModel = new GenericDialogViewModel(pageView)
                    .AddDialogButton("Previous", ShowLastPage)
                    .AddDialogButton("Next", ShowNextPage);
            }
            if (pageView.DataContext is DialogPageBase vm)
            {
                vm.Dialog = this;
                vm.DialogSettings = genericDialogViewModel; // Make it reachable from each Vm.
            }
            _pageViews.Add(new GenericDialogView()
            {
                DataContext = genericDialogViewModel,
            });
        }
    }

    public void ShowLastPage()
    {
        _currentPageIndex--;
        if (!ShowPage(_currentPageIndex))
        {
            // undo everything
            _currentPageIndex++; 
            ShowPage(_currentPageIndex);
        }
    }
    public void ShowNextPage()
    {
        _currentPageIndex++;
        if (!ShowPage(_currentPageIndex))
        {
            // undo everything
            _currentPageIndex--; 
            ShowPage(_currentPageIndex);
        }
    }
    public void CloseDialog()
    {
        if (OnDialogCanceled != null)
            OnDialogCanceled.Invoke();
        //InteractiveContainer.CloseDialog();
    }

    private void CloseDialogWithAccepted()
    {
        if (OnDialogAccepted != null)
            OnDialogAccepted.Invoke();
        //InteractiveContainer.CloseDialog();
    }
    /// <summary>
    /// Shows the n Page
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <returns>true when the page could be displayed</returns>
    public bool ShowPage(int pageNumber)
    {
        if (0 <= pageNumber && pageNumber < _pageViews.Count)
        {
            Content = _pageViews[pageNumber];
            if(_pageViews[pageNumber].DataContext is GenericDialogViewModel genericDialogViewModel)
            {
                if (genericDialogViewModel.ContentOfTheDialog != null)
                {
                    if (genericDialogViewModel.ContentOfTheDialog.DataContext is DialogPageBase vm)
                    {
                        vm.OnPageShow();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public enum OnePageButtonTypes
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }
}
