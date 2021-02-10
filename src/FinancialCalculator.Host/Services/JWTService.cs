namespace FinancialCalculator.Host.Services
{
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Serilog;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public class JWTService : IJWTService
    {
        private readonly ILogger _logger;

        private IConfiguration _config;

        private readonly IUserDataService userDataService;

        public JWTService(ILogger logger, IConfiguration config, IUserDataService service)
        {
            _logger = logger.ForContext<JWTService>();
            _config = config;
            userDataService = service;
        }

        public string GenerateJSONWebToken(UserLoginRequestModel userInfo)
        {
            UserModel user = userDataService.getFullUserByName(userInfo.Username).Result;
            bool isProvidedPasswordCorrect = userDataService.isUserPasswordCorrect(user.Password, userInfo.Password);

            if (!isProvidedPasswordCorrect)
            {
                // change exception to custom one
                throw new Exception("Wrong Password");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async void decodeJWT(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(jwt) as JwtSecurityToken;
            var email = jwtToken.Claims.First(claim => claim.Type == "Email").Value;
            var subject = jwtToken.Claims.First(claim => claim.Type == "sub").Value;

            bool isUserExisting = await userDataService.isUserExisting(subject, email);

            if (!isUserExisting)
            {
                throw new Exception();
            }
        }
    }
}