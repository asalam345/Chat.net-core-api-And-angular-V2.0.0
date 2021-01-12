using DAL;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat_server.Services
{
    public static class ExtractServices
    {
        public static void ExtractChatServices(IServiceCollection services)
        {
            services.AddScoped<IGenericService<UserVM>, DA_User>();
            services.AddScoped<IGenericService<MessageVM>, DA_Chat>();
            services.AddScoped<IGenericService<tblLogedinStatus>, DA_LogInStatus>();
        }
    }
}
