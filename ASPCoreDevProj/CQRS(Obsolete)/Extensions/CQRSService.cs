using ASPCoreDevProj;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Extension
{
    public static class CQRSServiceExtensions
    {
        public static void AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface) && !t.IsAbstract);

            // BE AWARE!
            //  The aim of query handler is so the query handler depending on the generics parameters will provide 
            //  the appropriate class, do not use open generics e.g IQueryHandler<>, GetRandomClass<> as
            //  IQueryHandler will be only assigned to GetRandomClass and nothing else going against the point of QueryHandler
            //  will be more like GetRandomClassQueryHandler<>
            foreach (var handler in handlers)
            {
                var Interface = handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);
                services.AddScoped(Interface, handler);
            }
        }
    }
}
