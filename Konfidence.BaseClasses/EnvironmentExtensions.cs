using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Konfidence.Base
{
    public static class EnvironmentExtensions
    {
        // this should be Linux proof
        public static bool TryGetEnvironmentVariable(this string environmentVariable, out string value)
        {
            value = Environment
                .GetEnvironmentVariables(EnvironmentVariableTarget.User)
                .GetValue(environmentVariable);

            if (value.IsAssigned())
            {
                return true;
            }

            value = Environment
                .GetEnvironmentVariables(EnvironmentVariableTarget.Machine)
                .GetValue(environmentVariable);

            if (value.IsAssigned())
            {
                return true;
            }

            value = Environment
                .GetEnvironmentVariables(EnvironmentVariableTarget.Process)
                .GetValue(environmentVariable);

            return value.IsAssigned();
        }

        private static string GetValue(this IDictionary environmentVariables, string environmentVariable)
        {
            List<string> keys = environmentVariables.Keys.OfType<string>().ToList();

            if (keys.Any())
            {
                string key = keys.FirstOrDefault(x => x.Equals(environmentVariable, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

                if (key.IsAssigned())
                {
                    string? stringValue = (string?)environmentVariables[key];

                    if (stringValue.IsAssigned())
                    {
                        return stringValue;
                    }
                }
            }

            return string.Empty;
        }
    }
}
