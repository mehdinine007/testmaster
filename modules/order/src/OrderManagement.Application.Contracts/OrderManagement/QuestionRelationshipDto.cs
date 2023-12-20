﻿
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class QuestionRelationshipDto
    {
        public int Id { get; set; }
        public int QuestionRelationId { get; set; }
        public int OperationType { get; set; }
        public long QuestionAnswerId { get; set; }
        public int QuestionId { get; set; }
        public virtual QuestionAnswerDto QuestionAnswer { get; set; }

    }
}
