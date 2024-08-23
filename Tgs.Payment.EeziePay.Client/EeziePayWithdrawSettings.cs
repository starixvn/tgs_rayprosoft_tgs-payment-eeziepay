using System;

namespace Tgs.Payment.EeziePay
{
    public record EeziePayWithdrawSettings(string MerchantEncryptionKey, string ApiVersion, string MerchantCode, Guid MerchantTransactionId, string MembersId, string TransactionCurrency, string MemeberAccountName, string MemberAccountNumber, string MemberBankProvince, string MemberBankCity, string MemberBankBranch, string MemberDepositBankCode)
    {
        public Guid FormId { get; set; }
    }
}