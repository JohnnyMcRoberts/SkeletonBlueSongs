namespace SongsBurger.Controllers.RequestsResponses
{
    public enum UserAddResponseCode
    {
        Success = 0,
        DuplicateName,
        DuplicateEmail
    };

    public class UserAddResponse
    {
        public string Name { get; set; }

        public int ErrorCode { get; set; }

        public string FailReason { get; set; }

        public string UserId { get; set; }
    }
}
