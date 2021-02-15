namespace FinancialCalculator.Host.Controllers
{
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/[controller]/")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataService userDataService;

        public UserController(IUserDataService service)
        {
            userDataService = service;
        }

        [HttpGet]
        [Route("{Id}")]
        public UserRequestModel GetUserById(long id)
        {
            UserRequestModel userDto = userDataService.getUserById(id).Result;
            return userDto;
        }

        [HttpGet]
        [Route("")]
        public List<UserRequestModel> GetAllUsers()
        {
            return userDataService.getAllUsers().Result;
        }

        [HttpDelete]
        [Route("{Id}")]
        public void DeleteUserById(long id)
        {
            userDataService.deleteUserById(id);
        }
    }
}
