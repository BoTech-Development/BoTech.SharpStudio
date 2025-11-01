using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BoTech.SharpStudio.Services;
using BoTech.SharpStudio.ViewModels;
using Dock.Model.Core;
using ReactiveUI;
using StaticViewLocator;

namespace BoTech.SharpStudio;

public class ViewLocator : IDataTemplate
{
	public Control Build(object data)
	{
		var name = data.GetType().FullName!.Replace("ViewModel", "View");
		var type = Type.GetType(name);

		if (type != null)
		{
			return (Control)Activator.CreateInstance(type)!;
		}
		else
		{
			return new TextBlock { Text = "Not Found: " + name };
		}
	}

	public bool Match(object data)
	{
		return data is ViewModelBase;
	}
}
/*
public class ViewLocator : IDataTemplate, IViewLocator
{
    private readonly IServiceProvider _provider;

    public ViewLocator()//IServiceProvider provider)
    {
        // _provider = provider;
        _provider = DockingServiceGenerator.Initialize();
    }

    private IViewFor? Resolve(object viewModel)
    {
        var vmType = viewModel.GetType();
        var serviceType = typeof(IViewFor<>).MakeGenericType(vmType);
        
        if (_provider.GetService(serviceType) is IViewFor view)
        {
            view.ViewModel = viewModel;
            return view;
        }

        var viewName = vmType.FullName?.Replace("ViewModel", "View");
        if (viewName is not null)
        {
            var viewType = Type.GetType(viewName);
            if (viewType != null && _provider.GetService(viewType) is IViewFor view2)
            {
                view2.ViewModel = viewModel;
                return view2;
            }
        }

        return null;
    }

    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        if (Resolve(data) is IViewFor view && view is Control control)
            return control;

        var viewName = data.GetType().FullName?.Replace("ViewModel", "View");
        return new TextBlock { Text = $"Not Found: {viewName}" };
    }

    public bool Match(object? data)
    {
        if (data is null)
        {
            return false;
        }

        if (data is IDockable)
        {
            return true;
        }

        return Resolve(data) is not null;
    }

    IViewFor? IViewLocator.ResolveView<T>(T? viewModel, string? contract) where T : default =>
        viewModel is null ? null : Resolve(viewModel);
}*/
/*[StaticViewLocator]
public partial class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var type = data.GetType();
        if (s_views.TryGetValue(type, out var func))
            return func.Invoke();

        throw new Exception($"Unable to create view for type: {type}");
    }

    public bool Match(object? data)
    {
        if (data is null)
        {
            return false;
        }

        var type = data.GetType();
        return data is IDockable || s_views.ContainsKey(type);
    }
}*/
/*
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
*/