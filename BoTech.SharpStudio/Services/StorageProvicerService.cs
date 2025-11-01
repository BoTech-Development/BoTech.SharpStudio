using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using BoTech.SharpStudio.ViewModels;
using BoTech.SharpStudio.Views;

namespace BoTech.SharpStudio.Services;

public class StorageProviderService
{
    public static StorageProviderService? Instance;
    private TopLevel _topLevel;

    private StorageProviderService(TopLevel topLevel)
    {
        _topLevel = topLevel;
    }
    public static void CreateInstance(Visual topLevelControl)
    {
        TopLevel?  topLevel = TopLevel.GetTopLevel(topLevelControl);
        if(topLevel != null)
            Instance = new StorageProviderService(topLevel);
        else
            throw new NullReferenceException("Can not get the TopLevel Control!");
    }

    public static IStorageProvider GetStorageProvider()
    {
        if(Instance == null) throw new InvalidOperationException("StorageProviderService not instantiated before accessing this Method.");
        return Instance.GetStorageProviderFromMainWindow();
    }

    private IStorageProvider GetStorageProviderFromMainWindow()
    {
        return _topLevel.StorageProvider;
    }
}