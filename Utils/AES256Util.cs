using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace K2GGTT.Utils
{
    public class AES256Util
    {
        private static string iv = "jvHJ1EFA0IXBrxxz";

        public string encrypt(string plainText, string key)
        {
            {
                UTF8Encoding ue = new UTF8Encoding();

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Key = ue.GetBytes(key);
                    aesAlg.IV = ue.GetBytes(iv);

                    byte[] message = ue.GetBytes(plainText.Replace("\r\n", "").Replace(" ", ""));
                    byte[] enc;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor();

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(message, 0, message.Length);
                        }

                        enc = msEncrypt.ToArray();
                    }
                    return Convert.ToBase64String(enc, 0, enc.Length).Replace("/", "_").Replace("+", "-");
                }
            }
        }
    }
}
