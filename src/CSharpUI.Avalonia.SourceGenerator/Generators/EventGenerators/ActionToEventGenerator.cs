using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;

namespace CSharpUI.Avalonia.SourceGenerator.Generators.EventGenerators;

public class ActionToEventGenerator : ExtensionGeneratorBase<EventExtensionInfo>
{
    protected override string? GetExtension(EventExtensionInfo @event)
    {
        var eventHandler = @event.EventHandler;
        var eventParameterTypes = new List<string>() { "object?" };
        eventParameterTypes.AddRange(@event.EventParameterTypes);
        var argsString = $"Action<{string.Join(", ", eventParameterTypes)}> action";

        // Generate the lambda parameter names (arg0, arg1, etc.)
        var lambdaParameters = string.Join(", ", eventParameterTypes.Select((type, index) => $"arg{index}"));

        // Generate the action call string
        var actionCallStr = $"action({lambdaParameters})";

        // If the delegate has more than one parameter, split them into individual arguments
        if (@event.HasMultipleParameters)
        {
            lambdaParameters = string.Join(", ", eventParameterTypes.Select((type, index) => $"arg{index}"));
        }
        else if (@event.HasSingleParameter)
        {
            lambdaParameters = "arg0";
        }
        else
        {
            lambdaParameters = "args";
        }

        var eventName = @event.EventName;
        var extensionName = "On" + eventName;

        var extensionBody =
            $"        => control._setEvent(({eventHandler})(({lambdaParameters}) => {actionCallStr}), h => control.{eventName} += h);";

        if (@event.IsRoutedEvent)
        {
            argsString += ", RoutingStrategies? routes = null";

            extensionBody = $"        => control.AddHandler({@event.ControlTypeName}.{@eventName}Event, (_, args) => action(args), routes ?? default(RoutingStrategies));";

        }


        var extensionText =
            (@event.IsObsolete ? "[Obsolete]" : "")
            + $"    public static {@event.ReturnType} {extensionName}{@event.GenericArg}"
            + $"(this {@event.ReturnType} control, {argsString}) {@event.GenericConstraint}{Extensions.NewLine}"
            + extensionBody;

        return extensionText;
    }

}