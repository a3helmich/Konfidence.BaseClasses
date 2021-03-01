using System;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    public static class CommandLineArgumentExtensions
    {
        [UsedImplicitly]
        public static bool TryParseArgument([NotNull] this string[] args, [NotNull] Enum argument, [NotNull] out string commandLineArgument, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            commandLineArgument = string.Empty;

            if (!args.Any())
            {
                return false;
            }

            var arg = $"-{argument}";

            if (arg.Length > 2)
            {
                arg = $"-{arg}";
            }

            var argumentValues = args
                .Where(x => x.StartsWith(arg, stringComparison))
                .Select(x => x.TrimStartIgnoreCase(arg, true))
                .Where(x => x.StartsWith(" ") || x.TrimStart().StartsWith("=") || x.TrimStart().StartsWith(":"))
                .Select(x => x.TrimStart().TrimStart("=").TrimStart(":"))
                .ToList();

            if (!argumentValues.Any())
            {
                return false;
            }

            commandLineArgument = argumentValues.First();

            return true;
        }
    }
}
