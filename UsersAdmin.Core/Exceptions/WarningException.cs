using System;

namespace UsersAdmin.Core.Exceptions
{
    public class WarningException : Exception
    {
        public int? Code { get; private set; }

        public WarningException(string message) : base(message) { }

        public WarningException(int code, string message) : this(message)
        {
            this.Code = code;
        }

        public WarningException(Exception exception) : base(exception.Message, exception.InnerException) { }

        public WarningException(int code, Exception exception) : this(exception)
        {
            this.Code = code;
        }
    }
}