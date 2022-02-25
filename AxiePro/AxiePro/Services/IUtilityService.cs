using System.Security.Cryptography;
using System.Text;

namespace AxiePro.Services
{
    public interface IUtilityService
    {
        public string GetHashString(string inputString);
    }
    public class UtilityService : IUtilityService
    {

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
