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

        [Test]
        public void TestWithdrawSignExample()
        {
            var merchantEncryptionKey = "AbCdEfGhIjK1234567";
           
            var dict = new Dictionary<string, object>
            {
                ["service_version"] = "3.0",
                ["partner_code"] = "1bvc123",
                ["partner_orderid"] = "1234567",
                ["member_id"] = "member1",
                ["currency"] = "VND",
                ["amount"] = "1000",
                ["account_name"] = "abd",
                ["account_number"] = "12345678",
                ["bank_province"] = "HCM",
                ["bank_city"] = "HCM",
                ["bank_branch"] = "HCM",//更前面範例沒這個參數 A345D436F81573FC4421B1F71CB90298CF446D4D
                ["bank_code"] = "VCB.VN",
                ["notify_url"] = "http://abc.com/ereturn.asp"
            };
            //066FAD5B5590BFAF5CAC9DED7687D97433553F49

            var except = "EDB2846C6C5001727A7E3D296EDE7AE28A83E31A";
            var hasher = new EeziePayHasher();
            var actualHasher = hasher.ComputeHash(merchantEncryptionKey, dict);
            var fullText = "service_version=3.0&partner_code=1bvc123&partner_orderid=1234567&member_id=member1&currency=VND&amount=1000&account_name=abd&account_number=12345678&bank_province=HCM&bank_city=HCM&bank_branch=HCM&bank_code=VCB.VN&notify_url=http://abc.com/ereturn.asp&key=AbCdEfGhIjK1234567";

            //3491C549285386AD13665A667DEA3443F57FD154
            
            //066FAD5B5590BFAF5CAC9DED7687D97433553F49
            //var fullText = "service_version=3.0&partner_code=1bvc123&partner_orderid=1234567&member_id=member1&currency=VND&amount=1000&account_name=abd&account_number=12345678&bank_province=HCM&bank_city=HCM&bank_code=VCB.VN&notify_url=http://abc.com/ereturn.asp&key=AbCdEfGhIjK1234567";
            //A345D436F81573FC4421B1F71CB90298CF446D4D
            //var fullText = "service_version=3.0&partner_code=1bvc123&partner_orderid=1234567&member_id=member1&currency=VND&amount=1000&account_name=abd&account_number=12345678&bank_province=HCM&bank_city=HCM&bank_branch=HCM&bank_code=VCB.VN&notify_url=http://abc.com/ereturn.asp&key=AbCdEfGhIjK1234567";
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(fullText);
            var bytesHash = sha1.ComputeHash(bytes);
            var str = BitConverter.ToString(bytesHash);
            var actual = str.Replace("-", "").ToUpper();

            Assert.AreEqual(except, actual);

        }
   
    }
}