using BadgeBoard.Api.Extensions.Response;

namespace BadgeBoard.Api.Modules.BadgeGlobal.Exceptions
{
    public class ResponseException : Exception
    {
        public ApiResponse Response { get; private set; }
        public ResponseException(ApiResponse response)
        {
            Response = response;
        }
    }
}
