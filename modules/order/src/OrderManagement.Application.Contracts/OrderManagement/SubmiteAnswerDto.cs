﻿namespace OrderManagement.Application.Contracts
{
    public class SubmitteAnswerDto
    {
        public int AnswerId { get; set; }

        public string AnswerDescription { get; set; }

        public long UserId { get; set; }

        public int Id { get; set; }

        public int? QuestionnaireId { get; set; }
    }
}
