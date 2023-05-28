using NPOI.POIFS.Crypt;
using System.Text;

namespace PaymentManagement.Application.Mellat
{
    public static class Encrypt
    {
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string EncryptPinBlock(byte[] key, byte[] pinBlock) //throws Exception
        {
            Cipher cipher = Cipher.GetInstance("DES/ECB/PKCS5Padding");
            SecretKeySpec keySpec = new SecretKeySpec(key, "DES");
            cipher.Init(Cipher.ENCRYPT_MODE, keySpec);
            byte[] enc = cipher.DoFinal(pinBlock);
            return ByteArrayToString(enc);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
