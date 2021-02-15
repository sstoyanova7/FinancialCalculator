namespace FinancialCalculatorFE
{
    using System.Text;
    using FinancialCalculator.BL.Services;
    using FinancialCalculator.BL.Validation;
    using FinancialCalculator.Host.Config;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using FinancialCalculator.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Serilog;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // Return the issuer as in test project and in appsettings
                    ValidIssuer = "localhost:5000",
                    ValidAudience = "localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"))
                };
            });

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
            services.AddTransient<IJWTService, JWTService>();
            services.AddTransient<IRequestHistoryDataService, RequestHistoryDataService>();

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

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //app.UseExceptionHandler();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{Id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
