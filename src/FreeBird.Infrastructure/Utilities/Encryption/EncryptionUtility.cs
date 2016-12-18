using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.Utilities.Encryption
{
    public static class EncryptionUtility
    {
        public static string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public static string EncryptSHA1(string input, string salt)
        {
            // 将密码和salt值转换成字节形式并连接起来
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            // 选择算法，对连接后的值进行散列
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);

            // 以字符串形式返回散列值
            return Convert.ToBase64String(inArray);
        }

    }
}
