using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPISignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPISignalR
{
    public static class SerivceRegistration
    {
        public static void AddSignalServices(this IServiceCollection collection)
        {
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddSignalR();
        }
    }
}
