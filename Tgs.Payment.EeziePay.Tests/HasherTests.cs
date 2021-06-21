using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace Tgs.Payment.EeziePay.Tests
{
    [TestFixture]
    public class HasherTests
    {
        [Test]
        public void TestDepositSignExample()
        {
            var dict = new Dictionary<string, string>
            {
                ["service_version"] = "3.0",
                ["partner_code"] = "ABC123",
                ["partner_orderid"] = "20190606004030",
                ["member_id"] = "test",
                ["member_ip"] = "205.245.167.89",
                ["currency"] = "VND",
                ["amount"] = "100000",
                ["backend_url"] = "http://www.returnlocation.com/server.asp",
                ["redirect_url"] = "http://www.returnlocation.com/browser.asp",
                ["bank_code"] = "VCB.VN",
                ["trans_time"] = "2019-06-06 13:10:00"
            };

            var signatureString = string.Join("&", dict.Select(p => $"{p.Key}={p.Value}"));
            var merchantEncryptionKey = "AbCdEfGhIjK1234567";
            var fullText = $"{signatureString}&key={merchantEncryptionKey}";

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(fullText);
            var bytesHash = sha1.ComputeHash(bytes);
            var str = BitConverter.ToString(bytesHash);

            var actual = str.Replace("-", "").ToUpper();
            var except = "6775B9E757161D6626E202982D1158CB3C288AA2";

            Assert.AreEqual(except, actual);
        }

        [Test]
        public void TestEeziePayHasher()
        {
            var merchantEncryptionKey = "AbCdEfGhIjK1234567";
            var dict = new Dictionary<string, object>
            {
                ["service_version"] = "3.0",
                ["partner_code"] = "ABC123",
                ["partner_orderid"] = "20190606004030",
                ["member_id"] = "test",
                ["member_ip"] = "205.245.167.89",
                ["currency"] = "VND",
                ["amount"] = "100000",
                ["backend_url"] = "http://www.returnlocation.com/server.asp",
                ["redirect_url"] = "http://www.returnlocation.com/browser.asp",
                ["bank_code"] = "VCB.VN",
                ["trans_time"] = "2019-06-06 13:10:00"
            };

            var except = "6775B9E757161D6626E202982D1158CB3C288AA2";

            var hasher = new EeziePayHasher();
            var actual = hasher.ComputeHash(merchantEncryptionKey, dict);

            Assert.AreEqual(except, actual);
        }

        [Test]
        public void TestFinalTransactionResultNotificationSignExample()
        {
            var merchantEncryptionKey = "AbCdEfGhIjK1234567";
            var dict = new Dictionary<string, object>
            {
                ["service_version"] = "3.0",
                ["billno"] = "2019060648000016",
                ["partner_orderid"] = "abc123-123456",
                ["currency"] = "VND",
                ["request_amount"] = "5000000",
                ["receive_amount"] = "18000000",
                ["fee"] = "1800",
                ["status"] = "002"
            };

            var except = "D8515E3797C847806C8CBDF2503D7E594AB9073E";

            var hasher = new EeziePayHasher();
            var actual = hasher.ComputeHash(merchantEncryptionKey, dict);

            Assert.AreEqual(except, actual);
        }
    }
}