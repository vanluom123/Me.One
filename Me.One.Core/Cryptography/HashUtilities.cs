using System.Security.Cryptography;
using System.Text;

namespace Me.One.Core.Cryptography
{
    public static class HashUtilities
    {
        public static string Hash(string data, HashType hashType = HashType.MD5)
        {
            byte[] numArray = null;
            var bytes = Encoding.UTF8.GetBytes(data);
            var stringBuilder = new StringBuilder();
            switch (hashType)
            {
                case HashType.MD5:
                    using (var md5 = MD5.Create())
                    {
                        numArray = md5.ComputeHash(bytes);
                        break;
                    }
                case HashType.SHA1:
                    using (var shA1 = SHA1.Create())
                    {
                        numArray = shA1.ComputeHash(bytes);
                        break;
                    }
                case HashType.SHA256:
                    using (var shA256 = SHA256.Create())
                    {
                        numArray = shA256.ComputeHash(bytes);
                        break;
                    }
            }

            if (numArray == null)
                return stringBuilder.ToString();
            
            foreach (var num in numArray)
                stringBuilder.Append(num.ToString("x2"));
            
            return stringBuilder.ToString();
        }

        public static string HMAC(string data, string secretKey, HMACType type)
        {
            var bytes1 = Encoding.UTF8.GetBytes(secretKey);
            var bytes2 = Encoding.UTF8.GetBytes(data);
            var stringBuilder = new StringBuilder();
            byte[] numArray = null;
            switch (type)
            {
                case HMACType.MD5:
                    using (var hmacmD5 = new HMACMD5(bytes1))
                    {
                        numArray = hmacmD5.ComputeHash(bytes2);
                        break;
                    }
                case HMACType.SHA256:
                    using (var hmacshA256 = new HMACSHA256(bytes1))
                    {
                        numArray = hmacshA256.ComputeHash(bytes2);
                        break;
                    }
            }

            if (numArray == null)
                return stringBuilder.ToString();
            
            foreach (var num in numArray)
                stringBuilder.Append(num.ToString("x2"));
            
            return stringBuilder.ToString();
        }

        public static bool ValidateHashData(string inputData, string storedHashData, HashType type = HashType.MD5)
        {
            return string.CompareOrdinal(Hash(inputData, type), storedHashData) == 0;
        }
    }
}