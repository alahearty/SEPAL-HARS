using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Utilities
{
    public enum ErrorCode
    {
        BadRequest = 400,
        UnAuthorized = 401,
        NotFound = 404,
        InternalServerError = 500,
        OK = 200
    }

    public class ErrorMessages
    {
        public const string INTERNAL_SERVER_ERROR = "Sorry, we encountered an error completing your request";
        public const string BAD_REQUEST = "Bad request";
        public const string UNAUTHORIZED = "Unauthorized";
        public const string NOT_FOUND = "Not Found";
    }

    public class ActionResult
    {
        internal ActionResult() { }
        public bool WasSuccessful { get; set; }

        public bool NotSuccessful => WasSuccessful == false;

        public ErrorCode Code { get; set; }

        public List<string> Errors { get; protected set; } = new List<string>();

        public ActionResult AddError(string error)
        {
            Errors.Add(error);
            return this;
        }

        public ActionResult SetErrors(IEnumerable<string> errors)
        {
            Errors = errors?.ToList();
            return this;
        }

        public static ActionResult Failed(ErrorCode errorCode = ErrorCode.InternalServerError)
        {
            return new ActionResult() { WasSuccessful = false, Code = errorCode };
        }

        public static ActionResult Success()
        {
            return new ActionResult() { WasSuccessful = true, Code = ErrorCode.OK };
        }
    }

    public class ActionResult<T> : ActionResult
    {
        internal ActionResult()
        {

        }

        public static new ActionResult<T> Failed(ErrorCode code = ErrorCode.InternalServerError)
        {
            return new ActionResult<T>() { WasSuccessful = false, Code = code };
        }

        public static ActionResult<T> Failed(T data, ErrorCode code = ErrorCode.InternalServerError)
        {
            return new ActionResult<T>() { WasSuccessful = false, Data = data, Code = code };
        }

        public static ActionResult<T> Success(T output)
        {
            return new ActionResult<T>() { WasSuccessful = true, Data = output, Code = ErrorCode.OK };
        }

        public new ActionResult<T> SetErrors(IEnumerable<string> errors)
        {
            if (errors == null) return this;
            Errors.AddRange(errors);
            return this;
        }

        public new ActionResult<T> AddError(string error)
        {
            Errors.Add(error);
            return this;
        }

        public T Data { get; private set; }
    }
}
