using System.Linq;
using System.Text;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Formatters
{
    public static class SimpleHelpFormatter
    {
        private const string FORMAT = "\t{0, -30} : {1, 0}";
        private const int TEXT_WRAP_LEFT_PADDING = 41;

        public static string Format(HelpInfo helpInfo, int windowWidth = 120)
        {
            var stringBuilder = new StringBuilder();

            if (helpInfo.Type == HelpInfoType.Category)
            {
                WriteParentInfo(helpInfo, stringBuilder, windowWidth);
                WriteSubgroupInfo(helpInfo, stringBuilder, windowWidth);
                WriteSubcommandInfo(helpInfo, stringBuilder, windowWidth);
                WriteParameterFormatNotes(stringBuilder, windowWidth);
            }
            else
            {
                WriteParentInfo(helpInfo, stringBuilder, windowWidth);
                WriteParameterInfo(helpInfo, stringBuilder, windowWidth);
                WriteParameterFormatNotes(stringBuilder, windowWidth);
            }

            return stringBuilder.ToString();
        }

        private static void WriteParentInfo(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(helpInfo.Type == HelpInfoType.Category ? "Category:" : "Command:");
            stringBuilder.AppendLine();

            FormatAndWrite(stringBuilder, windowWidth, helpInfo.Name, helpInfo.Description);
        }

        private static void WriteSubgroupInfo(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
        {
            var subgroups = helpInfo.Children
                .Where(i => i.Type == HelpInfoType.Category)
                .ToList();

            if (!subgroups.Any())
                return;

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Subcategories:");
            stringBuilder.AppendLine();

            foreach (var subgroup in subgroups)
                FormatAndWrite(stringBuilder, windowWidth, subgroup.Name, subgroup.Description);
        }

        private static void WriteSubcommandInfo(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
        {
            var commands = helpInfo.Children
                .Where(i => i.Type == HelpInfoType.Executable)
                .ToList();

            if (!commands.Any())
                return;

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Commands:");
            stringBuilder.AppendLine();

            foreach (var command in commands)
                FormatAndWrite(stringBuilder, windowWidth, command.Name, command.Description);
        }

        private static void WriteParameterInfo(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
        {
            var parameters = helpInfo.Children
                .Where(i => i.Type == HelpInfoType.Parameter)
                .ToList();

            if (!parameters.Any())
                return;

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Parameters:");
            stringBuilder.AppendLine();

            foreach (var parameter in parameters)
            {
                FormatAndWrite(stringBuilder, windowWidth, $"--{parameter.Name}", parameter.Description);
            }
        }

        private static void WriteParameterFormatNotes(StringBuilder stringBuilder, int windowWidth)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Parameter Format:");
            stringBuilder.AppendLine();

            FormatAndWrite(stringBuilder, windowWidth, 
                "Basic",
                "Parameter name followed by a space and the parameter value. Parameter values that include spaces must be wrapped in double-quotes.",
                new[] {
                    "--param value",
                    "--param \"value with spaces\""
                });

            FormatAndWrite(stringBuilder, windowWidth, 
                "Date/Time", 
                "Follows the same rules as basic parameters and values. Date format parsing is determined by the API configuration.");

            FormatAndWrite(stringBuilder, windowWidth, 
                "List",
                "Multiple parameter values can be passed by separating values with a space. Parameter values that include spaces must be wrapped in double-quotes.",
                new[] {
                    "--param value1 value2 value3",
                    "--param \"first value with spaces\" \"second value with spaces\"",
                    "--param \"first value with spaces\" secondValueNoSpaces"
                });

            FormatAndWrite(stringBuilder, windowWidth, 
                "Key/Value Pair",
                "Keys and Values follow the same rules as basic and list parameters. Values are prefixed with an equals and wrapped in square braces.",
                new[] {
                    "--param key[=value]",
                    "--param key[=value1] key[=value2]",
                    "--param key[=\"value with spaces\"]",
                    "--param \"key with spaces\"[=value]",
                    "--param \"key with spaces\"[=\"value with spaces\"]"
                });
        }

        private static void FormatAndWrite(StringBuilder stringBuilder, int windowWidth, string name, string description, params string[] examples)
        {
            var descriptionWrapped = description.Wrap(windowWidth - TEXT_WRAP_LEFT_PADDING).ToList();

            if(examples.Any())
            {
                descriptionWrapped.Add("Examples:");
                descriptionWrapped.AddRange(examples.Select(e => $"  {e}"));
            }

            stringBuilder.AppendFormat(FORMAT, name, descriptionWrapped.Count > 0
                ? descriptionWrapped[0]
                : null);
            stringBuilder.AppendLine();

            if (descriptionWrapped.Count <= 1)
                return;

            for (var i = 1; i < descriptionWrapped.Count; i++)
                stringBuilder.AppendLine(new string(' ', TEXT_WRAP_LEFT_PADDING) + descriptionWrapped[i]);
        }
    }
}
