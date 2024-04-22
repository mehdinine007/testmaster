using System;
using System.Collections.Generic;
using System.Globalization;

namespace OrderManagement.ReportDesigner
{
    public class CustomerOrder_OrderDetailDto
    {
        internal static PersianCalendar _pc;

        internal string FixFormat(int datePart)
            => datePart.ToString().Length == 2
                ? $"{datePart}"
                : $"0{datePart}";

        public CustomerOrder_OrderDetailDto()
        {
            _pc = new PersianCalendar();
        }

        public long? PaymentPrice { get; set; }

        public string ProductTitle { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreationTimePersian => $"{_pc.GetYear(CreationTime)}/{FixFormat(_pc.GetMonth(CreationTime))}/{FixFormat(_pc.GetDayOfMonth(CreationTime))}";

        public int OrderId { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime? TransactionCommitDate { get; set; }

        public string TransactionCommitDatePersian => TransactionCommitDate.HasValue
            ? $"{_pc.GetYear(TransactionCommitDate.Value)}/{FixFormat(_pc.GetMonth(TransactionCommitDate.Value))}/{FixFormat(_pc.GetDayOfMonth(TransactionCommitDate.Value))}"
            : null;

        public string TransactionId { get; set; }
    }
}