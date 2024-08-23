using System;

namespace Tgs.Payment.EeziePay
{
    public record EeziePayDepositSettings(Guid FormId, string MerchantEncryptionKey, string ApiVersion, string MerchantCode, Guid MerchantTransactionId, string MembersId, string TransactionCurrency, string RedirectUrl);
}