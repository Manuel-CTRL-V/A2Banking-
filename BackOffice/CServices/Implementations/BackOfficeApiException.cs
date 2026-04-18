using System;

namespace BackOffice.Services.Implementations
{
    public class BackOfficeApiException : Exception
    {
        public int  ErrorCode        { get; }
        public bool IsConnectionError => ErrorCode == 0;
        public bool IsInvalidCredentials => ErrorCode == 50001;
        public bool IsCharacterNotFound  => ErrorCode == 50010;
        public bool IsAccountNotPending  => ErrorCode == 50021;

        public BackOfficeApiException(string message)
            : base(message) { ErrorCode = 0; }

        public BackOfficeApiException(int code, string message)
            : base(message) { ErrorCode = code; }

        public BackOfficeApiException(string message, Exception inner)
            : base(message, inner) { ErrorCode = 0; }
    }
}
