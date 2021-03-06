using System;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    public static class CommandLineArgumentExtensions
    {
        /// <summary>
        /// Intended to be used together with the configuration argument line parser.
        /// This is not a full fledged argument parser.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="argument"></param>
        /// <param name="commandLineArgument"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
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
