using System.Text;
using CSharpUI.Avalonia.SourceGenerator.Analysis;
using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;
using Microsoft.CodeAnalysis;

namespace CSharpUI.Avalonia.SourceGenerator.ExternalGenerators;

internal sealed class ExtensionGroupGenerator(
    string groupName,
    Func<ControlModel, IEnumerable<IMemberExtensionInfo>> membersQuery,
    params ExtensionGeneratorBase[] generators)
{
    public string GroupName { get; } = groupName;

    public string? Generate(ControlModel controlModel, out int generationsCount)
    {
        generationsCount = 0;

        var sb = new StringBuilder(1024);
        foreach (var extensionInfo in membersQuery(controlModel))
        {
            sb.AppendLine($" // {extensionInfo.MemberName}");
            foreach (var generator in generators)
            {
                var extensionCode = generator.GetExtension(extensionInfo);
                if (string.IsNullOrWhiteSpace(extensionCode))
                {
                    continue;
                }

                sb.AppendLine();
                sb.AppendLine($"/*{generator.GetType().Name}*/");
                sb.AppendLine(extensionCode);
                generationsCount++;
            }

            sb.AppendLine();
            sb.AppendLine();
        }

        return generationsCount == 0 ? null : sb.ToString();
    }
}
