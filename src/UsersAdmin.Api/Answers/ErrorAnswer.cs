using System;

namespace Tatisoft.UsersAdmin.Api.Answers
{
    public class ErrorAnswer : Answer
    {
        public ErrorAnswer(int code, string message, string additionalInfo = "") :
            base(code, message, true, false)
        {
            this.Code = code;
            this.Message = message;
            this.AdditionalInfo = additionalInfo;
        }

        public ErrorAnswer() :
          this(ERROR_CODE_DEFAULT, ERROR_MSG_DEFAULT, "")
        { }

        public ErrorAnswer(string message, string additionalInfo = "") :
           this(ERROR_CODE_DEFAULT, message, additionalInfo)
        { }

        public ErrorAnswer(Exception ex, Exception inner = null) :
        this(ERROR_CODE_DEFAULT,
            ex.Message,
            inner != null ? inner.Message : ""
            )
        { }

        public string AdditionalInfo { get; set; }
    }
}
