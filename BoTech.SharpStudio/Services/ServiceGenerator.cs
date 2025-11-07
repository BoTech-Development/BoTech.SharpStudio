using System;
using BoTech.SharpStudio.ViewModels;
using BoTech.SharpStudio.ViewModels.Dialogs.Git;
using BoTech.SharpStudio.ViewModels.Editor.Tools;
using BoTech.SharpStudio.Views;
using BoTech.SharpStudio.Views.Dialogs.Git;
using Microsoft.Extensions.DependencyInjection;
using ShadUI;

namespace BoTech.SharpStudio.Services;
/// <summary>
/// This class manages all singletons as Services and injects it through dependency injection into the classes that needs an instance of the object.
/// </summary>
public class ServiceGenerator
{
    public static ServiceCollection? Services {get; private set;}
    public static ServiceProvider? ServiceProvider {get; private set;}
    /// <summary>
    /// Creates the <see cref="ServiceProvider"/> instance and the <see cref="Services"/> List.
    /// </summary>
    public static void CreateServices()
    {
        Services = new ServiceCollection();
        Services.AddKeyedSingleton(typeof(DialogManager), null);
        Services.AddKeyedSingleton(typeof(ToastManager), null);
        Services.AddTransient(typeof(MainViewModel));
        ServiceProvider = Services.BuildServiceProvider();
    }

    public static void InitializeDiaologManager()
    {
        if(ServiceProvider == null) throw new InvalidOperationException("ServiceProvider not initialized");
        DialogManager? manager = ServiceProvider.GetService(typeof(DialogManager)) as DialogManager;
        if (manager != null)
        {
            manager.Register<LoginToGitHubDialogView, LoginToGitHubDialogViewModel>();
            manager.Register<ManageProjectDependenciesView, ManageProjectDependenciesViewModel>();
        }
    }
}
