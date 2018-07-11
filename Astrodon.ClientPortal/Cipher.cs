#region Usings
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace Astrodon.ClientPortal
{
    public class Cipher
    {
        private static readonly string PasswordHash = "@22qf5n!a";
        private static readonly string SaltKey = "3yla5jge2";
        private static readonly string VIKey = "^d5%4uwc#8NqU5j(";
        private static readonly string NumbericEncryptionKey = "TREYS^%&hH3#j^D";

        public static string Encrypt(string plainText)
        {
            // var aes = System.Security.Cryptography.Aes.Create();

            if (string.IsNullOrWhiteSpace(plainText))
                return plainText;
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

                var symmetricKey = System.Security.Cryptography.Aes.Create();
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.Zeros;

                var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

                byte[] cipherTextBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                    }
                }
                return ByteArrayToString(cipherTextBytes); // Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return encryptedText;
            try
            {
                string result = null;
                if (encryptedText != null)
                    encryptedText = encryptedText.Trim();
                var cipherTextBytes = StringToByteArray(encryptedText); // Convert.FromBase64String(encryptedText);
                var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

                var symmetricKey = System.Security.Cryptography.Aes.Create();
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.Zeros;

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainTextBytes = new byte[cipherTextBytes.Length];

                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                        var tmp = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                        result = tmp;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", "").Trim();
        }

        public static byte[] StringToByteArray(string hex)
        {
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars / 2];
            for (var i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string EncodeLong(long? p)
        {
            if (p == null)
                p = 0;
            var plainText = p.Value + "_" + NumbericEncryptionKey;
            return Encrypt(plainText);
        }

        public static long DecryptLong(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            var v = Decrypt(value);

            var spl = v.Split("_".ToCharArray());
            return Convert.ToInt64(spl[0]);
        }

        public static long StringToLong(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            var v = (value);

            var spl = v.Split("_".ToCharArray());
            return Convert.ToInt64(spl[0]);
        }
    }
}
