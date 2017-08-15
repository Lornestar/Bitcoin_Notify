using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Bitcoin_Background.Startup))]

namespace Bitcoin_Background
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // configure HangFire to use SqlServer for persistent storage of jobs.
            // Set polling to every minute
            GlobalConfiguration.Configuration.UseSqlServerStorage(
                "DefaultConnection",
                new SqlServerStorageOptions
                {
                    // set to true to have HangFire create your schema
                    PrepareSchemaIfNecessary = true,
                    // set to false and run HangFire.sql to create your sql.
                    //PrepareSchemaIfNecessary = false, 
                    QueuePollInterval = TimeSpan.FromSeconds(1)
                });


            // enable HangFire dashboard to display information about job and server status
            app.UseHangfireDashboard("/Admin/Jobs");

            // ensures processing of HangFire jobs in ASP.NET web app using OWIN + IIS
            app.UseHangfireServer();



        }
    }
}