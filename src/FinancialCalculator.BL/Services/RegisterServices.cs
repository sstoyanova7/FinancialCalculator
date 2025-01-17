﻿namespace FinancialCalculator.BL.Services
{
    using FinancialCalculator.BL.Configuration.Database;
    using FinancialCalculator.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserDataService, UserDataService>();
            string connectionString = "Server=tcp:financial-calculator-db.database.windows.net,1433;Initial Catalog=financial-calculator;Persist Security Info=False;User ID=financial-calculator-admin;Password=StrongPassword123#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddTransient<IDatabaseConnectionFactory>(e => {
                return new SqlConnectionFactory(connectionString);
            });
            return services;
        }
    }
}
