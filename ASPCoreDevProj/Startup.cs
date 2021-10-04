using MediatR;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ASPCoreDevProj.Data;
using Microsoft.AspNetCore.Identity;
using CQS.Data.BookQuery;
using System;
using CQS.Model;
using CQRS.Extension;
using ASPCoreDevProj.Profiles;
using DapperProject.Context;
using DapperProject.Interfaces;
using DapperProject.Connections;
using ASPCoreDevProj.Data.Pipeline;

namespace ASPCoreDevProj
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));

            //  Overwrite DbContext ApplicationDbContext to be used with IApplicationDbContext
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            //  Dapper Read, Write, Stored Procedures
            services.AddScoped<IAppWriteDbConnection, DapperSharedContextWriteConnection>();                                               
            services.AddScoped<IAppReadTransaction, AppReadTransactionDbConnection>();

            services.AddRazorPages();

            services.AddDatabaseDeveloperPageExceptionFilter();

            /*  Custom Build
            //services.AddCommandQueryHandlers(typeof(ICommandHandler<>));
            //services.AddCommandQueryHandlers(typeof(IQueryHandler<,>));
            //services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();*/

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(c => c.AddProfile<BookProfile>(), typeof(Startup));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggerPipelineBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehaviour<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

    }
}
