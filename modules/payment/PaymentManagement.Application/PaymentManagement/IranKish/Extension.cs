using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Application.IranKish
{
    public static class Extension
    {
        public static byte[] HexStringToByteArray(this string hexString)
            => Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(value: hexString.Substring(startIndex: x, length: 2), fromBase: 16))
                .ToArray();

        public static string ByteArrayToHexString(this byte[] bytes)
            => bytes.Select(t => t.ToString(format: "X2")).Aggregate((a, b) => $"{a}{b}");
    }
}
