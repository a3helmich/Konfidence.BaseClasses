using System;
using System.Linq;

namespace Konfidence.Base
{
    public static class EnvironmentExtender
    {
        public static bool TryGetEnvironmentVariable(this string environmentVariable, out string value)
        {
            string key;

            var environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);

            if ((key = environmentVariables.Keys.Cast<string>().ToList().FirstOrDefault(x => x.Equals(environmentVariable, StringComparison.OrdinalIgnoreCase))) != null)
            {
                value = (string)environmentVariables[key];

                return true;
            }

            environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

            if ((key = environmentVariables.Keys.Cast<string>().ToList().FirstOrDefault(x => x.Equals(environmentVariable, StringComparison.OrdinalIgnoreCase))) != null)
            {
                value = (string)environmentVariables[key];

                return true;
            }

            environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);

            if ((key = environmentVariables.Keys.Cast<string>().ToList().FirstOrDefault(x => x.Equals(environmentVariable, StringComparison.OrdinalIgnoreCase))) != null)
            {
                value = (string)environmentVariables[key];

                return true;
            }

            value = string.Empty;

            return false;
        }
    }
}
