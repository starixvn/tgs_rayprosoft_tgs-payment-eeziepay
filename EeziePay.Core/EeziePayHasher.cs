using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tgs.Payment.EeziePay
{
    public class EeziePayHasher
    {
        private readonly static SHA1 _SHA1;

        static EeziePayHasher()
        {
            _SHA1 = SHA1.Create();
        }

        public string ComputeHash(string merchantEncryptionKey, IDictionary<string, object> dict)
        {
            var content = string.Join(
                "&",
                dict.Select(p =>
                {
                    if (p.Key == "remarks")
                        return "";
                    else
                        return $"{p.Key}={p.Value}";
                }));

            var fullText = $"{content}&key={merchantEncryptionKey}";

            var bytes = Encoding.UTF8.GetBytes(fullText);
            var bytesHash = _SHA1.ComputeHash(bytes);
            var str = BitConverter.ToString(bytesHash);

            var result = str.Replace("-", "").ToUpper();

            return result;
        }
    }
}
