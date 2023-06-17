using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Aitoe.Vigilant.Controller.BL.Services
{
    public class EncryptionService
    {
        private const string RijndaelKeyString = "D4-D5-26-E6-1B-BA-C6-B5-C2-AE-E7-B5-3E-D0-83-C5-D2-1B-DD-49-D9-B9-12-2D-54-5A-B3-2B-CA-7B-9A-59";
        private const string RijndaelIVString = "6B-0E-20-4E-D7-7E-13-77-06-BC-3E-17-1E-07-32-5F";

        public string EncryptText(string text)
        {
            byte[] keyBytes = RijndaelKeyString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            byte[] IVBytes = RijndaelIVString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

            // Check arguments.
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (keyBytes == null || keyBytes.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IVBytes == null || IVBytes.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = keyBytes;
                rijAlg.IV = IVBytes;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            var encryptedText = BitConverter.ToString(encrypted).Replace("-", "");

            //return encrypted;
            return encryptedText;
        }

        private byte[] GetBytesFromString(string str, int chunkSize)
        {
            var stringWithHiphen = "";
            //return Enumerable.Range(0, str.Length / chunkSize)
            //    .Select(i => str.Substring(i * chunkSize, chunkSize));
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                var temp = str.Substring(i, chunkSize);
                stringWithHiphen = stringWithHiphen + temp + "-";
            }

            stringWithHiphen = stringWithHiphen.TrimEnd('-');
            byte[] bytes = stringWithHiphen.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            return bytes;
        }

        public string DecryptText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            byte[] cipherText = GetBytesFromString(text, 2);
            return DecryptTextFromBytes(cipherText);
        }

        private string DecryptTextFromBytes(byte[] cipherText)
        {
            byte[] keyBytes = RijndaelKeyString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            byte[] IVBytes = RijndaelIVString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (keyBytes == null || keyBytes.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IVBytes == null || IVBytes.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = keyBytes;
                rijAlg.IV = IVBytes;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }
    }
}
