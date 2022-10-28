using HARS.Shared.DataBases;
using HARS.Shared.Infrastructure.Bootstrapper;
using HARS.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using utils = HARS.Shared.Utilities;

namespace HARS.API.Utilities
{
    public abstract class BaseController : ControllerBase
    {
        public BaseController(IApplication application)
        {
            Application = application;
        }
        protected IApplication Application { get; }
    }

    public static class ActionResultExtension
    {
        public static IActionResult ResponseResult(this utils.ActionResult response)
        {
            return response.Code switch
            {
                ErrorCode.BadRequest => new BadRequestObjectResult(response),
                ErrorCode.NotFound => new NotFoundObjectResult(response),
                ErrorCode.UnAuthorized => new UnauthorizedObjectResult(response),
                ErrorCode.InternalServerError => new InternalServerErrorObjectResult(response),
                ErrorCode.OK => new OkObjectResult(response),
                _ => new OkObjectResult(response),
            };
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
        public InternalServerErrorObjectResult() : base(null)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

    public static class ApplicationConfigurationExtension
    {
        private static string ExtractSQLConnectionString(this IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("SQLDB");
            return connString;
        }

        public static CoreModuleSettings ExtractCoreApplicationSettings(this IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            return new CoreModuleSettings()
            {
                SQLDatabaseType = DatabaseTypes.MsSql,
                LoggerFactory = loggerFactory,
                SQLConnectionString = configuration.ExtractSQLConnectionString(),
            };
        }
    }
}
