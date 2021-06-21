using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay
{
    public class EeziePayWithdrawResponse
    {
        [JsonPropertyName("service_version")]
        public string ApiVersion { get; init; } = default!;

        [JsonPropertyName("sign")]
        public string Signature { get; init; } = default!;

        [JsonPropertyName("billno")]
        public string SystemRefId { get; init; } = default!;

        [JsonPropertyName("partner_orderid")]
        public string MerchantTransacitonReference { get; init; } = default!;

        [JsonPropertyName("currency")]
        public string TransactionCurrency { get; init; } = default!;

        [JsonPropertyName("amount")]
        public int PayountAmountRegistered { get; init; }

        [JsonPropertyName("fee")]
        public int PayoutFeeAppliedToThisTransaction { get; init; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; init; } = default!;

        /// <summary>
        /// 000: Success<br/>
        /// 111: Fail<br/>
        /// 001: Pending<br/>
        /// 112: Rejected
        /// </summary>
        [JsonPropertyName("status")]
        public string StatusOfTheTransaction { get; init; } = default!;

        /// <summary>
        /// [ErrorCode] Error Code Description<br/>
        /// [26] Version not supported<br/>
        /// [20003] Merchant Ref ID Was Repeated / Not Unique for reference no 756535665<br/>
        /// [20004] Missing value in mandatory parameters: partner_code<br/>
        /// [20004] Missing value in mandatory parameters: partner_orderid<br/>
        /// [20004] Missing value or invalid data in parameters: member_id<br/>
        /// [20004] Missing value or invalid data in parameters: currency<br/>
        /// [20004] Missing value or invalid data in parameters: trans_time<br/>
        /// [20004] Missing value or invalid data in parameters: bank_province<br/>
        /// [20004] Missing value or invalid data in parameters: bank_city<br/>
        /// [20004] Missing value or invalid data in parameters: bank_branch<br/>
        /// [20004] Missing value or invalid data in parameters: notify_url<br/>
        /// [bankaccountname] Invalid data for member bank info: bankaccountname()<br/>
        /// [bankaccountno] Invalid data for member bank info: bankaccountno()<br/>
        /// [accountid] Can not find appropriate account to perform fundout for partner TEST0001<br/>
        /// [partnerid] Unable to perform fundout for partner TEST0001 with the currency MYR<br/>
        /// [20012] Fund out amount limit error or invalid - Merchant Ref: test123, amount 88.00<br/>
        /// [20019] Invalid Signature
        /// </summary>
        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("error_description")]
        public string? ErrorDescription { get; set; }
    }
}