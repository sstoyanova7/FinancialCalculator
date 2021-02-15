namespace FinancialCalculator.Host.Controllers
{
    using FinancialCalculator.Host.Exceptions;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net.Http.Headers;
    using System.Text;
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
            try
            {
                if (login.Username.Length < 4 || login.Username.Length > 40)
                {
                    throw new BadRequestException("Incorrect Data! Username must between " + 4 + " and " + 40 + " symbols!");
                }
                if (login.Password.Length < 6 || login.Password.Length > 40)
                {
                    throw new BadRequestException("Incorrect Data! Password must between " + 6 + " and " + 40 + " symbols!");
                }
                string jwt = _jWTService.GenerateJSONWebToken(login);

                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMinutes(120);

                Response.Cookies.Append("Auth-Tst", jwt);

                IActionResult response = Ok();
                return response;
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("BadRequestException"))
                {
                    Response.StatusCode = 400;
                    return Content(e.Message);
                } else if (e.GetType().Name.Equals("NotFoundException"))
                {
                    Response.StatusCode = 404;
                    return Content(e.Message);
                } else
                {
                    Response.StatusCode = 500;
                    return Content(e.Message);
                }
            }
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
        public UserRequestModel InsertUser([FromBody] UserCreateRequestModel user)
        {
            try
            {
                _ = _userDataService.InsertUser(user).Result;
            }
            catch (Exception e)
            {
                UserRequestModel model = new UserRequestModel();
                if (e.Message.Contains("Incorrect Data!"))
                {
                    Response.StatusCode = 400;
                    model.Status = (System.Net.HttpStatusCode)400;
                    model.ErrorMessage = e.Message;
                    return model;
                }
                else if (e.Message.Contains("already exist"))
                {
                    Response.StatusCode = 409;
                    model.Status = (System.Net.HttpStatusCode)409;
                    model.ErrorMessage = e.Message;
                    return model;
                }
                else
                {
                    Response.StatusCode = 500;
                    model.Status = (System.Net.HttpStatusCode)500;
                    model.ErrorMessage = e.Message;
                    return model;
                }
            }
            Response.StatusCode = 200;
            return new UserRequestModel();
        }
    }
}
