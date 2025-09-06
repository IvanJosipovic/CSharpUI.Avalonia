using CSharpUI.Avalonia;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(HotReloadManager))]
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

namespace CSharpUI.Avalonia;

public static class HotReloadManager
{
    private static readonly Dictionary<Type, HashSet<IReloadable>> Instances = new();

    public static event Action<Type[]?>? HotReloaded;

    public static bool IsEnabled { get; private set; }

    public static void Activate() => IsEnabled = true;

    public static void Deactivate() => IsEnabled = false;

    private static void OnHotReloaded(Type[]? types) => HotReloaded?.Invoke(types);

    public static void ClearCache(Type[]? types)
    {
        Console.WriteLine("ClearCache for types: " + PrintTypes(types));
    }

    public static void UpdateApplication(Type[]? types)
    {
        if (IsEnabled)
            ReloadInstances(types);

        OnHotReloaded(types);
    }

    private static void ReloadInstances(Type[]? types)
    {
        Console.WriteLine("UpdateApplication for types: " + PrintTypes(types));
        if (types == null)
            return;

        foreach (var type in types)
        {
            if (!Instances.TryGetValue(type, out var instances))
                continue;

            foreach (var instance in instances)
                instance.Reload();
        }
    }

    public static string PrintTypes(Type[]? types)
    {
        if (types != null)
        {
            return string.Join(", ", types.Select(t => t.Name));
        }

        return "";
    }

    [RequiresUnreferencedCode("Calls CSharpUI.Avalonia.HotReloadManager.RegisterMethodWatchers(Type)")]
    internal static void RegisterInstance(IReloadable instance)
    {
        if (!IsEnabled) return;

        var type = instance.GetType();

        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }

        if (!Instances.TryGetValue(type, out var instances))
        {
            instances = [];
            Instances[type] = instances;
        }

        instances.Add(instance);
    }

    internal static void UnregisterInstance(IReloadable instance)
    {
        if (!IsEnabled) return;

        var type = instance.GetType();

        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }

        if (!Instances.TryGetValue(type, out var instances)) return;

        instances.Remove(instance);
    }
}