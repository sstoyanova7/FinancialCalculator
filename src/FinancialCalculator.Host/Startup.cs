namespace FinancialCalculatorFE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FinancialCalculator.BL.Configuration.Database;
    using FinancialCalculator.BL.Validation;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using FinancialCalculator.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddControllersWithViews();
            services.AddSwaggerGen();
            services.AddSingleton<ILogger>(new LoggerConfiguration().WriteTo.Console().MinimumLevel.Debug().CreateLogger());
            services.AddSingleton<ICalculatorService<NewLoanResponseModel, NewLoanRequestModel>, NewLoanCalculatorService>();
            services.AddSingleton<ICalculatorService<RefinancingLoanResponseModel, RefinancingLoanRequestModel>, RefinancingLoanCalculatorService>();
            services.AddSingleton<ICalculatorService<LeasingLoanResponseModel, LeasingLoanRequestModel>, LeasingLoanCalculatorService>();
            services.AddSingleton<IValidator<FeeModel>, FeeValidator>();
            services.AddSingleton<IValidator<NewLoanRequestModel>, NewLoanRequestValidator>();
            services.AddSingleton<IValidator<RefinancingLoanRequestModel>, RefinancingLoanRequestValidator>();
            services.AddSingleton<IValidator<LeasingLoanRequestModel>, LeasingLoanRequestValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseDeveloperExceptionPage()
                    .UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                    {
                        ConfigFile = "webpack.config.js",
                        HotModuleReplacement = true
                    });
                app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
