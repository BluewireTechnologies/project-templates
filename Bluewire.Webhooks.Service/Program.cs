using Bluewire.Common.Console;

namespace Bluewire.Webhooks.Service
{
    class Program
    {
        static int Main(string[] args)
        {
            return DaemonRunner.Run(args, new ServiceDaemonisable());
        }
    }
}
