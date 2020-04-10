using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using Bluewire.Webhooks.Service.Configuration;

namespace Bluewire.Webhooks.Service
{
    public class ConfigurationReader
    {
        public ConfigurationSettings ReadConfiguration()
        {
            var section = (ServiceConfigurationSection)ConfigurationManager.GetSection("service");
            return new ConfigurationSettings
            {
                BaseUris = section.Hosting.GetBaseUris(),
            };
        }

        private Uri GetUri(string uriString)
        {
            if (string.IsNullOrWhiteSpace(uriString)) return null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out var uri)) return uri;
            throw new ConfigurationErrorsException($"Not a valid absolute URI: {uriString}");
        }

        private string GetFullPathFromConfiguredPath(string configuredPath)
        {
            if (Path.IsPathRooted(configuredPath)) return configuredPath;
            var serviceRoot = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (configuredPath.StartsWith("~/"))
            {
                var relativePath = configuredPath.Substring(1).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);;
                if (Path.IsPathRooted(configuredPath)) throw new ConfigurationErrorsException($"Invalid server-relative path: {configuredPath}");

                return Path.Combine(serviceRoot, relativePath);
            }
            return Path.Combine(serviceRoot, configuredPath);
        }

        private NetworkCredential GetUserNameAndPasswordCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;
            return new NetworkCredential(userName, password);
        }

        public class ConfigurationSettings
        {
            public string[] BaseUris { get; set; }
        }
    }
}
