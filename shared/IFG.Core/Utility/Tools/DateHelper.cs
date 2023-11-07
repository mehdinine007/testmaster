using System.Globalization;

namespace IFG.Core.Utility.Tools
{
    public static class DateHelper
    {
        private static PersianCalendar _pc;

        static DateHelper()
        {
            _pc = new PersianCalendar();
        }

        public static DateTime ToDateTime(this string persinaDate)
            => new DateTime(int.Parse(persinaDate.Substring(0, 4)), int.Parse(persinaDate.Substring(5, 2)), int.Parse(persinaDate.Substring(8, 2)), _pc);
    }
}
