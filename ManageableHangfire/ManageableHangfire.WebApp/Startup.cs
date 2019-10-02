using Hangfire;
using ManageableHangfire.Core.Executors;
using ManageableHangfire.Core.Queue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ManageableHangfire.WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => HangfireConfig.GetServiceConfiguration(configuration));
            services.AddHangfireServer();
            HangfireConfig.RegisterManagerJobServer();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();
            //app.Run(async (context) =>
            //{
            //    await CreateHangfireTestMessages(context);
            //});
        }

        private static async Task CreateHangfireTestMessages(HttpContext context)
        {
            const int QUANTITY_PER_QUEUE = 10000;

            // Upload
            for (int i = 0; i < QUANTITY_PER_QUEUE; i++)
            {
                HangfireQueueManager.CreateJob(() => new Executor().Upload(i), QueueConstants.UPLOAD);
                await context.Response.WriteAsync("Created upload " + i);
            }

            // Download
            for (int i = 0; i < QUANTITY_PER_QUEUE; i++)
            {
                HangfireQueueManager.CreateJob(() => new Executor().Download(i, JobCancellationToken.Null), QueueConstants.DOWNLOAD);
                await context.Response.WriteAsync("Created download " + i);
            }
        }
    }
}
