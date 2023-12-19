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

    }
}
