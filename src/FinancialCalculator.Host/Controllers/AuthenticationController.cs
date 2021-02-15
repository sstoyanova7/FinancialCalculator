namespace FinancialCalculator.Host.Controllers
{
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]/")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IJWTService _jWTService;
        private readonly IUserDataService _userDataService;

        public AuthenticationController(IConfiguration config, IJWTService service, IUserDataService userDataService)
        {
            this._config = config;
            _jWTService = service;
            _userDataService = userDataService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sign-in")]
        public IActionResult Login([FromBody] UserLoginRequestModel login)
        {
            string jwt = _jWTService.GenerateJSONWebToken(login);

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

        [AllowAnonymous]
        [HttpPost]
        [Route("sign-up")]
        public async Task InsertUserAsync([FromBody] UserCreateRequestModel user)
        {
            await Task.Run(() => _userDataService.InsertUser(user));
        }
    }
}
