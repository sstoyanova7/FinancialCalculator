namespace FinancialCalculator.Host.Controllers
{
    using FinancialCalculator.Host.Services;
    using FinancialCalculator.Models.RequestModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;

    [ApiController]
    [Route("api/[controller]/")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;

        private readonly IJWTService jWTService;

        public AuthenticationController(IConfiguration config, IJWTService service)
        {
            this._config = config;
            jWTService = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sign-in")]
        public IActionResult Login([FromBody] UserLoginRequestModel login)
        {
            string jwt = jWTService.GenerateJSONWebToken(login);

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(120);

            Response.Cookies.Append("Auth-Tst", jwt);

            IActionResult response = Ok();
            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            string cookie = Request.Cookies["Auth-Tst"];
            if (cookie != null)
            {
                Response.Cookies.Delete("Auth-Tst");
            }
            IActionResult response = Ok();
            return response;
        }
    }
}
