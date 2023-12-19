using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants
{
    public class ValidationConstant
    {
        public const string ItemNotFound = "مورد یافت نشد";
        public const string ItemNotFoundId = "0005";
        public const string QuestionnerNotFound = "پرسشنامه خالی است";
        public const string QuestionnerNotFoundId = "0006";
        public const string QuestionNotFound = "پرسش وجود ندارد";
        public const string QuestionNotFoundId = "0009";  
        public const string AnswerNotFound = "پاسخ وجود ندارد";
        public const string AnswerNotFoundId = "0010";
        public const string OperatorNotFound = "عملگر وجود ندارد";
        public const string OperatorNotFoundId = "0011";
    }
    public static class RuleSets
    {
        public const string Add = "Add";
        public const string AddId = "0001";
        public const string Edit = "Edit";
        public const string EditId = "0002";
        public const string Delete = "Delete";
        public const string DeleteId = "0003";
        public const string GetById = "GetById";
        public const string GetByIdId = "0004";
        public const string GetListByQuestionId = "GetListByQuestionId";
        public const string GetListByQuestionIdId = "0006";
        

    }
}
