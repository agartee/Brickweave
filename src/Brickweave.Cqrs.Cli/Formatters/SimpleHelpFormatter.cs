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
                WriteParentInfoToConsole(helpInfo, stringBuilder, windowWidth);
                WriteSubgroupInfoToConsole(helpInfo, stringBuilder, windowWidth);
                WriteSubcommandInfoToConsole(helpInfo, stringBuilder, windowWidth);
            }
            else
            {
                WriteParentInfoToConsole(helpInfo, stringBuilder, windowWidth);
                WriteParameterInfoToConsole(helpInfo, stringBuilder, windowWidth);
            }

            return stringBuilder.ToString();
        }

        private static void WriteParentInfoToConsole(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(helpInfo.Type == HelpInfoType.Category ? "Category:" : "Command:");
            stringBuilder.AppendLine();

            FormatAndWriteToConsole(helpInfo.Name, helpInfo.Description, stringBuilder, windowWidth);
        }

        private static void WriteSubgroupInfoToConsole(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
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
                FormatAndWriteToConsole(subgroup.Name, subgroup.Description, stringBuilder, windowWidth);
        }

        private static void WriteSubcommandInfoToConsole(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
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
                FormatAndWriteToConsole(command.Name, command.Description, stringBuilder, windowWidth);
        }

        private static void WriteParameterInfoToConsole(HelpInfo helpInfo, StringBuilder stringBuilder, int windowWidth)
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
                FormatAndWriteToConsole($"--{parameter.Name}", parameter.Description, stringBuilder, windowWidth);
            }
        }

        private static void FormatAndWriteToConsole(string name, string description, StringBuilder stringBuilder, int windowWidth)
        {
            var descriptionWrapped = description.Wrap(windowWidth - TEXT_WRAP_LEFT_PADDING);

            stringBuilder.AppendFormat(FORMAT, name, descriptionWrapped.Length > 0
                ? descriptionWrapped[0]
                : null);
            stringBuilder.AppendLine();

            if (descriptionWrapped.Length <= 1)
                return;

            for (var i = 1; i < descriptionWrapped.Length; i++)
                stringBuilder.AppendLine(new string(' ', TEXT_WRAP_LEFT_PADDING) + descriptionWrapped[i]);
        }
    }
}
