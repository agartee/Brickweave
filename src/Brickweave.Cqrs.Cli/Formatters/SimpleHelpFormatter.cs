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
                WriteParentInfoToConsole();
                WriteSubgroupInfoToConsole();
                WriteSubcommandInfoToConsole();
            }
            else
            {
                WriteParentInfoToConsole();
                WriteParameterInfoToConsole();
            }

            return stringBuilder.ToString();

            void WriteParentInfoToConsole()
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(helpInfo.Type == HelpInfoType.Category ? "Category:" : "Command:");
                stringBuilder.AppendLine();

                FormatAndWriteToConsole(helpInfo.Name, helpInfo.Description);
            }

            void WriteSubgroupInfoToConsole()
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
                    FormatAndWriteToConsole(subgroup.Name, subgroup.Description);
            }

            void WriteSubcommandInfoToConsole()
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
                    FormatAndWriteToConsole(command.Name, command.Description);
            }

            void WriteParameterInfoToConsole()
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
                    FormatAndWriteToConsole($"--{parameter.Name}", parameter.Description);
                }
            }

            void FormatAndWriteToConsole(string name, string description)
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
}
