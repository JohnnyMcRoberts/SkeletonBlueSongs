﻿namespace SongsBurger.Controllers.RequestsResponses
{
    public enum UserResponseCode
    {
        Success = 0,
        DuplicateName,
        DuplicateEmail,
        UnknownUser,
        IncorrectPassword
    };

    public class UserAddResponse
    {
        public string Name { get; set; }

        public int ErrorCode { get; set; }

        public string FailReason { get; set; }

        public string UserId { get; set; }
    }
}
