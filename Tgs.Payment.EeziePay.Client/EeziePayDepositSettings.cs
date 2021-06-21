namespace Tgs.Payment.EeziePay
{
    public record EeziePayDepositSettings(string MerchantEncryptionKey, string ApiVersion, string MerchantCode, string MerchantTransactionId, string MembersId, string TransactionCurrency);
}