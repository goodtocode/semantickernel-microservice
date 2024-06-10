using System;

namespace $safeprojectname$
{
    public static class EnvironmentVariables
    {
        public static void Validate()
        {
            //if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.AppConfigurationConnection)))
            //    throw new ArgumentNullException(EnvironmentVariableKeys.AppConfigurationConnection);
        }
    }

    public struct EnvironmentVariableKeys
    {
        public const string AppConfigurationConnection = "MYCO_MICROSERVICE_CONNECTION";
        public const string EnvironmentAspNetCore = "ASPNETCORE_ENVIRONMENT";
        public const string EnvironmentAzureFunctions = "AZURE_FUNCTIONS_ENVIRONMENT";
    }

    public struct EnvironmentVariableDefaults
    {
        public const string Environment = "Production";
    }
}
