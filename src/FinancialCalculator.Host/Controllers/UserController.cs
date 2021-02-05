using FinancialCalculator.BL.Services;
using FinancialCalculator.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialCalculator.Host.Controllers
{
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
        [Route("{id}")]
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
        [Route("{id}")]
        public void DeleteUserById(long id)
        {
            userDataService.deleteUserById(id);
        }
    }
}
