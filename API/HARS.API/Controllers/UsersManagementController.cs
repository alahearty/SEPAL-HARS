using Admin.Application.Commands.CreateUser;
using Admin.Application.Queries.FetchAllUsers;
using Admin.Infrastructure;
using HARS.API.Utilities;
using HARS.Shared.Infrastructure.Bootstrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HARS.API.Controllers
{
    [ApiController]
    [Route("users")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UsersManagementController : BaseController
    {
        public UsersManagementController(IApplication application) : base(application) { }

        [HttpGet()]
        public async Task<IActionResult> FetchAllUserAsync()
        {
            var response = await Application
                .SendQueryAsync<AdminModule, FetchAllUsersQuery, IEnumerable<UserProfileDto>>(new FetchAllUsersQuery());

            return response.ResponseResult();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserCommand createUserCommand)
        {
            var response = await Application.ExecuteCommandAsync<AdminModule, CreateUserCommand, Guid>(createUserCommand);

            return response.ResponseResult();

        }

    }
}