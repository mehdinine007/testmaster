using System;
using System.Globalization;

namespace OrderManagement.ReportDesigner
{
    public class OrderDetailDto
    {
        internal static PersianCalendar _pc;

        internal string FixFormat(int datePart)
            => datePart.ToString().Length == 2
                ? $"{datePart}"
                : $"0{datePart}";

        internal string ToPersian(DateTime? date)
        {
            return date.HasValue
                ? $"{_pc.GetYear(date.Value)}/{FixFormat(_pc.GetMonth(date.Value))}/{FixFormat(_pc.GetDayOfMonth(date.Value))}"
                : string.Empty;
        }

        public OrderDetailDto()
        {
            _pc = new PersianCalendar();
        }

        public Guid UserId { get; set; }

        public long? PaymentPrice { get; set; }

        public string ProductTitle { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreationTimePersian => ToPersian(CreationTime);

        public int OrderId { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime? TransactionCommitDate { get; set; }

        public string TransactionCommitDatePersian => ToPersian(TransactionCommitDate);

        public string TransactionId { get; set; }

        public DateTime BirthDate { get; set; }

        public string BirthDatePersian => ToPersian(BirthDate);

        public string IssuingCityTitle { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string BirthCertId { get; set; }

        public string LiveDate => ToPersian(DateTime.Now);

        public string PspTitle { get; set; }

        public string NationalCode { get; set; }

        public string BirthCityTitle { get; set; }
    }
}
