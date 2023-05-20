using System.Globalization;

namespace PaymentManagement.Application.Utilities
{
    public static class DateUtil
    {
        private static readonly PersianCalendar pc = new PersianCalendar();
        public static string Now { get { return string.Format("{0}{1}{2}", pc.GetYear(DateTime.Now), pc.GetMonth(DateTime.Now).ToString().PadLeft(2, '0'), pc.GetDayOfMonth(DateTime.Now).ToString().PadLeft(2, '0')); } }
        public static bool IsValidDate(string date)
        {
            try
            {
                var dateParts = date.Split("/");
                pc.ToDateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), 0, 0, 0, 0);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
