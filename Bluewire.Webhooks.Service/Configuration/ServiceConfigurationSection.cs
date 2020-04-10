using System.Configuration;
using System.Linq;

namespace Bluewire.Webhooks.Service.Configuration
{
    public class ServiceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("hosting", IsRequired = true)]
        public HostingConfigurationElementCollection Hosting => (HostingConfigurationElementCollection)base["hosting"];

        public class HostingConfigurationElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement() => new BaseUriConfigurationElement();
            protected override object GetElementKey(ConfigurationElement element) => ((BaseUriConfigurationElement)element).BaseUri;

            public string[] GetBaseUris() => this.Cast<BaseUriConfigurationElement>().Select(e => e.BaseUri).ToArray();

            public class BaseUriConfigurationElement : ConfigurationElement
            {
                [ConfigurationProperty("baseUri", IsKey = true, IsRequired = true)]
                public string BaseUri => (string)base["baseUri"];
            }
        }
    }
}
