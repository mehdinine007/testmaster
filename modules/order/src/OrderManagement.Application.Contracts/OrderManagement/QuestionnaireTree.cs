using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class QuestionnaireTree
    {
        public QuestionnaireDto Questionnaire { get; set; }

        public List<QuestionnaireAnswerDto> QuestionnaireAnswers { get; set; }

        public int? SubmitedAnswerId { get; set; }
    }
}
