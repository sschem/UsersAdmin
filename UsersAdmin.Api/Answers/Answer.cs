using System;

namespace UsersAdmin.Api.Answers
{
    public class Answer
    {
        public static int OK_CODE = 0;
        public static string OK_MSG_DEFAULT = "Proceso realizado";

        public static int ERROR_CODE_DEFAULT = 96;
        public static string ERROR_MSG_DEFAULT = "Error al realizar el proceso!";

        public static int WARN_CODE_DEFAULT = 100;

        public static Answer OK_ANSWER = new Answer(OK_CODE, OK_MSG_DEFAULT, false, false);
        public static Answer WARN_ANSWER = new Answer(WARN_CODE_DEFAULT, "", false, true);
        public static Answer ERROR_ANSWER = new Answer(ERROR_CODE_DEFAULT, ERROR_MSG_DEFAULT, true, false);

        public Answer(int code, string message, bool isError, bool isWarning, string guid)
        {
            this.Code = code;
            this.Message = message;
            this.Guid = guid;
            this.Date = DateTime.Now;
            this.IsError = isError;
            this.IsWarning = isWarning;
        }

        public Answer(int code, string message, bool isError, bool isWarning) :
            this(code, message, isError, isWarning, System.Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper())
        { }

        public Answer() :
        this(-1, string.Empty, false, false, string.Empty)
        { }

        public int Code { get; set; }
        public string Message { get; set; }
        public string Guid { get; set; }
        public DateTime Date { get; set; }
        public bool IsError { get; set; }
        public bool IsWarning { get; set; }

        public override string ToString() => $"[Answer: Code:{Code} - Message:{Message} - Guid:{Guid} - IsError:{IsError} - IsWarning:{IsWarning}]";
    }
}
