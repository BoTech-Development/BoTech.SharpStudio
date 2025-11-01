using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BoTech.SharpStudio.Services;
using BoTech.SharpStudio.ViewModels;
using BoTech.SharpStudio.Views;

namespace BoTech.SharpStudio;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Init Services...
        ServiceGenerator.CreateServices();
        ServiceGenerator.InitializeDiaologManager();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceGenerator.ServiceProvider.GetService(typeof(MainViewModel))
            };
            StorageProviderService.CreateInstance(desktop.MainWindow);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainWindow()
            {
                DataContext = ServiceGenerator.ServiceProvider.GetService(typeof(MainViewModel))
            };
            StorageProviderService.CreateInstance(singleViewPlatform.MainView);
        }

        base.OnFrameworkInitializationCompleted();
    }
}