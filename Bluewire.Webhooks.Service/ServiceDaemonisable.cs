using System;
using System.Threading;
using System.Threading.Tasks;
using Bluewire.Common.Console;
using log4net;
using Microsoft.Owin.Hosting;
using Owin;

namespace Bluewire.Webhooks.Service
{
    public class ServiceDaemonisable : IDaemonisable
    {
        private static readonly ILog log = LogManager.GetLogger("Bluewire.Webhooks.Service");

        public string Name => "Bluewire.Webhooks.Service";

        public string[] GetDependencies() => new string[0];

        public Task<IDaemon> Start(CancellationToken token)
        {
            var daemon = new Daemon();
            try
            {
                log.Info("Starting...");
                var configuration = new ConfigurationReader().ReadConfiguration();

                var resolver = new DependencyResolver();

                log.Info("Starting web server...");
                var startOptions = new StartOptions();
                foreach (var baseUri in configuration.BaseUris)
                {
                    log.Info($"Base URI: {baseUri}");
                    startOptions.Urls.Add(baseUri);
                }
                daemon.Track(
                    WebApp.Start(startOptions, builder => {
                        var config = new WebAppConfigurationBuilder().BuildConfiguration(resolver);
                        builder.UseWebApi(config);
                    }));

                return Task.FromResult<IDaemon>(daemon);
            }
            catch (Exception ex)
            {
                log.Fatal("Failed to start", ex);
                daemon.Dispose();
                throw;
            }
        }

        private class Daemon : DaemonBase
        {
            private readonly CancellationTokenSource shutdownCts = new CancellationTokenSource();
            public CancellationToken ShutdownToken => shutdownCts.Token;

            protected override void Dispose(bool disposing)
            {
                try
                {
                    if (!disposing) return;
                    log.Info("Shutting down.");
                    shutdownCts.Cancel();
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
