using System.Threading.Tasks;
using System.Web.Http.Filters;
using log4net;

namespace Bluewire.Webhooks.Service
{
    public class ExceptionLogFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // TaskCanceledException usually means that the request was cancelled by the client before it completed.
            if (context.Exception.GetBaseException() is TaskCanceledException) return;

            LogManager.GetLogger(GetType()).Error(context.Exception);
        }
    }
}
