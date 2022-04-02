using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using System.Security.Cryptography;
using System.Text;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public class SecurityManager : ISecurityManager
    {
        public string Encrypt(string key, string plainText)
        {
            if (plainText == null)
            {
                return null;
            }
            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(key);

            // Hash the password with SHA256
            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public string Decrypt(string key, string encryptedText)
        {
            if (encryptedText == null)
            {
                return null;
            }
            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(key);

            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }


        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            return SecurityProcess(bytesToBeEncrypted, passwordBytes, true);
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            return SecurityProcess(bytesToBeDecrypted, passwordBytes, false);
        }

        // @SONAR_STOP@
        internal static byte[] SecurityProcess(byte[] bytes, byte[] passwordBytes, bool encrypt)
        {
            byte[] result = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    if (encrypt)
                    {
                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);
                            cs.Close();
                        }
                    }
                    else
                    {
                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);
                            cs.Close();
                        }
                    }

                    result = ms.ToArray();
                }
            }

            return result;
        }

    }
}
