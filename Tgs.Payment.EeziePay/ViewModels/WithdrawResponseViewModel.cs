using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay.ViewModels
{
    public class WithdrawResponseViewModel
    {
        [JsonPropertyName("service_version")]
        public string ApiVersion { get; init; } = default!;

        [JsonPropertyName("sign")]
        public string Signature { get; init; } = default!;

        [JsonPropertyName("billno")]
        public string SystemRefId { get; init; } = default!;

        [JsonPropertyName("partner_orderid")]
        public string MerchantTransactionReference { get; init; } = default!;

        [JsonPropertyName("currency")]
        public string TransactionCurrency { get; init; } = default!;

        [JsonPropertyName("amount")]
        public int PayoutAmount { get; init; }

        [JsonPropertyName("account_name")]
        public string MemberName { get; init; } = default!;

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; init; } = default!;

        [JsonPropertyName("bank_code")]
        public string BankCode { get; init; } = default!;

        [JsonPropertyName("status")]
        public string StatusOfTheTransaction { get; init; } = default!;

        [JsonPropertyName("fee")]
        public int AssignFee { get; init; }
        /// <summary>
        /// The fee charges by bank
        /// itself for processing this
        /// transaction.
        /// </summary>
        [JsonPropertyName("bank_charge")]
        public int BankCharges { get; init; }

        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; init; }

        [JsonPropertyName("error_description")]
        public string? ErrorDescription { get; init; }
    }
}