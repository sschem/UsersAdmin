
namespace Tatisoft.UsersAdmin.Api.Answers
{
    public class Answer<T> : Answer
    {
        public Answer(T content) :
            base(Answer.OK_CODE, "", false, false)
        {
            this.Content = content;
        }

        public T Content { get; set; }
    }
}
