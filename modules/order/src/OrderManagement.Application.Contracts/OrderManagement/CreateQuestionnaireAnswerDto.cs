using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class CreateQuestionnaireAnswerDto
    {
        public List<string> AnswerDescriptions { get; set; }

        public int QuestionnaireId { get; set; }
    }

}
