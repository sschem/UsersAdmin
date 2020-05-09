using System;
using UsersAdmin.Core.Exceptions;

namespace UsersAdmin.Api.Dtos.Answers
{
    public class WarningAnswer : Answer
    {
        public WarningAnswer(int code, string message) :
            base(code, message, false, true)
        {
            this.Code = code;
            this.Message = message;
        }

        public WarningAnswer(string message) :
           this(WARN_CODE_DEFAULT, message)
        { }

        public WarningAnswer(WarningException wEx) :
        this(wEx.Code.HasValue ? wEx.Code.Value : WARN_CODE_DEFAULT, wEx.Message)
        { }
    }
}
