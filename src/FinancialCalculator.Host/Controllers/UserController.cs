namespace FinancialCalculator.Host.Controllers
{
    using FinancialCalculator.Host.Services;
    using FinancialCalculator.Models.RequestModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/[controller]s/")]
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

        [HttpPost]
        [Route("")]
        public void InsertUser([FromBody] UserCreateRequestModel user)
        {
            userDataService.insertUser(user);
        }

        [HttpDelete]
        [Route("{Id}")]
        public void DeleteUserById(long id)
        {
            userDataService.deleteUserById(id);
        }
    }
}
