using CramerGui.Hubs;
using CramerGui.Repositories;
using CramerGui.Services;
using CramerGui.Services.Interfaces;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;

namespace CramerGui
{
    public class HtmlOutputFormatter : StringOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add("text/html");
        }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetSection("DB").GetSection("connString").Value;
            var upgrader = DeployChanges.To
                                        .SQLiteDatabase(connectionString)
                                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                        .LogToConsole()
                                        .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully ran migrations");
            Console.ResetColor();

            services.Configure<InfluxSettings>(Configuration.GetSection("Influx"));
            services.Configure<GrafanaSettings>(Configuration.GetSection("Grafana"));
            services.Configure<Mqtt>(Configuration.GetSection("Mqtt"));

            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new HtmlOutputFormatter());
            });

            services.AddSignalR();
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(Configuration.GetSection("DB").GetSection("connString").Value));

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<ILightService, LightService>();
            services.AddSingleton<IRoomService, RoomService>();
            services.AddSingleton<ISceneService, SceneService>();
            services.AddSingleton<IInfluxService, InfluxService>();
            services.AddSingleton<IMqttService, MqttService>();

            services.AddScoped<ILightRepository, LightRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ISceneRepository, SceneRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<TheHub>("/TheHub");
            });

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            Console.WriteLine("Starting starup, thread: " + Thread.CurrentThread.ManagedThreadId);


            IMqttService mqttService = app.ApplicationServices.GetRequiredService<IMqttService>();
            mqttService.StartMqtt();

            ILightService lightService = app.ApplicationServices.GetRequiredService<ILightService>();
            lightService.Init();

            ISceneService sceneService = app.ApplicationServices.GetRequiredService<ISceneService>();
            sceneService.Init();
        }
    }
}
