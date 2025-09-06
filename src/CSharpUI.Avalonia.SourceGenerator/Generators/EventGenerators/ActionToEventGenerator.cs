using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace CSharpUI.Avalonia.SourceGenerator.Generators.EventGenerators;

public class ActionToEventGenerator : ExtensionGeneratorBase<EventExtensionInfo>
{
    protected override string? GetExtension(EventExtensionInfo @event)
    {
        var eventHandler = @event.EventHandler;
        //var eventParameterTypes = new List<string>() { "global::System.Object?" };

        var argsString = $"global::System.Action<{string.Join(", ", @event.EventParameterTypes)}> action";

        // Generate the lambda parameter names (arg0, arg1, etc.)
        var lambdaParameters = string.Join(", ", @event.EventParameterTypes.Select((type, index) => $"arg{index}"));

        // Generate the action call string
        var actionCallStr = $"action({lambdaParameters})";

        // If the delegate has more than one parameter, split them into individual arguments
        if (@event.HasMultipleParameters)
        {
            lambdaParameters = string.Join(", ", @event.EventParameterTypes.Select((type, index) => $"arg{index}"));
        }
        else if (@event.HasSingleParameter)
        {
            lambdaParameters = "arg0, arg1";
            actionCallStr = "action(arg0, arg1)";
        }

        var eventName = @event.EventName;
        var extensionName = "On" + eventName;

        var extensionBody =
            $"        => control._setEvent(({eventHandler})(({lambdaParameters}) => {actionCallStr}), h => control.{eventName} += h);";

        if (@event.IsRoutedEvent)
        {
            argsString += ", global::Avalonia.Interactivity.RoutingStrategies? routes = null";

            extensionBody =
                  $"{Extensions.NewLine}{{{Extensions.NewLine}"
                + $"        => control.AddHandler({@event.ControlTypeName}.{@eventName}Event, (_, args) => action(args), routes ?? default(RoutingStrategies));"
                + $"}}{Extensions.NewLine}";
        }

        var extensionText =
            (@event.IsObsolete ? "[Obsolete]" : "")
            + $"    public static {@event.ReturnType} {extensionName}{@event.GenericArg}"
            + $"(this {@event.ReturnType} control, {argsString}) {@event.GenericConstraint}{Extensions.NewLine}"
            + extensionBody;

        return extensionText;
    }

}