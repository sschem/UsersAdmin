using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Services;
using Tatisoft.UsersAdmin.Test.Integration.Controller.Factory;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller
{
    public abstract class ControllerBaseTest
    {
        protected readonly WebAppFactoryFixture _fixture;
        public ControllerBaseTest(WebAppFactoryFixture fixture)
        {
            _fixture = fixture;
            Task.Run(() => _fixture.ClearCache(UserService.GET_ALL_CACHE_KEY)).Wait();
        }

        protected async Task<Answer<T>> GetOkAnswerChecked<T>(HttpResponseMessage response, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            var responseString = await this.ReadStringChecked(response, httpStatusCode);
            var answer = JsonConvert.DeserializeObject<Answer<T>>(responseString);
            this.ValidateAnswer(answer, Answer.OK_ANSWER);
            answer.Content.Should().NotBeNull();
            return answer;
        }

        protected async Task<Answer> GetOkAnswerChecked(HttpResponseMessage response, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return await this.GetAnswerChecked(response, Answer.OK_ANSWER, httpStatusCode);
        }

        protected async Task<Answer> GetWarnAnswerChecked(HttpResponseMessage response)
        {
            return await this.GetAnswerChecked(response, Answer.WARN_ANSWER);
        }

        protected async Task<Answer> GetErrorAnswerChecked(HttpResponseMessage response)
        {
            return await this.GetAnswerChecked(response, Answer.ERROR_ANSWER);
        }

        private async Task<Answer> GetAnswerChecked(HttpResponseMessage response, Answer expectedAnswer, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            var responseString = await this.ReadStringChecked(response, httpStatusCode);
            var answer = JsonConvert.DeserializeObject<Answer>(responseString);
            this.ValidateAnswer(answer, expectedAnswer);
            return answer;
        }

        private async Task<string> ReadStringChecked(HttpResponseMessage response, HttpStatusCode httpStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();            
            response.StatusCode.Should().Be(httpStatusCode);
            response.Content.Headers.ContentType.ToString().Should().Be(_fixture.CONTENT_TYPE);
            return responseString;
        }

        private void ValidateAnswer(Answer obtainedAnswer, Answer expectedAnswer)
        {
            obtainedAnswer.Code.Should().Be(expectedAnswer.Code);
            obtainedAnswer.IsWarning.Should().Be(expectedAnswer.IsWarning);
            obtainedAnswer.IsError.Should().Be(expectedAnswer.IsError);
        }
    }
}
