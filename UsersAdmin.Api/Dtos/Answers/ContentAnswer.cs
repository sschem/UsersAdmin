using System;

namespace UsersAdmin.Api.Dtos.Answers
{
    public class Answer<T> : Answer
    {
        public Answer(T content) :
            base(Answer.OK_CODE, "", false, false)
        {
            this.Content = content;
        }

        public T Content { get; set; }

        // public static object MakeOkAnswerFromGeneric(object innerValue)
        // {
        //     Type answerType = typeof(Answer<>);
        //     Type[] innerTypeArg = { innerValue.GetType() };
        //     Type constructed = answerType.MakeGenericType(innerTypeArg);
        //     var answerResult = Activator.CreateInstance(constructed, OK_CODE, string.Empty, false, false, innerValue);
        //     return answerResult;
        // }
    }
}
