﻿using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Questionnaire : FullAuditedEntity<int>
    {
        private ICollection<QuestionnaireAnswer> _questionnaireAnswers;

        private ICollection<SubmittedAnswers> _submittedAnswers;

        public string QuestiuonTitle { get; set; }

        public int AnswerComponentId { get; set; }

        public virtual AnswerComponentType AnswerComponentType { get; protected set; }

        public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers
        {
            get => _questionnaireAnswers ?? (_questionnaireAnswers = new List<QuestionnaireAnswer>());
            protected set => _questionnaireAnswers = value;
        }

        public virtual ICollection<SubmittedAnswers> SubmittedAnswers
        {
            get => _submittedAnswers ?? (_submittedAnswers= new List<SubmittedAnswers>());
            protected set => _submittedAnswers = value;
        }
    }
}
